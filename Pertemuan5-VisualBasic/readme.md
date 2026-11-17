# Laporan Praktikum Pemrograman Visual
### Pertemuan 05 — Modul, Data, Fungsi, dan Array pada Visual Basic .NET

| | |
|---|---|
| **Nama** | Tiwy Lamberkat Silaban |
| **NIM** | 241712001 |
| **Kelas** | KOM A1 |
| **Mata Kuliah** | Praktikum Pemrograman Visual |
| **Pertemuan Ke-** | 5 |

Catatan praktikum ini membahas empat topik dasar: modul, variabel dan tipe data, fungsi dan prosedur, serta array. Seluruh contoh memakai tema **toko** (barang, harga, stok).

---

## 1. Modul

### 1.1 Apa itu modul?

*Module* adalah wadah untuk mengelompokkan kode seperti variabel, konstanta, prosedur, dan fungsi dalam satu unit. Sifatnya statis: anggota modul bisa langsung dipakai tanpa membuat objek lebih dulu.

Manfaat memakai modul:

- Kode lebih tertata dan mudah dirawat.
- Logika program dipisah sesuai tugasnya.
- Kode yang sama dapat dipakai ulang di banyak tempat.

### 1.2 Jenis modul

| Jenis | Penjelasan |
|---|---|
| Standard Module | Menampung variabel, konstanta, prosedur, dan fungsi bersifat global (`Module ... End Module`). |
| Class Module | Mendefinisikan kelas untuk pemrograman berorientasi objek (`Class ... End Class`). |
| Form Module | Terikat pada satu form dan berisi kode penanganan event komponen di form itu. |

Perbandingan Module dan Class:

```vb
' Standard Module: langsung dipanggil
Module Hitung
    Function JumlahkanHarga(a As Integer, b As Integer) As Integer
        Return a + b
    End Function
End Module

' Class: harus dibuat objeknya dulu dengan New
Class Barang
    Public Nama As String
    Public Kode As String
End Class
```

Pemakaiannya:

```vb
Dim total As Integer = Hitung.JumlahkanHarga(5000, 7500)   ' tanpa New

Dim b As New Barang()                                       ' wajib New
b.Nama = "Pensil"
```

### 1.3 Struktur penulisan

```vb
Module NamaModul
    ' variabel tingkat modul
    Dim totalTransaksi As Integer

    Sub TampilkanSapaan()
        MessageBox.Show("Selamat datang di toko kami")
    End Sub

    Function Lipatkan(angka As Integer) As Integer
        Return angka * 2
    End Function
End Module
```

Isi modul dapat dipanggil dari modul lain dalam satu proyek, kecuali dibatasi dengan *access modifier*.

### 1.4 Cakupan akses (scope)

- `Public`: dapat diakses dari mana saja, termasuk modul dan kelas lain.
- `Private`: hanya dapat diakses dari dalam modul itu sendiri.

```vb
Module DataToko
    Public NamaToko As String = "Toko Sejahtera"
    Private jumlahPelanggan As Integer = 0

    Public Sub CatatPelanggan()
        jumlahPelanggan += 1
    End Sub
End Module
```

`NamaToko` bisa dibaca dari modul lain, sedangkan `jumlahPelanggan` hanya bisa diubah lewat `CatatPelanggan` di dalam modul yang sama.

---

## 2. Data: Variabel dan Tipe Data

### 2.1 Variabel

Variabel adalah lokasi di memori yang diberi nama untuk menyimpan nilai yang boleh berubah saat program berjalan. Tipe datanya menentukan jenis nilai yang boleh disimpan.

### 2.2 Aturan penamaan

- Harus diawali huruf, bukan angka atau simbol.
- Tidak boleh ada spasi atau karakter khusus, kecuali garis bawah (`_`).
- Tidak boleh sama dengan kata kunci VB seperti `Dim`, `Module`, atau `Function`.
- Pilih nama yang menggambarkan isinya, misalnya `namaBarang`, `hargaSatuan`.

### 2.3 Tipe data dasar

| Tipe | Ukuran | Kegunaan | Contoh |
|---|---|---|---|
| Integer | 4 byte | Bilangan bulat | `10`, `-5` |
| Long | 8 byte | Bilangan bulat besar | `10000000000` |
| Single | 4 byte | Pecahan presisi tunggal | `3.14` |
| Double | 8 byte | Pecahan presisi ganda | `3.14159265` |
| Decimal | 16 byte | Pecahan presisi tinggi (uang) | `12500.75` |
| String | variabel | Teks | `"Visual Basic"` |
| Char | 2 byte | Satu karakter | `"A"c` |
| Boolean | 2 byte | Nilai logika | `True` / `False` |
| Date | 8 byte | Tanggal dan waktu | `#12/25/2026#` |
| Object | variabel | Menampung tipe apa pun | - |

### 2.4 Deklarasi

Format dasar: `Dim <nama> As <tipe>`.

```vb
Dim namaBarang As String
Dim stok As Integer
Dim harga As Decimal = 15000D
Dim tersedia As Boolean = True
```

Kata kunci deklarasi lain:

| Kata kunci | Fungsi |
|---|---|
| `Public` | Bisa diakses dari luar modul/kelas |
| `Private` | Hanya bisa diakses di modul/kelas yang sama |
| `Static` | Nilai tidak di-reset antarpemanggilan prosedur |
| `Const` | Nilai tetap yang tidak bisa diubah |

```vb
Const PPN As Double = 0.11
```

Contoh `Static` (nilai bertahan antarpemanggilan):

```vb
Sub HitungPemanggilan()
    Static jumlah As Integer = 0
    jumlah += 1
    Console.WriteLine("Dipanggil " & jumlah & " kali")
End Sub
```

### 2.5 Konversi tipe data

| Fungsi | Hasil |
|---|---|
| `CInt()` | Integer |
| `CStr()` | String |
| `CDbl()` | Double |
| `CBool()` | Boolean |
| `Val()` | Mengambil angka dari awal string |

```vb
Dim teksStok As String = "25"
Dim stokAngka As Integer = CInt(teksStok)
Console.WriteLine(stokAngka + 5)   ' 30
```

---

## 3. Fungsi dan Prosedur

### 3.1 Sub

`Sub` menjalankan sekumpulan perintah **tanpa** mengembalikan nilai.

```vb
Sub CetakBarang(nama As String, stok As Integer)
    Console.WriteLine("Barang : " & nama)
    Console.WriteLine("Stok   : " & stok)
End Sub

CetakBarang("Buku Tulis", 40)
```

### 3.2 Function

`Function` mengembalikan nilai lewat `Return`.

```vb
Function HitungSubtotal(harga As Double, jumlah As Integer) As Double
    Return harga * jumlah
End Function

Dim subtotal As Double = HitungSubtotal(3500, 4)
Console.WriteLine("Subtotal: " & subtotal)   ' 14000
```

### 3.3 Parameter ByVal dan ByRef

- **ByVal** (default): yang dikirim adalah salinan nilai, sehingga variabel asli tidak berubah.
- **ByRef**: yang dikirim adalah referensi, sehingga perubahan di dalam prosedur ikut mengubah variabel asli.

```vb
Sub TambahStokByVal(ByVal stok As Integer)
    stok += 10
End Sub

Sub TambahStokByRef(ByRef stok As Integer)
    stok += 10
End Sub

Dim stokAwal As Integer = 5
TambahStokByVal(stokAwal)
Console.WriteLine(stokAwal)   ' 5  (tetap)

TambahStokByRef(stokAwal)
Console.WriteLine(stokAwal)   ' 15 (berubah)
```

### 3.4 Overloading

*Overloading* berarti beberapa fungsi bernama sama tetapi berbeda jumlah atau tipe parameternya. Kompiler memilih versi yang cocok berdasarkan argumen yang diberikan.

```vb
Overloads Function Gabung(a As Integer, b As Integer) As Integer
    Return a + b
End Function

Overloads Function Gabung(a As Double, b As Double) As Double
    Return a + b
End Function

Console.WriteLine(Gabung(2, 3))        ' 5
Console.WriteLine(Gabung(2.5, 3.5))    ' 6
```

### 3.5 Fungsi rekursif

Fungsi rekursif memanggil dirinya sendiri sampai mencapai kondisi berhenti (*base case*). Contoh klasik adalah faktorial.

```vb
Function Faktorial(n As Integer) As Integer
    If n <= 1 Then
        Return 1
    Else
        Return n * Faktorial(n - 1)
    End If
End Function

Console.WriteLine(Faktorial(5))   ' 120
```

Tanpa kondisi berhenti, fungsi akan memanggil dirinya tanpa akhir dan program error (*stack overflow*).

---

## 4. Array

### 4.1 Pengertian

Array menyimpan banyak data bertipe sama dalam satu variabel, dan tiap elemen diakses lewat **indeks** yang dimulai dari 0. Dengan array kita tidak perlu membuat variabel terpisah untuk setiap data.

### 4.2 Deklarasi

```vb
' Ukuran tetap: 5 elemen (indeks 0 sampai 4)
Dim hargaBarang(4) As Integer

' Langsung diisi
Dim kategori() As String = {"Alat Tulis", "Makanan", "Minuman"}
```

Angka dalam kurung pada `Dim hargaBarang(4)` adalah **indeks tertinggi**, bukan jumlah elemen.

### 4.3 Array satu dimensi

```vb
Dim stokBarang(4) As Integer

stokBarang(0) = 20
stokBarang(1) = 35
stokBarang(2) = 12
stokBarang(3) = 50
stokBarang(4) = 8

For i As Integer = 0 To stokBarang.Length - 1
    Console.WriteLine("Barang ke-" & (i + 1) & ": " & stokBarang(i))
Next
```

### 4.4 Array multi dimensi

Dipakai untuk data berbentuk tabel (baris dan kolom).

```vb
' 3 baris x 2 kolom: kolom 0 = harga, kolom 1 = stok
Dim data(2, 1) As Integer

data(0, 0) = 2000 : data(0, 1) = 10
data(1, 0) = 5000 : data(1, 1) = 25
data(2, 0) = 7500 : data(2, 1) = 5

For baris As Integer = 0 To 2
    For kolom As Integer = 0 To 1
        Console.Write(data(baris, kolom) & " ")
    Next
    Console.WriteLine()
Next
```

### 4.5 Metode bawaan array

| Metode / properti | Fungsi |
|---|---|
| `Array.Sort(arr)` | Mengurutkan elemen |
| `Array.Reverse(arr)` | Membalik urutan elemen |
| `arr.Length` | Jumlah elemen |
| `Array.IndexOf(arr, nilai)` | Mencari indeks suatu nilai (`-1` jika tidak ada) |
| `Array.Clear(arr, index, length)` | Mengosongkan sebagian elemen |

```vb
Dim harga() As Integer = {7500, 2000, 12000, 500, 9000}

Array.Sort(harga)
' {500, 2000, 7500, 9000, 12000}

Array.Reverse(harga)
' {12000, 9000, 7500, 2000, 500}

Console.WriteLine(Array.IndexOf(harga, 7500))   ' 2
```

### 4.6 Perulangan pada array

`For Each` lebih ringkas bila semua elemen diproses tanpa perlu indeksnya.

```vb
Dim cabang() As String = {"Medan", "Binjai", "Deli Serdang"}

For Each namaCabang As String In cabang
    Console.WriteLine(namaCabang)
Next
```

---

## Ringkasan

| Topik | Poin utama |
|---|---|
| Modul | Wadah kode yang statis; atur akses dengan `Public`/`Private` |
| Variabel | Pilih tipe data sesuai kebutuhan; gunakan `Const` untuk nilai tetap |
| Sub vs Function | Sub tidak mengembalikan nilai, Function mengembalikan lewat `Return` |
| ByVal vs ByRef | Salinan nilai vs referensi ke variabel asli |
| Array | Data sejenis dengan indeks mulai dari 0; `For` atau `For Each` untuk menelusuri |
