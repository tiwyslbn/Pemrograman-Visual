PERTEMUAN 07
Implementasi Validasi Data, Penanganan Kesalahan, dan Standar Penamaan Kontrol pada VB.NET
1. Pendahuluan

Pada pertemuan ketujuh ini, praktikum membahas bagaimana membangun aplikasi Windows Forms menggunakan VB.NET dengan memperhatikan ketepatan data, keamanan proses, dan keteraturan penulisan kode.

Dalam pembuatan aplikasi, kesalahan input maupun kesalahan saat program dijalankan merupakan hal yang sering terjadi. Oleh karena itu, diperlukan mekanisme untuk memastikan data yang dimasukkan pengguna sesuai dengan ketentuan sebelum diproses lebih lanjut.

Terdapat tiga materi utama yang dipelajari dalam praktikum ini:

Naming Convention, yaitu penggunaan aturan penamaan pada komponen form agar kode lebih mudah dipahami dan dikelola.
Error Handling, yaitu teknik menangani kesalahan yang muncul selama program berjalan agar aplikasi tidak langsung berhenti.
Input Validation, yaitu proses pemeriksaan data pengguna sebelum data disimpan atau digunakan.

Sebagai penerapan dari ketiga materi tersebut, dibuat aplikasi pendaftaran mahasiswa yang memiliki fitur pemeriksaan NIM, nama, jurusan, tahun angkatan, dan IPK. Aplikasi juga dilengkapi fasilitas penyimpanan data sementara serta ekspor ke format CSV.

2. Persiapan dan Perancangan Form

Sebelum menuliskan program, terlebih dahulu dilakukan penyusunan tampilan menggunakan Windows Forms Designer.

Form utama dirancang dengan beberapa komponen yang memiliki tugas masing-masing. Setiap komponen diberikan nama khusus agar tidak menggunakan nama bawaan Visual Studio.

2.1 Daftar Komponen
Komponen	Nama Kontrol	Fungsi
Form	FrmPendaftaran	Menjadi halaman utama aplikasi
Label	lblJudul	Menampilkan judul aplikasi
Label	lblStatus	Memberikan informasi hasil proses
TextBox	txtNim	Memasukkan nomor induk mahasiswa
TextBox	txtNama	Mengisi nama lengkap
ComboBox	cboJurusan	Menentukan program studi
TextBox	txtAngkatan	Menginput tahun masuk mahasiswa
TextBox	txtIpk	Menginput nilai IPK
Button	btnSimpan	Menambahkan data mahasiswa
Button	btnBersihkan	Menghapus seluruh input
Button	btnEkspor	Menyimpan data ke CSV
ListBox	lstMahasiswa	Menampilkan data yang telah dimasukkan
ErrorProvider	errInput	Memberikan tanda pada input yang salah
2.2 Pengaturan Properti Kontrol

Beberapa properti perlu disesuaikan agar komponen bekerja sesuai kebutuhan:

FrmPendaftaran: judul form diatur menjadi Pendaftaran Mahasiswa.
txtNim: menggunakan MaxLength = 9 untuk membatasi panjang input.
cboJurusan: menggunakan DropDownStyle = DropDownList agar pengguna hanya dapat memilih pilihan yang tersedia.
lstMahasiswa: mengaktifkan FormattingEnabled.
errInput: menggunakan BlinkStyle = NeverBlink agar ikon kesalahan tidak berkedip.

Pada ComboBox, pilihan jurusan dapat ditambahkan melalui Properties bagian Items, misalnya Teknik Informatika, Sistem Informasi, dan Manajemen Informatika.

3. Implementasi Program
3.1 Pembuatan Modul Pemeriksaan Data

File ModValidasi.vb digunakan untuk menyimpan fungsi pemeriksaan data yang dapat dipanggil dari berbagai bagian program.

Dengan memisahkan fungsi validasi ke dalam modul, kode utama form menjadi lebih sederhana dan tidak perlu menuliskan pemeriksaan yang sama berulang kali.

vb
Option Strict On
Imports System.Globalization

Public Module ModValidasi

    ' Memastikan input memiliki isi
    Public Function CekKosong(teks As String) As Boolean
        Return String.IsNullOrWhiteSpace(teks)
    End Function

    ' Memastikan input hanya berisi angka sesuai jumlah digit
    Public Function CekDigit(teks As String, jumlah As Integer) As Boolean
        If teks.Length <> jumlah Then Return False

        For Each karakter As Char In teks
            If Not Char.IsDigit(karakter) OrElse
               karakter < "0"c OrElse karakter > "9"c Then
                Return False
            End If
        Next

        Return True
    End Function

    ' Membaca nilai desimal dengan format koma atau titik
    Public Function ParseDesimal(teks As String,
                                 ByRef nilai As Decimal) As Boolean

        Dim input As String = teks.Trim().Replace(",", ".")

        Return Decimal.TryParse(
            input,
            NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,
            nilai
        )
    End Function

End Module


Penjelasan:

CekKosong() digunakan untuk memeriksa apakah pengguna belum mengisi suatu kolom.
CekDigit() memastikan panjang dan karakter input NIM sesuai ketentuan.
ParseDesimal() menangani konversi IPK agar dapat membaca angka dengan tanda koma maupun titik.

Penggunaan fungsi terpisah membuat proses pemeriksaan lebih terorganisasi dan mudah digunakan kembali.

3.2 Program Form Pendaftaran Mahasiswa

File FrmPendaftaran.vb menjadi pusat pengolahan data. Pada bagian ini diterapkan pemeriksaan input, pengecekan NIM ganda, penyimpanan ke ListBox, dan penulisan file CSV.

vb
Option Strict On
Imports System.IO
Imports System.ComponentModel

Public Class FrmPendaftaran

    Private ReadOnly dataNim As New List(Of String)

    Private ReadOnly fileTujuan As String =
        Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments),
            "DataPendaftaran.csv"
        )

    ' Pemeriksaan semua kolom sebelum data diproses
    Private Function PeriksaForm() As Boolean

        errInput.Clear()
        Dim status As Boolean = True

        If CekKosong(txtNama.Text) Then
            errInput.SetError(txtNama, "Nama belum diisi.")
            status = False
        End If

        Dim tahun As Integer

        If Not Integer.TryParse(txtAngkatan.Text, tahun) OrElse
           tahun < 2015 OrElse tahun > Date.Today.Year Then

            errInput.SetError(
                txtAngkatan,
                "Masukkan tahun angkatan yang sesuai."
            )
            status = False
        End If

        If Not CekDigit(txtNim.Text, 9) Then

            errInput.SetError(txtNim, "NIM wajib terdiri dari 9 digit.")

            status = False

        ElseIf dataNim.Contains(txtNim.Text) Then

            errInput.SetError(txtNim, "NIM tersebut sudah digunakan.")
            status = False

        End If

        If cboJurusan.SelectedIndex < 0 Then
            errInput.SetError(cboJurusan, "Jurusan belum dipilih.")
            status = False
        End If

        Dim nilaiIpk As Decimal

        If Not ParseDesimal(txtIpk.Text, nilaiIpk) OrElse
           nilaiIpk < 0D OrElse nilaiIpk > 4D Then

            errInput.SetError(txtIpk, "IPK harus berada antara 0 sampai 4.")
            status = False

        End If

        Return status

    End Function

    ' Menyimpan data setelah seluruh pemeriksaan berhasil
    Private Sub btnSimpan_Click(sender As Object,
                                e As EventArgs) Handles btnSimpan.Click

        If Not PeriksaForm() Then
            lblStatus.Text = "Masih terdapat data yang belum sesuai."
            Return
        End If

        Dim ipk As Decimal
        ParseDesimal(txtIpk.Text, ipk)

        Dim data As String =
            txtNim.Text & " | " &
            txtNama.Text.Trim() & " | " &
            cboJurusan.Text & " | " &
            txtAngkatan.Text & " | " &
            ipk.ToString("0.00")

        lstMahasiswa.Items.Add(data)
        dataNim.Add(txtNim.Text)

        lblStatus.Text = "Pendaftaran berhasil disimpan."

        ResetInput()

    End Sub

    ' Pemeriksaan NIM ketika pengguna meninggalkan kolom
    Private Sub txtNim_Validating(
        sender As Object,
        e As CancelEventArgs
    ) Handles txtNim.Validating

        If String.IsNullOrWhiteSpace(txtNim.Text) Then Return

        If Not CekDigit(txtNim.Text, 9) Then
            errInput.SetError(txtNim, "Format NIM tidak sesuai.")
        ElseIf dataNim.Contains(txtNim.Text) Then
            errInput.SetError(txtNim, "NIM sudah terdaftar.")
        Else
            errInput.SetError(txtNim, "")
        End If

    End Sub

    ' Menuliskan data ke dalam file CSV
    Private Sub btnEkspor_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnEkspor.Click

        If lstMahasiswa.Items.Count = 0 Then
            lblStatus.Text = "Belum ada data untuk diekspor."
            Return
        End If

        Try

            Using penulis As New StreamWriter(fileTujuan)

                For Each item As Object In lstMahasiswa.Items
                    penulis.WriteLine(
                        item.ToString().Replace(" | ", ";")
                    )
                Next

            End Using

            lblStatus.Text = "Ekspor data berhasil dilakukan."

        Catch ex As UnauthorizedAccessException

            MessageBox.Show(
                "Aplikasi tidak memiliki izin menulis file."
            )

        Catch ex As IOException

            MessageBox.Show(
                "Terjadi masalah ketika menyimpan file: " & ex.Message
            )

        Finally

            ' Bagian ini dijalankan setelah proses ekspor selesai

        End Try

    End Sub

    ' Mengembalikan seluruh komponen ke kondisi awal
    Private Sub ResetInput()

        txtNim.Clear()
        txtNama.Clear()
        txtAngkatan.Clear()
        txtIpk.Clear()

        cboJurusan.SelectedIndex = -1

        errInput.Clear()
        txtNim.Focus()

    End Sub

    Private Sub btnBersihkan_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnBersihkan.Click

        ResetInput()
        lblStatus.Text = "Seluruh kolom telah dikosongkan."

    End Sub

End Class


Penjelasan alur program:

Pengguna mengisi informasi mahasiswa pada form.
Ketika tombol Simpan ditekan, fungsi PeriksaForm() memeriksa seluruh kolom.
Jika ditemukan kesalahan, ErrorProvider akan memberikan tanda pada bagian yang bermasalah.
Jika seluruh data benar, informasi dimasukkan ke ListBox.
NIM yang berhasil disimpan dimasukkan ke dalam List agar tidak dapat digunakan kembali.
Tombol Ekspor akan menuliskan seluruh data ke file CSV di folder Documents.

Penggunaan Using pada proses penulisan file membantu memastikan StreamWriter ditutup secara otomatis setelah proses selesai, termasuk ketika terjadi exception.

3.3 Form Pengujian Konversi Data

Selain aplikasi pendaftaran, dibuat form tambahan bernama FrmUjiKonversi.vb untuk mengetahui perbedaan beberapa metode konversi angka.

Komponen yang dibutuhkan:

txtInput: tempat memasukkan teks.
lblHasil: menampilkan hasil pengujian.
btnCInt: menguji fungsi CInt.
btnVal: menguji fungsi Val.
btnTryParse: menguji Integer.TryParse.
btnCatchBertingkat: menguji beberapa jenis exception.

Berikut contoh implementasinya:

vb
Option Strict On

Public Class FrmUjiKonversi

    Private Sub btnCInt_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnCInt.Click

        Try
            Dim angka As Integer = CInt(txtInput.Text)
            lblHasil.Text = "Hasil konversi: " & angka.ToString()

        Catch ex As Exception
            lblHasil.Text = "Konversi gagal: " & ex.Message
        End Try

    End Sub

    Private Sub btnVal_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnVal.Click

        Dim angka As Double = Val(txtInput.Text)

        lblHasil.Text = "Nilai dari Val: " & angka.ToString()

    End Sub

    Private Sub btnTryParse_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnTryParse.Click

        Dim angka As Integer

        If Integer.TryParse(txtInput.Text, angka) Then
            lblHasil.Text = "Input valid, hasil: " & angka.ToString()
        Else
            lblHasil.Text = "Input tidak dapat dikonversi menjadi Integer."
        End If

    End Sub

    Private Sub btnCatchBertingkat_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnCatchBertingkat.Click

        Try

            Dim angka As Integer = Integer.Parse(txtInput.Text)
            Dim hasil As Integer = 1000 \ angka

            lblHasil.Text = "Hasil pembagian: " & hasil.ToString()

        Catch ex As FormatException

            lblHasil.Text = "Kesalahan: format input tidak sesuai."

        Catch ex As OverflowException

            lblHasil.Text = "Kesalahan: nilai melebihi kapasitas Integer."

        Catch ex As DivideByZeroException

            lblHasil.Text = "Kesalahan: angka tidak boleh nol."

        Catch ex As Exception

            lblHasil.Text = "Kesalahan lainnya: " & ex.Message

        End Try

    End Sub

End Class


Catatan: Pada contoh ini, CInt dibungkus dengan Try...Catch agar kesalahan konversi dapat ditampilkan tanpa menghentikan aplikasi.

4. Pemahaman Materi Praktikum
4.1 Pentingnya Naming Convention

Naming Convention merupakan aturan pemberian nama pada variabel, fungsi, maupun komponen antarmuka.

Dalam VB.NET, nama kontrol yang masih menggunakan nama bawaan seperti Button1 atau TextBox1 dapat menyulitkan proses pembacaan kode ketika jumlah komponen semakin banyak.

Oleh sebab itu, digunakan prefix yang menunjukkan jenis kontrol.

Contohnya:

Prefix	Jenis Kontrol	Contoh
txt	TextBox	txtNama
btn	Button	btnSimpan
lbl	Label	lblStatus
cbo	ComboBox	cboJurusan
lst	ListBox	lstMahasiswa
err	ErrorProvider	errInput
frm	Form	FrmPendaftaran

Dengan sistem tersebut, programmer dapat mengetahui fungsi sebuah komponen hanya dengan melihat namanya.

4.2 Mengenali Jenis Kesalahan Program

Kesalahan dalam pemrograman dapat dikelompokkan menjadi tiga jenis.

A. Syntax Error

Kesalahan yang terjadi akibat penulisan kode tidak sesuai aturan bahasa pemrograman.

Contohnya:

Kurang menuliskan tanda kurung.
Salah menuliskan nama keyword.
Tidak menutup blok If atau Sub.

Kesalahan ini biasanya diketahui ketika proses kompilasi dilakukan.

B. Runtime Error

Kesalahan yang terjadi ketika aplikasi sedang dijalankan.

Contohnya:

Melakukan pembagian dengan angka nol.
Membaca file yang tidak tersedia.
Mengonversi teks yang tidak sesuai menjadi angka.

Kesalahan ini dapat ditangani dengan menggunakan Try...Catch.

C. Logic Error

Kesalahan yang tidak menyebabkan program berhenti, tetapi menghasilkan keluaran yang tidak sesuai harapan.

Misalnya, program menghitung nilai IPK dengan rumus yang salah sehingga hasil akhirnya tidak akurat.

Untuk menemukan kesalahan jenis ini, diperlukan pengujian terhadap berbagai kondisi input.

4.3 Perbandingan Metode Konversi

VB.NET menyediakan beberapa metode untuk mengubah teks menjadi angka. Masing-masing memiliki karakteristik berbeda.

| Metode | Karakter
