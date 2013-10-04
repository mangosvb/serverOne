'
' Copyright (C) 2013 getMaNGOS <http://www.getMangos.co.uk>
'
' This program is free software; you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation; either version 2 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with this program; if not, write to the Free Software
' Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
'
Imports System.Threading
Imports System.Runtime.CompilerServices
Imports mangosVB.Common.BaseWriter

Public Module WS_GameObjects

    #Region "WS.GameObjects.TypeDef"

    'WARNING: Use only with GAMEOBJECTSDatabase()
    Public Class GameObjectInfo
    Implements IDisposable

        Public ID As Integer = 0
        Public Model As Integer = 0
        Public Type As Byte = 0
        Public Name As String = ""
        Public RespawnTime As Integer = 0
        Public Fields(23) As Integer
        Private found_ As Boolean = False

        Public Sub New(ByVal ID_ As Integer)
            Dim MySQLQuery As New DataTable
            WorldDatabase.Query(String.Format("SELECT * FROM gameObjects WHERE gameObject_id = {0};", ID_), MySQLQuery)

            If MySQLQuery.Rows.Count = 0 Then
                Log.WriteLine(LogType.FAILED, "GameObject {0} not found in SQL database!", ID_)
                found_ = False
                Exit Sub
            End If
            found_ = True

            ID = ID_

            Model = MySQLQuery.Rows(0).Item("gameObject_Model")
            Type = MySQLQuery.Rows(0).Item("gameObject_Type")
            Name = MySQLQuery.Rows(0).Item("gameObject_Name")
            RespawnTime = MySQLQuery.Rows(0).Item("gameObject_RespawnTime")

            For i As Byte = 0 To 23
                Fields(i) = MySQLQuery.Rows(0).Item("gameObject_Field" & i)
            Next

            GAMEOBJECTSDatabase.Add(ID, Me)
        End Sub

        Public Sub Dispose() Implements System.IDisposable.Dispose
            GAMEOBJECTSDatabase.Remove(ID)
        End Sub
        Public Sub Save()
            If found_ = False Then
                WorldDatabase.Update("INSERT INTO gameObjects (gameObject_id)  VALUES (" & ID & ");")
            End If

            Dim tmp As String = "UPDATE gameObjects SET"

            tmp = tmp & " gameObject_Model=""" & Model & """"
            tmp = tmp & ", gameObject_Type=""" & Type & """"
            tmp = tmp & ", gameObject_Name='" & Name & "'"
            tmp = tmp & ", gameObject_RespawnTime='" & RespawnTime & "'"

            For i As Byte = 0 To 23
                tmp = tmp & ", gameObject_Field" & i & " =""" & Fields(i) & """"
            Next

            tmp = tmp + String.Format(" WHERE gameObject_id = ""{0}"";", ID)
            WorldDatabase.Update(tmp)
        End Sub
    End Class
    'WARNING: Use only with WORLD_GAMEOBJECTs()
    Public Class GameObjectObject
    Inherits BaseObject
    Implements IDisposable

        Public ID As Integer = 0
        Public Flags As Integer = 0
        Public Size As Single = 1
        Public Faction As Integer = 0
        Public State As GameObjectLootState = GameObjectLootState.LOOT_UNLOOTED
        Public Rotation() As Single = {0, 0, 0, 0}
        Public Owner As ULong
        Public Loot As LootObject = Nothing
        Public Despawned As Boolean = False

        Private RespawnTimer As Threading.Timer = Nothing

        Public ReadOnly Property ObjectInfo() As GameObjectInfo
            Get
                Return GAMEOBJECTSDatabase(ID)
            End Get
        End Property

        Public ReadOnly Property Name() As String
            Get
                Return ObjectInfo.Name
            End Get
        End Property
        Public ReadOnly Property Type() As Byte
            Get
                Return ObjectInfo.Type
            End Get
        End Property
        Public ReadOnly Property Sound(ByVal Index As Byte) As Integer
            Get
                Return ObjectInfo.Fields(Index)
            End Get
        End Property

        Public ReadOnly Property LockID() As Integer
            Get
                Select Case ObjectInfo.Type
                    Case GameObjectType.GAMEOBJECT_TYPE_DOOR
                        Return Sound(1)
                    Case GameObjectType.GAMEOBJECT_TYPE_BUTTON
                        Return Sound(1)
                    Case GameObjectType.GAMEOBJECT_TYPE_QUESTGIVER
                        Return Sound(0)
                    Case GameObjectType.GAMEOBJECT_TYPE_CHEST
                        Return Sound(0)
                    Case GameObjectType.GAMEOBJECT_TYPE_TRAP
                        Return Sound(0)
                    Case GameObjectType.GAMEOBJECT_TYPE_GOOBER
                        Return Sound(0)
                    Case GameObjectType.GAMEOBJECT_TYPE_AREADAMAGE
                        Return Sound(0)
                    Case GameObjectType.GAMEOBJECT_TYPE_CAMERA
                        Return Sound(0)
                    Case GameObjectType.GAMEOBJECT_TYPE_FLAGSTAND
                        Return Sound(0)
                    Case GameObjectType.GAMEOBJECT_TYPE_FISHINGHOLE
                        Return Sound(4)
                    Case GameObjectType.GAMEOBJECT_TYPE_FLAGDROP
                        Return Sound(0)
                    Case Else
                        Return 0
                End Select
            End Get
        End Property

        Public Sub FillAllUpdateFlags(ByRef Update As UpdateClass, ByRef Character As CharacterObject)
            Update.SetUpdateFlag(EObjectFields.OBJECT_FIELD_GUID, GUID)
            Update.SetUpdateFlag(EObjectFields.OBJECT_FIELD_TYPE, CType(ObjectType.TYPE_GAMEOBJECT + ObjectType.TYPE_OBJECT, Integer))
            Update.SetUpdateFlag(EObjectFields.OBJECT_FIELD_ENTRY, CType(ID, Integer))
            Update.SetUpdateFlag(EObjectFields.OBJECT_FIELD_SCALE_X, Size)

            If Owner Then Update.SetUpdateFlag(EGameObjectFields.OBJECT_FIELD_CREATED_BY, Owner)
            Update.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_POS_X, positionX)
            Update.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_POS_Y, positionY)
            Update.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_POS_Z, positionZ)
            Update.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_FACING, orientation)

            'If a game object has bit 4 set in the flag it needs to be activated (used for quests)
            'DynFlags = Activate a game object (Chest = 9, Goober = 1)
            Dim DynFlags As Integer = 0
            If Type = GameObjectType.GAMEOBJECT_TYPE_CHEST Then
                Dim UsedForQuest As Byte = IsGameObjectUsedForQuest(Me, Character)
                If UsedForQuest > 0 Then
                    Flags = Flags Or 4
                    If UsedForQuest = 2 Then
                        DynFlags = 9
                    End If
                End If
            ElseIf Type = GameObjectType.GAMEOBJECT_TYPE_GOOBER Then
                'TODO: Check conditions
                DynFlags = 1
            End If
            Update.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_DYN_FLAGS, DynFlags)
            Update.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_STATE, State)

            Update.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_FACTION, Faction)
            ''''Update.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_LEVEL, ObjectInfo.Level)
            Update.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_FLAGS, Flags)
            Update.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_DISPLAYID, ObjectInfo.Model)
            Update.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_TYPE_ID, ObjectInfo.Type)
            Update.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_ROTATION, Rotation(0))
            Update.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_ROTATION + 1, Rotation(1))
            Update.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_ROTATION + 2, Rotation(2))
            Update.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_ROTATION + 3, Rotation(3))
            'Update.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_TIMESTAMP, 0)

            Update.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_ANIMPROGRESS, 300)
        End Sub
        Private Sub Dispose() Implements System.IDisposable.Dispose
            Me.RemoveFromWorld()
            WORLD_GAMEOBJECTs.Remove(GUID)
        End Sub
        Public Sub New(ByVal ID_ As Integer)
            'WARNING: Use only for spawning new object
            If Not GAMEOBJECTSDatabase.ContainsKey(ID_) Then
                Dim baseGameObject As New GameObjectInfo(ID_)
            End If

            ID = ID_
            GUID = GetNewGUID()
            WORLD_GAMEOBJECTs.Add(GUID, Me)
        End Sub
        Public Sub New(ByVal ID_ As Integer, ByVal MapID_ As UInteger, ByVal PosX As Single, ByVal PosY As Single, ByVal PosZ As Single, ByVal Orientation As Single, Optional ByVal Owner_ As ULong = 0)
            'WARNING: Use only for spawning new object
            If Not GAMEOBJECTSDatabase.ContainsKey(ID_) Then
                Dim baseGameObject As New GameObjectInfo(ID_)
            End If

            ID = ID_
            GUID = GetNewGUID()
            MapID = MapID_
            positionX = PosX
            positionY = PosY
            positionZ = PosZ
            Orientation = Orientation
            Owner = Owner_
            WORLD_GAMEOBJECTs.Add(GUID, Me)
        End Sub
        Public Sub New(ByVal cGUID As ULong, Optional ByRef Info As DataRow = Nothing)
            'WARNING: Use only for loading from DB
            If Info Is Nothing Then
                Dim MySQLQuery As New DataTable
                WorldDatabase.Query(String.Format("SELECT * FROM spawns_gameobjects WHERE spawn_id = {0};", cGUID), MySQLQuery)
                If MySQLQuery.Rows.Count > 0 Then
                    Info = MySQLQuery.Rows(0)
                Else
                    Log.WriteLine(LogType.FAILED, "GameObject Spawn not found in database. [cGUID={0:X}]", cGUID)
                End If
            End If

            positionX = Info.Item("spawn_positionX")
            positionY = Info.Item("spawn_positionY")
            positionZ = Info.Item("spawn_positionZ")
            orientation = Info.Item("spawn_orientation")
            MapID = Info.Item("spawn_map")

            Rotation(0) = Info.Item("spawn_orientation1")
            Rotation(1) = Info.Item("spawn_orientation2")
            Rotation(2) = Info.Item("spawn_orientation3")
            Rotation(3) = Info.Item("spawn_orientation4")

            ID = Info.Item("spawn_entry")
            State = Info.Item("spawn_state")
            Flags = Info.Item("spawn_flags")
            Faction = Info.Item("spawn_faction")
            Size = Info.Item("spawn_scale")

            If Not GAMEOBJECTSDatabase.ContainsKey(ID) Then
                Dim baseGameObject As New GameObjectInfo(ID)
            End If

            GUID = cGUID + GUID_GAMEOBJECT
            WORLD_GAMEOBJECTs.Add(GUID, Me)

            'If there's a loottable open for this gameobject already then hook it to the gameobject
            If LootTable.ContainsKey(GUID) Then
                Loot = CType(LootTable(GUID), LootObject)
            End If
        End Sub

        Public Sub AddToWorld()
            GetMapTile(positionX, positionY, CellX, CellY)
            If Maps(MapID).Tiles(CellX, CellY) Is Nothing Then MAP_Load(CellX, CellY, MapID)
            Try
                Maps(MapID).Tiles(CellX, CellY).GameObjectsHere.Add(GUID)
            Catch
                Exit Sub
            End Try

            Dim list() As ULong

            'DONE: Generate loot at spawn
            If Type = GameObjectType.GAMEOBJECT_TYPE_CHEST AndAlso Loot Is Nothing Then GenerateLoot()

            'DONE: Sending to players in nearby cells
            For i As Short = -1 To 1
                For j As Short = -1 To 1
                    If (CellX + i) >= 0 AndAlso (CellX + i) <= 63 AndAlso (CellY + j) >= 0 AndAlso (CellY + j) <= 63 AndAlso Maps(MapID).Tiles(CellX + i, CellY + j) IsNot Nothing AndAlso Maps(MapID).Tiles(CellX + i, CellY + j).PlayersHere.Count > 0 Then
                        With Maps(MapID).Tiles(CellX + i, CellY + j)
                            list = .PlayersHere.ToArray
                            For Each plGUID As ULong In list
                                If CHARACTERs.ContainsKey(plGUID) AndAlso CHARACTERs(plGUID).CanSee(Me) Then
                                    Dim packet As New PacketClass(OPCODES.SMSG_UPDATE_OBJECT)
                                    packet.AddInt32(1)
                                    packet.AddInt8(0)
                                    Dim tmpUpdate As New UpdateClass(FIELD_MASK_SIZE_GAMEOBJECT)
                                    FillAllUpdateFlags(tmpUpdate, CHARACTERs(plGUID))
                                    tmpUpdate.AddToPacket(packet, ObjectUpdateType.UPDATETYPE_CREATE_OBJECT, Me, 0)
                                    tmpUpdate.Dispose()

                                    CHARACTERs(plGUID).Client.SendMultiplyPackets(packet)
                                    CHARACTERs(plGUID).gameObjectsNear.Add(GUID)
                                    SeenBy.Add(plGUID)

                                    packet.Dispose()
                                End If
                            Next
                        End With
                    End If
                Next
            Next

        End Sub
        Public Sub RemoveFromWorld()
            GetMapTile(positionX, positionY, CellX, CellY)
            Maps(MapID).Tiles(CellX, CellY).GameObjectsHere.Remove(GUID)

            'DONE: Removing from players that can see the object
            For Each plGUID As ULong In SeenBy.ToArray
                If CHARACTERs(plGUID).gameObjectsNear.Contains(GUID) Then
                    CHARACTERs(plGUID).guidsForRemoving_Lock.AcquireWriterLock(DEFAULT_LOCK_TIMEOUT)
                    CHARACTERs(plGUID).guidsForRemoving.Add(GUID)
                    CHARACTERs(plGUID).guidsForRemoving_Lock.ReleaseWriterLock()

                    CHARACTERs(plGUID).gameObjectsNear.Remove(GUID)
                End If
            Next
        End Sub

        Public Sub LootObject(ByRef Character As CharacterObject, ByVal LootingType As LootType)
            If Loot Is Nothing Then Exit Sub

            State = GameObjectLootState.LOOT_LOOTED

            Loot.SendLoot(Character.Client)
        End Sub
        Public Function GenerateLoot() As Boolean
            If Not Loot Is Nothing Then Return True
            'DONE: Loot generation
            Dim MySQLQuery As New DataTable
            WorldDatabase.Query(String.Format("SELECT * FROM loots_gameobject WHERE loot_object = {0};", ID), MySQLQuery)
            If MySQLQuery.Rows.Count = 0 Then Return False

            'TODO: Check if we're in a heroic instance!
            Loot = New LootObject(GUID, LootType.LOOTTYPE_SKINNNING)
            For Each LootRow As DataRow In MySQLQuery.Rows
                If CType(LootRow.Item("loot_chance"), Single) * 10000 > (Rnd.Next(1, 2000001) Mod 1000000) Then
                    Dim ItemCount As Byte = CByte(Rnd.Next(CType(LootRow.Item("loot_min"), Byte), CType(LootRow.Item("loot_max"), Byte) + 1))
                    Loot.Items.Add(New LootItem(CType(LootRow.Item("loot_item"), Integer), ItemCount))
                End If
            Next

            Loot.LootOwner = 0

            Return True
        End Function

        Public Sub SpawnAnimation()
            Dim packet As New PacketClass(OPCODES.SMSG_UPDATE_OBJECT)
            packet.AddUInt64(GUID)
            SendToNearPlayers(packet)
            packet.Dispose()
        End Sub
        Public Sub Respawn()
            Log.WriteLine(LogType.DEBUG, "Gameobject {0:X} respawning.", GUID)

            'DONE: Remove the timer
            RespawnTimer.Dispose()
            RespawnTimer = Nothing
            Despawned = False

            'DONE: Add to world
            Loot = Nothing
            State = GameObjectLootState.LOOT_UNLOOTED
            AddToWorld()
        End Sub
        Public Sub Despawn()
            Log.WriteLine(LogType.DEBUG, "Gameobject {0:X} despawning.", GUID)

            'DONE: Remove from world
            Despawned = True
            If Not Loot Is Nothing Then Loot.Dispose()
            RemoveFromWorld()

            'DONE: Start the respawn timer
            RespawnTimer = New Threading.Timer(New TimerCallback(AddressOf Respawn), Nothing, ObjectInfo.RespawnTime, Timeout.Infinite)
        End Sub
        Public Sub Destroy()
            Dim packet As New PacketClass(OPCODES.SMSG_DESTROY_OBJECT)
            packet.AddUInt64(GUID)
            SendToNearPlayers(packet)
            packet.Dispose()

            Me.Dispose()
        End Sub

        Public Sub TurnTo(ByRef Target As BaseObject)
            TurnTo(Target.positionX, Target.positionY)
        End Sub
        Public Sub TurnTo(ByVal x As Single, ByVal y As Single)
            orientation = GetOrientation(positionX, x, positionY, y)
            Rotation(2) = Math.Sin(orientation / 2)
            Rotation(3) = Math.Cos(orientation / 2)

            If SeenBy.Count > 0 Then

                'TODO: Rotation change is not visible with simple update
                Dim packet As New PacketClass(OPCODES.SMSG_UPDATE_OBJECT)
                packet.AddInt32(2)
                packet.AddInt8(0)
                Dim tmpUpdate As New UpdateClass(FIELD_MASK_SIZE_GAMEOBJECT)
                tmpUpdate.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_FACING, orientation)
                tmpUpdate.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_ROTATION, Rotation(0))
                tmpUpdate.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_ROTATION + 1, Rotation(1))
                tmpUpdate.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_ROTATION + 2, Rotation(2))
                tmpUpdate.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_ROTATION + 3, Rotation(3))
                tmpUpdate.AddToPacket(packet, ObjectUpdateType.UPDATETYPE_VALUES, Me, 0)
                tmpUpdate.Dispose()

                SendToNearPlayers(packet)
                packet.Dispose()
            End If
        End Sub
    End Class

    Public Enum GameObjectLootState As Byte
        DOOR_OPEN = 0
        DOOR_CLOSED = 1
        LOOT_UNAVIABLE = 0
        LOOT_UNLOOTED = 1
        LOOT_LOOTED = 2
    End Enum

    #End Region
    #Region "WS.GameObjects.HelperSubs"

    <MethodImplAttribute(MethodImplOptions.Synchronized)> _
    Private Function GetNewGUID() As ULong
        GameObjectsGUIDCounter += 1
        GetNewGUID = GameObjectsGUIDCounter
    End Function
    Public Enum GameObjectType As Byte
        GAMEOBJECT_TYPE_DOOR = 0
        GAMEOBJECT_TYPE_BUTTON = 1
        GAMEOBJECT_TYPE_QUESTGIVER = 2
        GAMEOBJECT_TYPE_CHEST = 3
        GAMEOBJECT_TYPE_BINDER = 4
        GAMEOBJECT_TYPE_GENERIC = 5
        GAMEOBJECT_TYPE_TRAP = 6
        GAMEOBJECT_TYPE_CHAIR = 7
        GAMEOBJECT_TYPE_SPELL_FOCUS = 8
        GAMEOBJECT_TYPE_TEXT = 9
        GAMEOBJECT_TYPE_GOOBER = 10
        GAMEOBJECT_TYPE_TRANSPORT = 11
        GAMEOBJECT_TYPE_AREADAMAGE = 12
        GAMEOBJECT_TYPE_CAMERA = 13
        GAMEOBJECT_TYPE_MAP_OBJECT = 14
        GAMEOBJECT_TYPE_MO_TRANSPORT = 15
        GAMEOBJECT_TYPE_DUEL_ARBITER = 16
        GAMEOBJECT_TYPE_FISHINGNODE = 17
        GAMEOBJECT_TYPE_RITUAL = 18
        GAMEOBJECT_TYPE_MAILBOX = 19
        GAMEOBJECT_TYPE_AUCTIONHOUSE = 20
        GAMEOBJECT_TYPE_GUARDPOST = 21
        GAMEOBJECT_TYPE_SPELLCASTER = 22
        GAMEOBJECT_TYPE_MEETINGSTONE = 23
        GAMEOBJECT_TYPE_FLAGSTAND = 24
        GAMEOBJECT_TYPE_FISHINGHOLE = 25
        GAMEOBJECT_TYPE_FLAGDROP = 26
        GAMEOBJECT_TYPE_MINI_GAME = 27
        GAMEOBJECT_TYPE_LOTTERY_KIOSK = 28
        GAMEOBJECT_TYPE_CAPTURE_POINT = 29
        GAMEOBJECT_TYPE_AURA_GENERATOR = 30
        GAMEOBJECT_TYPE_DUNGEON_DIFFICULTY = 31
        GAMEOBJECT_TYPE_DO_NOT_USE_YET = 32
        GAMEOBJECT_TYPE_DESTRUCTIBLE_BUILDING = 33
        GAMEOBJECT_TYPE_GUILD_BANK = 34
    End Enum

    Public Sub On_CMSG_GAMEOBJECT_QUERY(ByRef packet As PacketClass, ByRef Client As ClientClass)
        If (packet.Data.Length - 1) < 17 Then Exit Sub
        Dim response As New PacketClass(OPCODES.SMSG_GAMEOBJECT_QUERY_RESPONSE)

        packet.GetInt16()
        Dim GameObjectID As Integer = packet.GetInt32
        Dim GameObjectGUID As ULong = packet.GetUInt64

        Try
            Dim GameObject As GameObjectInfo

            If GAMEOBJECTSDatabase.ContainsKey(GameObjectID) = False Then
                Log.WriteLine(LogType.DEBUG, "[{0}:{1}] CMSG_GAMEOBJECT_QUERY [GameObject {2} not loaded.]", Client.IP, Client.Port, GameObjectID)
                Exit Sub
            Else
                GameObject = GAMEOBJECTSDatabase(GameObjectID)
            'Log.WriteLine(LogType.DEBUG, "[{0}:{1}] CMSG_GAMEOBJECT_QUERY [GameObjectID={2} GameObjectGUID={3:X}]", Client.IP, Client.Port, GameObjectID, GameObjectGUID - GUID_GAMEOBJECT)
            End If

            response.AddInt32(GameObject.ID)
            response.AddInt32(GameObject.Type)
            response.AddInt32(GameObject.Model)
            response.AddString(GameObject.Name)
            response.AddInt32(0)                    'New in 1.12 - 4 strings (3 = names, 1 = unk)
            response.AddInt16(0)                    'New in 2.0.3 - (1 = Cast Bar Text, 1 = unk)

            For i As Byte = 0 To 23
                response.AddInt32(GameObject.Fields(i))
            Next i

            Client.Send(response)
            response.Dispose()
            'Log.WriteLine(LogType.DEBUG, "[{0}:{1}] SMSG_GAMEOBJECT_QUERY_RESPONSE", Client.IP, Client.Port)
        Catch e As Exception
            Log.WriteLine(LogType.FAILED, "Unknown Error: Unable to find GameObjectID={0} in database.", GameObjectID)
        End Try
    End Sub
    Public Sub On_CMSG_GAMEOBJ_USE(ByRef packet As PacketClass, ByRef Client As ClientClass)
        If (packet.Data.Length - 1) < 13 Then Exit Sub
        packet.GetInt16()
        Dim GameObjectGUID As ULong = packet.GetUInt64

        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] CMSG_GAMEOBJ_USE [GUID={2:X}]", Client.IP, Client.Port, GameObjectGUID)

        If WORLD_GAMEOBJECTs.ContainsKey(GameObjectGUID) = False Then Exit Sub

        Log.WriteLine(LogType.DEBUG, "GameObjectType: {0}", CType(WORLD_GAMEOBJECTs(GameObjectGUID).Type, GameObjectType))
        Select Case CType(WORLD_GAMEOBJECTs(GameObjectGUID).Type, GameObjectType)

            Case GameObjectType.GAMEOBJECT_TYPE_DOOR
                'DONE: Doors opening
                CType(WORLD_GAMEOBJECTs(GameObjectGUID), GameObjectObject).State = GameObjectLootState.DOOR_OPEN

                Dim response As New PacketClass(OPCODES.SMSG_UPDATE_OBJECT)
                response.AddInt32(1)
                response.AddInt8(1)
                Dim UpdateData As New UpdateClass
                UpdateData.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_STATE, WORLD_GAMEOBJECTs(GameObjectGUID).State)
                UpdateData.SetUpdateFlag(EGameObjectFields.GAMEOBJECT_FLAGS, WORLD_GAMEOBJECTs(GameObjectGUID).Flags)
                UpdateData.AddToPacket(response, ObjectUpdateType.UPDATETYPE_VALUES, CType(WORLD_GAMEOBJECTs(GameObjectGUID), GameObjectObject), 1)
                WORLD_GAMEOBJECTs(GameObjectGUID).SendToNearPlayers(response)
                UpdateData.Dispose()
                response.Dispose()

                ThreadPool.RegisterWaitForSingleObject(New AutoResetEvent(False), New WaitOrTimerCallback(AddressOf CType(WORLD_GAMEOBJECTs(GameObjectGUID), GameObjectObject).Respawn), Nothing, 6000, True)

            Case GameObjectType.GAMEOBJECT_TYPE_CHAIR
                'DONE: Chair sitting
                Client.Character.Teleport(WORLD_GAMEOBJECTs(GameObjectGUID).positionX, WORLD_GAMEOBJECTs(GameObjectGUID).positionY, WORLD_GAMEOBJECTs(GameObjectGUID).positionZ, WORLD_GAMEOBJECTs(GameObjectGUID).orientation, WORLD_GAMEOBJECTs(GameObjectGUID).MapID)

                'STATE_SITTINGCHAIRLOW = 4
                'STATE_SITTINGCHAIRMEDIUM = 5
                'STATE_SITTINGCHAIRHIGH = 6
                Client.Character.StandState = (3 + WORLD_GAMEOBJECTs(GameObjectGUID).Sound(1))

            Case GameObjectType.GAMEOBJECT_TYPE_QUESTGIVER
                Dim qm As QuestMenu = GetQuestMenuGO(Client.Character, GameObjectGUID)
                SendQuestMenu(Client.Character, GameObjectGUID, , qm)

            Case GameObjectType.GAMEOBJECT_TYPE_RITUAL
                Log.WriteLine(LogType.DEBUG, "Clicked a ritual.")
                'DONE: You can only click on rituals by group members
                If WORLD_GAMEOBJECTs(GameObjectGUID).Owner <> Client.Character.GUID AndAlso Client.Character.IsInGroup = False Then Exit Sub
                If WORLD_GAMEOBJECTs(GameObjectGUID).Owner <> Client.Character.GUID Then
                    If CHARACTERs.ContainsKey(WORLD_GAMEOBJECTs(GameObjectGUID).Owner) = False OrElse CHARACTERs(WORLD_GAMEOBJECTs(GameObjectGUID).Owner).IsInGroup = False Then Exit Sub
                    If Not CHARACTERs(WORLD_GAMEOBJECTs(GameObjectGUID).Owner).Group Is Client.Character.Group Then Exit Sub
                End If

                Log.WriteLine(LogType.DEBUG, "Casting ritual spell.")
                Client.Character.CastOnSelf(WORLD_GAMEOBJECTs(GameObjectGUID).Sound(1))

            Case GameObjectType.GAMEOBJECT_TYPE_SPELLCASTER
                Log.WriteLine(LogType.DEBUG, "Clicked a spellcaster.")

                WORLD_GAMEOBJECTs(GameObjectGUID).Flags = 2

                'DONE: Check if you're in the same party
                If WORLD_GAMEOBJECTs(GameObjectGUID).Sound(2) Then
                    Log.WriteLine(LogType.DEBUG, "Spellcaster requires same group.")
                    Log.WriteLine(LogType.DEBUG, "Owner: {0:X}  You: {1:X}", WORLD_GAMEOBJECTs(GameObjectGUID).Owner, Client.Character.GUID)
                    If WORLD_GAMEOBJECTs(GameObjectGUID).Owner <> Client.Character.GUID AndAlso Client.Character.IsInGroup = False Then Exit Sub
                    If WORLD_GAMEOBJECTs(GameObjectGUID).Owner <> Client.Character.GUID Then
                        If CHARACTERs.ContainsKey(WORLD_GAMEOBJECTs(GameObjectGUID).Owner) = False OrElse CHARACTERs(WORLD_GAMEOBJECTs(GameObjectGUID).Owner).IsInGroup = False Then Exit Sub
                        If Not CHARACTERs(WORLD_GAMEOBJECTs(GameObjectGUID).Owner).Group Is Client.Character.Group Then Exit Sub
                    End If
                End If

                Log.WriteLine(LogType.DEBUG, "Casted spellcaster spell.")
                Client.Character.CastOnSelf(WORLD_GAMEOBJECTs(GameObjectGUID).Sound(0))

                'TODO: Remove one charge

            Case GameObjectType.GAMEOBJECT_TYPE_MEETINGSTONE
                If Client.Character.Level < WORLD_GAMEOBJECTs(GameObjectGUID).Sound(0) Then 'Too low level
                    'TODO: Send the correct packet.
                    SendCastResult(SpellFailedReason.CAST_FAIL_LEVEL_REQUIREMENT, Client, 23598, 0)
                    Exit Sub
                End If
                If Client.Character.Level > WORLD_GAMEOBJECTs(GameObjectGUID).Sound(1) Then 'Too high level
                    'TODO: Send the correct packet.
                    SendCastResult(SpellFailedReason.CAST_FAIL_LEVEL_REQUIREMENT, Client, 23598, 0)
                    Exit Sub
                End If
                Client.Character.CastOnSelf(23598)

        End Select
    End Sub

    #End Region

End Module
