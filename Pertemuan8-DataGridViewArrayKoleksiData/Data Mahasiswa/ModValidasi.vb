Option Strict On
Imports System.Globalization

Public Module ModValidasi

    Public Function Kosong(teks As String) As Boolean
        Return String.IsNullOrWhiteSpace(teks)
    End Function

    Public Function SemuaDigit(teks As String, panjang As Integer) As Boolean
        If String.IsNullOrEmpty(teks) OrElse teks.Length <> panjang Then
            Return False
        End If

        For Each c As Char In teks
            If Not Char.IsDigit(c) Then
                Return False
            End If
        Next

        Return True
    End Function

    Public Function CobaDesimal(teks As String, ByRef hasil As Decimal) As Boolean
        If String.IsNullOrWhiteSpace(teks) Then
            Return False
        End If

        Dim teksTitik As String = teks.Trim().Replace(",", ".")
        Return Decimal.TryParse(teksTitik, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, hasil)
    End Function

End Module