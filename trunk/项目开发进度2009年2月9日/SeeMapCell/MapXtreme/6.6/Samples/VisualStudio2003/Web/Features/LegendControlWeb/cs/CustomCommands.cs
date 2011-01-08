using System;
using System.Drawing;
using System.IO;
using System.Web;
using MapInfo.Mapping;
using MapInfo.Mapping.Legends;
using MapInfo.Mapping.Thematics;
using MapInfo.WebControls;
using MapInfo.Styles;
using Font = MapInfo.Styles.Font;

namespace LegendControlWeb
{
	/// <summary>
	/// some constants used in this project
	/// </summary>
	public class ProjectConstants
	{
		public static readonly string ThemeLayerAlias = "world";
		public static readonly string ThemeAlias = "worldtheme";
		public static readonly string IndColumnName = "Country";
		
	}

	/// <summary>
	/// CreateTheme command for creating individual theme and legend.
	/// </summary>
	[Serializable]
	public class CreateTheme  : MapInfo.WebControls.MapBaseCommand
	{
		/// <summary>
		/// Constructor for CreateTheme class
		/// </summary>
		/// 
		public CreateTheme()
		{
			Name = "CreateTheme";
		}

		
		public override void Process()
		{
			MapControlModel model = MapControlModel.GetModelFromSession();
			model.SetMapSize(MapAlias, MapWidth, MapHeight);
			
			//get map object from map model
			MapInfo.Mapping.Map map = model.GetMapObj(MapAlias);
			FeatureLayer fLyr = map.Layers[ProjectConstants.ThemeLayerAlias] as FeatureLayer;
			
			fLyr.Modifiers.Clear();
			map.Legends.Clear();
			
			//create theme
			IndividualValueTheme iTheme = new IndividualValueTheme(fLyr, ProjectConstants.IndColumnName, ProjectConstants.ThemeAlias);
			fLyr.Modifiers.Insert(0, iTheme);			
			
			//create legend based on the individual value theme
			Legend lg = map.Legends.CreateLegend(new Size(236, 282));
			
			//create legend frame
			ThemeLegendFrame lgFrame = LegendFrameFactory.CreateThemeLegendFrame(iTheme);						
			lg.Frames.Append(lgFrame);
			
			//modify legend frame style
			lgFrame.BackgroundBrush = new SolidBrush(Color.AliceBlue);
			lgFrame.Title = "World Country Legend";
			lgFrame.SubTitle = " ";
			
			MapInfo.Styles.Font titleFont = new MapInfo.Styles.Font("Arial", 10);
			titleFont.ForeColor = Color.DarkBlue;
			titleFont.FontWeight = FontWeight.Bold;
			
			lgFrame.TitleStyle = titleFont;

			MapInfo.Styles.Font rowTextStyle = new Font("Arial", 8);
			rowTextStyle.FontWeight = FontWeight.Bold;
																
			lgFrame.RowTextStyle = rowTextStyle;
			
			//stream map image back to client
			MemoryStream ms = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat);
			StreamImageToClient(ms);
		}
	}

	/// <summary>
	/// RemoveTheme command for removing all themes from the map.
	/// </summary>
	[Serializable]
	public class RemoveTheme  : MapInfo.WebControls.MapBaseCommand
	{
		/// <summary>
		/// Constructor for RemoveTheme class
		/// </summary>
		/// 
		public RemoveTheme()
		{
			Name = "RemoveTheme";
		}

		
		public override void Process()
		{
			MapControlModel model = MapControlModel.GetModelFromSession();
			model.SetMapSize(MapAlias, MapWidth, MapHeight);
			
			//get map object from map model
			MapInfo.Mapping.Map map = model.GetMapObj(MapAlias);
			FeatureLayer fLyr = map.Layers[ProjectConstants.ThemeLayerAlias] as FeatureLayer;			
			fLyr.Modifiers.Clear();
			map.Legends.Clear();			
			
			MemoryStream ms = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat);
			StreamImageToClient( ms);
		}
	}

	/// <summary>
	/// GetLegend command for streaming legend image back to client.
	/// </summary>
	[Serializable]
	public class GetLegend  : MapInfo.WebControls.MapBaseCommand
	{
		
		//key to get LegendExportFormat value from client request
		public readonly string LegendExportFormatKey = "LegendExportFormat";

		/// <summary>
		/// Constructor for CreateTheme class
		/// </summary>
		public GetLegend()
		{
			Name = "GetLegend";
		}

		/// <summary>
		/// Override parent ParseContext method and add assigning value to LegendExportFormat property.
		/// </summary>
		public override void ParseContext()
		{
			base.ParseContext ();
			LegendExportFormat = HttpContext.Current.Request[LegendExportFormatKey];
		}

		/// <summary>
		/// Override the Execute method in MapBasicCommand class to not save state, because
		/// for legend control, which does not change map state, so there is no need to save map state.
		/// </summary>
		public override void Execute()
		{
			
			StateManager sm = StateManager.GetStateManagerFromSession();
			if (sm == null) 
			{
				if(StateManager.IsManualState())
				{
					throw new NullReferenceException("Cannot find instance of StateManager in the ASP.NET session.");
				}
			} 
			ParseContext();
			if(sm != null)
			{
				PrepareStateManagerParamsDictionary(sm);
				sm.RestoreState();
			}

			Process();
		}

		
		public override void Process()
		{
			MapControlModel model = MapControlModel.GetModelFromSession();
						
			//get map object from map model
			MapInfo.Mapping.Map map = model.GetMapObj(MapAlias);

			if(map.Legends.Count == 0)
				return;

			Legend legend = map.Legends[0];

			LegendExport legendExp = new LegendExport(map, legend);			
			legendExp.Format = (MapInfo.Mapping.ExportFormat)MapInfo.Mapping.ExportFormat.Parse(typeof(ExportFormat),LegendExportFormat, true);
						
			//export Legend to memorystream
			MemoryStream stream = new MemoryStream();
			legendExp.Export(stream);
			stream.Position = 0;			
			legendExp.Dispose();

			//stream legend image back to client
			StreamImageToClient( stream);
		}

		/// <summary>
		/// Stream legend image in memory stream back to client.
		/// </summary>
		/// <param name="ms">memory stream holding legend image</param>
		public override void StreamImageToClient(MemoryStream ms) 
		{
			if (ms != null) 
			{
				string contentType = string.Format("image/{0}", LegendExportFormat);
				BinaryReader reader = new BinaryReader(ms);
				int length = (int)ms.Length;
				if (contentType != null)    HttpContext.Current.Response.ContentType = contentType;
				HttpContext.Current.Response.OutputStream.Write(reader.ReadBytes(length), 0, length);
				reader.Close();
				ms.Close();
			}
		}

		private string _legendExportFormat;
		public string LegendExportFormat
		{
			get { return _legendExportFormat ;}
			set { _legendExportFormat = value; }
		} 
	}	
}
