Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls

'/ <summary>
'/ This handler's job is to process command and generate new image for MapControl to display
'/ </summary>
'/ <remarks>
'/ The controls from the client create a url pointing to this handler with command and data in it. When the source of the image is set to this url,
'/ this handler is called. Handler tries to get model object from ASP.NET session, if not creates one and then calls appropriate method based on the
'/ command it trying to process. After the model has done it's job the image is exported and streamed back to client and thus only the map image 
'/ gets updated and there is no need to do postback of the entire page. The model has the actual business logic to do operation, therefore the controls
'/ are not dependant upon any product.
'/ </remarks>
Public Class MapController
    Implements IHttpHandler, IRequiresSessionState
    '/ <summary>
    '/ Constructor for the handler
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Sub New()
    End Sub

    '/ <summary>
    '/ Resue the instance of this handler
    '/ </summary>
    '/ <value>Returns true</value>
    '/ <remarks>None</remarks>
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return True
        End Get
    End Property

    '/ <summary>
    '/ This method executes commands. It delegates the execution of the command to Model. The Model extracts the command and calls appropriate 
    '/ method to do the operation
    '/ </summary>
    '/ <remarks>See description of the class for more details</remarks>
    '/ <param name="context">Current context</param>
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim mapModel As MapControlModel = MapControlModel.SetDefaultModelInSession()
        If Not mapModel Is Nothing Then
            mapModel.InvokeCommand()
        End If
    End Sub
End Class

