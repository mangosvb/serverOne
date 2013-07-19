Imports mangosVB.Common
Imports System.IO

Public Class Form1
    Public SpuriousDB As New SQL
    Public SpuriousRevision As Integer = 0
    Public Unsuccessfull As Integer = 0
    Public TotalLines As Integer = 0

    Public Sub SLQEventHandler(ByVal MessageID As SQL.EMessages, ByVal OutBuf As String)
        Select Case MessageID
            Case SQL.EMessages.ID_Error
                MsgBox(OutBuf, MsgBoxStyle.Critical, "Error in Query")
                Unsuccessfull += 1
            Case SQL.EMessages.ID_Message
                MsgBox(OutBuf, MsgBoxStyle.Critical, "MySQL Message")
        End Select
    End Sub

    Private Sub cmdConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConnect.Click
        SpuriousDB.SQLTypeServer = SQL.DB_Type.MySQL
        SpuriousDB.SQLHost = txtHost.Text
        SpuriousDB.SQLPort = txtPort.Text
        SpuriousDB.SQLUser = txtUser.Text
        SpuriousDB.SQLPass = txtPass.Text
        SpuriousDB.SQLDBName = txtDatabase.Text
        SpuriousDB.Connect()
        SpuriousDB.Update("SET SESSION sql_mode='STRICT_ALL_TABLES';")

        AddHandler SpuriousDB.SQLMessage, AddressOf SLQEventHandler

        Dim MysqlQuery As New DataTable
        SpuriousDB.Query("SELECT db_version, db_name, spurious_revision FROM db_info LIMIT 1", MysqlQuery)
        If MysqlQuery.Rows.Count > 0 Then
            lblVersion.Text = MysqlQuery.Rows(0).Item("db_version")
            lblName.Text = MysqlQuery.Rows(0).Item("db_name")
            SpuriousRevision = MysqlQuery.Rows(0).Item("spurious_revision")
            lblRevision.Text = SpuriousRevision
            cmdUpdate.Enabled = True
        Else
            cmdUpdate.Enabled = False
            lblVersion.Text = ""
            lblName.Text = ""
            lblRevision.Text = ""
            SpuriousRevision = 0
            MsgBox("Missing database info." & vbNewLine & "Please do a full database installation instead.", MsgBoxStyle.Exclamation, "DB Error")
        End If
    End Sub

    Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
        Dim FileRev As Integer = 0, NumFiles As Integer = 0
        Unsuccessfull = 0
        If SpuriousRevision > 0 Then
            Dim sFileList As String = Dir(CurDir() & "\Development\updates\", FileAttribute.Normal + FileAttribute.Archive + FileAttribute.ReadOnly)
            If sFileList <> "" Then
                Do While sFileList <> ""
                    If InStr(sFileList, ".sql", CompareMethod.Text) > 0 Then 'Only get sql files
                        If IsNumeric(Replace(sFileList, ".sql", "")) Then
                            FileRev = CInt(Replace(sFileList, ".sql", ""))
                            If FileRev > SpuriousRevision Then
                                'Execute the SQL file
                                Dim fs As FileStream = New FileStream(CurDir() & "\Development\updates\" & sFileList, FileMode.Open, FileAccess.Read)
                                Dim d As New StreamReader(fs)
                                d.BaseStream.Seek(0, SeekOrigin.Begin)
                                Dim CurLine As String = ""
                                While d.Peek() > -1
                                    CurLine &= d.ReadLine()
                                    If InStr(CurLine, ";") > 0 Then
                                        SpuriousDB.Update(CurLine)
                                        TotalLines += 1
                                        CurLine = ""
                                    End If
                                End While
                                If CurLine.Length > 0 Then
                                    SpuriousDB.Update(CurLine)
                                    TotalLines += 1
                                End If
                                d.Close()

                                NumFiles += 1
                            End If
                        End If
                    End If
                    sFileList = Dir()
                Loop
            Else
                MsgBox("No update files could be found, is this program located in the correct directory?", MsgBoxStyle.Exclamation, "Not found")
                Exit Sub
            End If
            If NumFiles > 0 Then
                If Unsuccessfull > 0 Then
                    MsgBox(String.Format("Database was partly updated to revision {0}, {2} error messages.{1} files were used with a total of {3} queries.", FileRev, vbNewLine & NumFiles, Unsuccessfull, TotalLines), MsgBoxStyle.Information, "Warning")
                Else
                    SpuriousRevision = FileRev
                    SpuriousDB.Update(String.Format("UPDATE db_info SET spurious_revision = {0}", SpuriousRevision))
                    MsgBox(String.Format("Database was successfully updated to revision {0}.{1} files were used with a total of {2} queries.", SpuriousRevision, vbNewLine & NumFiles, TotalLines), MsgBoxStyle.Information, "Success")
                End If
            Else
                MsgBox("You're already using the last revision.", MsgBoxStyle.Information, "Success")
            End If
        End If
    End Sub
End Class