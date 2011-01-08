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
	public partial class WebForm1_temp : System.Web.UI.Page {
	
		protected void Page_Load(object sender, System.EventArgs e) {
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
		protected void Page_UnLoad(object sender, System.EventArgs e) {
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

		}
		#endregion
	}

	/// <summary>
	/// State management can be complex operation. It is efficient to save and restore what is needed.
	/// The method used here is described in the BEST PRACTISES documentation. This is a template application
	/// which changes zoom, center, default selection and layer visibility. Hence we save and restore only these objects.
	/// </summary>
}
