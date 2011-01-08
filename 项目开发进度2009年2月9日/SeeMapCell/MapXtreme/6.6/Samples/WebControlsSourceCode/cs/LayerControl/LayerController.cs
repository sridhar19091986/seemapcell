using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MapInfo.WebControls
{
	/// <summary>
	/// Handler for the LayerController.
	/// </summary>
	/// <remarks>The HTML for this control is generated on demand when a map changes because the user interface contains values that 
	/// need updating when the map changes.</remarks>
	public class LayerController : IHttpHandler, IRequiresSessionState
	{
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
		/// Processes requests for the LayerControl.
		/// </summary>
		/// <param name="context">HttpContext</param>
		/// <remarks>This method parses the URL. Gets the model for the LayerControl from the session and then uses that to get the XML. Then the HTML is generated
		/// by using XSLT transform and returnes back as response.
		/// </remarks>
		public void ProcessRequest(HttpContext context)
		{
			string _command = context.Request["Command"];
			string _uniqueID = context.Request["UniqueID"];
			string _mapAlias = context.Request["MapAlias"];
			string _mapControlID = context.Request["MapControlID"];
			// Get Xslt file name
			string _xsltFile = context.Request["XsltFile"];
			// Get the resource path name passed as data and properly encode it.
			byte[] buffer = new byte[context.Request.InputStream.Length];
			int read = context.Request.InputStream.Read(buffer, 0, (int)context.Request.InputStream.Length);
			string _resourcePath = System.Text.UTF8Encoding.UTF8.GetString( buffer );
			string _imgPath = _resourcePath;
			// Form the full path for xslt file
			string _xsltPath = context.Server.MapPath(_resourcePath) + "\\" + _xsltFile;
			LayerControlModel layerModel = LayerControlModel.SetDefaultModelInSession();

			switch(_command) {
				case "GetHTML":
					XmlDocument treeDoc = layerModel.GetLayerXML(_mapAlias,  _uniqueID, _imgPath);

					// load the xslt file into reader
					System.IO.TextReader tr = new System.IO.StreamReader(_xsltPath);
					MemoryStream ms = new MemoryStream();
					StreamWriter sw = new StreamWriter(ms);
					// Now replace the tags in the xslt file with actual strings from resources
					string line = "";
					while((line = tr.ReadLine()) != null) {
						Regex regexp = new Regex("(Treeview2_([0-9]+))");
						MatchCollection mc = regexp.Matches(line);
						for (int i=mc.Count-1; i>=0;i--) {
							Match m = mc[i];
							Group g = m.Groups[0];
							Capture c = m.Groups[0].Captures[0];
							string resName = line.Substring(c.Index, c.Length);
							if (resName.Equals("Treeview2_3")) {
								line = line.Replace(resName, Resources.ResourceFolder);
							} else {
								string resString = L10NUtils.Resources.GetString(resName);
								line = line.Replace(resName, resString);
							}
						}
						sw.WriteLine(line);
					}

					sw.Flush();
					ms.Position = 0;

					// Use that TextReader as the Source for the XmlTextReader
					System.Xml.XmlReader xr = new System.Xml.XmlTextReader(ms);
					// Create a new XslTransform class
					System.Xml.Xsl.XslTransform treeView = new System.Xml.Xsl.XslTransform();
					// Load the XmlReader StyleSheet into the XslTransform class
					treeView.Load(xr, null, null);

					StringWriter sw2 = new StringWriter();

					XPathNavigator nav = treeDoc.CreateNavigator();

					// Do the transform and write to response
					treeView.Transform(nav, null,  sw2,  null);
					context.Response.Write(sw2.ToString());
					tr.Close();
					sw.Close();
					ms.Close();
					sw2.Close();
					
					/*
					XmlDocument treeDoc = layerModel.GetLayerXML(_mapAlias,  _uniqueID, _imgPath);

					XslTransform treeView = new XslTransform();
					treeView.Load(_xsltPath);
   
					StringWriter sw = new StringWriter();

					XPathNavigator nav = treeDoc.CreateNavigator();

					// Do the transform and write to response
					treeView.Transform(nav, null,  sw,  null);
					context.Response.Write(sw.ToString());
*/					
					break;
			}
		}
	}
}
