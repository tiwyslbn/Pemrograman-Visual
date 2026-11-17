# Laporan Praktikum Pemrograman Visual
### Pertemuan 04 — Perulangan (Looping) pada Visual Basic .NET

| | |
|---|---|
| **Nama** | Tiwy Lamberkat Silaban |
| **NIM** | 241712001 |
| **Kelas** | KOM A1 |
| **Mata Kuliah** | Praktikum Pemrograman Visual |
| **Pertemuan Ke-** | 4 |

## 1. Konsep Dasar

### 1.1 Pengertian

Perulangan adalah struktur kontrol yang menjalankan satu atau beberapa baris perintah berkali-kali selama syarat tertentu masih berlaku. Jumlah putarannya bisa ditentukan di awal (*fixed*), atau bergantung pada kondisi logika yang diperiksa tiap putaran (*conditional*).

### 1.2 Manfaat

- Mengurangi kode yang ditulis berulang sehingga program lebih ringkas dan mudah dirawat.
- Mempercepat pengolahan data dalam jumlah banyak, misalnya mengisi array atau menampilkan isi tabel.
- Membuat program fleksibel karena jumlah putaran dapat mengikuti input pengguna.
- Membantu validasi, misalnya meminta input terus sampai nilainya benar.

---

## 2. Jenis Struktur Perulangan

### 2.1 For...Next

Dipakai saat jumlah putaran sudah diketahui. Variabel penghitung (*counter*) berubah otomatis di setiap putaran.

```vb
For counter = nilaiAwal To nilaiAkhir [Step langkah]
    ' perintah
Next
```

```vb
' Mencetak nomor antrean 1 sampai 5
For antrean As Integer = 1 To 5
    Console.WriteLine("Antrean nomor " & antrean)
Next
```

`Step` mengatur kenaikan atau penurunan counter, misalnya untuk hitung mundur:

```vb
' Hitung mundur dengan loncatan 2
For detik As Integer = 10 To 1 Step -2
    Console.WriteLine(detik)
Next
' Output: 10, 8, 6, 4, 2
```

### 2.2 For Each...Next

Dipakai untuk menelusuri tiap elemen dalam koleksi (array, `List`, `ArrayList`) tanpa perlu tahu jumlah elemennya.

```vb
For Each elemen As Tipe In koleksi
    ' perintah
Next
```

```vb
Dim menu() As String = {"Nasi Goreng", "Mie Ayam", "Soto"}

For Each hidangan As String In menu
    Console.WriteLine(hidangan)
Next
```

### 2.3 Do While...Loop

Berjalan **selama** kondisi bernilai `True`. Kondisi diperiksa di awal, sehingga jika sejak awal `False`, isi perulangan tidak dijalankan sama sekali.

```vb
Do While kondisi
    ' perintah
Loop
```

```vb
Dim saldo As Integer = 50000

Do While saldo >= 10000
    Console.WriteLine("Saldo: " & saldo)
    saldo -= 15000
Loop
' Output: 50000, 35000, 20000
```

### 2.4 Do...Loop While

Kondisi diperiksa di **akhir**, sehingga isi perulangan pasti berjalan minimal satu kali walaupun kondisinya sudah `False` sejak awal.

```vb
Do
    ' perintah
Loop While kondisi
```

```vb
Dim percobaan As Integer = 10

Do
    Console.WriteLine("Percobaan ke-" & percobaan)
    percobaan += 1
Loop While percobaan <= 5
' Output: Percobaan ke-10 (tetap tampil sekali)
```

### 2.5 Do Until...Loop

Kebalikan dari `Do While`: berjalan **selama** kondisi bernilai `False` dan berhenti saat kondisi menjadi `True`. Pemeriksaan di awal.

```vb
Do Until kondisi
    ' perintah
Loop
```

```vb
Dim stok As Integer = 10

Do Until stok <= 0
    Console.WriteLine("Sisa stok: " & stok)
    stok -= 3
Loop
' Output: 10, 7, 4, 1
```

### 2.6 Do...Loop Until

Seperti `Do...Loop While`, kondisi diperiksa di akhir (isi minimal berjalan sekali), tetapi perulangan berhenti saat kondisi bernilai `True`.

```vb
Do
    ' perintah
Loop Until kondisi
```

```vb
Dim putaran As Integer = 1

Do
    Console.WriteLine("Putaran " & putaran)
    putaran += 1
Loop Until putaran > 3
' Output: Putaran 1, 2, 3
```

### 2.7 While...End While

Fungsinya sama dengan `Do While...Loop` (syarat di awal, berjalan selama `True`) dengan penulisan yang lebih singkat.

```vb
While kondisi
    ' perintah
End While
```

```vb
Dim nomor As Integer = 1

While nomor <= 5
    Console.WriteLine(nomor)
    nomor += 1
End While
```

---

## 3. Kontrol Alur dalam Perulangan

Kadang perulangan perlu dihentikan lebih cepat, atau sebagian putarannya dilewati.

### 3.1 Exit For dan Exit Do

Menghentikan perulangan seketika, biasanya dipasangkan dengan `If`.

```vb
' Berhenti saat menemukan nilai 5
For angka As Integer = 1 To 10
    If angka = 5 Then
        Exit For
    End If
    Console.WriteLine(angka)
Next
' Output: 1, 2, 3, 4
```

```vb
Dim hitung As Integer = 1

Do While hitung <= 10
    If hitung = 5 Then
        Exit Do
    End If
    Console.WriteLine(hitung)
    hitung += 1
Loop
' Output: 1, 2, 3, 4
```

### 3.2 Continue For dan Continue Do

Melewati sisa perintah pada putaran yang sedang berjalan, lalu lanjut ke putaran berikutnya tanpa menghentikan seluruh perulangan.

```vb
For angka As Integer = 1 To 10
    If angka Mod 2 = 0 Then
        Continue For
    End If
    Console.WriteLine(angka)
Next
' Output: 1, 3, 5, 7, 9 (angka genap dilewati)
```

---

## 4. Perulangan Bersarang (Nested Loop)

Perulangan di dalam perulangan lain, lazim dipakai untuk data dua dimensi seperti tabel atau matriks, dan untuk membuat pola tampilan.

```vb
For baris As Integer = 1 To 3
    For kolom As Integer = 1 To 3
        Console.Write(baris & "-" & kolom & "  ")
    Next
    Console.WriteLine()
Next
```

Output:

```
1-1  1-2  1-3
2-1  2-2  2-3
3-1  3-2  3-3
```

Setiap satu putaran perulangan luar (`baris`), perulangan dalam (`kolom`) berjalan penuh tiga kali, sehingga totalnya 3 x 3 = 9 kombinasi.

---

## 5. Perbandingan Struktur Perulangan

| Struktur | Pemeriksaan kondisi | Minimal dijalankan | Berhenti ketika | Cocok untuk |
|---|---|---|---|---|
| `For...Next` | Di awal (lewat counter) | 0 kali | Counter melewati nilai akhir | Jumlah putaran sudah pasti |
| `For Each...Next` | Di awal (per elemen) | 0 kali | Semua elemen koleksi terbaca | Membaca seluruh isi array/List tanpa indeks |
| `Do While...Loop` | Di awal | 0 kali | Kondisi menjadi `False` | Putaran berbasis syarat, jumlah tidak pasti |
| `Do...Loop While` | Di akhir | 1 kali | Kondisi menjadi `False` | Proses yang harus jalan sekali dulu sebelum syarat dicek |
| `Do Until...Loop` | Di awal | 0 kali | Kondisi menjadi `True` | Mengulang sampai target tercapai |
| `Do...Loop Until` | Di akhir | 1 kali | Kondisi menjadi `True` | Seperti `Do Until`, tetapi wajib jalan minimal sekali |
| `While...End While` | Di awal | 0 kali | Kondisi menjadi `False` | Alternatif singkat untuk `Do While` |

### Dua perbedaan yang perlu diingat

**1. While vs Until**

- `While`: terus berjalan selama kondisi `True`, berhenti saat `False`.
- `Until`: terus berjalan selama kondisi `False`, berhenti saat `True`.

**2. Syarat di atas vs di bawah**

- *Syarat di atas* (`Do While...`, `Do Until...`): diperiksa sebelum masuk perulangan, jadi bisa berjalan 0 kali.
- *Syarat di bawah* (`...Loop While`, `...Loop Until`): diperiksa setelah isi perulangan selesai, jadi dijamin berjalan minimal 1 kali.

### Kesalahan yang sering terjadi

- **Perulangan tak berujung.** Pada `Do`/`While`, lupa mengubah variabel yang diuji (misalnya tidak ada `angka += 1`) membuat kondisi tidak pernah berubah.
- **Salah batas.** `For i = 0 To arr.Length` melewati indeks terakhir; gunakan `arr.Length - 1`.
- **Tertukar `While` dan `Until`.** Kondisi yang benar untuk `While` biasanya menjadi kebalikannya pada `Until`.
