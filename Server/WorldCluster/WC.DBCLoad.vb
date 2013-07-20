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

Imports System
Imports System.IO
Imports mangosVB.Common
Imports mangosVB.Common.BaseWriter

Public Module WS_DBCLoad

#Region "Maps"
    Public Sub InitializeMaps()
        Try
            Dim tmpDBC As DBC.BufferedDBC = New DBC.BufferedDBC("dbc\Map.dbc")

            Dim i As Integer = 0
            For i = 0 To tmpDBC.Rows - 1
                Dim m As New MapInfo
                m.ID = tmpDBC.Item(i, 0, DBC.DBCValueType.DBC_INTEGER)
                m.Type = tmpDBC.Item(i, 2, DBC.DBCValueType.DBC_INTEGER)
                m.Name = tmpDBC.Item(i, 4, DBC.DBCValueType.DBC_STRING)
                m.ParentMap = tmpDBC.Item(i, 117, DBC.DBCValueType.DBC_INTEGER)
                m.ResetTime_Raid = tmpDBC.Item(i, 120, DBC.DBCValueType.DBC_INTEGER)
                m.ResetTime_Heroic = tmpDBC.Item(i, 121, DBC.DBCValueType.DBC_INTEGER)
                m.Expansion = tmpDBC.Item(i, 124, DBC.DBCValueType.DBC_INTEGER)

                Maps.Add(m.ID, m)
            Next i

            tmpDBC.Dispose()
            Log.WriteLine(LogType.INFORMATION, "DBC: {0} Maps initialized.", i)
        Catch e As System.IO.DirectoryNotFoundException
            Console.ForegroundColor = System.ConsoleColor.DarkRed
            Console.WriteLine("DBC File : Maps missing.")
            Console.ForegroundColor = System.ConsoleColor.Gray
        End Try
    End Sub
#End Region
#Region "Chat Channels"
    Public Sub InitializeChatChannels()
        Try
            Dim tmpDBC As DBC.BufferedDBC = New DBC.BufferedDBC("dbc\ChatChannels.dbc")

            Dim i As Integer = 0
            For i = 0 To tmpDBC.Rows - 1
                Dim c As New ChatChannelInfo
                c.Index = tmpDBC.Item(i, 0, DBC.DBCValueType.DBC_INTEGER)
                c.Flags = tmpDBC.Item(i, 1, DBC.DBCValueType.DBC_INTEGER)
                c.Name = tmpDBC.Item(i, 3, DBC.DBCValueType.DBC_STRING)

                ChatChannelsInfo.Add(c.Index, c)
            Next i

            tmpDBC.Dispose()
            Log.WriteLine(LogType.INFORMATION, "DBC: {0} ChatChannels initialized.", i)
        Catch e As System.IO.DirectoryNotFoundException
            Console.ForegroundColor = System.ConsoleColor.DarkRed
            Console.WriteLine("DBC File : ChatChannels missing.")
            Console.ForegroundColor = System.ConsoleColor.Gray
        End Try
    End Sub
#End Region
#Region "Other"
    Public Sub InitializeInternalDatabase()

        InitializeLoadDBCs()

        Try
            'Set all characters offline
            CharacterDatabase.Update("UPDATE characters SET char_online = 0;")

        Catch e As Exception
            Log.WriteLine(LogType.FAILED, "Internal database initialization failed! [{0}]{1}{2}", e.Message, vbNewLine, e.ToString)
        End Try
    End Sub

    Public Sub InitializeLoadDBCs()
        InitializeMaps()
        InitializeChatChannels()
    End Sub

#End Region

End Module