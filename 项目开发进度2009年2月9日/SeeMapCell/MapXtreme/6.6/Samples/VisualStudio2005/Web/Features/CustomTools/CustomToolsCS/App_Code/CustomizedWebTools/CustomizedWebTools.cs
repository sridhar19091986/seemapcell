using System;
using MapInfo.WebControls;

namespace CustomizedWebTools
{
	/// <summary>
	/// PinPointWebTool control for AddPinPointCommand and ClearPinPointCommand tools.
	/// </summary>
	public class PinPointWebTool : MapInfo.WebControls.WebTool
	{
		public PinPointWebTool()
		{
			//Command = "";
			this.ClientInteraction = ClientInteractionEnum.ClickInteraction.ToString();
			Active = false;
			//CursorImageUrl = string.Format("{0}/MapInfoWeb{1}.cur", MapInfo.WebControls.Resources._resourceFolderMain, Command);
			CursorImageUrl = "";
		}
	}
}
