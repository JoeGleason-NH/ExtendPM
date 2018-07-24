<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMessage
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
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'WebBrowser1
        '
        Me.WebBrowser1.AllowNavigation = False
        Me.WebBrowser1.AllowWebBrowserDrop = False
        Me.WebBrowser1.IsWebBrowserContextMenuEnabled = False
        Me.WebBrowser1.Location = New System.Drawing.Point(11, 11)
        Me.WebBrowser1.Margin = New System.Windows.Forms.Padding(2)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(13, 13)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.ScriptErrorsSuppressed = True
        Me.WebBrowser1.Size = New System.Drawing.Size(814, 402)
        Me.WebBrowser1.TabIndex = 4
        Me.WebBrowser1.Url = New System.Uri("about:blank", System.UriKind.Absolute)
        '
        'cmdOK
        '
        Me.cmdOK.Location = New System.Drawing.Point(328, 421)
        Me.cmdOK.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(160, 27)
        Me.cmdOK.TabIndex = 3
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'frmMessage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(843, 459)
        Me.Controls.Add(Me.WebBrowser1)
        Me.Controls.Add(Me.cmdOK)
        Me.Name = "frmMessage"
        Me.Text = "frmMessage"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents WebBrowser1 As WebBrowser
    Friend WithEvents cmdOK As Button
End Class
