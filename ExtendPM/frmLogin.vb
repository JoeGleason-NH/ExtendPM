Imports System.Data.SQLite
Imports System.Net
Imports System.Net.Mail
Imports System.IO
Imports System.Threading

Public Class frmLogin
    Private Sub CmdLogin_Click(sender As Object, e As EventArgs) Handles cmdLogin.Click
        'User has to enter the correct username and password to successfully login.
        'If no user/password match is found try again.
        Dim strTestPassword As String = "None"

        Using conn As New SQLiteConnection(gconnstrSQLITE)
            Dim selectQuery As String = "SELECT UserName, Password, SecurityToken FROM Users WHERE UserName=@User"
            Dim cmd As New SQLiteCommand(selectQuery, conn)
            cmd.Parameters.AddWithValue("@User", txtUserName.Text)
            conn.Open()
            Dim dr As SQLiteDataReader = cmd.ExecuteReader
            'Loop through the rows in the datareader
            While dr.Read()
                strTestPassword = Decrypt(dr.Item("Password"))
                gUserName = dr.Item("UserName")
                gPassword = Decrypt(dr.Item("Password"))
                gSecurityToken = Decrypt(dr.Item("SecurityToken"))
            End While

            If strTestPassword = txtPassword.Text Then
                frmMain.Show()
            Else
                gUserName = Nothing
                gPassword = Nothing
                gSecurityToken = Nothing
                MsgBox("Password incorrect, try again.")
            End If

        End Using

    End Sub

    Private Sub CmdAddUser_Click(sender As Object, e As EventArgs) Handles cmdAddUser.Click
        'Anyone can supply credentials and be added to the SQLite database.  Password and security token are encrypted and stored.
        Using conn As New SQLiteConnection(gconnstrSQLITE)
            Dim insertQuery As String = "INSERT INTO Users(UserName,Password, SecurityToken) VALUES(@User,@Pwd, @SecToken)"
            Dim cmd As New SQLiteCommand(insertQuery, conn)
            cmd.Parameters.AddWithValue("@User", txtUserName.Text)
            cmd.Parameters.AddWithValue("@Pwd", Encrypt(txtPassword.Text))
            cmd.Parameters.AddWithValue("@SecToken", Encrypt(txtSecurityToken.Text))
            conn.Open()
            cmd.ExecuteNonQuery()
            MsgBox(String.Format("{0} has been added", txtUserName.Text))

        End Using

    End Sub

    Private Sub CmdDeleteUser_Click(sender As Object, e As EventArgs) Handles cmdDeleteUser.Click
        'Delete a user from the SQLite database. Only the username is required.
        Using conn As New SQLiteConnection(gconnstrSQLITE)
            Dim deleteQuery As String = "DELETE FROM Users WHERE UserName =@User"
            Dim cmd As New SQLiteCommand(deleteQuery, conn)
            cmd.Parameters.AddWithValue("@User", txtUserName.Text)
            conn.Open()
            cmd.ExecuteNonQuery()
            MsgBox(String.Format("{0} has been Deleted", txtUserName.Text))
        End Using
    End Sub

    Private Sub cmdUpdateUser_Click(sender As Object, e As EventArgs) Handles cmdUpdateUser.Click

    End Sub

    Private Sub cmdConfig_Click(sender As Object, e As EventArgs) Handles cmdConfig.Click

        frmSettings.Show()

    End Sub

    Private Sub InsertUsers()

        'SQLITE - INSERT a record sample code

        Using conn As New SQLiteConnection(gconnstrSQLITE)
            Dim insertQuery As String = "INSERT INTO Users(UserName,Password, SecurityToken) VALUES(@User,@Pwd, @SecToken)"
            Dim cmd As New SQLiteCommand(insertQuery, conn)
            cmd.Parameters.AddWithValue("@User", txtUserName.Text)
            cmd.Parameters.AddWithValue("@Pwd", Encrypt(txtPassword.Text))
            cmd.Parameters.AddWithValue("@SecToken", Encrypt(txtSecurityToken.Text))
            conn.Open()
            cmd.ExecuteNonQuery()
            MsgBox(String.Format("{0} has been added", txtUserName.Text))

        End Using
    End Sub

    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Startup of app this form loads
        'Set initial values first
        gconnstrSQLITE = "Data Source=" & Application.StartupPath & "\LocalDB\sfdb.db;Version=3" 'The SQLite database file location

        'GUI Display or CLI? 
        If My.Application.CommandLineArgs.Count = 1 Then
            Call CLILogin(My.Application.CommandLineArgs(0))
        Else
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
                    WriteLogFileEntry("ERROR:CLILogin-Settings", ex.Message)
                End Try
            End Using

            Me.Show()
        End If

    End Sub
    Private Sub CLILogin(ByVal UName As String)

        Using conn As New SQLiteConnection(gconnstrSQLITE)
            Try
                Dim selectQuery As String = "SELECT distinct UserName, Password, SecurityToken FROM Users WHERE UserName=@User"
                Dim cmd As New SQLiteCommand(selectQuery, conn)
                cmd.Parameters.AddWithValue("@User", UName)
                conn.Open()
                Dim dr As SQLiteDataReader = cmd.ExecuteReader
                'Loop through the rows in the datareader
                While dr.Read()
                    gUserName = dr.Item("UserName")
                    gPassword = Decrypt(dr.Item("Password"))
                    gSecurityToken = Decrypt(dr.Item("SecurityToken"))
                End While
            Catch ex As SQLiteException
                WriteLogFileEntry("ERROR:CLILogin", ex.Message)
            End Try
        End Using

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
                WriteLogFileEntry("ERROR:CLILogin-Settings", ex.Message)
            End Try
        End Using

        Call GetPreShowEmailsAndProcess() 'PreShow Email
        Application.Exit()

    End Sub

    Private Sub GetPreShowEmailsAndProcess()
        'This routine looks up the jobs ready to be sent from the EmailShows table in a local SQLite database.  
        'It can loop through multiple jobs And will write a log entry for every job And email it sends.
        'Date Created: 3/28/2017
        'Date Modified: 07/01/2018
        'Author: Joe Gleason, jgleason@ccanh.com
        Dim SQL As String = Nothing
        Dim intCount As Integer = 0
        Dim intX As Integer = 0
        Dim intY As Integer = 0
        Dim intU As Int16 = 0
        'Get the current time to build the criteria for data retrieval
        Dim strNow As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
        Dim strSubject As String

        '# 1 - Retrieve the number of jobs to process in this run. We will use it to dimension an array 
        SQL = "SELECT count(objid) from EmailJobs WHERE senddate < '" & strNow & "' and sendstatus = 'Ready'"
        Using conn As New SQLiteConnection(gconnstrSQLITE)
            conn.Open()
            Using cmd As New SQLiteCommand(SQL, conn)
                Try
                    intU = Convert.ToInt16(cmd.ExecuteScalar())
                Catch ex As SQLiteException
                    WriteLogFileEntry("ERROR:GetPreShowEmailsAndProcess_1", ex.Message)
                End Try
            End Using
        End Using

        'If no rows are returned there is nothing to process. Exit the sub
        If intU = 0 Then
            Exit Sub
        End If

        '# 2 - Finding something to process next we retrieve the base information for the job
        'Multi-dimensional array to store the job data.  Job ID in row, fields in columns ex. (0,0) = first objectid, first field
        Dim strarrJobData(intU, 31) As String
        Using conn As New SQLiteConnection(gconnstrSQLITE)
            Try
                'Set up a SQL statement to get the data for the preshow email
                SQL = "Select objID, EventInstanceId, SendDate, SendStatus,EventTitle, ShowDate, ShowTime, "
                SQL = SQL & "LobbyTime, RunTime, Venue, VenueAddress, EventURL, NumSent, Block1, Block2, Block3, "
                SQL = SQL & "Block4, Block5, Block6, F1Title, F1Date, F1URL, F1Img, "
                SQL = SQL & "F2Title, F2Date, F2URL, F2Img, F3Title, F3Date, F3URL, F3Img, TemplateName from EmailJobs"
                SQL = SQL & " WHERE senddate < '" & strNow & "' and sendstatus = 'Ready'"

                Dim cmd As New SQLiteCommand(SQL, conn)
                conn.Open()
                Dim dr As SQLiteDataReader = cmd.ExecuteReader

                If dr.HasRows Then
                    intX = 0
                    Do While dr.Read()
                        For intY = 0 To 31
                            strarrJobData(intX, intY) = Convert.ToString(dr(intY))
                        Next intY
                        intX = intX + 1
                    Loop
                End If
            Catch ex As SQLiteException
                WriteLogFileEntry("ERROR:GetPreShowEmailsAndProcess_2", ex.Message)
            End Try
        End Using

        '# 3 - Now that we have an array we can loop through it and retrieve the specific email address for each job, prepping a preshow email,
        'and finally, send the email one at a time to the recipients.

        intU = intX - 1
        For intX = 0 To intU 'Loop through the array, the first dimension is the event instance id used to retrieve all ticketholders
            'First lets get the email list from Salesforce and put it in a SQLite table
            Call InsertEmailList(strarrJobData(intX, 1))

            'Second - now that the email list is populated into the preshowlist table we can retrieve the set from the defined view
            SQL = "Select EventInstanceID, FirstName, LastName, Email, OrderName "
            SQL = SQL & " From v_PreShowList "
            SQL = SQL & " Where EventInstanceID = '" & strarrJobData(intX, 1) & "' "
            'SQL = SQL & " WHERE EventInstanceID = 'a0F3600000IXQOtEAP'"
            SQL = SQL & " ORDER BY Email "

            Using conn As New SQLiteConnection(gconnstrSQLITE)
                Try
                    Dim cmd As New SQLiteCommand(SQL, conn)
                    conn.Open()
                    Dim dr As SQLiteDataReader = cmd.ExecuteReader

                    If dr.HasRows Then
                        'Set the status to "Sent" for the current objectid
                        'UpdatePreStatus(strarrJobData(intX, 0), intCount)

                        'Set the Subject line
                        strSubject = "Information on your tickets for " & strarrJobData(intX, 4)
                        'Send a copy to admin staff (settings EmailTestSendTo
                        PopulateSendPreShowEmail(gEmailTestSendTo, strSubject, Application.StartupPath & "\Templates\" & strarrJobData(intX, 31),
                            "EmailTestSendTo", strarrJobData(intX, 4), strarrJobData(intX, 11), strarrJobData(intX, 7), strarrJobData(intX, 6),
                             strarrJobData(intX, 5), strarrJobData(intX, 8), strarrJobData(intX, 9), strarrJobData(intX, 10), strarrJobData(intX, 19), strarrJobData(intX, 20), strarrJobData(intX, 21),
                            strarrJobData(intX, 23), strarrJobData(intX, 24), strarrJobData(intX, 25), strarrJobData(intX, 27), strarrJobData(intX, 28), strarrJobData(intX, 29),
                            strarrJobData(intX, 22), strarrJobData(intX, 26), strarrJobData(intX, 30), strarrJobData(intX, 13), strarrJobData(intX, 14), strarrJobData(intX, 15),
                            strarrJobData(intX, 16), strarrJobData(intX, 17), strarrJobData(intX, 18), "TEST")

                        Call WriteLogFileEntry("GetPreShowEmailsAndProcess", "NONE - " & "CCAList - " & strarrJobData(intX, 4) & " - " & strarrJobData(intX, 5))

                        Do While dr.Read()

                            'Loop through the email list and process each one.  Pause as needed to avoid bulk send limits
                            intCount = intCount + 1

                            'Uncomment function to send emails to the list - LEAVE commented for testing
                            '  PopulateSendPreShowEmail(Convert.ToString(dr("Email")), strSubject, Application.StartupPath & "\Templates\" & strarrJobData(intX, 31),
                            '  Convert.ToString(dr("FirstName")), strarrJobData(intX, 4), strarrJobData(intX, 11), strarrJobData(intX, 7), strarrJobData(intX, 6),
                            '  strarrJobData(intX, 5), strarrJobData(intX, 8), strarrJobData(intX, 9), strarrJobData(intX, 10), strarrJobData(intX, 19), strarrJobData(intX, 20), strarrJobData(intX, 21),
                            '  strarrJobData(intX, 23), strarrJobData(intX, 24), strarrJobData(intX, 25), strarrJobData(intX, 27), strarrJobData(intX, 28), strarrJobData(intX, 29),
                            '  strarrJobData(intX, 22), strarrJobData(intX, 26), strarrJobData(intX, 30), strarrJobData(intX, 13), strarrJobData(intX, 14), strarrJobData(intX, 15),
                            '  strarrJobData(intX, 16), strarrJobData(intX, 17), strarrJobData(intX, 18), Convert.ToString(dr("OrderName")))
                            'Thread.Sleep(1250) 'Wait 1.25 seconds then process the next email (max rate of 60 per minute so shoot for 55 per minute)

                            Call WriteLogFileEntry("GetPreShowEmailsAndProcess", Convert.ToString(dr("OrderName")) & " - " & Convert.ToString(dr("Email")))

                        Loop
                        'UPDATE the Number SENT after the last email has processed
                        UpdatePreStatus(strarrJobData(intX, 0), intCount - 1)
                    End If
                Catch ex As SQLiteException
                    WriteLogFileEntry("ERROR: GetPreShowEmailsAndProcess_3", ex.Message)
                End Try
            End Using
        Next intX
    End Sub

    Private Sub WriteLogFileEntry(strSource As String, strLogMessage As String)
        Dim filePath As String = String.Format(Path.GetDirectoryName(Application.ExecutablePath) + "\Logs\{0}.txt", DateTime.Today.ToString("yyyy-MM-dd"))
        Using writer As New StreamWriter(filePath, True)
            If File.Exists(filePath) Then
                writer.WriteLine(DateTime.Now & " - " & strSource & " - " & strLogMessage)
            Else
                writer.WriteLine(DateTime.Now & " - " & strSource & " - " & strLogMessage)
            End If
        End Using
    End Sub

    Private Sub PopulateSendPreShowEmail(ByVal strTo As String, ByVal strSubject As String,
      ByVal strTemplate As String, ByVal strFirstName As String, ByVal strEventTitle As String, ByVal strEventURL As String,
      ByVal strLobbyTime As String, ByVal strShowTime As String, ByVal strShowDate As String, ByVal strRunTime As String,
      ByVal strVenue As String, ByVal strVenueAddress As String, ByVal strF1Title As String, ByVal strF1Date As String, ByVal strF1URL As String,
      ByVal strF2Title As String, ByVal strF2Date As String, ByVal strF2URL As String,
      ByVal strF3Title As String, ByVal strF3Date As String, ByVal strF3URL As String,
      ByVal strF1Img As String, ByVal strF2Img As String, ByVal strF3Img As String, ByVal strShowDetails As String,
      ByVal strFoodDrinks As String, ByVal strRemember As String, ByVal strMoreInfo As String, ByVal strPlanTrip As String,
      ByVal strRestaurants As String, ByVal strOrderName As String)

        Try
            'Create the mail message
            Dim mail As New MailMessage()
            'set the addresses
            mail.From = New MailAddress(gFromEmail, gFromDisplayName)

            'Deal with email addresses
            'Split the TO email address strings into separate address items
            'There should only be one address unless it is the test message from settings table
            Dim list As IList(Of String) = New List(Of String)(strTo.Split(New String() {";"}, StringSplitOptions.None))
            'Loop through the list and check each matching item in the event list
            For Each strid As String In list
                If Len(strid) > 1 Then
                    mail.To.Add(strid)
                End If
            Next

            'set the subject line
            mail.Subject = strSubject
            Dim strBody As String = Nothing
            'Read the html file
            Dim reader As StreamReader = My.Computer.FileSystem.OpenTextFileReader(strTemplate)
            'Read the entire html file into a string
            strBody = reader.ReadToEnd
            'Replace the dynamic placeholders, basically a mail merge
            strBody = strBody.Replace("{FirstName}", strFirstName)
            strBody = strBody.Replace("{EventTitle}", strEventTitle)
            strBody = strBody.Replace("{EventURL}", strEventURL)
            strBody = strBody.Replace("{LobbyTime}", strLobbyTime)
            strBody = strBody.Replace("{ShowTime}", strShowTime)
            strBody = strBody.Replace("{ShowDate}", strShowDate)
            strBody = strBody.Replace("{RunTime}", strRunTime)
            strBody = strBody.Replace("{Venue}", strVenue)
            strBody = strBody.Replace("{VenueAddress}", strVenueAddress)
            strBody = strBody.Replace("{OrderName}", strOrderName)
            strBody = strBody.Replace("{Block1}", strShowDetails)
            strBody = strBody.Replace("{Block2}", strFoodDrinks)
            strBody = strBody.Replace("{Block3}", strRemember)
            strBody = strBody.Replace("{Block4}", strMoreInfo)
            strBody = strBody.Replace("{Block5}", strPlanTrip)
            strBody = strBody.Replace("{Block6}", strRestaurants)
            strBody = strBody.Replace("{F1Title}", strF1Title)
            strBody = strBody.Replace("{F1Date}", strF1Date)
            strBody = strBody.Replace("{F1URL}", strF1URL)
            strBody = strBody.Replace("{F1Img}", strF1Img)
            strBody = strBody.Replace("{F2Title}", strF2Title)
            strBody = strBody.Replace("{F2Date}", strF2Date)
            strBody = strBody.Replace("{F2URL}", strF2URL)
            strBody = strBody.Replace("{F2Img}", strF2Img)
            strBody = strBody.Replace("{F3Title}", strF3Title)
            strBody = strBody.Replace("{F3Date}", strF3Date)
            strBody = strBody.Replace("{F3URL}", strF3URL)
            strBody = strBody.Replace("{F3Img}", strF3Img)

            'create the view and pass in the dynamic values to replace the variables
            Dim htmlView As AlternateView = AlternateView.CreateAlternateViewFromString(strBody, Nothing, "Text/Html")

            mail.AlternateViews.Add(htmlView)
            mail.IsBodyHtml = True

            'send the message
            Dim smtp As New SmtpClient()

            smtp.Host = gSMTPhost 'Gmail=smtp.gmail.com; Yahoo=smtp.mail.yahoo.com, Hotmail=smtp.live.com
            smtp.UseDefaultCredentials = False
            smtp.Credentials = New System.Net.NetworkCredential(gFromEmail, gEmailPassword)
            smtp.Port = CInt(gSMTPport)
            smtp.EnableSsl = True

            smtp.Send(mail)

        Catch ex As Exception
            Call WriteLogFileEntry("PopulateSendPreShowEmail", strTo & " - " & ex.Message)
        End Try

    End Sub
    Private Sub UpdatePreStatus(ByVal strObjID As String, ByVal intX As Integer)

        Using conn As New SQLiteConnection(gconnstrSQLITE)
            Dim updateQuery As String = "UPDATE EmailJobs SET SendStatus = @SetStatus, NumSent = @NumSent WHERE ObjId = @ObjID"
            Dim cmd As New SQLiteCommand(updateQuery, conn)
            cmd.Parameters.AddWithValue("@ObjID", strObjID)
            cmd.Parameters.AddWithValue("@SetStatus", "Sent")
            cmd.Parameters.AddWithValue("@NumSent", intX)
            conn.Open()
            cmd.ExecuteNonQuery()
        End Using

    End Sub

    Private Sub InsertEmailList(ByVal EvtInstanceID As String)

        'Remove any existing rows that match the event instance id
        Using conn As New SQLiteConnection(gconnstrSQLITE)
            Dim deleteQuery As String = "DELETE FROM PreShowList WHERE EventInstanceID =@EvtID"
            Dim cmd As New SQLiteCommand(deleteQuery, conn)
            cmd.Parameters.AddWithValue("@EvtID", EvtInstanceID)
            conn.Open()
            cmd.ExecuteNonQuery()
        End Using

        'Retrieve the ticketholders list from PatronManager and loop through it, inserting into SQLite table
        If gSessionID Is Nothing Or gSessionID = "" Then
            LoginSF(gUserName, gPassword, gSecurityToken)
        End If

        Try
            ' Store SessionId in SessionHeader; We will need while making query() call
            Dim sHeader As ApexApi.SessionHeader = New ApexApi.SessionHeader
            sHeader.sessionId = gSessionID

            Dim sSQL = "Select event_instance_id__c,PatronTicket__TicketOrder__r.PatronTicket__FirstName__c"
            sSQL = sSQL & " ,PatronTicket__TicketOrder__r.PatronTicket__LastName__c,PatronTicket__TicketOrder__r.PatronTicket__Email__c"
            sSQL = sSQL & " ,PatronTicket__TicketOrder__r.name From PatronTicket__TicketOrderItem__c"
            sSQL = sSQL & " Where event_instance_id__c = '" & EvtInstanceID & "'"
            sSQL = sSQL & " And PatronTicket__TicketOrder__r.PatronTicket__Email__c <>''"
            sSQL = sSQL & " And PatronTicket__TicketOrder__r.isdeleted = False And isdeleted = False"
            sSQL = sSQL & " And PatronTicket__Status__c = 'Active'"
            sSQL = sSQL & " And PatronTicket__TicketOrder__r.PatronTicket__OrderStatus__c in ('Complete','To Be Qualified')"
            sSQL = sSQL & " order by PatronTicket__TicketOrder__r.PatronTicket__Email__c"

            ' Variable to store query results
            Dim qr As ApexApi.QueryResult = New ApexApi.QueryResult
            Dim strEmail As String = Nothing
            Dim strOrder As String = Nothing
            Using ss1 As ApexApi.SoapClient = New ApexApi.SoapClient
                ss1.ChannelFactory.Endpoint.Address = New System.ServiceModel.EndpointAddress(gServerURL)
                ss1.query(sHeader, Nothing, Nothing, Nothing, Nothing, sSQL, qr)

            End Using

            Dim records As ApexApi.sObject() = qr.records

            For i As Integer = 0 To records.Length - 1
                Dim record As ApexApi.sObject = qr.records(i)
                'If we have seen this email already then don't add a new row
                If strEmail = getChildFieldValue("PatronTicket__TicketOrder__r", "PatronTicket__Email__c", record.Any).ToString() Then
                    'Seen this email already so don't insert again
                Else
                    Using conn As New SQLiteConnection(gconnstrSQLITE)
                        Dim insertQuery As String = "INSERT INTO PreShowList(EventInstanceID,FirstName,LastName,Email,OrderName) VALUES(@EventInstanceID,@FirstName, @LastName,@Email,@OrderName)"
                        Dim cmd As New SQLiteCommand(insertQuery, conn)
                        cmd.Parameters.AddWithValue("@EventInstanceID", getFieldValue("event_instance_id__c", record.Any))
                        cmd.Parameters.AddWithValue("@FirstName", getChildFieldValue("PatronTicket__TicketOrder__r", "PatronTicket__FirstName__c", record.Any).ToString())
                        cmd.Parameters.AddWithValue("@LastName", getChildFieldValue("PatronTicket__TicketOrder__r", "PatronTicket__LastName__c", record.Any).ToString())
                        cmd.Parameters.AddWithValue("@Email", getChildFieldValue("PatronTicket__TicketOrder__r", "PatronTicket__Email__c", record.Any).ToString())
                        cmd.Parameters.AddWithValue("@OrderName", getChildFieldValue("PatronTicket__TicketOrder__r", "name", record.Any).ToString())
                        conn.Open()
                        cmd.ExecuteNonQuery()
                        strEmail = getChildFieldValue("PatronTicket__TicketOrder__r", "PatronTicket__Email__c", record.Any).ToString()
                    End Using
                End If

            Next

        Catch ex As Exception

            MsgBox(ex.Message)

        End Try
        ' MsgBox("Inserted values in the PreShowList table")
    End Sub

    Private Sub bUserName_Click(sender As Object, e As EventArgs) Handles bUserName.Click
        frmMessage.Show()
        frmMessage.WebBrowser1.DocumentText = "<br/>Enter your Salesforce/PatronManager username."
    End Sub

    Private Sub bPassword_Click(sender As Object, e As EventArgs) Handles bPassword.Click
        frmMessage.Show()
        frmMessage.WebBrowser1.DocumentText = "<br/>Enter your Salesforce/PatronManager password."
    End Sub

    Private Sub bSecurityToken_Click(sender As Object, e As EventArgs) Handles bSecurityToken.Click
        frmMessage.Show()
        frmMessage.WebBrowser1.DocumentText = "<br/>If you are adding a user or updating a user, enter the Salesforce/PatronManager Security Token for that user.  "
    End Sub
End Class