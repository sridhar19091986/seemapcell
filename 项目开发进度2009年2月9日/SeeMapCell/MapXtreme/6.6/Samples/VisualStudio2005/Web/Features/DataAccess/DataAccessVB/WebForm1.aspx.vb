Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary


Namespace DataAccessWeb


Partial Class WebForm1
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub


    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' The first time in
        If Me.Session.IsNewSession Then
            '//******************************************************************************//
            '//*   You need to follow below lines in your own application.                  *//
            '//*   You don't need this state manager if the "MapInfo.Engine.Session.State" *//
            '//*   in the web.config is not set to "Manual"                                 *//
            '//******************************************************************************//
                If AppStateManager.IsManualState() Then

                    Dim stateManager As New AppStateManager
                    '// tell the state manager which map alias you want to use.
                    '// You could also add your own key/value pairs, the value should be serializable.
                    stateManager.ParamsDictionary.Item(AppStateManager.ActiveMapAliasKey) = Me.MapControl1.MapAlias
                    '// Put state manager into HttpSession, so we could get it later on.
                    AppStateManager.PutStateManagerInSession(stateManager)
                End If

            ' Initialize map setting
            Me.InitState()
            ' WarningLabel is invisible in the init state
            WarningLabel.Visible = False
        End If

        ' Restore state.
            If AppStateManager.IsManualState Then
                MapInfo.WebControls.StateManager.GetStateManagerFromSession().RestoreState()
            End If

        If Not MyBase.IsPostBack Then
            ' DataBind named connection names to RadioButtonList
                Me.CheckBoxList1.DataSource = MapInfo.Engine.Session.Current.Catalog.NamedConnections.Keys
                Me.CheckBoxList1.DataBind()
            ' Populate aliases of opened tables to Repeater web control
            Me.BindOpenedTablesAliasToRepeater()
        End If

    End Sub

    Private Sub Page_UnLoad(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
            If AppStateManager.IsManualState Then
                MapInfo.WebControls.StateManager.GetStateManagerFromSession().SaveState()
            End If
    End Sub

    Private Sub InitState()
        Dim myMap As MapInfo.Mapping.Map = Me.GetMapObj

        '// We need to put original state of applicatin into HttpApplicationState.
        If Application.Get("DataAccessWeb") Is Nothing Then
            Dim iEnum As IEnumerator = MapInfo.Engine.Session.Current.MapFactory.GetEnumerator
            Do While iEnum.MoveNext
                Dim tempMap As MapInfo.Mapping.Map = CType(iEnum.Current, MapInfo.Mapping.Map)
                Dim mapBits As Byte() = MapInfo.WebControls.ManualSerializer.BinaryStreamFromObject(tempMap)
                MyBase.Application.Add(tempMap.Alias, mapBits)
            Loop

            '// Load Named connections into catalog.
            If (MapInfo.Engine.Session.Current.Catalog.NamedConnections.Count = 0) Then
                    Dim path As String = HttpContext.Current.Server.MapPath("")
                Dim fileName As String = System.IO.Path.Combine(path, "namedconnection.xml")
                MapInfo.Engine.Session.Current.Catalog.NamedConnections.Load(fileName)
            End If

            '// Put Catalog into a byte[] and keep it in HttpApplicationState
            Dim catalogBits As Byte() = MapInfo.WebControls.ManualSerializer.BinaryStreamFromObject(MapInfo.Engine.Session.Current.Catalog)
            MyBase.Application.Add("Catalog", catalogBits)

            '// Put a marker key/value.
            MyBase.Application.Add("DataAccessWeb", "Here")
        Else
            '// Apply original Catalog state.
            Dim obj As Object = MyBase.Application.Get("Catalog")
            If (Not obj Is Nothing) Then
                Dim tempObj As Object = MapInfo.WebControls.ManualSerializer.ObjectFromBinaryStream(CType(obj, Byte()))
            End If
            obj = MyBase.Application.Get(Me.MapControl1.MapAlias)
            If (Not obj Is Nothing) Then
                Dim tempObj As Object = MapInfo.WebControls.ManualSerializer.ObjectFromBinaryStream(CType(obj, Byte()))
            End If
        End If

        ' Set the initial state of the map
        ' This step is needed because you may get a dirty map from mapinfo Session.Current which is retrived from session pool.
        myMap.Zoom = New MapInfo.Geometry.Distance(25000, MapInfo.Geometry.DistanceUnit.Mile)
        myMap.Center = New MapInfo.Geometry.DPoint(27775.805792979896, -147481.33999999985)

        myMap.Size = New Size(CType(Me.MapControl1.Width.Value, Integer), CType(Me.MapControl1.Height.Value, Integer))

    End Sub

    Private Function GetMapObj() As MapInfo.Mapping.Map
        ' Get the map
        Dim myMap As MapInfo.Mapping.Map = MapInfo.Engine.Session.Current.MapFactory.Item(Me.MapControl1.MapAlias)
        If (myMap Is Nothing) Then
            myMap = MapInfo.Engine.Session.Current.MapFactory.Item(0)
        End If
        Return myMap
    End Function


        Public Sub ApplyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApplyButton.Click
            If (Not Me.SelectClauseTextBox.Text Is Nothing) Then
                Me.SetDataGrid(Me.DataGrid1, Me.SelectClauseTextBox.Text.Trim, False)
                Me.DataGrid1.DataBind()
            End If
        End Sub
    Private Sub SetDataGrid(ByVal dataGrid As DataGrid, ByVal commandText As String, ByVal bShowSchema As Boolean)
        Dim miConnection As New MapInfo.Data.MIConnection
        miConnection.Open()
        Dim miCommand As MapInfo.Data.MICommand = miConnection.CreateCommand
        miCommand.CommandText = commandText
        Dim miReader As MapInfo.Data.MIDataReader = Nothing
        Try
            miReader = miCommand.ExecuteReader()
            If bShowSchema Then
                dataGrid.DataSource = miReader.GetSchemaTable
            Else
                Dim dt As New DataTable("Data")
                Dim index As Integer
                For index = 0 To miReader.FieldCount - 1
                    Dim column1 As DataColumn = dt.Columns.Add(miReader.GetName(index))
                Next index
                Do While miReader.Read
                    Dim dr As DataRow = dt.NewRow
                    For index = 0 To miReader.FieldCount - 1
                        dr.Item(index) = miReader.GetValue(index)
                    Next index
                    dt.Rows.Add(dr)
                Loop
                dataGrid.DataSource = dt
            End If
        Catch exception1 As Exception
            Me.SelectClauseTextBox.Text = (Me.SelectClauseTextBox.Text & " ***Wrong select clause, try it.***")
        Finally
            If (Not miReader Is Nothing) Then
                miReader.Close()
            End If
        End Try
    End Sub

    Private Sub BindOpenedTablesAliasToRepeater()
        Dim aliasList As New ArrayList
        Dim iEnum As MapInfo.Data.ITableEnumerator = MapInfo.Engine.Session.Current.Catalog.EnumerateTables
        Do While iEnum.MoveNext
            aliasList.Add(iEnum.Current.Alias)
        Loop
        Me.Repeater1.DataSource = aliasList
        Me.Repeater1.DataBind()
    End Sub

    Private Function OpenTable(ByVal connectionName As String, ByVal tableName As String) As MapInfo.Data.Table
        Dim nci As MapInfo.Data.NamedConnectionInfo = MapInfo.Engine.Session.Current.Catalog.NamedConnections.Get(connectionName)
        If (nci Is Nothing) Then
            Return Nothing
        End If
        If nci.DBType.ToLower.Equals("file") Then
            Return Me.OpenNativeTable(nci, tableName)
        End If
        Return Me.OpenDataBaseTable(nci, tableName)
    End Function

    Private Function OpenNativeTable(ByVal nci As MapInfo.Data.NamedConnectionInfo, ByVal tableName As String) As MapInfo.Data.Table
        Dim fileName As String = tableName
        Dim aliasName As String = (tableName & "_" & nci.Name)
        Dim table As MapInfo.Data.Table = MapInfo.Engine.Session.Current.Catalog.GetTable(aliasName)
        If (table Is Nothing) Then

            If (tableName.ToLower.IndexOf(".tab") <= 0) Then
                fileName = (tableName & ".tab")
            End If
            table = MapInfo.Engine.Session.Current.Catalog.OpenTable(nci.Name, aliasName, fileName)
        End If
        Return table
    End Function

    Private Function OpenDataBaseTable(ByVal nci As MapInfo.Data.NamedConnectionInfo, ByVal dbTableName As String) As MapInfo.Data.Table
        Dim tableAlias As String = (dbTableName & "_" & nci.Name.Replace(" ", "_").Replace(".", "_"))
        Dim table As MapInfo.Data.Table = MapInfo.Engine.Session.Current.Catalog.GetTable(tableAlias)
        ' we need to create a new table if there is no table in the catalog.
        If (table Is Nothing) Then
            Dim tis As New MapInfo.Data.TableInfoServer(("tableInfoServer_" & dbTableName))
            tis.ConnectString = nci.ConnectionString
            tis.Query = ("select * from " & dbTableName)
            tis.Toolkit = Me.GetServerToolkit(nci)
            tis.CacheSettings = New MapInfo.Data.CacheParameters(MapInfo.Data.CacheOption.On)
            tis.Alias = tableAlias
            table = MapInfo.Engine.Session.Current.Catalog.OpenTable(nci.Name, tis)
        End If
        Return table
    End Function

    Private Function GetServerToolkit(ByVal nci As MapInfo.Data.NamedConnectionInfo) As MapInfo.Data.ServerToolkit
        Select Case nci.ConnectionMethod
            Case MapInfo.Data.ConnectionMethod.Odbc
                Return MapInfo.Data.ServerToolkit.Odbc
            Case MapInfo.Data.ConnectionMethod.OracleOci
                Return MapInfo.Data.ServerToolkit.Oci
        End Select
        Throw New ArgumentException("ConnectionMethod should only be Odbc or OracleOci!", "nci")
    End Function

        Public Sub OpenTableButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenTableButton.Click
            If (Me.CheckBoxList1.SelectedValue Is Nothing) Then Return
            WarningLabel.Visible = False
            If ((Not Me.OpenTableTextBox.Text Is Nothing) AndAlso (Me.OpenTableTextBox.Text.Trim.Length <> 0)) Then
                Dim tableName As String = Me.OpenTableTextBox.Text.Trim
                Dim table As MapInfo.Data.Table = Nothing
                Try

                    table = Me.OpenTable(Me.CheckBoxList1.SelectedValue, tableName)
                    If (Not table Is Nothing) Then
                        Dim lyrAlias As String = ("alias_flyr_" & table.Alias)
                        Dim myMap As MapInfo.Mapping.Map = Me.GetMapObj
                        If (Not myMap Is Nothing) Then
                            If (Not myMap.Layers.Item(lyrAlias) Is Nothing) Then
                                myMap.Layers.Remove(lyrAlias)
                            End If
                            Dim fLyr As New MapInfo.Mapping.FeatureLayer(table, ("LayerName_" & tableName), lyrAlias)
                            myMap.Layers.Insert(0, fLyr)
                            ' Need to rebind again since a new table got opened.
                            Me.BindOpenedTablesAliasToRepeater()
                        End If
                    Else
                        WarningLabel.Visible = True
                    End If
                Catch exception1 As Exception
                    WarningLabel.Visible = True
                    If (Not table Is Nothing) Then
                        table.Close()
                    End If
                End Try
            End If
        End Sub
End Class

End Namespace
