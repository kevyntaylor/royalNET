Public Class OrdernarTotal

    Public delimitador() As Char = "%"

    Public Sub OrdernarTotal(ByVal delimit As Char)

        Dim delimitador As Char = delimit
    End Sub

    Public Function ObtenerTotalNombre(ByVal totalItem As String) As String

        Dim delimitado() As String = totalItem.Split(delimitador)
        Return delimitado(0)

    End Function
    Public Function ObtenerTotalCantidad(ByVal totalItem As String) As String

        Dim delimitado() As String = totalItem.Split(delimitador)
        Return delimitado(1)
    End Function

    Public Function GenerarTotal(ByVal totalName As String, ByVal price As String) As String

        GenerarTotal = totalName + delimitador(0) + price
    End Function

End Class
