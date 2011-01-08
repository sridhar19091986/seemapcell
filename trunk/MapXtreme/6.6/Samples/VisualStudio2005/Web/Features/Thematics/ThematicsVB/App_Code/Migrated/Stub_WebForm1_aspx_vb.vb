'===========================================================================
' This file was generated as part of an ASP.NET 2.0 Web project conversion.
' This code file 'App_Code\Migrated\Stub_WebForm1_aspx_vb.vb' was created and contains an abstract class 
' used as a base class for the class 'Migrated_WebForm1' in file 'WebForm1.aspx.vb'.
' This allows the the base class to be referenced by all code files in your project.
' For more information on this code pattern, please refer to http://go.microsoft.com/fwlink/?LinkId=46995 
'===========================================================================




Namespace ThematicsWeb


MustInherit Public Class WebForm1
    Inherits System.Web.UI.Page
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


End Class


End Namespace