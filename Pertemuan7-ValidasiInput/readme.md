<div align="center">

# PERTEMUAN 07
### Validasi Input, Error Handling, dan Naming Convention (VB.NET)

</div>

---

## 1. Deskripsi Praktikum

Praktikum ini berfokus pada teknik perancangan aplikasi **Windows Forms (VB.NET)** yang andal (*robust*), rapi, dan tahan terhadap kesalahan (*fault-tolerant*) melalui penerapan tiga pilar utama:
1. **Standar Penamaan Kontrol (*Naming Convention*)**: Mengganti nama default (*misal: Button1, TextBox1*) menjadi pola terstruktur `prefix + NamaFungsi` untuk meningkatkan keterbacaan kode (*readability*).
2. **Penanganan Kesalahan (*Error Handling*)**: Memahami klasifikasi error (*Syntax*, *Runtime/Exception*, dan *Logic*) serta mengendalikan kegagalan eksternal menggunakan blok `Try...Catch...Finally` dan *Catch Bertingkat*.
3. **Validasi Input Terpadu**: Mencegah data salah diproses sejak awal menggunakan metode `TryParse`, fungsi validasi kustom, serta komponen visual `ErrorProvider` untuk memberikan umpan balik tanpa mengganggu pengguna (*non-intrusive feedback*).

Studi kasus yang dikembangkan adalah **Form Pendaftaran Mahasiswa** yang memvalidasi integritas data (NIM unik 9 digit, nama tidak kosong, angkatan dalam rentang tahun valid, format desimal IPK, dan pemilihan jurusan), serta mendukung ekspor data ke file CSV.

---

## 2. Arsitektur Komponen Visual (UI Controls)

Penerapan standar prefix penamaan kontrol pada form pendaftaran:

| Kontrol UI | Identitas `(Name)` | Properti Kunci | Deskripsi Fungsional |
| :--- | :--- | :--- | :--- |
| **Form** | `FrmPendaftaran` | `Text: "Pendaftaran Mahasiswa"` | Jendela utama form pendaftaran |
| **Label** | `lblJudul` | `Text: "Pendaftaran Mahasiswa"` | Label judul formulir |
| **Label** | `lblStatus` | `Text: "Siap."` | Menampilkan pesan status keberhasilan atau peringatan |
| **TextBox** | `txtNim` | `MaxLength: 9` | Input Nomor Induk Mahasiswa (tepat 9 digit angka) |
| **TextBox** | `txtNama` | `Text: ""` | Input nama lengkap mahasiswa (wajib diisi) |
| **ComboBox** | `cboJurusan` | `DropDownStyle: DropDownList` | Pilihan program studi / jurusan |
| **TextBox** | `txtAngkatan` | `Text: ""` | Input tahun angkatan (rentang 2015 s.d. tahun berjalan) |
| **TextBox** | `txtIpk` | `Text: ""` | Input nilai IPK (rentang 0,00 s.d. 4,00) |
| **Button** | `btnSimpan` | `Text: "Simpan"` | Memvalidasi seluruh masukan dan menambahkan data ke daftar |
| **Button** | `btnBersihkan` | `Text: "Bersihkan"` | Mengosongkan seluruh kolom input dan menghapus ikon error |
| **Button** | `btnEkspor` | `Text: "Ekspor ke CSV"` | Mengekspor daftar data mahasiswa ke file eksternal CSV |
| **ListBox** | `lstMahasiswa` | `FormattingEnabled: True` | Menampung dan menampilkan daftar pendaftar yang tersimpan |
| **ErrorProvider**| `errInput` | `BlinkStyle: NeverBlink` | Menampilkan ikon peringatan merah di samping kontrol yang tidak valid |

---

## 3. Logika Kode Program

### 3.1 `ModValidasi.vb`
Modul utilitas berisi fungsi-fungsi validasi murni yang dapat digunakan kembali (*reusable*):

```vb
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

### 3.2 `FrmPendaftaran.vb`
Kode antarmuka utama pendaftaran yang menerapkan validasi komprehensif dan ekspor file:

```vb
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

### 3.3 `FrmUjiKonversi.vb`
Form pengujian perbandingan perilaku konversi (`CInt`, `Val`, `TryParse`) dan penerapan *Catch Bertingkat*:

```vb
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

---

## 4. Pembahasan Detail Konsep Teknis

### 4.1 Standar Penamaan (*Naming Convention*)
* **Format Dasar**: `prefix` (3 huruf kecil) + `NamaFungsi` (PascalCase), tanpa spasi dan tidak diawali angka.
  * Contoh: `txtNama`, `btnSimpan`, `cboJurusan`, `lstMahasiswa`, `errInput`.
* **Prinsip Penting**:
  * Mengubah properti **`(Name)`** di jendela Properties (bukan properti `Text`).
  * Ganti nama kontrol **sebelum** melakukan *double-click* membuat event handler agar nama method tidak tertinggal sebagai `Button1_Click`.
  * Hindari pencampuran bahasa (konsisten gunakan Bahasa Indonesia atau Bahasa Inggris secara seragam).

### 4.2 Tiga Kategori Error dalam Pemrograman
1. **Syntax Error**: Kesalahan tata bahasa penulisan kode (ditandai garis bawah bergelombang merah oleh compiler). Program tidak dapat dikompilasi hingga kesalahan diperbaiki.
2. **Runtime Error (Exception)**: Kesalahan yang muncul saat program sedang berjalan (misal: membagi dengan nol, membuka file yang terkunci, atau konversi string non-angka). Jika tidak ditangani, program akan berhenti paksa (*crash*).
3. **Logic Error**: Program berjalan normal tanpa crash, namun menghasilkan keluaran kalkulasi atau alur logika yang keliru. Ditemukan melalui pengujian data (*testing*).

### 4.3 Analisis Bahaya Fungsi `Val()` vs Keunggulan `TryParse()`
* **Bahaya `Val()`**: `Val("12abc")` menghasilkan nilai `12` dan `Val("abc")` menghasilkan `0` tanpa memunculkan peringatan. Hal ini menyebabkan data yang cacat tersimpan diam-diam ke dalam sistem (*silent data corruption*).
* **Kelemahan `CInt()` / `Integer.Parse()`**: Langsung melempar `InvalidCastException` atau `FormatException` saat menerima string non-numerik, memicu crash jika tidak dibungkus `Try...Catch`.
* **Keunggulan `TryParse()`**: Mengembalikan nilai `Boolean` (`True` jika sukses, `False` jika gagal) dan mengisi variabel penampung melalui parameter `ByRef` tanpa membebani performa aplikasi dengan exception overhead.

### 4.4 Jebakan Karakter Desimal Budaya (*Culture / Localization*)
Di sistem operasi Windows dengan pengaturan regional Bahasa Indonesia, pemisah desimal standar adalah **koma (`,`)**, sedangkan sistem Inggris menggunakan **titik (`.`)**.
* `Decimal.TryParse("3.75")` di Windows Bahasa Indonesia dapat terbaca sebagai `375` (titik dianggap pemisah ribuan).
* **Solusi**: Normalisasi input menggunakan `.Replace(",", ".")` lalu baca dengan format seragam menggunakan `CultureInfo.InvariantCulture`.

### 4.5 Prinsip Penggunaan `Try...Catch...Finally`
* **Prinsip Utama**: *"Cegah dengan validasi apa yang bisa dicegah. Tangkap dengan Try...Catch apa yang tidak bisa dicegah."*
* **Kapan Menggunakan `Try...Catch`**:
  * Tepat digunakan pada operasi I/O di luar kendali langsung aplikasi: pembacaan/penulisan file disk, koneksi jaringan/API, dan query database.
  * **Tidak Tepat** digunakan untuk menggantikan validasi dasar input (misal: mengecek isian kosong cukup dengan `If Kosong(...)`, bukan membiarkannya melempar exception).
* **Catch Bertingkat**: Evaluasi exception dilakukan dari atas ke bawah. Blok yang lebih spesifik (`FormatException`, `DivideByZeroException`) harus diletakkan di atas, sedangkan blok `Exception` umum diletakkan paling bawah.
* **Blok `Finally`**: Blok yang **pasti dijalankan** dalam kondisi apa pun (baik ada error maupun berhasil). Sangat krusial untuk menutup resource (*cleanup*) seperti `penulis.Close()`.

### 4.6 Penggunaan Visual `ErrorProvider`
Komponen `ErrorProvider` menampilkan ikon merah peringatan di samping kontrol UI yang tidak valid:
* Menghindarkan pengguna dari gangguan tumpukan kotak dialog peringatan beruntun (*MessageBox overload*).
* Memungkinkan pengguna melihat seluruh kesalahan isian secara bersamaan dalam satu formulir.

---

## 5. Klasifikasi Komparasi Teknik Konversi dan Validasi

| Masukan (*Input*) | `CInt(teks)` | `Val(teks)` | `Integer.TryParse(...)` | Evaluasi Validitas |
| :--- | :--- | :--- | :--- | :--- |
| `"25"` | `25` (Sukses) | `25` (Sukses) | `True`, angka = `25` | Valid bulat |
| `"abc"` | `InvalidCastException` ❌ | `0` (Salah semantik) ⚠️ | `False` (Aman) | Tidak valid |
| `"12abc"` | `InvalidCastException` ❌ | `12` (Potong data) ⚠️ | `False` (Aman) | Tidak valid |
| `"3,5"` | `35` atau `4` (Tergantung OS) | `3` (Potong desimal) ⚠️ | `False` (Aman) | Bukan Integer |
| `"99999999999"` | `OverflowException` ❌ | `99999999999` (Double) | `False` (Aman) | Melebihi batas Int32 |
| `""` (Kosong) | `InvalidCastException` ❌ | `0` (Salah semantik) ⚠️ | `False` (Aman) | Tidak valid |

---

## 6. Kesimpulan

Praktikum Pertemuan 07 memberikan panduan penting dalam rekayasa perangkat lunak berbasis Windows Forms:

1. **Konsistensi Kode**: Penamaan kontrol sesuai prefix standar mempermudah pemahaman kode tim dan mempercepat proses penelusuran bug (*debugging*).
2. **Filosofi Pertahanan Berlapis**: Validasi input dilakukan sebelum pemrosesan logika untuk memastikan hanya data bersih yang masuk ke aliran komputasi atau database.
3. **Keandalan TryParse**: Menghindari penggunaan `Val()` dan memprioritaskan `TryParse` untuk menjamin keamanan tipe data tanpa melempar runtime exception.
4. **Pemisahan Peran Exception Handling**: `Try...Catch...Finally` difokuskan untuk menangani kegagalan lingkungan eksternal (seperti I/O file CSV), bukan sebagai pengganti logika percabangan kondisi input.
5. **Kualitas Pengalaman Pengguna (*UX*)**: Penggunaan `ErrorProvider` memberikan indikasi kesalahan yang informatif, estetis, dan tidak memblokir kenyamanan pengguna formulir.

