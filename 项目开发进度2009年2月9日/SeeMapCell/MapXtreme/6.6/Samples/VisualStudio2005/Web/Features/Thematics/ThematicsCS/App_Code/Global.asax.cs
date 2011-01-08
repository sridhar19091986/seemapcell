using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.SessionState;
using MapInfo.WebControls;

namespace ThematicsWeb
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global : System.Web.HttpApplication
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)
		{
		}
 
		protected void Session_Start(Object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_Error(Object sender, EventArgs e) {
			Exception ex = Server.GetLastError();
			if (ex != null && ex.Message.Length > 0) {
				// If the request was for the map image then write the messages into a bitmap and send it back.
				if (HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("MapController") >= 0) {
					// Get height and width from the request if it was a request for map image.
					int mapWidth = System.Convert.ToInt32(HttpContext.Current.Request[MapBaseCommand.WidthKey]);
					int mapHeight = System.Convert.ToInt32(HttpContext.Current.Request[MapBaseCommand.HeightKey]);

					StringBuilder builder1 = new StringBuilder();
					Bitmap b = new Bitmap(mapWidth, mapHeight);
					Graphics g = Graphics.FromImage(b);

					// Append the message from exception
					builder1.Append(ex.Message);
					builder1.Append("\r\n");

					// Create stack trace for exception
					StackTrace st = new StackTrace(ex, true);
					for (int num2 = 0; num2 < st.FrameCount; num2++) {
						StackFrame frame1 = st.GetFrame(num2);
						MethodBase base1 = frame1.GetMethod();
						Type type1 = base1.DeclaringType;
						string text1 = string.Empty;
						if (type1 != null) {
							text1 = type1.Namespace;
						}
						if (text1 != null) {
							if (text1.Equals("_ASP") || text1.Equals("ASP")){
								//this._fGeneratedCodeOnStack = true;
							}
							text1 = text1 + ".";
						}
						if (type1 == null) {
							builder1.Append("   " + base1.Name + "(");
						}
						else {
							string[] textArray1 = new string[] { "   ", text1, type1.Name, ".", base1.Name, "(" } ;
							builder1.Append(string.Concat(textArray1));
						}
						ParameterInfo[] infoArray1 = base1.GetParameters();
						for (int num3 = 0; num3 < infoArray1.Length; num3++) {
							builder1.Append(((num3 != 0) ? ", " : "") + infoArray1[num3].ParameterType.Name + " " + infoArray1[num3].Name);
						}
						builder1.Append(")");
						builder1.Append("\r\n");
					}

					// write the strings into the rectangle
					g.DrawString(builder1.ToString(), new Font("Tahoma", 8), new SolidBrush(Color.Yellow), new RectangleF(0, 0, mapWidth, mapHeight), StringFormat.GenericDefault);

					// Save the bitmap into the stream to send it back
					string contentType = string.Format("text/HTML");
					if (contentType != null) HttpContext.Current.Response.ContentType = contentType;
					b.Save(HttpContext.Current.Response.OutputStream, ImageFormat.Gif);

					// End the response here
					HttpContext.Current.Response.End();
				} else {
					// Let the exception pass through
				}
			}
		}

		protected void Session_End(Object sender, EventArgs e)
		{
		}

		protected void Application_End(Object sender, EventArgs e)
		{

		}
			
		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.components = new System.ComponentModel.Container();
		}
		#endregion
	}
}

