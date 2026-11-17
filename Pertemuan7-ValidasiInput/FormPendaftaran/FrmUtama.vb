Public Class FrmUtama
    ' Membuka contoh konversi input.
    Private Sub btnUjiKonversi_Click(sender As Object, e As EventArgs) Handles btnUjiKonversi.Click
        Dim frm As New FrmUjiKonversi()
        frm.ShowDialog()
    End Sub

    ' Membuka studi kasus pendaftaran.
    Private Sub btnPendaftaran_Click(sender As Object, e As EventArgs) Handles btnPendaftaran.Click
        Dim frm As New FrmPendaftaran()
        frm.ShowDialog()
    End Sub
End Class
