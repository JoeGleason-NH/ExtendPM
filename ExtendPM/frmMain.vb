Imports ExtendPM.ApexApi
Imports System.ComponentModel
Imports System.Data.SQLite
Imports System.IO

Public Class frmMain

    Private Sub LoginSF(strUserName As String, strPassword As String, strSecurityToken As String)

        Try
            Dim lr As ApexApi.LoginResult
            Using ss As ApexApi.SoapClient = New ApexApi.SoapClient
                If gSessionID Is Nothing Or gSessionID = "" Then
                    ' Login Call
                    lr = ss.login(Nothing, Nothing, strUserName, strPassword & strSecurityToken)
                    If lr.passwordExpired Then
                        MsgBox("Password Expired")
                        Exit Sub
                    End If
                    gSessionID = lr.sessionId.ToString().Trim()
                    gServerURL = lr.serverUrl.ToString().Trim()
                End If
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Function getFieldValue(ByVal fieldName As String, ByVal fields() As System.Xml.XmlElement) As String
        'Retrieve the value stored in the specified field
        Dim returnValue As String = ""
        If Not fields Is Nothing Then
            For i As Integer = 0 To fields.GetUpperBound(0)
                ' MsgBox(fields(i).OuterXml)
                If fields(i).LocalName.ToLower().Equals(fieldName.ToLower()) Then

                    Select Case fields(i).LocalName
                        Case "PatronTicket__TicketableEvent__r"
                            returnValue = fields(i).LastChild.InnerText
                        Case "PatronTicket__Venue__r"
                            MsgBox(fields(i).OuterXml)
                            returnValue = fields(i).LastChild.InnerText
                        Case Else
                            returnValue = fields(i).InnerText
                    End Select

                End If
            Next
        End If
        Return returnValue
    End Function

    Private Function getChildFieldValue(ByVal fieldName As String, ByVal childField As String, ByVal fields() As System.Xml.XmlElement) As String
        'Retrieve the value stored in the specified field
        Dim returnValue As String = ""
        If Not fields Is Nothing Then
            For i As Integer = 0 To fields.GetUpperBound(0)
                'Match the parent field first.  This will be the field that ends in __r to indicate pulling from another table's fields
                If fields(i).LocalName.ToLower().Equals(fieldName.ToLower()) Then
                    'Display the contents of the child nodes.
                    If fields(i).HasChildNodes Then
                        Dim c As Integer
                        For c = 0 To fields(i).ChildNodes.Count - 1
                            If fields(i).ChildNodes(c).LocalName.ToLower().Equals(childField.ToLower()) Then
                                returnValue = fields(i).ChildNodes(c).InnerText
                            End If
                        Next c
                    End If
                End If
            Next
        End If
        Return returnValue
    End Function

    Private Function PopulateShowList()

        lvwShows.Items.Clear()

        If gSessionID Is Nothing Or gSessionID = "" Then
            LoginSF(gUserName, gPassword, gSecurityToken)
        End If

        Try
            ' Store SessionId in SessionHeader; We will need while making query() call
            Dim sHeader As ApexApi.SessionHeader = New ApexApi.SessionHeader
            sHeader.sessionId = gSessionID

            Dim sSQL = "SELECT Id,PatronTicket__TicketableEvent__r.Name,PatronTicket__InstanceDate__c,PatronTicket__Venue__r.PatronTicket__DisplayName__c, "
            sSQL = sSQL & " PatronTicket__Venue__r.PatronTicket__AddressInformation__c"
            sSQL = sSQL & " FROM PatronTicket__EventInstance__c"
            sSQL = sSQL & " WHERE PatronTicket__InstanceDate__c =  NEXT_N_DAYS:30"
            sSQL = sSQL & " ORDER BY PatronTicket__InstanceDate__c"

            ' Variable to store query results
            Dim qr As ApexApi.QueryResult = New ApexApi.QueryResult
            Using ss1 As ApexApi.SoapClient = New ApexApi.SoapClient
                ss1.ChannelFactory.Endpoint.Address = New System.ServiceModel.EndpointAddress(gServerURL)

                ss1.query(sHeader, Nothing, Nothing, Nothing, Nothing, sSQL, qr)
            End Using

            Dim records As ApexApi.sObject() = qr.records

            With lvwShows
                .Columns.Add("EvtID", 2, HorizontalAlignment.Left)
                .Columns.Add("Event Name", 170, HorizontalAlignment.Left)
                .Columns.Add("Event Date", 130, HorizontalAlignment.Left)
                .Columns.Add("Venue", 130, HorizontalAlignment.Left)
                .Columns.Add("Venue Address", 120, HorizontalAlignment.Left)
            End With

            For i As Integer = 0 To records.Length - 1
                Dim record As ApexApi.sObject = qr.records(i)

                Dim ls As New ListViewItem(getFieldValue("ID", record.Any))
                ls.SubItems.Add(getFieldValue("PatronTicket__TicketableEvent__r", record.Any)).ToString()
                ls.SubItems.Add(DateTime.Parse((getFieldValue("PatronTicket__InstanceDate__c", record.Any)).ToString()))
                ls.SubItems.Add(getChildFieldValue("PatronTicket__Venue__r", "PatronTicket__DisplayName__c", record.Any)).ToString()
                ls.SubItems.Add(getChildFieldValue("PatronTicket__Venue__r", "PatronTicket__AddressInformation__c", record.Any)).ToString()
                lvwShows.Items.Add(ls)

            Next

        Catch ex As Exception

            MsgBox(ex.Message)

        End Try

    End Function

    Private Function PopulatePreShowListView()

        'Clean slate the list view
        lvwPreShow.Clear()

        Using conn As New SQLiteConnection(gconnstrSQLITE)

            Dim sSQL As String = "Select objID, EventInstanceId, SendDate, SendStatus,EventTitle, ShowDate, ShowTime, "
            sSQL = sSQL & "LobbyTime, RunTime, Venue, VenueAddress, EventURL, NumSent, Block1, Block2, Block3, "
            sSQL = sSQL & "Block4, Block5, Block6, F1Title, F1Date, F1URL, F1Img, "
            sSQL = sSQL & "F2Title, F2Date, F2URL, F2Img, F3Title, F3Date, F3URL, F3Img, TemplateName from EmailJobs"
            Dim cmd As New SQLiteCommand(sSQL, conn)
            conn.Open()
            Dim dr As SQLiteDataReader = cmd.ExecuteReader

            If dr.HasRows Then
                With lvwPreShow
                    .Columns.Add(dr.GetName(0), 2, HorizontalAlignment.Left) 'objID
                    .Columns.Add(dr.GetName(1), 2, HorizontalAlignment.Left) 'EventInstanceID
                    .Columns.Add(dr.GetName(2), 100, HorizontalAlignment.Left) 'SendDate
                    .Columns.Add(dr.GetName(3), 75, HorizontalAlignment.Left) 'SendStatus
                    .Columns.Add(dr.GetName(4), 140, HorizontalAlignment.Left) 'Event Title
                    .Columns.Add(dr.GetName(5), 100, HorizontalAlignment.Left) 'ShowDate
                    .Columns.Add(dr.GetName(6), 100, HorizontalAlignment.Left) 'ShowTime
                    .Columns.Add(dr.GetName(7), 100, HorizontalAlignment.Left) 'LobbyTime
                    .Columns.Add(dr.GetName(8), 100, HorizontalAlignment.Left) 'RunTime
                    .Columns.Add(dr.GetName(9), 100, HorizontalAlignment.Left) 'Venue
                    .Columns.Add(dr.GetName(10), 100, HorizontalAlignment.Left) 'VenueAddress
                    .Columns.Add(dr.GetName(11), 100, HorizontalAlignment.Left) 'EventURL
                    .Columns.Add(dr.GetName(12), 100, HorizontalAlignment.Left) 'NumSent
                    .Columns.Add(dr.GetName(13), 100, HorizontalAlignment.Left) 'Block1
                    .Columns.Add(dr.GetName(14), 100, HorizontalAlignment.Left) 'Block2
                    .Columns.Add(dr.GetName(15), 100, HorizontalAlignment.Left) 'Block3
                    .Columns.Add(dr.GetName(16), 100, HorizontalAlignment.Left) 'Block4
                    .Columns.Add(dr.GetName(17), 100, HorizontalAlignment.Left) 'Block5
                    .Columns.Add(dr.GetName(18), 100, HorizontalAlignment.Left) 'Block6
                    .Columns.Add(dr.GetName(19), 100, HorizontalAlignment.Left) 'F1Title
                    .Columns.Add(dr.GetName(20), 100, HorizontalAlignment.Left) 'F1Date
                    .Columns.Add(dr.GetName(21), 100, HorizontalAlignment.Left) 'F1URL
                    .Columns.Add(dr.GetName(22), 100, HorizontalAlignment.Left) 'F1Img
                    .Columns.Add(dr.GetName(23), 100, HorizontalAlignment.Left) 'F2Title
                    .Columns.Add(dr.GetName(24), 100, HorizontalAlignment.Left) 'F2Date
                    .Columns.Add(dr.GetName(25), 100, HorizontalAlignment.Left) 'F2URL
                    .Columns.Add(dr.GetName(26), 100, HorizontalAlignment.Left) 'F2Img
                    .Columns.Add(dr.GetName(27), 100, HorizontalAlignment.Left) 'F3Title
                    .Columns.Add(dr.GetName(28), 100, HorizontalAlignment.Left) 'F3Date
                    .Columns.Add(dr.GetName(29), 100, HorizontalAlignment.Left) 'F3URL
                    .Columns.Add(dr.GetName(30), 100, HorizontalAlignment.Left) 'F3Img
                    .Columns.Add(dr.GetName(31), 100, HorizontalAlignment.Left) 'TemplateName
                End With

                While dr.Read
                    Dim ls As New ListViewItem(dr.Item("objID").ToString())
                    ls.SubItems.Add(dr.Item("EventInstanceID").ToString()) '1
                    ls.SubItems.Add(dr.Item("SendDate").ToString()) '2
                    ls.SubItems.Add(dr.Item("SendStatus").ToString()) '3
                    ls.SubItems.Add(dr.Item("EventTitle").ToString()) '4
                    ls.SubItems.Add(dr.Item("ShowDate").ToString()) '5
                    ls.SubItems.Add(dr.Item("ShowTime").ToString()) '6
                    ls.SubItems.Add(dr.Item("LobbyTime").ToString()) '7
                    ls.SubItems.Add(dr.Item("RunTime").ToString()) '8
                    ls.SubItems.Add(dr.Item("Venue").ToString()) '9
                    ls.SubItems.Add(dr.Item("VenueAddress").ToString()) '10
                    ls.SubItems.Add(dr.Item("EventURL").ToString()) '11
                    ls.SubItems.Add(dr.Item("NumSent").ToString()) '12
                    ls.SubItems.Add(dr.Item("Block1").ToString()) '13
                    ls.SubItems.Add(dr.Item("Block2").ToString()) '14
                    ls.SubItems.Add(dr.Item("Block3").ToString()) '15
                    ls.SubItems.Add(dr.Item("Block4").ToString()) '16
                    ls.SubItems.Add(dr.Item("Block5").ToString()) '17
                    ls.SubItems.Add(dr.Item("Block6").ToString()) '18
                    ls.SubItems.Add(dr.Item("F1Title").ToString()) '19
                    ls.SubItems.Add(dr.Item("F1Date").ToString()) '20
                    ls.SubItems.Add(dr.Item("F1URL").ToString()) '21
                    ls.SubItems.Add(dr.Item("F1Img").ToString()) '22
                    ls.SubItems.Add(dr.Item("F2Title").ToString()) '23
                    ls.SubItems.Add(dr.Item("F2Date").ToString()) '24
                    ls.SubItems.Add(dr.Item("F2URL").ToString()) '25
                    ls.SubItems.Add(dr.Item("F2Img").ToString()) '26
                    ls.SubItems.Add(dr.Item("F3Title").ToString()) '27
                    ls.SubItems.Add(dr.Item("F3Date").ToString()) '28
                    ls.SubItems.Add(dr.Item("F3URL").ToString()) '29
                    ls.SubItems.Add(dr.Item("F3Img").ToString()) '30
                    ls.SubItems.Add(dr.Item("TemplateName").ToString()) '31
                    lvwPreShow.Items.Add(ls)
                End While

            End If

        End Using

    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        frmLogin.Dispose()
        'lblIntro.Text = "Manage PreShow Emails here.  List below shows all recently sent And currently scheduled emails." + vbCrLf + "Modify an existing entry by double-cliking it And change the fields below."
        'lblSelectShow.Text = "Double-click on a row to add a New scheduled preshow email."
        Call PopulatePreShowListView()
        Call PopulateShowList()
    End Sub

    Private Sub lvwPreShow_DoubleClick(sender As Object, e As EventArgs) Handles lvwPreShow.DoubleClick

        '  ClearCheckboxes()
        '  ClearTextboxes()
        '  intNew = 1 'Set update flag

        Dim i As Integer
        For Each item As ListViewItem In lvwPreShow.SelectedItems
            i = item.Index
        Next
        txtObjID.Text = lvwPreShow.Items(i).Text
        txtEventInstanceID.Text = lvwPreShow.Items(i).SubItems(1).Text
        dtpSendDate.Text = lvwPreShow.Items(i).SubItems(2).Text
        cboSendStatus.Text = lvwPreShow.Items(i).SubItems(3).Text
        txtPerf.Text = lvwPreShow.Items(i).SubItems(4).Text
        txtEventURL.Text = lvwPreShow.Items(i).SubItems(11).Text
        txtShowDate.Text = lvwPreShow.Items(i).SubItems(5).Text
        txtLobbyTime.Text = lvwPreShow.Items(i).SubItems(7).Text
        txtShowTime.Text = lvwPreShow.Items(i).SubItems(6).Text
        txtRunTime.Text = lvwPreShow.Items(i).SubItems(8).Text
        txtVenue.Text = lvwPreShow.Items(i).SubItems(9).Text
        txtVenueAddress.Text = lvwPreShow.Items(i).SubItems(10).Text
        txtF1Title.Text = lvwPreShow.Items(i).SubItems(19).Text
        txtF1Date.Text = lvwPreShow.Items(i).SubItems(20).Text
        txtF1URL.Text = lvwPreShow.Items(i).SubItems(21).Text
        txtF1Image.Text = lvwPreShow.Items(i).SubItems(22).Text
        txtF2Title.Text = lvwPreShow.Items(i).SubItems(23).Text
        txtF2Date.Text = lvwPreShow.Items(i).SubItems(24).Text
        txtF2URL.Text = lvwPreShow.Items(i).SubItems(25).Text
        txtF2Image.Text = lvwPreShow.Items(i).SubItems(26).Text
        txtF3Title.Text = lvwPreShow.Items(i).SubItems(27).Text
        txtF3Date.Text = lvwPreShow.Items(i).SubItems(28).Text
        txtF3URL.Text = lvwPreShow.Items(i).SubItems(29).Text
        txtF3Image.Text = lvwPreShow.Items(i).SubItems(30).Text
        txtNumSent.Text = lvwPreShow.Items(i).SubItems(12).Text
        If Len(lvwPreShow.Items(i).SubItems(13).Text) > 1 Then txtBlock1.Text = lvwPreShow.Items(i).SubItems(13).Text
        If Len(lvwPreShow.Items(i).SubItems(14).Text) > 1 Then txtBlock2.Text = lvwPreShow.Items(i).SubItems(14).Text
        If Len(lvwPreShow.Items(i).SubItems(15).Text) > 1 Then txtBlock3.Text = lvwPreShow.Items(i).SubItems(15).Text
        If Len(lvwPreShow.Items(i).SubItems(16).Text) > 1 Then txtBlock4.Text = lvwPreShow.Items(i).SubItems(16).Text
        If Len(lvwPreShow.Items(i).SubItems(17).Text) > 1 Then txtBlock5.Text = lvwPreShow.Items(i).SubItems(17).Text
        If Len(lvwPreShow.Items(i).SubItems(18).Text) > 1 Then txtBlock6.Text = lvwPreShow.Items(i).SubItems(18).Text
        txtTemplateName.Text = lvwPreShow.Items(i).SubItems(31).Text
        'dtExpir.CustomFormat = "yyyy-MM-dd HH:mm"
        'dtExpireDate.Text = lvwPreShow.Items(i).SubItems(12).Text
        'cboReportName.Text = lvwPreShow.Items(i).SubItems(13).Text

        cboSendStatus.Enabled = True
        dtpSendDate.Enabled = True

    End Sub

    Private Sub lvwShows_DoubleClick(sender As Object, e As EventArgs) Handles lvwShows.DoubleClick

        Dim i As Integer

        For Each item As ListViewItem In lvwShows.SelectedItems
            i = item.Index
        Next

        txtObjID.Text = "" 'lvwShows.Items(i).SubItems(0).Text
        txtEventInstanceID.Text = lvwShows.Items(i).SubItems(0).Text
        txtPerf.Text = UCase(lvwShows.Items(i).SubItems(1).Text)
        txtVenue.Text = lvwShows.Items(i).SubItems(3).Text
        txtVenueAddress.Text = lvwShows.Items(i).SubItems(4).Text
        txtShowDate.Text = DateTime.Parse(lvwShows.Items(i).SubItems(2).Text).ToString("dddd, MMMMM  d")
        txtShowTime.Text = DateTime.Parse(lvwShows.Items(i).SubItems(2).Text).ToString("h:mm tt")
        txtNumSent.Text = 0
        dtpSendDate.CustomFormat = "yyyy-MM-dd HH:mm tt"
        cboSendStatus.Text = "Ready"
        cboSendStatus.Enabled = True
        dtpSendDate.Enabled = True


    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        Dim insertQuery As String = Nothing
        Using conn As New SQLiteConnection(gconnstrSQLITE)
            If txtObjID.Text = "" Then

                insertQuery = "INSERT INTO EMailJobs(EventInstanceID,SendDate,SendStatus,EventTitle,ShowDate,ShowTime,LobbyTime,RunTime,Venue,VenueAddress,EventURL,"
                insertQuery = insertQuery & "Block1,Block2,Block3,Block4,Block5,Block6,F1Title,F1Date,F1URL,F1Img,F2Title,F2Date,F2URL,F2Img,F3Title,F3Date,F3URL,F3Img,TemplateName) "
                insertQuery = insertQuery & " VALUES(@EventInstanceID,@SendDate, @SendStatus, @EventTitle,@ShowDate,@ShowTime,@LobbyTime,@RunTime,@Venue,@VenueAddress,@EventURL,"
                insertQuery = insertQuery & "@Block1,@Block2,@Block3,@Block4,@Block5,@Block6,@F1Title,@F1Date,@F1URL,@F1Img,@F2Title,@F2Date,@F2URL,@F2Img,@F3Title,@F3Date,@F3URL,@F3Img,@TemplateName)"
            Else
                insertQuery = "UPDATE EMailJobs SET EventInstanceID=@EventInstanceID,SendDate=@SendDate,SendStatus=@SendStatus,EventTitle=@EventTitle, "
                insertQuery = insertQuery & "ShowDate=@ShowDate,ShowTime=@ShowTime,LobbyTime=@LobbyTime,RunTime=@RunTime,Venue=@Venue,VenueAddress=@VenueAddress,EventURL=@EventURL,"
                insertQuery = insertQuery & "Block1=@Block1,Block2=@Block2,Block3=@Block3,Block4=@Block4,Block5=@Block5,Block6=@Block6,F1Title=@F1Title,F1Date=@F1Date,"
                insertQuery = insertQuery & "F1URL=@F1URL,F1Img=@F1Img,F2Title=@F2Title,F2Date=@F2Date,F2URL=@F2URL,F2Img=@F2Img,F3Title=@F3Title,F3Date=@F3Date,F3URL=@F3URL,F3Img=@F3Img,TemplateName=@TemplateName"
                insertQuery = insertQuery & " WHERE objid=@objID"
            End If

            Dim cmd As New SQLiteCommand(insertQuery, conn)
            cmd.Parameters.AddWithValue("@EventInstanceID", txtEventInstanceID.Text)
            cmd.Parameters.AddWithValue("@SendDate", dtpSendDate.Value.ToString("yyyy-MM-dd HH:mm"))
            cmd.Parameters.AddWithValue("@SendStatus", cboSendStatus.Text)
            cmd.Parameters.AddWithValue("@EventTitle", txtPerf.Text)
            cmd.Parameters.AddWithValue("@ShowDate", txtShowDate.Text)
            cmd.Parameters.AddWithValue("@ShowTime", txtShowTime.Text)
            cmd.Parameters.AddWithValue("@LobbyTime", txtLobbyTime.Text)
            cmd.Parameters.AddWithValue("@RunTime", txtRunTime.Text)
            cmd.Parameters.AddWithValue("@Venue", txtVenue.Text)
            cmd.Parameters.AddWithValue("@VenueAddress", txtVenueAddress.Text)
            cmd.Parameters.AddWithValue("@Block1", txtBlock1.Text)
            cmd.Parameters.AddWithValue("@Block2", txtBlock2.Text)
            cmd.Parameters.AddWithValue("@Block3", txtBlock3.Text)
            cmd.Parameters.AddWithValue("@Block4", txtBlock4.Text)
            cmd.Parameters.AddWithValue("@Block5", txtBlock5.Text)
            cmd.Parameters.AddWithValue("@Block6", txtBlock6.Text)
            cmd.Parameters.AddWithValue("@EventURL", txtEventURL.Text)
            cmd.Parameters.AddWithValue("@F1Title", txtF1Title.Text)
            cmd.Parameters.AddWithValue("@F1Date", txtF1Date.Text)
            cmd.Parameters.AddWithValue("@F1URL", txtF1URL.Text)
            cmd.Parameters.AddWithValue("@F1Img", txtF1Image.Text)
            cmd.Parameters.AddWithValue("@F2Title", txtF2Title.Text)
            cmd.Parameters.AddWithValue("@F2Date", txtF2Date.Text)
            cmd.Parameters.AddWithValue("@F2URL", txtF2URL.Text)
            cmd.Parameters.AddWithValue("@F2Img", txtF2Image.Text)
            cmd.Parameters.AddWithValue("@F3Title", txtF3Title.Text)
            cmd.Parameters.AddWithValue("@F3Date", txtF3Date.Text)
            cmd.Parameters.AddWithValue("@F3URL", txtF3URL.Text)
            cmd.Parameters.AddWithValue("@F3Img", txtF3Image.Text)
            cmd.Parameters.AddWithValue("@TemplateName", txtTemplateName.Text)

            If txtObjID.Text <> "" Then
                cmd.Parameters.AddWithValue("@ObjID", txtObjID.Text)
            End If
            conn.Open()
            cmd.ExecuteNonQuery()
            MsgBox(String.Format("{0} has been added/updated", txtPerf.Text))

        End Using

        Call PopulatePreShowListView()
        Call PopulateShowList()

    End Sub

    Private Sub DeletePreShowRow(objID As Integer)

        'SQLITE - DELETE a record sample code

        Using conn As New SQLiteConnection(gconnstrSQLITE)
            Dim deleteQuery As String = "DELETE FROM EmailJobs WHERE objid =@objID"
            Dim cmd As New SQLiteCommand(deleteQuery, conn)
            cmd.Parameters.AddWithValue("@objID", objID)
            conn.Open()
            cmd.ExecuteNonQuery()
            MsgBox(String.Format("{0} has been Deleted", objID.ToString))
        End Using

        Call PopulatePreShowListView()
        Call PopulateShowList()

    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click
        Call DeletePreShowRow(CInt(txtObjID.Text))
    End Sub

    Private Sub cmdPreview_Click(sender As Object, e As EventArgs) Handles cmdPreview.Click

        Dim strBody As String = Nothing
        'Read the html file
        Dim reader As StreamReader = My.Computer.FileSystem.OpenTextFileReader(Application.StartupPath & "\Templates\" & txtTemplateName.Text)
        'Read the entire html file into a string
        strBody = reader.ReadToEnd
        'Replace the dynamic placeholders, basically a mail merge

        strBody = strBody.Replace("{EventTitle}", txtPerf.Text)
        strBody = strBody.Replace("{LobbyTime}", txtLobbyTime.Text)
        strBody = strBody.Replace("{ShowTime}", txtShowTime.Text)
        strBody = strBody.Replace("{ShowDate}", txtShowDate.Text)
        strBody = strBody.Replace("{RunTime}", txtRunTime.Text)
        strBody = strBody.Replace("{Venue}", txtVenue.Text)
        strBody = strBody.Replace("{VenueAddress}", txtVenueAddress.Text)
        strBody = strBody.Replace("{Block1}", txtBlock1.Text)
        strBody = strBody.Replace("{Block2}", txtBlock2.Text)
        strBody = strBody.Replace("{Block3}", txtBlock3.Text)
        strBody = strBody.Replace("{Block3}", txtBlock3.Text)
        strBody = strBody.Replace("{Block3}", txtBlock3.Text)
        strBody = strBody.Replace("{Block3}", txtBlock3.Text)
        strBody = strBody.Replace("{Block4}", txtBlock4.Text)
        strBody = strBody.Replace("{Block5}", txtBlock5.Text)
        strBody = strBody.Replace("{Block6}", txtBlock6.Text)
        strBody = strBody.Replace("{EventURL}", txtEventURL.Text)
        strBody = strBody.Replace("{F1Title}", txtF1Title.Text)
        strBody = strBody.Replace("{F1Date}", txtF1Date.Text)
        strBody = strBody.Replace("{F1URL}", txtF1URL.Text)
        strBody = strBody.Replace("{F1Img}", txtF1Image.Text)
        strBody = strBody.Replace("{F2Title}", txtF2Title.Text)
        strBody = strBody.Replace("{F2Date}", txtF2Date.Text)
        strBody = strBody.Replace("{F2URL}", txtF2URL.Text)
        strBody = strBody.Replace("{F2Img}", txtF2Image.Text)
        strBody = strBody.Replace("{F3Title}", txtF3Title.Text)
        strBody = strBody.Replace("{F3Date}", txtF3Date.Text)
        strBody = strBody.Replace("{F3URL}", txtF3URL.Text)
        strBody = strBody.Replace("{F3Img}", txtF3Image.Text)

        If File.Exists(Application.StartupPath + "\Preview\Preview.html") Then
            File.Delete(Application.StartupPath + "\Preview\Preview.html")
        End If

        Call WritePreviewFile(strBody)
        Process.Start("file://" & Application.StartupPath + "\Preview\Preview.html")
        'frmMessage.WebBrowser1.DocumentText = strBody
        'frmMessage.Show()

    End Sub

    Private Sub WritePreviewFile(strHTMLPreview As String)
        Dim filePath As String = Application.StartupPath + "\Preview\Preview.html"
        Using writer As New StreamWriter(filePath, True)
            If File.Exists(filePath) Then

                writer.WriteLine(strHTMLPreview)
            Else
                writer.WriteLine(strHTMLPreview)
            End If
        End Using
    End Sub

    Private Sub frmMain_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Application.Exit()
    End Sub
End Class