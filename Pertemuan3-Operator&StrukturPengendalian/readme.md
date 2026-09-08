# Laporan Praktikum Pemrograman Visual
### Pertemuan 03 — Operator, Struktur Kendali, dan Validasi Input pada VB.NET

| | |
|---|---|
| **Nama** | Tiwy Lamberkat Silaban |
| **NIM** | 241712001 |
| **Kelas** | KOM A1 |
| **Mata Kuliah** | Praktikum Pemrograman Visual |
| **Pertemuan Ke-** | 1 |

---

## Daftar Isi
1. [Ringkasan Praktikum](#1-ringkasan-praktikum)
2. [Tujuan Praktikum](#2-tujuan-praktikum)
3. [Rancangan Antarmuka (UI Controls)](#3-rancangan-antarmuka-ui-controls)
4. [Kode Program](#4-kode-program)
5. [Alur Logika Program](#5-alur-logika-program)
6. [Aturan Evaluasi Skor](#6-aturan-evaluasi-skor)
7. [Catatan Teknis Tambahan](#7-catatan-teknis-tambahan)
8. [Langkah Menjalankan Proyek](#8-langkah-menjalankan-proyek)
9. [Kesimpulan](#9-kesimpulan)

---

## 1. Ringkasan Praktikum

| Keterangan | Isi |
|---|---|
| Mata Kuliah | Pemrograman Visual |
| Topik | Operator, struktur pengendalian, dan validasi input |
| Bahasa | Visual Basic .NET (VB.NET) |
| Platform | .NET / Windows Forms (WinForms) |
| Tools | Visual Studio |

Praktikum ini membangun sebuah **sistem evaluasi nilai** sederhana: pengguna memasukkan skor 0–100, lalu program memvalidasi apakah input tersebut berupa angka dan berada dalam rentang yang wajar, sebelum akhirnya menampilkan gambar yang berbeda-beda tergantung tingkatan nilainya. Studi kasus ini dipakai untuk melatih penggunaan struktur percabangan, operator logika, serta validasi input pada TextBox.

## 2. Tujuan Praktikum

- Memahami cara memvalidasi input pengguna agar sesuai format dan rentang yang diharapkan.
- Memahami cara kerja operator logika *short-circuit* (`AndAlso`, `OrElse`) pada VB.NET.
- Mampu menyusun struktur percabangan bertingkat (`If...ElseIf...Else`) untuk mengambil keputusan berdasarkan beberapa kondisi.
- Mampu membatasi karakter yang bisa diketik pada `TextBox` lewat event `KeyPress`.
- Memahami cara memuat aset gambar secara dinamis saat aplikasi berjalan (*runtime*).

## 3. Rancangan Antarmuka (UI Controls)

| Kontrol | Nama Objek | Properti Kunci | Keterangan |
|---|---|---|---|
| PictureBox | `picImage` | `SizeMode = StretchImage` | Menampilkan gambar sesuai hasil evaluasi nilai |
| TextBox | `txtNilai` | `Text = ""` | Tempat pengguna memasukkan nilai |
| Button | `btnInput` | `Text = "Input"` | Memicu proses validasi dan evaluasi nilai |

## 4. Kode Program

Berkas: `Form1.vb`

```vb
Public Class Form1

    Private Sub btnInput_Click(sender As Object, e As EventArgs) Handles btnInput.Click
        Dim nilai As Integer

        ' Validasi format numerik
        If Not Integer.TryParse(txtNilai.Text, nilai) Then
            MessageBox.Show("Masukkan dalam bentuk angka")
            txtNilai.Focus()
            Return
        End If

        ' Validasi jangkauan nilai 0 - 100
        If nilai < 0 OrElse nilai > 100 Then
            MessageBox.Show("Masukkan Nilai 0-100")
            txtNilai.Focus()
            Return
        End If

        ' Struktur pengkondisian pemilihan aset
        If nilai <= 50 Then
            picImage.Image = Image.FromFile("Asset\1.jpeg")
        ElseIf nilai <= 70 Then
            picImage.Image = Image.FromFile("Asset\2.png")
        Else
            picImage.Image = Image.FromFile("Asset\3.png")
        End If
    End Sub

    Private Sub txtNilai_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNilai.KeyPress
        ' Restriksi karakter: hanya menerima angka dan tombol kontrol
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

End Class
```

## 5. Alur Logika Program

Setiap kali tombol **Input** diklik, program menjalankan tiga tahap pemeriksaan secara berurutan sebelum menampilkan gambar:

```
┌─────────────────────────┐
│ 1. Cek format angka     │──✗ gagal──▶ tampilkan pesan "harus angka" + Return
│    (Integer.TryParse)   │
└───────────┬─────────────┘
            │ ✓ valid
            ▼
┌─────────────────────────┐
│ 2. Cek rentang 0–100    │──✗ di luar rentang──▶ tampilkan pesan peringatan + Return
│    (nilai < 0 OrElse    │
│     nilai > 100)        │
└───────────┬─────────────┘
            │ ✓ dalam rentang
            ▼
┌─────────────────────────┐
│ 3. Pilih gambar sesuai  │
│    tingkatan nilai      │
│    (If...ElseIf...Else) │
└─────────────────────────┘
```

Pola `If ... Then Return` pada dua tahap pertama disebut **guard clause** — begitu ada input yang tidak valid, program langsung keluar dari subrutin lebih awal tanpa perlu membungkus sisa logika dalam blok `Else` yang bertumpuk.

Selain itu, event `txtNilai_KeyPress` bekerja secara terpisah dan aktif setiap kali pengguna menekan tombol keyboard di dalam `txtNilai` — bukan hanya saat tombol **Input** diklik. Event ini menyaring karakter sebelum sempat tampil di kolom teks.

## 6. Aturan Evaluasi Skor

| Rentang Nilai | Kondisi pada Kode | Gambar yang Ditampilkan | Keterangan |
|---|---|---|---|
| 0 – 50 | `nilai <= 50` | `Asset\1.jpeg` | Kategori nilai rendah |
| 51 – 70 | `nilai > 50 AndAlso nilai <= 70` | `Asset\2.png` | Kategori nilai menengah |
| 71 – 100 | `nilai > 70 AndAlso nilai <= 100` | `Asset\3.png` | Kategori nilai tinggi |
| < 0 atau > 100 | `nilai < 0 OrElse nilai > 100` | *(tidak ada)* | Program menolak & menampilkan peringatan |

## 7. Catatan Teknis Tambahan

### 7.1 `Integer.TryParse` untuk Validasi Aman
`Integer.TryParse` mencoba mengonversi teks menjadi angka tanpa membuat program *crash* jika gagal — berbeda dengan `Integer.Parse` biasa yang akan melempar *exception* saat inputnya bukan angka atau kosong. Hasil percobaan konversi dikembalikan sebagai nilai `Boolean` (`True`/`False`), sementara angka hasil konversinya (jika berhasil) langsung disimpan ke variabel `nilai`.

### 7.2 Operator Short-Circuit: `AndAlso` vs `OrElse`
| Operator | Berhenti Mengevaluasi Ketika | Contoh Pemakaian di Kode |
|---|---|---|
| `OrElse` | Kondisi pertama sudah **True** | `nilai < 0 OrElse nilai > 100` |
| `AndAlso` | Kondisi pertama sudah **False** | `Not Char.IsControl(...) AndAlso Not Char.IsDigit(...)` |

Sifat *short-circuit* ini membuat evaluasi lebih efisien karena kondisi kedua tidak perlu dicek jika hasil akhirnya sudah bisa dipastikan dari kondisi pertama.

### 7.3 Membatasi Input lewat `KeyPress` dan `e.Handled`
Event `KeyPress` terpicu setiap kali ada tombol yang ditekan di dalam kontrol. Dengan mengecek `e.KeyChar`, program bisa menentukan apakah karakter tersebut termasuk **angka** (`Char.IsDigit`) atau **tombol kontrol** seperti Backspace (`Char.IsControl`). Jika karakter di luar dua kategori tersebut, `e.Handled = True` akan membatalkan karakter itu sehingga tidak pernah muncul di `TextBox`.

### 7.4 Memuat Gambar Dinamis dengan `Image.FromFile`
`Image.FromFile()` memuat berkas gambar dari path relatif seperti `Asset\1.jpeg`, `Asset\2.png`, dan `Asset\3.png`, lalu menetapkannya ke properti `Image` milik `PictureBox` sesuai hasil evaluasi.

>  **Catatan:** pastikan folder `Asset` beserta isinya disertakan dalam proyek, dan properti **Copy to Output Directory** pada tiap file gambar diatur ke **Copy if newer** — jika tidak, gambar tidak akan ditemukan saat aplikasi dijalankan dari folder build.

## 8. Langkah Menjalankan Proyek

1. Buka proyek pada **Visual Studio**.
2. Pastikan folder `Asset` (berisi `1.jpeg`, `2.png`, `3.png`) sudah ada di direktori proyek dan ter-*copy* ke output.
3. Build proyek lewat **Build → Build Solution** (`Ctrl + Shift + B`).
4. Jalankan aplikasi dengan `F5`.
5. Uji coba program:
   - Masukkan huruf di `txtNilai` → pastikan karakter tidak bisa diketik sama sekali.
   - Masukkan angka di luar 0–100 (misalnya `150`) → pastikan muncul peringatan rentang.
   - Masukkan angka valid (misalnya `45`, `65`, `90`) → pastikan gambar yang tampil sesuai kategori nilainya.

## 9. Kesimpulan

Melalui praktikum ini, beberapa konsep inti pemrograman VB.NET berhasil diterapkan sekaligus dalam satu studi kasus: validasi input dengan `Integer.TryParse`, pengambilan keputusan bertingkat lewat `If...ElseIf...Else`, efisiensi logika dengan operator *short-circuit* `AndAlso`/`OrElse`, pembatasan karakter input lewat event `KeyPress` beserta `e.Handled`, hingga pemuatan aset gambar secara dinamis dengan `Image.FromFile`. Kombinasi konsep-konsep ini menghasilkan program yang tidak hanya berfungsi, tetapi juga tahan terhadap kesalahan input dari pengguna.

---

<p align="center"><i>Disusun sebagai bagian dari Laporan Praktikum Mata Kuliah Pemrograman Visual — Pertemuan 03.</i></p>
