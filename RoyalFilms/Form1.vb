Imports System
Imports System.Threading
Imports System.Net.Sockets
Imports System.IO
Imports System.Text
Imports Newtonsoft.Json.Linq
Imports System.Drawing.Printing
Imports System.Math

Public Class Form1

    Inherits System.Windows.Forms.Form
    Public Graph As Graphics
    Dim TEXTOCLIENTE As String
    Dim TEXTOMENSAJE As String
    Dim WithEvents WinSockServer As New WinSockServer()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Start()
        With WinSockServer
            'Establezco el puerto donde escuchar
            .PuertoDeEscucha = 8050
            'Comienzo la escucha
            .Escuchar()
        End With


    End Sub

    Function WinSockServer_DatosRecibidos(ByVal IDTerminal As System.Net.IPEndPoint) Handles WinSockServer.DatosRecibidos
        TEXTOCLIENTE = WinSockServer.ObtenerDatos(IDTerminal)
    End Function

    Private Sub RELOJ_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles RELOJ.Tick
        If TEXTOCLIENTE <> "" Then
            CHAT.SelectionColor = Color.Black
            CHAT.Text = TEXTOCLIENTE
            TEXTOCLIENTE = ""
            settea_peticion()
            recorre_Json()
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Try
            If CHAT.Text <> "" Then
                If CHAT.Text = "#reiniciar" Then
                    Process.Start("CMD.exe", "/C shutdown /r /t 60")
                    CHAT.Clear()
                End If
                If CHAT.Text = "#apagar" Then
                    Process.Start("CMD.exe", "/C shutdown /s /f /t 60")
                    CHAT.Clear()
                End If
                If CHAT.Text = "#nullshut" Then
                    Process.Start("CMD.exe", "/C shutdown /a")
                    CHAT.Clear()
                End If
                If CHAT.Text = "#SesionOff" Then
                    Process.Start("CMD.exe", "/C shutdown /l /f /t 60")
                    CHAT.Clear()
                End If
                If CHAT.Text = "#msgErr" Then
                    MsgBox("Windows no ah podido recuperarse de un error en el sistema", MsgBoxStyle.Critical, "Error Windows")
                    CHAT.Clear()
                End If
            Else

            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub settea_peticion()
        Dim jake As List(Of String) = CHAT.Lines.ToList()
        jake.RemoveAt(0)
        CHAT.Lines = jake.ToArray()
        CHAT.Refresh()
        jake.RemoveAt(0)
        CHAT.Lines = jake.ToArray()
        CHAT.Refresh()
        jake.RemoveAt(0)
        CHAT.Lines = jake.ToArray()
        CHAT.Refresh()
        jake.RemoveAt(0)
        CHAT.Lines = jake.ToArray()
        CHAT.Refresh()
        jake.RemoveAt(0)
        CHAT.Lines = jake.ToArray()
        CHAT.Refresh()
        jake.RemoveAt(0)
        CHAT.Lines = jake.ToArray()
        CHAT.Refresh()
        jake.RemoveAt(0)
        CHAT.Lines = jake.ToArray()
        CHAT.Refresh()
        jake.RemoveAt(0)
        CHAT.Lines = jake.ToArray()
        CHAT.Refresh()
        jake.RemoveAt(0)
        CHAT.Lines = jake.ToArray()
        CHAT.Refresh()
        jake.RemoveAt(0)
        CHAT.Lines = jake.ToArray()
        CHAT.Refresh()
        jake.RemoveAt(0)
    End Sub

    Private Sub recorre_Json()
        Dim jsonpuros As JArray = JArray.Parse(CHAT.Text)
        Dim obj As JObject = jsonpuros.Item(0)
        Dim json As JObject = JObject.Parse(jsonpuros.Item(1).ToString())
        'Console.WriteLine(obj)
        'Console.WriteLine(obj.SelectToken("movie")("sname"))
        Dim jsonpuros2 As JArray = JArray.Parse("[" + json.SelectToken("cafeteria").ToString() + "]")
        Console.WriteLine(jsonpuros2)
        For Each item As JObject In jsonpuros2
            Dim name As String = item.GetValue("PRO_Description").ToString()
            Console.WriteLine(name)
        Next

        For Each xxx In json("cafeteria")
            Console.WriteLine(xxx.SelectToken("PRO_Price"))
        Next

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim a As ticket = New ticket
        a.TamanoLetra = 8
        a.AnadirLineaCabeza("Royal films S.A.S")
        a.AnadirLineaCabeza("NIT 890105652-3                PREFIJO VBT")
        a.AnadirLineaCabeza("Res 18762011468135             ")
        a.AnadirLineaCabeza("Desde VBT 375001 a 1000000     ")
        a.AnadirLineaCabeza("Multicine                     ")
        a.AnadirElemento("Dcto.Equivalente No fgdfg", "Dcto.Equivalente No fgdfg")
        a.AnadirElemento("Hora: d", "Hora: d")
        a.AnadirElemento("Fecha: fd", "Fecha: fd")
        a.AnadirElemento("Sala 2", "Sala 2")
        a.AnadirElemento("Silla: dfsdf", "Silla: dfsdf")
        a.AnadirElemento("Tarifa: 123", "Tarifa: 123")
        a.AnadirElemento("Precio: $122", "Precio: $122")
        a.AnadirElemento("IVA:   $0", "IVA:   $0")
        a.AnadirElemento("TOTAL: $123", "TOTAL: $123")
        a.AnadirElemento("TERMINAL TRM27", "TERMINAL TRM27")
        a.AnadirElemento("CAJERO: pruebas", "CAJERO: pruebas")
        a.AnadirElemento("FECHA VENTA", "FECHA VENTA")
        a.AnadirElemento("HORA VENTA", "HORA VENTA")
        a.AnadirElemento("AUDITORIA: 123", "AUDITORIA: 123")

        a.ImprimeTicket("EPSON TM-T88V Receipt")
    End Sub
End Class

Public Class WinSockServer
#Region "ESTRUCTURAS"

    Private Structure InfoDeUnCliente

        'Esta estructura permite guardar la información sobre un cliente



        Public Socket As Socket 'Socket utilizado para mantener la conexion con el cliente

        Public Thread As Thread 'Thread utilizado para escuchar al cliente

        Public UltimosDatosRecibidos As String 'Ultimos datos enviados por el cliente

    End Structure

#End Region
#Region "VARIABLES"

    Private tcpLsn As TcpListener

    Private Clientes As New Hashtable() 'Aqui se guarda la informacion de todos los clientes conectados

    Private tcpThd As Thread

    Private IDClienteActual As Net.IPEndPoint 'Ultimo cliente conectado

    Private m_PuertoDeEscucha As String

#End Region
#Region "EVENTOS"

    Public Event NuevaConexion(ByVal IDTerminal As Net.IPEndPoint)

    Public Event DatosRecibidos(ByVal IDTerminal As Net.IPEndPoint)

    Public Event ConexionTerminada(ByVal IDTerminal As Net.IPEndPoint)

#End Region
#Region "PROPIEDADES"

    Property PuertoDeEscucha() As String
        Get
            PuertoDeEscucha = m_PuertoDeEscucha
        End Get
        Set(ByVal Value As String)
            m_PuertoDeEscucha = Value
        End Set
    End Property

#End Region
#Region "METODOS"



    Public Sub Escuchar()

        tcpLsn = New TcpListener(PuertoDeEscucha)
        'Inicio la escucha
        tcpLsn.Start()
        'Creo un thread para que se quede escuchando la llegada de un cliente
        tcpThd = New Thread(AddressOf EsperarCliente)
        tcpThd.Start()
    End Sub



    Public Function ObtenerDatos(ByVal IDCliente As Net.IPEndPoint) As String

        Dim InfoClienteSolicitado As InfoDeUnCliente
        'Obtengo la informacion del cliente solicitado
        InfoClienteSolicitado = Clientes(IDCliente)
        ObtenerDatos = InfoClienteSolicitado.UltimosDatosRecibidos

    End Function



    Public Sub Cerrar(ByVal IDCliente As Net.IPEndPoint)

        Dim InfoClienteActual As InfoDeUnCliente



        'Obtengo la informacion del cliente solicitado

        InfoClienteActual = Clientes(IDCliente)



        'Cierro la conexion con el cliente

        InfoClienteActual.Socket.Close()

    End Sub



    Public Sub Cerrar()

        Dim InfoClienteActual As InfoDeUnCliente



        'Recorro todos los clientes y voy cerrando las conexiones

        For Each InfoClienteActual In Clientes.Values

            Call Cerrar(InfoClienteActual.Socket.RemoteEndPoint)

        Next

    End Sub
    Public Sub EnviarDatos(ByVal IDCliente As Net.IPEndPoint, ByVal Datos As String)

        Dim Cliente As InfoDeUnCliente



        'Obtengo la informacion del cliente al que se le quiere enviar el mensaje

        Cliente = Clientes(IDCliente)



        'Le envio el mensaje

        Cliente.Socket.Send(Encoding.ASCII.GetBytes(Datos))

    End Sub



    Public Sub EnviarDatos(ByVal Datos As String)

        Dim Cliente As InfoDeUnCliente



        'Recorro todos los clientes conectados, y les envio el mensaje recibido

        'en el parametro Datos

        For Each Cliente In Clientes.Values

            EnviarDatos(Cliente.Socket.RemoteEndPoint, Datos)

        Next

    End Sub



#End Region
#Region "FUNCIONES PRIVADAS"

    Private Sub EsperarCliente()

        Dim InfoClienteActual As InfoDeUnCliente



        With InfoClienteActual
            While True
                .Socket = tcpLsn.AcceptSocket()
                IDClienteActual = .Socket.RemoteEndPoint
                .Thread = New Thread(AddressOf LeerSocket)
                SyncLock Me
                    Clientes.Add(IDClienteActual, InfoClienteActual)
                End SyncLock
                RaiseEvent NuevaConexion(IDClienteActual)
                .Thread.Start()
            End While
        End With



    End Sub
    Private Sub LeerSocket()
        Dim IDReal As Net.IPEndPoint 'ID del cliente que se va a escuchar
        Dim Recibir() As Byte 'Array utilizado para recibir los datos que llegan
        Dim InfoClienteActual As InfoDeUnCliente 'Informacion del cliente que se va escuchar
        Dim Ret As Integer = 0
        IDReal = IDClienteActual
        InfoClienteActual = Clientes(IDReal)
        With InfoClienteActual
            While True
                If .Socket.Connected Then
                    Recibir = New Byte(5000) {}
                    Try
                        Ret = .Socket.Receive(Recibir, Recibir.Length, SocketFlags.None)
                        If Ret > 0 Then
                            .UltimosDatosRecibidos = Encoding.ASCII.GetString(Recibir)
                            Clientes(IDReal) = InfoClienteActual
                            RaiseEvent DatosRecibidos(IDReal)
                        Else
                            RaiseEvent ConexionTerminada(IDReal)
                            Exit While
                        End If
                    Catch e As Exception
                        If Not .Socket.Connected Then
                            'Genero el evento de la finalizacion de la conexion
                            RaiseEvent ConexionTerminada(IDReal)
                            Exit While
                        End If
                    End Try
                End If
            End While
            Call CerrarThread(IDReal)
        End With

    End Sub
    Private Sub CerrarThread(ByVal IDCliente As Net.IPEndPoint)
        Dim InfoClienteActual As InfoDeUnCliente

        InfoClienteActual = Clientes(IDCliente)
        Try

            InfoClienteActual.Thread.Abort()



        Catch e As Exception

            SyncLock Me

                Clientes.Remove(IDCliente)

            End SyncLock

        End Try


    End Sub


#End Region



End Class
