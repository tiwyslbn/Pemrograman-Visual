# Laporan Praktikum Pemrograman Visual
### Pertemuan 02 — Komponen Visual, Properti, dan Penanganan Event pada Windows Forms

| | |
|---|---|
| **Nama** | Tiwy Lamberkat Silaban |
| **NIM** | 241712001 |
| **Kelas** | KOM A1 |
| **Mata Kuliah** | Praktikum Pemrograman Visual |
| **Pertemuan Ke-** | 2 |

---

## Daftar Isi
1. [Ringkasan Praktikum](#1-ringkasan-praktikum)
2. [Tujuan Praktikum](#2-tujuan-praktikum)
3. [Rancangan Antarmuka (UI Controls)](#3-rancangan-antarmuka-ui-controls)
4. [Kode Program](#4-kode-program)
5. [Cara Kerja Program](#5-cara-kerja-program)
6. [Catatan Teknis Tambahan](#6-catatan-teknis-tambahan)
7. [Langkah Menjalankan Proyek](#7-langkah-menjalankan-proyek)
8. [Kesimpulan](#8-kesimpulan)

---

## 1. Ringkasan Praktikum

| Keterangan | Isi |
|---|---|
| Mata Kuliah | Pemrograman Visual |
| Topik | Komponen visual, properti dasar, dan event handling |
| Bahasa | Visual Basic .NET (VB.NET) |
| Platform | .NET / Windows Forms (WinForms) |
| Tools | Visual Studio |

Praktikum kali ini membahas cara membangun form input data sederhana menggunakan **Windows Forms**. Studi kasus yang dibuat adalah **form profil mahasiswa** dengan tiga kolom input — Nama, NIM, dan Kelas (KOM) — beserta tiga tombol aksi: **Tampilkan**, **Hapus**, dan **Keluar**. Lewat studi kasus ini, praktikum menyasar pemahaman tentang bagaimana komponen GUI saling berinteraksi lewat mekanisme *event-driven programming*.

## 2. Tujuan Praktikum

- Memahami cara menempatkan dan mengatur properti komponen dasar Windows Forms (`Label`, `TextBox`, `Button`).
- Memahami konsep *event handling* pada VB.NET, khususnya event `Click`.
- Mampu membuat form input data sederhana yang dapat menampilkan, mengosongkan, dan menutup sesi.
- Mengenal konvensi penamaan kontrol (*naming convention*) yang umum dipakai dalam pengembangan WinForms.

## 3. Rancangan Antarmuka (UI Controls)

| Kontrol | Nama Objek | Teks Awal | Properti Tambahan | Keterangan |
|---|---|---|---|---|
| Form | `Form1` | Profil Mahasiswa | `StartPosition = CenterScreen` | Jendela utama aplikasi |
| Label | `lblNama` | Nama | `AutoSize = True` | Keterangan kolom nama |
| Label | `lblNIM` | NIM | `AutoSize = True` | Keterangan kolom NIM |
| Label | `Label1` | KOM | `AutoSize = True` | Keterangan kolom kelas |
| TextBox | `txtNama` | *(kosong)* | `TabIndex = 0` | Input nama mahasiswa |
| TextBox | `txtNim` | *(kosong)* | `TabIndex = 1` | Input NIM mahasiswa |
| TextBox | `txtKom` | *(kosong)* | `TabIndex = 2` | Input kelas mahasiswa |
| Button | `btnTampilkan` | Tampilkan | `TabIndex = 3` | Menampilkan data ke `MessageBox` |
| Button | `txtHapus` | Hapus | `TabIndex = 4` | Mengosongkan semua input |
| Button | `txtKeluar` | Keluar | `TabIndex = 5` | Menutup aplikasi |

>  **Perhatian:** dua tombol terakhir (`txtHapus`, `txtKeluar`) masih memakai prefiks `txt`, padahal keduanya bertipe `Button`. Idealnya prefiks disesuaikan tipe kontrolnya — lihat [bagian 6.5](#6-catatan-teknis-tambahan).

## 4. Kode Program

Berkas: `Form1.vb`

```vb
Public Class Form1

    ' Menampilkan rekap data ke MessageBox
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnTampilkan.Click
        MessageBox.Show("Halo Selamat Datang!" & vbCrLf &
                        "Nama: " & txtNama.Text & vbCrLf &
                        "NIM: " & txtNim.Text & vbCrLf &
                        "Kom: " & txtKom.Text,
                        "Informasi Data Mahasiswa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information)
    End Sub

    ' Mengosongkan seluruh kolom input
    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles txtHapus.Click
        txtNama.Clear()
        txtNim.Clear()
        txtKom.Clear()

        txtNama.Focus() ' fokus kembali ke kolom pertama
    End Sub

    ' Menutup form / mengakhiri aplikasi
    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles txtKeluar.Click
        Me.Close()
    End Sub

End Class
```

## 5. Cara Kerja Program

| Tombol | Event yang Ditangkap | Apa yang Terjadi |
|---|---|---|
| **Tampilkan** | `btnTampilkan.Click` | Mengambil nilai `Text` dari `txtNama`, `txtNim`, dan `txtKom`, lalu menggabungkannya menjadi satu pesan dan menampilkannya lewat `MessageBox.Show`. |
| **Hapus** | `txtHapus.Click` | Memanggil `.Clear()` pada ketiga `TextBox`, kemudian memindahkan fokus kursor kembali ke `txtNama` lewat `.Focus()`. |
| **Keluar** | `txtKeluar.Click` | Memanggil `Me.Close()` untuk menutup form aktif dan mengakhiri aplikasi (karena form ini adalah *Main Form*). |

Ketiga subrutin di atas terhubung ke tombolnya masing-masing lewat klausa `Handles`, bukan lewat nama subrutinnya. Artinya nama seperti `Button1_Click` boleh diganti bebas — yang menentukan tombol mana yang memicu subrutin tersebut adalah bagian setelah kata kunci `Handles`.

## 6. Catatan Teknis Tambahan

### 6.1 Penggabungan String: `&` vs `+`
Gunakan operator **`&`** untuk menggabungkan teks, bukan `+`. Operator `+` bisa ambigu karena VB.NET akan mencoba menafsirkannya sebagai operasi penjumlahan angka jika salah satu nilainya berupa string angka, sehingga berisiko memicu galat konversi tipe data.

### 6.2 Konstanta Baris Baru
| Konstanta | Keterangan |
|---|---|
| `vbCrLf` | Gabungan *Carriage Return* + *Line Feed*, gaya VB klasik |
| `vbNewLine` | Alias bawaan VB Runtime, setara `vbCrLf` |
| `Environment.NewLine` | Mengikuti standar sistem operasi, direkomendasikan untuk kode .NET modern |

### 6.3 Mengosongkan Isi TextBox
| Cara | Catatan |
|---|---|
| `txtNama.Clear()` | Disarankan, method resmi kontrol `TextBox` |
| `txtNama.Text = String.Empty` | Sedikit lebih efisien, tidak membuat objek string baru |
| `txtNama.Text = ""` | Cara paling umum, literal string kosong |

### 6.4 Mengakhiri Aplikasi
| Perintah | Efek |
|---|---|
| `Me.Close()` | Menutup form aktif; jika form ini *Main Form*, aplikasi ikut berhenti dengan proses *cleanup* normal |
| `Application.Exit()` | Menghentikan seluruh *message loop* aplikasi di semua form yang terbuka |
| `End` | Mematikan proses secara paksa, **tidak disarankan** karena melewati event `FormClosing` dan proses pembersihan resource |

### 6.5 Konvensi Penamaan Kontrol
Standar umum penamaan kontrol pada WinForms:

- `btn` → Button (`btnTampilkan`, `btnHapus`, `btnKeluar`)
- `txt` → TextBox (`txtNama`, `txtNim`, `txtKom`)
- `lbl` → Label (`lblNama`, `lblNIM`, `lblKom`)

Pada proyek ini, `txtHapus` dan `txtKeluar` sebenarnya bertipe **Button**, bukan **TextBox** — sehingga penamaannya tidak konsisten dengan konvensi di atas. Sebaiknya diganti menjadi `btnHapus` dan `btnKeluar` agar lebih mudah dibaca dan dipelihara.

### 6.6 Fokus Otomatis dengan `.Focus()`
Baris `txtNama.Focus()` setelah proses `Clear()` memindahkan kursor secara otomatis ke kolom input pertama, sehingga pengguna bisa langsung mengetik data baru tanpa perlu mengklik kolom secara manual — sebuah sentuhan kecil untuk kenyamanan pengguna (*UX*).

## 7. Langkah Menjalankan Proyek

1. Buka file proyek (`.slnx` atau `.vbproj`) menggunakan **Visual Studio**.
2. Lakukan build lewat **Build → Build Solution** atau tekan `Ctrl + Shift + B`.
3. Jalankan aplikasi dengan menekan tombol **Start** atau `F5`.
4. Uji coba form:
   - Isi kolom **Nama**, **NIM**, dan **KOM**.
   - Klik **Tampilkan** → pastikan `MessageBox` menampilkan data yang sesuai.
   - Klik **Hapus** → pastikan semua kolom kembali kosong dan fokus balik ke kolom Nama.
   - Klik **Keluar** → pastikan aplikasi tertutup dengan baik.

## 8. Kesimpulan

Praktikum ini menunjukkan bagaimana sebuah form sederhana dapat dibangun dari kombinasi komponen visual dasar (`Label`, `TextBox`, `Button`) yang saling terhubung lewat event `Click`. Selain memahami cara kerja *event-driven programming*, praktikum ini juga menekankan pentingnya kebiasaan baik dalam pengembangan WinForms — seperti pemilihan operator string yang tepat, cara membersihkan input yang efisien, perbedaan metode penutupan aplikasi, serta konsistensi konvensi penamaan kontrol agar kode lebih mudah dibaca dan dirawat ke depannya.

---

<p align="center"><i>Disusun sebagai bagian dari Laporan Praktikum Mata Kuliah Pemrograman Visual — Pertemuan 02.</i></p>
