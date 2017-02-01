'
' Copyright (C) 2013 - 2017 getMaNGOS <http://www.getmangos.eu>
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
Imports System.Net.Sockets
Imports System.Xml.Serialization
Imports System.IO
Imports System.Net
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports mangosVB.Common.BaseWriter
Imports mangosVB.Common

Public Module WC_Handlers_Auth

    Public Sub SendLoginOK(ByRef Client As ClientClass)
        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] CMSG_AUTH_SESSION [{2}]", Client.IP, Client.Port, Client.Account)

        Thread.Sleep(500)

        Dim response As New PacketClass(OPCODES.SMSG_AUTH_RESPONSE)
        response.AddInt16(AuthResponseCodes.AUTH_OK)
        response.AddInt32(timeGetTime)          'time in minutes before "?"
        response.AddInt32(0)                    'always 00
        response.AddInt8(Client.Expansion)      'ExpansionRacesEnable
        Client.Send(response)
    End Sub
    Public Sub On_CMSG_AUTH_SESSION(ByRef packet As PacketClass, ByRef Client As ClientClass)
        'DumpPacket(packet.Data, Client)
        Dim i As Integer
        'Console.WriteLine("[{0}] [{1}:{2}] CMSG_AUTH_SESSION", Format(TimeOfDay, "hh:mm:ss"), Client.IP, Client.Port)

        packet.GetInt16()
        Dim clientVersion As Integer = packet.GetInt32
        Dim clientSesionID As Integer = packet.GetInt32
        Dim clientAccount As String = packet.GetString
        Dim clientSalt As Integer = packet.GetInt32
        Dim clientEncryptedPassword(19) As Byte
        For i = 0 To 19
            clientEncryptedPassword(i) = packet.GetInt8
        Next
        Dim clientAddOnsSize As Integer = packet.GetInt32

        'DONE: Set Client.Account
        Dim tmp As String = clientAccount

        'DONE: Kick if existing
        For Each tmpClientEntry As KeyValuePair(Of UInteger, ClientClass) In CLIENTs
            If Not tmpClientEntry.Value Is Nothing Then
                If tmpClientEntry.Value.Account = tmp Then
                    If Not tmpClientEntry.Value.Character Is Nothing Then
                        tmpClientEntry.Value.Character.Dispose()
                        tmpClientEntry.Value.Character = Nothing
                    End If
                    Try
                        tmpClientEntry.Value.Socket.Shutdown(SocketShutdown.Both)
                    Catch
                        tmpClientEntry.Value.Socket.Close()
                    End Try
                End If
            End If
        Next
        Client.Account = tmp

        'DONE: Set Client.SS_Hash
        Dim result As New DataTable
        Dim query As String
        query = "SELECT * FROM accounts WHERE account = '" & Client.Account & "';"
        AccountDatabase.Query(query, result)
        If result.Rows.Count > 0 Then
            tmp = result.Rows(0).Item("last_sshash")
            Client.Access = result.Rows(0).Item("plevel")
            Client.Expansion = result.Rows(0).Item("expansion")
        Else
            Log.WriteLine(LogType.USER, "[{0}:{1}] AUTH_UNKNOWN_ACCOUNT: Account not in DB!", Client.IP, Client.Port)
            Dim response_no_acc As New PacketClass(OPCODES.SMSG_AUTH_RESPONSE)
            response_no_acc.AddInt8(AuthResponseCodes.AUTH_UNKNOWN_ACCOUNT)
            Client.Send(response_no_acc)
            Exit Sub
        End If
        ReDim Client.SS_Hash(19)
        For i = 0 To Len(tmp) - 1 Step 2
            Client.SS_Hash(i \ 2) = Val("&H" & Mid(tmp, i + 1, 2))
        Next
        Client.Encryption = True

        'DONE: If server full then queue, If GM/Admin let in
        If CLIENTs.Count > Config.ServerLimit And Client.Access <= AccessLevel.Player Then
            ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf Client.EnQueue))
        Else
            SendLoginOK(Client)
        End If

        'DONE: Addons info reading
        Dim decompressBuffer(packet.Data.Length - packet.Offset) As Byte
        Array.Copy(packet.Data, packet.Offset, decompressBuffer, 0, packet.Data.Length - packet.Offset)
        packet.Data = DeCompress(decompressBuffer)
        packet.Offset = 0
        'DumpPacket(packet.Data)

        Dim AddOnsNames As New List(Of String)
        Dim AddOnsHashes As New List(Of ULong)
        'Dim AddOnsConsoleWrite As String = String.Format("[{0}:{1}] Client addons loaded:", Client.IP, Client.Port)
        While packet.Offset < clientAddOnsSize
            AddOnsNames.Add(packet.GetString)
            AddOnsHashes.Add(packet.GetUInt64)
            'AddOnsConsoleWrite &= String.Format("{0}{1} AddOnName: [{2,-30}], AddOnHash: [{3:X}]", vbNewLine, vbTab, AddOnsNames(AddOnsNames.Count - 1), AddOnsHashes(AddOnsHashes.Count - 1))
            packet.GetInt8()
        End While
        'Log.WriteLine(LogType.DEBUG, AddOnsConsoleWrite)

        'DONE: Build mysql addons query
        'Not needed already - in 1.11 addons list is removed.

        'DONE: Send packet
        Dim addOnsEnable As New PacketClass(OPCODES.SMSG_ADDON_INFO)
        For i = 0 To AddOnsNames.Count - 1
            If IO.File.Exists(String.Format("interface\{0}.pub", AddOnsNames(i))) And (AddOnsHashes(i) <> &H4C1C776D01) Then
                'We have hash data
                addOnsEnable.AddInt8(2)                    'AddOn Type [1-enabled, 0-banned, 2-blizzard]
                addOnsEnable.AddInt8(1)                    'Unk

                Dim fs As New IO.FileStream(String.Format("interface\{0}.pub", AddOnsNames(i)), IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read, 258, IO.FileOptions.SequentialScan)
                Dim fb(256) As Byte
                fs.Read(fb, 0, 257)

                'NOTE: Read from file
                addOnsEnable.AddByteArray(fb)
                addOnsEnable.AddInt8(0)
                addOnsEnable.AddInt32(0)
            Else
                'We don't have hash data or already sent to client
                addOnsEnable.AddInt8(2)                    'AddOn Type [1-enabled, 0-banned, 2-blizzard]
                addOnsEnable.AddInt8(1)                    'Unk
                addOnsEnable.AddInt8(0)                    'Hash       [0-NoData, 1-256bytes of data]
                addOnsEnable.AddInt8(0)
                addOnsEnable.AddInt32(0)
            End If
        Next
        Client.Send(addOnsEnable)
        addOnsEnable.Dispose()
    End Sub
    Public Sub On_CMSG_PING(ByRef packet As PacketClass, ByRef Client As ClientClass)
        If (packet.Data.Length - 1) < 9 Then Exit Sub
        packet.GetInt16()

        Dim response As New PacketClass(OPCODES.SMSG_PONG)
        response.AddInt32(packet.GetInt32)
        Client.Send(response)

        If Not Client.Character Is Nothing Then
            Client.Character.Latency = packet.GetInt32
        End If

        'Log.WriteLine(LogType.NETWORK, "[{0}:{1}] SMSG_PONG [{2}]", Client.IP, Client.Port, Client.Character.Latency)
    End Sub
    Public Sub On_CMSG_REALM_SPLIT(ByRef packet As PacketClass, ByRef Client As ClientClass)
        Dim response As New PacketClass(OPCODES.SMSG_REALM_SPLIT)
        response.AddInt32(-1)
        response.AddInt32(0)                        'State: 0x0 normal, 0x1 split, 0x2 split pending
        response.AddString("01/01/01")              'Date
        Client.Send(response)
        response.Dispose()
    End Sub

    Public Sub On_CMSG_UPDATE_ACCOUNT_DATA(ByRef packet As PacketClass, ByRef Client As ClientClass)
        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] CMSG_UPDATE_ACCOUNT_DATA", Client.IP, Client.Port)
        Dim DataID As Integer = packet.GetInt32
        Dim DataLEN As Integer = packet.Length - 6     'May be used and packet.Length
        Dim Data(DataLEN - 1) As Char

        Dim i As Integer
        For i = 0 To DataLEN - 1
            Data(i) = Chr(packet.GetInt8)
        Next

        'Console.WriteLine("DEBUG: MD5Data={0}", CStr(Data))
    End Sub
    Public Sub On_CMSG_REQUEST_ACCOUNT_DATA(ByRef packet As PacketClass, ByRef Client As ClientClass)
        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] CMSG_REQUEST_ACCOUNT_DATA", Client.IP, Client.Port)
    End Sub

    <Flags()> _
    Private Enum CharacterFlagState
        CHARACTER_FLAG_NONE = &H0
        CHARACTER_FLAG_UNK1 = &H1
        CHARACTER_FLAG_UNK2 = &H2
        CHARACTER_FLAG_LOCKED_FOR_TRANSFER = &H4                    'Character Locked for Paid Character Transfer
        CHARACTER_FLAG_UNK4 = &H8
        CHARACTER_FLAG_UNK5 = &H10
        CHARACTER_FLAG_UNK6 = &H20
        CHARACTER_FLAG_UNK7 = &H40
        CHARACTER_FLAG_UNK8 = &H80
        CHARACTER_FLAG_UNK9 = &H100
        CHARACTER_FLAG_UNK10 = &H200
        CHARACTER_FLAG_HIDE_HELM = &H400
        CHARACTER_FLAG_HIDE_CLOAK = &H800
        CHARACTER_FLAG_UNK13 = &H1000
        CHARACTER_FLAG_GHOST = &H2000                               'Player is ghoust in char selection screen
        CHARACTER_FLAG_RENAME = &H4000                              'On login player will be asked to change name
        CHARACTER_FLAG_UNK16 = &H8000
        CHARACTER_FLAG_UNK17 = &H10000
        CHARACTER_FLAG_UNK18 = &H20000
        CHARACTER_FLAG_UNK19 = &H40000
        CHARACTER_FLAG_UNK20 = &H80000
        CHARACTER_FLAG_UNK21 = &H100000
        CHARACTER_FLAG_UNK22 = &H200000
        CHARACTER_FLAG_UNK23 = &H400000
        CHARACTER_FLAG_UNK24 = &H800000
        CHARACTER_FLAG_LOCKED_BY_BILLING = &H1000000
        CHARACTER_FLAG_DECLINED = &H2000000
        CHARACTER_FLAG_UNK27 = &H4000000
        CHARACTER_FLAG_UNK28 = &H8000000
        CHARACTER_FLAG_UNK29 = &H10000000
        CHARACTER_FLAG_UNK30 = &H20000000
        CHARACTER_FLAG_UNK31 = &H40000000
        CHARACTER_FLAG_UNK32 = &H80000000
    End Enum
    Public Sub On_CMSG_CHAR_ENUM(ByRef packet As PacketClass, ByRef Client As ClientClass)
        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] CMSG_CHAR_ENUM", Client.IP, Client.Port)

        'DONE: Query Characters DB
        Dim response As New PacketClass(OPCODES.SMSG_CHAR_ENUM)
        Dim MySQLQuery As New DataTable
        Dim Account_ID As Integer

        Try
            AccountDatabase.Query(String.Format("SELECT account_id FROM accounts WHERE account = ""{0}"";", Client.Account), MySQLQuery)
            Account_ID = CType(MySQLQuery.Rows(0).Item("account_id"), Integer)
            MySQLQuery.Clear()
            CharacterDatabase.Query(String.Format("SELECT * FROM characters WHERE account_id = ""{0}"" ORDER BY char_guid;", Account_ID), MySQLQuery)

            'DONE: Make The Packet
            response.AddInt8(MySQLQuery.Rows.Count)
            For i As Integer = 0 To MySQLQuery.Rows.Count - 1
                Dim DEAD As Boolean = False
                Dim DeadMySQLQuery As New DataTable
                CharacterDatabase.Query(String.Format("SELECT COUNT(*) FROM tmpspawnedcorpses WHERE corpse_owner = {0};", MySQLQuery.Rows(i).Item("char_guid")), DeadMySQLQuery)
                If CInt(DeadMySQLQuery.Rows(0).Item(0)) > 0 Then DEAD = True

                response.AddInt64(MySQLQuery.Rows(i).Item("char_guid"))
                response.AddString(MySQLQuery.Rows(i).Item("char_name"))
                response.AddInt8(MySQLQuery.Rows(i).Item("char_race"))
                response.AddInt8(MySQLQuery.Rows(i).Item("char_class"))
                response.AddInt8(MySQLQuery.Rows(i).Item("char_gender"))
                response.AddInt8(MySQLQuery.Rows(i).Item("char_skin"))
                response.AddInt8(MySQLQuery.Rows(i).Item("char_face"))
                response.AddInt8(MySQLQuery.Rows(i).Item("char_hairStyle"))
                response.AddInt8(MySQLQuery.Rows(i).Item("char_hairColor"))
                response.AddInt8(MySQLQuery.Rows(i).Item("char_facialHair"))
                response.AddInt8(MySQLQuery.Rows(i).Item("char_level"))
                response.AddInt32(MySQLQuery.Rows(i).Item("char_zone_id"))
                response.AddInt32(MySQLQuery.Rows(i).Item("char_map_id"))
                response.AddSingle(MySQLQuery.Rows(i).Item("char_positionX"))
                response.AddSingle(MySQLQuery.Rows(i).Item("char_positionY"))
                response.AddSingle(MySQLQuery.Rows(i).Item("char_positionZ"))
                response.AddInt32(MySQLQuery.Rows(i).Item("char_guildId"))

                Dim playerState As UInteger = CharacterFlagState.CHARACTER_FLAG_NONE
                If CInt(MySQLQuery.Rows(i).Item("force_restrictions")) = 3 Then
                    playerState += CharacterFlagState.CHARACTER_FLAG_LOCKED_FOR_TRANSFER
                End If
                If CInt(MySQLQuery.Rows(i).Item("force_restrictions")) = 2 Then
                    playerState += CharacterFlagState.CHARACTER_FLAG_LOCKED_BY_BILLING
                End If
                If CInt(MySQLQuery.Rows(i).Item("force_restrictions")) = 1 Then
                    playerState += CharacterFlagState.CHARACTER_FLAG_RENAME
                End If
                If DEAD Then
                    playerState += CharacterFlagState.CHARACTER_FLAG_GHOST
                End If

                response.AddUInt32(playerState)
                response.AddInt8(MySQLQuery.Rows(i).Item("char_restState"))
                response.AddInt32(0)    'response.AddInt32(MySQLQuery.Rows(i).Item("pet_infoId"))
                response.AddInt32(0)    'response.AddInt32(MySQLQuery.Rows(i).Item("pet_level"))
                response.AddInt32(0)    'response.AddInt32(MySQLQuery.Rows(i).Item("pet_familyId"))

                'DONE: Get items
                Dim GUID As Long = MySQLQuery.Rows(i).Item("char_guid")
                Dim ItemsMySQLQuery As New DataTable
               'CharacterDatabase.Query(String.Format("SELECT item_slot, displayid, inventorytype FROM characters_inventory, items WHERE item_bag = {0} AND item_slot <> 255 AND entry = item_id  ORDER BY item_slot;", GUID), ItemsMySQLQuery)
                CharacterDatabase.Query(String.Format("SELECT item_slot, item_id, displayid, inventorytype FROM characters_inventory, items WHERE item_bag = {0} AND item_slot <> 255 AND items.entry = characters_inventory.item_id ORDER BY item_slot;", GUID), ItemsMySQLQuery)
                Dim e As IEnumerator = ItemsMySQLQuery.Rows.GetEnumerator
                e.Reset()
                e.MoveNext()
                Dim r As DataRow = e.Current

                'DONE: Add model info
                For slot As Byte = 0 To EQUIPMENT_SLOT_END '- 1
                    If r Is Nothing OrElse CInt(r.Item("item_slot")) <> slot Then
                        'No equiped item in this slot
                        response.AddInt32(0) 'Item Model
                        response.AddInt8(0)  'Item Slot
                        response.AddInt32(0) ' New in 2.4 (Enchant maybe?)
                    Else
                        response.AddInt32(r.Item("displayid"))          'Item Model
                        response.AddInt8(r.Item("inventorytype"))       'Item Slot
                        response.AddInt32(0)                            ' New in 2.4 (Enchant maybe?)

                        e.MoveNext()
                        r = e.Current
                    End If
                Next
            Next i

        Catch e As Exception
            Log.WriteLine(LogType.FAILED, "[{0}:{1}] Unable to enum characters. [{2}]", Client.IP, Client.Port, e.Message)
            'TODO: Find what opcode officials use
            response = New PacketClass(OPCODES.SMSG_CHAR_CREATE)
            response.AddInt8(AuthResponseCodes.CHAR_LIST_FAILED)
        End Try

        Client.Send(response)
        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] SMSG_CHAR_ENUM", Client.IP, Client.Port)
    End Sub
    Public Sub On_CMSG_CHAR_DELETE(ByRef packet As PacketClass, ByRef Client As ClientClass)
        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] CMSG_CHAR_DELETE", Client.IP, Client.Port)

        Dim response As New PacketClass(OPCODES.SMSG_CHAR_DELETE)
        Dim guid As Long = 0
        guid = packet.GetInt16()    'int16 unknown
        guid = packet.GetInt64()    'int64 guid

        Try
            Dim q As New DataTable

            CharacterDatabase.Query(String.Format("SELECT item_guid FROM characters_inventory WHERE item_bag = {0};", guid), q)
            For Each row As DataRow In q.Rows
                'DONE: Delete items
                CharacterDatabase.Update(String.Format("DELETE FROM characters_inventory WHERE item_guid = ""{0}"";", row.Item("item_guid")))
                'DONE: Delete items in bags
                CharacterDatabase.Update(String.Format("DELETE FROM characters_inventory WHERE item_bag = ""{0}"";", CULng(row.Item("item_guid")) + GUID_ITEM))
            Next
            q.Clear()
            CharacterDatabase.Query(String.Format("SELECT mail_id FROM characters_mail WHERE mail_receiver = ""{0}"";", guid), q)
            For Each row As DataRow In q.Rows
                'DONE: Delete mails
                CharacterDatabase.Update(String.Format("DELETE FROM characters_mail WHERE mail_id = ""{0}"";", row.Item("mail_id")))
                'DONE: Delete mail items
                CharacterDatabase.Update(String.Format("DELETE FROM mail_items WHERE mail_id = ""{0}"";", row.Item("mail_id")))
            Next
            CharacterDatabase.Update(String.Format("DELETE FROM characters WHERE char_guid = ""{0}"";", guid))
            CharacterDatabase.Update(String.Format("DELETE FROM characters_honor WHERE char_guid = ""{0}"";", guid))
            CharacterDatabase.Update(String.Format("DELETE FROM characters_quests WHERE char_guid = ""{0}"";", guid))
            CharacterDatabase.Update(String.Format("DELETE FROM characters_social WHERE char_guid = ""{0}"";", guid))
            CharacterDatabase.Update(String.Format("DELETE FROM petitions WHERE petition_owner = ""{0}"";", guid))
            CharacterDatabase.Update(String.Format("DELETE FROM auctionhouse WHERE auction_owner = ""{0}"";", guid))
            CharacterDatabase.Update(String.Format("DELETE FROM characters_tickets WHERE char_guid = ""{0}"";", guid))
            CharacterDatabase.Update(String.Format("DELETE FROM tmpspawnedcorpses WHERE corpse_owner = ""{0}"";", guid))

            q.Clear()
            CharacterDatabase.Query(String.Format("SELECT guild_id FROM guilds WHERE guild_leader = ""{0}"";", guid), q)
            If q.Rows.Count > 0 Then
                CharacterDatabase.Update(String.Format("UPDATE characters SET char_guildid=0, char_guildrank=0, char_guildpnote='', charguildoffnote='' WHERE char_guildid=""{0}"";", q.Rows(0).Item("guild_id")))
                CharacterDatabase.Update(String.Format("DELETE FROM guild WHERE guild_id=""{0}"";", q.Rows(0).Item("guild_id")))
            End If
            response.AddInt8(AuthResponseCodes.CHAR_DELETE_SUCCESS)
        Catch e As Exception
            response.AddInt8(AuthResponseCodes.CHAR_DELETE_FAILED)
        End Try

        Client.Send(response)
        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] SMSG_CHAR_DELETE [{2:X}]", Client.IP, Client.Port, guid)
    End Sub
    Public Sub On_CMSG_CHAR_RENAME(ByRef packet As PacketClass, ByRef Client As ClientClass)
        packet.GetInt16()
        Dim GUID As Long = packet.GetInt64()
        Dim Name As String = packet.GetString
        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] CMSG_CHAR_RENAME [{2}:{3}]", Client.IP, Client.Port, GUID, Name)

        Dim ErrCode As Byte = AuthResponseCodes.RESPONSE_SUCCESS

        'DONE: Check for existing name
        Dim q As New DataTable
        CharacterDatabase.Query(String.Format("SELECT char_name FROM characters WHERE char_name LIKE ""{0}"";", Name), q)
        If q.Rows.Count > 0 Then
            ErrCode = AuthResponseCodes.CHAR_CREATE_NAME_IN_USE
        End If

        'DONE: Do the rename
        If ErrCode = AuthResponseCodes.RESPONSE_SUCCESS Then CharacterDatabase.Update(String.Format("UPDATE characters SET char_name = ""{1}"", force_restrictions = 0 WHERE char_guid = {0};", GUID, Name))

        'DONE: Send response
        Dim response As New PacketClass(OPCODES.SMSG_CHAR_RENAME)
        response.AddInt8(ErrCode)
        Client.Send(response)
        response.Dispose()

        On_CMSG_CHAR_ENUM(Nothing, Client)
    End Sub
    Public Sub On_CMSG_CHAR_CREATE(ByRef packet As PacketClass, ByRef Client As ClientClass)
        packet.GetInt16()

        Dim Name As String = packet.GetString

        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] CMSG_CHAR_CREATE [{2}]", Client.IP, Client.Port, Name)

        Dim Race As Byte = packet.GetInt8
        Dim Classe As Byte = packet.GetInt8
        Dim Gender As Byte = packet.GetInt8
        Dim Skin As Byte = packet.GetInt8
        Dim Face As Byte = packet.GetInt8
        Dim HairStyle As Byte = packet.GetInt8
        Dim HairColor As Byte = packet.GetInt8
        Dim FacialHair As Byte = packet.GetInt8
        Dim OutfitId As Byte = packet.GetInt8

        Dim result As Integer = AuthResponseCodes.CHAR_CREATE_DISABLED

        'Try to pass the packet to one of World Servers
        Try
            If WS.Worlds.ContainsKey(0) Then
                result = WS.Worlds(0).ClientCreateCharacter(Client.Account, Name, Race, Classe, Gender, Skin, Face, HairStyle, HairColor, FacialHair, OutfitId)
            ElseIf WS.Worlds.ContainsKey(1) Then
                result = WS.Worlds(1).ClientCreateCharacter(Client.Account, Name, Race, Classe, Gender, Skin, Face, HairStyle, HairColor, FacialHair, OutfitId)
            ElseIf WS.Worlds.ContainsKey(530) Then
                result = WS.Worlds(530).ClientCreateCharacter(Client.Account, Name, Race, Classe, Gender, Skin, Face, HairStyle, HairColor, FacialHair, OutfitId)
            End If
        Catch ex As Exception
            result = AuthResponseCodes.CHAR_CREATE_FAILED
            Log.WriteLine(LogType.FAILED, "[{0}:{1}] Character creation failed!{2}{3}", Client.IP, Client.Port, vbNewLine, ex.ToString)
        End Try

        Dim response As New PacketClass(OPCODES.SMSG_CHAR_CREATE)
        response.AddInt8(result)
        Client.Send(response)
    End Sub

    Public Sub On_CMSG_PLAYER_LOGIN(ByRef packet As PacketClass, ByRef Client As ClientClass)
        Dim GUID As ULong = 0
        packet.GetInt16()               'int16 unknown
        GUID = packet.GetUInt64()       'uint64 guid
        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] CMSG_PLAYER_LOGIN [0x{2:X}]", Client.IP, Client.Port, GUID)

        Try
            If Client.Character Is Nothing Then
                Client.Character = New CharacterObject(GUID, Client)
            Else
                If Client.Character.GUID <> GUID Then
                    Client.Character.Dispose()
                    Client.Character = New CharacterObject(GUID, Client)
                Else
                    Client.Character.ReLoad()
                End If
            End If

            If WS.InstanceCheck(Client, Client.Character.Map) Then
                Client.Character.GetWorld.ClientConnect(Client.Index, Client.GetClientInfo)
                Client.Character.IsInWorld = True
                Client.Character.GetWorld.ClientLogin(Client.Index, Client.Character.GUID)

                Client.Character.OnLogin()
            Else
                Log.WriteLine(LogType.FAILED, "[{0:000000}] Unable to login: WORLD SERVER DOWN", Client.Index)

                Client.Character.Dispose()
                Client.Character = Nothing

                Dim r As New PacketClass(OPCODES.SMSG_CHARACTER_LOGIN_FAILED)
                r.AddInt8(AuthLoginCodes.CHAR_LOGIN_NO_WORLD)
                Client.Send(r)
                r.Dispose()
            End If

        Catch ex As Exception
            Log.WriteLine(LogType.FAILED, "[{0:000000}] Unable to login: {1}", Client.Index, ex.ToString)

            Client.Character.Dispose()
            Client.Character = Nothing

            Dim r As New PacketClass(OPCODES.SMSG_CHARACTER_LOGIN_FAILED)
            r.AddInt8(AuthResponseCodes.CHAR_LOGIN_FAILED)
            Client.Send(r)
            r.Dispose()
        End Try

    End Sub
    Public Sub On_CMSG_PLAYER_LOGOUT(ByRef packet As PacketClass, ByRef Client As ClientClass)
        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] CMSG_PLAYER_LOGOUT", Client.IP, Client.Port)

        Client.Character.OnLogout()

        Client.Character.GetWorld.ClientDisconnect(Client.Index)
        Client.Character.Dispose()
        Client.Character = Nothing
    End Sub

    Public Sub On_MSG_MOVE_WORLDPORT_ACK(ByRef packet As PacketClass, ByRef Client As ClientClass)
        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] MSG_MOVE_WORLDPORT_ACK", Client.IP, Client.Port)

        Try
            If Not WS.InstanceCheck(Client, Client.Character.Map) Then Exit Sub

            If Client.Character.IsInWorld Then
                'Inside server transfer
                Client.Character.GetWorld.ClientLogin(Client.Index, Client.Character.GUID)
            Else
                'Inter-server transfer
                Client.Character.ReLoad()

                Client.Character.GetWorld.ClientConnect(Client.Index, Client.GetClientInfo)
                Client.Character.IsInWorld = True
                Client.Character.GetWorld.ClientLogin(Client.Index, Client.Character.GUID)
            End If
        Catch ex As Exception
            Log.WriteLine(LogType.CRITICAL, "{0}", ex.ToString)
        End Try

    End Sub

End Module
