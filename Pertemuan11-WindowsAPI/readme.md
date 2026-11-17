# Pertemuan 11: Windows API pada VB.NET

**DemoWindowsApi** adalah aplikasi Windows Forms berupa *timer presentasi* yang memanfaatkan fungsi-fungsi Windows API lewat P/Invoke. Aplikasi ini membuktikan bahwa fitur yang tidak disediakan langsung oleh .NET (jendela selalu di atas, shortcut global, bunyi sistem) tetap bisa dipakai dengan memanggil pustaka bawaan Windows.

## Daftar Isi

1. [Konsep Windows API](#1-konsep-windows-api)
2. [Cara Memanggil API dari VB.NET](#2-cara-memanggil-api-dari-vbnet)
3. [Struktur Proyek](#3-struktur-proyek)
4. [Fungsi API yang Dipakai](#4-fungsi-api-yang-dipakai)
5. [Alur Kerja Aplikasi](#5-alur-kerja-aplikasi)
6. [Menjalankan Proyek](#6-menjalankan-proyek)
7. [Cara Menggunakan Aplikasi](#7-cara-menggunakan-aplikasi)
8. [Catatan dan Praktik Baik](#8-catatan-dan-praktik-baik)

---

## 1. Konsep Windows API

**Windows API** (Application Programming Interface) adalah kumpulan fungsi bawaan sistem operasi Windows yang disimpan dalam pustaka DLL. Semua aplikasi Windows, termasuk .NET, pada akhirnya memanggil fungsi-fungsi ini untuk membuat jendela, membaca keyboard, memutar bunyi, dan sebagainya.

| DLL | Isi |
|---|---|
| `kernel32.dll` | Fungsi inti: waktu, memori, proses, file |
| `user32.dll` | Jendela, pesan, input, shortcut |
| `gdi32.dll` | Grafik dan penggambaran |

.NET membungkus sebagian besar fungsi ini ke dalam kelas seperti `Form` atau `Timer`. Namun tidak semua tersedia, sehingga kadang kita perlu memanggil API-nya langsung. Teknik ini disebut **P/Invoke** (*Platform Invocation Services*).

---

## 2. Cara Memanggil API dari VB.NET

Ada dua gaya penulisan, dan keduanya dipakai pada proyek ini.

### 2.1 `Declare Function` (gaya klasik VB)

```vb
Public Declare Function MessageBeep Lib "user32.dll" (
    uType As UInteger) As Boolean
```

Ringkas dan cocok untuk fungsi sederhana.

### 2.2 `<DllImport>` (gaya .NET)

```vb
<DllImport("user32.dll", ExactSpelling:=True, SetLastError:=True)>
Public Function SetWindowPos(hWnd As IntPtr, hWndInsertAfter As IntPtr,
    x As Integer, y As Integer, cx As Integer, cy As Integer,
    uFlags As UInteger) As Boolean
End Function
```

Lebih fleksibel karena atribut bisa diatur:

| Opsi | Fungsi |
|---|---|
| `ExactSpelling:=True` | Memakai nama fungsi persis seperti ditulis, tanpa mencari varian `A`/`W` |
| `SetLastError:=True` | Menyimpan kode error Windows agar bisa dibaca dengan `Marshal.GetLastPInvokeError()` |

### 2.3 Padanan tipe data

| Tipe Windows | Tipe VB.NET |
|---|---|
| `HWND` (handle jendela) | `IntPtr` |
| `UINT`, `DWORD` | `UInteger` |
| `int` | `Integer` |
| `BOOL` | `Boolean` |
| `ULONGLONG` | `ULong` |

---

## 3. Struktur Proyek

```
DemoWindowsApi/
├── DemoWindowsApi.sln          Solution Visual Studio
├── DemoWindowsApi.vbproj       Konfigurasi proyek (.NET 10, Windows Forms)
├── Program.vb                  Titik masuk aplikasi (Sub Main)
├── ModWindowsApi.vb            Deklarasi API, konstanta, fungsi bantu
├── FrmDemoApi.vb               Logika form (timer, shortcut, event)
├── FrmDemoApi.Designer.vb      Tata letak kontrol (dibuat designer)
└── FrmDemoApi.resx             Resource form
```

| File | Peran |
|---|---|
| `Program.vb` | Mengatur DPI, visual style, lalu menjalankan `FrmDemoApi` |
| `ModWindowsApi.vb` | Satu tempat untuk semua deklarasi API dan konstantanya |
| `FrmDemoApi.vb` | Memakai API dan mengatur perilaku timer |

Proyek memakai `Option Strict On`, sehingga semua konversi tipe harus eksplisit.

### Kontrol pada form

| Kontrol | Fungsi |
|---|---|
| `lblSisa` | Menampilkan sisa waktu (`mm:ss`) |
| `lblTahap`, `lblKeterangan` | Status dan petunjuk sesuai kondisi timer |
| `btnMulai`, `btnUlangi` | Mulai/jeda dan reset |
| `nudMenit`, `nudDetik` | Mengatur durasi (menit 0-180, detik 0-59) |
| `chkTeratas` | Mengaktifkan mode selalu di atas |
| `chkBunyi` | Membunyikan notifikasi saat waktu habis |
| `btnRingkas` | Menyembunyikan panel pengaturan |
| `lblShortcut`, `lblStatus` | Info shortcut dan pesan status |
| `tmrHitung` | `Timer` yang memperbarui tampilan |

---

## 4. Fungsi API yang Dipakai

Proyek ini memakai lima fungsi API, masing-masing untuk kebutuhan berbeda.

### 4.1 `GetTickCount64` (kernel32)

Mengembalikan jumlah milidetik sejak Windows dinyalakan. Dipakai sebagai jam acuan untuk menghitung waktu yang telah berlalu.

```vb
Public Declare Function GetTickCount64 Lib "kernel32.dll" () As ULong
```

Penghitungan sisa waktu:

```vb
Dim elapsed As ULong = terpakai + GetTickCount64() - mulaiTick
Dim sisa As ULong = HitungSisa(durasi, elapsed)
```

Waktu dihitung dari selisih tick, bukan dari jumlah *tick* `Timer`. Dengan begitu hasilnya tetap akurat meski `Timer` Windows Forms terlambat memanggil event.

### 4.2 `MessageBeep` (user32)

Memutar bunyi sistem. Dipanggil saat waktu habis.

```vb
If chkBunyi.Checked Then
    If Not MessageBeep(MB_ICONINFORMATION) Then
        lblStatus.Text = "Waktu habis. Bunyi tidak dapat diputar; penanda visual tetap aktif."
    End If
End If
```

Hasil `Boolean` diperiksa; bila gagal, pengguna tetap diberi tahu lewat penanda visual (panel berubah warna).

### 4.3 `SetWindowPos` (user32)

Mengatur posisi dan urutan tumpukan (*z-order*) jendela. Dipakai untuk fitur **Tetap di atas aplikasi lain**.

| Konstanta | Nilai | Arti |
|---|---|---|
| `HWND_TOPMOST` | `-1` | Jendela selalu berada di atas jendela biasa |
| `HWND_NOTOPMOST` | `-2` | Kembali ke urutan normal |
| `SWP_NOMOVE` | `&H2` | Jangan ubah posisi |
| `SWP_NOSIZE` | `&H1` | Jangan ubah ukuran |
| `SWP_NOACTIVATE` | `&H10` | Jangan rebut fokus |

```vb
Dim posisi As IntPtr = If(chkTeratas.Checked, HWND_TOPMOST, HWND_NOTOPMOST)
Dim flags As UInteger = SWP_NOMOVE Or SWP_NOSIZE Or SWP_NOACTIVATE
If SetWindowPos(Me.Handle, posisi, 0, 0, 0, 0, flags) Then
    ' berhasil
Else
    Dim kode As Integer = Marshal.GetLastPInvokeError()
    ' batalkan centang dan tampilkan kode error
End If
```

Jika pemanggilan gagal, kode error Windows diambil dan *checkbox* dikembalikan ke kondisi sebelumnya. Variabel `mengubahPin` mencegah pembatalan itu memicu event berulang.

### 4.4 `RegisterHotKey` dan `UnregisterHotKey` (user32)

Mendaftarkan **shortcut global**, yaitu tombol pintas yang tetap bekerja walaupun jendela aplikasi tidak aktif. Ini berguna saat presentasi, ketika fokus ada di aplikasi slide.

| Shortcut | ID | Fungsi |
|---|---|---|
| `Ctrl + Alt + F9` | `ID_MULAI` (1) | Mulai atau jeda |
| `Ctrl + Alt + F10` | `ID_ULANGI` (2) | Ulangi timer |

Pendaftaran dilakukan saat *handle* jendela dibuat, dan pembatalannya saat handle dihancurkan:

```vb
Private Sub FrmDemoApi_HandleCreated(...) Handles MyBase.HandleCreated
    handleShortcut = Me.Handle
    Dim modifier As UInteger = MOD_CONTROL Or MOD_ALT Or MOD_NOREPEAT
    shortcutMulai  = RegisterHotKey(handleShortcut, ID_MULAI,  modifier, VK_F9)
    shortcutUlangi = RegisterHotKey(handleShortcut, ID_ULANGI, modifier, VK_F10)
    ...
End Sub

Private Sub FrmDemoApi_HandleDestroyed(...) Handles MyBase.HandleDestroyed
    If shortcutMulai  Then UnregisterHotKey(handleShortcut, ID_MULAI)
    If shortcutUlangi Then UnregisterHotKey(handleShortcut, ID_ULANGI)
    ...
End Sub
```

`MOD_NOREPEAT` mencegah shortcut terpicu berulang bila tombol ditahan.

Karena shortcut bisa saja sudah dipakai aplikasi lain, hasil pendaftaran disimpan di `shortcutMulai` dan `shortcutUlangi`. Jika gagal, label menampilkan "Shortcut tidak tersedia" dan tombol di layar tetap berfungsi.

### 4.5 Menerima pesan hotkey lewat `WndProc`

Windows memberi tahu aplikasi bahwa shortcut ditekan dengan mengirim pesan `WM_HOTKEY` (`&H312`). Pesan ini ditangkap dengan mengganti (*override*) `WndProc`:

```vb
Protected Overrides Sub WndProc(ByRef m As Message)
    If m.Msg = WM_HOTKEY Then
        If m.WParam.ToInt32() = ID_MULAI  AndAlso shortcutMulai  Then MulaiAtauJeda()
        If m.WParam.ToInt32() = ID_ULANGI AndAlso shortcutUlangi Then UlangiTimer()
    End If
    MyBase.WndProc(m)
End Sub
```

`WParam` berisi ID shortcut yang ditekan. `MyBase.WndProc(m)` wajib tetap dipanggil agar pesan lain diproses seperti biasa.

---

## 5. Alur Kerja Aplikasi

```
Form dibuat
   |-- HandleCreated  -> RegisterHotKey (Ctrl+Alt+F9, F10)
   |-- Load           -> UlangiTimer() (siapkan durasi awal)
   v
Pengguna menekan Mulai (tombol atau Ctrl+Alt+F9)
   |-- simpan GetTickCount64() sebagai mulaiTick
   |-- tmrHitung.Start()
   v
Tiap tick Timer
   |-- elapsed = terpakai + (tick sekarang - mulaiTick)
   |-- sisa = durasi - elapsed  -> tampilkan mm:ss
   v
sisa = 0 -> WaktuHabis()
   |-- panel berubah warna, teks berubah
   |-- MessageBeep (jika diaktifkan)
```

### Fungsi bantu di `ModWindowsApi`

```vb
Public Function HitungSisa(durasi As ULong, terpakai As ULong) As ULong
    If terpakai >= durasi Then Return 0UL
    Return durasi - terpakai
End Function
```

Pengecekan `terpakai >= durasi` penting karena `ULong` tidak boleh negatif; tanpa pengecekan ini hasil pengurangan akan *overflow*.

```vb
Public Function FormatSisa(milidetik As ULong) As String
    Dim detik As ULong = milidetik \ 1000UL
    If milidetik Mod 1000UL > 0UL Then detik += 1UL
    Return $"{detik \ 60UL:00}:{detik Mod 60UL:00}"
End Function
```

Sisa milidetik dibulatkan ke atas, sehingga tampilan `00:00` baru muncul saat waktu benar-benar habis.

### Jeda dan lanjut

Saat dijeda, waktu yang sudah lewat disimpan ke `terpakai`:

```vb
terpakai += GetTickCount64() - mulaiTick
```

Saat dilanjutkan, `mulaiTick` diambil ulang, sehingga waktu selama jeda tidak ikut terhitung.

---

## 6. Menjalankan Proyek

**Syarat:** Windows, Visual Studio 2022 atau lebih baru (dengan workload *.NET desktop development*), dan .NET SDK sesuai `TargetFramework` pada `DemoWindowsApi.vbproj` (`net10.0-windows`).

1. Buka `DemoWindowsApi.sln` di Visual Studio.
2. Pastikan file designer bernama `FrmDemoApi.Designer.vb` agar form terbaca di Designer.
3. Tekan **F5** untuk menjalankan.

Atau lewat terminal:

```
dotnet run --project DemoWindowsApi.vbproj
```

Karena memakai `user32.dll` dan `kernel32.dll`, aplikasi hanya berjalan di Windows.

---

## 7. Cara Menggunakan Aplikasi

1. Atur durasi pada kolom **menit** dan **detik**.
2. Centang **Tetap di atas aplikasi lain** agar timer tidak tertutup aplikasi slide.
3. Tekan **Mulai presentasi** atau `Ctrl+Alt+F9`. Tombol berubah menjadi **Jeda**.
4. Gunakan **Mode ringkas** untuk menyembunyikan panel pengaturan dan memperkecil jendela.
5. Saat waktu habis, panel berubah warna dan terdengar bunyi (jika opsi bunyi aktif).
6. Tekan **Ulangi** atau `Ctrl+Alt+F10` untuk mengatur sesi berikutnya.

Durasi hanya bisa diubah saat timer belum berjalan. Setelah dijeda, pilih **Ulangi** untuk mengubah durasi.

---

## 8. Catatan dan Praktik Baik

- **Selalu periksa nilai kembalian API.** Fungsi seperti `SetWindowPos` dan `RegisterHotKey` bisa gagal; aplikasi ini menampilkan pesan status dan tidak crash.
- **Gunakan `SetLastError:=True`** pada `DllImport` bila ingin membaca kode error dengan `Marshal.GetLastPInvokeError()`. Pada .NET lama, padanannya adalah `Marshal.GetLastWin32Error()`.
- **Kumpulkan deklarasi API dalam satu modul** (`ModWindowsApi`) agar mudah dicari dan dipakai ulang.
- **Bersihkan sumber daya.** Shortcut yang didaftarkan harus dibatalkan saat jendela ditutup (`HandleDestroyed`).
- **Pakai konstanta bernama**, bukan angka mentah seperti `&H312`, agar kode terbaca jelas.
- **Cocokkan tipe data** dengan dokumentasi API (misalnya `IntPtr` untuk handle). Tipe yang salah bisa menyebabkan error yang sulit dilacak.
- Dokumentasi resmi fungsi-fungsinya ada di Microsoft Learn (cari nama fungsi, misalnya `RegisterHotKey function`).
