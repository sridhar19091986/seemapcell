Imports System
Imports System.Collections
Imports System.Drawing
Imports System.Windows.Forms
Imports MapInfo.Mapping
Imports MapInfo.Windows.Dialogs
Imports MapInfo.Windows.Controls
Imports MapInfo.Data

' A class that enhances a desktop LayerControl with custom context-menu items
' 
' The following code demonstrates how you can use this class to add
' custom menuitems to the layer tree's right-click menu.   
' 
'   Dim lce As New LayerControlEnhancer
'   lce.LayerControl = dlg.LayerControl
'   lce.AddLayerToLabelEnhancement()
'   
' (assuming that dlg is a reference to an existing LayerControlDlg object). 
' 
' Once the enhancement has been applied, the user will be able to
' right-click on a FeatureLayer node or a LabelSource node, then 
' choose items off the context menu to find the LabelSource node
' (if any) that corresponds to the FeatureLayer node, and vice versa.
' 
    Public Class LayerControlEnhancer
        Private _lc As MapInfo.Windows.Controls.LayerControl = Nothing

        ' Keep a reference to the first LabelLayer in the map, 
        ' if there is one: 
    Private _firstLabelLayer As MapInfo.Mapping.LabelLayer = Nothing



        Public Sub New()
        End Sub 'New


    ' The LayerControl property must be assigned before you can 
    ' call the AddLayerToLabelEnhancement method. 

    Public Property LayerControl() As MapInfo.Windows.Controls.LayerControl
        Get
            Return _lc
        End Get
        Set(ByVal Value As MapInfo.Windows.Controls.LayerControl)
            _lc = Value
        End Set
    End Property


    ' Call this method to enhance the LayerControl with custom 
    ' menu items.  
    Public Sub AddLayerToLabelEnhancement()
        If _lc Is Nothing Then
            Throw New NullReferenceException("LayerControl property is null")
        End If

        ' Create new menuitems to find a LabelSource in the layer tree. 
        ' "Find Labels" Menu item:
        ' jumps from the selected FeatureLayer to a matching LabelSource 
        Dim gotoLabelsMenuItem As New MenuItem("Find &Labels", New System.EventHandler(AddressOf Me.MenuItemFindMatchingLabels), Shortcut.CtrlL)

        ' "Find Similar Labels" menu item: 
        ' jumps from a LabelSource to the NEXT LabelSource that uses 
        ' the same table: 
        Dim gotoNextLabelsMenuItem As New MenuItem("Find Similar L&abels", New System.EventHandler(AddressOf Me.MenuItemFindMatchingLabels), Shortcut.None)

        ' Create new menuitems to find a FeatureLayer in the layer tree:
        ' "Find Layer" Menu item: 
        ' jumps from the selected LabelSource to a matching FeatureLayer  
        Dim gotoLayerMenuItem As New MenuItem("Find &Layer", New System.EventHandler(AddressOf Me.MenuItemFindMatchingLayer), Shortcut.CtrlL)

        ' "Find Similar Layer" Menu item: 
        ' jumps from the selected FeatureLayer node to another FeatureLayer 
        ' that is displaying the same table: 
        Dim gotoNextLayerMenuItem As New MenuItem("Find Similar L&ayer", New System.EventHandler(AddressOf Me.MenuItemFindMatchingLayer), Shortcut.None)

        ' Each type of object that can appear in the layer tree 
        ' has a collection of menuitems displayed when the user
        ' right-clicks.  Obtain a reference to that collection, 
        ' and add our new menuitems to the collection. 
        Dim labelsourceMenuItems As IList = _lc.GetLayerTypeMenuItems(GetType(MapInfo.Mapping.LabelSource))

        ' Insert a separator and new menuitems to the LabelSource menu. 
        labelsourceMenuItems.Add(New MenuItem("-"))
        labelsourceMenuItems.Add(gotoLayerMenuItem)
        labelsourceMenuItems.Add(gotoNextLabelsMenuItem)


        Dim featurelayerMenuItems As IList = _lc.GetLayerTypeMenuItems(GetType(MapInfo.Mapping.FeatureLayer))

        ' Insert a separator and new menuitems to the FeatureLayer menu. 
        featurelayerMenuItems.Add(New MenuItem("-"))
        featurelayerMenuItems.Add(gotoLabelsMenuItem)
        featurelayerMenuItems.Add(gotoNextLayerMenuItem)
    End Sub 'AddLayerToLabelEnhancement


    ' The method called when the user chooses the Find Labels menuitem 
    Private Sub MenuItemFindMatchingLabels(ByVal sender As [Object], ByVal e As System.EventArgs)
        PerformFindLabels()
    End Sub 'MenuItemFindMatchingLabels


    Private Sub PerformFindLabels()
        ' See which item the user right-clicked on. 
        ' NOTE: Do not use the LayerControl.SelectedObject property here, 
        ' because nodes in the layer tree are not immediately selected 
        ' when the user right-clicks; instead, use the 
        ' ContextMenuTargetObject property. 
        Dim obj As Object = _lc.ContextMenuTargetObject
        If TypeOf obj Is FeatureLayer Then
            ' Identify the table that's used by the 
            ' currently-selected FeatureLayer:  
            Dim sourceTable As Table = CType(obj, FeatureLayer).Table
            SelectNextLabelSource(sourceTable, Nothing)
        Else
            If TypeOf obj Is MapInfo.Mapping.LabelSource Then
                ' Identify the table that's the basis for the 
                ' currently-selected LabelSource: 
                Dim sourceTable As Table = CType(obj, MapInfo.Mapping.LabelSource).Table
                SelectNextLabelSource(sourceTable, CType(obj, MapInfo.Mapping.LabelSource))
            End If
        End If
    End Sub 'PerformFindLabels


    ' The method called when the user chooses the Find Layer menuitem 
    Private Sub MenuItemFindMatchingLayer(ByVal sender As [Object], ByVal e As System.EventArgs)
        PerformFindLayer()
    End Sub 'MenuItemFindMatchingLayer


    Private Sub PerformFindLayer()
        ' See which item the user right-clicked on. 
        ' NOTE: Do not use the LayerControl.SelectedObject property here, 
        ' because nodes in the layer tree are not immediately selected 
        ' when the user right-clicks; instead, use the 
        ' ContextMenuTargetObject property.
        Dim obj As Object = _lc.ContextMenuTargetObject

        If TypeOf obj Is MapInfo.Mapping.LabelSource Then
            ' Identify the table that's used by the
            ' currently-selected LabelSource 
            Dim sourceTable As Table = CType(obj, MapInfo.Mapping.LabelSource).Table
            SelectNextFeatureLayer(sourceTable, Nothing)
        Else
            If TypeOf obj Is FeatureLayer Then
                ' Identify the table that's used by the
                ' currently-selected FeatureLayer 
                Dim sourceTable As Table = CType(obj, FeatureLayer).Table
                SelectNextFeatureLayer(sourceTable, CType(obj, FeatureLayer))
            End If
        End If
    End Sub 'PerformFindLayer

    ' Find a LabelSource node in the layer tree which uses the 
    ' specified table, and select that node.  If there are no 
    ' LabelSources that use the specified table, ask the user 
    ' whether a LabelSource should be added to the map. 
    '
    ' Parameters: 
    ' sourceTable
    ' A table which may or may not be labeled currently</param>
    '
    ' currentLabelSource
    ' If null is passed, we will select the first LabelSource we find
    ' that uses the specified table; if currentLabelSource is not
    ' null, we will search for the next LabelSource that uses 
    ' the same table. 
    Private Sub SelectNextLabelSource(ByVal sourceTable As Table, ByVal currentLabelSource As MapInfo.Mapping.LabelSource)
        Dim matchingLabelSource As MapInfo.Mapping.LabelSource = LocateNextLabelSource(_lc.Map, sourceTable, currentLabelSource, True)

        If Not (matchingLabelSource Is Nothing) Then
            ' There is at least one LabelSource found that uses
            ' the specified table.  
            If _lc.ContextMenuTargetObject Is matchingLabelSource Then
                ' The user right-clicked on a LabelSource in the tree,
                ' and the only LabelSource that we found is the 
                ' same one the user right-clicked.  So we failed
                ' to find a Next LabelSource. 
                MapInfo.Windows.MessageBox.Show(("There are no more label sources based on the table:" + ControlChars.Lf + ControlChars.Lf + sourceTable.TableInfo.Description))
            Else
                ' We found an appropriate match; select its tree node. 
                _lc.SelectedObject = matchingLabelSource
            End If
        Else
            ' There are NO LabelSource nodes that use the specified table.
            ' The user must have right-clicked a FeatureLayer node 
            ' and then clicked Find Labels, but we could not find any.
            ' Ask the user if we should create a new LabelSource. 
            Dim buttons As MessageBoxButtons = MessageBoxButtons.YesNo
            Dim dr As DialogResult = MessageBox.Show(Nothing, "There are no labels for the selected layer: " + ControlChars.Lf + ControlChars.Lf + sourceTable.TableInfo.Description + ControlChars.Lf + ControlChars.Lf + "Do you want to create a new label source?", "No Labels Found", buttons)

            If dr = DialogResult.Yes Then
                If _firstLabelLayer Is Nothing Then
                    ' There are NO LabelLayers in the map; build one.
                    _firstLabelLayer = New MapInfo.Mapping.LabelLayer("Label Layer")
                    _lc.Map.Layers.Insert(0, _firstLabelLayer)
                End If
                Dim newSource As New MapInfo.Mapping.LabelSource(sourceTable)
                If sourceTable.TableInfo.TableType = TableType.Wms Or sourceTable.TableInfo.TableType = TableType.Grid Or sourceTable.TableInfo.TableType = TableType.Raster Then
                    ' For a Raster Image layer, just label with the layer name.
                    newSource.DefaultLabelProperties.Caption = "'" + sourceTable.TableInfo.Description + "'"
                End If

                _firstLabelLayer.Sources.Append(newSource)

                If Not _lc.UpdateWhenCollectionChanges Then
                    ' The LayerControl is currently configured to NOT update
                    ' automatically when a new node is added to the tree; 
                    ' so we will force the LayerTree to regenerate, so that
                    ' the new LabelSource node appears. 
                    Dim map As Map = _lc.Map
                    _lc.Map = Nothing
                    _lc.Map = map
                End If

                _lc.SelectedObject = newSource
            End If
        End If
    End Sub 'SelectNextLabelSource


    ' Find a LabelSource in the map which uses the 
    ' specified table, and return it, or return null 
    ' if no suitable LabelSource exists.  
    ' 
    ' Parameters: 
    ' 
    ' map: The Map object to search
    ' 
    ' sourceTable: 
    ' A table which may or may not be labeled currently</param>
    ' 
    ' currentLabelSource: 
    ' If null is passed, we will return the first LabelSource we find
    ' that uses the specified table; if currentLabelSource is not
    ' null, we will search for the next LabelSource that uses 
    ' the same table. 
    ' 
    ' wrapAround:  true if you want a "find next" search
    ' to wrap around to the beginning of the layer tree, if necessary,
    ' to find the next LabelSource; false if you do not the search to wrap.
    ' 
    ' Returns a LabelSource object (which may be identical to
    ' the currentLabelSource param, e.g. if the currentLabelSource 
    ' param represents the only LabelSource in the map); or null if 
    ' the map does not contain any LabelSource based on the specified table. 
    Private Function LocateNextLabelSource(ByVal map As Map, ByVal sourceTable As Table, ByVal currentLabelSource As MapInfo.Mapping.LabelSource, ByVal wrapAround As Boolean) As MapInfo.Mapping.LabelSource
        ' Get a collection of all LabelLayers in the map
        Dim labelLayerFilter As New FilterByLayerType(LayerType.Label)

        Dim mapLayerEnum As MapLayerEnumerator = map.Layers.GetMapLayerEnumerator(labelLayerFilter, MapLayerEnumeratorOptions.Recurse)

        _firstLabelLayer = Nothing
        Dim matchingLabelSource As MapInfo.Mapping.LabelSource = Nothing
        Dim firstMatchingLabelSource As MapInfo.Mapping.LabelSource = Nothing
        Dim bPassedCurrentLabelSource As Boolean = False

        ' Given the set of all LabelLayers in the layer tree,
        ' search each LabelLayer to try to find a LabelSource 
        ' child node that is based on the specified table 
        ' (the sourceTable param).  
        ' OR: If a non-null currentLabelSource param
        ' was passed in, then it represents an existing
        ' LabelSource, and our job is to find the NEXT 
        ' LabelSource that uses the same table. 
        Dim ll As MapInfo.Mapping.LabelLayer
        For Each ll In mapLayerEnum
            If _firstLabelLayer Is Nothing Then
                ' Make a note of the first LabelLayer we find;
                ' later, if we decide to create a new LabelSource, 
                ' it will go into this first LabelLayer. 
                _firstLabelLayer = ll
            End If

            ' Look through this LabelLayer's collection of LabelSources
            Dim lblSource As MapInfo.Mapping.LabelSource
            For Each lblSource In ll.Sources
                If lblSource.Table Is sourceTable Then
                    ' This LabelSource uses the correct table
                    If firstMatchingLabelSource Is Nothing Then
                        ' We found our first match, so make a note of it,
                        ' even though it may not be ideal (i.e. it may not
                        ' be the "next" LabelSource that was requested).  
                        firstMatchingLabelSource = lblSource
                    End If

                    ' Now determine whether this match is an ideal match.
                    ' We may have been passed a LabelSource, and asked
                    ' to find the "next" LabelSource.  So we may need 
                    ' to skip over the current LabelSource if it's
                    ' the same as the LabelSource that was passed in. 
                    If Not (currentLabelSource Is Nothing) And Not bPassedCurrentLabelSource Then
                        ' We WERE asked to find the "next" LabelSource,
                        ' which means that our first task is to find 
                        ' the LabelSource that the user right-clicked.
                        ' But according to the flag, we have not yet
                        ' looped past the currently-selected LabelSource. 
                        ' So we will continue the loop instead of 
                        ' assigning  matchingLabelSource.  
                        If lblSource Is currentLabelSource Then
                            ' We were asked to find the next LabelSource,
                            ' and this LabelSource is the same one that 
                            ' was passed in.  In this case, just set the
                            ' flag, so that the next match will be used.  
                            bPassedCurrentLabelSource = True
                        End If
                        GoTo ContinueForEach2
                    End If

                    ' We found a LabelSource that is a perfect match.
                    matchingLabelSource = lblSource
                    Exit For
                End If
ContinueForEach2:
            Next lblSource ' This ends the "for each LabelSource in this LabelLayer" loop
            If Not (matchingLabelSource Is Nothing) Then
                ' We found an ideal match, so we can skip searching 
                ' the other LabelLayers.  Break the outer foreach loop: 
                Exit For
            End If
        Next ll ' This ends the "for each LabelLayer" loop
        If matchingLabelSource Is Nothing Then
            ' We end up here if we did not find an ideal match; for
            ' example, if we were asked to find the "next" node, and 
            ' we did not find a next matching node, but we did find a 
            ' previous matching node, we end up here.  
            ' At this point, since we did not find a perfect match, 
            ' we will consider a less-than-perfect match.
            If wrapAround Then
                ' Wrapping is On, meaning that when we search for
                ' the next LabelSource, we should wrap around to the top of
                ' the layer list, if necessary
                matchingLabelSource = firstMatchingLabelSource
            Else
                ' Wrap is Off, meaning: if we did not find a Next LabelSource,
                ' return the LabelSource that was originally specified, 
                ' which will tell the caller, "there IS no Next LabelSource." 
                matchingLabelSource = currentLabelSource
            End If
        End If
        Return matchingLabelSource
    End Function 'LocateNextLabelSource


    ' Find a FeatureLayer in the layer tree which uses the 
    ' specified table, and select that layer's node.  
    ' If there are no layers that use the specified table, 
    ' ask the user whether a layer should be added to the map. 
    ' 
    ' Parameters: 
    ' 
    ' sourceTable: 
    ' A table which may or may not be displayed in the map</param>
    ' 
    ' currentFeatureLayer: 
    ' If null is passed, we will return the first FeatureLayer we find
    ' that uses the specified table; if currentFeatureLayer is not
    ' null, we will search for the next FeatureLayer that uses 
    ' the same table. 
    Private Sub SelectNextFeatureLayer(ByVal sourceTable As Table, ByVal currentFeatureLayer As FeatureLayer)
        Dim matchingFeatureLayer As FeatureLayer = LocateNextFeatureLayer(_lc.Map, sourceTable, currentFeatureLayer, True)

        If Not (matchingFeatureLayer Is Nothing) Then
            ' There is at least one FeatureLayer found that uses
            ' the specified table.  
            If _lc.ContextMenuTargetObject Is matchingFeatureLayer Then
                ' The user right-clicked on a FeatureLayer in the tree,
                ' and the only FeatureLayer that we found is the 
                ' same one the user right-clicked.  So we failed
                ' to find a Next FeatureLayer. 
                MapInfo.Windows.MessageBox.Show(("There are no more layers based on the table:" + ControlChars.Lf + ControlChars.Lf + sourceTable.TableInfo.Description))
            Else
                ' We found an appropriate match; select its tree node.
                _lc.SelectedObject = matchingFeatureLayer
            End If
        Else
            ' There are NO FeatureLayer nodes that use the specified table.
            ' The user must have right-clicked a LabelSource node 
            ' and then clicked Find Layer, but we could not find one.
            ' Ask the user if we should create a new FeatureLayer.
            Dim buttons As MessageBoxButtons = MessageBoxButtons.YesNo
            Dim result As DialogResult = MessageBox.Show(Nothing, "The selected label source displays labels for this table: " + ControlChars.Lf + ControlChars.Lf + sourceTable.TableInfo.Description + ControlChars.Lf + ControlChars.Lf + "but there are no layers displaying that table. " + ControlChars.Lf + ControlChars.Lf + "Do you want to insert a new layer to display that table?", "No Layer Found", buttons)

            If result = DialogResult.Yes Then
                Dim newLayer As New FeatureLayer(sourceTable)
                _lc.Map.Layers.Add(newLayer)

                If Not _lc.UpdateWhenCollectionChanges Then
                    ' The LayerControl is currently configured to NOT update
                    ' automatically when a new node is added to the tree; 
                    ' so we will force the LayerTree to regenerate, so that
                    ' the new layer node appears. 
                    Dim map As Map = _lc.Map
                    _lc.Map = Nothing
                    _lc.Map = map
                End If

                _lc.SelectedObject = newLayer
            End If
        End If
    End Sub 'SelectNextFeatureLayer


    ' Find a FeatureLayer in the map which uses the 
    ' specified table, and return it. 
    ' 
    ' Parameters: 
    ' 
    ' map:  The Map object to search 
    ' 
    ' sourceTable: 
    ' A table which may or may not be displayed in a FeatureLayer
    ' 
    ' currentFeatureLayer: 
    ' If null is passed, we will return the first FeatureLayer we find
    ' that uses the specified table; if currentFeatureLayer is not
    ' null, we will search for the next FeatureLayer that uses 
    ' the same table. 
    ' 
    ' wrapAround:  true if you want a "find next" search
    ' to wrap around to the beginning of the layer tree, if necessary,
    ' to find the next layer; false if you do not want the search to wrap.
    ' 
    ' Returns a FeatureLayer object (which may be identical to
    ' the currentFeatureLayer param, e.g. if the currentFeatureLayer 
    ' param represents the only FeatureLayer in the map); or null if 
    ' the map does not contain any FeatureLayer based on the specified table. 
    Private Function LocateNextFeatureLayer(ByVal map As Map, ByVal sourceTable As Table, ByVal currentFeatureLayer As FeatureLayer, ByVal wrapAround As Boolean) As FeatureLayer
        Dim featureLayerFilter As New FilterByLayerType(LayerType.Normal, LayerType.Grid, LayerType.Raster, LayerType.Wms)

        Dim mapLayerEnum As MapLayerEnumerator = map.Layers.GetMapLayerEnumerator(featureLayerFilter, MapLayerEnumeratorOptions.Recurse)

        Dim matchingFeatureLayer As FeatureLayer = Nothing
        Dim firstMatchingFeatureLayer As FeatureLayer = Nothing
        Dim bPassedCurrentFeatureLayer As Boolean = False

        Dim featLyr As FeatureLayer
        For Each featLyr In mapLayerEnum
            If featLyr.Table Is sourceTable Then
                ' This FeatureLayer uses the correct table
                If firstMatchingFeatureLayer Is Nothing Then
                    ' We found our first match, so make a note of it,
                    ' even though it may not be ideal (i.e. it may not
                    ' be the "next" FeatureLayer that was requested). 
                    firstMatchingFeatureLayer = featLyr
                End If

                ' Now determine whether this match is an ideal match.
                ' We may have been passed a FeatureLayer, and asked
                ' to find the "next" FeatureLayer.  So we may need 
                ' to skip over the current FeatureLayer if it's
                ' the same as the FeatureLayer that was passed in. 
                If Not (currentFeatureLayer Is Nothing) And Not bPassedCurrentFeatureLayer Then
                    ' We WERE asked to find the "next" FeatureLayer,
                    ' which means that our first task is to find 
                    ' the FeatureLayer that the user right-clicked.
                    ' But according to the flag, we have not yet
                    ' looped past the currently-selected FeatureLayer. 
                    ' So we will continue the loop instead of 
                    ' assigning  matchingFeatureLayer. 
                    If featLyr Is currentFeatureLayer Then
                        bPassedCurrentFeatureLayer = True
                    End If
                    GoTo ContinueForEach1
                End If

                matchingFeatureLayer = featLyr
                Exit For
            End If
ContinueForEach1:
        Next featLyr

        If matchingFeatureLayer Is Nothing Then
            ' We did not find an ideal match (i.e. we may have been
            ' asked to find the Next FeatureLayer, and there may not
            ' have been a Next FeatureLayer).  
            ' At this point, since we did not find a perfect match, 
            ' we will consider a less-than-perfect match.
            If wrapAround Then
                ' wrapAround is true, meaning that when we search for
                ' the next layer, we should wrap around to the top of
                ' the layer list, if necessary
                matchingFeatureLayer = firstMatchingFeatureLayer
            Else
                ' wrapAround is false, meaning that when we cannot find
                ' a next layer, we should return the layer that was 
                ' originally specified, which will tell the caller, 
                ' "there is no Next layer." 
                matchingFeatureLayer = currentFeatureLayer
            End If
        End If
        Return matchingFeatureLayer
    End Function 'LocateNextFeatureLayer
End Class 'LayerControlEnhancer 
