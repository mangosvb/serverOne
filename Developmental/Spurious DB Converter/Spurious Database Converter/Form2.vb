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
Imports MySql.Data.MySqlClient
Imports mangosVB.Common
Public Class Form2

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bCancle.Click
        End
    End Sub

    Public Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bOK.Click
        'Base Database
        Dim BaseDatabase As New SQL
        Dim SpuriousDatabase As New SQL
        BaseDatabase.SQLTypeServer = SQL.DB_Type.MySQL
        BaseDatabase.SQLHost = txtHost.Text
        BaseDatabase.SQLPort = txtPort.Text
        BaseDatabase.SQLUser = txtUsername.Text
        BaseDatabase.SQLPass = txtPassword.Text
        BaseDatabase.SQLDBName = txtDatabase.Text
        BaseDatabase.Connect()

        'Spurious Database
        SpuriousDatabase.SQLTypeServer = SQL.DB_Type.MySQL
        SpuriousDatabase.SQLHost = txtHost.Text
        SpuriousDatabase.SQLPort = txtPort.Text
        SpuriousDatabase.SQLUser = txtUsername.Text
        SpuriousDatabase.SQLPass = txtPassword.Text
        SpuriousDatabase.SQLDBName = txtDatabase2.Text
        SpuriousDatabase.Connect()
        Me.Hide()
        Form1.Show()
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        MsgBox("You must have the Spurious structure downloaded. You may download it here: SITEHERE ")
    End Sub
End Class