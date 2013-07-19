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
Public Class AscentVendors

#Region "Constants"
    Public v_entry As Integer = 0
    Public v_item As Integer = 0
    Public v_amount As Integer = 0
    Public v_max_amount As Integer = 0
    Public v_inctime As Integer = 0

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

    Public Property Item() As Integer
        Get
            Item = v_item
        End Get
        Set(ByVal Value As Integer)
            v_item = Value
        End Set
    End Property

    Public Property Amount() As Integer
        Get
            Amount = v_amount
        End Get
        Set(ByVal Value As Integer)
            v_amount = Value
        End Set
    End Property

    Public Property Max_Amount() As Integer
        Get
            Max_Amount = v_max_amount
        End Get
        Set(ByVal Value As Integer)
            v_max_amount = Value
        End Set
    End Property

    Public Property Inctime() As Integer
        Get
            Inctime = v_inctime
        End Get
        Set(ByVal Value As Integer)
            v_inctime = Value
        End Set
    End Property

#End Region

End Class