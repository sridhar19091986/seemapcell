Imports System.Runtime.Serialization
Imports MapInfo.Data
Imports MapInfo.Engine
Imports MapInfo.Mapping

' This class demonstrates how to create your own UserDraw layer by deriving from MapInfo.Mapping.UserDrawLayer.
' There a 2 requirements 
' 1. you must imlement the draw method to draw something. This class draws a translucent highligh rect around a bunch of cities.
' 2 The class must have be serializable if it is going to be used in the LayerControl dialog because it uses Map.Clone
' which uses serialization. to make a UserDrawLayer serializable it needs:
'   serializable attribute
'   a constructor with name and alias that should call the base class constructor
'   optionally override GetObjectData and SetObjectData to save and restore any properties or members.
<Serializable()> _
Public Class UserDrawHighlighter
  Inherits UserDrawLayer

  Private _highlightColor As Color = Color.Yellow
  Private _opacity As Integer = 128
  Private cities As String() = {"Washington DC", "Baghdad", "Hanoi", "Madrid", "Mexico City", "Tokyo", "Paris", "Vienna"}

  ' this constructor is required by UserDrawLayer for deserialization
  Public Sub New(ByVal name As String, ByVal myAlias As String)
    MyBase.New(name, myAlias)
  End Sub
  ' Serialize our color and opacity
  Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
    MyBase.GetObjectData(info, context)
    info.AddValue("Color", _highlightColor)
    info.AddValue("Opacity", _opacity)
  End Sub
  ' Deserialize our color and opacity
  Public Overrides Sub SetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
    MyBase.SetObjectData(info, context)
    _highlightColor = info.GetValue("Color", GetType(Color))
    _opacity = info.GetInt32("Opacity")
  End Sub
  Public Property HighlightColor()
    Get
      HighlightColor = _highlightColor
    End Get
    Set(ByVal Value)
      _highlightColor = Value
      Dim m As Map
      Dim l As Layers = Me.Parent
      m = l.Parent
      m.Invalidate()
    End Set
  End Property
  Public Property Opacity()
    Get
      Opacity = _opacity
    End Get
    Set(ByVal Value)
      _opacity = Value
      Dim m As Map
      Dim l As Layers = Me.Parent
      m = l.Parent
      m.Invalidate()
    End Set
  End Property
  ' Draws a fixed size translucent rectangle around a given point
  Private Sub HighlightPoint(ByVal point As System.Drawing.Point, ByVal graphics As Graphics)
    'TODO: could create pen and brush as class members so don't need to recreate constantly
    Dim blackPen As Pen = New Pen(Color.Black, 3)
    Dim brush As SolidBrush = New SolidBrush(Color.FromArgb(_opacity, _highlightColor))
    ' Create location and size of rectangle.

    Dim x As Single = point.X - 10
    Dim y As Single = point.Y - 10
    Dim width As Single = 20.0F
    Dim height As Single = 20.0F

    'Draw rectangle to screen.
    'graphics.DrawRectangle(blackPen, x, y, width, height) ' in case you want a border
    graphics.FillRectangle(brush, x, y, width, height)
  End Sub
  ' Draws each city in cities with the translucent highlight color around it.
  Public Overrides Sub Draw(ByVal clientRect As System.Drawing.Rectangle, ByVal updateRect As System.Drawing.Rectangle, ByVal graphics As System.Drawing.Graphics)
    Dim searchInfo As SearchInfo
    Dim t As Table = Session.Current.Catalog.Item("worldcap")
    t.BeginAccess(TableAccessMode.Read)
    For Each s As String In cities
      searchInfo = MapInfo.Data.SearchInfoFactory.SearchWhere("Capital='" + s + "'")
      Dim f As Feature = Session.Current.Catalog.SearchForFeature(t, searchInfo)
      If Not f Is Nothing Then
        Dim fl As FeatureLayer = Me.Map.Layers.Item("worldcap")
        Dim point As New System.Drawing.Point
        ' need to convert from map to screen coordinates
        fl.DisplayTransform.ToDisplay(f.Geometry.Centroid, point)
        ' now draw the point
        HighlightPoint(point, graphics)
      End If
    Next
    t.EndAccess()
  End Sub
End Class

