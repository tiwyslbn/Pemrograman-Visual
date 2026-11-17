# Laporan Praktikum Pemrograman Visual
### Pertemuan 12 — Administrasi Mahasiswa dengan MDI

| | |
|---|---|
| **Nama** | Tiwy Lamberkat Silaban |
| **NIM** | 241712001 |
| **Kelas** | KOM A1 |
| **Mata Kuliah** | Praktikum Pemrograman Visual |
| **Pertemuan Ke-** | 12 | 

Proyek **AdministrasiMahasiswaMdi** adalah aplikasi Windows Forms (VB.NET, .NET 10) yang dirancang memakai **MDI** (*Multiple Document Interface*): satu jendela induk yang menampung beberapa jendela anak di dalamnya.


## 1. Konsep MDI

Pada aplikasi MDI terdapat dua jenis form:

| Jenis | Peran |
|---|---|
| **Form induk** (*MDI parent / container*) | Wadah utama, biasanya memuat menu dan toolbar |
| **Form anak** (*MDI child*) | Jendela kerja yang tampil di dalam area form induk |

Contohnya Microsoft Word versi lama atau Photoshop: satu jendela besar, banyak dokumen di dalamnya. Pada administrasi mahasiswa, bentuknya bisa seperti ini:

```
+--------------------------------------------------+
| Form1 (induk)                                    |
| [Data] [Jendela] [Keluar]            <- MenuStrip|
|--------------------------------------------------|
|   +-----------------+  +-----------------+       |
|   | Data Mahasiswa  |  | Data Mata Kuliah|       |
|   |  (form anak)    |  |  (form anak)    |       |
|   +-----------------+  +-----------------+       |
+--------------------------------------------------+
```

Kelebihan MDI:

- Semua fitur ada dalam satu jendela, sehingga tampilan tertata.
- Beberapa form dapat dibuka bersamaan.
- Menu dan toolbar dipakai bersama oleh semua form anak.
- Jendela anak tidak bisa keluar dari batas jendela induk.

---

## 2. Status Proyek Saat Ini

File yang ada saat README ini ditulis masih berupa **kerangka awal**:

| Komponen | Kondisi |
|---|---|
| `Form1` | Form kosong (800 x 450, judul "Form1"), belum menjadi MDI container |
| Menu / toolbar | Belum ada |
| Form anak | Belum ada |
| Pengaturan aplikasi | `Form1` ditetapkan sebagai form utama, tutup `Form1` berarti aplikasi berhenti |

Bagian 4 sampai 6 menjelaskan **langkah pengembangan** untuk menjadikannya aplikasi MDI yang utuh. Nama `FrmMahasiswa` dan `FrmMataKuliah` adalah contoh; sesuaikan dengan form yang Anda buat.

---

## 3. Struktur Proyek

```
AdministrasiMahasiswaMdi/
├── AdministrasiMahasiswaMdi.slnx       Solution
├── AdministrasiMahasiswaMdi.vbproj     Konfigurasi proyek
├── Form1.vb                            Logika form induk
├── Form1.Designer.vb                   Tata letak form induk
├── Form1.resx                          Resource form
└── My Project/
    ├── Application.myapp               Pengaturan aplikasi
    ├── Application.Designer.vb         Kode hasil generate (jangan diubah manual)
    └── ApplicationEvents.vb            Event tingkat aplikasi
```

Hal yang diatur proyek ini:

| Pengaturan | Nilai |
|---|---|
| Target framework | `net10.0-windows` |
| Jenis keluaran | `WinExe` (aplikasi Windows) |
| Form utama | `Form1` |
| Mode tutup | Aplikasi berhenti saat form utama ditutup |
| Visual styles | Aktif |
| Single instance | Nonaktif |

---

## 4. Langkah Membuat Form Induk MDI

### 4.1 Jadikan `Form1` sebagai container

Di jendela **Properties** `Form1`, atur:

| Properti | Nilai |
|---|---|
| `IsMdiContainer` | `True` |
| `Text` | `Administrasi Mahasiswa` |
| `WindowState` | `Maximized` (opsional) |

Bila diatur lewat kode:

```vb
Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.IsMdiContainer = True
    End Sub
End Class
```

Area form berubah menjadi abu-abu: itulah ruang untuk form anak.

### 4.2 Tambahkan menu

Tarik **MenuStrip** dari Toolbox ke `Form1`, lalu buat susunan berikut:

| Menu | Submenu | Nama kontrol (saran) |
|---|---|---|
| **Data** | Data Mahasiswa | `mnuMahasiswa` |
| | Data Mata Kuliah | `mnuMataKuliah` |
| | Keluar | `mnuKeluar` |
| **Jendela** | Cascade | `mnuCascade` |
| | Tile Horizontal | `mnuTileH` |
| | Tile Vertical | `mnuTileV` |
| | Rapikan Ikon | `mnuIkon` |
| | Tutup Semua | `mnuTutupSemua` |

Untuk menu **Jendela**, set properti `MdiWindowListItem` pada MenuStrip agar daftar form anak yang terbuka muncul otomatis di menu tersebut.

---

## 5. Membuat dan Membuka Form Anak

### 5.1 Membuat form anak

**Project > Add Form (Windows Forms)**, beri nama misalnya `FrmMahasiswa`, lalu isi dengan kontrol yang dibutuhkan (DataGridView, TextBox, tombol, dan sebagainya). Form anak dibuat seperti form biasa; yang menjadikannya "anak" adalah properti `MdiParent` saat dibuka.

### 5.2 Membuka form anak

```vb
Private Sub mnuMahasiswa_Click(sender As Object, e As EventArgs) Handles mnuMahasiswa.Click
    Dim anak As New FrmMahasiswa()
    anak.MdiParent = Me
    anak.Show()
End Sub
```

Dua hal yang wajib diperhatikan:

- `MdiParent` harus diisi **sebelum** `Show()`.
- Gunakan `Show()`, bukan `ShowDialog()`. `ShowDialog` menampilkan form modal dan tidak berlaku sebagai form anak MDI.

### 5.3 Mencegah form terbuka ganda

Tanpa pemeriksaan, setiap klik menu membuka jendela baru. Prosedur generik berikut membuka form baru hanya bila belum ada, dan mengaktifkannya bila sudah terbuka:

```vb
Private Sub BukaForm(Of T As {Form, New})()
    For Each f As Form In Me.MdiChildren
        If TypeOf f Is T Then
            f.Activate()
            Return
        End If
    Next

    Dim baru As New T()
    baru.MdiParent = Me
    baru.Show()
End Sub

Private Sub mnuMahasiswa_Click(sender As Object, e As EventArgs) Handles mnuMahasiswa.Click
    BukaForm(Of FrmMahasiswa)()
End Sub

Private Sub mnuMataKuliah_Click(sender As Object, e As EventArgs) Handles mnuMataKuliah.Click
    BukaForm(Of FrmMataKuliah)()
End Sub
```

`MdiChildren` adalah daftar semua form anak yang sedang terbuka.

### 5.4 Keluar dari aplikasi

```vb
Private Sub mnuKeluar_Click(sender As Object, e As EventArgs) Handles mnuKeluar.Click
    Me.Close()
End Sub
```

Karena `Form1` adalah form utama, menutupnya menghentikan seluruh aplikasi.

---

## 6. Mengatur Tata Letak Jendela

Metode `LayoutMdi` menata semua form anak sekaligus:

| Nilai `MdiLayout` | Hasil |
|---|---|
| `Cascade` | Jendela bertumpuk berundak |
| `TileHorizontal` | Jendela tersusun atas-bawah |
| `TileVertical` | Jendela tersusun kiri-kanan |
| `ArrangeIcons` | Ikon jendela yang diminimalkan dirapikan |

```vb
Private Sub mnuCascade_Click(sender As Object, e As EventArgs) Handles mnuCascade.Click
    Me.LayoutMdi(MdiLayout.Cascade)
End Sub

Private Sub mnuTileH_Click(sender As Object, e As EventArgs) Handles mnuTileH.Click
    Me.LayoutMdi(MdiLayout.TileHorizontal)
End Sub

Private Sub mnuTileV_Click(sender As Object, e As EventArgs) Handles mnuTileV.Click
    Me.LayoutMdi(MdiLayout.TileVertical)
End Sub

Private Sub mnuIkon_Click(sender As Object, e As EventArgs) Handles mnuIkon.Click
    Me.LayoutMdi(MdiLayout.ArrangeIcons)
End Sub
```

Menutup semua form anak:

```vb
Private Sub mnuTutupSemua_Click(sender As Object, e As EventArgs) Handles mnuTutupSemua.Click
    For Each f As Form In Me.MdiChildren
        f.Close()
    Next
End Sub
```

---

## 7. Properti dan Event Penting

| Anggota | Berlaku pada | Fungsi |
|---|---|---|
| `IsMdiContainer` | Form induk | Menjadikan form sebagai wadah MDI |
| `MdiParent` | Form anak | Menunjuk form induknya |
| `MdiChildren` | Form induk | Array semua form anak yang terbuka |
| `ActiveMdiChild` | Form induk | Form anak yang sedang aktif (bernilai `Nothing` bila tidak ada) |
| `MdiChildActivate` | Form induk | Event saat form anak aktif berganti |
| `LayoutMdi(...)` | Form induk | Menata jendela anak |
| `MdiWindowListItem` | MenuStrip | Menu yang otomatis memuat daftar jendela anak |

Contoh memakai `ActiveMdiChild` dan `MdiChildActivate`:

```vb
Private Sub Form1_MdiChildActivate(sender As Object, e As EventArgs) Handles MyBase.MdiChildActivate
    If Me.ActiveMdiChild IsNot Nothing Then
        Me.Text = "Administrasi Mahasiswa - " & Me.ActiveMdiChild.Text
    Else
        Me.Text = "Administrasi Mahasiswa"
    End If
End Sub
```

---

## 8. Menjalankan Proyek

**Syarat:** Windows, Visual Studio 2022 atau lebih baru dengan workload *.NET desktop development*, serta .NET SDK yang mendukung `net10.0-windows`.

1. Buka `AdministrasiMahasiswaMdi.slnx` di Visual Studio.
2. Tekan **F5** untuk menjalankan.

Atau lewat terminal:

```
dotnet run --project AdministrasiMahasiswaMdi/AdministrasiMahasiswaMdi.vbproj
```

Aplikasi hanya berjalan di Windows.

---

## 9. Masalah Umum

| Gejala | Penyebab | Solusi |
|---|---|---|
| Form anak tampil terpisah, tidak di dalam induk | `MdiParent` belum diisi, atau memakai `ShowDialog()` | Isi `MdiParent = Me` lalu panggil `Show()` |
| `InvalidOperationException` saat mengisi `MdiParent` | Form induk belum `IsMdiContainer = True` | Aktifkan `IsMdiContainer` pada form induk |
| Form yang sama terbuka berkali-kali | Tidak ada pemeriksaan sebelum membuat form baru | Pakai prosedur `BukaForm(Of T)` pada bagian 5.3 |
| Daftar jendela tidak muncul di menu Jendela | `MdiWindowListItem` belum diatur | Pilih menu **Jendela** pada properti `MdiWindowListItem` milik MenuStrip |
| Kontrol di induk tertutup area abu-abu | Kontrol (misalnya Panel) menutupi area MDI | Gunakan MenuStrip, ToolStrip, atau StatusStrip dengan `Dock`, hindari panel besar |
| Menu anak ikut menyatu dengan menu induk | Perilaku bawaan *menu merging* | Atur `AllowMerge` pada MenuStrip bila tidak diinginkan |
