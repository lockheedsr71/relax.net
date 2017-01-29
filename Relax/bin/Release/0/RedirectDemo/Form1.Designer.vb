<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.StartBtn = New System.Windows.Forms.Button
        Me.OutputBox = New System.Windows.Forms.RichTextBox
        Me.InputBox = New System.Windows.Forms.TextBox
        Me.InputBtn = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.PathBox = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.HideChk = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'StartBtn
        '
        Me.StartBtn.Location = New System.Drawing.Point(434, 15)
        Me.StartBtn.Name = "StartBtn"
        Me.StartBtn.Size = New System.Drawing.Size(75, 23)
        Me.StartBtn.TabIndex = 0
        Me.StartBtn.Text = "Start"
        Me.StartBtn.UseVisualStyleBackColor = True
        '
        'OutputBox
        '
        Me.OutputBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OutputBox.Location = New System.Drawing.Point(12, 142)
        Me.OutputBox.Name = "OutputBox"
        Me.OutputBox.ReadOnly = True
        Me.OutputBox.Size = New System.Drawing.Size(654, 373)
        Me.OutputBox.TabIndex = 1
        Me.OutputBox.Text = ""
        '
        'InputBox
        '
        Me.InputBox.Location = New System.Drawing.Point(65, 80)
        Me.InputBox.Name = "InputBox"
        Me.InputBox.Size = New System.Drawing.Size(197, 20)
        Me.InputBox.TabIndex = 2
        '
        'InputBtn
        '
        Me.InputBtn.Location = New System.Drawing.Point(268, 78)
        Me.InputBtn.Name = "InputBtn"
        Me.InputBtn.Size = New System.Drawing.Size(75, 23)
        Me.InputBtn.TabIndex = 3
        Me.InputBtn.Text = "Send"
        Me.InputBtn.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Program:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 83)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Input:"
        '
        'PathBox
        '
        Me.PathBox.Location = New System.Drawing.Point(65, 17)
        Me.PathBox.Name = "PathBox"
        Me.PathBox.Size = New System.Drawing.Size(363, 20)
        Me.PathBox.TabIndex = 8
        Me.PathBox.Text = "C:\Windows\System32\cmd.exe"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 122)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(42, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Output:"
        '
        'HideChk
        '
        Me.HideChk.AutoSize = True
        Me.HideChk.Location = New System.Drawing.Point(65, 43)
        Me.HideChk.Name = "HideChk"
        Me.HideChk.Size = New System.Drawing.Size(127, 17)
        Me.HideChk.TabIndex = 10
        Me.HideChk.Text = "Hide process window"
        Me.HideChk.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AcceptButton = Me.InputBtn
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(678, 527)
        Me.Controls.Add(Me.HideChk)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.PathBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.InputBtn)
        Me.Controls.Add(Me.InputBox)
        Me.Controls.Add(Me.OutputBox)
        Me.Controls.Add(Me.StartBtn)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Redirection Demo"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StartBtn As System.Windows.Forms.Button
    Friend WithEvents OutputBox As System.Windows.Forms.RichTextBox
    Friend WithEvents InputBox As System.Windows.Forms.TextBox
    Friend WithEvents InputBtn As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents PathBox As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents HideChk As System.Windows.Forms.CheckBox

End Class
