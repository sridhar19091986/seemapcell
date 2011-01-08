using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Web.SessionState;

namespace LegendControlWeb
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

		protected void Application_Error(Object sender, EventArgs e)
		{
			Exception ex = Server.GetLastError();
			if (ex != null && ex.Message.Length > 0) {
				Bitmap b = new Bitmap(300, 300);
				Graphics g = Graphics.FromImage(b);
				string msg = ex.Message;
				if (ex.InnerException != null) msg+= ex.InnerException.Message;
				g.DrawString(msg, new Font("Tahoma", 10), new SolidBrush(Color.Yellow), new RectangleF(0, 0, 300, 300), StringFormat.GenericDefault);
				string contentType = string.Format("text/HTML");
				if (contentType != null) HttpContext.Current.Response.ContentType = contentType;
				b.Save(HttpContext.Current.Response.OutputStream, ImageFormat.Gif);
				HttpContext.Current.Response.End();
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

