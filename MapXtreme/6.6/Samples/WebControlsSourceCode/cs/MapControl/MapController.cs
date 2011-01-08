using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Xml;

namespace MapInfo.WebControls
{
	/// <summary>
	/// Processes commands and generates a new image for the MapControl to display.
	/// </summary>
	/// <remarks>
	/// The controls from the client create a URL pointing to this handler with commands and data in it. 
	/// When the source of the image is set to this URL, this handler is called. The handler tries to get a model object from 
	/// the ASP.NET session. If it cannot get the model, it creates one and then calls the appropriate method based on the
	/// command it trying to process. After the model has performed the task, the image is exported and streamed back to client 
	/// so that only the map image gets updated. There is no need to do a postback of the entire page. 
	/// The model has the business logic to do the operation. Therefore, the controls
	/// are not dependant upon a particular product.
	/// </remarks>
	public class MapController : IHttpHandler, IRequiresSessionState {
		/// <summary>
		/// Reuses the instance of this handler.
		/// </summary>
		/// <value>Returns <c>true</c>.</value>
		/// <remarks>None</remarks>
		public bool IsReusable {
			get {
				return true;
			}
		}

		/// <summary>
		/// Executes commands and delegates the execution of the command to the model.
		/// </summary>
		/// <remarks>The model extracts the command and calls the appropriate method to perform the operation.
		/// See description of the class for more details.</remarks>
		/// <param name="context">Current context</param>
		public void ProcessRequest(HttpContext context) {
			MapControlModel mapModel = MapControlModel.SetDefaultModelInSession();
			if (mapModel != null) {
				mapModel.InvokeCommand();
			}
		}
	}
}
