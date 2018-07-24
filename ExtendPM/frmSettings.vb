Imports System.Data.SQLite

Public Class frmSettings
    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        Using conn As New SQLiteConnection(gconnstrSQLITE)
            Try
                Dim updateQuery As String = "UPDATE Settings SET EmailLogin = @EmailLogin, EmailDisplayName = @EmailDisplayName, 
EmailPassword = @EmailPassword, SMTPHost = @SMTPHost, SMTPPort = @SMTPPort, EmailTestSendTo = @EmailTestSendTo"
                Dim cmd As New SQLiteCommand(updateQuery, conn)
                cmd.Parameters.AddWithValue("@EmailLogin", txtEmailLogin.Text)
                cmd.Parameters.AddWithValue("@EmailDisplayName", txtEmailDisplay.Text)
                cmd.Parameters.AddWithValue("@EmailPassword", Encrypt(txtEmailPassword.Text))
                cmd.Parameters.AddWithValue("@SMTPHost", txtSMTPHost.Text)
                cmd.Parameters.AddWithValue("@SMTPPort", txtSMTPPort.Text)
                cmd.Parameters.AddWithValue("@EmailTestSendTo", txtTestSendTo.Text)

                conn.Open()
                cmd.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message)

            End Try

        End Using

    End Sub

    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Using conn As New SQLiteConnection(gconnstrSQLITE)
            Try
                Dim selectQuery As String = "SELECT EmailLogin, EmailDisplayName, EmailPassword, SMTPHost, SMTPPort, EmailTestSendTo FROM Settings"
                Dim cmd As New SQLiteCommand(selectQuery, conn)
                conn.Open()
                Dim dr As SQLiteDataReader = cmd.ExecuteReader
                'Loop through the rows in the datareader
                While dr.Read()
                    gFromEmail = dr.Item("EmailLogin")
                    gFromDisplayName = dr.Item("EmailDisplayName")
                    gEmailPassword = Decrypt(dr.Item("EmailPassword"))
                    gSMTPhost = dr.Item("SMTPHost")
                    gSMTPport = dr.Item("SMTPPort")
                    gEmailTestSendTo = dr.Item("EmailTestSendTo")

                End While
            Catch ex As SQLiteException

            End Try
        End Using

        txtEmailDisplay.Text = gFromDisplayName
        txtEmailLogin.Text = gFromEmail
        txtEmailPassword.Text = gEmailPassword
        txtSMTPHost.Text = gSMTPhost
        txtSMTPPort.Text = gSMTPport
        txtTestSendTo.Text = gEmailTestSendTo

    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        Using conn As New SQLiteConnection(gconnstrSQLITE)
            Try
                Dim selectQuery As String = "SELECT EmailLogin, EmailDisplayName, EmailPassword, SMTPHost, SMTPPort, EmailTestSendTo FROM Settings"
                Dim cmd As New SQLiteCommand(selectQuery, conn)
                conn.Open()
                Dim dr As SQLiteDataReader = cmd.ExecuteReader
                'Loop through the rows in the datareader
                While dr.Read()
                    gFromEmail = dr.Item("EmailLogin")
                    gFromDisplayName = dr.Item("EmailDisplayName")
                    gEmailPassword = Decrypt(dr.Item("EmailPassword"))
                    gSMTPhost = dr.Item("SMTPHost")
                    gSMTPport = dr.Item("SMTPPort")
                    gEmailTestSendTo = dr.Item("EmailTestSendTo")

                End While
            Catch ex As SQLiteException

            End Try
        End Using
    End Sub
End Class