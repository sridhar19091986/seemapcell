imports System.Web
<Serializable> _
Public Class ManualSerializer

    ' These methods use the System formatters to save the MapXtreme objects in the session state as a binary blob.
    ' If you simply send it to the Session state then it will automagically extract itself the next time the user
    ' accesses the site. We want to be able to deserialize certain objects when we want them.
    ' This method takes an object and saves it's binary stream.
    Public Shared Function BinaryStreamFromObject(ByVal ser As Object) As Byte()
        Dim MemStr As System.IO.MemoryStream = New System.IO.MemoryStream
        Dim formatter As System.Runtime.Serialization.Formatters.Binary.BinaryFormatter = New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        formatter.Serialize(MemStr, ser)
        Return MemStr.GetBuffer
    End Function

    ' These methods use the System formatters to save the MapXtreme objects in the session state as a binary blob.
    ' If you simply send it to the Session state then it will automagically extract itself the next time the user
    ' accesses the site. We want to be able to deserialize certain objects when we want them.
    ' This method takes a binary stream and returns an object. Casting happens later.
    Public Shared Function ObjectFromBinaryStream(ByVal bits As Byte()) As Object
        Dim ret As Object = Nothing
        If Not bits Is Nothing Then
            Dim MemStr As System.IO.MemoryStream = New System.IO.MemoryStream(bits)
            Dim formatter As System.Runtime.Serialization.Formatters.Binary.BinaryFormatter = New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            ret = formatter.Deserialize(MemStr)
        End If
        Return ret
    End Function

    Default Public Property Item(ByVal hashName As String) As Object
        Get
            Return ObjectFromBinaryStream(CType(HttpContext.Current.Session(hashName), Byte()))
        End Get
        Set(ByVal Value As Object)
            HttpContext.Current.Session(hashName) = BinaryStreamFromObject(Value)
        End Set
    End Property
    '/ <summary>
    '/ Saves MapXtreme object into HttpSessionState
    '/ </summary>
    '/ <param name="o">MapXtreme object</param>
    '/ <param name="name">Name to be used as key</param>
    '/ <remarks>This function uses BinaryFormatter to save stream of bytes into HttpSessionState. 
    '/ The error handling for this method has to be taken care of by users in their application.
    '/ </remarks>
    Public Shared Sub SaveMapXtremeObjectIntoHttpSession(ByVal o As Object, ByVal name As String)
        HttpContext.Current.Session(name) = ManualSerializer.BinaryStreamFromObject(o)
    End Sub

    '/ <summary>
    '/ Restores MapXtreme from HttpSessionState into MapXtreme Session.
    '/ </summary>
    '/ <param name="name">Name used as key to save the object</param>
    '/ <remarks>This function restores MapXtreme object from HttpSessionState into MapXtreme session.
    '/ 	The error handling for this method has to be taken care of by users in their application.
    '/ </remarks>
    Public Shared Sub RestoreMapXtremeObjectFromHttpSession(ByVal name As String)
        If Not HttpContext.Current.Session(name) Is Nothing Then
            Dim bits() As Byte = CType(HttpContext.Current.Session(name), Byte())
            Dim o As Object = ManualSerializer.ObjectFromBinaryStream(bits)
        End If
    End Sub
End Class

