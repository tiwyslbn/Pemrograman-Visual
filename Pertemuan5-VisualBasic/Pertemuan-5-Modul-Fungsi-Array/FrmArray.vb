Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class FrmArray
    Private Sub btnTampil_Click(sender As Object, e As EventArgs) Handles btnTampil.Click
        'MessageBox.Show(Hitung(txtPanjang.Text, txtLebar.Text))
        'MessageBox.Show(nilai(50))
        'lstNilai.Items.Add(nilai(5))
        'For i As Integer = 0 To nilai.Length - 1
        'lstNilai.Items.Add(nilai(i))
        MessageBox.Show(nilai2D(1, 2))
    End Sub

    Private Sub lblPanjang_Click(sender As Object, e As EventArgs) Handles lblPanjang.Click

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles txtPanjang.TextChanged
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles txtLebar.TextChanged
    End Sub

    Private Sub txtPanjang_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPanjang.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtLebar_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtLebar.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub lstNilai_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstNilai.SelectedIndexChanged
    End Sub
End Class
