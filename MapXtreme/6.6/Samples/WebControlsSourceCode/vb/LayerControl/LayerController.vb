Imports System
Imports System.IO
Imports System.Web
Imports System.Web.SessionState
Imports System.Xml
Imports System.Xml.XPath
Imports System.Xml.Xsl
Imports System.Text.RegularExpressions


Public Class LayerController
    Implements IHttpHandler, IRequiresSessionState

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim command As String = context.Request("Command")
        Dim uniqueID As String = context.Request("UniqueID")
        Dim mapAlias As String = context.Request("MapAlias")
        Dim mapControlID As String = context.Request("MapControlID")
        ' Get Xslt file name
        Dim xsltFile As String = context.Request("XsltFile")
        ' Get the resource path name passed as data and properly encode it.
        Dim buffer() As Byte = New Byte(context.Request.InputStream.Length) {}
        Dim read As Integer = context.Request.InputStream.Read(buffer, 0, CType(context.Request.InputStream.Length, Integer))
        Dim resourcePath As String = System.Text.UTF8Encoding.UTF8.GetString(buffer, 0, read)
        ' in VB trim the null out of the string. CS works fine without trimming.
        Dim imgPath As String = resourcePath
        ' Form the full path for xslt file
        Dim xsltPath As String = context.Server.MapPath(resourcePath) + "\\" + xsltFile

        Dim layerModel As LayerControlModel = LayerControlModel.SetDefaultModelInSession()

        Select Case command
            Case "GetHTML"

                Dim treeDoc As XmlDocument = layerModel.GetLayerXML(mapAlias, uniqueID, imgPath)

                ' load the xslt file into reader
                Dim tr As System.IO.TextReader = New System.IO.StreamReader(xsltPath)
                Dim ms As MemoryStream = New MemoryStream
                Dim sw As StreamWriter = New StreamWriter(ms)
                ' Now replace the tags in the xslt file with actual strings from resources
                Dim line As String = ""
                While True
                    line = tr.ReadLine()
                    If (line Is Nothing) Then
                        Exit While
                    End If
                    Dim regexp As Regex = New Regex("(Treeview2_([0-9]+))")
                    Dim mc As MatchCollection = regexp.Matches(line)
                    Dim i As Integer
                    For i = mc.Count - 1 To 0 Step i - 1
                        Dim m As Match = mc(i)
                        Dim g As Group = m.Groups(0)
                        Dim c As Capture = m.Groups(0).Captures(0)
                        Dim resName As String = line.Substring(c.Index, c.Length)
                        If (resName.Equals("Treeview2_3")) Then
                            line = line.Replace(resName, Resources.ResourceFolder)
                        Else
                            Dim resString As String = L10NUtils.Resources.GetString(resName)
                            line = line.Replace(resName, resString)
                        End If
                    Next
                    sw.WriteLine(line)
                End While

                sw.Flush()
                ms.Position = 0

                ' Use that TextReader as the Source for the XmlTextReader
                Dim xr As System.Xml.XmlReader = New System.Xml.XmlTextReader(ms)
                ' Create a new XslTransform class
                Dim treeView As System.Xml.Xsl.XslTransform = New System.Xml.Xsl.XslTransform
                ' Load the XmlReader StyleSheet into the XslTransform class
                treeView.Load(xr, Nothing, Nothing)

                Dim sw2 As StringWriter = New StringWriter

                Dim nav As XPathNavigator = treeDoc.CreateNavigator()

                ' Do the transform and write to response
                treeView.Transform(nav, Nothing, sw2, Nothing)
                context.Response.Write(sw2.ToString())
                tr.Close()
                sw.Close()
                ms.Close()
                sw2.Close()
                Return
        End Select
    End Sub

End Class
