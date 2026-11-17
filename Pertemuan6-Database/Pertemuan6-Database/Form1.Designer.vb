<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
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
        lblNim = New Label()
        lblNama = New Label()
        lblJurusan = New Label()
        txtNim = New TextBox()
        txtNama = New TextBox()
        txtJurusan = New TextBox()
        btnKoneksi = New Button()
        btnTambah = New Button()
        btnTampil = New Button()
        dgvMahasiswa = New DataGridView()
        CType(dgvMahasiswa, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblNim
        ' 
        lblNim.AutoSize = True
        lblNim.Location = New Point(198, 90)
        lblNim.Name = "lblNim"
        lblNim.Size = New Size(37, 20)
        lblNim.TabIndex = 0
        lblNim.Text = "NIM"
        ' 
        ' lblNama
        ' 
        lblNama.AutoSize = True
        lblNama.Location = New Point(198, 50)
        lblNama.Name = "lblNama"
        lblNama.Size = New Size(49, 20)
        lblNama.TabIndex = 1
        lblNama.Text = "Nama"
        ' 
        ' lblJurusan
        ' 
        lblJurusan.AutoSize = True
        lblJurusan.Location = New Point(198, 135)
        lblJurusan.Name = "lblJurusan"
        lblJurusan.Size = New Size(57, 20)
        lblJurusan.TabIndex = 2
        lblJurusan.Text = "Jurusan"
        ' 
        ' txtNim
        ' 
        txtNim.Location = New Point(275, 83)
        txtNim.Name = "txtNim"
        txtNim.Size = New Size(217, 27)
        txtNim.TabIndex = 3
        ' 
        ' txtNama
        ' 
        txtNama.Location = New Point(275, 43)
        txtNama.Name = "txtNama"
        txtNama.Size = New Size(217, 27)
        txtNama.TabIndex = 4
        ' 
        ' txtJurusan
        ' 
        txtJurusan.Location = New Point(275, 128)
        txtJurusan.Name = "txtJurusan"
        txtJurusan.Size = New Size(217, 27)
        txtJurusan.TabIndex = 5
        ' 
        ' btnKoneksi
        ' 
        btnKoneksi.Location = New Point(198, 178)
        btnKoneksi.Name = "btnKoneksi"
        btnKoneksi.Size = New Size(136, 29)
        btnKoneksi.TabIndex = 6
        btnKoneksi.Text = "Tes Koneksi"
        btnKoneksi.UseVisualStyleBackColor = True
        ' 
        ' btnTambah
        ' 
        btnTambah.Location = New Point(354, 178)
        btnTambah.Name = "btnTambah"
        btnTambah.Size = New Size(138, 29)
        btnTambah.TabIndex = 7
        btnTambah.Text = "Tambah Data"
        btnTambah.UseVisualStyleBackColor = True
        ' 
        ' btnTampil
        ' 
        btnTampil.Location = New Point(264, 213)
        btnTampil.Name = "btnTampil"
        btnTampil.Size = New Size(150, 29)
        btnTampil.TabIndex = 8
        btnTampil.Text = "Tampilkan Data"
        btnTampil.UseVisualStyleBackColor = True
        ' 
        ' dgvMahasiswa
        ' 
        dgvMahasiswa.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvMahasiswa.Location = New Point(198, 262)
        dgvMahasiswa.Name = "dgvMahasiswa"
        dgvMahasiswa.RowHeadersWidth = 51
        dgvMahasiswa.Size = New Size(300, 151)
        dgvMahasiswa.TabIndex = 9
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(dgvMahasiswa)
        Controls.Add(btnTampil)
        Controls.Add(btnTambah)
        Controls.Add(btnKoneksi)
        Controls.Add(txtJurusan)
        Controls.Add(txtNama)
        Controls.Add(txtNim)
        Controls.Add(lblJurusan)
        Controls.Add(lblNama)
        Controls.Add(lblNim)
        Name = "Form1"
        Text = "Form1"
        CType(dgvMahasiswa, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblNim As Label
    Friend WithEvents lblNama As Label
    Friend WithEvents lblJurusan As Label
    Friend WithEvents txtNim As TextBox
    Friend WithEvents txtNama As TextBox
    Friend WithEvents txtJurusan As TextBox
    Friend WithEvents btnKoneksi As Button
    Friend WithEvents btnTambah As Button
    Friend WithEvents btnTampil As Button
    Friend WithEvents dgvMahasiswa As DataGridView

End Class
