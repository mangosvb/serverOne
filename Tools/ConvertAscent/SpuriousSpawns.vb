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
Public Class SpuriousSpawns
#Region "Constants"
    Public v_id As Integer = 0
    Public v_entry As Integer = 0
    Public v_time As Integer = 0
    Public v_positionx As Single = 0
    Public v_positiony As Single = 0
    Public v_positionz As Single = 0
    Public v_orientation As Single = 0
    Public v_spawned As Integer = 0
    Public v_range As Integer = 0
    Public v_type As Integer = 0
    Public v_map As Integer = 0
    Public v_left As Integer = 0
    Public v_waypoints As Integer = 0
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

    Public Property Time() As Integer
        Get
            Time = v_time
        End Get
        Set(ByVal Value As Integer)
            v_time = Value
        End Set
    End Property

    Public Property PositionX() As Single
        Get
            PositionX = v_positionx
        End Get
        Set(ByVal Value As Single)
            v_positionx = Value
        End Set
    End Property

    Public Property PositionY() As Single
        Get
            PositionY = v_positiony
        End Get
        Set(ByVal Value As Single)
            v_positiony = Value
        End Set
    End Property

    Public Property PositionZ() As Single
        Get
            PositionZ = v_positionz
        End Get
        Set(ByVal Value As Single)
            v_positionz = Value
        End Set
    End Property

    Public Property Orientation() As Single
        Get
            Orientation = v_orientation
        End Get
        Set(ByVal Value As Single)
            v_orientation = Value
        End Set
    End Property

    Public Property Spawned() As Integer
        Get
            Spawned = v_spawned
        End Get
        Set(ByVal Value As Integer)
            v_spawned = Value
        End Set
    End Property

    Public Property Range() As Integer
        Get
            Range = v_range
        End Get
        Set(ByVal Value As Integer)
            v_range = Value
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

    Public Property Map() As Integer
        Get
            Map = v_map
        End Get
        Set(ByVal Value As Integer)
            v_map = Value
        End Set
    End Property

    Public Property Left() As Integer
        Get
            Left = v_left
        End Get
        Set(ByVal Value As Integer)
            v_left = Value
        End Set
    End Property

    Public Property WayPoints() As Integer
        Get
            WayPoints = v_waypoints
        End Get
        Set(ByVal Value As Integer)
            v_waypoints = Value
        End Set
    End Property

#End Region

End Class
