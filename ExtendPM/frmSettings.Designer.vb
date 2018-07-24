<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSettings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtEmailLogin = New System.Windows.Forms.TextBox()
        Me.txtEmailDisplay = New System.Windows.Forms.TextBox()
        Me.txtEmailPassword = New System.Windows.Forms.TextBox()
        Me.txtSMTPHost = New System.Windows.Forms.TextBox()
        Me.txtSMTPPort = New System.Windows.Forms.TextBox()
        Me.txtTestSendTo = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmdSave = New System.Windows.Forms.Button()
        Me.cmdRefresh = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtEmailLogin
        '
        Me.txtEmailLogin.Location = New System.Drawing.Point(126, 82)
        Me.txtEmailLogin.Name = "txtEmailLogin"
        Me.txtEmailLogin.Size = New System.Drawing.Size(143, 20)
        Me.txtEmailLogin.TabIndex = 0
        '
        'txtEmailDisplay
        '
        Me.txtEmailDisplay.Location = New System.Drawing.Point(126, 108)
        Me.txtEmailDisplay.Name = "txtEmailDisplay"
        Me.txtEmailDisplay.Size = New System.Drawing.Size(143, 20)
        Me.txtEmailDisplay.TabIndex = 1
        '
        'txtEmailPassword
        '
        Me.txtEmailPassword.Location = New System.Drawing.Point(126, 134)
        Me.txtEmailPassword.Name = "txtEmailPassword"
        Me.txtEmailPassword.Size = New System.Drawing.Size(143, 20)
        Me.txtEmailPassword.TabIndex = 2
        '
        'txtSMTPHost
        '
        Me.txtSMTPHost.Location = New System.Drawing.Point(126, 160)
        Me.txtSMTPHost.Name = "txtSMTPHost"
        Me.txtSMTPHost.Size = New System.Drawing.Size(143, 20)
        Me.txtSMTPHost.TabIndex = 3
        '
        'txtSMTPPort
        '
        Me.txtSMTPPort.Location = New System.Drawing.Point(126, 186)
        Me.txtSMTPPort.Name = "txtSMTPPort"
        Me.txtSMTPPort.Size = New System.Drawing.Size(143, 20)
        Me.txtSMTPPort.TabIndex = 4
        '
        'txtTestSendTo
        '
        Me.txtTestSendTo.Location = New System.Drawing.Point(126, 212)
        Me.txtTestSendTo.Name = "txtTestSendTo"
        Me.txtTestSendTo.Size = New System.Drawing.Size(143, 20)
        Me.txtTestSendTo.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(47, 85)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Email Login:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 111)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(103, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Email Display Name:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(28, 137)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(84, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Email Password:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(47, 163)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(65, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "SMTP Host:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(50, 189)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(62, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "SMTP Port:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(9, 215)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(103, 13)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Send Test Email To:"
        '
        'cmdSave
        '
        Me.cmdSave.Location = New System.Drawing.Point(131, 271)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(97, 27)
        Me.cmdSave.TabIndex = 12
        Me.cmdSave.Text = "Save/Update"
        Me.cmdSave.UseVisualStyleBackColor = True
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Location = New System.Drawing.Point(254, 271)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.Size = New System.Drawing.Size(69, 26)
        Me.cmdRefresh.TabIndex = 13
        Me.cmdRefresh.Text = "Refresh"
        Me.cmdRefresh.UseVisualStyleBackColor = True
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(411, 366)
        Me.Controls.Add(Me.cmdRefresh)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtTestSendTo)
        Me.Controls.Add(Me.txtSMTPPort)
        Me.Controls.Add(Me.txtSMTPHost)
        Me.Controls.Add(Me.txtEmailPassword)
        Me.Controls.Add(Me.txtEmailDisplay)
        Me.Controls.Add(Me.txtEmailLogin)
        Me.Name = "frmSettings"
        Me.Text = "frmSettings"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtEmailLogin As TextBox
    Friend WithEvents txtEmailDisplay As TextBox
    Friend WithEvents txtEmailPassword As TextBox
    Friend WithEvents txtSMTPHost As TextBox
    Friend WithEvents txtSMTPPort As TextBox
    Friend WithEvents txtTestSendTo As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents cmdSave As Button
    Friend WithEvents cmdRefresh As Button
End Class
