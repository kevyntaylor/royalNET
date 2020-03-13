Public Class OrdenarElementos

    Public delimitador() As Char = "%"


    Public Sub OrdenarElementos(ByVal delimit As Char)
        Dim delimitador As Char = delimit
    End Sub


    Public Function ObtenerCantidadDeElementos(ByVal orderItem As String) As String

        Dim delimitado() As String = orderItem.Split(delimitador)
        Return delimitado(0)
    End Function

    Public Function ObtenerNombreElemento(ByVal orderItem As String) As String

        Dim delimitado() As String = orderItem.Split(delimitador)
        Return delimitado(1)
    End Function

    Public Function GenerarElemento(ByVal cantidad As String, ByVal NombreElemento As String) As String

        Return " " + cantidad + delimitador(0) + NombreElemento + delimitador(0)
    End Function
End Class
