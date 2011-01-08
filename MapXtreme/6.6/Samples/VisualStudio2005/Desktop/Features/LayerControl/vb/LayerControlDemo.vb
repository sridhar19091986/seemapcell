
Imports System.IO
Imports MapInfo.Data
Imports MapInfo.Geometry
Imports MapInfo.Mapping
Imports MapInfo.Engine
Imports MapInfo.Windows.Controls
Imports MapInfo.Windows.Dialogs

Public Class LayerControlDemoForm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        ' For this application, we'll reset Resize Method, because
        ' the form is resizable, and the app is more responsive if
        ' it doesn't redraw the app repeatedly as the user resizes. 
        MapControl1.Map.ResizeMethod = ResizeMethod.PreserveScale

        ' Now assign the map, which will populate the nodes of
        ' the layer tree.  
        LayerControl1.Map = MapControl1.Map

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

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents MapControl1 As MapInfo.Windows.Controls.MapControl
    Friend WithEvents LayerControl1 As MapInfo.Windows.Controls.LayerControl
    Friend WithEvents checkBoxShowAddButton As System.Windows.Forms.CheckBox
    Friend WithEvents checkBoxToolTips As System.Windows.Forms.CheckBox
    Friend WithEvents checkBoxShowMapNode As System.Windows.Forms.CheckBox
    Friend WithEvents checkBoxConfirmationPrompt As System.Windows.Forms.CheckBox
    Friend WithEvents checkBoxShowContextMenu As System.Windows.Forms.CheckBox
    Friend WithEvents buttonCustomStyleTab As System.Windows.Forms.Button
    Friend WithEvents buttonAddMenuItem As System.Windows.Forms.Button
    Friend WithEvents buttonRemoveMapViewTab As System.Windows.Forms.Button
    Friend WithEvents buttonDisableMapViewTab As System.Windows.Forms.Button
    Friend WithEvents buttonDisableRemovals As System.Windows.Forms.Button
    Friend WithEvents buttonLabelLayers As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.MapControl1 = New MapInfo.Windows.Controls.MapControl
        Me.LayerControl1 = New MapInfo.Windows.Controls.LayerControl
        Me.checkBoxShowAddButton = New System.Windows.Forms.CheckBox
        Me.checkBoxToolTips = New System.Windows.Forms.CheckBox
        Me.checkBoxShowMapNode = New System.Windows.Forms.CheckBox
        Me.checkBoxConfirmationPrompt = New System.Windows.Forms.CheckBox
        Me.checkBoxShowContextMenu = New System.Windows.Forms.CheckBox
        Me.buttonCustomStyleTab = New System.Windows.Forms.Button
        Me.buttonAddMenuItem = New System.Windows.Forms.Button
        Me.buttonRemoveMapViewTab = New System.Windows.Forms.Button
        Me.buttonDisableMapViewTab = New System.Windows.Forms.Button
        Me.buttonDisableRemovals = New System.Windows.Forms.Button
        Me.buttonLabelLayers = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'MapControl1
        '
        Me.MapControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MapControl1.Location = New System.Drawing.Point(304, 8)
        Me.MapControl1.Name = "MapControl1"
        Me.MapControl1.Size = New System.Drawing.Size(352, 208)
        Me.MapControl1.TabIndex = 0
        Me.MapControl1.Text = "MapControl1"
        '
        'LayerControl1
        '
        Me.LayerControl1.AllowDragAndDrop = True
        Me.LayerControl1.AllowRenaming = True
        Me.LayerControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LayerControl1.AutomaticLabelRemoval = MapInfo.Windows.Controls.LayerControl.LabelRemoval.Prompt
        Me.LayerControl1.ConfirmLayerRemoval = True
        Me.LayerControl1.EditNameAfterInsertingLayer = True
        Me.LayerControl1.FeatureLayerImageType = MapInfo.Windows.Controls.LayerControl.ImageType.Sample
        Me.LayerControl1.ItemHeight = 20
        Me.LayerControl1.Location = New System.Drawing.Point(8, 8)
        Me.LayerControl1.Map = Nothing
        Me.LayerControl1.Name = "LayerControl1"
        Me.LayerControl1.OriginalMap = Nothing
        Me.LayerControl1.PointSampleMaximumPointSize = 18
        Me.LayerControl1.SelectedObject = Nothing
        Me.LayerControl1.SelectedTab = MapInfo.Windows.Controls.PropertiesCategory.Custom
        Me.LayerControl1.ShowContextMenu = True
        Me.LayerControl1.ShowMapNode = True
        Me.LayerControl1.Size = New System.Drawing.Size(288, 440)
        Me.LayerControl1.TabIndex = 1
        Me.LayerControl1.Tools = Nothing
        Me.LayerControl1.UpdateWhenCollectionChanges = True
        Me.LayerControl1.UpdateWhenMapViewChanges = True
        Me.LayerControl1.UpdateWhenNameChanges = True
        '
        'checkBoxShowAddButton
        '
        Me.checkBoxShowAddButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.checkBoxShowAddButton.Checked = True
        Me.checkBoxShowAddButton.CheckState = System.Windows.Forms.CheckState.Checked
        Me.checkBoxShowAddButton.Location = New System.Drawing.Point(304, 224)
        Me.checkBoxShowAddButton.Name = "checkBoxShowAddButton"
        Me.checkBoxShowAddButton.Size = New System.Drawing.Size(344, 24)
        Me.checkBoxShowAddButton.TabIndex = 2
        Me.checkBoxShowAddButton.Text = "Show the Add button on the LayerControl toolbar"
        '
        'checkBoxToolTips
        '
        Me.checkBoxToolTips.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.checkBoxToolTips.Checked = True
        Me.checkBoxToolTips.CheckState = System.Windows.Forms.CheckState.Checked
        Me.checkBoxToolTips.Location = New System.Drawing.Point(304, 248)
        Me.checkBoxToolTips.Name = "checkBoxToolTips"
        Me.checkBoxToolTips.Size = New System.Drawing.Size(344, 24)
        Me.checkBoxToolTips.TabIndex = 3
        Me.checkBoxToolTips.Text = "Show ToolTips over the layers tree"
        '
        'checkBoxShowMapNode
        '
        Me.checkBoxShowMapNode.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.checkBoxShowMapNode.Checked = True
        Me.checkBoxShowMapNode.CheckState = System.Windows.Forms.CheckState.Checked
        Me.checkBoxShowMapNode.Location = New System.Drawing.Point(304, 272)
        Me.checkBoxShowMapNode.Name = "checkBoxShowMapNode"
        Me.checkBoxShowMapNode.Size = New System.Drawing.Size(344, 24)
        Me.checkBoxShowMapNode.TabIndex = 3
        Me.checkBoxShowMapNode.Text = "Show a Map node at the root of the layers tree"
        '
        'checkBoxConfirmationPrompt
        '
        Me.checkBoxConfirmationPrompt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.checkBoxConfirmationPrompt.Checked = True
        Me.checkBoxConfirmationPrompt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.checkBoxConfirmationPrompt.Location = New System.Drawing.Point(304, 296)
        Me.checkBoxConfirmationPrompt.Name = "checkBoxConfirmationPrompt"
        Me.checkBoxConfirmationPrompt.Size = New System.Drawing.Size(344, 24)
        Me.checkBoxConfirmationPrompt.TabIndex = 3
        Me.checkBoxConfirmationPrompt.Text = "Display ""Do you want to remove..."" confirmation prompts"
        '
        'checkBoxShowContextMenu
        '
        Me.checkBoxShowContextMenu.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.checkBoxShowContextMenu.Checked = True
        Me.checkBoxShowContextMenu.CheckState = System.Windows.Forms.CheckState.Checked
        Me.checkBoxShowContextMenu.Location = New System.Drawing.Point(304, 320)
        Me.checkBoxShowContextMenu.Name = "checkBoxShowContextMenu"
        Me.checkBoxShowContextMenu.Size = New System.Drawing.Size(344, 24)
        Me.checkBoxShowContextMenu.TabIndex = 3
        Me.checkBoxShowContextMenu.Text = "Show context menu when user right-clicks on layers tree"
        '
        'buttonCustomStyleTab
        '
        Me.buttonCustomStyleTab.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.buttonCustomStyleTab.Location = New System.Drawing.Point(304, 424)
        Me.buttonCustomStyleTab.Name = "buttonCustomStyleTab"
        Me.buttonCustomStyleTab.Size = New System.Drawing.Size(168, 23)
        Me.buttonCustomStyleTab.TabIndex = 95
        Me.buttonCustomStyleTab.Text = "Add a custom Map Style tab..."
        '
        'buttonAddMenuItem
        '
        Me.buttonAddMenuItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.buttonAddMenuItem.Location = New System.Drawing.Point(480, 424)
        Me.buttonAddMenuItem.Name = "buttonAddMenuItem"
        Me.buttonAddMenuItem.Size = New System.Drawing.Size(176, 23)
        Me.buttonAddMenuItem.TabIndex = 96
        Me.buttonAddMenuItem.Text = "Add custom menu items..."
        '
        'buttonRemoveMapViewTab
        '
        Me.buttonRemoveMapViewTab.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.buttonRemoveMapViewTab.Location = New System.Drawing.Point(480, 392)
        Me.buttonRemoveMapViewTab.Name = "buttonRemoveMapViewTab"
        Me.buttonRemoveMapViewTab.Size = New System.Drawing.Size(176, 23)
        Me.buttonRemoveMapViewTab.TabIndex = 94
        Me.buttonRemoveMapViewTab.Text = "Remove the Map's View tab..."
        '
        'buttonDisableMapViewTab
        '
        Me.buttonDisableMapViewTab.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.buttonDisableMapViewTab.Location = New System.Drawing.Point(480, 360)
        Me.buttonDisableMapViewTab.Name = "buttonDisableMapViewTab"
        Me.buttonDisableMapViewTab.Size = New System.Drawing.Size(176, 23)
        Me.buttonDisableMapViewTab.TabIndex = 92
        Me.buttonDisableMapViewTab.Text = "Disable the Map's View tab..."
        '
        'buttonDisableRemovals
        '
        Me.buttonDisableRemovals.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.buttonDisableRemovals.Location = New System.Drawing.Point(304, 360)
        Me.buttonDisableRemovals.Name = "buttonDisableRemovals"
        Me.buttonDisableRemovals.Size = New System.Drawing.Size(168, 23)
        Me.buttonDisableRemovals.TabIndex = 91
        Me.buttonDisableRemovals.Text = "Disable layer removal..."
        '
        'buttonLabelLayers
        '
        Me.buttonLabelLayers.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.buttonLabelLayers.Location = New System.Drawing.Point(304, 392)
        Me.buttonLabelLayers.Name = "buttonLabelLayers"
        Me.buttonLabelLayers.Size = New System.Drawing.Size(168, 23)
        Me.buttonLabelLayers.TabIndex = 93
        Me.buttonLabelLayers.Text = "Simplify LabelLayer nodes..."
        '
        'LayerControlDemoForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(664, 454)
        Me.Controls.Add(Me.buttonCustomStyleTab)
        Me.Controls.Add(Me.buttonAddMenuItem)
        Me.Controls.Add(Me.buttonRemoveMapViewTab)
        Me.Controls.Add(Me.buttonDisableMapViewTab)
        Me.Controls.Add(Me.buttonDisableRemovals)
        Me.Controls.Add(Me.buttonLabelLayers)
        Me.Controls.Add(Me.checkBoxToolTips)
        Me.Controls.Add(Me.checkBoxShowAddButton)
        Me.Controls.Add(Me.MapControl1)
        Me.Controls.Add(Me.checkBoxShowMapNode)
        Me.Controls.Add(Me.checkBoxConfirmationPrompt)
        Me.Controls.Add(Me.checkBoxShowContextMenu)
        Me.Controls.Add(Me.LayerControl1)
        Me.MinimumSize = New System.Drawing.Size(660, 470)
        Me.Name = "LayerControlDemoForm"
        Me.Text = "Layer Control Demo"
        Me.ResumeLayout(False)

    End Sub

#End Region

    ' The method called when the user checks or clears the 
    ' "Show The Add Button" checkbox
    Private Sub checkBoxShowAddButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkBoxShowAddButton.CheckedChanged
        ' Show or hide the Layer Control's Add button, 
        ' based on whether the checkbox is currently checked. 
        ShowAddButton(checkBoxShowAddButton.Checked)
    End Sub
    ' The method called when the user checks or clears the 
    ' Show ToolTips checkbox. 
    Private Sub checkBoxToolTips_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkBoxToolTips.CheckedChanged
        LayerControl1.ToolTip.Active = checkBoxToolTips.Checked
    End Sub
    ' The method called when the user checks or clears the 
    ' Show Map Node checkbox. 
    Private Sub checkBoxShowMapNode_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkBoxShowMapNode.CheckedChanged
        LayerControl1.ShowMapNode = checkBoxShowMapNode.Checked
        RefreshLayerTree()
    End Sub
    ' The method called when the user checks or clears the 
    ' Show Confirmation Prompts checkbox. 
    Private Sub checkBoxConfirmationPrompt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkBoxConfirmationPrompt.CheckedChanged
        ' Set the LayerControl object's ConfirmLayerRemoval property, 
        ' which can be used to globally suppress all confirmation 
        ' prompts that are displayed when the user removes a node
        ' from the layer tree.  
        LayerControl1.ConfirmLayerRemoval = checkBoxConfirmationPrompt.Checked

        ' NOTE: You can configure LayerControl so that it displays a 
        ' confirmation prompt for some types of layers (e.g. 
        ' "Do you want to remove this layer?") but does not display 
        ' a confirmation prompt for other types of layers (e.g. 
        ' you might not want to display a confirmation prompt when the
        ' user removes a Style Override node).  To control confirmation
        ' prompts separately for different types of layers, obtain 
        ' the appropriate ILayerNodeHelper object and set its properties.
    End Sub
    ' The method called when the user checks or clears 
    ' the Show Context Menu checkbox
    Private Sub checkBoxShowContextMenu_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkBoxShowContextMenu.CheckedChanged
        LayerControl1.ShowContextMenu = checkBoxShowContextMenu.Checked
    End Sub
    ' The method called when the user clicks the Disable Layer Removal button
    Private Sub buttonDisableRemovals_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonDisableRemovals.Click
        Dim prompt As String = "This button prevents the user from removing FeatureLayer, GroupLayer, or LabelLayer nodes.  It does not prevent the user from removing theme nodes or LabelSource nodes."

        If ConfirmCustomization(prompt) Then
            RestrictLayerRemoval()
            RefreshLayerTree()
            buttonDisableRemovals.Enabled = False
        End If
    End Sub
    ' The method called when the user clicks Disable the Map's View Tab.
    Private Sub buttonDisableMapViewTab_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonDisableMapViewTab.Click
        Dim prompt As String = "This button disables the options on the View tab, which appears when the user selects the Map node."
        If ConfirmCustomization(prompt) Then
            DisableMapViewTab()
            buttonDisableMapViewTab.Enabled = False
        End If
    End Sub
    ' The method called when the user clicks Simplify LabelLayer Nodes.
    Private Sub buttonLabelLayers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonLabelLayers.Click
        Dim prompt As String = "This button hides all LabelSource nodes, and changes the icon that appears next to LabelLayer nodes."
        If ConfirmCustomization(prompt) Then
            CustomizeLabelLayerNodes()
            RefreshLayerTree()
            buttonLabelLayers.Enabled = False
        End If
    End Sub
    ' The method called when the user clicks Remove the Map's View Tab. 
    Private Sub buttonRemoveMapViewTab_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonRemoveMapViewTab.Click
        Dim prompt As String = "This button removes the View tab, so that the TabControl does not contain a View tab when the user chooses the Map node."
        If ConfirmCustomization(prompt) Then
            RemoveMapViewTab()
            RefreshLayerTree()
            buttonRemoveMapViewTab.Enabled = False
            ' Since we have removed the View tab, we must now disable 
            ' the button on the demo form that allows you to make 
            ' other changes to the View tab
            buttonDisableMapViewTab.Enabled = False
        End If
    End Sub
    ' The method called when the user clicks Add Custom Map Style Tab. 
    Private Sub buttonCustomStyleTab_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCustomStyleTab.Click
        Dim prompt As String = "This button adds a custom 'Style' tab that allows the user to select the Map node, then choose a map background color."
        If ConfirmCustomization(prompt) Then
            AddCustomMapStyleTab()
            RefreshLayerTree()
            buttonCustomStyleTab.Enabled = False
        End If
    End Sub
    ' The method called when the user clicks Add Custom Menuitems.
    Private Sub buttonAddMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonAddMenuItem.Click
        Dim prompt As String = "This button adds custom menu items to right-click menus: a 'Choose Coordinate System' menu item (on the Map node's menu); a 'Find Labels' menu item (on each FeatureLayer node's menu); and a 'Find Layers' menu item (on each LabelSource node's menu)."
        If ConfirmCustomization(prompt) Then
            ' Add the custom item to the Map node's menu 
            AddChooseCoordSysMenuitem()

            ' Add custom items to the FeatureLayer and LabelSource menus
            Dim lce As New LayerControlEnhancer
            lce.LayerControl = LayerControl1
            lce.AddLayerToLabelEnhancement()

            buttonAddMenuItem.Enabled = False
        End If
    End Sub

    Private Sub LayerControlDemoForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set table search path to value sampledatasearch registry key
        ' if not found, then just use the app's current directory
        Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\MapInfo\MapXtreme\6.6")
        Dim s As String = CType(key.GetValue("SampleDataSearchPath"), String)
        If s = Nothing Then
            s = Environment.CurrentDirectory
        End If
        If s.EndsWith("\") = False Then
            s += "\"
        End If
        key.Close()

        Session.Current.TableSearchPath.Path = s

        ' open up world.gst using a maploader
        Try
            Dim gl As New MapGeosetLoader(s + "world.gst")
            MapControl1.Map.Load(gl)
        Catch fnf As FileNotFoundException
            MessageBox.Show("File not found " + fnf.Message)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub


    ' A method that re-assigns the LayerControl's Map property, which
    ' causes the LayerControl to regenerate the hierarchy of nodes 
    ' displayed in the layer tree.  (Most applications do not need to
    ' re-assign the Map property in this manner, because most 
    ' applications set LayerControl properties once, at the beginning;
    ' but this application lets you set LayerControl properties on the
    ' fly, which can create situations where the layer tree needs to
    ' be recreated.) 
    Private Sub RefreshLayerTree()
        layerControl1.Map = mapControl1.Map
    End Sub 'RefreshLayerTree


    ' A method that displays a confirmation dialog and determines whether
    ' the user chooses OK or Cancel. 
    Private Function ConfirmCustomization(ByVal prompt As String) As Boolean
        Dim result As DialogResult = DialogResult.Yes
        Dim caption As String = "Confirm Layer Control Customization"
        Dim buttons As MessageBoxButtons = MessageBoxButtons.YesNo
        result = MessageBox.Show(Me, prompt + " Do you want to proceed?", caption, buttons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)

        If result = DialogResult.Yes Then
            Return True
        End If
        Return False
    End Function 'ConfirmCustomization

    ' Method that places restrictions on what types of objects  
    ' the user is allowed to remove from the layer tree.  
    ' This example shows how to prevent the user from removing 
    ' FeatureLayer, GroupLayer, and LabelLayer nodes; the same 
    ' technique could be used to prohibit any or all types of 
    ' layers and themes from being removed by the user.  
    Private Sub RestrictLayerRemoval()
        ' Declare a LayerNodeHelper object, which has properties 
        ' that dictate the appearance and behavior of the specified
        ' type of layer.  
        Dim helper As ILayerNodeHelper

        ' Get the LayerNodeHelper that is currently being used to 
        ' dictate how FeatureLayer nodes behave in LayerControl.  
        helper = layerControl1.GetLayerTypeHelper(GetType(FeatureLayer))

        ' Reconfigure the helper to not allow the removal of FeatureLayers
        helper.AllowRemoval = False

        ' Now use the same technique to reconfigure other types of layers
        helper = layerControl1.GetLayerTypeHelper(GetType(GroupLayer))
        helper.AllowRemoval = False

        ' helper = layerControl1.GetLayerTypeHelper(GetType(LabelLayer))
        helper = LayerControl1.GetLayerTypeHelper(GetType(MapInfo.Mapping.LabelLayer))
        helper.AllowRemoval = False

        ' Apply the same technique to the Map node.  If you right-click
        ' the Map node, there is no Remove menuitem.  However, the Map
        ' node's context menu does have a "clear all" menuitem.  In the 
        ' case of the Map node, the helper's AllowRemoval property 
        ' controls whether the clear all menuitem is shown. 
        helper = layerControl1.GetLayerTypeHelper(GetType(Map))
        helper.AllowRemoval = False
    End Sub 'RestrictLayerRemoval


    ' Show or hide the Add button on the LayerControl's ToolBar. 
    ' Also show or hide the separator that separates the Add button
    ' from the Remove button. 
    Private Sub ShowAddButton(ByVal bShow As Boolean)
        Dim tb As ToolBar = layerControl1.ToolBar
        Dim spacer As ToolBarButton = tb.Buttons(1)
        Dim addButton As ToolBarButton = tb.Buttons(0)
        addButton.Visible = bShow
        spacer.Visible = bShow
    End Sub 'ShowAddButton


    ' Customize the appearance and behavior of all LabelLayer nodes
    Private Sub CustomizeLabelLayerNodes()
        ' Obtain the helper object that is being used to dictate the
        ' behavior of LabelLayer nodes in the layer tree.
        Dim labelLayerHelper As ILayerNodeHelper = LayerControl1.GetLayerTypeHelper(GetType(MapInfo.Mapping.LabelLayer))

        ' Reconfigure the helper to specify that it should NOT display 
        ' any "child" nodes.  By default, each LabelLayer does show a  
        ' child node for each LabelSource in the layer.  Setting 
        ' ShowChildren to false will cause LayerControl to not display
        ' any nodes for LabelSource objects.  This simplifies the layer
        ' tree, and prevents the user from seeing properties of 
        ' individual LabelSources (which might or might not be 
        ' appropriate for your application). 
        labelLayerHelper.ShowChildren = False

        ' Change the ToolTip text used for LabelLayer nodes
        labelLayerHelper.ToolTipText = "Labels for various map layers"

        ' Change which image is displayed for LabelLayer nodes.  
        ' In this example we will set all LabelLayer nodes to display
        ' with the icon that is ordinarly used for LabelSource nodes.
        Dim labelSourceHelper As ILayerNodeHelper = LayerControl1.GetLayerTypeHelper(GetType(MapInfo.Mapping.LabelSource))

        labelLayerHelper.Image = labelSourceHelper.Image
    End Sub 'CustomizeLabelLayerNodes


    ' Disable all options on the Map node's View tab, so that the user can
    ' still see the Map's zoom width, etc., but the user cannot type in 
    ' new values. 
    Private Sub DisableMapViewTab()
        ' Call a method on LayerControl class that returns the
        ' collection of PropertiesUserControl objects currently
        ' associated with the Map class.  This collection of 
        ' PropertiesUserControl objects is what populates the 
        ' TabControl whenever the Map node is selected. 
        Dim controlList As IList = LayerControl1.GetLayerTypeControls(GetType(MapInfo.Mapping.Map))

        ' Determine which of the controls in the collection is 
        ' the View control.  
        Dim obj As Object
        For Each obj In controlList
            ' Try to cast to a MapViewControl class, which might not work
            Dim mvc As MapViewControl = obj '
            If Not (mvc Is Nothing) Then
                ' The current control is the one shown on the View tab.
                ' Disable everything on the tab. 
                mvc.Enabled = False

                ' The MapViewControl has an Apply button; there is no
                ' point showing a button that is disabled.  
                ' So hide the button:  
                mvc.ShowApplyButton = False

                Exit For
            End If
        Next obj
    End Sub 'DisableMapViewTab


    ' Remove the Map node's View tab, so that when the user selects 
    ' the Map node, the TabControl will not contain a View tab.  
    ' The same technique could be used to remove any or all of the tabs
    ' associated with any type of node in the layers tree. 
    Private Sub RemoveMapViewTab()
        ' Call a method on LayerControl class that returns the
        ' collection of PropertiesUserControl objects currently
        ' associated with the Map class. 
        Dim controlList As IList = LayerControl1.GetLayerTypeControls(GetType(MapInfo.Mapping.Map))

        ' Determine which of the controls in the collection is 
        ' the View control.  
        Dim obj As Object
        For Each obj In controlList
            Dim mvc As MapViewControl = obj
            If Not (mvc Is Nothing) Then
                ' The current control is the one shown on the View tab.
                ' Remove it from the collection. 
                controlList.Remove(obj)
                Exit For
            End If
        Next obj
    End Sub 'RemoveMapViewTab


    ' Create a custom "Style" tab which will appear on the TabControl
    ' whenever the user selects the Map node at the root of the tree. 
    Private Sub AddCustomMapStyleTab()
        ' Instantiate a custom control, MapBackgroundControl, the 
        ' source code for which is provided as part of this project.  
        Dim bgControl As New MapBackgroundControl

        ' Call a method on LayerControl class that returns the
        ' collection of PropertiesUserControl objects currently
        ' associated with the Map class. 
        Dim controlList As IList = LayerControl1.GetLayerTypeControls(GetType(MapInfo.Mapping.Map))

        ' Add the custom control to the collection.  The next time the
        ' user selects the Map node at the root of the layer tree, 
        ' the TabControl will contain an extra, non-standard "Style" tab
        ' that displays a MapBackgroundControl object (which allows the
        ' user to choose a background color for the map).
        controlList.Add(bgControl)
    End Sub 'AddCustomMapStyleTab


    ' Create a custom menuitem, "Choose Coordinate System", which is available 
    ' when the user right-clicks the Map node.  Choosing this menuitem
    ' displays the Choose Coordinate System dialog.  
    Private Sub AddChooseCoordSysMenuitem()
        ' Create a new menuitem, which calls the MenuItemChooseCoordSys
        ' method (see below). 
        Dim chooseCoordSysMenuItem As New MenuItem("&Choose Coordinate System...", New System.EventHandler(AddressOf Me.MenuItemChooseCoordSys))

        ' Each type of object that can appear in the layer tree 
        ' has a collection of menuitems displayed when the user
        ' right-clicks.  Obtain a reference to that collection, 
        ' and add our new menuitem to the collection. 
        Dim menuItems As IList = LayerControl1.GetLayerTypeMenuItems(GetType(MapInfo.Mapping.Map))

        ' Insert a separator and a new menuitem to the collection of menuitems.  
        menuItems.Add(New MenuItem("-"))
        menuItems.Add(chooseCoordSysMenuItem)
    End Sub 'AddChooseCoordSysMenuitem


    ' The method called when the user chooses the Choose Coordinate System menuitem
    Private Sub MenuItemChooseCoordSys(ByVal sender As [Object], ByVal e As System.EventArgs)
        If MapControl1.Map.IsDisplayCoordSysReadOnly Then
            ' We cannot allow the user to change the coordinate system if 
            ' the coordinate system is locked due to a raster layer. 
            MessageBox.Show("Coordinate system is currently restricted, due to a raster layer.")
        Else
            Dim csysDlg As New CoordSysPickerDlg
            csysDlg.SelectedCoordSys = MapControl1.Map.GetDisplayCoordSys()
            If csysDlg.ShowDialog(Me) = DialogResult.OK Then
                Dim csysNew As CoordSys = csysDlg.SelectedCoordSys
                If Not csysNew Is MapControl1.Map.GetDisplayCoordSys() Then
                    MapControl1.Map.SetDisplayCoordSys(csysNew)
                End If
            End If
        End If
    End Sub 'MenuItemChooseCoordSys


End Class
