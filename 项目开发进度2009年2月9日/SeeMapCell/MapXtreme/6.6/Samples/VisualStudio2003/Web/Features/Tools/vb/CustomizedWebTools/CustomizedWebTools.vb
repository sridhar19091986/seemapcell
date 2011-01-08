Imports System.Web
' <summary>
' PinPointWebTool control for AddPinPointCommand and ClearPinPointCommand tools.
' </summary>
Public Class PinPointWebTool
    Inherits MapInfo.WebControls.WebTool
    Public Sub New()
        ' Command = ""
        Me.ClientInteraction = ClientInteractionEnum.ClickInteraction.ToString()
        Active = False
        CursorImageUrl = ""
    End Sub
End Class
