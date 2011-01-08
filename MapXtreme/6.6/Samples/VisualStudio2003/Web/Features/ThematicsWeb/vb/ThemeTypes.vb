Imports System.Collections
Public Enum ThemeAndModifierTypes
    RangedTheme
    IndividualValueTheme
    FeatureOverrideStyleModifier
    DotDensityTheme
    BarTheme
    PieTheme
    GraduatedSymbolTheme
    Label_IndividualValueLabelTheme
    Label_OverrideLabelModifier
    Label_RangedLabelTheme
End Enum

Public Class ThemesAndModifiers
    Private Shared _hash As Hashtable

    Shared Sub New()
        _hash = New Hashtable
        _hash.Add(ThemeAndModifierTypes.BarTheme.ToString, ThemeAndModifierTypes.BarTheme)
        _hash.Add(ThemeAndModifierTypes.PieTheme.ToString, ThemeAndModifierTypes.PieTheme)
        _hash.Add(ThemeAndModifierTypes.RangedTheme.ToString, ThemeAndModifierTypes.RangedTheme)
        _hash.Add(ThemeAndModifierTypes.IndividualValueTheme.ToString, ThemeAndModifierTypes.IndividualValueTheme)
        _hash.Add(ThemeAndModifierTypes.GraduatedSymbolTheme.ToString, ThemeAndModifierTypes.GraduatedSymbolTheme)
        _hash.Add(ThemeAndModifierTypes.FeatureOverrideStyleModifier.ToString, ThemeAndModifierTypes.FeatureOverrideStyleModifier)
        _hash.Add(ThemeAndModifierTypes.DotDensityTheme.ToString, ThemeAndModifierTypes.DotDensityTheme)
        _hash.Add(ThemeAndModifierTypes.Label_IndividualValueLabelTheme.ToString, ThemeAndModifierTypes.Label_IndividualValueLabelTheme)
        _hash.Add(ThemeAndModifierTypes.Label_RangedLabelTheme.ToString, ThemeAndModifierTypes.Label_RangedLabelTheme)
        _hash.Add(ThemeAndModifierTypes.Label_OverrideLabelModifier.ToString, ThemeAndModifierTypes.Label_OverrideLabelModifier)
    End Sub
    Public Shared Sub FillThemeNames(ByVal list As IList)
        Dim iEnum As IEnumerator = _hash.Keys.GetEnumerator
        Do While iEnum.MoveNext
            list.Add(_hash.Item(iEnum.Current).ToString)
        Loop
    End Sub

    Public Shared Function GetThemeAndModifierTypesByName(ByVal themeName As String) As ThemeAndModifierTypes
        If Not _hash.Contains(themeName) Then
            Throw New ArgumentException("Wrong themeName!!!")
        End If
        Return CType(_hash.Item(themeName), ThemeAndModifierTypes)
    End Function
End Class

