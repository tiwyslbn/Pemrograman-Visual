Public Class FormPajak

    Private namaUser As String
    Private roleUser As String

    ' Constructor menerima data dari Form1 (form login)
    Public Sub New(nama As String, role As String)
        InitializeComponent()

        namaUser = nama
        roleUser = role
    End Sub

    Private Sub btnHitung_Click(sender As Object, e As EventArgs) Handles btnHitung.Click

        Dim pendapatan As Double

        If Not Double.TryParse(txtPendapatan.Text.Trim(), pendapatan) Then
            MessageBox.Show(
                "Masukkan pendapatan dengan angka yang benar",
                "Pajak",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )
            Return
        End If

        Dim persenPajak As Double

        If pendapatan > 100000000 Then
            persenPajak = 0.3

        ElseIf pendapatan > 30000000 Then
            persenPajak = 0.2

        ElseIf pendapatan > 5000000 Then
            persenPajak = 0.1

        Else
            persenPajak = 0
        End If

        Dim jumlahPajak As Double = pendapatan * persenPajak

        Dim pendapatanBersih As Double = pendapatan - jumlahPajak

        lblHasil.Text =
            "Pajak (" & (persenPajak * 100) & "%) : Rp " &
            jumlahPajak.ToString("N0") &
            vbCrLf &
            "Pendapatan Bersih : Rp " &
            pendapatanBersih.ToString("N0")

    End Sub

    Private Sub btnKeluar_Click(sender As Object, e As EventArgs) Handles btnKeluar.Click
        Application.Exit()
    End Sub

End Class
