Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Web
Imports System.Web.SessionState
Imports System.IO
Imports System.Diagnostics
Imports System.Text
Imports System.Reflection
Imports MapInfo.WebControls

Public Class Global
    Inherits System.Web.HttpApplication

#Region " Component Designer Generated Code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container
    End Sub

#End Region

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal Sender As Object, ByVal e As EventArgs)

        Dim ex As Exception = Server.GetLastError()
        If Not ex Is Nothing And ex.Message.Length > 0 Then
            ' If the request was for the map image then write the messages into a bitmap and send it back.
            If HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("MapController") >= 0 Then
                ' Get height and width from the request if it was a request for map image.
                Dim mapWidth As Integer = System.Convert.ToInt32(HttpContext.Current.Request(MapBaseCommand.WidthKey))
                Dim mapHeight As Integer = System.Convert.ToInt32(HttpContext.Current.Request(MapBaseCommand.HeightKey))

                Dim builder1 As StringBuilder = New StringBuilder
                Dim b As Bitmap = New Bitmap(mapWidth, mapHeight)
                Dim g As Graphics = Graphics.FromImage(b)

                ' Append the message from exception
                builder1.Append(ex.Message)
                builder1.Append(System.Environment.NewLine)

                ' Create stack trace for exception
                Dim st As StackTrace = New StackTrace(ex, True)
                Dim num2 As Integer
                For num2 = 0 To st.FrameCount - 1 Step num2 + 1
                    Dim frame1 As StackFrame = st.GetFrame(num2)
                    Dim base1 As MethodBase = frame1.GetMethod()
                    Dim type1 As Type = base1.DeclaringType
                    Dim text1 As String = String.Empty
                    If Not type1 Is Nothing Then
                        text1 = type1.Namespace
                    End If
                    If Not text1 Is Nothing Then
                        If text1.Equals("_ASP") Or text1.Equals("ASP") Then
                            'this._fGeneratedCodeOnStack = true;
                        End If
                        text1 = text1 + "."
                    End If
                    If type1 Is Nothing Then
                        builder1.Append("   " + base1.Name + "(")
                    Else
                        Dim textArray1() As String = New String() {"   ", text1, type1.Name, ".", base1.Name, "("}

                        builder1.Append(String.Concat(textArray1))
                    End If
                    Dim infoArray1() As ParameterInfo = base1.GetParameters()
                    Dim num3 As Integer
                    For num3 = 0 To infoArray1.Length - 1 Step num3 + 1
                        Dim tmp As String
                        If (num3 <> 0) Then
                            tmp = ","
                        Else
                            tmp = ""
                        End If
                        builder1.Append(tmp + infoArray1(num3).ParameterType.Name + " " + infoArray1(num3).Name)
                    Next
                    builder1.Append(")")
                    builder1.Append(System.Environment.NewLine)
                Next

                ' write the strings into the rectangle
                g.DrawString(builder1.ToString(), New Font("Tahoma", 7), New SolidBrush(Color.Yellow), New RectangleF(0, 0, mapWidth, mapHeight), StringFormat.GenericDefault)

                ' Save the bitmap into the stream to send it back
                Dim contentType As String = String.Format("text/HTML")
                If Not contentType Is Nothing Then
                    HttpContext.Current.Response.ContentType = contentType
                End If
                b.Save(HttpContext.Current.Response.OutputStream, ImageFormat.Gif)

                ' End the response here
                HttpContext.Current.Response.End()
            Else
                ' Let the exception pass through
            End If
        End If
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

End Class

