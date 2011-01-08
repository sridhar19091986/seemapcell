Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports System.IO
Imports System.Reflection
Imports MapInfo.Data
Imports MapInfo.Mapping
Imports MapInfo.Engine
Imports MapInfo.Styles


Public Class Form1
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'me call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Private _map As MapInfo.Mapping.Map = Nothing
    Private _rasterTable As Table = Nothing

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents button1 As System.Windows.Forms.Button
    Friend WithEvents statusBar1 As System.Windows.Forms.StatusBar
    Friend WithEvents button2 As System.Windows.Forms.Button
    Friend WithEvents panel1 As System.Windows.Forms.Panel
    Friend WithEvents mapControl1 As MapInfo.Windows.Controls.MapControl
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.button1 = New System.Windows.Forms.Button
        Me.statusBar1 = New System.Windows.Forms.StatusBar
        Me.button2 = New System.Windows.Forms.Button
        Me.panel1 = New System.Windows.Forms.Panel
        Me.mapControl1 = New MapInfo.Windows.Controls.MapControl
        Me.panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'button1
        '
        Me.button1.Location = New System.Drawing.Point(48, 16)
        Me.button1.Name = "button1"
        Me.button1.Size = New System.Drawing.Size(115, 27)
        Me.button1.TabIndex = 11
        Me.button1.Text = "Get RasterInfo"
        '
        'statusBar1
        '
        Me.statusBar1.Location = New System.Drawing.Point(0, 378)
        Me.statusBar1.Name = "statusBar1"
        Me.statusBar1.Size = New System.Drawing.Size(568, 22)
        Me.statusBar1.TabIndex = 10
        '
        'button2
        '
        Me.button2.Location = New System.Drawing.Point(176, 16)
        Me.button2.Name = "button2"
        Me.button2.Size = New System.Drawing.Size(134, 27)
        Me.button2.TabIndex = 12
        Me.button2.Text = "Get RasterStyle"
        '
        'panel1
        '
        Me.panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panel1.Controls.Add(Me.mapControl1)
        Me.panel1.Location = New System.Drawing.Point(24, 56)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(517, 327)
        Me.panel1.TabIndex = 9
        '
        'mapControl1
        '
        Me.mapControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.mapControl1.Location = New System.Drawing.Point(0, 0)
        Me.mapControl1.Name = "mapControl1"
        Me.mapControl1.Size = New System.Drawing.Size(513, 323)
        Me.mapControl1.TabIndex = 0
        Me.mapControl1.Text = "mapControl1"
        Me.mapControl1.Tools.LeftButtonTool = "ZoomIn"
        Me.mapControl1.Tools.MiddleButtonTool = Nothing
        Me.mapControl1.Tools.RightButtonTool = Nothing
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(568, 400)
        Me.Controls.Add(Me.button1)
        Me.Controls.Add(Me.statusBar1)
        Me.Controls.Add(Me.button2)
        Me.Controls.Add(Me.panel1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set table search path to value sampledatasearch registry key
        ' if not found, then just use the app's current directory
        Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\MapInfo\MapXtreme\6.6")
        Dim s As String = CType(key.GetValue("SampleDataSearchPath"), String)
        If s <> Nothing And s.Length > 0 Then
            If s.EndsWith("\\") = False Then
                s += "\\"
            End If
        Else
            s = Environment.CurrentDirectory
        End If
        key.Close()

        Session.Current.TableSearchPath.Path = s

        ' load some layers, set csys and default view
        _map = mapControl1.Map
        Try
            _map.Load(New MapTableLoader("florida.tab"))
        Catch ex As Exception
            MessageBox.Show(Me, "Unable to load tables. Sample Data may not be installed.\r\n" + ex.Message)
            Me.Close()
            Return
        End Try

        SetUpRasterLayer()
    End Sub


    Private Sub SetUpRasterLayer()
        Dim myRasterLayer As FeatureLayer = _map.Layers("florida")
        _rasterTable = myRasterLayer.Table


        Dim rs As RasterStyle = New RasterStyle
        rs.Contrast = 33
        rs.Grayscale = True

        ' this composite style will affect the raster as intended 
        Dim csRaster As CompositeStyle = New CompositeStyle(rs)
        Dim fosm As FeatureOverrideStyleModifier = New FeatureOverrideStyleModifier("Style Mod", csRaster)

        Dim modifiers As FeatureStyleModifiers = myRasterLayer.Modifiers
        modifiers.Append(fosm)
    End Sub

    ' Handler function called when the active map's view changes
    Private Sub Map_ViewChanged(ByVal o As Object, ByVal e As ViewChangedEventArgs)
        ' Get the map
        Dim map As MapInfo.Mapping.Map = CType(o, MapInfo.Mapping.Map)
        ' Display the zoom level
        Dim dblZoom As Double = System.Convert.ToDouble(String.Format("{0:E2}", mapControl1.Map.Zoom.Value))
        statusBar1.Text = "Zoom: " + dblZoom.ToString() + " " + mapControl1.Map.Zoom.Unit.ToString()
    End Sub


    Private Sub button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button1.Click

        DoShowRasterInfo(_rasterTable)
    End Sub

    ' Gets RasterInfo from a given table.
    ' Each raster table has exactly one associated raster image, and thus only one record.
    ' Reading that record with a data reader, we can get 
    ' a FeatureGeometry - a bounding rectangle,
    ' a Style - the Raster style,
    ' a key 
    ' a RasterInfo object.
    Private Function GetRasterInfo(ByVal table As Table) As MapInfo.Raster.RasterInfo
        Dim rasterInfo As MapInfo.Raster.RasterInfo = Nothing
        Dim rdr As MIDataReader = Nothing
        Dim projectionlist As String = "obj, MI_Key, MI_Raster, MI_Style"
        rdr = table.ExecuteReader(projectionlist)

        Dim name As String
        Dim typename As String
        Dim n As Integer = rdr.FieldCount
        Dim style As MapInfo.Styles.Style
        Dim featureGeomeTry As MapInfo.GeomeTry.FeatureGeometry
        Dim key As MapInfo.Data.Key
        If rdr.Read() Then
            Dim i As Integer
            For i = 0 To rdr.FieldCount - 1 Step i + 1
                name = rdr.GetName(i)
                typename = rdr.GetDataTypeName(i)
                If typename = "MapInfo.Styles.Style" Then
                    style = rdr.GetStyle(i)
                ElseIf typename = "MapInfo.Geometry.FeatureGeometry" Then
                    featureGeomeTry = rdr.GetFeatureGeometry(i)
                ElseIf typename = "MapInfo.Data.Key" Then
                    key = rdr.GetKey(i)
                ElseIf typename = "MapInfo.Raster.RasterInfo" Then
                    rasterInfo = rdr.GetRasterInfo(i)
                End If
            Next
        End If
        rdr.Close()
        rdr.Dispose()
        rdr = Nothing
        Return rasterInfo
    End Function

    Private Function GetRasterStyle(ByVal table As Table) As MapInfo.Styles.RasterStyle
        Dim rasterStyle As MapInfo.Styles.RasterStyle = Nothing
        Dim rdr As MIDataReader = Nothing
        Dim projectionlist As String = "MI_Style"
        rdr = table.ExecuteReader(projectionlist)

        Dim name As String
        Dim typename As String
        Dim n As Integer = rdr.FieldCount
        Dim style As MapInfo.Styles.Style = Nothing
        If rdr.Read() Then
            Dim i As Integer
            For i = 0 To rdr.FieldCount - 1 Step i + 1
                name = rdr.GetName(i)
                typename = rdr.GetDataTypeName(i)
                If typename = "MapInfo.Styles.Style" Then
                    style = rdr.GetStyle(i)
                End If
            Next
        End If
        rdr.Close()
        rdr.Dispose()
        rdr = Nothing
        If Not style Is Nothing Then
            rasterStyle = style
        End If
        Return rasterStyle
    End Function

    Private Sub DoShowRasterInfo(ByVal table As MapInfo.Data.Table)
        ' Get Raster info from the table.
        Dim rasterInfo As MapInfo.Raster.RasterInfo = GetRasterInfo(table)
        MessageBox.Show(Me, String.Format("imageHeight = {0}\n imageWidth = {1}\n", rasterInfo.Height, rasterInfo.Width))
    End Sub

    Private Sub DoShowRasterStyle(ByVal table As MapInfo.Data.Table)
        ' Get Raster info from the table.
        Dim rasterStyle As MapInfo.Styles.RasterStyle = GetRasterStyle(table)
        MessageBox.Show(Me, String.Format("Brightness = {0}\n Contrast = {1}\n", rasterStyle.Brightness, rasterStyle.Contrast))
    End Sub

    Private Sub button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button2.Click
        DoShowRasterStyle(_rasterTable)
    End Sub

End Class
