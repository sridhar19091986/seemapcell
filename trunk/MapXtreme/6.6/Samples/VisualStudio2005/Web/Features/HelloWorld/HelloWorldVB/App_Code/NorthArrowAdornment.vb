Imports System.IO
Imports System.Drawing
Imports System.Runtime.Serialization


Namespace HelloWorldWeb

<Serializable()> _
Public NotInheritable Class NorthArrowAdornment
    Implements MapInfo.Mapping.IAdornment, ISerializable
    ' Fields
    Private _alias As String
    Private _imageStream As System.IO.MemoryStream
    Private _location As System.Drawing.Point
    Private _mapAlias As String
    Private _name As String
    Private _size As System.Drawing.Size
    Private _blockViewChangedEvent As Boolean = False

    Public Sub New(ByVal mapAlias As String, ByVal size As System.Drawing.Size, ByVal adornmentAlias As String, ByVal name As String, ByVal fileName As String)
        Me._size = size
        Me._alias = adornmentAlias
        Me._name = name
        Me._location = New Point(0, 0)
        Me._mapAlias = mapAlias
        Me._imageStream = New System.IO.MemoryStream
        Dim image As Image = image.FromFile(fileName)
        image.Save(Me._imageStream, image.RawFormat)
        Me._imageStream.Position = 0
        image.Dispose()
    End Sub

    Public Sub New(ByVal adornmentAlias As String, ByVal mapAlias As String)
        Me._alias = adornmentAlias
        Me._mapAlias = mapAlias
    End Sub
#Region "IAdornment Members"
    Public Property Size() As System.Drawing.Size Implements MapInfo.Mapping.IAdornment.Size
        Get
            Return _size
        End Get
        Set(ByVal value As Size)
            If (Not _size.Equals(value)) Then
                Me._size = value
                Me.OnViewSizeChanged()
            End If
        End Set
    End Property
    Public Property Name() As String Implements MapInfo.Mapping.IAdornment.Name
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Public Property Location() As System.Drawing.Point Implements MapInfo.Mapping.IAdornment.Location
        Get
            Return _location
        End Get
        Set(ByVal value As System.Drawing.Point)
            If (Not Me.BlockViewChangedEvent AndAlso (Not _location.Equals(value))) Then
                _location = value
                OnViewLocationChanged()
            End If
        End Set
    End Property

    Public Property [Alias]() As String Implements MapInfo.Mapping.IAdornment.Alias
        Get
            Return _alias
        End Get
        Set(ByVal value As String)
            If (Not Me.Map Is Nothing) Then
                Dim adornment As MapInfo.Mapping.IAdornment = Me.Map.Adornments.Item(value)
                If ((Not adornment Is Nothing) AndAlso Not Object.ReferenceEquals(Me, adornment)) Then
                    Throw New ArgumentException("AliasNotUnique!!!")
                End If
            End If
            _alias = value
        End Set
    End Property

    Public ReadOnly Property Map() As MapInfo.Mapping.Map Implements MapInfo.Mapping.IAdornment.Map
        Get
            Return MapInfo.Engine.Session.Current.MapFactory.Item(_mapAlias)
        End Get
    End Property


    Public Sub Draw(ByVal graphics As System.Drawing.Graphics, ByVal updateArea As System.Drawing.Rectangle, ByVal drawPnt As System.Drawing.Point) Implements MapInfo.Mapping.IAdornment.Draw
        _imageStream.Position = 0
        Dim image As System.Drawing.Image = System.Drawing.Image.FromStream(Me._imageStream)
        graphics.DrawImage(image, drawPnt)
        image.Dispose()
    End Sub

    Public Event ViewChangedEvent(ByVal sender As Object, ByVal e As MapInfo.Mapping.ViewChangedEventArgs) Implements MapInfo.Mapping.IAdornment.ViewChangedEvent

#End Region

    Public Property BlockViewChangedEvent() As Boolean
        Get
            Return _blockViewChangedEvent
        End Get
        Set(ByVal value As Boolean)
            _blockViewChangedEvent = value
        End Set
    End Property

    Private Sub OnViewChanged(ByVal e As MapInfo.Mapping.ViewChangedEventArgs)
        If Not Me.BlockViewChangedEvent Then
            RaiseEvent ViewChangedEvent(Me, e)
        End If
    End Sub

    Private Sub OnViewChanged()
        Dim e As New MapInfo.Mapping.ViewChangedEventArgs
        Me.OnViewChanged(e)
    End Sub

    Private Sub OnViewSizeChanged()
        Dim e As New MapInfo.Mapping.ViewChangedEventArgs
        e.SizeChange = True
        Me.OnViewChanged(e)
    End Sub

    Private Sub OnViewLocationChanged()
        Dim e As New MapInfo.Mapping.ViewChangedEventArgs
        e.LocationChange = True
        Me.OnViewChanged(e)
    End Sub

    Private Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext) Implements ISerializable.GetObjectData
        info.AddValue("_alias", Me._alias)
        info.AddValue("_name", Me._name)
        info.AddValue("_size", Me._size)
        info.AddValue("_mapAlias", Me._mapAlias)
        info.AddValue("_location", Me._location)
        info.AddValue("_imageStream", Me._imageStream)
        info.SetType(GetType(NorthArrowAdornmentDeserializer))
    End Sub

    Public Sub SetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        Me._name = info.GetString("_name")
        Me._size = CType(info.GetValue("_size", GetType(Size)), Size)
        Me._alias = info.GetString("_alias")
        Me._mapAlias = info.GetString("_mapAlias")
        Me._location = CType(info.GetValue("_location", GetType(Point)), Point)
        Me._imageStream = CType(info.GetValue("_imageStream", GetType(MemoryStream)), MemoryStream)
    End Sub
End Class

' <summary>Implements deserialization of a NorthArrowAdornment instance.</summary>
' <remarks>See the article at 
' http://msdn.microsoft.com/msdnmag/issues/02/07/net/print.asp.
' See also the "Object References" section of the article at
' http://community.borland.com/article/0,1410,29787,00.html.
' </remarks>
<Serializable()> _
Public NotInheritable Class NorthArrowAdornmentDeserializer
    Implements IObjectReference, ISerializable

    ' Fields
    Private _adornment As NorthArrowAdornment = Nothing

    Private Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        Dim [alias] As String = info.GetString("_alias")
        Dim mapAlias As String = info.GetString("_mapAlias")

        ' See if the "same" map exists.
        Dim map As MapInfo.Mapping.Map = MapInfo.Engine.Session.Current.MapFactory.Item(mapAlias)
        If (map Is Nothing) Then
            Throw New SerializationException(MapInfo.Engine.Session.Current.Resources.GetString("MapInfo.Serialization.AdornmentMapNotFound", [alias], mapAlias))
        End If

        Dim adornment As MapInfo.Mapping.IAdornment = map.Adornments.Item([alias])
        If (Not adornment Is Nothing) Then
            If TypeOf adornment Is NorthArrowAdornment Then
                _adornment = CType(adornment, NorthArrowAdornment)
            Else
                map.Adornments.Remove([alias])
                _adornment = Nothing
                adornment = Nothing
            End If
        End If
        ' The "same" adornment doesn't exist. Create it from scratch.
        If (adornment Is Nothing) Then
            _adornment = New NorthArrowAdornment([alias], mapAlias)
            map.Adornments.Append(_adornment)
        End If
        _adornment.SetObjectData(info, context)
    End Sub


    Private Function GetRealObject(ByVal context As StreamingContext) As Object Implements IObjectReference.GetRealObject
        ' NOTE: This method is called twice for each deserialization.
        ' According to the http://community.borland.com/article/0,1410,29787,00.html
        ' article, this was known by Microsoft as a bug in the 1.1 beta.
        ' There is a work around, but as long as
        ' there's no processing done here, no harm is done.
        Return _adornment
    End Function

    ' <summary>Required by the ISerializable interface, but this one is never called.</summary>
    ' <param name="info">The SerializationInfo to populate with data.</param>
    ' <param name="context">The destination for this serialization.</param>
    Private Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext) Implements ISerializable.GetObjectData
    End Sub
End Class

End Namespace

