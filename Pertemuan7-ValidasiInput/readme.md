# PERTEMUAN 7 — Validasi Input

### Penerapan Validasi Input, Penanganan Error, dan Naming Convention pada VB.NET

---

## 1. Gambaran Umum Praktikum

Kegiatan praktikum ini membahas cara membangun aplikasi **Windows Forms (VB.NET)** yang lebih terstruktur, mudah dipelihara, serta mampu menangani kondisi yang tidak diharapkan. Pembahasan mencakup tiga aspek utama berikut:

1. **Konvensi Penamaan Kontrol ( *Naming Convention* )**: Mengubah nama bawaan seperti `Button1` atau `TextBox1` menjadi format `prefix + NamaFungsi` agar fungsi setiap kontrol lebih mudah dikenali.
2. **Penanganan Error ( *Error Handling* )**: Mengenali error *Syntax*, *Runtime/Exception*, dan *Logic*, lalu menangani kesalahan saat program berjalan melalui `Try...Catch...Finally` serta beberapa blok `Catch`.
3. **Validasi Input**: Memeriksa data sebelum diproses dengan `TryParse`, fungsi validasi buatan sendiri, dan komponen `ErrorProvider` untuk menampilkan pemberitahuan kesalahan secara langsung.

Contoh penerapannya berupa **Formulir Pendaftaran Mahasiswa**. Form ini memeriksa NIM unik yang terdiri dari 9 digit, kelengkapan nama, tahun angkatan, nilai IPK dalam format desimal, dan pilihan jurusan. Data yang sudah dimasukkan juga dapat diekspor ke berkas CSV.

---

## 2. Susunan Komponen Antarmuka (Kontrol UI)

Berikut daftar kontrol yang digunakan pada form beserta nama dan kegunaannya:

| **Kontrol UI**    | **Identitas`(Name)`** | **Properti Kunci**              | **Deskripsi Fungsional**                                              |
| ----------------- | --------------------- | ------------------------------- | --------------------------------------------------------------------- |
| **Formulir**      | `FrmPendaftaran`      | `Text: "Pendaftaran Mahasiswa"` | Jendela utama aplikasi pendaftaran                                    |
| **Label**         | `lblJudul`            | `Text: "Pendaftaran Mahasiswa"` | Menampilkan judul formulir                                                           |
| **Label**         | `lblStatus`           | `Text: "Siap."`                 | Menampilkan informasi proses, keberhasilan, atau peringatan                 |
| **Kotak Teks**    | `txtNim`              | `MaxLength: 9`                  | Input Nomor Induk Mahasiswa (tepat 9 digit angka)                     |
| **Kotak Teks**    | `txtNama`             | `Text: ""`                      | Kolom nama lengkap yang harus diisi                               |
| **Kotak Kombo**   | `cboJurusan`          | `DropDownStyle: DropDownList`   | Pilihan program studi / jurusan                                       |
| **Kotak Teks**    | `txtAngkatan`         | `Text: ""`                      | Memasukkan tahun angkatan dari 2015 hingga tahun berjalan                 |
| **Kotak Teks**    | `txtIpk`              | `Text: ""`                      | Memasukkan IPK dengan nilai 0,00 sampai 4,00                                |
| **Tombol**        | `btnSimpan`           | `Text: "Simpan"`                | Memeriksa isian lalu menyimpan data ke daftar            |
| **Tombol**        | `btnBersihkan`        | `Text: "Bersihkan"`             | Menghapus isi formulir sekaligus tanda kesalahan             |
| **Tombol**        | `btnEkspor`           | `Text: "Ekspor ke CSV"`         | Menyimpan daftar mahasiswa dalam format CSV                    |
| **Kotak Daftar**  | `lstMahasiswa`        | `FormattingEnabled: True`       | Menampilkan data mahasiswa yang berhasil dimasukkan             |
| **ErrorProvider** | `errInput`            | `BlinkStyle: NeverBlink`        | Memberi indikator pada kontrol yang berisi input tidak valid |

---

## 3. Implementasi Logika Program

### 3.1`ModValidasi.vb`

Modul ini menyediakan beberapa fungsi pemeriksaan sederhana yang bisa dipanggil kembali dari bagian program lain:

```
Option Strict On
Imports System.Globalization

Public Module ModValidasi

    ' Memeriksa apakah string kosong atau hanya berisi spasi putih
    Public Function Kosong(teks As String) As Boolean
        Return teks.Trim() = ""
    End Function

    ' Memeriksa apakah teks memiliki panjang tepat dan hanya terdiri atas karakter digit (0-9)
    Public Function SemuaDigit(teks As String, panjang As Integer) As Boolean
        If teks.Length <> panjang Then Return False
        For Each karakter As Char In teks
            If karakter < "0"c OrElse karakter > "9"c Then Return False
        Next
        Return True
    End Function

    ' Mengonversi string ke Decimal secara aman dengan menerima pemisah koma maupun titik
    Public Function CobaDesimal(teks As String, ByRef hasil As Decimal) As Boolean
        Dim teksTitik As String = teks.Trim().Replace(",", ".")
        Return Decimal.TryParse(teksTitik, NumberStyles.AllowDecimalPoint,
                                CultureInfo.InvariantCulture, hasil)
    End Function

End Module
```

### 3.2`FrmPendaftaran.vb`

Bagian ini merupakan kode utama form pendaftaran, mulai dari pemeriksaan data hingga proses penyimpanan dan ekspor:

```
Option Strict On
Imports System.ComponentModel
Imports System.IO

Public Class FrmPendaftaran

    ' Koleksi lokal untuk verifikasi keunikan NIM (mencegah duplikasi data)
    Private ReadOnly daftarNim As New List(Of String)

    ' Lokasi tujuan ekspor file CSV di direktori dokumen pengguna
    Private ReadOnly lokasiEkspor As String =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "data-mahasiswa.csv")

    ' Memeriksa seluruh masukan formulir dan memasang ikon error jika tidak valid
    Private Function ValidasiInput() As Boolean
        errInput.Clear() ' Bersihkan tanda error sebelumnya
        Dim valid As Boolean = True

        ' 1. Validasi Wajib Isi: Nama
        If Kosong(txtNama.Text) Then
            errInput.SetError(txtNama, "Nama wajib diisi.")
            valid = False
        End If

        ' 2. Validasi Tipe Data & Rentang: Angkatan
        Dim angkatan As Integer
        If Not Integer.TryParse(txtAngkatan.Text, angkatan) OrElse
           angkatan < 2015 OrElse angkatan > Date.Today.Year Then
            errInput.SetError(txtAngkatan, "Angkatan harus angka 2015 s.d. tahun ini.")
            valid = False
        End If

        ' 3. Validasi Format & Keunikan: NIM
        If Not SemuaDigit(txtNim.Text, 9) Then
            errInput.SetError(txtNim, "NIM harus 9 digit angka.")
            valid = False
        ElseIf daftarNim.Contains(txtNim.Text) Then
            errInput.SetError(txtNim, "NIM sudah terdaftar.")
            valid = False
        End If

        ' 4. Validasi Pemilihan: ComboBox Jurusan
        If cboJurusan.SelectedIndex = -1 Then
            errInput.SetError(cboJurusan, "Pilih salah satu jurusan.")
            valid = False
        End If

        ' 5. Validasi Angka Pecahan & Rentang: IPK
        Dim ipk As Decimal
        If Not CobaDesimal(txtIpk.Text, ipk) OrElse ipk < 0D OrElse ipk > 4D Then
            errInput.SetError(txtIpk, "IPK harus angka 0,00 s.d. 4,00.")
            valid = False
        End If

        Return valid
    End Function

    ' Event Handler Tombol Simpan (Pola: Validasi Dulu, Keluar Jika Gagal)
    Private Sub btnSimpan_Click(sender As Object, e As EventArgs) Handles btnSimpan.Click
        If Not ValidasiInput() Then
            lblStatus.Text = "Periksa isian yang bertanda merah."
            Return
        End If

        ' Konversi data (terjamin berhasil karena telah lolos validasi)
        Dim ipk As Decimal
        CobaDesimal(txtIpk.Text, ipk)

        lstMahasiswa.Items.Add(txtNim.Text & " | " & txtNama.Text.Trim() & " | " &
                               cboJurusan.Text & " | " & txtAngkatan.Text & " | " & ipk.ToString("0.00"))
        daftarNim.Add(txtNim.Text)

        lblStatus.Text = "Data " & txtNama.Text.Trim() & " tersimpan."
        BersihkanForm()
    End Sub

    ' Validasi individual saat fokus keluar dari kontrol NIM
    Private Sub txtNim_Validating(sender As Object, e As CancelEventArgs) Handles txtNim.Validating
        If txtNim.Text = "" Then Return
        If SemuaDigit(txtNim.Text, 9) Then
            errInput.SetError(txtNim, "")
        Else
            errInput.SetError(txtNim, "NIM harus 9 digit angka.")
        End If
    End Sub

    ' Ekspor data ke file CSV dengan penanganan exception I/O
    Private Sub btnEkspor_Click(sender As Object, e As EventArgs) Handles btnEkspor.Click
        Dim penulis As StreamWriter = Nothing
        Try
            penulis = New StreamWriter(lokasiEkspor)
            For Each baris As Object In lstMahasiswa.Items
                penulis.WriteLine(baris.ToString().Replace(" | ", ";"))
            Next
            lblStatus.Text = "Data diekspor ke " & lokasiEkspor
        Catch ex As IOException
            MessageBox.Show("File sedang dipakai program lain atau tidak bisa ditulis." &
                            vbCrLf & ex.Message, "Ekspor Gagal")
        Finally
            If penulis IsNot Nothing Then penulis.Close() ' Menjamin file stream selalu ditutup
        End Try
    End Sub

    Private Sub BersihkanForm()
        txtNim.Clear()
        txtNama.Clear()
        txtAngkatan.Clear()
        txtIpk.Clear()
        cboJurusan.SelectedIndex = -1
        errInput.Clear()
        txtNim.Focus()
    End Sub

    Private Sub btnBersihkan_Click(sender As Object, e As EventArgs) Handles btnBersihkan.Click
        BersihkanForm()
        lblStatus.Text = "Form dibersihkan."
    End Sub

End Class
```

### 3.3`FrmUjiKonversi.vb`

Form pengujian berikut digunakan untuk membandingkan cara kerja konversi `CInt`, `Val`, dan `TryParse`, sekaligus mencoba penanganan beberapa jenis exception:

```
Option Strict On
Public Class FrmUjiKonversi

    Private Sub btnCInt_Click(sender As Object, e As EventArgs) Handles btnCInt.Click
        ' Melempar InvalidCastException jika input non-numerik (Program Crash tanpa penanganan)
        Dim angka As Integer = CInt(txtInput.Text)
        lblHasil.Text = "CInt berhasil: " & angka.ToString()
    End Sub

    Private Sub btnVal_Click(sender As Object, e As EventArgs) Handles btnVal.Click
        ' Tidak pernah error, namun menghasilkan angka parsial keliru ("12abc" -> 12)
        Dim angka As Double = Val(txtInput.Text)
        lblHasil.Text = "Val menghasilkan: " & angka.ToString()
    End Sub

    Private Sub btnTryParse_Click(sender As Object, e As EventArgs) Handles btnTryParse.Click
        ' Mengembalikan Boolean tanpa crash
        Dim angka As Integer
        If Integer.TryParse(txtInput.Text, angka) Then
            lblHasil.Text = "Valid. Nilai angka = " & angka.ToString()
        Else
            lblHasil.Text = "Bukan bilangan bulat yang valid. Tidak ada exception."
        End If
    End Sub

    Private Sub btnCatchBertingkat_Click(sender As Object, e As EventArgs) Handles btnCatchBertingkat.Click
        Try
            Dim angka As Integer = Integer.Parse(txtInput.Text)
            Dim hasil As Integer = 1000 \ angka
            lblHasil.Text = "1000 \ " & angka.ToString() & " = " & hasil.ToString()
        Catch ex As FormatException
            lblHasil.Text = "Format salah: input bukan bilangan bulat."
        Catch ex As OverflowException
            lblHasil.Text = "Angka terlalu besar untuk tipe Integer."
        Catch ex As DivideByZeroException
            lblHasil.Text = "Pembagian dengan nol tidak diperbolehkan."
        Catch ex As Exception
            lblHasil.Text = "Error lain: " & ex.Message
        End Try
    End Sub

End Class
```
