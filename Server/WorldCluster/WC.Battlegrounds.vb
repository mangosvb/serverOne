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

Public Module WC_Battlegrounds

#Region "WC.Battlegrounds.Constants"

#End Region
#Region "WC.Battlegrounds.Handlers"
    Public Sub On_CMSG_BATTLEMASTER_JOIN(ByRef packet As PacketClass, ByRef Client As ClientClass)
        'TODO: Join battleground
    End Sub

    Public Sub On_CMSG_BATTLEMASTER_JOIN_ARENA(ByRef packet As PacketClass, ByRef Client As ClientClass)
        If (packet.Data.Length - 1) < 16 Then Exit Sub
        packet.GetInt16()
        Dim GUID As ULong = packet.GetUInt64
        Dim Type As Byte = packet.GetInt8
        Dim AsGroup As Byte = packet.GetInt8
        Dim IsRated As Byte = packet.GetInt8

        'NOTE: There is no way to check if the NPC exist and is a battlemaster since this is located in the cluser.

        'TODO: Check if we are already in the queue
        'TODO: Ignore if we are already have too many queue's (max = 3)

        Dim ArenaType As Byte = 0
        Select Case Type
            Case 0
                ArenaType = 2
            Case 1
                ArenaType = 3
            Case 2
                ArenaType = 5
            Case Else
                Exit Sub
        End Select

        'TODO: Check if in arena team for rated game

        If AsGroup AndAlso Client.Character.IsInGroup = False Then Exit Sub

        'TODO: Save entry position

        Dim response As New PacketClass(OPCODES.SMSG_BATTLEFIELD_STATUS)
        response.AddInt32(0) 'SlotID
        response.AddUInt64(CULng(ArenaType) Or (CULng(&HD) << CULng(8)) Or (CULng(6) << CULng(16)) Or (CULng(&H1F90) << CULng(48)))
        response.AddInt32(0) 'Unk
        response.AddInt8(IsRated) '1 = Rated / 0 = Unrated
        response.AddInt32(1) '1 = Wait queue
        response.AddUInt32(10000) 'Average wait time
        response.AddUInt32(0) 'Time in queue
        Client.Send(response)
        response.Dispose()
    End Sub
#End Region
#Region "WC.Battlegrounds.Functions"

#End Region

End Module