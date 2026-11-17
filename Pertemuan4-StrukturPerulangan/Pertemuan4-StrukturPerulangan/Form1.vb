Public Class Form1

    Private Sub txtNilaiAwal_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNilaiAwal.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtNilaiAkhir_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNilaiAkhir.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnInput_Click(sender As Object, e As EventArgs) Handles btnInput.Click

        Dim nilaiAwal As Integer
        Dim nilaiAkhir As Integer

        If Not Integer.TryParse(txtNilaiAwal.Text, nilaiAwal) Then
            MessageBox.Show("Masukkan angka")
            txtNilaiAwal.Focus()
            Return
        End If

        If Not Integer.TryParse(txtNilaiAkhir.Text, nilaiAkhir) Then
            MessageBox.Show("Masukkan angka")
            txtNilaiAkhir.Focus()
            Return
        End If

        lstHasil.Items.Clear()

        If nilaiAwal > nilaiAkhir Then

            For i As Integer = nilaiAwal To nilaiAkhir Step -1
                lstHasil.Items.Add(i)
            Next

        Else

            For i As Integer = nilaiAwal To nilaiAkhir
                lstHasil.Items.Add(i)
            Next

        End If
    End Sub
End Class