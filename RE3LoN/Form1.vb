Imports System.IO
Imports System.Text.RegularExpressions
Public Class Form1
    Private archivoTexto As String = "bio3.ini"

    Private Sub ModificarArchivoTexto(ByVal nombreArchivo As String, ByVal nuevaLinea As String, ByVal lineaAnterior As String)
        Dim lineas As List(Of String) = File.ReadAllLines(nombreArchivo).ToList()
        Dim lineaIndex As Integer = lineas.FindIndex(Function(linea) linea.StartsWith(lineaAnterior))
        lineas(lineaIndex) = nuevaLinea
        File.WriteAllLines(nombreArchivo, lineas)
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://www.moddb.com/mods/resident-evil-3-overhaul-mod-sourcenext")
    End Sub

    Private Sub LeerConfiguracion()
        Dim path As String = "bio3.ini"
        Dim texto As String = File.ReadAllText(path)


        Dim regexSEvol As New Regex("SEvol=(\d+)")
        Dim matchSEvol As Match = regexSEvol.Match(texto)
        If matchSEvol.Success Then
            Dim valorSEvol As Integer = Integer.Parse(matchSEvol.Groups(1).Value)
            tbSonido.Value = valorSEvol
        End If

        Dim regexBGMvol As New Regex("BGMvol=(\d+)")
        Dim matchBGMvol As Match = regexBGMvol.Match(texto)
        If matchBGMvol.Success Then
            Dim valorBGMvol As Integer = Integer.Parse(matchBGMvol.Groups(1).Value)
            tbMusica.Value = valorBGMvol
        End If
    End Sub


    Private Sub ModificarConfiguracion()
        Dim path As String = "bio3.ini"
        Dim texto As String = File.ReadAllText(path)

        ' Modificar las líneas necesarias del archivo bio3.ini
        texto = Regex.Replace(texto, "DisableMovie=\w+", "DisableMovie=off")
        texto = Regex.Replace(texto, "DisableAlpha=\w+", "DisableAlpha=off")
        texto = Regex.Replace(texto, "DisableLinear=\w+", "DisableLinear=off")
        texto = Regex.Replace(texto, "DisableSpecular=\w+", "DisableSpecular=off")
        texto = Regex.Replace(texto, "TextureAdjust=\w+", "TextureAdjust=off")

        ' Guardar los cambios en el archivo
        File.WriteAllText(path, texto)
    End Sub

    Private Function GetModeValue() As String
        Dim iniFile As String = "bio3.ini"
        Dim modeValue As String = ""

        If File.Exists(iniFile) Then
            Using reader As New StreamReader(iniFile)
                While Not reader.EndOfStream
                    Dim line As String = reader.ReadLine()
                    If line.StartsWith("Mode=") Then
                        modeValue = line.Split("="c)(1).Trim()
                        Exit While
                    End If
                End While
            End Using
        End If

        Return modeValue
    End Function


    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ' Comprobar si el archivo bio3.ini existe en el directorio actual
        If Not File.Exists("bio3.ini") Then
            MessageBox.Show("Por favor coloque este launcher en el directorio del juego", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit()
        End If


        If Process.GetProcessesByName(Process.GetCurrentProcess.ProcessName).Length > 1 Then
            MessageBox.Show("No se permite más de una instancia de este programa", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Application.Exit()
        End If

        'AÑADE TODAS LAS OPCIONES DE VIDEO EN OFF
        ModificarConfiguracion()

        'LEE Y CARGA EL SONIDO Y MUSICA
        LeerConfiguracion()

        ' Leer el archivo "bio3.ini"
        Dim contenido As String = My.Computer.FileSystem.ReadAllText("bio3.ini")

        ' Buscar los valores actuales de "Height" y "Width" y mostrarlos en los TextBox
        Dim regexHeight As New Regex("Height=(\d+)")
        Dim regexWidth As New Regex("Width=(\d+)")
        Dim matchHeight As Match = regexHeight.Match(contenido)
        Dim matchWidth As Match = regexWidth.Match(contenido)

        If matchHeight.Success Then
            TextBox1.Text = matchHeight.Groups(1).Value
        End If

        If matchWidth.Success Then
            TextBox2.Text = matchWidth.Groups(1).Value
        End If

        Dim modeValue As String = GetModeValue()
        If modeValue = "Fullscreen" Then
            cmbModo.SelectedIndex = 1 ' Índice del elemento Fullscreen
        ElseIf modeValue = "Windowed" Then
            cmbModo.SelectedIndex = 0 ' Índice del elemento Windowed
        End If

    End Sub

    Private Sub ModificarConfiguracion(ByVal clave As String, ByVal valor As String)
        ' Abrir el archivo bio3.ini para lectura y escritura
        Dim archivo As String = "bio3.ini"
        Dim lineas As List(Of String) = File.ReadAllLines(archivo).ToList()

        ' Buscar la clave en el archivo y actualizar su valor
        For i As Integer = 0 To lineas.Count - 1
            If lineas(i).StartsWith(clave + "=") Then
                lineas(i) = clave + "=" + valor
                Exit For
            End If
        Next

        ' Guardar los cambios en el archivo
        File.WriteAllLines(archivo, lineas)
    End Sub

    Private Sub TextBox1_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles TextBox1.Leave
        ' Obtener el valor ingresado en el TextBox
        Dim height As Integer = 0
        Integer.TryParse(TextBox1.Text, height)

        ' Modificar el archivo "bio3.ini" con el valor correspondiente
        Dim contenido As String = My.Computer.FileSystem.ReadAllText("bio3.ini")
        contenido = Regex.Replace(contenido, "Height=\d+", "Height=" & height.ToString())
        My.Computer.FileSystem.WriteAllText("bio3.ini", contenido, False)
    End Sub
    Private Sub TextBox2_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles TextBox2.Leave
        ' Obtener el valor ingresado en el TextBox
        Dim width As Integer = 0
        Integer.TryParse(TextBox2.Text, width)

        ' Modificar el archivo "bio3.ini" con el valor correspondiente
        Dim contenido As String = My.Computer.FileSystem.ReadAllText("bio3.ini")
        contenido = Regex.Replace(contenido, "Width=\d+", "Width=" & width.ToString())
        My.Computer.FileSystem.WriteAllText("bio3.ini", contenido, False)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

    End Sub
End Class
