Public Class Form1
    Private Sub btnKoneksi_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnKoneksi.Click
        ModDatabase.DatabaseKoneksi()
    End Sub
    Private Sub btnTambah_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnTambah.Click
        If ModQuery.TambahData(
            txtNim.Text,
            txtNama.Text,
            txtJurusan.Text
        ) Then
            MessageBox.Show("Data Berhasil Disimpan")
        End If
        txtNim.Clear()
        txtNama.Clear()
        txtJurusan.Clear()
    End Sub
    Private Sub btnTampil_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnTampil.Click
        dgvMahasiswa.AutoGenerateColumns = True
        dgvMahasiswa.DataSource =
            ModQuery.TampilkanData()
    End Sub
End Class