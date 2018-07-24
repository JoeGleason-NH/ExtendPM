Imports System.Security.Cryptography
Imports System.Text

Module modMain
    'Some global variables
    Public gSessionID As String = Nothing
    Public gServerURL As String = Nothing
    Public gSOQL As String = Nothing
    Public gUserName As String = Nothing
    Public gPassword As String = Nothing
    Public gSecurityToken As String = Nothing
    Public gconnstrSQLITE As String = Nothing
    Public gSMTPhost As String = Nothing
    Public gSMTPport As String = Nothing
    Public gFromEmail As String = Nothing
    Public gFromDisplayName As String = Nothing
    Public gEmailPassword As String = Nothing
    Public gEmailTestSendTo As String = Nothing




    Public Sub LoginSF(strUserName As String, strPassword As String, strSecurityToken As String)

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

    Public Function getFieldValue(ByVal fieldName As String, ByVal fields() As System.Xml.XmlElement) As String
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

    Public Function getChildFieldValue(ByVal fieldName As String, ByVal childField As String, ByVal fields() As System.Xml.XmlElement) As String
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

    Public Function Encrypt(ByVal Data As String) As String
        Dim shaM As New SHA1Managed
        Convert.ToBase64String(shaM.ComputeHash(Encoding.ASCII.GetBytes(Data)))
        Dim eNC_data() As Byte = ASCIIEncoding.ASCII.GetBytes(Data)
        Dim eNC_str As String = Convert.ToBase64String(eNC_data)
        Encrypt = eNC_str
    End Function

    Public Function Decrypt(ByVal Data As String) As String
        Dim dEC_data() As Byte = Convert.FromBase64String(Data)
        Dim dEC_Str As String = ASCIIEncoding.ASCII.GetString(dEC_data)
        Decrypt = dEC_Str
    End Function
End Module
