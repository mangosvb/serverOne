'
' Copyright (C) 2013 - 2017 getMaNGOS <http://www.getmangos.eu>
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
Option Strict On
Option Explicit On

Imports System.IO

Friend Class dlgSettings
    Inherits System.Windows.Forms.Form
    Private Sub Command1_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command1.Click
        MsgBox("The world server host is where users will connect after successfully logging in. The 'world server host' textbox can be an IP (i.e. 127.0.0.1) or a DNS (i.e. Server.SpuriousEmu.com).", MsgBoxStyle.Information)
    End Sub
    Private Sub Command10_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command10.Click
        If Command10.Text = "S" Then
            If MsgBox("Do you want to show the SQL password?", CType(MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, MsgBoxStyle), "MaNGOSVB Control Center") = MsgBoxResult.Yes Then
                Text8.PasswordChar = CChar("")
                Command10.Text = "H"
            End If
        ElseIf Command10.Text = "H" Then
            If MsgBox("Do you want to hide the SQL password?", CType(MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, MsgBoxStyle), "MaNGOSVB Control Center") = MsgBoxResult.Yes Then
                Text8.PasswordChar = CChar("*")
                Command10.Text = "S"
            End If
        End If
    End Sub
    Private Sub Command12_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command12.Click
        MsgBox("The SQL database that holds all server information (characters, accounts, etc...). Default value is MaNGOSVB.", MsgBoxStyle.Information)
    End Sub
    Private Sub Command13_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command13.Click
        MsgBox("The SQL database that holds all server information (characters, accounts, etc...). Default value is MaNGOSVB.", MsgBoxStyle.Information)
    End Sub
    Private Sub Command15_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
        If Command15.Text = "S" Then
            If MsgBox("Do you want to show the SQL password?", CType(MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, MsgBoxStyle), "MaNGOSVB Control Center") = MsgBoxResult.Yes Then
                Text13.PasswordChar = CChar("")
                Command15.Text = "H"
            End If
        ElseIf Command15.Text = "H" Then
            If MsgBox("Do you want to hide the SQL password?", CType(MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, MsgBoxStyle), "MaNGOSVB Control Center") = MsgBoxResult.Yes Then
                Text13.PasswordChar = CChar("*")
                Command15.Text = "S"
            End If
        End If
    End Sub
    Private Sub Command16_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command16.Click
        MsgBox("The SQL username that is required to access your SQL server. Default value is root.", MsgBoxStyle.Information)
    End Sub
    Private Sub Command17_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command17.Click
        MsgBox("The SQL password that is required to access your SQL server.", MsgBoxStyle.Information)
    End Sub
    Private Sub Command18_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command18.Click
        MsgBox("The port number that the sql server will listen on. Default port is 3306.", MsgBoxStyle.Information)
    End Sub
    Private Sub Command19_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command19.Click
        MsgBox("The sql host is where the server will connect to the database. The 'sql host' textbox can be an IP (i.e. 127.0.0.1) or a DNS (i.e. database.yourserver.org).", MsgBoxStyle.Information)
    End Sub
    Private Sub Command2_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command2.Click
        MsgBox("The port number that the world server will listen on to accept connections. The Default port is 8085.", MsgBoxStyle.Information)
    End Sub
    Private Sub Command20_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command20.Click
        Main.Show()
    End Sub
    Private Sub Command21_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command21.Click
        MsgBox("The 'xp rate' textbox designates how much experience a user will gain after killing a creature, and completing a quest. Default value is 1.0.", MsgBoxStyle.Information)
    End Sub
    Private Sub Command22_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command22.Click
        Me.Close()
    End Sub
    Private Sub Command23_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command23.Click
        MsgBox("The port number that the realm server will listen on to accept connections. Default port is 3125.", MsgBoxStyle.Information)
    End Sub
    Private Sub Command24_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command24.Click
        MsgBox("The realm server host is where users will attempt to connect before entering the world server. The 'realm server host' textbox can be an IP (i.e. 127.0.0.1) or a DNS (i.e. realmserver.yourserver.org).", MsgBoxStyle.Information)
    End Sub
    Private Sub Command3_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command3.Click
        MsgBox("The 'player limit' textbox allows the server administrator to designate how many players can log in to the world server at a designated time. Default value is 100.", MsgBoxStyle.Information)
    End Sub
    Private Sub Command4_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command4.Click
        MsgBox("The 'health regeneration rate' textbox allows the administrator to specify how fast health regenerates. Default value is 1.0.", MsgBoxStyle.Information)
    End Sub
    Private Sub Command5_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command5.Click
        MsgBox("The 'mana regeneration rate' textbox allows the administrator to specify how fast mana regenerates. Default value is 1.0.", MsgBoxStyle.Information)
    End Sub
    Private Sub Command6_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command6.Click
        MsgBox("The sql host is where the server will connect to the database. The 'sql host' textbox can be an IP (i.e. 127.0.0.1) or a DNS (i.e. database.mangosVB.org).", MsgBoxStyle.Information)
    End Sub
    Private Sub Command7_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command7.Click
        MsgBox("The port number that the sql server will listen on. Default port is 3306.", MsgBoxStyle.Information)
    End Sub
    Private Sub Command8_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command8.Click
        MsgBox("The SQL password that is required to access your SQL server.", MsgBoxStyle.Information)
    End Sub
    Private Sub Command9_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command9.Click
        MsgBox("The SQL username that is required to access your SQL server. Default value is root.", MsgBoxStyle.Information)
    End Sub
End Class