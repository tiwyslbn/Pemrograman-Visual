```vb
Public Class Form1

    Private Sub btnInput_Click(sender As Object, e As EventArgs) Handles btnInput.Click
        Dim nilai As Integer

        ' Cek apakah input berupa angka
        If Not Integer.TryParse(txtNilai.Text, nilai) Then
            MessageBox.Show("Masukkan nilai dalam bentuk angka!")
            txtNilai Focus()
            Return
        End If
        If nilai < 0 OrElse nilai > 100 Then
            MessageBox Show("Masukkan nilai 0 - 100")
            txtNilai Focus()
            Return
        End If

        If nilai <= 50 Then
            picImage.Image = Image.FromFile("aset\pic2.jpeg")
        ElseIf nilai <= 75 Then
            picImage.Image = Image.FromFile("aset\pic3.jpeg")
        Else
            picImage.Image = Image.FromFile("aset\pic1.webp")
        End If

    End Sub

    Private Sub txtNilai_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNilai.KeyPress
        ' Hanya menerima angka dan tombol kontrol seperti Backspace
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

End Class
```
