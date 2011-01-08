Namespace ThematicsWeb

Public Class SampleConstants
    ' eword is a mdb table.
    Public Shared ReadOnly EWorldAlias As String = "eworld"
    ' eworld.tab wrapped eword.mdb table.
    Public Shared ReadOnly EWorldTabFileName As String = "eworld.tab"
    ' ThemeTableAlias is the table on which the sample will create. 
    Public Shared ReadOnly ThemeLayerAlias As String = "world"
    ' ThemeLayerAlias is the FeatureLayer on which the sample will create.
    Public Shared ReadOnly ThemeTableAlias As String = "world"
    ' new LabelLayer alias for this sample.
    Public Shared ReadOnly NewLabelLayerAlias As String = "LabelAliasForBoundData"
    ' alias name for a group layer which contents are going to be changed frenquently.
    Public Shared ReadOnly GroupLayerAlias = "tempGroupLayerAlias"
    ' Bound data columns, these columns come from eworld table.
    Public Shared ReadOnly BoundDataColumns As String() = New String() {"Pop_1994", "Pop_Male", "Pop_Fem"}
    ' Will be used in table's AddColumns method.
    Public Shared ReadOnly SouceMatchColumn As String = "Country"
    ' Will be used in table's AddColumns method.
    Public Shared ReadOnly TableMatchColumn As String = "Country"

    Private Sub New()
    End Sub
End Class

End Namespace
