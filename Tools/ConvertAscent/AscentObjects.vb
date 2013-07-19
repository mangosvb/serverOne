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
Public Class AscentObjects
#Region "Constants"
    Public v_entry As Integer = 0
    Public v_type As Integer = 0
    Public v_displayid As Integer = 0
    Public v_name As String = " "
    Public v_spellfocus As Integer = 0
    Public v_sound1 As Integer = 0
    Public v_sound2 As Integer = 0
    Public v_sound3 As Integer = 0
    Public v_sound4 As Integer = 0
    Public v_sound5 As Integer = 0
    Public v_sound6 As Integer = 0
    Public v_sound7 As Integer = 0
    Public v_sound8 As Integer = 0
    Public v_sound9 As Integer = 0
    Public v_unknown1 As Integer = 0
    Public v_unknown2 As Integer = 0
    Public v_unknown3 As Integer = 0
    Public v_unknown4 As Integer = 0
    Public v_unknown5 As Integer = 0
    Public v_unknown6 As Integer = 0
    Public v_unknown7 As Integer = 0
    Public v_unknown8 As Integer = 0
    Public v_unknown9 As Integer = 0
    Public v_unknown10 As Integer = 0
    Public v_unknown11 As Integer = 0
    Public v_unknown12 As Integer = 0
    Public v_unknown13 As Integer = 0
    Public v_unknown14 As Integer = 0
    Public as_map As Integer = 0
    Public as_position_x As Single = 0
    Public as_position_y As Single = 0
    Public as_position_z As Single = 0
    Public as_facing As Single = 0
    Public as_orientation1 As Single = 0
    Public as_orientation2 As Single = 0
    Public as_orientation3 As Single = 0
    Public as_orientation4 As Single = 0
    Public as_state As Integer = 0
    Public as_flags As Integer = 0
    Public as_faction As Integer = 0
    Public as_scale As Single = 0
    Public as_statenpclink As Integer = 0
    Public singlequote As String = Chr(34)
    Public doublequote As String = Chr(34) & Chr(34)
#End Region

#Region "Properties"
    Public Property Entry() As Integer
        Get
            Entry = v_entry
        End Get
        Set(ByVal Value As Integer)
            v_entry = Value
        End Set
    End Property

    Public Property Type() As Integer
        Get
            Type = v_type
        End Get
        Set(ByVal Value As Integer)
            v_type = Value
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

    Public Property Name() As String
        Get
            Name = v_name
        End Get
        Set(ByVal Value As String)
            v_name = Value
        End Set
    End Property

    Public Property SpellFocus() As Integer
        Get
            SpellFocus = v_spellfocus
        End Get
        Set(ByVal Value As Integer)
            v_spellfocus = Value
        End Set
    End Property

    Public Property Sound1() As Integer
        Get
            Sound1 = v_sound1
        End Get
        Set(ByVal Value As Integer)
            v_sound1 = Value
        End Set
    End Property

    Public Property Sound2() As Integer
        Get
            Sound2 = v_sound2
        End Get
        Set(ByVal Value As Integer)
            v_sound2 = Value
        End Set
    End Property

    Public Property Sound3() As Integer
        Get
            Sound3 = v_sound3
        End Get
        Set(ByVal Value As Integer)
            v_sound3 = Value
        End Set
    End Property

    Public Property Sound4() As Integer
        Get
            Sound4 = v_sound4
        End Get
        Set(ByVal Value As Integer)
            v_sound4 = Value
        End Set
    End Property

    Public Property Sound5() As Integer
        Get
            Sound5 = v_sound5
        End Get
        Set(ByVal Value As Integer)
            v_sound5 = Value
        End Set
    End Property

    Public Property Sound6() As Integer
        Get
            Sound6 = v_sound6
        End Get
        Set(ByVal Value As Integer)
            v_sound6 = Value
        End Set
    End Property

    Public Property Sound7() As Integer
        Get
            Sound7 = v_sound7
        End Get
        Set(ByVal Value As Integer)
            v_sound7 = Value
        End Set
    End Property

    Public Property Sound8() As Integer
        Get
            Sound8 = v_sound8
        End Get
        Set(ByVal Value As Integer)
            v_sound8 = Value
        End Set
    End Property

    Public Property Sound9() As Integer
        Get
            Sound9 = v_sound9
        End Get
        Set(ByVal Value As Integer)
            v_sound9 = Value
        End Set
    End Property

    Public Property Unknown1() As Integer
        Get
            Unknown1 = v_unknown1
        End Get
        Set(ByVal Value As Integer)
            v_unknown1 = Value
        End Set
    End Property

    Public Property Unknown2() As Integer
        Get
            Unknown2 = v_unknown2
        End Get
        Set(ByVal Value As Integer)
            v_unknown2 = Value
        End Set
    End Property

    Public Property Unknown3() As Integer
        Get
            Unknown3 = v_unknown3
        End Get
        Set(ByVal Value As Integer)
            v_unknown3 = Value
        End Set
    End Property

    Public Property Unknown4() As Integer
        Get
            Unknown4 = v_unknown4
        End Get
        Set(ByVal Value As Integer)
            v_unknown4 = Value
        End Set
    End Property

    Public Property Unknown5() As Integer
        Get
            Unknown5 = v_unknown5
        End Get
        Set(ByVal Value As Integer)
            v_unknown5 = Value
        End Set
    End Property

    Public Property Unknown6() As Integer
        Get
            Unknown6 = v_unknown6
        End Get
        Set(ByVal Value As Integer)
            v_unknown6 = Value
        End Set
    End Property

    Public Property Unknown7() As Integer
        Get
            Unknown7 = v_unknown7
        End Get
        Set(ByVal Value As Integer)
            v_unknown7 = Value
        End Set
    End Property

    Public Property Unknown8() As Integer
        Get
            Unknown8 = v_unknown8
        End Get
        Set(ByVal Value As Integer)
            v_unknown8 = Value
        End Set
    End Property

    Public Property Unknown9() As Integer
        Get
            Unknown9 = v_unknown9
        End Get
        Set(ByVal Value As Integer)
            v_unknown9 = Value
        End Set
    End Property

    Public Property Unknown10() As Integer
        Get
            Unknown10 = v_unknown10
        End Get
        Set(ByVal Value As Integer)
            v_unknown10 = Value
        End Set
    End Property

    Public Property Unknown11() As Integer
        Get
            Unknown11 = v_unknown11
        End Get
        Set(ByVal Value As Integer)
            v_unknown11 = Value
        End Set
    End Property

    Public Property Unknown12() As Integer
        Get
            Unknown12 = v_unknown12
        End Get
        Set(ByVal Value As Integer)
            v_unknown12 = Value
        End Set
    End Property

    Public Property Unknown13() As Integer
        Get
            Unknown13 = v_unknown13
        End Get
        Set(ByVal Value As Integer)
            v_unknown13 = Value
        End Set
    End Property

    Public Property Unknown14() As Integer
        Get
            Unknown14 = v_unknown14
        End Get
        Set(ByVal Value As Integer)
            v_unknown14 = Value
        End Set
    End Property

    Public Property ASpawn_Map() As Integer
        Get
            ASpawn_Map = as_map
        End Get
        Set(ByVal Value As Integer)
            as_map = Value
        End Set
    End Property

    Public Property ASpawn_X() As Single
        Get
            ASpawn_X = as_position_x
        End Get
        Set(ByVal Value As Single)
            as_position_x = Value
        End Set
    End Property

    Public Property ASpawn_Y() As Single
        Get
            ASpawn_Y = as_position_y
        End Get
        Set(ByVal Value As Single)
            as_position_y = Value
        End Set
    End Property

    Public Property ASpawn_Z() As Single
        Get
            ASpawn_Z = as_position_z
        End Get
        Set(ByVal Value As Single)
            as_position_z = Value
        End Set
    End Property

    Public Property ASpawn_Facing() As Single
        Get
            ASpawn_Facing = as_facing
        End Get
        Set(ByVal Value As Single)
            as_facing = Value
        End Set
    End Property

    Public Property ASpawn_O1() As Single
        Get
            ASpawn_O1 = as_orientation1
        End Get
        Set(ByVal Value As Single)
            as_orientation1 = Value
        End Set
    End Property

    Public Property ASpawn_O2() As Single
        Get
            ASpawn_O2 = as_orientation2
        End Get
        Set(ByVal Value As Single)
            as_orientation2 = Value
        End Set
    End Property

    Public Property ASpawn_O3() As Single
        Get
            ASpawn_O3 = as_orientation3
        End Get
        Set(ByVal Value As Single)
            as_orientation3 = Value
        End Set
    End Property

    Public Property ASpawn_O4() As Single
        Get
            ASpawn_O4 = as_orientation4
        End Get
        Set(ByVal Value As Single)
            as_orientation4 = Value
        End Set
    End Property

    Public Property ASpawn_State() As Integer
        Get
            ASpawn_State = as_state
        End Get
        Set(ByVal Value As Integer)
            as_state = Value
        End Set
    End Property

    Public Property ASpawn_Flags() As Integer
        Get
            ASpawn_Flags = as_flags
        End Get
        Set(ByVal Value As Integer)
            as_flags = Value
        End Set
    End Property

    Public Property ASpawn_Faction() As Integer
        Get
            ASpawn_Faction = as_faction
        End Get
        Set(ByVal Value As Integer)
            as_faction = Value
        End Set
    End Property

    Public Property ASpawn_Scale() As Single
        Get
            ASpawn_Scale = as_scale
        End Get
        Set(ByVal Value As Single)
            as_scale = Value
        End Set
    End Property

    Public Property ASpawn_StateNpcLink() As Integer
        Get
            ASpawn_StateNpcLink = as_statenpclink
        End Get
        Set(ByVal Value As Integer)
            as_statenpclink = Value
        End Set
    End Property

#End Region

End Class
