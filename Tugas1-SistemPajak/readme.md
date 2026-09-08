# Tugas 1 — Aplikasi Login & Perhitungan Pajak

Aplikasi desktop berbasis **VB.NET Windows Forms** yang terdiri dari dua form: form **Login** (dengan validasi role, nama, dan NIM) dan form **Perhitungan Pajak** (berdasarkan jumlah pendapatan).

|---|---|
| **Mata Kuliah** | Pemrograman Visual |
| **Bahasa** | Visual Basic .NET (VB.NET) |
| **Platform** | .NET / Windows Forms |
| **Nama** | Tiwy Lamberkat Silaban|
| **NIM** | 241712001 |

---

## 📑 Daftar Isi
1. [Deskripsi Singkat](#1-deskripsi-singkat)
2. [Tujuan Pembelajaran](#2-tujuan-pembelajaran)
3. [Struktur Form](#3-struktur-form)
4. [Form Login](#4-form-login)
5. [Form Pajak](#5-form-pajak)
6. [Peta Event & Method](#6-peta-event--method)
7. [Cara Menjalankan](#7-cara-menjalankan)

---

## 1. Deskripsi Singkat

Aplikasi ini mensimulasikan alur kerja sederhana: pengguna login dengan memilih **role** (Staff/Manager) beserta **nama** dan **NIM**, lalu — jika data valid — diarahkan ke form kalkulator pajak untuk menghitung besaran pajak dari suatu jumlah pendapatan sesuai aturan bertingkat yang telah ditentukan.

## 2. Tujuan Pembelajaran

- Membuat project Windows Forms menggunakan VB.NET.
- Merancang form dengan komponen visual pada Toolbox & Windows Forms Designer.
- Menggunakan `TextBox`, `ComboBox`, `PictureBox`, `Label`, dan `Button`.
- Menangani event `SelectedIndexChanged`, `Click`, dan `KeyPress`.
- Melakukan validasi input teks dan angka.
- Menerapkan percabangan `If...Then...ElseIf...Else`.
- Menggunakan method `Show`, `Hide`, `Close`, `Clear`, dan `Focus`.
- Menampilkan informasi/hasil lewat `MessageBox`.

## 3. Struktur Form

| Form | Peran | Ditampilkan Saat |
|---|---|---|
| `FrmLogin` | Validasi role, nama, dan NIM pengguna | Pertama kali aplikasi dijalankan |
| `FrmPajak` | Menghitung pajak dari pendapatan yang diinput | Setelah login berhasil |

>  Tidak menggunakan database — data akun disimpan langsung di kode program.

## 4. Form Login

### 4.1 Komponen

| Komponen | Name | Fungsi |
|---|---|---|
| `PictureBox` | `picImage` | Menampilkan gambar sesuai role yang dipilih |
| `ComboBox` | `cmbRole` | Memilih role `Staff` atau `Manager` |
| `TextBox` | `txtNama` | Input nama pengguna |
| `TextBox` | `txtNIM` | Input NIM pengguna |
| `Button` | `btnLogin` | Memproses login |
| `Label` | `lblRole`, `lblNama`, `lblNIM` | Label penanda tiap kolom |

### 4.2 Alur Perubahan Gambar

`cmbRole.SelectedIndexChanged` → cek role terpilih → set `picImage.Image`:
- **Staff** → gambar staff
- **Manager** → gambar manager

### 4.3 Validasi Input

| Kolom | Aturan | Ditangani Lewat |
|---|---|---|
| `txtNama` | Hanya huruf & spasi | Event `KeyPress` |
| `txtNIM` | Hanya angka | Event `KeyPress` |

- NIM yang tidak valid → login dibatalkan, fokus kembali ke `txtNIM`.
- Spasi di awal/akhir input (nama & NIM) harus ditangani dengan benar (mis. `Trim()`) sebelum dibandingkan.

### 4.4 Data Akun Pengujian

| Role | Nama | NIM |
|---|---|---:|
| Staff | Nadya | 241712051 |
| Manager | Shata Diyaul Haq | 241712061 |

Validasi login harus mencocokkan **ketiga data sekaligus** (role + nama + NIM) — kombinasi lain dianggap gagal.

### 4.5 Perilaku Tombol Login

| Kondisi | Aksi |
|---|---|
| Data cocok | Buka `FrmPajak` (`Show`) → sembunyikan `FrmLogin` (`Hide`) → kosongkan `txtNama` & `txtNIM` (`Clear`) |
| Data tidak cocok | Tampilkan pesan error (`MessageBox.Show`) → tetap di `FrmLogin`, `FrmPajak` **tidak** dibuka |

## 5. Form Pajak

### 5.1 Komponen

| Komponen | Name | Fungsi |
|---|---|---|
| `Label` | `lblLimaJuta` | Info ketentuan pajak tingkat 1 |
| `Label` | `lblTigaPuluhJuta` | Info ketentuan pajak tingkat 2 |
| `Label` | `lblSeratusJuta` | Info ketentuan pajak tingkat 3 |
| `Label` | `lblPendapatan` | Petunjuk input pendapatan |
| `Label` | `lblRupiah` | Prefix mata uang `Rp.` |
| `TextBox` | `txtPendapatan` | Input jumlah pendapatan |
| `Button` | `btnHitung` | Menghitung pajak |
| `Button` | `btnKeluar` | Menutup form & kembali ke login |

### 5.2 Aturan Perhitungan Pajak

Gunakan tipe data `Decimal` untuk perhitungan agar presisi nominal rupiah terjaga.

| Pendapatan | Tarif Pajak |
|---|---:|
| ≤ Rp5.000.000 | 0% |
| > Rp5.000.000 s.d. Rp30.000.000 | 10% |
| > Rp30.000.000 s.d. Rp100.000.000 | 20% |
| > Rp100.000.000 | 30% |

```
Pajak = Pendapatan × Tarif Pajak
```

### 5.3 Perilaku Tombol Hitung

1. Baca nilai `txtPendapatan`.
2. Validasi apakah nilainya numerik.
   - Jika **tidak valid** → `MessageBox` peringatan → fokus kembali ke `txtPendapatan` → hentikan proses.
3. Tentukan tarif pajak sesuai tabel pada 5.2.
4. Hitung pajak dan tampilkan hasil lewat `MessageBox`.
5. Format nominal sebagai rupiah tanpa desimal, contoh: `Rp 1.000.000`.

### 5.4 Validasi Input Pendapatan

- `txtPendapatan` hanya menerima angka & tombol kontrol (mis. Backspace) — ditangani lewat `KeyPress`.
- Kasus uji minimum: input kosong, input huruf, nilai tepat di batas tarif, dan nilai di atas batas tertinggi.

### 5.5 Perilaku Tombol Keluar

`btnKeluar.Click` → `FrmPajak.Close()` → `FrmLogin.Show()`.

## 6. Peta Event & Method

| Event | Kontrol | Tujuan |
|---|---|---|
| `SelectedIndexChanged` | `cmbRole` | Mengganti gambar sesuai role |
| `Click` | `btnLogin` | Validasi akun & buka form pajak |
| `KeyPress` | `txtNama` | Membatasi input hanya huruf & spasi |
| `KeyPress` | `txtNIM` | Membatasi input hanya angka |
| `Click` | `btnHitung` | Menghitung & menampilkan pajak |
| `KeyPress` | `txtPendapatan` | Membatasi input hanya angka |
| `Click` | `btnKeluar` | Kembali ke form login |

**Method yang dipakai:** `Show()` · `Hide()` · `Close()` · `Clear()` · `Focus()` · `MessageBox.Show()`

## 7. Cara Menjalankan

1. Buka project ini di **Visual Studio**.
2. Pastikan folder `Image` (berisi aset gambar Staff/Manager) sudah ada dan diatur **Copy to Output Directory → Copy if newer**.
3. Build lewat **Build → Build Solution** (`Ctrl + Shift + B`).
4. Jalankan dengan `F5` — aplikasi akan membuka `FrmLogin` terlebih dahulu.
5. Login dengan salah satu akun pengujian pada [bagian 4.4](#44-data-akun-pengujian), lalu lanjutkan uji coba perhitungan pajak pada form berikutnya.

---

<p align="center"><i>Tugas 1 — Aplikasi Login dan Perhitungan Pajak · Mata Kuliah Pemrograman Visual</i></p>
