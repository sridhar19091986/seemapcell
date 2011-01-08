Imports System
Imports System.IO
Imports System.Xml
Imports System.Xml.XPath
Imports System.Xml.Xsl
Imports MapInfo.Data
Imports MapInfo.GeomeTry
Imports MapInfo.Mapping
Imports MapInfo.Styles
Imports System.Web

<Serializable()> _
Public Class LayerControlModel
    Private _mapAlias As String = Nothing

    '/ <summary>
    '/ This method creates the default model provided by MapXTreme and sets the object in the ASP.NET session.
    '/ </summary>
    '/ <remarks>This model is extracted from the session and used to do business logic</remarks>
    '/ <param name="context">HttpContext</param>
    '/ <returns>MapControlMode from session</returns>
    Public Shared Function SetDefaultModelInSession() As LayerControlModel
        Dim context As HttpContext = HttpContext.Current
        Dim model As LayerControlModel = GetModelFromSession()
        If model Is Nothing Then
            model = New LayerControlModel
            SetModelInSession(model)
        End If
        Return model
    End Function

    '/ <summary>
    '/ Gets model from ASP.NET session
    '/ </summary>
    '/ <returns></returns>
    Public Shared Function GetModelFromSession() As LayerControlModel
        Dim context As HttpContext = HttpContext.Current
        Dim key As String = String.Format("{0}_LayerModel", context.Session.SessionID)
        Dim model As LayerControlModel = Nothing
        If Not context.Session(key) Is Nothing Then
            model = CType(context.Session(key), LayerControlModel)
        End If
        Return model
    End Function

    '/ <summary>
    '/ Set the given model in ASP.NET session
    '/ </summary>
    '/ <remarks>If custom model is used, then this method can be used to set it in session and that will be used instead</remarks?>
    '/ <param name="model">model to be set in the ASP.NET session</param>
    Public Shared Sub SetModelInSession(ByVal model As LayerControlModel)
        Dim context As HttpContext = HttpContext.Current
        Dim key As String = String.Format("{0}_LayerModel", context.Session.SessionID)
        context.Session(key) = model
    End Sub

    Private Function CreateFeatureLayerElement(ByVal _doc As XmlDocument, ByVal layer As IMapLayer, ByVal uniqueID As String) As XmlElement
        Dim map As map = GetMapObj(_mapAlias)
        Dim text1 As String
        Dim layer1 As FeatureLayer = layer
        Dim bRasterLayer As Boolean = False
        Dim bHasModifiers As Boolean = False
        Dim modifiers1 As FeatureStyleModifiers = layer1.Modifiers
        If (modifiers1.Count > 0) Then
            bHasModifiers = True
        End If
        If (((layer1.Type = LayerType.Raster) OrElse (layer1.Type = LayerType.Grid)) OrElse (layer1.Type = LayerType.Wms)) Then
            bRasterLayer = True
        End If
        Dim element1 As XmlElement = _doc.CreateElement("branch")
        If bHasModifiers Then
            element1.SetAttribute("branchtype", "", "folder")
            element1.SetAttribute("expanded", Nothing, "true")
        Else
            element1.SetAttribute("branchtype", "", "leaf")
        End If
        element1.SetAttribute("title", Nothing, layer.Name)
        element1.SetAttribute("alias", "", layer.Alias)
        element1.SetAttribute("type", "", layer.Type.ToString)
        element1.SetAttribute("uniqueid", "", uniqueID)
        If bRasterLayer Then
            element1.SetAttribute("code", "", "nonselectable")
        Else
            If bHasModifiers Then
                element1.SetAttribute("code", "", "4")
            Else
                element1.SetAttribute("code", "", "featurelayer")
            End If
        End If
        If layer.IsVisible Then
            element1.SetAttribute("visible", "true")
        Else
            element1.SetAttribute("visible", "false")
            If layer1.VisibleRangeEnabled Then
                Dim range1 As VisibleRange = layer1.VisibleRange
                If Not range1.Within(map.Zoom) Then
                    element1.SetAttribute("rangevisible", "true")
                Else
                    element1.SetAttribute("rangevisible", "false")
                End If
            End If
        End If

        text1 = Me.DetermineRemoteGeomType(layer1)
        If (text1 Is Nothing) Then
            text1 = "lclayerpoint.bmp"
            If bRasterLayer Then
                text1 = "lclayerraster.bmp"
            Else
                Dim info1 As TableInfo = layer1.Table.TableInfo
                Dim column1 As GeometryColumn = Nothing
                Dim columns1 As Columns = info1.Columns
                Dim column2 As Column
                For Each column2 In columns1
                    If (column2 Is GetType(GeometryColumn)) Then
                        column1 = CType(column2, GeometryColumn)
                    Else
                        Exit For
                    End If
                Next
                If (Not column1 Is Nothing) Then
                    If (column1.PredominantGeometryType = GeometryType.MultiCurve) Then
                        text1 = "lclayerline.bmp"
                    Else
                        If ((column1.PredominantGeometryType = GeometryType.MultiPolygon) OrElse (column1.PredominantGeometryType = GeometryType.Rectangle)) Then
                            text1 = "lclayerregion.bmp"
                        Else
                            If (column1.PredominantGeometryType = GeometryType.Point) Then
                                text1 = "lclayerpoint.bmp"
                            End If
                        End If
                    End If
                End If
            End If
        End If
        element1.SetAttribute("img", Nothing, text1)
        If bHasModifiers Then
            CreateFeatureModifierElement(_doc, layer1, element1, uniqueID)
        End If
        Return element1
    End Function

    Private Sub CreateFeatureModifierElement(ByVal _doc As XmlDocument, ByVal layer As FeatureLayer, ByVal lyrElement As XmlElement, ByVal uniqueID As String)
        Dim modifiers1 As FeatureStyleModifiers = layer.Modifiers
        Dim modifier1 As FeatureStyleModifier
        For Each modifier1 In modifiers1
            Dim element As XmlElement = _doc.CreateElement("branch")
            element.SetAttribute("branchtype", "", "leaf")
            element.SetAttribute("title", Nothing, modifier1.Name)
            element.SetAttribute("code", "", "nonselectable")
            element.SetAttribute("img", Nothing, "lcmodifier.bmp")
            If (modifier1.Enabled) Then
                element.SetAttribute("visible", "true")
            Else
                element.SetAttribute("visible", "false")
            End If
            element.SetAttribute("uniqueid", "", uniqueID)
            element.SetAttribute("alias", "", modifier1.Alias)
            element.SetAttribute("type", "", "Mod")
            lyrElement.AppendChild(element)
        Next
    End Sub

    Private Function CreateGroupLayerElement(ByVal _doc As XmlDocument, ByVal layer As IMapLayer, ByVal uniqueID As String) As XmlElement
        Dim map As map = GetMapObj(_mapAlias)
        Dim element As XmlElement = _doc.CreateElement("branch")
        element.SetAttribute("branchtype", "", "folder")
        element.SetAttribute("title", "", CType(layer, GroupLayer).Name)
        element.SetAttribute("code", "", "3")
        element.SetAttribute("uniqueid", "", uniqueID)
        element.SetAttribute("alias", "", layer.Alias)
        element.SetAttribute("expanded", Nothing, "true")
        element.SetAttribute("img", Nothing, "lcgroup.bmp")
        element.SetAttribute("expanded", Nothing, "true")
        element.SetAttribute("type", "", layer.Type.ToString())
        If layer.IsVisible Then
            element.SetAttribute("visible", "true")
        Else
            element.SetAttribute("visible", "false")
            If layer.VisibleRangeEnabled Then
                Dim range1 As VisibleRange = layer.VisibleRange
                If Not range1.Within(map.Zoom) Then
                    element.SetAttribute("rangevisible", "true")
                Else
                    element.SetAttribute("rangevisible", "false")
                End If
            End If
        End If

        Dim inlayer As IMapLayer
        For Each inlayer In CType(layer, GroupLayer)
            If TypeOf inlayer Is MapInfo.Mapping.LabelLayer Then
                element.AppendChild(Me.CreateLabelLayerElement(_doc, inlayer, uniqueID))
            End If
            If TypeOf inlayer Is FeatureLayer Then
                element.AppendChild(Me.CreateFeatureLayerElement(_doc, inlayer, uniqueID))
            End If
            If TypeOf inlayer Is GroupLayer Then
                element.AppendChild(Me.CreateGroupLayerElement(_doc, inlayer, uniqueID))
            End If
            If TypeOf inlayer Is ObjectThemeLayer Then
                element.AppendChild(CreateObjectThemeElement(_doc, inlayer, uniqueID))
            End If
        Next
        Return element
    End Function

    Private Function CreateLabelLayerElement(ByVal _doc As XmlDocument, ByVal layer As IMapLayer, ByVal uniqueID As String) As XmlElement
        Dim map As map = GetMapObj(_mapAlias)
        Dim element1 As XmlElement = _doc.CreateElement("branch")
        element1.SetAttribute("title", "", layer.Name)
        element1.SetAttribute("uniqueid", "", uniqueID)
        element1.SetAttribute("alias", "", layer.Alias)
        element1.SetAttribute("type", "", layer.Type.ToString)
        element1.SetAttribute("expanded", Nothing, "true")
        element1.SetAttribute("img", Nothing, "lclabel.bmp")
        element1.SetAttribute("branchtype", "", "folder")
        If layer.IsVisible Then
            element1.SetAttribute("visible", "true")
        Else
            element1.SetAttribute("visible", "false")
            If layer.VisibleRangeEnabled Then
                Dim range1 As VisibleRange = layer.VisibleRange
                If Not range1.Within(map.Zoom) Then
                    element1.SetAttribute("rangevisible", "true")
                Else
                    element1.SetAttribute("rangevisible", "false")
                End If
            End If
        End If

        Dim source1 As MapInfo.Mapping.LabelSource
        For Each source1 In CType(layer, MapInfo.Mapping.LabelLayer).Sources
            Dim element2 As XmlElement = _doc.CreateElement("branch")
            Dim flag1 As Boolean = False
            Dim modifiers1 As LabelModifiers = source1.Modifiers
            If (modifiers1.Count > 0) Then
                flag1 = True
            End If
            element2.SetAttribute("uniqueid", "", uniqueID)
            element2.SetAttribute("title", Nothing, source1.Name)
            element2.SetAttribute("alias", "", source1.Alias)
            element2.SetAttribute("type", "", layer.Type.ToString)
            If source1.Visible Then
                element2.SetAttribute("visible", "true")
            Else
                element2.SetAttribute("visible", "false")
                If source1.VisibleRangeEnabled Then
                    Dim range1 As VisibleRange = source1.VisibleRange
                    If Not range1.Within(map.Zoom) Then
                        element2.SetAttribute("rangevisible", "true")
                    Else
                        element2.SetAttribute("rangevisible", "false")
                    End If
                End If
            End If
            If flag1 Then
                element2.SetAttribute("branchtype", "", "folder")
                element2.SetAttribute("expanded", Nothing, "true")
                element2.SetAttribute("code", "", "2")
            Else
                element2.SetAttribute("branchtype", "", "leaf")
                element2.SetAttribute("code", "", "nonselectable")
            End If
            element2.SetAttribute("img", Nothing, "lclabelsource.bmp")
            If flag1 Then
                Me.CreateLabelModifierElement(_doc, source1, element2, uniqueID)
            End If
            element1.AppendChild(element2)
        Next
        Return element1
    End Function

    Private Sub CreateLabelModifierElement(ByVal _doc As XmlDocument, ByVal source As MapInfo.Mapping.LabelSource, ByVal lyrElement As XmlElement, ByVal uniqueID As String)
        Dim modifiers1 As LabelModifiers = source.Modifiers
        Dim modifier1 As MapInfo.Mapping.LabelModifier
        For Each modifier1 In modifiers1
            Dim element As XmlElement = _doc.CreateElement("branch")
            element.SetAttribute("branchtype", "", "leaf")
            element.SetAttribute("title", Nothing, modifier1.Name)
            element.SetAttribute("alias", "", modifier1.Alias)
            element.SetAttribute("code", "", "nonselectable")
            element.SetAttribute("type", "", "LabelMod")
            element.SetAttribute("img", Nothing, "lcmodifier.bmp")
            element.SetAttribute("uniqueid", "", uniqueID)
            If (modifier1.Enabled) Then
                element.SetAttribute("visible", "true")
            Else
                element.SetAttribute("visible", "false")
            End If
            lyrElement.AppendChild(element)
        Next
    End Sub

    Private Function CreateObjectThemeElement(ByVal _doc As XmlDocument, ByVal layer As IMapLayer, ByVal uniqueID As String) As XmlElement
        Dim map As map = GetMapObj(_mapAlias)
        Dim element As XmlElement = _doc.CreateElement("branch")
        element.SetAttribute("branchtype", "", "leaf")
        element.SetAttribute("title", Nothing, layer.Name)
        element.SetAttribute("code", "", "nonselectable")
        element.SetAttribute("img", Nothing, "lcmodifier.bmp")
        element.SetAttribute("alias", "", layer.Alias)
        element.SetAttribute("type", "", layer.Type.ToString())
        element.SetAttribute("uniqueid", "", uniqueID)
        If layer.Enabled Then
            element.SetAttribute("visible", "true")
        Else
            element.SetAttribute("visible", "false")
            If layer.VisibleRangeEnabled Then
                Dim range1 As VisibleRange = layer.VisibleRange
                If Not range1.Within(map.Zoom) Then
                    element.SetAttribute("rangevisible", "true")
                Else
                    element.SetAttribute("rangevisible", "false")
                End If
            End If
        End If

        Return element
    End Function

    Private Function DetermineRemoteGeomType(ByVal layer As FeatureLayer) As String
        Dim table1 As Table = layer.Table
        Dim style1 As Style = Nothing
        Dim connection1 As MIConnection = Nothing
        Dim command1 As MICommand = Nothing
        Dim reader1 As MIDataReader = Nothing
        Try
            connection1 = New MIConnection
            connection1.Open()
            command1 = connection1.CreateCommand
            command1.CommandText = "select mi_style from """ & table1.Alias & """"
            command1.CommandType = CommandType.Text
            reader1 = command1.ExecuteReader
            Do While reader1.Read
                If (Not reader1.IsDBNull(0)) Then
                    style1 = reader1.GetStyle(0)
                    Exit Do
                End If
            Loop
        Catch exception1 As MIException
        Finally
            If (Not command1 Is Nothing) Then
                command1.Dispose()
                command1 = Nothing
            End If
            If (Not reader1 Is Nothing) Then
                reader1.Close()
            End If
            If (Not connection1 Is Nothing) Then
                connection1.Close()
                connection1 = Nothing
            End If
        End Try
Label_0096:
        If (Not style1 Is Nothing) Then
            If TypeOf style1 Is SimpleLineStyle Then
                Return "lclayerline.bmp"
            End If
            If (TypeOf style1 Is SimpleInterior OrElse TypeOf style1 Is AreaStyle) Then
                Return "lclayerregion.bmp"
            End If
            If TypeOf style1 Is BasePointStyle Then
                Return "lclayerpoint.bmp"
            End If
            Return "lclayer.bmp"
        End If
        Return Nothing
    End Function

    Private Function GetMapObj(ByVal mapAlias As String) As Map
        Dim map1 As Map = Nothing
        If (mapAlias Is Nothing) Then
            map1 = MapInfo.Engine.Session.Current.MapFactory(0)
        ElseIf (mapAlias.Length <= 0) Then
            map1 = MapInfo.Engine.Session.Current.MapFactory(0)
        Else
            map1 = MapInfo.Engine.Session.Current.MapFactory(mapAlias)
            If (map1 Is Nothing) Then
                map1 = MapInfo.Engine.Session.Current.MapFactory(0)
            End If
        End If
        Return map1
    End Function

    Private Function InitDocument(ByVal doc As XmlDocument) As XmlDocument
        Dim instruction1 As XmlProcessingInstruction = doc.CreateProcessingInstruction("xml", " version='1.0' encoding='UTF-8'")
        doc.AppendChild(instruction1)
        Dim text1 As String = "type='text/xsl' href='treeview.xslt'"
        instruction1 = doc.CreateProcessingInstruction("xml-stylesheet", text1)
        doc.AppendChild(instruction1)
        Dim element1 As XmlElement = doc.CreateElement("treeview")
        doc.AppendChild(element1)
        Return doc
    End Function

    Public Function GetLayerXML(ByVal mapAlias As String, ByVal uniqueID As String, ByVal imgPath As String) As XmlDocument
        _mapAlias = mapAlias

        'Take care of state. Restore state first
        Dim sm As StateManager = StateManager.GetStateManagerFromSession()
        If sm Is Nothing Then
            If StateManager.IsManualState() Then
                Throw New NullReferenceException(L10NUtils.Resources.GetString(StateManager.StateManagerResErr1))
            End If
        End If
        If Not sm Is Nothing Then
            sm.ParamsDictionary(StateManager.ActiveMapAliasKey) = _mapAlias
            sm.RestoreState()
        End If

        Dim map As map = GetMapObj(_mapAlias)

        Dim document1 As XmlDocument = Nothing
        document1 = Me.InitDocument(New XmlDocument)
        Dim element1 As XmlElement = document1.DocumentElement
        Dim element2 As XmlElement = document1.CreateElement("custom-parameters")
        Dim element3 As XmlElement = document1.CreateElement("param")
        element3.SetAttribute("name", "", "param-shift-width")
        element3.SetAttribute("value", "", "15")
        element2.AppendChild(element3)
        Dim element4 As XmlElement = document1.CreateElement("param")
        element4.SetAttribute("name", "", "img-directory")
        element4.SetAttribute("value", "", (imgPath & "/"))
        element2.AppendChild(element4)
        Dim element5 As XmlElement = document1.CreateElement("param")
        element5.SetAttribute("name", "", "mapzoom")
        Dim distance1 As MapInfo.Geometry.Distance = map.Zoom
        Dim num1 As Double = distance1.Value
        element5.SetAttribute("value", "", num1.ToString)
        element2.AppendChild(element5)
        element1.AppendChild(element2)
        Dim element6 As XmlElement = document1.CreateElement("branch")
        element6.SetAttribute("title", "", map.Name)
        element6.SetAttribute("img", Nothing, "lcgroup.bmp")
        element6.SetAttribute("branchtype", "", "folder")
        element6.SetAttribute("expanded", Nothing, "true")
        element6.SetAttribute("nocheckbox", Nothing, "true")
        element1.AppendChild(element6)
        Dim layer1 As IMapLayer
        For Each layer1 In map.Layers
            If TypeOf layer1 Is MapInfo.Mapping.LabelLayer Then
                element6.AppendChild(Me.CreateLabelLayerElement(document1, layer1, uniqueID))
            End If
            If TypeOf layer1 Is FeatureLayer Then
                element6.AppendChild(Me.CreateFeatureLayerElement(document1, layer1, uniqueID))
            End If
            If TypeOf layer1 Is GroupLayer Then
                element6.AppendChild(Me.CreateGroupLayerElement(document1, layer1, uniqueID))
            End If
            If TypeOf layer1 Is ObjectThemeLayer Then
                element6.AppendChild(Me.CreateObjectThemeElement(document1, layer1, uniqueID))
            End If
        Next
        If (Not sm Is Nothing) Then
            sm.SaveState()
        End If
        Return document1
    End Function
End Class
