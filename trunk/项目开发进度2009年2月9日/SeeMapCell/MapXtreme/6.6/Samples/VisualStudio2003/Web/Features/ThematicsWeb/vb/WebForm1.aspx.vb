
Public Class WebForm1
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected MapControl1 As MapInfo.WebControls.MapControl
    Protected Zoomintool2 As MapInfo.WebControls.ZoomInTool
    Protected Zoomouttool2 As MapInfo.WebControls.ZoomOutTool
    Protected SouthNavigationTool2 As MapInfo.WebControls.SouthNavigationTool
    Protected NorthNavigationTool2 As MapInfo.WebControls.NorthNavigationTool
    Protected EastNavigationTool2 As MapInfo.WebControls.EastNavigationTool
    Protected WestNavigationTool2 As MapInfo.WebControls.WestNavigationTool
    Protected NorthEastNavigationTool1 As MapInfo.WebControls.NorthEastNavigationTool
    Protected SouthWestNavigationTool1 As MapInfo.WebControls.SouthWestNavigationTool
    Protected SouthEastNavigationTool1 As MapInfo.WebControls.SouthEastNavigationTool
    Protected NorthWestNavigationTool1 As MapInfo.WebControls.NorthWestNavigationTool
    Protected ZoomBarTool1 As MapInfo.WebControls.ZoomBarTool
    Protected ZoomBarTool2 As MapInfo.WebControls.ZoomBarTool
    Protected ZoomBarTool3 As MapInfo.WebControls.ZoomBarTool
    Protected ZoomBarTool4 As MapInfo.WebControls.ZoomBarTool
    Protected ZoomBarTool5 As MapInfo.WebControls.ZoomBarTool
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents Image2 As System.Web.UI.WebControls.Image
    Protected Centertool2 As MapInfo.WebControls.CenterTool
    Protected Pantool2 As MapInfo.WebControls.PanTool
    Protected WithEvents TitleLabel As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownList1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ApplyButton As System.Web.UI.WebControls.Button
    Protected WithEvents CheckBoxList1 As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents RadioButtonList1 As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
        Me.ApplyButton.Attributes.Add("onClick", "javascript:" & ApplyButton.ClientID + ".disabled=true;" & Me.GetPostBackEventReference(ApplyButton))
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' The first time in
        If Session.IsNewSession Then
            '//************************************************************//
            '//*   You need to follow below lines in your own application. //
            '//************************************************************//
            Dim stateManager As New AppStateManager
            ' tell the state manager which map alias you want to use.
            ' You could also add your own key/value pairs, the value should be serializable.
            stateManager.ParamsDictionary.Item(stateManager.ActiveMapAliasKey) = Me.MapControl1.MapAlias
            ' Put state manager into HttpSession, so we could get it later on from different class and requests.
            MapInfo.WebControls.StateManager.PutStateManagerInSession(stateManager)

            Me.InitState()
        End If
        MapInfo.WebControls.StateManager.GetStateManagerFromSession.RestoreState()

        PrepareData()
    End Sub
    Private Sub Page_UnLoad(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
        MapInfo.WebControls.StateManager.GetStateManagerFromSession().SaveState()
    End Sub
    Private Sub InitState()
        Dim myMap As MapInfo.Mapping.Map = Me.GetMapObj()

        '//***************************************************************************//
        '//*   Store and Restore the original state of the map.                      *//
        '//*   Store   - if no one puts the map into HttpSessionState yet,           *//
        '//*             the map is clean, store it.                                 *//
        '//*   Restore - if there is the map stored in HttpSessionState,             *//
        '//*             deserialize it and apply the state of the map automatically *//
        '//***************************************************************************//
        Dim originalMap As String = "original_map"
        If Not Me.Application(originalMap) Is Nothing Then
            Dim bytes As Byte() = CType(Me.Application.Item(originalMap), Byte())
            ' This step will deserialize myMap object back and all original states will be put back to 
            ' myMap if myMap has same alias name as the one stored in HttpApplicationState.
            Dim obj As Object = MapInfo.WebControls.ManualSerializer.ObjectFromBinaryStream(bytes)
        Else
            ' Set the initial zoom and center for the map
            myMap.Zoom = New MapInfo.Geometry.Distance(25000, MapInfo.Geometry.DistanceUnit.Mile)
            myMap.Center = New MapInfo.Geometry.DPoint(27775.805792979896, -147481.33999999985)
            ' adjust the map.Size to mapcontrol's size
            myMap.Size = New Size(CType(Me.MapControl1.Width.Value, Integer), CType(Me.MapControl1.Height.Value, Integer))

            ' Serialize myMap into a byte[] and store the original state of the map if no one stores it in HttpApplicationState yet.
            Me.Application.Item(originalMap) = MapInfo.WebControls.ManualSerializer.BinaryStreamFromObject(myMap)
        End If

        ' Create a GroupLayer to hold temp created label layer and object theme layer.
        ' We are going to put this group layer into HttpSessionState, and it will get restored 
        ' when requets come in with the same asp.net sessionID.
        If (Not myMap.Layers.Item(SampleConstants.GroupLayerAlias) Is Nothing) Then
            myMap.Layers.Remove(SampleConstants.GroupLayerAlias)
        End If
        ' put the GroupLayer on the top of Layers collection, so contents within it could get displayed.
        myMap.Layers.InsertGroup(0, "grouplayer", SampleConstants.GroupLayerAlias)
    End Sub

    Private Sub PrepareData()
        Dim mdbTable As MapInfo.Data.Table = MapInfo.Engine.Session.Current.Catalog.Item(SampleConstants.EWorldAlias)
        Dim worldTable As MapInfo.Data.Table = MapInfo.Engine.Session.Current.Catalog.Item(SampleConstants.ThemeTableAlias)
        ' worldTable is loaded by preloaded mapinfo workspace file specified in web.config.
        ' and MS Access table in this sample is loaded manually.
        ' we are not going to re-load it again once it got loaded because its content is not going to change in this sample.
        ' we will get performance gain if we use Pooled MapInfo Session.
        ' Note: It's better to put this MS Access table into pre-loaded workspace file, 
        '       so we don't need to do below code.
        '       We manually load this MS Access in this sample for demonstration purpose.
        If (mdbTable Is Nothing) Then
            Dim dataPath As String = HttpContext.Current.Server.MapPath(String.Format("/ThematicsWebVB_{0}_{1}", MapInfo.Engine.ProductInfo.MajorVersion, MapInfo.Engine.ProductInfo.MinorVersion))
            mdbTable = MapInfo.Engine.Session.Current.Catalog.OpenTable(System.IO.Path.Combine(dataPath, SampleConstants.EWorldTabFileName))

            Dim colAlias As String() = SampleConstants.BoundDataColumns
            ' DateBinding columns
            Dim col0 As MapInfo.Data.Column = MapInfo.Data.ColumnFactory.CreateDoubleColumn(colAlias(0))
            col0.ColumnExpression = (mdbTable.Alias & "." & colAlias(0))
            Dim col1 As MapInfo.Data.Column = MapInfo.Data.ColumnFactory.CreateIntColumn(colAlias(1))
            col1.ColumnExpression = (mdbTable.Alias & "." & colAlias(1))
            Dim col2 As MapInfo.Data.Column = MapInfo.Data.ColumnFactory.CreateIntColumn(colAlias(2))
            col2.ColumnExpression = (mdbTable.Alias & "." & colAlias(2))

            Dim cols As New MapInfo.Data.Columns
            cols.Add(col0)
            cols.Add(col1)
            cols.Add(col2)
            ' Databind access table data to existing worldTable.
            worldTable.AddColumns(cols, MapInfo.Data.BindType.DynamicCopy, mdbTable, SampleConstants.SouceMatchColumn, MapInfo.Data.Operator.Equal, SampleConstants.TableMatchColumn)
        End If
    End Sub

    Protected Overrides Sub OnPreRender(ByVal e As EventArgs)
        If Not MyBase.IsPostBack Then
            Dim colAlias As String() = SampleConstants.BoundDataColumns
            Dim list1 As New ArrayList
            ThemesAndModifiers.FillThemeNames(list1)

            Me.DropDownList1.Items.Clear()
            Me.DropDownList1.DataSource = list1
            Me.DropDownList1.DataBind()

            ' Prepare CheckBoxes, RadioButtons and note label contents.
            Me.CheckBoxList1.DataSource = colAlias
            Me.CheckBoxList1.DataBind()
            Me.CheckBoxList1.Visible = Me.IsCheckBoxListVisible(CType(list1.Item(0), String))

            Me.RadioButtonList1.DataSource = colAlias
            Me.RadioButtonList1.DataBind()
            Me.RadioButtonList1.Visible = Me.IsRadioButtonListVisible(CType(list1.Item(0), String))

            Me.Label2.Visible = Me.IsNoteLabelVisible(CType(list1.Item(0), String))
            Me.Label2.Text = "Please select two columns from below checkboxes."
        End If
    End Sub

    Private Function GetMapObj() As MapInfo.Mapping.Map
        Dim myMap As MapInfo.Mapping.Map = MapInfo.Engine.Session.Current.MapFactory.Item(Me.MapControl1.MapAlias)
        If (myMap Is Nothing) Then
            myMap = MapInfo.Engine.Session.Current.MapFactory.Item(0)
        End If
        Return myMap
    End Function

    Private Sub ApplyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApplyButton.Click
        If Me.ValidateThemeAndModifierParams(Me.DropDownList1.SelectedValue) Then
            If Me.Label2.Visible Then
                Me.Label2.BackColor = Color.LightGray
            End If
            Me.CreateThemeOrModifier(Me.DropDownList1.SelectedValue)
            Me.HandleLabelLayerVisibleStatus(Me.GetMapObj())
        Else
            Me.Label2.BackColor = Color.Red
        End If
    End Sub

    Private Sub DropDownList1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
        Me.PrepareThemeAndModifierOptions(Me.DropDownList1.SelectedValue)
        Me.RadioButtonList1.ClearSelection()
        Me.CheckBoxList1.ClearSelection()
        Me.CleanUp(MapInfo.Engine.Session.Current.MapFactory.Item(Me.MapControl1.MapAlias))
    End Sub

#Region "Methods to handle Themes, Modifiers and some other params to construct themes/modifiers."
    ' Remove all created themes, modifers and IMapLayers created by users.
    Private Sub CleanUp(ByVal myMap As MapInfo.Mapping.Map)
        Dim fLyr As MapInfo.Mapping.FeatureLayer = CType(myMap.Layers.Item(SampleConstants.ThemeLayerAlias), MapInfo.Mapping.FeatureLayer)

        ' Remove themes/modifiers one bye one instead of using Clear()
        Me.RemoveTheme(fLyr, Me.ConstructThemeAlias(ThemeAndModifierTypes.RangedTheme))
        Me.RemoveTheme(fLyr, Me.ConstructThemeAlias(ThemeAndModifierTypes.IndividualValueTheme))
        Me.RemoveTheme(fLyr, Me.ConstructThemeAlias(ThemeAndModifierTypes.DotDensityTheme))
        Me.RemoveTheme(fLyr, Me.ConstructThemeAlias(ThemeAndModifierTypes.FeatureOverrideStyleModifier))
        Dim groupLyr As MapInfo.Mapping.GroupLayer = GetTheGroupLayer()
        If (Not groupLyr Is Nothing) Then
            groupLyr.Clear()
        End If
    End Sub

    ' Remove a Layer object from Map.Layers collection.
    Private Sub RemoveLayer(ByVal lyrs As MapInfo.Mapping.Layers, ByVal layerAlias As String)
        If (Not lyrs.Item(layerAlias) Is Nothing) Then
            lyrs.Remove(layerAlias)
        End If
    End Sub

    ' Remove Themes from a FeatureLayer.
    Private Sub RemoveTheme(ByVal lyr As MapInfo.Mapping.FeatureLayer, ByVal themeAlias As String)
        If (Not lyr.Modifiers.Item(themeAlias) Is Nothing) Then
            lyr.Modifiers.Remove(themeAlias)
        End If
    End Sub

    Private Function ConstructThemeAlias(ByVal themeType As ThemeAndModifierTypes) As String
        Return (themeType.ToString & "_alias")
    End Function

    ' Disable other LabelLayers if we demo a new LabelLayer with theme or modifier sample.
    ' so User could see theme/modifier label layer clearly.
    Public Shared Sub HandleLabelLayerVisibleStatus(ByVal map As MapInfo.Mapping.Map)
        If (map Is Nothing) Then Return
        Dim index As Integer
        For index = 0 To map.Layers.Count - 1
            Dim lyr As MapInfo.Mapping.IMapLayer = map.Layers.Item(index)
            If TypeOf lyr Is MapInfo.Mapping.LabelLayer Then
                Dim ll As MapInfo.Mapping.LabelLayer = CType(lyr, MapInfo.Mapping.LabelLayer)
                If ((Not map.Layers.Item(SampleConstants.NewLabelLayerAlias) Is Nothing) AndAlso (Not ll.Alias Is SampleConstants.NewLabelLayerAlias)) Then
                    ll.Enabled = False
                Else
                    ll.Enabled = True
                End If
            End If
        Next index
    End Sub

    ' This is for theme or modifier which only requires one expression.
    Private Function GetThemeOrModifierExpression() As String
        Return Me.RadioButtonList1.SelectedValue
    End Function

    ' This is for Pie, Bar themes.
    Private Function GetThemeOrModifierExpressions() As String()
        Dim cols As New ArrayList(2)
        Dim index As Integer
        For index = 0 To Me.CheckBoxList1.Items.Count - 1
            If Me.CheckBoxList1.Items.Item(index).Selected Then
                cols.Add(Me.CheckBoxList1.Items.Item(index).Value)
            End If
        Next index
        Return CType(cols.ToArray(GetType(String)), String())
    End Function

    ' Validate user's inputs.
    Private Function ValidateThemeAndModifierParams(ByVal themeName As String) As Boolean
        Select Case ThemesAndModifiers.GetThemeAndModifierTypesByName(themeName)
            Case ThemeAndModifierTypes.RangedTheme, ThemeAndModifierTypes.IndividualValueTheme, ThemeAndModifierTypes.DotDensityTheme, ThemeAndModifierTypes.GraduatedSymbolTheme, ThemeAndModifierTypes.Label_IndividualValueLabelTheme, ThemeAndModifierTypes.Label_OverrideLabelModifier, ThemeAndModifierTypes.Label_RangedLabelTheme
                ' Need to select one column, otherwise we can not proceed it.
                If (Me.RadioButtonList1.SelectedItem Is Nothing) Then
                    Return False
                End If
                Return True
            Case ThemeAndModifierTypes.BarTheme, ThemeAndModifierTypes.PieTheme
                ' Need to select two columns.
                Dim selectedItemsSum As Integer = 0
                Dim index As Integer
                For index = 0 To Me.CheckBoxList1.Items.Count - 1
                    If Me.CheckBoxList1.Items.Item(index).Selected Then
                        selectedItemsSum += 1
                    End If
                Next index
                ' this is required for customer to select 2 cols.
                If (selectedItemsSum <> 2) Then
                    Return False
                End If
                Return True
            Case ThemeAndModifierTypes.FeatureOverrideStyleModifier
                Return True
        End Select
        Return False
    End Function

    ' Create all MapXtreme.Net themes and modifiers for the bound data
    ' and add them into the corresponding Map object.
    Private Sub CreateThemeOrModifier(ByVal themeName As String)
        Dim map As MapInfo.Mapping.Map = Me.GetMapObj
        If (Not map Is Nothing) Then
            ' Clean up all temp themes, modifiers and IMapLayer from the Map object.
            Me.CleanUp(map)
            Dim fLyr As MapInfo.Mapping.FeatureLayer = CType(map.Layers.Item(SampleConstants.ThemeLayerAlias), MapInfo.Mapping.FeatureLayer)
            Dim type As ThemeAndModifierTypes = ThemesAndModifiers.GetThemeAndModifierTypesByName(themeName)
            Dim themeAlias As String = Me.ConstructThemeAlias(type)
            Select Case type
                Case ThemeAndModifierTypes.RangedTheme
                    Dim rt As New MapInfo.Mapping.Thematics.RangedTheme(fLyr, Me.GetThemeOrModifierExpression, themeAlias, 5, MapInfo.Mapping.Thematics.DistributionMethod.EqualCountPerRange)
                    fLyr.Modifiers.Append(rt)
                    Return
                Case ThemeAndModifierTypes.IndividualValueTheme
                    Dim ivt As New MapInfo.Mapping.Thematics.IndividualValueTheme(fLyr, Me.GetThemeOrModifierExpression, themeAlias)
                    fLyr.Modifiers.Append(ivt)
                    Return
                Case ThemeAndModifierTypes.FeatureOverrideStyleModifier
                    Dim fosm As New MapInfo.Mapping.FeatureOverrideStyleModifier("OverrideTheme", themeAlias)
                    fLyr.Modifiers.Append(fosm)
                    Return
                Case ThemeAndModifierTypes.DotDensityTheme
                    Dim ddt As New MapInfo.Mapping.Thematics.DotDensityTheme(fLyr, Me.GetThemeOrModifierExpression, themeAlias, Color.Purple, MapInfo.Mapping.Thematics.DotDensitySize.Large)
                    ddt.ValuePerDot = 2000000
                    fLyr.Modifiers.Append(ddt)
                    Return
                Case ThemeAndModifierTypes.BarTheme
                    Dim bt As New MapInfo.Mapping.Thematics.BarTheme(map, fLyr.Table, Me.GetThemeOrModifierExpressions)
                    bt.DataValueAtSize = 10000000
                    bt.Size = New MapInfo.Engine.PaperSize(0.1, 0.1, MapInfo.Geometry.PaperUnit.Inch)
                    bt.Width = New MapInfo.Engine.PaperSize(0.1, MapInfo.Geometry.PaperUnit.Inch)
                    Dim btOtl As New MapInfo.Mapping.ObjectThemeLayer("BarTheme", themeAlias, bt)
                    GetTheGroupLayer().Insert(0, btOtl)
                    Return
                Case ThemeAndModifierTypes.PieTheme
                    Dim pt As New MapInfo.Mapping.Thematics.PieTheme(map, fLyr.Table, Me.GetThemeOrModifierExpressions)
                    Dim ptOtl As New MapInfo.Mapping.ObjectThemeLayer("PieTheme", themeAlias, pt)
                    GetTheGroupLayer().Insert(0, ptOtl)
                    Return
                Case ThemeAndModifierTypes.GraduatedSymbolTheme
                    Dim gst As New MapInfo.Mapping.Thematics.GraduatedSymbolTheme(fLyr.Table, Me.GetThemeOrModifierExpression)
                    Dim gstOtl As New MapInfo.Mapping.ObjectThemeLayer("GraduatedSymbolTheme", themeAlias, gst)
                    GetTheGroupLayer().Insert(0, gstOtl)
                    Return
                Case ThemeAndModifierTypes.Label_IndividualValueLabelTheme
                    Dim ivlt As New MapInfo.Mapping.Thematics.IndividualValueLabelTheme(fLyr.Table, Me.GetThemeOrModifierExpression, themeAlias)
                    Me.CreateLabelLayer(map, fLyr.Table, Me.GetThemeOrModifierExpression).Sources.Item(0).Modifiers.Append(ivlt)
                    Return
                Case ThemeAndModifierTypes.Label_OverrideLabelModifier
                    Dim olm As New MapInfo.Mapping.OverrideLabelModifier(themeAlias, "OverrideLabelModifier")
                    Dim font As New MapInfo.Styles.Font("Arial", 24, Color.Red, Color.Yellow, MapInfo.Styles.FontFaceStyle.Italic, _
                        MapInfo.Styles.FontWeight.Bold, MapInfo.Styles.TextEffect.Halo, MapInfo.Styles.TextDecoration.All, _
                        MapInfo.Styles.TextCase.AllCaps, True, True)

                    font.Attributes = MapInfo.Styles.StyleAttributes.FontAttributes.All

                    olm.Properties.Style = New MapInfo.Styles.TextStyle(font)
                    olm.Properties.Caption = GetThemeOrModifierExpression()
                    Me.CreateLabelLayer(map, fLyr.Table, Me.GetThemeOrModifierExpression).Sources.Item(0).Modifiers.Append(olm)
                    Return
                Case ThemeAndModifierTypes.Label_RangedLabelTheme
                    Dim rlt As New MapInfo.Mapping.Thematics.RangedLabelTheme(fLyr.Table, Me.GetThemeOrModifierExpression, themeAlias, 5, MapInfo.Mapping.Thematics.DistributionMethod.EqualCountPerRange)
                    Me.CreateLabelLayer(map, fLyr.Table, Me.GetThemeOrModifierExpression).Sources.Item(0).Modifiers.Append(rlt)
                    Return
            End Select
        End If
    End Sub

    Private Function GetTheGroupLayer() As MapInfo.Mapping.GroupLayer
        Dim myMap As MapInfo.Mapping.Map = Me.GetMapObj()
        If (myMap.Layers.Item(SampleConstants.GroupLayerAlias) Is Nothing) Then
            myMap.Layers.InsertGroup(0, "grouplayer", SampleConstants.GroupLayerAlias)
        End If
        Return CType(myMap.Layers.Item(SampleConstants.GroupLayerAlias), MapInfo.Mapping.GroupLayer)
    End Function
    ' Create a LabelLayer and add it into the Group Layer collection with index 0.
    Private Function CreateLabelLayer(ByVal myMap As MapInfo.Mapping.Map, ByVal table As MapInfo.Data.Table, ByVal caption As String) As MapInfo.Mapping.LabelLayer
        Dim ll As New MapInfo.Mapping.LabelLayer("Label Layer for bound data", SampleConstants.NewLabelLayerAlias)
        '' Insert this LabelLayer into the GroupLayer
        GetTheGroupLayer().Insert(0, ll)

        ' Create a LabelSource
        Dim ls As New MapInfo.Mapping.LabelSource(table)
        ls.DefaultLabelProperties.Caption = caption
        ll.Sources.Append(ls)
        Return ll
    End Function

    ' Decide what kind of infos shoud be displayed on the page.
    Private Sub PrepareThemeAndModifierOptions(ByVal themeName As String)
        Me.RadioButtonList1.Visible = Me.IsRadioButtonListVisible(themeName)
        Me.CheckBoxList1.Visible = Me.IsCheckBoxListVisible(themeName)
        Me.Label2.BackColor = Color.LightGray
        Me.Label2.Visible = Me.IsNoteLabelVisible(themeName)
    End Sub

    ' Decide if the CheckBoxList should be visible or not.
    Private Function IsCheckBoxListVisible(ByVal themeName As String) As Boolean
        Select Case ThemesAndModifiers.GetThemeAndModifierTypesByName(themeName)
            Case ThemeAndModifierTypes.BarTheme, ThemeAndModifierTypes.PieTheme
                Return True
        End Select
        Return False
    End Function

    ' Decide if the RadioButtonListVisible should be visible or not.
    Private Function IsRadioButtonListVisible(ByVal themeName As String) As Boolean
        Select Case ThemesAndModifiers.GetThemeAndModifierTypesByName(themeName)
            Case ThemeAndModifierTypes.RangedTheme, ThemeAndModifierTypes.IndividualValueTheme, ThemeAndModifierTypes.DotDensityTheme, ThemeAndModifierTypes.GraduatedSymbolTheme, ThemeAndModifierTypes.Label_IndividualValueLabelTheme, ThemeAndModifierTypes.Label_OverrideLabelModifier, ThemeAndModifierTypes.Label_RangedLabelTheme
                Return True
        End Select
        Return False
    End Function

    ' Decide if the note message should be displayed or not.
    Private Function IsNoteLabelVisible(ByVal themeName As String) As Boolean
        Select Case ThemesAndModifiers.GetThemeAndModifierTypesByName(themeName)
            Case ThemeAndModifierTypes.BarTheme, ThemeAndModifierTypes.PieTheme
                Return True
        End Select
        Return False
    End Function
#End Region

End Class
