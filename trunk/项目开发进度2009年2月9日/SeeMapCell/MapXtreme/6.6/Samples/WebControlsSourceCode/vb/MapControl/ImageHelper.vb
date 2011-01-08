Imports System
Imports System.Web

'/	<summary>If you are NOT using MapInfo WebControls, this class contains method
'/ to help you save map or legend images into Cache and return a URL that you can use
'/ in href of your img tag to pick up the image. The following are some code examples:</summary>
'/ <include file="../../Snippets/Dev/MapInfo.Web.cs.xml" path="CodeSamples/Snippets[@name='MapInfo_Web_ImageHelper']/*"/>
Public Class ImageHelper
    '/	<summary>Constructor for stream handler.</summary>
    Friend Sub New()
    End Sub

    '/	<summary>Indicates the Path of the URL where we get map image from.</summary>
    '/	<remarks>None.</remarks>
    Public Shared Path As String = "GetMapXtremeImage.aspx"
    '/	<summary>Used to extract image stream from Cache.</summary>
    '/	<remarks>None.</remarks>
    Public Shared StreamIDParameter As String = "StreamID"
    '/	<summary>Indicates which format was used to export the image by map.</summary>
    '/	<remarks>This is used to set the response content type.</remarks>
    Public Shared FormatParameter As String = "Format"

    '/	<summary>Returns an unique ID that you can use as the key to the cached
    '/ object.</summary>
    '/	<returns>Returns an unique ID, GUID.</returns>
    Public Shared Function GetUniqueID() As String
        Return Guid.NewGuid().ToString()
    End Function

    '/			<summary>Formats the query string to be used to extract the stream out of
    '/ Cache.</summary>
    '/			<param name="streamID">Stream ID.</param>
    '/			<param name="imageFormat">Image format.</param>
    '/			<returns>Returns a String containing the URL. You can put this string in href
    '/ of the image tag.</returns>
    Public Shared Function GetImageURL(ByVal streamID As String, ByVal imageFormat As String) As String
        Return String.Format("{0}?{1}={2}&{3}={4}", Path, StreamIDParameter, streamID, FormatParameter, imageFormat)
    End Function
    '/			<summary>Formats the query string to be used to extract the stream out of
    '/ Cache.</summary>
    '/			<param name="streamID">Stream ID.</param>
    '/			<returns>Returns a String containing the query.</returns>
    Public Shared Function GetImageURL(ByVal streamID As String) As String
        Return String.Format("{0}?{1}={2}", Path, StreamIDParameter, streamID)
    End Function

    '/	<summary>Inserts the obj that is associated with the key in the Context.Cache.</summary>
    '/ <param name="key">An unique key to the obj.</param>
    '/	<param name="obj">The object that need to be cached, normally this is a memory
    '/ Stream of an image.</param>
    '/	<param name="timeOutInMinutes">Auto-remove this obj from cache after this integer,
    '/ timeOutInMinutes.</param>
    Public Shared Sub SetImageToCache(ByVal key As String, ByVal obj As Object, ByVal timeOutInMinutes As Integer)
        HttpContext.Current.Cache.Insert(key, obj, Nothing, DateTime.Now.AddMinutes(timeOutInMinutes), TimeSpan.Zero)
    End Sub

    '/			<summary>Returns the obj that associated with the key from the Context.Cache
    '/ and this object is removed from the Cache.</summary>
    '/			<param name="key">An unique key to the obj.</param>
    '/			<returns>Returns an obj in the Cache.</returns>
    Public Shared Function GetImageFromCache(ByVal key As String) As Object
        Dim obj As Object = HttpContext.Current.Cache(key)
        HttpContext.Current.Cache.Remove(key)
        Return obj
    End Function

    '/			<summary>Returns the obj that associated with the key from the Context.Cache
    '/ and this object is removed from the Cache.</summary>
    '/			<param name="key">An unique key to the obj.</param>
    '/			<param name="bRemove">If <c>true</c>, remove the obj from the cache.</param>
    '/			<returns>Returns the obj in the cache.</returns>
    Public Shared Function GetImageFromCache(ByVal key As String, ByVal bRemove As Boolean) As Object
        Dim obj As Object = HttpContext.Current.Cache(key)
        If (bRemove = True) Then
            HttpContext.Current.Cache.Remove(key)
        End If
        Return obj
    End Function
End Class
