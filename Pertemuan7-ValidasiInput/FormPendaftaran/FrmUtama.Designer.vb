<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmUtama
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
        lblJudul = New Label()
        btnUjiKonversi = New Button()
        btnPendaftaran = New Button()
        SuspendLayout()
        ' 
        ' lblJudul
        ' 
        lblJudul.AutoSize = True
        lblJudul.Font = New Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblJudul.Location = New Point(49, 29)
        lblJudul.Name = "lblJudul"
        lblJudul.Size = New Size(289, 38)
        lblJudul.TabIndex = 0
        lblJudul.Text = "Contoh Pertemuan 7"
        ' 
        ' btnUjiKonversi
        ' 
        btnUjiKonversi.Location = New Point(25, 79)
        btnUjiKonversi.Name = "btnUjiKonversi"
        btnUjiKonversi.Size = New Size(347, 46)
        btnUjiKonversi.TabIndex = 1
        btnUjiKonversi.Text = "Uji Konversi: CInt, Val, TryParse, Try...Catch"
        btnUjiKonversi.UseVisualStyleBackColor = True
        ' 
        ' btnPendaftaran
        ' 
        btnPendaftaran.Location = New Point(25, 145)
        btnPendaftaran.Name = "btnPendaftaran"
        btnPendaftaran.Size = New Size(347, 46)
        btnPendaftaran.TabIndex = 2
        btnPendaftaran.Text = "Form Pendaftaran Mahasiswa"
        btnPendaftaran.UseVisualStyleBackColor = True
        ' 
        ' FrmUtama
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(420, 220)
        Controls.Add(btnPendaftaran)
        Controls.Add(btnUjiKonversi)
        Controls.Add(lblJudul)
        FormBorderStyle = FormBorderStyle.FixedSingle
        MaximizeBox = False
        Name = "FrmUtama"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Pertemuan 7 - Validasi & Error Handling"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblJudul As Label
    Friend WithEvents btnUjiKonversi As Button
    Friend WithEvents btnPendaftaran As Button

End Class
