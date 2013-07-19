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
Imports MySql.Data.MySqlClient
Imports mangosVB.Common

Public Class NCDBgameobjects
#Region "Variables"
    'gameobject_root table.
    Dim q_entry As Integer = 0
    Dim q_Type As Integer = 0
    Dim q_displayid As Integer = 0
    Dim q_Name As String = " "
    Dim q_Field0 As Integer = 0
    Dim q_Field1 As Integer = 0
    Dim q_Field2 As Integer = 0
    Dim q_Field3 As Integer = 0
    Dim q_Field4 As Integer = 0
    Dim q_Field5 As Integer = 0
    Dim q_Field6 As Integer = 0
    Dim q_Field7 As Integer = 0
    Dim q_Field8 As Integer = 0
    Dim q_Field9 As Integer = 0
    Dim q_Field10 As Integer = 0
    Dim q_Field11 As Integer = 0
    Dim q_Field12 As Integer = 0
    Dim q_Field13 As Integer = 0
    Dim q_Field14 As Integer = 0
    Dim q_Field15 As Integer = 0
    Dim q_Field16 As Integer = 0
    Dim q_Field17 As Integer = 0
    Dim q_Field18 As Integer = 0
    Dim q_Field19 As Integer = 0
    Dim q_Field20 As Integer = 0
    Dim q_Field21 As Integer = 0
    Dim q_Field22 As Integer = 0
    Dim q_Field23 As Integer = 0

    'gameobject_addons table.
    Dim qs_entry As Integer = 0
    Dim qs_faction As Integer = 0
    Dim qs_flags As Integer = 0
    Dim qs_scale As Integer = 0
    Dim qs_respawntimesecs As Integer = 0
    Dim qs_castbarcaption As Integer = 0
    Dim qs_animprogress As Integer = 0
#End Region
#Region "Properties"
    'Properties for NCDB table gameobject_root
    Public Property entry() As Integer
        Get
            entry = q_entry
        End Get
        Set(ByVal Value As Integer)
            q_entry = Value
        End Set
    End Property
    Public Property Type() As Integer
        Get
            Type = q_Type
        End Get
        Set(ByVal Value As Integer)
            q_Type = Value
        End Set
    End Property
    Public Property displayid() As Integer
        Get
            displayid = q_displayid
        End Get
        Set(ByVal Value As Integer)
            q_displayid = Value
        End Set
    End Property
    Public Property Name() As String
        Get
            Name = q_Name
        End Get
        Set(ByVal Value As String)
            q_Name = Value
        End Set
    End Property
    Public Property Field0() As Integer
        Get
            Field0 = q_Field0
        End Get
        Set(ByVal Value As Integer)
            q_Field0 = Value
        End Set
    End Property
    Public Property Field1() As Integer
        Get
            Field1 = q_Field1
        End Get
        Set(ByVal Value As Integer)
            q_Field1 = Value
        End Set
    End Property
    Public Property Field2() As Integer
        Get
            Field2 = q_Field2
        End Get
        Set(ByVal Value As Integer)
            q_Field2 = Value
        End Set
    End Property
    Public Property Field3() As Integer
        Get
            Field3 = q_Field3
        End Get
        Set(ByVal Value As Integer)
            q_Field3 = Value
        End Set
    End Property
    Public Property Field4() As Integer
        Get
            Field4 = q_Field4
        End Get
        Set(ByVal Value As Integer)
            q_Field4 = Value
        End Set
    End Property
    Public Property Field5() As Integer
        Get
            Field5 = q_Field5
        End Get
        Set(ByVal Value As Integer)
            q_Field5 = Value
        End Set
    End Property
    Public Property Field6() As Integer
        Get
            Field6 = q_Field6
        End Get
        Set(ByVal Value As Integer)
            q_Field6 = Value
        End Set
    End Property
    Public Property Field7() As Integer
        Get
            Field7 = q_Field7
        End Get
        Set(ByVal Value As Integer)
            q_Field7 = Value
        End Set
    End Property
    Public Property Field8() As Integer
        Get
            Field8 = q_Field8
        End Get
        Set(ByVal Value As Integer)
            q_Field8 = Value
        End Set
    End Property
    Public Property Field9() As Integer
        Get
            Field9 = q_Field9
        End Get
        Set(ByVal Value As Integer)
            q_Field9 = Value
        End Set
    End Property
    Public Property Field10() As Integer
        Get
            Field10 = q_Field10
        End Get
        Set(ByVal Value As Integer)
            q_Field10 = Value
        End Set
    End Property
    Public Property Field11() As Integer
        Get
            Field11 = q_Field11
        End Get
        Set(ByVal Value As Integer)
            q_Field11 = Value
        End Set
    End Property
    Public Property Field12() As Integer
        Get
            Field12 = q_Field12
        End Get
        Set(ByVal Value As Integer)
            q_Field12 = Value
        End Set
    End Property
    Public Property Field13() As Integer
        Get
            Field13 = q_Field13
        End Get
        Set(ByVal Value As Integer)
            q_Field13 = Value
        End Set
    End Property
    Public Property Field14() As Integer
        Get
            Field14 = q_Field14
        End Get
        Set(ByVal Value As Integer)
            q_Field14 = Value
        End Set
    End Property
    Public Property Field15() As Integer
        Get
            Field15 = q_Field15
        End Get
        Set(ByVal Value As Integer)
            q_Field15 = Value
        End Set
    End Property
    Public Property Field16() As Integer
        Get
            Field16 = q_Field16
        End Get
        Set(ByVal Value As Integer)
            q_Field16 = Value
        End Set
    End Property
    Public Property Field17() As Integer
        Get
            Field17 = q_Field17
        End Get
        Set(ByVal Value As Integer)
            q_Field17 = Value
        End Set
    End Property
    Public Property Field18() As Integer
        Get
            Field18 = q_Field18
        End Get
        Set(ByVal Value As Integer)
            q_Field18 = Value
        End Set
    End Property
    Public Property Field19() As Integer
        Get
            Field19 = q_Field19
        End Get
        Set(ByVal Value As Integer)
            q_Field19 = Value
        End Set
    End Property
    Public Property Field20() As Integer
        Get
            Field20 = q_Field20
        End Get
        Set(ByVal Value As Integer)
            q_Field20 = Value
        End Set
    End Property
    Public Property Field21() As Integer
        Get
            Field21 = q_Field21
        End Get
        Set(ByVal Value As Integer)
            q_Field21 = Value
        End Set
    End Property
    Public Property Field22() As Integer
        Get
            Field22 = q_Field22
        End Get
        Set(ByVal Value As Integer)
            q_Field22 = Value
        End Set
    End Property
    Public Property Field23() As Integer
        Get
            Field23 = q_Field23
        End Get
        Set(ByVal Value As Integer)
            q_Field20 = Value
        End Set
    End Property
    'End of gameobject_root table properties
    'Start of gameobject_addons table properties
    'Public Property entry() As Integer
    ' Get
    '  entry = qs_entry
    ' End Get
    ' Set(ByVal Value As Integer)
    '     q_entry = Value
    ' End Set
    'End Property
    Public Property faction() As Integer
        Get
            faction = qs_faction
        End Get
        Set(ByVal Value As Integer)
            qs_faction = Value
        End Set
    End Property
    Public Property flags() As Integer
        Get
            flags = qs_flags
        End Get
        Set(ByVal Value As Integer)
            qs_flags = Value
        End Set
    End Property
    Public Property scale() As Integer
        Get
            scale = qs_scale
        End Get
        Set(ByVal Value As Integer)
            qs_scale = Value
        End Set
    End Property
    Public Property respawntimesecs() As Integer
        Get
            respawntimesecs = qs_respawntimesecs
        End Get
        Set(ByVal Value As Integer)
            qs_respawntimesecs = Value
        End Set
    End Property
    Public Property castbarcaption() As Integer
        Get
            castbarcaption = qs_castbarcaption
        End Get
        Set(ByVal Value As Integer)
            qs_castbarcaption = Value
        End Set
    End Property
    Public Property animprogress() As Integer
        Get
            animprogress = qs_animprogress
        End Get
        Set(ByVal Value As Integer)
            qs_animprogress = Value
        End Set
    End Property
    'End of NCDB gameobject_addons table properties
#End Region
End Class
Public Class Spuriousgameobjects
#Region "Variables"
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
Public Class ConvertNCDBgameobjects
End Class