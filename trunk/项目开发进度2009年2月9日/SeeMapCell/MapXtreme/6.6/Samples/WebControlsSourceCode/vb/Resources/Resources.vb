Imports System
Imports System.IO
Imports System.Reflection
Imports System.Resources

'/<summary>Manages access to application resources.</summary>
'/<remarks>Used to obtain objects stored as resources (bitmaps, icons, cursors and strings).</remarks>
Friend Class Resources
    '/ <summary>
    '/ Location of the resouces such as javascript files images.
    '/ </summary>
    '/ <remarks>None</remarks>
    Private Shared _resourceFolder As String = "/MapXtremeWebResources 6_6"
    Private Shared _resouceFolder80 As String = "MapXtremeWebResources"

    '/ <summary>
    '/ Property to get and set the location of the resources such as javascript files  and images.
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Shared Property ResourceFolder() As String
        Get
            If System.Environment.Version.Major > 1 Then
                Return _resouceFolder80
            Else
                Return _resourceFolder
            End If
        End Get
        Set(ByVal Value As String)
            _resourceFolder = value
        End Set
    End Property

    ' Object for accessing resources.
    Private Shared _resources As MapInfo.L10N.Resources

    ' Property to return the resources object; creates it if ness.
    Private Shared ReadOnly Property ResourceManager() As MapInfo.L10N.Resources
        Get
            If _resources Is Nothing Then
                _resources = New MapInfo.L10N.Resources(System.Reflection.Assembly.GetExecutingAssembly(), "MapInfo.WebControls")
            End If
            Return _resources
        End Get
    End Property

    '/			<summary>Retrieves a String object from resources.</summary>
    '/			<remarks>The String returned is from the Strings resource table.</remarks>
    '/			<param name="resName">The name of the String to get.</param>
    '/			<returns>Returns a String object.</returns>
    '/			<exception cref="T:System.ArgumentNullException"/>
    '/			<exception cref="T:System.Resources.MissingManifestResourceException"/>
    Public Shared Function GetString(ByVal resName As String) As String
        Return _resources.GetString(resName)
    End Function

    '/	<summary>Retrieves a file from resources in the form of a stream.</summary>
    '/	<remarks>The object returned is from the Files resource table.</remarks>
    '/	<param name="resName">The name of the file resource to get.</param>
    '/	<returns>Returns a Stream object.</returns>
    '/	<exception cref="T:System.ArgumentNullException"/>
    '/	<exception cref="T:System.Resources.MissingManifestResourceException"/>
    Public Shared Function GetFile(ByVal resName As String) As Stream
        Return _resources.GetFile(resName)
    End Function
End Class

'/ <summary>
'/ A class providing access to localization utilities.
'/ </summary>
Friend Class L10NUtils
    Private Shared _resources As MapInfo.L10N.Resources

    '/ <summary>
    '/ Gets a MapInfo.L10N.Resources objects.
    '/ </summary>
    Friend Shared ReadOnly Property Resources() As MapInfo.L10N.Resources
        Get
            If _resources Is Nothing Then
                _resources = New MapInfo.L10N.Resources(System.Reflection.Assembly.GetExecutingAssembly(), "MapInfo.WebControls")
            End If
            Return _resources
        End Get
    End Property
End Class
