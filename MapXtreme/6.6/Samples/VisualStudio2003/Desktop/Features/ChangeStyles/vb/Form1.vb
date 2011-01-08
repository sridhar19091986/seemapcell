Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports MapInfo.Mapping
Imports MapInfo.Styles
Imports MapInfo.Windows.Dialogs


Public Class Form1
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
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


    Private _lineStyleDlg As LineStyleDlg = Nothing
    Private _areaStyleDlg As AreaStyleDlg = Nothing
    Private _textStyleDlg As TextStyleDlg = Nothing
    Private _symbolStyleDlg As SymbolStyleDlg = Nothing

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents button5 As System.Windows.Forms.Button
    Friend WithEvents mapControl1 As MapInfo.Windows.Controls.MapControl
    Friend WithEvents button4 As System.Windows.Forms.Button
    Friend WithEvents button3 As System.Windows.Forms.Button
    Friend WithEvents button2 As System.Windows.Forms.Button
    Friend WithEvents button1 As System.Windows.Forms.Button

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.button5 = New System.Windows.Forms.Button
        Me.mapControl1 = New MapInfo.Windows.Controls.MapControl
        Me.button4 = New System.Windows.Forms.Button
        Me.button3 = New System.Windows.Forms.Button
        Me.button2 = New System.Windows.Forms.Button
        Me.button1 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'button5
        '
        Me.button5.Location = New System.Drawing.Point(83, 391)
        Me.button5.Name = "button5"
        Me.button5.Size = New System.Drawing.Size(144, 27)
        Me.button5.TabIndex = 23
        Me.button5.Text = "Change Sparse Style"
        '
        'mapControl1
        '
        Me.mapControl1.Location = New System.Drawing.Point(54, 31)
        Me.mapControl1.Name = "mapControl1"
        Me.mapControl1.Size = New System.Drawing.Size(701, 286)
        Me.mapControl1.TabIndex = 22
        Me.mapControl1.Text = "mapControl1"
        Me.mapControl1.Tools.LeftButtonTool = "Arrow"
        Me.mapControl1.Tools.MiddleButtonTool = Nothing
        Me.mapControl1.Tools.RightButtonTool = Nothing
        '
        'button4
        '
        Me.button4.Location = New System.Drawing.Point(572, 336)
        Me.button4.Name = "button4"
        Me.button4.Size = New System.Drawing.Size(135, 37)
        Me.button4.TabIndex = 21
        Me.button4.Text = "Change SymbolStyle"
        '
        'button3
        '
        Me.button3.Location = New System.Drawing.Point(409, 336)
        Me.button3.Name = "button3"
        Me.button3.Size = New System.Drawing.Size(125, 28)
        Me.button3.TabIndex = 20
        Me.button3.Text = "Change TextStyle"
        '
        'button2
        '
        Me.button2.Location = New System.Drawing.Point(246, 336)
        Me.button2.Name = "button2"
        Me.button2.Size = New System.Drawing.Size(125, 28)
        Me.button2.TabIndex = 19
        Me.button2.Text = "Change LineStyle"
        '
        'button1
        '
        Me.button1.Location = New System.Drawing.Point(83, 336)
        Me.button1.Name = "button1"
        Me.button1.Size = New System.Drawing.Size(124, 26)
        Me.button1.TabIndex = 18
        Me.button1.Text = "Change AreaStyle"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(808, 448)
        Me.Controls.Add(Me.button5)
        Me.Controls.Add(Me.mapControl1)
        Me.Controls.Add(Me.button4)
        Me.Controls.Add(Me.button3)
        Me.Controls.Add(Me.button2)
        Me.Controls.Add(Me.button1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

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

        MapInfo.Engine.Session.Current.TableSearchPath.Path = s
        Dim geoSetName As String = "world.gst"
        Try
            mapControl1.Map.Load(New MapGeosetLoader(s + geoSetName))
        Catch
            MessageBox.Show("Geoset " + geoSetName + " not found.")
        End Try



    End Sub

    ' Change Area style of all regions in the region layer
    Private Sub button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button1.Click
        'Get the layer we want
        Dim _lyr As FeatureLayer = Me.mapControl1.Map.Layers("world")
        'Create and show the style dialog
        If _areaStyleDlg Is Nothing Then
            _areaStyleDlg = New AreaStyleDlg
        End If
        ' After getting style from dialog, create and apply the featureoverridestylemodifier object to layer
        If _areaStyleDlg.ShowDialog() = DialogResult.OK Then
            Dim fsm As FeatureOverrideStyleModifier = New FeatureOverrideStyleModifier(Nothing, New MapInfo.Styles.CompositeStyle(_areaStyleDlg.AreaStyle))
            _lyr.Modifiers.Append(fsm)
            Me.mapControl1.Map.Zoom = New MapInfo.GeomeTry.Distance(25000, MapInfo.Geometry.DistanceUnit.Mile)
        End If
    End Sub

    ' Change Area style of all regions in the region layer
    Private Sub button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button2.Click
        Dim _lyr As FeatureLayer = Me.mapControl1.Map.Layers("grid15")
        If _lineStyleDlg Is Nothing Then
            _lineStyleDlg = New LineStyleDlg
        End If
        If _lineStyleDlg.ShowDialog() = DialogResult.OK Then
            Dim fsm As FeatureOverrideStyleModifier = New FeatureOverrideStyleModifier(Nothing, New MapInfo.Styles.CompositeStyle(_lineStyleDlg.LineStyle))
            _lyr.Modifiers.Append(fsm)
            Me.mapControl1.Map.Zoom = New MapInfo.GeomeTry.Distance(25000, MapInfo.Geometry.DistanceUnit.Mile)
        End If
    End Sub
    Private Sub button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button3.Click
        Dim _lyr As MapInfo.Mapping.LabelLayer = Me.mapControl1.Map.Layers("worldlabels")
        If _textStyleDlg Is Nothing Then
            _textStyleDlg = New TextStyleDlg
        End If
        If _textStyleDlg.ShowDialog() = DialogResult.OK Then
            _lyr.Sources("world").DefaultLabelProperties.Style = New TextStyle(_textStyleDlg.FontStyle, _lyr.Sources("world").DefaultLabelProperties.Style.CalloutLine)
            _lyr.Sources("worldcap").DefaultLabelProperties.Style = New TextStyle(_textStyleDlg.FontStyle, _lyr.Sources("worldcap").DefaultLabelProperties.Style.CalloutLine)
            _lyr.Sources("wldcty25").DefaultLabelProperties.Style = New TextStyle(_textStyleDlg.FontStyle, _lyr.Sources("wldcty25").DefaultLabelProperties.Style.CalloutLine)
            Me.mapControl1.Map.Zoom = New MapInfo.GeomeTry.Distance(6250, MapInfo.Geometry.DistanceUnit.Mile)
        End If
    End Sub

    Private Sub button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button4.Click
        Dim _lyr As FeatureLayer = Me.mapControl1.Map.Layers("worldcap")
        If _symbolStyleDlg Is Nothing Then
            _symbolStyleDlg = New SymbolStyleDlg
        End If
        If _symbolStyleDlg.ShowDialog() = DialogResult.OK Then
            Dim fsm As FeatureOverrideStyleModifier = New FeatureOverrideStyleModifier(Nothing, New MapInfo.Styles.CompositeStyle(_symbolStyleDlg.SymbolStyle))
            _lyr.Modifiers.Append(fsm)
            Me.mapControl1.Map.Zoom = New MapInfo.GeomeTry.Distance(6250, MapInfo.Geometry.DistanceUnit.Mile)
        End If
    End Sub

    Private Sub button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button5.Click
        ' Get the layer we want
        Dim _lyr As FeatureLayer = Me.mapControl1.Map.Layers("worldcap")

        'Create a sparse point style
        Dim vs As MapInfo.Styles.SimpleVectorPointStyle = New SimpleVectorPointStyle

        'Just change the color and code and attributes flag to indicate that
        vs.Code = 55
        vs.PointSize = 25

        vs.Color = System.Drawing.Color.Red
        ' vs.Attributes = StyleAttributes.PointAttributes.Color | StyleAttributes.PointAttributes.VectorCode;



        ' And apply to the layer
        Dim fsm As FeatureOverrideStyleModifier = New FeatureOverrideStyleModifier(Nothing, New MapInfo.Styles.CompositeStyle(vs))
        _lyr.Modifiers.Append(fsm)
        Me.mapControl1.Map.Zoom = New MapInfo.GeomeTry.Distance(6250, MapInfo.Geometry.DistanceUnit.Mile)
    End Sub


End Class
