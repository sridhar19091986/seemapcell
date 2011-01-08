using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Schema;

using MapInfo.Data;
using MapInfo.Engine;
using MapInfo.Mapping;
using MapInfo.Wfs.Client;

namespace MapInfo.Wfs.Client.Samples {

	/// <summary>
	/// Simple sample to demonstrate how to register a WfsReader to handle 
	/// requests from a specific Wfs server, get the capabilities of the server, 
	/// get the schema for a feature type located on the server and getting all 
	/// of the features from the server.
	/// </summary>
	class SimpleSample {
		private const string URL = "www.mapinfo.com/wfs";

		[STAThread]
		static void Main(string[] args) {
			// register URL with a specific WFS reader
			WfsReaderFactory.RegisterHandler(URL, typeof(WfsReader));

			// Get the WFS capabilities of the WFS server using the HTTP GET method.
			try
			{

				// Get the WFS capabilities of the WFS server using the HTTP GET method.
				WfsCapabilities Capabilities = WfsClient.GetCapabilities(RequestMethod.GET, URL);
			}
			catch
			{
				MessageBox.Show("Please check if " + URL + " is a valid WFS URL");
				return;
			}


			// Do something with the the WfsCapabilities here...

			// Get the schema for the USA feature type
			string[] TypeNames = new string[] { "miwfs:USA" };

			// Do something with the schema here...
			XmlSchema usaSchema = WfsClient.DescribeFeatureType(URL, TypeNames);

			// Get all features from the USA feature type
			MultiFeatureCollection usa = WfsClient.GetFeature(URL, TypeNames, null, null, -1, null);
			IFeatureCollection fc = usa[0];

			// iterate over the Usa MultiFeatureCollection and add each 
			// IFeatureCollection to a MemTable, etc...
			TableInfoMemTable memTableInfo = new TableInfoMemTable("myMemTable");
			foreach (Column c in fc.Columns) {
				memTableInfo.Columns.Add(c);
			}
			Table memTable = Session.Current.Catalog.CreateTable(memTableInfo);
			memTable.InsertFeatures(fc);
			
			// create a layer from the MemTable
			FeatureLayer featureLayer = new FeatureLayer(memTable);

			// create the map and add the layer
			Map map = Session.Current.MapFactory.CreateEmptyMap(new Size(500, 500));
			map.Layers.Add(featureLayer);

			// export the map to a file
			if (args.Length > 0 && args[0] != null && args[0].Trim().Length != 0) {
				using (MapExport me = new MapExport(map)) {
					me.Format = ExportFormat.Gif;
					me.Export(args[0]);
				}
			}

			// clean up the map
			Session.Current.MapFactory.Remove(map);
		}
	}
}
