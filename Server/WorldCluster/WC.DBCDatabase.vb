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
Imports System.IO
Imports System.Runtime.InteropServices
Imports mangosVB.Common.BaseWriter
Imports mangosVB.Common

Public Module WS_DBCDatabase

#Region "Maps"

    Public Maps As New Dictionary(Of Integer, MapInfo)
    Public Class MapInfo
        Public ID As Integer
        Public Type As MapTypes = MapTypes.MAP_COMMON
        Public Name As String = ""
        Public ParentMap As Integer = -1
        Public ResetTime_Raid As Integer = 0
        Public ResetTime_Heroic As Integer = 0
        Public Expansion As ExpansionLevel = 0

        Public ReadOnly Property IsDungeon() As Boolean
            Get
                Return Type = MapTypes.MAP_INSTANCE OrElse Type = MapTypes.MAP_RAID
            End Get
        End Property
        Public ReadOnly Property IsRaid() As Boolean
            Get
                Return Type = MapTypes.MAP_RAID
            End Get
        End Property
        Public ReadOnly Property IsBattleGround() As Boolean
            Get
                Return Type = MapTypes.MAP_BATTLEGROUND
            End Get
        End Property
        Public ReadOnly Property IsBattleArena() As Boolean
            Get
                Return Type = MapTypes.MAP_ARENA
            End Get
        End Property
        Public ReadOnly Property SupportsHeroicMode() As Boolean
            Get
                Return (ResetTime_Heroic <> 0)
            End Get
        End Property
        Public ReadOnly Property HasResetTime() As Boolean
            Get
                Return (ResetTime_Heroic <> 0 OrElse ResetTime_Raid <> 0)
            End Get
        End Property

    End Class

#End Region
#Region "Chat Channels"

    <Flags()> _
    Public Enum ChatChannelsFlags
        FLAG_NONE = &H0
        FLAG_INITIAL = &H1              ' General, Trade, LocalDefense, LFG
        FLAG_ZONE_DEP = &H2             ' General, Trade, LocalDefense, GuildRecruitment
        FLAG_GLOBAL = &H4               ' WorldDefense
        FLAG_TRADE = &H8                ' Trade
        FLAG_CITY_ONLY = &H10           ' Trade, GuildRecruitment
        FLAG_CITY_ONLY2 = &H20          ' Trade, GuildRecruitment
        FLAG_DEFENSE = &H10000          ' LocalDefense, WorldDefense
        FLAG_GUILD_REQ = &H20000        ' GuildRecruitment
        FLAG_LFG = &H40000              ' LookingForGroup
    End Enum
    Public ChatChannelsInfo As New Dictionary(Of Integer, ChatChannelInfo)
    Public Class ChatChannelInfo
        Public Index As Integer
        Public Flags As Integer
        Public Name As String
    End Class

#End Region

End Module