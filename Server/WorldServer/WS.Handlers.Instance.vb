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
Imports System.Collections.Generic
Imports mangosVB.Common.BaseWriter

Public Module WS_Handlers_Instance

    Public Sub InstanceMapUpdate()
        'TODO: Here we should also respawn bosses
        Dim q As New DataTable
        Dim t As Integer = GetTimestamp(Now)
        CharacterDatabase.Query(String.Format("SELECT * FROM characters_instances WHERE expire < {0};", t), q)

        For Each r As DataRow In q.Rows
            If Maps.ContainsKey(r.Item("map")) Then InstanceMapExpire(r.Item("map"), r.Item("instance"))
        Next

        CharacterDatabase.Update(String.Format("DELETE FROM characters_instances WHERE expire < {0};", t))
    End Sub
    Public Function InstanceMapCreate(ByVal Map As UInteger) As UInteger
        Dim q As New DataTable

        'TODO: Spawn the instance

        'TODO: Save instance IDs in MAP class, using current way it may happen 2 groups to be in same instance
        CharacterDatabase.Query(String.Format("SELECT MAX(instance) FROM characters_instances WHERE map = {0};", Map), q)
        If q.Rows(0).Item(0) IsNot DBNull.Value Then
            Return CInt(q.Rows(0).Item(0)) + 1
        Else
            Return 0
        End If
    End Function
    Public Sub InstanceMapExpire(ByVal Map As UInteger, ByVal Instance As UInteger)
    'TODO: Respawn the instance if there are players
    'TODO: Delete the instance if there are no players
    End Sub

    Public Sub InstanceMapEnter(ByVal c As CharacterObject)
        If Maps(c.MapID).Type = MapTypes.MAP_COMMON Then
            c.instance = 0

            #If DEBUG Then
            c.SystemMessage(SetColor("You are not in instance.", 0, 0, 255))
            #End If
        Else
            'DONE: Instances expire check
            InstanceMapUpdate()

            Dim q As New DataTable

            'DONE: Check if player is already in instance
            CharacterDatabase.Query(String.Format("SELECT * FROM characters_instances WHERE char_guid = {0} AND map = {1};", c.GUID, c.MapID), q)
            If q.Rows.Count > 0 Then
                'Character is saved to instance
                c.instance = q.Rows(0).Item("instance")
                #If DEBUG Then
                c.SystemMessage(SetColor(String.Format("You are in instance #{0}, map {1}", c.instance, c.MapID), 0, 0, 255))
                #End If
                SendInstanceMessage(c.Client, c.MapID, q.Rows(0).Item("expire") - GetTimestamp(Now))
                Exit Sub
            End If

            'DONE: Check if group is already in instance
            If c.IsInGroup Then
                CharacterDatabase.Query(String.Format("SELECT * FROM characters_instances_group WHERE group_id = {0} AND map = {1};", c.Group.ID, c.MapID), q)

                If q.Rows.Count > 0 Then
                    'Group is saved to instance
                    c.instance = q.Rows(0).Item("instance")
                    #If DEBUG Then
                    c.SystemMessage(SetColor(String.Format("You are in instance #{0}, map {1}", c.instance, c.MapID), 0, 0, 255))
                    #End If
                    SendInstanceMessage(c.Client, c.MapID, q.Rows(0).Item("expire") - GetTimestamp(Now))
                    Exit Sub
                End If
            End If

            'DONE Create new instance
            Dim instanceNewID As Integer = InstanceMapCreate(c.MapID)
            Dim instanceNewResetTime As Integer = GetTimestamp(Now) + Maps(c.MapID).ResetTime(False)

            'Set instance
            c.instance = instanceNewID

            'Set instance in database
            CharacterDatabase.Update(String.Format("INSERT INTO characters_instances (char_guid, map, instance, expire) VALUES ({0}, {1}, {2}, {3});", c.GUID, c.MapID, instanceNewID, instanceNewResetTime))

            If c.IsInGroup Then
                'Set group in the same instance
                CharacterDatabase.Update(String.Format("INSERT INTO characters_instances_group (group_id, map, instance, expire) VALUES ({0}, {1}, {2}, {3});", c.Group.ID, c.MapID, instanceNewID, instanceNewResetTime))
            End If

            #If DEBUG Then
            c.SystemMessage(SetColor(String.Format("You are in instance #{0}, map {1}", c.instance, c.MapID), 0, 0, 255))
            #End If
            SendInstanceMessage(c.Client, c.MapID, GetTimestamp(Now) - instanceNewResetTime)
        End If
    End Sub
    Public Sub InstanceMapLeave(ByVal c As CharacterObject)
    'TODO: Start teleport timer
    End Sub

    'SMSG_INSTANCE_DIFFICULTY

    Public Enum ResetFailedReason As UInteger
        INSTANCE_RESET_FAILED_ZONING = 0
        INSTANCE_RESET_FAILED_OFFLINE = 1
        INSTANCE_RESET_FAILED = 2
        INSTANCE_RESET_SUCCESS = 3
    End Enum
    Public Sub SendResetInstanceSuccess(ByRef Client As ClientClass, ByVal Map As UInteger)
        Dim p As New PacketClass(OPCODES.SMSG_INSTANCE_RESET)
        p.AddUInt32(Map)
        Client.Send(p)
        p.Dispose()
    End Sub
    Public Sub SendResetInstanceFailed(ByRef Client As ClientClass, ByVal Map As UInteger, ByVal Reason As ResetFailedReason)
        Dim p As New PacketClass(OPCODES.SMSG_INSTANCE_RESET)
        p.AddUInt32(Reason)
        p.AddUInt32(Map)
        Client.Send(p)
        p.Dispose()
    End Sub
    Public Sub SendResetInstanceFailedNotify(ByRef Client As ClientClass, ByVal Map As UInteger)
        Dim p As New PacketClass(OPCODES.SMSG_RESET_FAILED_NOTIFY)
        p.AddUInt32(Map)
        Client.Send(p)
        p.Dispose()
    End Sub

    Private Sub SendUpdateInstanceOwnership(ByRef Client As ClientClass, ByVal Saved As UInteger)
        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] SMSG_UPDATE_INSTANCE_OWNERSHIP", Client.IP, Client.Port)

        Dim p As New PacketClass(OPCODES.SMSG_UPDATE_INSTANCE_OWNERSHIP)
        p.AddUInt32(Saved)                  'True/False if have been saved
        Client.Send(p)
        p.Dispose()
    End Sub
    Private Sub SendUpdateLastInstance(ByRef Client As ClientClass, ByVal Map As UInteger)
        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] SMSG_UPDATE_LAST_INSTANCE", Client.IP, Client.Port)

        Dim p As New PacketClass(OPCODES.SMSG_UPDATE_LAST_INSTANCE)
        p.AddUInt32(Map)
        Client.Send(p)
        p.Dispose()
    End Sub
    Public Sub SendInstanceSaved(ByVal Character As CharacterObject)
        Dim q As New DataTable
        CharacterDatabase.Query(String.Format("SELECT * FROM characters_instances WHERE char_guid = {0};", Character.GUID), q)

        SendUpdateInstanceOwnership(Character.Client, q.Rows.Count > 0)

        For Each r As DataRow In q.Rows
            SendUpdateLastInstance(Character.Client, r.Item("map"))
        Next
    End Sub

    Private Enum RaidInstanceMessage As UInteger
        RAID_INSTANCE_WARNING_HOURS = 1         ' WARNING! %s is scheduled to reset in %d hour(s).
        RAID_INSTANCE_WARNING_MIN = 2           ' WARNING! %s is scheduled to reset in %d minute(s)!
        RAID_INSTANCE_WARNING_MIN_SOON = 3      ' WARNING! %s is scheduled to reset in %d minute(s). Please exit the zone or you will be returned to your bind location!
        RAID_INSTANCE_WELCOME = 4               ' Welcome to %s. This raid instance is scheduled to reset in %s.
    End Enum
    Public Sub SendInstanceMessage(ByRef Client As ClientClass, ByVal Map As UInteger, ByVal Time As Integer)
        Dim Type As RaidInstanceMessage

        If Time < 0 Then
            Type = RaidInstanceMessage.RAID_INSTANCE_WELCOME
            Time = -Time
        ElseIf Time > 60 AndAlso Time < 3600 Then
            Type = RaidInstanceMessage.RAID_INSTANCE_WARNING_MIN
        ElseIf Time > 3600 Then
            Type = RaidInstanceMessage.RAID_INSTANCE_WARNING_HOURS
        ElseIf Time < 60 Then
            Type = RaidInstanceMessage.RAID_INSTANCE_WARNING_MIN_SOON
        End If

        Dim p As New PacketClass(OPCODES.SMSG_RAID_INSTANCE_MESSAGE)
        p.AddUInt32(Type)
        p.AddUInt32(Map)
        p.AddUInt32(Time)
        Client.Send(p)
        p.Dispose()
    End Sub

    Public Sub On_CMSG_RESET_INSTANCES(ByRef packet As PacketClass, ByRef Client As ClientClass)
        Log.WriteLine(LogType.DEBUG, "[{0}:{1}] CMSG_RESET_INSTANCES", Client.IP, Client.Port)

        If Client.Character.IsInGroup Then
            If Client.Character.IsGroupLeader Then
                SendResetInstanceFailed(Client, Client.Character.MapID, ResetFailedReason.INSTANCE_RESET_FAILED)
            End If
        Else
            SendResetInstanceFailed(Client, Client.Character.MapID, ResetFailedReason.INSTANCE_RESET_FAILED)
        End If

    End Sub

End Module