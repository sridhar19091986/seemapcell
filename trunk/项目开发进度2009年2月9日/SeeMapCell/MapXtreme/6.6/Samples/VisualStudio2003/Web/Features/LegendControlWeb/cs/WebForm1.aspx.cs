using System;
using System.Web;
using MapInfo.Engine;
using MapInfo.Geometry;
using MapInfo.Mapping.Legends;
using MapInfo.WebControls;
using MapInfo.Mapping;

namespace LegendControlWeb {
	/// <summary>
	/// Summary description for  LegendControlWeb.
	/// </summary>
	public class WebForm1_temp : System.Web.UI.Page {
		protected MapInfo.WebControls.MapControl MapControl1;
		protected MapInfo.WebControls.ZoomInTool ZoomInTool1;
		protected MapInfo.WebControls.ZoomOutTool ZoomOutTool1;
		protected MapInfo.WebControls.SouthNavigationTool SouthNavigationTool2;
		protected MapInfo.WebControls.NorthNavigationTool NorthNavigationTool2;
		protected MapInfo.WebControls.EastNavigationTool EastNavigationTool2;
		protected MapInfo.WebControls.WestNavigationTool WestNavigationTool2;
		protected MapInfo.WebControls.NorthEastNavigationTool NorthEastNavigationTool1;
		protected MapInfo.WebControls.SouthWestNavigationTool SouthWestNavigationTool1;
		protected MapInfo.WebControls.SouthEastNavigationTool SouthEastNavigationTool1;
		protected MapInfo.WebControls.NorthWestNavigationTool NorthWestNavigationTool1;
		protected MapInfo.WebControls.ZoomBarTool ZoomBarTool1;
		protected MapInfo.WebControls.ZoomBarTool ZoomBarTool2;
		protected MapInfo.WebControls.ZoomBarTool ZoomBarTool3;
		protected MapInfo.WebControls.ZoomBarTool ZoomBarTool4;
		protected MapInfo.WebControls.ZoomBarTool ZoomBarTool5;
		protected System.Web.UI.WebControls.Image Image1;
		protected System.Web.UI.WebControls.Image Image2;
		protected MapInfo.WebControls.CenterTool CenterTool1;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.TextBox TextBox1;
		protected MapInfo.WebControls.PanTool PanTool1;
	
		private void Page_Load(object sender, System.EventArgs e) {
			// Put user code to initialize the page here
			if (Session.IsNewSession) {
				//get default mapcontrol model from session
				MapInfo.WebControls.MapControlModel controlModel = MapControlModel.SetDefaultModelInSession();
				
				//add custom commands to map control model
				controlModel.Commands.Add(new CreateTheme());
				controlModel.Commands.Add(new RemoveTheme());
				controlModel.Commands.Add(new GetLegend());
				
				//instanciate AppStateManager class
				AppStateManager myStateManager = new AppStateManager();

				//put current map alias to state manager dictionary
				myStateManager.ParamsDictionary[StateManager.ActiveMapAliasKey] = this.MapControl1.MapAlias;

				//put state manager to session
				StateManager.PutStateManagerInSession(myStateManager);
			}

			// Now Restore State
			StateManager.GetStateManagerFromSession().RestoreState();
		}

		// At the time of unloading the page, save the state
		private void Page_UnLoad(object sender, System.EventArgs e) {
			StateManager.GetStateManagerFromSession().SaveState();
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {    
			this.Unload += new System.EventHandler(this.Page_UnLoad);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
