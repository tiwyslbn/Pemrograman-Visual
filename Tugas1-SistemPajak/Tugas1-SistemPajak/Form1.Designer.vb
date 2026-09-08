Imports System.IO

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormLogin
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
        txtNama = New TextBox()
        txtNIM = New TextBox()
        pbFoto = New PictureBox()
        lblRole = New Label()
        lblNama = New Label()
        Label3 = New Label()
        cbRole = New ComboBox()
        btnLogin = New Button()
        CType(pbFoto, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' txtNama
        ' 
        txtNama.Location = New Point(343, 266)
        txtNama.Name = "txtNama"
        txtNama.Size = New Size(150, 31)
        txtNama.TabIndex = 1
        ' 
        ' txtNIM
        ' 
        txtNIM.Location = New Point(343, 325)
        txtNIM.Name = "txtNIM"
        txtNIM.Size = New Size(150, 31)
        txtNIM.TabIndex = 2
        ' 
        ' pbFoto
        ' 
        pbFoto.Location = New Point(291, 48)
        pbFoto.Name = "pbFoto"
        pbFoto.Size = New Size(150, 129)
        pbFoto.TabIndex = 3
        pbFoto.TabStop = False
        ' 
        ' lblRole
        ' 
        lblRole.AutoSize = True
        lblRole.Location = New Point(238, 214)
        lblRole.Name = "lblRole"
        lblRole.Size = New Size(65, 25)
        lblRole.TabIndex = 4
        lblRole.Text = "Role   :"
        ' 
        ' lblNama
        ' 
        lblNama.AutoSize = True
        lblNama.Location = New Point(238, 265)
        lblNama.Name = "lblNama"
        lblNama.Size = New Size(68, 25)
        lblNama.TabIndex = 5
        lblNama.Text = "Nama :"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(238, 328)
        Label3.Name = "Label3"
        Label3.Size = New Size(60, 25)
        Label3.TabIndex = 6
        Label3.Text = "NIM  :"
        ' 
        ' cbRole
        ' 
        cbRole.DropDownStyle = ComboBoxStyle.DropDownList
        cbRole.FormattingEnabled = True
        cbRole.Items.AddRange(New Object() {"Staff", "Manager"})
        cbRole.Location = New Point(345, 218)
        cbRole.Name = "cbRole"
        cbRole.Size = New Size(148, 33)
        cbRole.TabIndex = 7
        ' 
        ' btnLogin
        ' 
        btnLogin.Location = New Point(312, 382)
        btnLogin.Name = "btnLogin"
        btnLogin.Size = New Size(112, 34)
        btnLogin.TabIndex = 8
        btnLogin.Text = "Login"
        btnLogin.UseVisualStyleBackColor = True
        ' 
        ' FormLogin
        ' 
        AutoScaleDimensions = New SizeF(10.0F, 25.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(747, 450)
        Controls.Add(btnLogin)
        Controls.Add(cbRole)
        Controls.Add(Label3)
        Controls.Add(lblNama)
        Controls.Add(lblRole)
        Controls.Add(pbFoto)
        Controls.Add(txtNIM)
        Controls.Add(txtNama)
        Name = "FormLogin"
        Text = "FormLogin"
        CType(pbFoto, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents txtNama As TextBox
    Friend WithEvents txtNIM As TextBox
    Friend WithEvents pbFoto As PictureBox
    Friend WithEvents lblRole As Label
    Friend WithEvents lblNama As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents cbRole As ComboBox
    Friend WithEvents btnLogin As Button

    Private Sub FormLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cbRole.SelectedIndex = 0 ' default pilihan pertama
    End Sub

    Private Sub cbRole_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbRole.SelectedIndexChanged
        Dim namaFile As String = cbRole.SelectedItem.ToString().ToLower() & ".jpg"
        Dim filePath As String = Path.Combine(Application.StartupPath, "Asset", namaFile)

        If File.Exists(filePath) Then
            pbFoto.Image = Image.FromFile(filePath)
        End If
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim nama As String = txtNama.Text.Trim()
        Dim nim As String = txtNIM.Text.Trim()

        If String.IsNullOrEmpty(nama) OrElse String.IsNullOrEmpty(nim) Then
            MessageBox.Show("Masukkan akun dengan benar", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim nimAngka As Long
        If Not Long.TryParse(nim, nimAngka) Then
            MessageBox.Show("Masukkan NIM dengan benar", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim formPajak As New FormPajak(nama, cbRole.SelectedItem.ToString())
        formPajak.Show()
        Me.Hide()
    End Sub

End Class
