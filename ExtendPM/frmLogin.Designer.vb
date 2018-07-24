<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmLogin
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.bSecurityToken = New System.Windows.Forms.Button()
        Me.bPassword = New System.Windows.Forms.Button()
        Me.bUserName = New System.Windows.Forms.Button()
        Me.cmdAddUser = New System.Windows.Forms.Button()
        Me.cmdLogin = New System.Windows.Forms.Button()
        Me.txtSecurityToken = New System.Windows.Forms.TextBox()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.txtUserName = New System.Windows.Forms.TextBox()
        Me.cmdUpdateUser = New System.Windows.Forms.Button()
        Me.cmdDeleteUser = New System.Windows.Forms.Button()
        Me.cmdConfig = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'bSecurityToken
        '
        Me.bSecurityToken.FlatAppearance.BorderSize = 0
        Me.bSecurityToken.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bSecurityToken.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bSecurityToken.Image = Global.ExtendPM.My.Resources.Resources.help
        Me.bSecurityToken.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.bSecurityToken.Location = New System.Drawing.Point(12, 71)
        Me.bSecurityToken.Margin = New System.Windows.Forms.Padding(2)
        Me.bSecurityToken.Name = "bSecurityToken"
        Me.bSecurityToken.Size = New System.Drawing.Size(108, 27)
        Me.bSecurityToken.TabIndex = 21
        Me.bSecurityToken.Text = "Security Token:"
        Me.bSecurityToken.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.bSecurityToken.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.bSecurityToken.UseVisualStyleBackColor = True
        '
        'bPassword
        '
        Me.bPassword.FlatAppearance.BorderSize = 0
        Me.bPassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bPassword.Image = Global.ExtendPM.My.Resources.Resources.help
        Me.bPassword.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.bPassword.Location = New System.Drawing.Point(12, 41)
        Me.bPassword.Margin = New System.Windows.Forms.Padding(2)
        Me.bPassword.Name = "bPassword"
        Me.bPassword.Size = New System.Drawing.Size(108, 27)
        Me.bPassword.TabIndex = 20
        Me.bPassword.Text = "Password:"
        Me.bPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.bPassword.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.bPassword.UseVisualStyleBackColor = True
        '
        'bUserName
        '
        Me.bUserName.FlatAppearance.BorderSize = 0
        Me.bUserName.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bUserName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bUserName.Image = Global.ExtendPM.My.Resources.Resources.help
        Me.bUserName.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.bUserName.Location = New System.Drawing.Point(12, 11)
        Me.bUserName.Margin = New System.Windows.Forms.Padding(2)
        Me.bUserName.Name = "bUserName"
        Me.bUserName.Size = New System.Drawing.Size(108, 27)
        Me.bUserName.TabIndex = 19
        Me.bUserName.Text = "User Name:"
        Me.bUserName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.bUserName.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.bUserName.UseVisualStyleBackColor = True
        '
        'cmdAddUser
        '
        Me.cmdAddUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddUser.Location = New System.Drawing.Point(131, 105)
        Me.cmdAddUser.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdAddUser.Name = "cmdAddUser"
        Me.cmdAddUser.Size = New System.Drawing.Size(93, 19)
        Me.cmdAddUser.TabIndex = 17
        Me.cmdAddUser.Text = "Add User"
        Me.cmdAddUser.UseVisualStyleBackColor = True
        '
        'cmdLogin
        '
        Me.cmdLogin.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdLogin.Location = New System.Drawing.Point(235, 105)
        Me.cmdLogin.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdLogin.Name = "cmdLogin"
        Me.cmdLogin.Size = New System.Drawing.Size(100, 73)
        Me.cmdLogin.TabIndex = 3
        Me.cmdLogin.Text = "Login"
        Me.cmdLogin.UseVisualStyleBackColor = True
        '
        'txtSecurityToken
        '
        Me.txtSecurityToken.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSecurityToken.Location = New System.Drawing.Point(131, 73)
        Me.txtSecurityToken.Margin = New System.Windows.Forms.Padding(2)
        Me.txtSecurityToken.Name = "txtSecurityToken"
        Me.txtSecurityToken.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtSecurityToken.Size = New System.Drawing.Size(205, 23)
        Me.txtSecurityToken.TabIndex = 4
        '
        'txtPassword
        '
        Me.txtPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPassword.Location = New System.Drawing.Point(131, 43)
        Me.txtPassword.Margin = New System.Windows.Forms.Padding(2)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(205, 23)
        Me.txtPassword.TabIndex = 2
        '
        'txtUserName
        '
        Me.txtUserName.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUserName.Location = New System.Drawing.Point(131, 13)
        Me.txtUserName.Margin = New System.Windows.Forms.Padding(2)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(205, 23)
        Me.txtUserName.TabIndex = 1
        '
        'cmdUpdateUser
        '
        Me.cmdUpdateUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUpdateUser.Location = New System.Drawing.Point(131, 130)
        Me.cmdUpdateUser.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdUpdateUser.Name = "cmdUpdateUser"
        Me.cmdUpdateUser.Size = New System.Drawing.Size(93, 19)
        Me.cmdUpdateUser.TabIndex = 22
        Me.cmdUpdateUser.Text = "Update User"
        Me.cmdUpdateUser.UseVisualStyleBackColor = True
        '
        'cmdDeleteUser
        '
        Me.cmdDeleteUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteUser.Location = New System.Drawing.Point(131, 158)
        Me.cmdDeleteUser.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdDeleteUser.Name = "cmdDeleteUser"
        Me.cmdDeleteUser.Size = New System.Drawing.Size(93, 19)
        Me.cmdDeleteUser.TabIndex = 23
        Me.cmdDeleteUser.Text = "Delete User"
        Me.cmdDeleteUser.UseVisualStyleBackColor = True
        '
        'cmdConfig
        '
        Me.cmdConfig.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdConfig.Location = New System.Drawing.Point(131, 181)
        Me.cmdConfig.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdConfig.Name = "cmdConfig"
        Me.cmdConfig.Size = New System.Drawing.Size(93, 25)
        Me.cmdConfig.TabIndex = 24
        Me.cmdConfig.Text = "Config Settings"
        Me.cmdConfig.UseVisualStyleBackColor = True
        '
        'frmLogin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(399, 211)
        Me.Controls.Add(Me.cmdConfig)
        Me.Controls.Add(Me.cmdDeleteUser)
        Me.Controls.Add(Me.cmdUpdateUser)
        Me.Controls.Add(Me.bSecurityToken)
        Me.Controls.Add(Me.bPassword)
        Me.Controls.Add(Me.bUserName)
        Me.Controls.Add(Me.cmdAddUser)
        Me.Controls.Add(Me.cmdLogin)
        Me.Controls.Add(Me.txtSecurityToken)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.txtUserName)
        Me.Name = "frmLogin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Login"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents bSecurityToken As Button
    Friend WithEvents bPassword As Button
    Friend WithEvents bUserName As Button
    Friend WithEvents cmdAddUser As Button
    Friend WithEvents cmdLogin As Button
    Friend WithEvents txtSecurityToken As TextBox
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents txtUserName As TextBox
    Friend WithEvents cmdUpdateUser As Button
    Friend WithEvents cmdDeleteUser As Button
    Friend WithEvents cmdConfig As Button
End Class
