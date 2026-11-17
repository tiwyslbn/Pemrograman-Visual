# Laporan Praktikum Pemrograman Visual
### Pertemuan 08 — Validasi Input

| | |
|---|---|
| **Nama** | Tiwy Lamberkat Silaban |
| **NIM** | 241712001 |
| **Kelas** | KOM A1 |
| **Mata Kuliah** | Praktikum Pemrograman Visual |
| **Pertemuan Ke-** | 8 |

## 1. Gambaran Umum Praktikum

Kegiatan praktikum ini membahas penggunaan kontrol **DataGridView** pada aplikasi **Windows Forms (VB.NET)** untuk mengelola dan menyajikan informasi dalam bentuk tabel yang terdiri atas beberapa baris dan kolom.

Berbeda dengan `ListBox` yang menampilkan data secara berurutan, `DataGridView` dapat menyusun informasi berdasarkan atribut masing-masing sehingga tampilannya lebih teratur seperti lembar kerja (*spreadsheet*).

Materi yang dipelajari meliputi:

1. **Unbound Mode (Pengisian Manual):** Memasukkan data dari Array 2 Dimensi ke `DataGridView` menggunakan perulangan bersarang (*nested loop*).
2. **Bound Mode (Data Binding):** Menampilkan kumpulan objek melalui `List(Of T)` serta tabel sementara `DataTable`.
3. **Penggunaan `BindingSource`:** Memahami komponen penghubung sumber data dan tampilan grid untuk membantu sinkronisasi, navigasi, pengurutan, serta penyaringan data.
4. **Pengaturan Kolom:** Mengubah judul header, posisi teks, format angka desimal (`N2`), dan ukuran kolom agar menyesuaikan area tampilan.
5. **Fitur Pencarian:** Menerapkan pencarian dengan `BindingSource.Filter` pada `DataTable` dan perulangan `For Each` pada koleksi `List(Of T)`.

---

## 2. Susunan Komponen Antarmuka (UI Controls)

Praktikum terdiri dari beberapa form yang dapat dibuka melalui form navigasi utama, yaitu `FrmUtama`.

| Kontrol UI | Nama Kontrol | Properti Utama | Kegunaan |
|---|---|---|---|
| **DataGridView** | `dgvMahasiswa` | `AutoSizeColumnsMode: Fill` | Menampilkan informasi mahasiswa dalam bentuk tabel. |
| **BindingSource** | `bsMahasiswa` | `Component Tray` | Menghubungkan sumber data dengan DataGridView. |
| **TextBox** | `txtCari` | `Text: ""` | Mencari mahasiswa berdasarkan NIM atau nama secara langsung. |
| **TextBox** | `txtNIM` | `MaxLength: 9` | Menginput nomor induk mahasiswa. |
| **TextBox** | `txtNama` | `Text: ""` | Mengisi nama lengkap mahasiswa. |
| **ComboBox** | `cboJurusan` | `DropDownStyle: DropDownList` | Memilih jurusan atau program studi. |
| **TextBox** | `txtAngkatan` | `Text: ""` | Memasukkan tahun angkatan mahasiswa. |
| **TextBox** | `txtIpk` | `Text: ""` | Memasukkan nilai IPK. |
| **Button** | `btnTampilkan` | `Text: "Tampilkan"` | Menampilkan data dari sumber ke tabel. |
| **Button** | `btnTambah` | `Text: "Tambah"` | Menyimpan objek mahasiswa baru ke dalam koleksi. |
| **Button** | `btnRapikan` | `Text: "Rapikan Kolom"` | Mengatur tampilan dan format kolom tabel. |
| **Label** | `lblJumlah` | `Text: "Jumlah data: ..."` | Memberikan informasi jumlah data yang sedang ditampilkan. |
| **ErrorProvider** | `errInput` | `Component Tray` | Memberi tanda apabila terdapat kesalahan pada input. |

---

## 3. Implementasi Logika Program

### 3.1 Pembuatan Model Data (`Mahasiswa.vb`)

Class `Mahasiswa` berfungsi sebagai representasi informasi seorang mahasiswa. Setiap objek yang dibuat dari class ini akan mewakili satu baris data pada `DataGridView`.

Properti di dalam class menjadi dasar pembentukan kolom pada tampilan tabel.

```vb
Option Strict On

Public Class Mahasiswa

    Public Property Nim As String
    Public Property Nama As String
    Public Property Jurusan As String
    Public Property Angkatan As Integer
    Public Property Ipk As Decimal

    Public Sub New(nim As String, nama As String, jurusan As String, angkatan As Integer, ipk As Decimal)
        Me.Nim = nim
        Me.Nama = nama
        Me.Jurusan = jurusan
        Me.Angkatan = angkatan
        Me.Ipk = ipk
    End Sub

End Class
```

### 3.2 Menampilkan Array 2 Dimensi Secara Manual (`FrmDariArray.vb`)

Pada bagian ini, data mahasiswa disimpan dalam Array 2 Dimensi. Selanjutnya, setiap elemen dimasukkan ke `DataGridView` menggunakan dua perulangan.

Cara tersebut termasuk **Unbound Mode**, karena data diisikan secara langsung tanpa menghubungkan grid ke `DataSource`.

```vb
Option Strict On

Public Class FrmDariArray

    ' Indeks pertama menunjukkan baris, sedangkan indeks kedua menunjukkan kolom
    Private ReadOnly dataMahasiswa(,) As String = {
        {"231712001", "Andi Pratama", "Teknik Informatika", "2023", "3.45"},
        {"231712002", "Sari Wulandari", "Sistem Informasi", "2023", "3.8"},
        {"221712015", "Budi Santoso", "Teknik Informatika", "2022", "3.12"},
        {"241712007", "Dimas Siregar", "Teknik Komputer", "2024", "2.95"}
    }

    Private Sub btnTampilkan_Click(sender As Object, e As EventArgs) Handles btnTampilkan.Click
        dgvMahasiswa.Rows.Clear() ' Menghindari data tampil dua kali

        For i As Integer = 0 To dataMahasiswa.GetUpperBound(0)
            Dim indeksBaris As Integer = dgvMahasiswa.Rows.Add()

            For j As Integer = 0 To dataMahasiswa.GetUpperBound(1)
                dgvMahasiswa.Rows(indeksBaris).Cells(j).Value = dataMahasiswa(i, j)
            Next
        Next

        lblJumlah.Text = "Jumlah data: " & dgvMahasiswa.Rows.Count.ToString()
    End Sub

    Private Sub btnKosongkan_Click(sender As Object, e As EventArgs) Handles btnKosongkan.Click
        dgvMahasiswa.Rows.Clear()
        lblJumlah.Text = "Jumlah data: 0"
    End Sub

End Class
```

### 3.3 Menampilkan Koleksi Menggunakan `List(Of Mahasiswa)` (`FrmDariList.vb`)

Metode berikut memanfaatkan kumpulan objek bertipe `Mahasiswa`. Data disimpan ke dalam `List(Of Mahasiswa)`, lalu ditampilkan melalui bantuan komponen `BindingSource`.

Dengan pendekatan ini, pengelolaan koleksi dan penghubungannya ke tampilan tabel menjadi lebih terstruktur.

```vb
Option Strict On

Public Class FrmDariList

    Private ReadOnly daftarMahasiswa As New List(Of Mahasiswa)

    Private Sub FrmDariList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        daftarMahasiswa.Add(New Mahasiswa("231712001", "Andi Pratama", "Teknik Informatika", 2023, 3.45D))
        daftarMahasiswa.Add(New Mahasiswa("231712002", "Sari Wulandari", "Sistem Informasi", 2023, 3.8D))

        ' Menghubungkan koleksi dengan DataGridView
        bsMahasiswa.DataSource = daftarMahasiswa
        dgvMahasiswa.DataSource = bsMahasiswa

        lblJumlah.Text = "Jumlah data: " & bsMahasiswa.Count.ToString()
    End Sub

    Private Sub btnTambah_Click(sender As Object, e As EventArgs) Handles btnTambah.Click
        If Not ValidasiInput() Then Return

        Dim ipk As Decimal
        CobaDesimal(txtIpk.Text, ipk)

        Dim baru As New Mahasiswa(txtNIM.Text, txtNama.Text.Trim(), cboJurusan.Text,
                                  Integer.Parse(txtAngkatan.Text), ipk)

        ' Memasukkan objek baru melalui BindingSource
        bsMahasiswa.Add(baru)

        lblJumlah.Text = "Jumlah data: " & bsMahasiswa.Count.ToString()
        BersihkanInput()
    End Sub

End Class
```

### 3.4 Penggunaan `DataTable` dan Fitur Pencarian (`FrmDataTable.vb`)

`DataTable` dipakai untuk membuat tabel sementara di dalam memori. Struktur tabel disusun dengan menentukan nama kolom dan tipe data untuk setiap kolom.

Setelah tabel terbentuk, sumber data dihubungkan ke `DataGridView` melalui `BindingSource`. Proses pencarian kemudian dapat memanfaatkan properti `Filter`.

```vb
Option Strict On

Public Class FrmDataTable

    Private tabelMahasiswa As DataTable

    Private Function BuatTabelMahasiswa() As DataTable
        Dim tabel As New DataTable("Mahasiswa")

        tabel.Columns.Add("Nim", GetType(String))
        tabel.Columns.Add("Nama", GetType(String))
        tabel.Columns.Add("Jurusan", GetType(String))
        tabel.Columns.Add("Angkatan", GetType(Integer))
        tabel.Columns.Add("Ipk", GetType(Decimal))

        tabel.Rows.Add("231712001", "Andi Pratama", "Teknik Informatika", 2023, 3.45D)
        tabel.Rows.Add("231712002", "Sari Wulandari", "Sistem Informasi", 2023, 3.8D)

        Return tabel
    End Function

    Private Sub FrmDataTable_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tabelMahasiswa = BuatTabelMahasiswa()

        bsMahasiswa.DataSource = tabelMahasiswa
        dgvMahasiswa.DataSource = bsMahasiswa

        PerbaruiJumlah()
    End Sub

    ' Mengatur tampilan kolom setelah sumber data terhubung
    Private Sub AturKolom()
        dgvMahasiswa.Columns("Nim").HeaderText = "NIM"
        dgvMahasiswa.Columns("Ipk").HeaderText = "IPK"

        dgvMahasiswa.Columns("Ipk").DefaultCellStyle.Format = "N2"
        dgvMahasiswa.Columns("Ipk").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        dgvMahasiswa.Columns("Angkatan").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvMahasiswa.Columns("Nama").FillWeight = 200

        dgvMahasiswa.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    ' Melakukan penyaringan setiap kali isi kotak pencarian berubah
    Private Sub txtCari_TextChanged(sender As Object, e As EventArgs) Handles txtCari.TextChanged
        Dim kata As String = AmankanTeksFilter(txtCari.Text.Trim())

        If kata = "" Then
            bsMahasiswa.RemoveFilter()
        Else
            bsMahasiswa.Filter = "Nama LIKE '%" & kata & "%' OR Nim LIKE '%" & kata & "%'"
        End If

        PerbaruiJumlah()
    End Sub

End Class
```

### 3.5 Mengamankan Karakter pada Pencarian (`ModPencarian.vb`)

Input pencarian dapat mengandung karakter khusus, misalnya tanda kutip satu, persen, atau kurung siku. Karakter tersebut perlu ditangani supaya tidak mengganggu susunan ekspresi filter.

Fungsi `AmankanTeksFilter` berikut memeriksa karakter satu per satu, kemudian menyesuaikan karakter tertentu sebelum digunakan dalam proses pencarian.

```vb
Option Strict On

Public Module ModPencarian

    Public Function AmankanTeksFilter(teks As String) As String
        Dim hasil As String = ""

        For Each karakter As Char In teks
            Select Case karakter
                Case "'"c
                    hasil &= "''"

                Case "["c, "]"c, "*"c, "%"
                    hasil &= "[" & karakter & "]"

                Case Else
                    hasil &= karakter
            End Select
        Next

        Return hasil
    End Function

End Module
```

---

## 4. Penjelasan Konsep dan Penerapan

### 4.1 Perbedaan Unbound Mode dan Bound Mode

`DataGridView` menyediakan dua pendekatan untuk menampilkan data:

**A. Unbound Mode (`Rows.Add`)**

- Kolom tabel disiapkan terlebih dahulu melalui Designer atau kode program.
- Nilai dimasukkan secara langsung ke sel dengan menentukan posisi baris dan kolom.
- Proses pengisian umumnya menggunakan perulangan.
- Metode ini cocok untuk data sederhana dengan jumlah terbatas.

**B. Bound Mode (`DataSource`)**

- Data diambil dari sumber yang telah dihubungkan ke grid.
- Kolom dapat dibentuk berdasarkan properti objek atau struktur `DataTable`.
- Pengisian data lebih praktis karena tidak perlu memasukkan nilai ke setiap sel secara manual.

> [!WARNING]
> **Jangan mencampurkan kedua mode pada DataGridView yang sama.** Jika grid sudah menggunakan `DataSource`, pemanggilan `Rows.Add()` dapat menyebabkan `InvalidOperationException`.

### 4.2 Keterbatasan Array 2 Dimensi Dibandingkan Koleksi Objek

Array 2 Dimensi cukup sederhana untuk menyimpan informasi berbentuk tabel. Meskipun demikian, penggunaannya memiliki beberapa batasan:

1. **Keseragaman Tipe Data:** Pada `Array(,) As String`, seluruh elemen tersimpan sebagai teks. Karena itu, nilai IPK perlu dikonversi terlebih dahulu sebelum digunakan sebagai angka.
2. **Ukuran yang Kurang Fleksibel:** Ukuran array ditentukan sejak awal. Penambahan baris tidak semudah menambahkan elemen pada koleksi dinamis. `ReDim Preserve` di VB.NET juga memiliki batasan dalam perubahan dimensi.
3. **Tidak Ada Sinkronisasi Otomatis:** Perubahan isi array tidak langsung tercermin pada tampilan grid.
4. **Risiko Kesalahan Indeks:** Pengambilan nilai seperti `data(i, 3)` dapat membingungkan ketika jumlah kolom bertambah atau urutannya berubah.

### 4.3 Konsep Objek: Satu Objek Mewakili Satu Baris

Dengan membuat class `Mahasiswa`, data dapat dikelompokkan berdasarkan identitas dan atribut masing-masing.

- Setiap objek yang dibuat melalui `New Mahasiswa(...)` menyimpan informasi seorang mahasiswa.
- Properti seperti `Nim`, `Nama`, `Jurusan`, dan lainnya menjadi dasar pembentukan kolom pada `DataGridView`.
- Tipe data setiap atribut dapat ditentukan sesuai kebutuhan, misalnya `Decimal` untuk IPK dan `Integer` untuk angkatan.
- Struktur program menjadi lebih mudah dibaca dan dikembangkan.

### 4.4 Fungsi `BindingSource` sebagai Penghubung

`BindingSource` berperan sebagai perantara antara tempat data disimpan dan komponen visual yang menampilkannya.

Komponen ini dapat diibaratkan sebagai petugas penghubung: `DataGridView` berinteraksi melalui `BindingSource`, sedangkan sumber data dapat berupa `List` atau `DataTable`.

Beberapa fungsi yang dapat dimanfaatkan antara lain:

- Menghubungkan sumber data dengan kontrol tampilan.
- Membantu penambahan data ke koleksi.
- Mendukung navigasi dan penyaringan.
- Mengelola hubungan data dengan antarmuka.

Sebagai contoh, pemanggilan `bsMahasiswa.Add(baru)` menambahkan objek melalui `BindingSource` sehingga perubahan dapat diteruskan ke tampilan yang terikat dengannya.

### 4.5 Pengaturan Format Kolom DataGridView

Pengaturan tampilan diperlukan agar informasi dalam tabel lebih mudah dibaca. Beberapa properti dapat diatur melalui jendela **Designer**, seperti:

- `ReadOnly = True` untuk membuat isi tabel tidak dapat diedit langsung oleh pengguna.
- `SelectionMode = FullRowSelect` agar satu baris dipilih secara keseluruhan.
- `AllowUserToAddRows = False` untuk menonaktifkan baris kosong tambahan.

Pengaturan kolom tertentu dilakukan setelah `DataSource` dipasang, misalnya:

- **Judul Kolom:** `dgv.Columns("Nim").HeaderText = "NIM"` untuk mengganti teks header.
- **Format Angka:** `DefaultCellStyle.Format = "N2"` agar angka ditampilkan dengan dua digit desimal, contohnya `3.80`.
- **Perataan Isi:** `MiddleLeft`, `MiddleCenter`, dan `MiddleRight` digunakan sesuai jenis data.
- **Lebar Kolom:** `AutoSizeColumnsMode.Fill` membuat kolom menyesuaikan ruang yang tersedia.

### 4.6 Strategi Pencarian Data

Terdapat dua cara pencarian yang dibahas dalam praktikum ini:

**1. Menggunakan `BindingSource.Filter` pada `DataTable`**

- Memakai ekspresi penyaringan yang menyerupai sintaks SQL, contohnya `Nama LIKE '%Andi%'`.
- Baris yang tidak sesuai dengan kata kunci disembunyikan dari tampilan.
- Data sumber tidak dihapus dan dapat ditampilkan kembali ketika filter dilepas.
- Pengurutan kolom juga dapat digunakan sesuai dukungan sumber data.

**2. Menggunakan Perulangan `For Each` pada `List(Of T)`**

- Setiap elemen dalam koleksi diperiksa satu per satu.
- Pencocokan dapat dilakukan menggunakan metode seperti `.Contains()`.
- Objek yang sesuai dengan kata kunci dikumpulkan ke dalam daftar hasil.
- Daftar hasil selanjutnya dapat dipasang sebagai sumber data untuk grid.

---

## 5. Perbandingan Sumber Data pada DataGridView

Ketiga sumber data berikut memiliki karakteristik dan kegunaan yang berbeda.

| Aspek | Array 2 Dimensi | `List(Of T)` + Class | `DataTable` |
|---|---|---|---|
| **Ukuran Data** | Tetap (*fixed size*) | Dinamis | Dinamis |
| **Tipe Data** | Umumnya satu tipe | Mengikuti tipe properti | Mengikuti tipe kolom |
| **Cara Menampilkan** | `Rows.Add()` | `DataSource` melalui `BindingSource` | `DataSource` melalui `BindingSource` |
| **Pembentukan Kolom** | Manual | Berdasarkan `Public Property` | Berdasarkan `DataColumn` |
| **Pengurutan** | Terbatas | Tidak aktif secara native | Didukung |
| **Fitur Filter** | Tidak tersedia secara langsung | Tidak didukung secara native oleh `BindingSource.Filter` | Didukung |
| **Penggunaan Ideal** | Data statis sederhana | Data terstruktur berbasis objek | Data tabel yang memerlukan filter dan pengurutan |

---

## 6. Kesimpulan

Praktikum Pertemuan 08 memberikan pemahaman mengenai cara menyimpan, mengatur, dan menampilkan data berbentuk tabel pada aplikasi Windows Forms menggunakan VB.NET.

Beberapa poin penting yang diperoleh dari praktikum ini adalah:

1. **Fungsi DataGridView:** Kontrol ini memudahkan penyajian data dengan banyak atribut dalam bentuk baris dan kolom.
2. **Pemilihan Mode Pengisian:** Unbound Mode digunakan untuk memasukkan data secara manual, sedangkan Bound Mode memanfaatkan sumber data agar proses penampilan lebih terstruktur.
3. **Penggunaan Class dan List:** Kombinasi class `Mahasiswa` dan `List(Of T)` membantu mengelompokkan informasi sekaligus mempertahankan tipe data setiap atribut.
4. **Manfaat BindingSource:** Komponen ini menjadi penghubung antara sumber data dan antarmuka sehingga pengelolaan data lebih mudah.
5. **Pencarian Data:** `DataTable` dan fitur filter dapat digunakan untuk menyaring informasi, sedangkan koleksi `List(Of T)` dapat ditelusuri menggunakan perulangan.

Melalui materi ini, mahasiswa dapat mengenali beberapa pendekatan pengelolaan data di VB.NET, mulai dari penggunaan array hingga koleksi objek dan `DataTable`. Pemilihan metode dapat disesuaikan dengan kebutuhan aplikasi yang sedang dibuat.
