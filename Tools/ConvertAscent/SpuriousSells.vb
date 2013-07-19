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
Public Class SpuriousSells
#Region "Constants"
    Public v_sell_id As Integer = 0
    Public v_sell_item As Integer = 0
    Public v_sell_count As Integer = 0
    Public v_sell_group As Single = 0
#End Region

#Region "Properties"
    Public Property Sell_Id() As Integer
        Get
            Sell_Id = v_sell_id
        End Get
        Set(ByVal Value As Integer)
            v_sell_id = Value
        End Set
    End Property

    Public Property Sell_Item() As Integer
        Get
            Sell_Item = v_sell_item
        End Get
        Set(ByVal Value As Integer)
            v_sell_item = Value
        End Set
    End Property

    Public Property Sell_Count() As Integer
        Get
            Sell_Count = v_sell_count
        End Get
        Set(ByVal Value As Integer)
            v_sell_count = Value
        End Set
    End Property

    Public Property Sell_Group() As Single
        Get
            Sell_Group = v_sell_group
        End Get
        Set(ByVal Value As Single)
            v_sell_group = Value
        End Set
    End Property
#End Region

End Class
