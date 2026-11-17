# Laporan Praktikum Pemrograman Visual
### Pertemuan 06 — Menghubungkan VB.NET ke Supabase (PostgreSQL) dengan Npgsql

| | |
|---|---|
| **Nama** | Tiwy Lamberkat Silaban |
| **NIM** | 241712001 |
| **Kelas** | KOM A1 |
| **Mata Kuliah** | Praktikum Pemrograman Visual |
| **Pertemuan Ke-** | 6 |

## 1. Landasan Teori

Ada empat komponen yang bekerja berurutan agar aplikasi bisa menyimpan data ke cloud:

```
Aplikasi VB.NET --> Npgsql --> Internet --> Supabase --> PostgreSQL
   (tampilan)      (driver)                 (layanan)    (penyimpan data)
```

| Komponen | Peran singkat |
|---|---|
| **Database** | Tempat data terstruktur disimpan dalam tabel (baris dan kolom). Setiap tabel punya *primary key* sebagai identitas unik tiap baris. |
| **PostgreSQL** | Mesin database relasional open-source yang mendukung SQL, transaksi, constraint, serta tipe data seperti JSON, array, dan UUID. |
| **Supabase** | Layanan *Backend-as-a-Service* yang menjalankan PostgreSQL di cloud lengkap dengan dashboard web, sehingga kita tidak perlu memasang server sendiri. |
| **Npgsql** | Data provider ADO.NET yang menjadi penghubung antara kode .NET dan PostgreSQL. Cara pakainya mirip `SqlConnection` pada SQL Server. |

Analogi sederhananya: PostgreSQL adalah mesin, Supabase adalah pabrik yang membungkusnya, dan Npgsql adalah jalur pengiriman dari aplikasi kita ke pabrik tersebut.

---

## 2. Menyiapkan Supabase dan Proyek

### 2.1 Membuat proyek

1. Masuk ke [supabase.com](https://supabase.com) memakai akun GitHub atau email.
2. Pilih **New project** dan tentukan organisasinya.
3. Beri nama proyek, lalu buat **Database Password** dan simpan baik-baik.
4. Pilih region terdekat. Dari Indonesia, **Singapore** biasanya paling cepat.
5. Klik **Create new project** dan tunggu 1 sampai 2 menit.

### 2.2 Membuat tabel

Buka **SQL Editor**, lalu jalankan skrip berikut:

```sql
CREATE TABLE buku (
    id         BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    kode       VARCHAR(15)  NOT NULL UNIQUE,
    judul      VARCHAR(150) NOT NULL,
    kategori   VARCHAR(50),
    created_at TIMESTAMPTZ  NOT NULL DEFAULT now()
);

INSERT INTO buku (kode, judul, kategori) VALUES
    ('BK-001', 'Pemrograman Visual Basic .NET', 'Komputer'),
    ('BK-002', 'Dasar-Dasar Basis Data',        'Komputer'),
    ('BK-003', 'Laskar Pelangi',                'Novel');
```

| Kolom | Tipe | Catatan |
|---|---|---|
| `id` | BIGINT IDENTITY | Primary key, dibuat otomatis oleh database |
| `kode` | VARCHAR(15) | Wajib diisi dan tidak boleh kembar |
| `judul` | VARCHAR(150) | Wajib diisi |
| `kategori` | VARCHAR(50) | Opsional |
| `created_at` | TIMESTAMPTZ | Waktu pencatatan, terisi otomatis |

### 2.3 Mengambil data koneksi

Aplikasi desktop terhubung langsung ke Postgres (bukan lewat REST API), jadi kita butuh detail koneksi:

1. Klik tombol **Connect** di dashboard proyek.
2. Buka tab **Connection String** dan pilih **Session pooler**.
3. Catat nilai berikut:

| Parameter | Contoh | Catatan |
|---|---|---|
| Host | `aws-0-ap-southeast-1.pooler.supabase.com` | Alamat pooler |
| Port | `5432` | Port session pooler |
| Database | `postgres` | Database bawaan |
| Username | `postgres.abcdefghijklmnop` | Formatnya `postgres.<project-ref>` |
| Password | (milik Anda) | Rahasia, jangan di-commit ke repositori |

### 2.4 Paket NuGet

Pasang tiga paket berikut lewat NuGet Package Manager:

| Paket | Fungsi |
|---|---|
| `Npgsql` | Driver untuk PostgreSQL |
| `Microsoft.Extensions.Configuration` | Inti sistem konfigurasi (`ConfigurationBuilder`) |
| `Microsoft.Extensions.Configuration.Json` | Membaca file JSON lewat `AddJsonFile` |

---

## 3. Mengatur Koneksi

### 3.1 Kelas-kelas Npgsql yang dipakai

| Kelas | Tugas |
|---|---|
| `NpgsqlConnection` | Membuka dan mengelola sambungan ke database |
| `NpgsqlCommand` | Menjalankan perintah SQL (SELECT, INSERT, UPDATE, DELETE) |
| `NpgsqlDataAdapter` | Memindahkan hasil kueri ke `DataTable`/`DataSet` dan mengirim balik perubahannya |
| `NpgsqlCommandBuilder` | Membuat perintah INSERT/UPDATE/DELETE otomatis dari sebuah SELECT |

```
          NpgsqlConnection
                 |
        +--------+---------+
        |                  |
  NpgsqlCommand    NpgsqlDataAdapter
                           |
                  NpgsqlCommandBuilder
```

### 3.2 Menyimpan connection string di `appsettings.json`

Kredensial sebaiknya dipisah dari kode. Tambahkan file lewat **Add > New Item > JSON File** bernama `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=aws-0-ap-southeast-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.abcdefghijklmnop;Password=PASSWORD_ANDA;SSL Mode=Require;Trust Server Certificate=true"
  }
}
```

| Bagian | Arti |
|---|---|
| `Host`, `Port`, `Database` | Lokasi server dan database tujuan |
| `Username`, `Password` | Identitas login |
| `SSL Mode=Require` | Memaksa koneksi terenkripsi (diwajibkan Supabase) |
| `Trust Server Certificate=true` | Praktis untuk belajar; pada produksi, verifikasi sertifikat dengan benar |

> **Jangan lupa:** pada jendela Properties file `appsettings.json`, ubah **Copy to Output Directory** menjadi **Copy if newer**. Jika tidak, program akan error `FileNotFoundException`.
>
> Karena file ini berisi password, masukkan ke `.gitignore` bila proyek diunggah ke GitHub.

### 3.3 Modul `ModKoneksi`

```vb
Imports Microsoft.Extensions.Configuration
Imports Npgsql

Module ModKoneksi

    Public Function GetConnectionString() As String
        Dim konfigurasi As IConfiguration =
            New ConfigurationBuilder() _
            .SetBasePath(AppContext.BaseDirectory) _
            .AddJsonFile("appsettings.json", optional:=False, reloadOnChange:=True) _
            .Build()

        Return konfigurasi.GetConnectionString("DefaultConnection")
    End Function

    Public Sub UjiKoneksi()
        Try
            Using conn As New NpgsqlConnection(GetConnectionString())
                conn.Open()
                MessageBox.Show("Terhubung ke Supabase.")
            End Using
        Catch ex As Exception
            MessageBox.Show("Tidak dapat terhubung:" & vbCrLf & ex.Message)
        End Try
    End Sub

End Module
```

Cara kerja `GetConnectionString`:

- `ConfigurationBuilder` merakit sumber pengaturan.
- `SetBasePath` menunjuk folder tempat aplikasi berjalan.
- `AddJsonFile` memuat file JSON. `optional:=False` artinya file wajib ada, dan `reloadOnChange:=True` membaca ulang bila file berubah.
- `Build` menghasilkan objek `IConfiguration`, lalu `GetConnectionString` mengambil nilai `DefaultConnection`.

### 3.4 Uji koneksi

```vb
Private Sub FormUtama_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    ModKoneksi.UjiKoneksi()
End Sub
```

Jika muncul "Terhubung ke Supabase", semuanya siap. Jika gagal, lihat tabel di [subbab 7.3](#73-masalah-umum).

---

## 4. Arsitektur Kode

Kode dipisah menjadi lapisan agar form tetap bersih dan SQL tidak tersebar:

```
FormUtama     -> input pengguna dan tampilan
   |
ModKueri      -> menyusun SQL dan parameter
   |
ModDataAkses  -> menjalankan SQL ke database
   |
ModKoneksi    -> menyediakan connection string
   |
Supabase (PostgreSQL)
```

### 4.1 `ModDataAkses`

Berisi dua fungsi inti: `BacaData` (untuk SELECT) dan `JalankanPerintah` (untuk INSERT/UPDATE/DELETE). Keduanya menerima parameter lewat `Dictionary`.

```vb
Imports Npgsql
Imports System.Data

Module ModDataAkses

    ' Untuk SELECT, dengan atau tanpa parameter
    Public Function BacaData(sql As String,
                             Optional param As Dictionary(Of String, Object) = Nothing) As DataTable
        Dim hasil As New DataTable

        Try
            Using conn As New NpgsqlConnection(ModKoneksi.GetConnectionString())
                conn.Open()
                Using cmd As New NpgsqlCommand(sql, conn)
                    IsiParameter(cmd, param)
                    Using adapter As New NpgsqlDataAdapter(cmd)
                        adapter.Fill(hasil)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Gagal membaca data:" & vbCrLf & ex.Message, "Error Database")
        End Try

        Return hasil
    End Function

    ' Untuk INSERT, UPDATE, DELETE
    Public Function JalankanPerintah(sql As String,
                                     param As Dictionary(Of String, Object)) As Boolean
        Try
            Using conn As New NpgsqlConnection(ModKoneksi.GetConnectionString())
                conn.Open()
                Using cmd As New NpgsqlCommand(sql, conn)
                    IsiParameter(cmd, param)
                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        Catch ex As PostgresException When ex.SqlState = "23505"
            MessageBox.Show("Kode buku sudah terdaftar.", "Peringatan")
            Return False
        Catch ex As Exception
            MessageBox.Show("Gagal menjalankan perintah:" & vbCrLf & ex.Message, "Error Database")
            Return False
        End Try
    End Function

    Private Sub IsiParameter(cmd As NpgsqlCommand, param As Dictionary(Of String, Object))
        If param Is Nothing Then Return
        For Each p In param
            cmd.Parameters.AddWithValue(p.Key, p.Value)
        Next
    End Sub

End Module
```

Poin penting:

- Blok `Using` otomatis menutup koneksi, command, dan adapter, bahkan saat terjadi error.
- Jika `BacaData` gagal, fungsi tetap mengembalikan `DataTable` kosong sehingga aplikasi tidak crash.
- `JalankanPerintah` mengembalikan `True`/`False` agar pemanggil tahu hasilnya.
- Kode error `23505` berarti pelanggaran kolom unik. Blok `Catch` yang spesifik harus ditulis **sebelum** `Catch ex As Exception`.
- Parameter bersifat opsional pada `BacaData`, jadi satu fungsi melayani SELECT biasa dan pencarian.

### 4.2 `ModKueri`

```vb
Imports System.Data

Module ModKueri

    Public Function SimpanBuku(kode As String, judul As String, kategori As String) As Boolean
        Dim sql As String =
            "INSERT INTO buku (kode, judul, kategori) VALUES (@kode, @judul, @kategori)"

        Dim p As New Dictionary(Of String, Object) From {
            {"@kode", kode},
            {"@judul", judul},
            {"@kategori", kategori}
        }
        Return ModDataAkses.JalankanPerintah(sql, p)
    End Function

    Public Function SemuaBuku() As DataTable
        Return ModDataAkses.BacaData("SELECT * FROM buku ORDER BY id ASC")
    End Function

    Public Function PerbaruiBuku(id As Long, kode As String,
                                 judul As String, kategori As String) As Boolean
        Dim sql As String =
            "UPDATE buku SET kode = @kode, judul = @judul, kategori = @kategori " &
            "WHERE id = @id"

        Dim p As New Dictionary(Of String, Object) From {
            {"@id", id},
            {"@kode", kode},
            {"@judul", judul},
            {"@kategori", kategori}
        }
        Return ModDataAkses.JalankanPerintah(sql, p)
    End Function

    Public Function HapusBuku(id As Long) As Boolean
        Dim p As New Dictionary(Of String, Object) From {{"@id", id}}
        Return ModDataAkses.JalankanPerintah("DELETE FROM buku WHERE id = @id", p)
    End Function

    Public Function CariBuku(kataKunci As String) As DataTable
        Dim sql As String =
            "SELECT * FROM buku WHERE judul ILIKE @kata OR kode ILIKE @kata ORDER BY id ASC"

        Dim p As New Dictionary(Of String, Object) From {
            {"@kata", "%" & kataKunci.Trim() & "%"}
        }
        Return ModDataAkses.BacaData(sql, p)
    End Function

End Module
```

`id` dan `created_at` tidak ikut di INSERT karena diisi otomatis oleh database. `ILIKE` adalah operator PostgreSQL untuk pencarian tanpa membedakan huruf besar/kecil, dan `%` mewakili sembarang karakter.

---

## 5. Operasi CRUD

| Operasi | SQL | Fungsi di `ModKueri` | Fungsi di `ModDataAkses` |
|---|---|---|---|
| Create | INSERT | `SimpanBuku` | `JalankanPerintah` |
| Read | SELECT | `SemuaBuku`, `CariBuku` | `BacaData` |
| Update | UPDATE | `PerbaruiBuku` | `JalankanPerintah` |
| Delete | DELETE | `HapusBuku` | `JalankanPerintah` |

Kontrol yang dipakai pada form: `dgvBuku`, `txtKode`, `txtJudul`, `txtKategori`, `txtCari`, `btnTampil`, `btnSimpan`, `btnUbah`, `btnHapus`.

### 5.1 Kode lengkap `FormUtama`

```vb
Public Class FormUtama

    Private idDipilih As Long = 0

    Private Sub FormUtama_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ModKoneksi.UjiKoneksi()
        RefreshGrid()
    End Sub

    Private Sub RefreshGrid()
        dgvBuku.AutoGenerateColumns = True
        dgvBuku.DataSource = ModKueri.SemuaBuku()
    End Sub

    Private Sub KosongkanForm()
        idDipilih = 0
        txtKode.Clear()
        txtJudul.Clear()
        txtKategori.Clear()
        txtKode.Focus()
    End Sub

    Private Function InputValid() As Boolean
        If String.IsNullOrWhiteSpace(txtKode.Text) OrElse
           String.IsNullOrWhiteSpace(txtJudul.Text) Then
            MessageBox.Show("Kode dan judul wajib diisi.", "Peringatan",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        Return True
    End Function

    ' READ
    Private Sub btnTampil_Click(sender As Object, e As EventArgs) Handles btnTampil.Click
        RefreshGrid()
    End Sub

    ' CREATE
    Private Sub btnSimpan_Click(sender As Object, e As EventArgs) Handles btnSimpan.Click
        If Not InputValid() Then Return

        If ModKueri.SimpanBuku(txtKode.Text, txtJudul.Text, txtKategori.Text) Then
            MessageBox.Show("Buku berhasil disimpan.")
            KosongkanForm()
            RefreshGrid()
        End If
    End Sub

    ' Mengisi form saat baris grid diklik
    Private Sub dgvBuku_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
        Handles dgvBuku.CellClick

        If e.RowIndex < 0 Then Return

        Dim baris As DataGridViewRow = dgvBuku.Rows(e.RowIndex)
        idDipilih = CLng(baris.Cells("id").Value)
        txtKode.Text = baris.Cells("kode").Value.ToString()
        txtJudul.Text = baris.Cells("judul").Value.ToString()
        txtKategori.Text = baris.Cells("kategori").Value.ToString()
    End Sub

    ' UPDATE
    Private Sub btnUbah_Click(sender As Object, e As EventArgs) Handles btnUbah.Click
        If idDipilih = 0 Then
            MessageBox.Show("Pilih baris yang akan diubah terlebih dahulu.")
            Return
        End If
        If Not InputValid() Then Return

        If ModKueri.PerbaruiBuku(idDipilih, txtKode.Text, txtJudul.Text, txtKategori.Text) Then
            MessageBox.Show("Data buku diperbarui.")
            KosongkanForm()
            RefreshGrid()
        End If
    End Sub

    ' DELETE
    Private Sub btnHapus_Click(sender As Object, e As EventArgs) Handles btnHapus.Click
        If idDipilih = 0 Then
            MessageBox.Show("Pilih baris yang akan dihapus terlebih dahulu.")
            Return
        End If

        Dim jawaban = MessageBox.Show("Hapus buku ini secara permanen?", "Konfirmasi",
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If jawaban <> DialogResult.Yes Then Return

        If ModKueri.HapusBuku(idDipilih) Then
            MessageBox.Show("Buku dihapus.")
            KosongkanForm()
            RefreshGrid()
        End If
    End Sub

    ' Pencarian langsung saat mengetik
    Private Sub txtCari_TextChanged(sender As Object, e As EventArgs) Handles txtCari.TextChanged
        dgvBuku.DataSource = ModKueri.CariBuku(txtCari.Text)
    End Sub

End Class
```

> **Peringatan:** `UPDATE` dan `DELETE` tanpa klausa `WHERE` akan memengaruhi seluruh baris tabel. Karena itu form menolak aksi bila `idDipilih` masih 0.

---

## 6. Alternatif: DataAdapter dan CommandBuilder

Pendekatan ini memakai model *disconnected*: data ditarik ke `DataTable`, diedit di memori (misalnya langsung di grid), lalu semua perubahan dikirim sekaligus lewat `Update()`. `NpgsqlCommandBuilder` membuat perintah INSERT/UPDATE/DELETE secara otomatis, dengan syarat SELECT-nya memuat primary key dan hanya berasal dari satu tabel.

```vb
Imports Npgsql

Public Class FormGridEdit

    Private conn As NpgsqlConnection
    Private adapter As NpgsqlDataAdapter
    Private builder As NpgsqlCommandBuilder
    Private tabel As DataTable

    Private Sub FormGridEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MuatTabel()
    End Sub

    Private Sub MuatTabel()
        conn = New NpgsqlConnection(ModKoneksi.GetConnectionString())
        adapter = New NpgsqlDataAdapter(
            "SELECT id, kode, judul, kategori FROM buku ORDER BY id", conn)
        builder = New NpgsqlCommandBuilder(adapter)

        tabel = New DataTable()
        adapter.Fill(tabel)
        dgvBuku.DataSource = tabel
    End Sub

    Private Sub btnSimpanSemua_Click(sender As Object, e As EventArgs) Handles btnSimpanSemua.Click
        Try
            dgvBuku.EndEdit()
            Dim jumlah As Integer = adapter.Update(tabel)
            MessageBox.Show(jumlah & " baris tersimpan.")
            MuatTabel()
        Catch ex As Exception
            MessageBox.Show("Penyimpanan gagal: " & ex.Message)
        End Try
    End Sub

End Class
```

Atur `dgvBuku` dengan `ReadOnly = False` dan `AllowUserToAddRows = True`.

Karena `id` berstatus `GENERATED ALWAYS`, database menolak nilai `id` dari aplikasi. Bila penambahan baris gagal, ubah tabel menjadi `GENERATED BY DEFAULT AS IDENTITY` atau jadikan kolom `id` read-only di `DataTable`.

| Aspek | NpgsqlCommand berlapis (bab 4-5) | DataAdapter + CommandBuilder (bab 6) |
|---|---|---|
| Banyak kode | Lebih banyak | Lebih sedikit |
| Kendali SQL | Penuh | Terbatas pada SQL otomatis |
| Pengiriman | Langsung tiap aksi | Sekaligus saat `Update()` |
| Cocok untuk | Form input dengan validasi ketat | Mengedit tabel langsung di grid |
| Kueri JOIN | Mudah | Tidak didukung CommandBuilder |

---

## 7. Keamanan dan Pemecahan Masalah

### 7.1 Selalu gunakan query berparameter

```vb
' SALAH: rentan SQL Injection
Dim sql As String = "SELECT * FROM buku WHERE kode = '" & txtKode.Text & "'"

' BENAR: nilai dikirim sebagai parameter
Dim sql As String = "SELECT * FROM buku WHERE kode = @kode"
Dim p As New Dictionary(Of String, Object) From {{"@kode", txtKode.Text}}
```

Pada contoh yang salah, pengguna cukup mengetik `' OR '1'='1` untuk menampilkan seluruh data. Dengan parameter, isi kotak teks selalu dianggap nilai, bukan bagian dari perintah SQL.

### 7.2 Validasi input

Event `KeyPress` dapat membatasi karakter yang boleh diketik. Contoh: kolom kategori hanya menerima huruf dan spasi.

```vb
Private Sub txtKategori_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtKategori.KeyPress
    If Not (Char.IsLetter(e.KeyChar) OrElse e.KeyChar = " "c OrElse Char.IsControl(e.KeyChar)) Then
        e.Handled = True
    End If
End Sub
```

`e.Handled = True` membuang karakter terlarang sebelum masuk ke kotak teks. Selain itu, periksa kolom wajib sebelum memanggil `ModKueri` (lihat fungsi `InputValid` pada bab 5).

### 7.3 Masalah umum

| Gejala | Penyebab | Solusi |
|---|---|---|
| `FileNotFoundException` (`appsettings.json`) | File tidak ikut tersalin saat build | Set **Copy to Output Directory** = *Copy if newer* |
| `ConfigurationBuilder` / `AddJsonFile` tidak dikenali | Paket NuGet belum terpasang | Pasang paket Configuration dan Configuration.Json |
| Timeout atau host tidak ditemukan | Direct connection pada jaringan tanpa IPv6 | Pakai **Session pooler** |
| `password authentication failed` | Password salah atau format username keliru | Pakai `postgres.<project-ref>`; reset password bila perlu |
| Error SSL/sertifikat | SSL belum diatur | Tambahkan `SSL Mode=Require;Trust Server Certificate=true` |
| `relation "buku" does not exist` | Tabel belum dibuat | Jalankan ulang SQL pembuatan tabel, cek di Table Editor |
| `duplicate key value violates unique constraint` | Kode buku kembar | Sudah ditangani lewat `PostgresException` kode `23505` |
| `cannot insert a non-DEFAULT value into column "id"` | `id` (GENERATED ALWAYS) ikut dikirim | Jangan sertakan `id` pada INSERT |

### 7.4 Ide pengembangan

- Pisahkan pemanggilan `MessageBox` dari modul data agar lapisan data tidak bergantung pada antarmuka; kembalikan pesan error ke form.
- Tambahkan tombol *Reset* untuk membersihkan form dan memuat ulang grid.
- Simpan `appsettings.json` di luar repositori publik, atau gunakan *User Secrets* untuk menyimpan password.
