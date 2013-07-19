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
Public Class AscentSpawns
#Region "Constants"
    Public v_id As Integer = 0
    Public v_entry As Integer = 0
    Public v_map As Integer = 0
    Public v_x As Single = 0
    Public v_y As Single = 0
    Public v_z As Single = 0
    Public v_o As Single = 0
    Public v_movetype As Integer = 0
    Public v_displayid As Integer = 0
    Public v_factionid As Integer = 0
    Public v_flags As Integer = 0
    Public v_bytes As Integer = 0
    Public v_bytes2 As Integer = 0
    Public v_emote_state As Integer = 0
    Public v_respawnnpclink As Integer = 0
    Public v_channel_spell As Integer = 0
    Public v_channel_target_sqlid As Integer = 0
    Public v_channel_target_sqlid_creature As Integer = 0
    Public v_standstate As Integer = 0
#End Region

#Region "Properties"
    Public Property ID() As Integer
        Get
            ID = v_id
        End Get
        Set(ByVal Value As Integer)
            v_id = Value
        End Set
    End Property

    Public Property Entry() As Integer
        Get
            Entry = v_entry
        End Get
        Set(ByVal Value As Integer)
            v_entry = Value
        End Set
    End Property

    Public Property Map() As Integer
        Get
            Map = v_map
        End Get
        Set(ByVal Value As Integer)
            v_map = Value
        End Set
    End Property

    Public Property X() As Single
        Get
            X = v_x
        End Get
        Set(ByVal Value As Single)
            v_x = Value
        End Set
    End Property

    Public Property Y() As Single
        Get
            Y = v_y
        End Get
        Set(ByVal Value As Single)
            v_y = Value
        End Set
    End Property

    Public Property Z() As Single
        Get
            Z = v_z
        End Get
        Set(ByVal Value As Single)
            v_z = Value
        End Set
    End Property

    Public Property O() As Single
        Get
            O = v_o
        End Get
        Set(ByVal Value As Single)
            v_o = Value
        End Set
    End Property

    Public Property MoveType() As Integer
        Get
            MoveType = v_movetype
        End Get
        Set(ByVal Value As Integer)
            v_movetype = Value
        End Set
    End Property

    Public Property DisplayID() As Integer
        Get
            DisplayID = v_displayid
        End Get
        Set(ByVal Value As Integer)
            v_displayid = Value
        End Set
    End Property

    Public Property FactionID() As Integer
        Get
            FactionID = v_factionid
        End Get
        Set(ByVal Value As Integer)
            v_factionid = Value
        End Set
    End Property

    Public Property Flags() As Integer
        Get
            Flags = v_flags
        End Get
        Set(ByVal Value As Integer)
            v_flags = Value
        End Set
    End Property

    Public Property Bytes() As Integer
        Get
            Bytes = v_bytes
        End Get
        Set(ByVal Value As Integer)
            v_bytes = Value
        End Set
    End Property

    Public Property Bytes2() As Integer
        Get
            Bytes2 = v_bytes2
        End Get
        Set(ByVal Value As Integer)
            v_bytes2 = Value
        End Set
    End Property

    Public Property Emote_State() As Integer
        Get
            Emote_State = v_emote_state
        End Get
        Set(ByVal Value As Integer)
            v_emote_state = Value
        End Set
    End Property

    Public Property RespawnNpcLink() As Integer
        Get
            RespawnNpcLink = v_respawnnpclink
        End Get
        Set(ByVal Value As Integer)
            v_respawnnpclink = Value
        End Set
    End Property

    Public Property ChannelSpell() As Integer
        Get
            ChannelSpell = v_channel_spell
        End Get
        Set(ByVal Value As Integer)
            v_channel_spell = Value
        End Set
    End Property

    Public Property ChannelTargetSQLID() As Integer
        Get
            ChannelTargetSQLID = v_channel_target_sqlid
        End Get
        Set(ByVal Value As Integer)
            v_channel_target_sqlid = Value
        End Set
    End Property

    Public Property ChannelTargetSQLIDCreature() As Integer
        Get
            ChannelTargetSQLIDCreature = v_channel_target_sqlid_creature
        End Get
        Set(ByVal Value As Integer)
            v_channel_target_sqlid_creature = Value
        End Set
    End Property

    Public Property StandState() As Integer
        Get
            StandState = v_standstate
        End Get
        Set(ByVal Value As Integer)
            v_standstate = Value
        End Set
    End Property

#End Region

End Class
