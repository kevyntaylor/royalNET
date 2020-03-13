Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.IO
Public Class ticket
    Public LineasDeLaCabeza As ArrayList = New ArrayList()
    Public LineasDeLaSubCabeza As ArrayList = New ArrayList()
    Public Elementos As ArrayList = New ArrayList()
    Public Totales As ArrayList = New ArrayList()
    Public LineasDelPie As ArrayList = New ArrayList()
    Private headerImagep As Image
    Public contador As Integer = 0
    Public CaracteresMaximos As Integer = 200
    Public CaracteresMaximosDescripcion As Integer = 200
    Public imageHeight As Integer = 0
    Public MargenIzquierdo As Double = 0
    Public MargenSuperior As Double = 0
    Public NombreDeLaFuente As String = "Arial"
    Public TamanoDeLaFuente As Integer = 7
    Public TamanoDeLaFuenteNegrita As Integer = 9
    Public FuenteImpresa As Font
    Public ColorDeLaFuente As SolidBrush = New SolidBrush(Color.Red)
    Public gfx As Graphics
    Public CadenaPorEscribirEnLinea As String = ""
    Private WithEvents DocumentoAImprimir As New PrintDocument
    Public Sub Ticket()


    End Sub

    Public Property HeaderImage() As Image
        Get
            Return headerImagep
        End Get
        Set(ByVal value As Image)
            'If headerImagep.Width <> value.Width Then 

            'End If 
            headerImagep = value
        End Set
    End Property



    Public Property MaximoCaracter() As Integer
        Get
            Return CaracteresMaximos
        End Get
        Set(ByVal value As Integer)
            If (value <> CaracteresMaximosDescripcion) Then CaracteresMaximosDescripcion = value
        End Set
    End Property



    Public Property MaximoCaracterDescripcion() As Integer
        Get
            Return CaracteresMaximosDescripcion
        End Get
        Set(ByVal value As Integer)
            If (value <> CaracteresMaximosDescripcion) Then CaracteresMaximosDescripcion = value
        End Set
    End Property



    Public Property TamanoLetra() As Integer
        Get
            Return TamanoDeLaFuente
        End Get
        Set(ByVal value As Integer)
            If (value <> TamanoDeLaFuente) Then TamanoDeLaFuente = value
        End Set
    End Property


    Public Property NombreLetra() As String
        Get
            Return NombreDeLaFuente
        End Get
        Set(ByVal value As String)
            If (value <> NombreDeLaFuente) Then NombreDeLaFuente = value
        End Set
    End Property


    Public Sub AnadirLineaCabeza(ByVal linea As String)
        LineasDeLaCabeza.Add(linea)
    End Sub

    Public Sub AnadirLineaSubcabeza(ByVal linea As String)

        LineasDeLaSubCabeza.Add(linea)
    End Sub

    Public Sub AnadirElemento(ByVal cantidad As String, ByVal elemento As String)

        Dim NuevoElemento As OrdenarElementos = New OrdenarElementos()
        '''''items.Add(newitem. 
        Elementos.Add(NuevoElemento.GenerarElemento(cantidad, elemento))
    End Sub

    Public Sub AnadirTotal(ByVal Nombre As String, ByVal Precio As String)
        Dim NuevoTotal As OrdernarTotal = New OrdernarTotal
        ' OrderTotal(newtotal) 

        Totales.Add(NuevoTotal.GenerarTotal(Nombre, Precio))
    End Sub

    Public Sub AnadeLineaAlPie(ByVal linea As String)
        LineasDelPie.Add(linea)
    End Sub

    Private Function AlineaTextoaLaDerecha(ByVal Izquierda As Integer) As String

        Dim espacios As String = ""
        Dim spaces As Integer = MaximoCaracter() - Izquierda
        Dim x As Integer
        For x = 0 To spaces
            espacios += " "
        Next
        Return espacios
    End Function

    Private Function DottedLine() As String

        Dim dotted As String = ""
        Dim x As Integer
        For x = 0 To MaximoCaracter()
            dotted += "="
        Next
        Return dotted
    End Function
    Private Sub DibujaTotales()

        Dim ordTot As OrdernarTotal = New OrdernarTotal()

        For Each total As String In Totales

            CadenaPorEscribirEnLinea = ordTot.ObtenerTotalCantidad(total)
            CadenaPorEscribirEnLinea = AlineaTextoaLaDerecha(CadenaPorEscribirEnLinea.Length) + CadenaPorEscribirEnLinea

            gfx.DrawString(CadenaPorEscribirEnLinea, FuenteImpresa, ColorDeLaFuente, 0, Renglon(), New StringFormat())
            MargenIzquierdo = 10

            CadenaPorEscribirEnLinea = " " + ordTot.ObtenerTotalNombre(total)
            gfx.DrawString(CadenaPorEscribirEnLinea, FuenteImpresa, ColorDeLaFuente, 0, Renglon(), New StringFormat())
            contador += 1
        Next total
        MargenIzquierdo = 10
        DibujaEspacio()
        DibujaEspacio()
    End Sub

    Public Function ImpresoraExistente(ByVal impresora As String) As Boolean

        For Each strPrinter As String In PrinterSettings.InstalledPrinters

            If impresora = strPrinter Then
                Return True
            End If
        Next strPrinter
        Return False
    End Function

    Public Sub ImprimeTicket(ByVal impresora As String)

        FuenteImpresa = New Font(NombreLetra, TamanoLetra, FontStyle.Regular)
        'Dim pr As New PrintDocument 
        DocumentoAImprimir.PrinterSettings.PrinterName = impresora
        'pr.PrinterSettings.printpa() 
        ' pr.PrintPage += New 
        ' PrintPageEventHandler(pr_PrintPage) 
        DocumentoAImprimir.Print()

    End Sub

    Private Sub DocumentoAImprimir_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles DocumentoAImprimir.PrintPage
        e.Graphics.PageUnit = GraphicsUnit.Millimeter
        gfx = e.Graphics

        'DrawImage()
        'DrawHeader() 
        DibujaLaCabecera()
        'DrawSubHeader() 
        'DibujaLaSubCabecera()
        'DrawItems() 
        DibujaBoleta()
        'DrawTotales() 
        'DibujaTotales()
        'DibujarPieDePagina()

        'If (headerImagep.Width <> 0) Then 
        ' HeaderImage.Dispose() 
        ' HeaderImage.Dispose() 
        'End If 
    End Sub

    Private Function Renglon() As Double
        Return MargenSuperior + (contador * FuenteImpresa.GetHeight(gfx) + imageHeight)
    End Function

    Private Sub DrawImage()

        If (headerImagep.Width <> 0) Then

            Try

                gfx.DrawImage(HeaderImage, New Point(CInt(5), CInt(Renglon())))
                Dim height As Double = (HeaderImage.Height / 58) * 15
                imageHeight = CInt(Math.Round(height) + 3)

            Catch ex As Exception

            End Try


        End If
    End Sub

    Private Sub DibujaLaCabecera()

        For Each Cabecera As String In LineasDeLaCabeza

            If (Cabecera.Length > MaximoCaracter()) Then

                Dim CaracterActual As Integer = 0
                Dim LongitudDeCabecera As Integer = Cabecera.Length

                While (LongitudDeCabecera > MaximoCaracter())

                    CadenaPorEscribirEnLinea = Cabecera.Substring(CaracterActual, MaximoCaracter)
                    gfx.DrawString(CadenaPorEscribirEnLinea, FuenteImpresa, ColorDeLaFuente, MargenIzquierdo, Renglon(), New StringFormat())
                    contador += 1
                    CaracterActual += MaximoCaracter()
                    LongitudDeCabecera -= MaximoCaracter()
                End While
                CadenaPorEscribirEnLinea = Cabecera
                gfx.DrawString(CadenaPorEscribirEnLinea.Substring(CaracterActual, CadenaPorEscribirEnLinea.Length - CaracterActual), FuenteImpresa, ColorDeLaFuente, MargenIzquierdo, Renglon(), New StringFormat())
                contador += 1

            Else


                CadenaPorEscribirEnLinea = Cabecera
                gfx.DrawString(CadenaPorEscribirEnLinea, FuenteImpresa, ColorDeLaFuente, MargenIzquierdo, Renglon(), New StringFormat())

                contador += 1
            End If
        Next Cabecera
        DibujaEspacio()
    End Sub

    Private Sub DibujaLaSubCabecera()

        For Each SubCabecera As String In LineasDeLaSubCabeza

            If (SubCabecera.Length > MaximoCaracter()) Then

                Dim CaracterActual As Integer = 0
                Dim LongitudSubcabecera As Integer = SubCabecera.Length

                While (LongitudSubcabecera > MaximoCaracter())

                    CadenaPorEscribirEnLinea = SubCabecera
                    gfx.DrawString(CadenaPorEscribirEnLinea.Substring(CaracterActual, MaximoCaracter), FuenteImpresa, ColorDeLaFuente, MargenIzquierdo, Renglon(), New StringFormat())

                    contador += 1
                    CaracterActual += MaximoCaracter()
                    LongitudSubcabecera -= MaximoCaracter()
                End While
                CadenaPorEscribirEnLinea = SubCabecera
                gfx.DrawString(CadenaPorEscribirEnLinea.Substring(CaracterActual, CadenaPorEscribirEnLinea.Length - CaracterActual), FuenteImpresa, ColorDeLaFuente, MargenIzquierdo, Renglon(), New StringFormat())
                contador += 1

            Else

                CadenaPorEscribirEnLinea = SubCabecera

                gfx.DrawString(CadenaPorEscribirEnLinea, FuenteImpresa, ColorDeLaFuente, MargenIzquierdo, Renglon(), New StringFormat())

                contador += 1

                CadenaPorEscribirEnLinea = DottedLine()

                gfx.DrawString(CadenaPorEscribirEnLinea, FuenteImpresa, ColorDeLaFuente, MargenIzquierdo, Renglon(), New StringFormat())

                contador += 1
            End If
        Next SubCabecera
        DibujaEspacio()
    End Sub

    Private Sub DibujaElementos()

        Dim OrdenElemento As OrdenarElementos = New OrdenarElementos()

        gfx.DrawString("CANT  DESCRIPCION                   IMPORTE", FuenteImpresa, ColorDeLaFuente, MargenIzquierdo, Renglon(), New StringFormat())

        contador += 1
        DibujaEspacio()

        For Each Elemento As String In Elementos

            CadenaPorEscribirEnLinea = OrdenElemento.ObtenerCantidadDeElementos(Elemento)

            gfx.DrawString(CadenaPorEscribirEnLinea, FuenteImpresa, ColorDeLaFuente, 2, Renglon(), New StringFormat())
            CadenaPorEscribirEnLinea = AlineaTextoaLaDerecha(CadenaPorEscribirEnLinea.Length) + CadenaPorEscribirEnLinea

            gfx.DrawString(CadenaPorEscribirEnLinea, FuenteImpresa, ColorDeLaFuente, 2, Renglon(), New StringFormat())

            Dim Nombre As String = OrdenElemento.ObtenerNombreElemento(Elemento)

            MargenIzquierdo = 10
            If (Nombre.Length > MaximoCaracterDescripcion) Then

                Dim CaracterActual As Integer = 0
                Dim LongitudElemento As Integer = Nombre.Length

                While (LongitudElemento > MaximoCaracterDescripcion)

                    CadenaPorEscribirEnLinea = OrdenElemento.ObtenerNombreElemento(Elemento)
                    gfx.DrawString(" " + CadenaPorEscribirEnLinea.Substring(CaracterActual, MaximoCaracterDescripcion), FuenteImpresa, ColorDeLaFuente, MargenIzquierdo, Renglon(), New StringFormat())

                    contador += 1
                    CaracterActual += MaximoCaracterDescripcion
                    LongitudElemento -= MaximoCaracterDescripcion
                End While

                CadenaPorEscribirEnLinea = OrdenElemento.ObtenerNombreElemento(Elemento)
                gfx.DrawString(" " + CadenaPorEscribirEnLinea.Substring(CaracterActual, CadenaPorEscribirEnLinea.Length - CaracterActual), FuenteImpresa, ColorDeLaFuente, MargenIzquierdo, Renglon() + 10, New StringFormat())
                contador += 1

            Else

                gfx.DrawString(" " + OrdenElemento.ObtenerNombreElemento(Elemento), FuenteImpresa, ColorDeLaFuente, MargenIzquierdo, Renglon(), New StringFormat())

                contador += 1
            End If
        Next Elemento

        MargenIzquierdo = 0
        DibujaEspacio()
        CadenaPorEscribirEnLinea = DottedLine()

        gfx.DrawString(CadenaPorEscribirEnLinea, FuenteImpresa, ColorDeLaFuente, MargenIzquierdo, Renglon(), New StringFormat())

        contador += 1
        DibujaEspacio()
    End Sub


    Private Sub DibujaBoleta()

        Dim OrdenElemento As OrdenarElementos = New OrdenarElementos()
        For Each Elemento As String In Elementos

            CadenaPorEscribirEnLinea = OrdenElemento.ObtenerCantidadDeElementos(Elemento)

            gfx.DrawString(CadenaPorEscribirEnLinea, FuenteImpresa, ColorDeLaFuente, 2, Renglon(), New StringFormat())
            CadenaPorEscribirEnLinea = AlineaTextoaLaDerecha(CadenaPorEscribirEnLinea.Length) + CadenaPorEscribirEnLinea

            gfx.DrawString(CadenaPorEscribirEnLinea, FuenteImpresa, ColorDeLaFuente, 2, Renglon(), New StringFormat())

            Dim Nombre As String = OrdenElemento.ObtenerNombreElemento(Elemento)

            MargenIzquierdo = 40
            If (Nombre.Length > MaximoCaracterDescripcion) Then

                Dim CaracterActual As Integer = 0
                Dim LongitudElemento As Integer = Nombre.Length

                While (LongitudElemento > MaximoCaracterDescripcion)

                    CadenaPorEscribirEnLinea = OrdenElemento.ObtenerNombreElemento(Elemento)
                    gfx.DrawString(" " + CadenaPorEscribirEnLinea.Substring(CaracterActual, MaximoCaracterDescripcion), FuenteImpresa, ColorDeLaFuente, MargenIzquierdo, Renglon(), New StringFormat())

                    contador += 1
                    CaracterActual += MaximoCaracterDescripcion
                    LongitudElemento -= MaximoCaracterDescripcion
                End While

                CadenaPorEscribirEnLinea = OrdenElemento.ObtenerNombreElemento(Elemento)
                gfx.DrawString(" " + CadenaPorEscribirEnLinea.Substring(CaracterActual, CadenaPorEscribirEnLinea.Length - CaracterActual), FuenteImpresa, ColorDeLaFuente, MargenIzquierdo, Renglon() + 10, New StringFormat())
                contador += 1

            Else

                gfx.DrawString(" " + OrdenElemento.ObtenerNombreElemento(Elemento), FuenteImpresa, ColorDeLaFuente, MargenIzquierdo, Renglon(), New StringFormat())

                contador += 1
            End If
        Next Elemento

        MargenIzquierdo = 0

        contador += 1
        DibujaEspacio()
    End Sub

    Private Sub DibujarPieDePagina()

        For Each PieDePagina As String In LineasDelPie

            If (PieDePagina.Length > MaximoCaracter()) Then

                Dim currentChar As Integer = 0
                Dim LongitudPieDePagina As Integer = PieDePagina.Length

                While (LongitudPieDePagina > MaximoCaracter())

                    CadenaPorEscribirEnLinea = PieDePagina
                    gfx.DrawString(CadenaPorEscribirEnLinea.Substring(currentChar, MaximoCaracter), FuenteImpresa, ColorDeLaFuente, 3, Renglon(), New StringFormat())

                    contador += 1
                    currentChar += MaximoCaracter()
                    LongitudPieDePagina -= MaximoCaracter()
                End While
                CadenaPorEscribirEnLinea = PieDePagina
                gfx.DrawString(CadenaPorEscribirEnLinea.Substring(currentChar, CadenaPorEscribirEnLinea.Length - currentChar), FuenteImpresa, ColorDeLaFuente, 1, Renglon(), New StringFormat())
                contador += 1

            Else

                CadenaPorEscribirEnLinea = PieDePagina
                gfx.DrawString(CadenaPorEscribirEnLinea, FuenteImpresa, ColorDeLaFuente, 3, Renglon(), New StringFormat())

                contador += 1
            End If
        Next PieDePagina
        MargenIzquierdo = 0
        DibujaEspacio()
    End Sub

    Private Sub DibujaEspacio()

        CadenaPorEscribirEnLinea = " "

        gfx.DrawString(CadenaPorEscribirEnLinea, FuenteImpresa, ColorDeLaFuente, 0, Renglon(), New StringFormat())

        contador += 1

    End Sub

    Public Sub New()

    End Sub


End Class








