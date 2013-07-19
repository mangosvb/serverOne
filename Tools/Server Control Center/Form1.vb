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
Option Strict On
Option Explicit On
Friend Class Main
    Inherits System.Windows.Forms.Form
    Private Sub RealmTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RealmTimer.Tick
        If Me.RealmProcess.HasExited Then
            Me.RealmProcess.Start()
            TextBox2.Text = CStr(CDbl(TextBox2.Text) + 1)
        End If
    End Sub
    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            Me.RealmTimer.Enabled = True
        ElseIf CheckBox1.Checked = False Then
            Me.RealmTimer.Enabled = False
        End If
    End Sub
    Private Sub WorldTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WorldTimer.Tick
        If Me.WorldProcess.HasExited Then
            Me.WorldProcess.Start()
            TextBox3.Text = CStr(CDbl(TextBox3.Text) + 1)
        End If
    End Sub
    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = True Then
            Me.WorldTimer.Enabled = True
        ElseIf CheckBox2.Checked = False Then
            Me.WorldTimer.Enabled = False
        End If
    End Sub
    Private Sub SystemTrayIcon_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles SystemTrayIcon.MouseDoubleClick
        MyBase.Show()
        Me.SystemTrayIcon.Visible = False
    End Sub

    Private Sub ConfigureCongifFilesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConfigureCongifFilesToolStripMenuItem.Click
        dlgSettings.Show()
    End Sub

    Private Sub MinimizeToTrayToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MinimizeToTrayToolStripMenuItem.Click
        MyBase.Hide()
        Me.SystemTrayIcon.Visible = True
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Me.Button1.Text = "Start mangosVB.RealmServer" Then
            Me.RealmProcess.Start()
            Me.CheckBox1.Enabled = True
            Me.Button1.Text = "Stop mangosVB.RealmServer"
        ElseIf Me.Button1.Text = "Stop mangosVB.RealmServer" Then
            Me.RealmProcess.Kill()
            If Me.CheckBox1.Checked = True Then
                Me.Button1.Text = "Stop mangosVB.RealmServer"
            ElseIf Me.CheckBox1.Checked = False Then
                Me.CheckBox1.Enabled = False
                Me.Button1.Text = "Start mangosVB.RealmServer"
            End If
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If Me.Button2.Text = "Start mangosVB.WorldServer" Then
            Me.WorldProcess.Start()
            Me.CheckBox2.Enabled = True
            Me.Button2.Text = "Stop mangosVB.WorldServer"
        ElseIf Me.Button2.Text = "Stop mangosVB.WorldServer" Then
            Me.WorldProcess.Kill()
            If Me.CheckBox2.Checked = True Then
                Me.Button2.Text = "Stop mangosVB.WorldServer"
            ElseIf Me.CheckBox2.Checked = False Then
                Me.CheckBox2.Enabled = False
                Me.Button2.Text = "Start mangosVB.WorldServer"
            End If
        End If
    End Sub
End Class