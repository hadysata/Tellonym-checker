Imports System.Net
Imports System.Threading

Public Class Form1
    Dim list() As String
    Dim avs As Integer = 0
    Dim takens As Integer = 0
    Dim errors As Integer = 0

    Dim stoop As Boolean = False

    'For user can move the form 'Panel'
    Private isMouseDown As Boolean = False
    Private mouseOffset As Point

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        End
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Control.CheckForIllegalCrossThreadCalls = False
        Try
            list = IO.File.ReadAllLines("list.txt")
        Catch ex As Exception
            MsgBox("Can't found list.txt file", MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Function check(ByVal username As String) As String
        Try

            Using Web As New WebClient
                ServicePointManager.DefaultConnectionLimit = 300
                ServicePointManager.UseNagleAlgorithm = False
                ServicePointManager.Expect100Continue = False
                ServicePointManager.CheckCertificateRevocationList = False
                If Web.DownloadString("https://api.tellonym.me/accounts/check?username=" & username & "&limit=13") = "{""username"":true}" Then
                    ListBox1.Items.Add(username)
                    avs += 1
                Else
                    takens += 1
                End If
                GC.Collect()
            End Using

        Catch ex As Exception
            errors += 1
            Return "Error"
        End Try
    End Function

    Private Sub work()
        Try

            For i As Integer = 0 To list.LongCount - 1
                Dim th As New Thread(AddressOf check) : th.Start(list(i))
                Thread.Sleep(NumericUpDown1.Value)
                If stoop = True Then
                    Exit Sub
                End If
            Next

        Catch ex As Exception
            Thread.Sleep(2000)
            Dim th As New Thread(AddressOf work) : th.Start()
        End Try
    End Sub

     

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Label4.Text = "Avaliable : " & avs
        Label5.Text = "Taken : " & takens
        Label6.Text = "Errors : " & errors
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim th As New Thread(AddressOf work) : th.Start()
    End Sub


    'For user can move the form 'Panel'

    Private Sub Panel4_MouseDown1(sender As Object, e As MouseEventArgs) Handles Panel4.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            mouseOffset = New Point(-e.X, -e.Y)
            isMouseDown = True
        End If
    End Sub
    Private Sub Panel4_MouseMove1(sender As Object, e As MouseEventArgs) Handles Panel4.MouseMove
        If isMouseDown Then
            Dim mousePos As Point = Control.MousePosition
            mousePos.Offset(mouseOffset.X, mouseOffset.Y)
            Me.Location = mousePos
        End If
    End Sub
    Private Sub Panel4_MouseUp1(sender As Object, e As MouseEventArgs) Handles Panel4.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Left Then
            isMouseDown = False
        End If
    End Sub
    Private Sub Label7_MouseDown(sender As Object, e As MouseEventArgs) Handles Label7.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            mouseOffset = New Point(-e.X, -e.Y)
            isMouseDown = True
        End If
    End Sub
    Private Sub Label7_MouseMove(sender As Object, e As MouseEventArgs) Handles Label7.MouseMove
        If isMouseDown Then
            Dim mousePos As Point = Control.MousePosition
            mousePos.Offset(mouseOffset.X, mouseOffset.Y)
            Me.Location = mousePos
        End If
    End Sub
    Private Sub Label7_MouseUp(sender As Object, e As MouseEventArgs) Handles Label7.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Left Then
            isMouseDown = False
        End If
    End Sub
End Class
