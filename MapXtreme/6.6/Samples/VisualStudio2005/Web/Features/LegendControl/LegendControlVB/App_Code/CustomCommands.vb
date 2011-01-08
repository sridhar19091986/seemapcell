Imports System
Imports System.Drawing
Imports System.IO
Imports System.Web
Imports MapInfo.Mapping
Imports MapInfo.Mapping.Legends
Imports MapInfo.Mapping.Thematics
Imports MapInfo.WebControls
Imports MapInfo.Styles
Imports Font = MapInfo.Styles.Font


Namespace LegendControlWeb





' <summary>
' some constants used in this project
' </summary>
Public Class ProjectConstants
    Public Shared ThemeLayerAlias As String = "world"
    Public Shared ThemeAlias As String = "worldtheme"
    Public Shared IndColumnName As String = "Country"
End Class

' <summary>
' CreateTheme command for creating individual theme and legend.
' </summary>
<Serializable()> _
Public Class CreateTheme
    Inherits MapInfo.WebControls.MapBaseCommand

    ' <summary>
    ' Constructor for CreateTheme class
    ' </summary> 
    Public Sub New()
        Name = "CreateTheme"
    End Sub


    Public Overrides Sub Process()
        Dim model As MapControlModel = MapControlModel.GetModelFromSession()
        model.SetMapSize(MapAlias, MapWidth, MapHeight)

        'get map object from map model
        Dim map As MapInfo.Mapping.Map = model.GetMapObj(MapAlias)
        Dim fLyr As FeatureLayer = map.Layers(ProjectConstants.ThemeLayerAlias)
        fLyr.Modifiers.Clear()
        map.Legends.Clear()

        'create theme
        Dim iTheme As New IndividualValueTheme(fLyr, ProjectConstants.IndColumnName, ProjectConstants.ThemeAlias)
        fLyr.Modifiers.Insert(0, iTheme)

        'create legend based on the individual value theme
        Dim lg As MapInfo.Mapping.Legends.Legend = map.Legends.CreateLegend(New Size(236, 282))

        'create legend frame
        Dim lgFrame As ThemeLegendFrame = LegendFrameFactory.CreateThemeLegendFrame(iTheme)
        lg.Frames.Append(lgFrame)

        'modify legend frame style
        lgFrame.BackgroundBrush = New SolidBrush(Color.AliceBlue)
        lgFrame.Title = "World Country Legend"
        lgFrame.SubTitle = " "

        Dim titleFont As New MapInfo.Styles.Font("Arial", 10)
        titleFont.ForeColor = Color.DarkBlue
        titleFont.FontWeight = FontWeight.Bold

        lgFrame.TitleStyle = titleFont

        Dim rowTextStyle = New Font("Arial", 8)
        rowTextStyle.FontWeight = FontWeight.Bold

        lgFrame.RowTextStyle = rowTextStyle

        'stream map image back to client
        Dim ms As MemoryStream = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat)
        StreamImageToClient(ms)
    End Sub
End Class

' <summary>
' RemoveTheme command for removing all themes from the map.
' </summary>
<Serializable()> _
Public Class RemoveTheme
    Inherits MapInfo.WebControls.MapBaseCommand

    ' <summary>
    ' Constructor for RemoveTheme class
    ' </summary>
    ' 
    Public Sub New()
        Name = "RemoveTheme"
    End Sub

    Public Overrides Sub Process()
        Dim model As MapControlModel = MapControlModel.GetModelFromSession()
        model.SetMapSize(MapAlias, MapWidth, MapHeight)

        'get map object from map model
        Dim map As MapInfo.Mapping.Map = model.GetMapObj(MapAlias)
        Dim fLyr As FeatureLayer = map.Layers(ProjectConstants.ThemeLayerAlias)
        fLyr.Modifiers.Clear()
        map.Legends.Clear()

        Dim ms As MemoryStream = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat)
        StreamImageToClient(ms)
    End Sub
End Class


' <summary>
' GetLegend command for streaming legend image back to client.
' </summary>
<Serializable()> _
Public Class GetLegend
    Inherits MapInfo.WebControls.MapBaseCommand

    'key to get LegendExportFormat value from client request
    Public LegendExportFormatKey As String = "LegendExportFormat"


    ' <summary>
    '  Constructor for CreateTheme class
    ' </summary>
    Public Sub New()
        Name = "GetLegend"
    End Sub

    ' <summary>
    ' Override parent ParseContext method and add assigning value to LegendExportFormat property.
    ' </summary>
    Public Overrides Sub ParseContext()
        MyBase.ParseContext()
        LegendExportFormat = HttpContext.Current.Request(LegendExportFormatKey)
    End Sub


    ' <summary>
    ' Override the Execute method in MapBasicCommand class to not save state, because for legend control, which does not change map state, so there is no need to save map state.
    ' </summary>
    Public Overrides Sub Execute()

        Dim sm As StateManager = StateManager.GetStateManagerFromSession()
        If sm Is Nothing Then
            If StateManager.IsManualState() Then
                Throw New NullReferenceException("Cannot find instance of StateManager in the ASP.NET session.")
            End If
        End If
        ParseContext()
        If Not (sm Is Nothing) Then
            PrepareStateManagerParamsDictionary(sm)
            sm.RestoreState()
        End If

        Process()
    End Sub



    Public Overrides Sub Process()
        Dim model As MapControlModel = MapControlModel.GetModelFromSession()

        'get map object from map model
        Dim map As MapInfo.Mapping.Map = model.GetMapObj(MapAlias)

        If map.Legends.Count = 0 Then
            Return
        End If
        Dim legend As MapInfo.Mapping.Legends.Legend = map.Legends(0)

        Dim legendExp As New LegendExport(map, legend)
        legendExp.Format = CType(MapInfo.Mapping.ExportFormat.Parse(GetType(ExportFormat), LegendExportFormat, True), MapInfo.Mapping.ExportFormat)

        'export Legend to memorystream
        Dim stream As New MemoryStream
        legendExp.Export(stream)
        stream.Position = 0
        legendExp.Dispose()

        'stream legend image back to client
        StreamImageToClient(stream)
    End Sub


    ' <summary>
    ' Stream legend image in memory stream back to client.
    ' </summary>
    ' <param name="ms">memory stream holding legend image</param>
    Public Overrides Sub StreamImageToClient(ByVal ms As MemoryStream)
        If Not (ms Is Nothing) Then
            Dim contentType As String = String.Format("image/{0}", LegendExportFormat)
            Dim reader As New BinaryReader(ms)
            Dim length As Integer = CInt(ms.Length)
            If Not (contentType Is Nothing) Then
                HttpContext.Current.Response.ContentType = contentType
            End If
            HttpContext.Current.Response.OutputStream.Write(reader.ReadBytes(length), 0, length)
            reader.Close()
            ms.Close()
        End If
    End Sub

    Private _legendExportFormat As String

    Public Property LegendExportFormat() As String
        Get
            Return _legendExportFormat
        End Get
        Set(ByVal Value As String)
            _legendExportFormat = Value
        End Set
    End Property
End Class

End Namespace
