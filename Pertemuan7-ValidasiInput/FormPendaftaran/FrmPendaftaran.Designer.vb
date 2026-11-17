<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmPendaftaran
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
        components = New ComponentModel.Container()
        lblJudul = New Label()
        lblNim = New Label()
        lblNama = New Label()
        lblJurusan = New Label()
        lblAngkatan = New Label()
        lblIpk = New Label()
        txtNim = New TextBox()
        txtNama = New TextBox()
        txtAngkatan = New TextBox()
        txtIpk = New TextBox()
        cboJurusan = New ComboBox()
        btnSimpan = New Button()
        btnBersihkan = New Button()
        lblDaftar = New Label()
        lstMahasiswa = New ListBox()
        btnEkspor = New Button()
        lblStatus = New Label()
        errInput = New ErrorProvider(components)
        CType(errInput, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblJudul
        ' 
        lblJudul.AutoSize = True
        lblJudul.Font = New Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblJudul.Location = New Point(12, 9)
        lblJudul.Name = "lblJudul"
        lblJudul.Size = New Size(329, 38)
        lblJudul.TabIndex = 0
        lblJudul.Text = "Pendaftaran Mahasiswa"
        ' 
        ' lblNim
        ' 
        lblNim.AutoSize = True
        lblNim.Location = New Point(12, 79)
        lblNim.Name = "lblNim"
        lblNim.Size = New Size(46, 25)
        lblNim.TabIndex = 1
        lblNim.Text = "NIM"
        ' 
        ' lblNama
        ' 
        lblNama.AutoSize = True
        lblNama.Location = New Point(12, 124)
        lblNama.Name = "lblNama"
        lblNama.Size = New Size(59, 25)
        lblNama.TabIndex = 2
        lblNama.Text = "Nama"
        ' 
        ' lblJurusan
        ' 
        lblJurusan.AutoSize = True
        lblJurusan.Location = New Point(12, 169)
        lblJurusan.Name = "lblJurusan"
        lblJurusan.Size = New Size(71, 25)
        lblJurusan.TabIndex = 3
        lblJurusan.Text = "Jurusan"
        ' 
        ' lblAngkatan
        ' 
        lblAngkatan.AutoSize = True
        lblAngkatan.Location = New Point(12, 216)
        lblAngkatan.Name = "lblAngkatan"
        lblAngkatan.Size = New Size(88, 25)
        lblAngkatan.TabIndex = 4
        lblAngkatan.Text = "Angkatan"
        ' 
        ' lblIpk
        ' 
        lblIpk.AutoSize = True
        lblIpk.Location = New Point(12, 259)
        lblIpk.Name = "lblIpk"
        lblIpk.Size = New Size(37, 25)
        lblIpk.TabIndex = 5
        lblIpk.Text = "IPK"
        ' 
        ' txtNim
        ' 
        txtNim.Location = New Point(136, 76)
        txtNim.MaxLength = 9
        txtNim.Name = "txtNim"
        txtNim.Size = New Size(273, 31)
        txtNim.TabIndex = 6
        ' 
        ' txtNama
        ' 
        txtNama.Location = New Point(136, 121)
        txtNama.MaxLength = 100
        txtNama.Name = "txtNama"
        txtNama.Size = New Size(273, 31)
        txtNama.TabIndex = 7
        ' 
        ' txtAngkatan
        ' 
        txtAngkatan.Location = New Point(136, 213)
        txtAngkatan.MaxLength = 4
        txtAngkatan.Name = "txtAngkatan"
        txtAngkatan.Size = New Size(273, 31)
        txtAngkatan.TabIndex = 8
        ' 
        ' txtIpk
        ' 
        txtIpk.Location = New Point(136, 256)
        txtIpk.MaxLength = 4
        txtIpk.Name = "txtIpk"
        txtIpk.Size = New Size(273, 31)
        txtIpk.TabIndex = 9
        ' 
        ' cboJurusan
        ' 
        cboJurusan.DropDownStyle = ComboBoxStyle.DropDownList
        cboJurusan.FormattingEnabled = True
        cboJurusan.Items.AddRange(New Object() {"Teknik Informatika", "Sistem Informasi", "Manajemen Informatika", "Teknik Komputer"})
        cboJurusan.Location = New Point(136, 166)
        cboJurusan.Name = "cboJurusan"
        cboJurusan.Size = New Size(273, 33)
        cboJurusan.TabIndex = 10
        ' 
        ' btnSimpan
        ' 
        btnSimpan.Location = New Point(136, 312)
        btnSimpan.Name = "btnSimpan"
        btnSimpan.Size = New Size(112, 34)
        btnSimpan.TabIndex = 11
        btnSimpan.Text = "Simpan"
        btnSimpan.UseVisualStyleBackColor = True
        ' 
        ' btnBersihkan
        ' 
        btnBersihkan.CausesValidation = False
        btnBersihkan.Location = New Point(297, 312)
        btnBersihkan.Name = "btnBersihkan"
        btnBersihkan.Size = New Size(112, 34)
        btnBersihkan.TabIndex = 12
        btnBersihkan.Text = "Bersihkan"
        btnBersihkan.UseVisualStyleBackColor = True
        ' 
        ' lblDaftar
        ' 
        lblDaftar.AutoSize = True
        lblDaftar.Location = New Point(442, 41)
        lblDaftar.Name = "lblDaftar"
        lblDaftar.Size = New Size(134, 25)
        lblDaftar.TabIndex = 13
        lblDaftar.Text = "Data Tersimpan"
        ' 
        ' lstMahasiswa
        ' 
        lstMahasiswa.FormattingEnabled = True
        lstMahasiswa.HorizontalScrollbar = True
        lstMahasiswa.IntegralHeight = False
        lstMahasiswa.Location = New Point(452, 79)
        lstMahasiswa.Name = "lstMahasiswa"
        lstMahasiswa.Size = New Size(292, 267)
        lstMahasiswa.TabIndex = 14
        ' 
        ' btnEkspor
        ' 
        btnEkspor.CausesValidation = False
        btnEkspor.Location = New Point(452, 361)
        btnEkspor.Name = "btnEkspor"
        btnEkspor.Size = New Size(147, 34)
        btnEkspor.TabIndex = 15
        btnEkspor.Text = "Ekspor ke CSV"
        btnEkspor.UseVisualStyleBackColor = True
        ' 
        ' lblStatus
        ' 
        lblStatus.Location = New Point(8, 412)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New Size(736, 38)
        lblStatus.TabIndex = 16
        lblStatus.Text = "Siap."
        ' 
        ' errInput
        ' 
        errInput.ContainerControl = Me
        ' 
        ' FrmPendaftaran
        ' 
        AcceptButton = btnSimpan
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(780, 470)
        Controls.Add(lblStatus)
        Controls.Add(btnEkspor)
        Controls.Add(lstMahasiswa)
        Controls.Add(lblDaftar)
        Controls.Add(btnBersihkan)
        Controls.Add(btnSimpan)
        Controls.Add(cboJurusan)
        Controls.Add(txtIpk)
        Controls.Add(txtAngkatan)
        Controls.Add(txtNama)
        Controls.Add(txtNim)
        Controls.Add(lblIpk)
        Controls.Add(lblAngkatan)
        Controls.Add(lblJurusan)
        Controls.Add(lblNama)
        Controls.Add(lblNim)
        Controls.Add(lblJudul)
        FormBorderStyle = FormBorderStyle.FixedSingle
        MaximizeBox = False
        Name = "FrmPendaftaran"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Pendaftaran Mahasiswa"
        CType(errInput, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblJudul As Label
    Friend WithEvents lblNim As Label
    Friend WithEvents lblNama As Label
    Friend WithEvents lblJurusan As Label
    Friend WithEvents lblAngkatan As Label
    Friend WithEvents lblIpk As Label
    Friend WithEvents txtNim As TextBox
    Friend WithEvents txtNama As TextBox
    Friend WithEvents txtAngkatan As TextBox
    Friend WithEvents txtIpk As TextBox
    Friend WithEvents cboJurusan As ComboBox
    Friend WithEvents btnSimpan As Button
    Friend WithEvents btnBersihkan As Button
    Friend WithEvents lblDaftar As Label
    Friend WithEvents lstMahasiswa As ListBox
    Friend WithEvents btnEkspor As Button
    Friend WithEvents lblStatus As Label
    Friend WithEvents errInput As ErrorProvider
End Class
