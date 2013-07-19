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
Public Class SpuriousObjects
#Region "Constants"
    Public v_ID As Integer = 0
    Public v_size As Single = 0
    Public v_model As Integer = 0
    Public v_flags As Integer = 0
    Public v_name As String = " "
    Public v_type As Integer = 0
    Public v_faction As Integer = 0
    Public v_sound0 As Integer = 0
    Public v_sound1 As Integer = 0
    Public v_sound2 As Integer = 0
    Public v_sound3 As Integer = 0
    Public v_sound4 As Integer = 0
    Public v_sound5 As Integer = 0
    Public v_sound6 As Integer = 0
    Public v_sound7 As Integer = 0
    Public v_sound8 As Integer = 0
    Public v_sound9 As Integer = 0
    Public v_loot As Integer = 0
    Public singlequote As String = Chr(34)
    Public doublequote As String = Chr(34) & Chr(34)
#End Region

#Region "Properties"
    Public Property ID() As Integer
        Get
            ID = v_ID
        End Get
        Set(ByVal Value As Integer)
            v_ID = Value
        End Set
    End Property

    Public Property Size() As Single
        Get
            Size = v_size
        End Get
        Set(ByVal Value As Single)
            v_size = Value
        End Set
    End Property

    Public Property Model() As Integer
        Get
            Model = v_model
        End Get
        Set(ByVal Value As Integer)
            v_model = Value
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

    Public Property Name() As String
        Get
            Name = v_name
        End Get
        Set(ByVal Value As String)
            v_name = Value
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

    Public Property Faction() As Integer
        Get
            Faction = v_faction
        End Get
        Set(ByVal Value As Integer)
            v_faction = Value
        End Set
    End Property

    Public Property Sound0() As Integer
        Get
            Sound0 = v_sound0
        End Get
        Set(ByVal Value As Integer)
            v_sound0 = Value
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

    Public Property Loot() As Integer
        Get
            Loot = v_loot
        End Get
        Set(ByVal Value As Integer)
            v_loot = Value
        End Set
    End Property

#End Region

End Class
