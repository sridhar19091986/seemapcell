//===========================================================================
// This file was modified as part of an ASP.NET 2.0 Web project conversion.
// The class name was changed and the class modified to inherit from the abstract base class 
// in file 'App_Code\Migrated\Stub_WebForm1_aspx_cs.cs'.
// During runtime, this allows other classes in your web application to bind and access 
// the code-behind page using the abstract base class.
// The associated content page 'WebForm1.aspx' was also modified to refer to the new class name.
// For more information on this code pattern, please refer to http://go.microsoft.com/fwlink/?LinkId=46995 
//===========================================================================
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MapInfo;
using MapInfo.Data.Find;
using MapInfo.Data;
using MapInfo.Geometry;
using MapInfo.Styles;
using MapInfo.Mapping.Thematics;
using MapInfo.Mapping ;
using MapInfo.Mapping.Legends ;
using MapInfo.WebControls;

namespace ThematicsWeb
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public partial class Migrated_WebForm1 : WebForm1
	{
		protected System.Web.UI.WebControls.Label Label1;

		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// The first time in
			if (Session.IsNewSession)
			{
				//************************************************************//
				//*   You need to follow below lines in your own application. //
				//************************************************************//
                AppStateManager stateManager = new AppStateManager();
				// tell the state manager which map alias you want to use.
				// You could also add your own key/value pairs, the value should be serializable.
				stateManager.ParamsDictionary[AppStateManager.ActiveMapAliasKey] = this.MapControl1.MapAlias;
				// Put state manager into HttpSession, so we could get it later on from different class and requests.
                AppStateManager.PutStateManagerInSession(stateManager);

				InitState();
			}
			MapInfo.WebControls.StateManager.GetStateManagerFromSession().RestoreState();
			PrepareData();
		}

		private void Page_UnLoad(object sender, System.EventArgs e)
		{
			MapInfo.WebControls.StateManager.GetStateManagerFromSession().SaveState();
		}

		protected override void OnPreRender(EventArgs e)
		{
			if (!IsPostBack)
			{
				string[] colAlias = SampleConstants.BoundDataColumns;
				ArrayList ThemeList = new ArrayList();
				ThemesAndModifiers.FillThemeNames(ThemeList);

				DropDownList1.Items.Clear();
				DropDownList1.DataSource = ThemeList;
				DropDownList1.DataBind();

				// Prepare CheckBoxes, RadioButtons and note label contents.
				CheckBoxList1.DataSource = colAlias;
				CheckBoxList1.DataBind();
				CheckBoxList1.Visible = this.IsCheckBoxListVisible(ThemeList[0] as string);

				RadioButtonList1.DataSource = colAlias;
				RadioButtonList1.DataBind();
				RadioButtonList1.Visible = this.IsRadioButtonListVisible(ThemeList[0]as string);

				Label2.Visible = this.IsNoteLabelVisible(ThemeList[0] as string);
				Label2.Text = "Please select two columns from below checkboxes.";

				DropDownList1.Items.Clear();
				DropDownList1.DataSource = ThemeList;
				DropDownList1.DataBind();
			}
		}

		private MapInfo.Mapping.Map GetMapObj()
		{
			MapInfo.Mapping.Map myMap = MapInfo.Engine.Session.Current.MapFactory[MapControl1.MapAlias];
			if( myMap == null)
			{
				myMap = MapInfo.Engine.Session.Current.MapFactory[0];
			}
			return myMap;
		}

		private void InitState()
		{		
			MapInfo.Mapping.Map myMap = GetMapObj();

			#region Store and Restore the orignal state of the map.
			//***************************************************************************//
			//*   Store and Restore the original state of the map.                      *//
			//*   Store   - if no one puts the map into HttpSessionState yet,           *//
			//*             the map is clean, store it.                                 *//
			//*   Restore - if there is the map stored in HttpSessionState,             *//
			//*             deserialize it and apply the state of the map automatically *//
			//***************************************************************************//
			string originalMap = "original_map";
			if(this.Application[originalMap] != null)
			{
				byte[] bytes = this.Application[originalMap] as byte[];
				// This step will deserialize myMap object back and all original states will be put back to 
				// myMap if myMap has same alias name as the one stored in HttpApplicationState.
				Object obj = MapInfo.WebControls.ManualSerializer.ObjectFromBinaryStream(bytes);
			}
			else
			{
				// Set the initial zoom and center for the map
				myMap.Zoom = new MapInfo.Geometry.Distance(25000, DistanceUnit.Mile);
				myMap.Center = new DPoint(27775.805792979896,-147481.33999999985);
				// adjust the map.Size to mapcontrol's size
				myMap.Size = new System.Drawing.Size((int)MapControl1.Width.Value, (int)MapControl1.Height.Value);

				// Serialize myMap into a byte[] and store the original state of the map if no one stores it in HttpApplicationState yet.
				this.Application[originalMap] = MapInfo.WebControls.ManualSerializer.BinaryStreamFromObject(myMap);
			}
			#endregion

			// Create a GroupLayer to hold temp created label layer and object theme layer.
			// We are going to put this group layer into HttpSessionState, and it will get restored 
			// when requets come in with the same asp.net sessionID.
			if(myMap.Layers[SampleConstants.GroupLayerAlias] != null)
			{
				myMap.Layers.Remove(SampleConstants.GroupLayerAlias);
			}
			// put the GroupLayer on the top of Layers collection, so contents within it could get displayed.
			myMap.Layers.InsertGroup(0, "grouplayer", SampleConstants.GroupLayerAlias);
		}

		private void PrepareData()
		{
			MapInfo.Data.Table mdbTable = MapInfo.Engine.Session.Current.Catalog[SampleConstants.EWorldAlias];
			MapInfo.Data.Table worldTable = MapInfo.Engine.Session.Current.Catalog[SampleConstants.ThemeTableAlias];
			// worldTable is loaded by preloaded mapinfo workspace file specified in web.config.
			// and MS Access table in this sample is loaded manually.
			// we are not going to re-load it again once it got loaded because its content is not going to change in this sample.
			// we will get performance gain if we use Pooled MapInfo Session.
			// Note: It's better to put this MS Access table into pre-loaded workspace file, 
			//       so we don't need to do below code.
			//       We manually load this MS Access in this sample for demonstration purpose.
			if(mdbTable == null)
			{
				System.Web.HttpServerUtility util = HttpContext.Current.Server;
				string dataPath = util.MapPath("");
				mdbTable = MapInfo.Engine.Session.Current.Catalog.OpenTable(System.IO.Path.Combine(dataPath, SampleConstants.EWorldTabFileName));
			
				string[] colAlias = SampleConstants.BoundDataColumns;
				// DateBinding columns
				Column col0 = MapInfo.Data.ColumnFactory.CreateDoubleColumn(colAlias[0]);
				col0.ColumnExpression = mdbTable.Alias + "." + colAlias[0];
				Column col1 = MapInfo.Data.ColumnFactory.CreateIntColumn(colAlias[1]);
				col1.ColumnExpression = mdbTable.Alias + "." + colAlias[1];
				Column col2 = MapInfo.Data.ColumnFactory.CreateIntColumn(colAlias[2]);
				col2.ColumnExpression = mdbTable.Alias + "." + colAlias[2];

				Columns cols = new Columns();
				cols.Add(col0);
				cols.Add(col1);
				cols.Add(col2);
				
				// Databind MS Access table data to existing worldTable.
				worldTable.AddColumns(cols, BindType.DynamicCopy, mdbTable, SampleConstants.SouceMatchColumn, Operator.Equal, SampleConstants.TableMatchColumn);
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			this.Unload += new System.EventHandler(this.Page_UnLoad);
			base.OnInit(e);
			this.ApplyButton.Attributes.Add("onClick","javascript:" + 
				ApplyButton.ClientID + ".disabled=true;" + 
				this.GetPostBackEventReference(ApplyButton));
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		protected void ApplyButton_Click(object sender, System.EventArgs e)
		{
			if(ValidateThemeAndModifierParams(DropDownList1.SelectedValue))
			{
				if(Label2.Visible)
				{
					Label2.BackColor = Color.LightGray;
				}
				CreateThemeOrModifier(DropDownList1.SelectedValue); 
				HandleLabelLayerVisibleStatus(this.GetMapObj());
			}
			else
			{
				Label2.BackColor = Color.Red;
			}
		}

		protected void DropDownList1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			PrepareThemeAndModifierOptions(DropDownList1.SelectedValue);
			RadioButtonList1.ClearSelection();
			CheckBoxList1.ClearSelection();
			CleanUp(GetMapObj());
		}

		#region Methods to handle Themes, Modifiers and some other params to construct themes/modifiers.
		// Disable other LabelLayers if we demo a new LabelLayer with theme or modifier sample.
		// so User could see theme/modifier label layer clearly.
//		public static void HandleLabelLayerVisibleStatus(Map map)
//		{
//			if(map == null) return;
//			for(int index=0; index < map.Layers.Count; index++)
//			{
//				IMapLayer lyr = map.Layers[index];
//				if(lyr is LabelLayer)
//				{
//					LabelLayer ll = lyr as LabelLayer;
//					if(map.Layers[SampleConstants.NewLabelLayerAlias] != null 
//						&& ll.Alias != SampleConstants.NewLabelLayerAlias)
//					{
//						ll.Enabled = false;
//					}
//					else
//					{
//						ll.Enabled = true;
//					}
//				}
//			}
//		}

		// Create all MapXtreme.Net themes and modifiers for the bound data
		// and add them into the corresponding Map object.
		private void CreateThemeOrModifier(string themeName)
		{
			Map myMap = GetMapObj();
			if(myMap == null)return;
			// Clean up all temp themes or modifiers from the Map object.
			this.CleanUp(myMap);

			FeatureLayer fLyr = myMap.Layers[SampleConstants.ThemeLayerAlias] as FeatureLayer;
			ThemeAndModifierTypes themeType = ThemesAndModifiers.GetThemeAndModifierTypesByName(themeName);

			string alias = ConstructThemeAlias(themeType);
			switch(themeType)
			{
				case ThemeAndModifierTypes.RangedTheme:
					RangedTheme rt = new RangedTheme(fLyr, GetThemeOrModifierExpression(), alias, 5, MapInfo.Mapping.Thematics.DistributionMethod.EqualCountPerRange);
					fLyr.Modifiers.Append(rt);
					break;
				case ThemeAndModifierTypes.DotDensityTheme:
					DotDensityTheme ddt = new DotDensityTheme(fLyr, GetThemeOrModifierExpression(), alias, Color.Purple, DotDensitySize.Large);
					ddt.ValuePerDot = 2000000d;
					fLyr.Modifiers.Append(ddt);
					break;
				case ThemeAndModifierTypes.IndividualValueTheme:
					IndividualValueTheme ivt = new IndividualValueTheme(fLyr, GetThemeOrModifierExpression(), alias);
					fLyr.Modifiers.Append(ivt);
					break;
				case ThemeAndModifierTypes.PieTheme:
					PieTheme pt = new PieTheme(myMap, fLyr.Table, GetThemeOrModifierExpressions());
					ObjectThemeLayer otlPt = new ObjectThemeLayer("PieTheme", alias, pt);
					GetTheGroupLayer().Insert(0, otlPt);
					break;
				case ThemeAndModifierTypes.BarTheme:
					BarTheme bt = new BarTheme(myMap, fLyr.Table, GetThemeOrModifierExpressions());
                    bt.DataValueAtSize = 10000000;
                    bt.Size = new MapInfo.Engine.PaperSize(0.1, 0.1, MapInfo.Geometry.PaperUnit.Inch);
                    bt.Width = new MapInfo.Engine.PaperSize(0.1, MapInfo.Geometry.PaperUnit.Inch);
					ObjectThemeLayer otlBt = new ObjectThemeLayer("BarTheme", alias, bt);

					GetTheGroupLayer().Insert(0, otlBt);
					break;
				case ThemeAndModifierTypes.GraduatedSymbolTheme:
					GraduatedSymbolTheme gst = new GraduatedSymbolTheme(fLyr.Table, GetThemeOrModifierExpression());
					ObjectThemeLayer otlGst = new ObjectThemeLayer("GraduatedSymbolTheme", alias, gst);
					GetTheGroupLayer().Insert(0, otlGst);
					break;
				case ThemeAndModifierTypes.FeatureOverrideStyleModifier:
					FeatureOverrideStyleModifier fosm = new FeatureOverrideStyleModifier("OverrideTheme", alias);
					
					SimpleInterior fs = new SimpleInterior((SimpleInterior.MaxFillPattern - SimpleInterior.MinFillPattern) / 2, Color.FromArgb(10, 23, 90), Color.FromArgb(33, 35, 35), false);
					fs.SetApplyAll();
					SimpleLineStyle lineStyle = new SimpleLineStyle(new LineWidth(2, LineWidthUnit.Point), (SimpleLineStyle.MaxLinePattern - SimpleLineStyle.MinLinePattern) /2);
					lineStyle.Color = Color.FromArgb(111, 150, 230);
					lineStyle.Interleaved = false;
					lineStyle.SetApplyAll();

					fosm.Style.AreaStyle = new AreaStyle(lineStyle, fs);
					fLyr.Modifiers.Append(fosm);
					break;
				case ThemeAndModifierTypes.Label_IndividualValueLabelTheme:
					IndividualValueLabelTheme ivlt = new IndividualValueLabelTheme(fLyr.Table, GetThemeOrModifierExpression(), alias);
					CreateLabelLayer(myMap, fLyr.Table, GetThemeOrModifierExpression()).Sources[0].Modifiers.Append(ivlt);
					break;
				case ThemeAndModifierTypes.Label_RangedLabelTheme:
					RangedLabelTheme rlt = new RangedLabelTheme(fLyr.Table, GetThemeOrModifierExpression(), alias, 5, DistributionMethod.EqualCountPerRange);
					CreateLabelLayer(myMap, fLyr.Table, GetThemeOrModifierExpression()).Sources[0].Modifiers.Append(rlt);
					break;
				case ThemeAndModifierTypes.Label_OverrideLabelModifier:
                    OverrideLabelModifier olm = new OverrideLabelModifier(alias, "OverrideLabelModifier");
					MapInfo.Styles.Font font = new MapInfo.Styles.Font("Arial", 24, Color.Red, Color.Yellow, FontFaceStyle.Italic, 
						FontWeight.Bold, TextEffect.Halo, TextDecoration.All, TextCase.AllCaps, true, true);

					font.Attributes = StyleAttributes.FontAttributes.All;

					olm.Properties.Style = new TextStyle(font);
					olm.Properties.Caption = GetThemeOrModifierExpression();
					CreateLabelLayer(myMap, fLyr.Table, GetThemeOrModifierExpression()).Sources[0].Modifiers.Append(olm);
					break;
				default:
					break;
			}
		}


		private MapInfo.Mapping.GroupLayer GetTheGroupLayer()
		{
			MapInfo.Mapping.Map myMap = this.GetMapObj();
			if(myMap.Layers[SampleConstants.GroupLayerAlias] == null)
			{
				myMap.Layers.InsertGroup(0, "grouplayer", SampleConstants.GroupLayerAlias);
			}
			return myMap.Layers[SampleConstants.GroupLayerAlias] as MapInfo.Mapping.GroupLayer;
		}

		// Create a LabelLayer and add it into the Group Layer collection with index 0.
		private LabelLayer CreateLabelLayer(Map myMap, MapInfo.Data.Table table, string caption)
		{
			LabelLayer ll = new LabelLayer("Label Layer for bound data", SampleConstants.NewLabelLayerAlias);
			// Insert this LabelLayer into the GroupLayer
			GroupLayer gLyr = GetTheGroupLayer();
			gLyr.Insert(0, ll);
			
			LabelSource ls = new LabelSource(table);
			ls.DefaultLabelProperties.Caption = caption;
			ll.Sources.Append(ls);
			return ll;
		}

		// Remove all created temp themes, modifers and label layers created by users.
		private void CleanUp(Map myMap)
		{
			FeatureLayer fLyr = myMap.Layers[SampleConstants.ThemeLayerAlias] as FeatureLayer;
			// Remove themes/modifiers one bye one instead of using Clear()
			RemoveTheme(fLyr, this.ConstructThemeAlias(ThemeAndModifierTypes.RangedTheme));
			RemoveTheme(fLyr, this.ConstructThemeAlias(ThemeAndModifierTypes.IndividualValueTheme));
			RemoveTheme(fLyr, this.ConstructThemeAlias(ThemeAndModifierTypes.DotDensityTheme));
			RemoveTheme(fLyr, this.ConstructThemeAlias(ThemeAndModifierTypes.FeatureOverrideStyleModifier));

			// Clear the group layer.
			MapInfo.Mapping.GroupLayer groupLyr = GetTheGroupLayer();
			if(groupLyr != null)
			{
				groupLyr.Clear();
			}
		}

		// Remove Themes from a FeatureLayer.
		private void RemoveTheme(FeatureLayer lyr, string themeAlias)
		{
			if(lyr.Modifiers[themeAlias] != null)
			{
				lyr.Modifiers.Remove(themeAlias);
			}
		}

		// This is for theme or modifier which only requires one expression.
		private string GetThemeOrModifierExpression()
		{
			return RadioButtonList1.SelectedValue;
		}

		// This is for Pie, Bar themes.
		private string[] GetThemeOrModifierExpressions()
		{
			ArrayList cols = new ArrayList(2);
			for(int index=0; index < CheckBoxList1.Items.Count; index++)
			{
				if(CheckBoxList1.Items[index].Selected)
				{
					cols.Add(CheckBoxList1.Items[index].Value);
				}
			}
			return (string[])cols.ToArray(typeof(string));
		}

		// Validate user's inputs.
		private bool ValidateThemeAndModifierParams(string themeName)
		{
			ThemeAndModifierTypes themeType = ThemesAndModifiers.GetThemeAndModifierTypesByName(themeName);
			switch(themeType)
			{
				case ThemeAndModifierTypes.RangedTheme:
				case ThemeAndModifierTypes.IndividualValueTheme:
				case ThemeAndModifierTypes.DotDensityTheme:
				case ThemeAndModifierTypes.GraduatedSymbolTheme:
				case ThemeAndModifierTypes.Label_IndividualValueLabelTheme:
				case ThemeAndModifierTypes.Label_RangedLabelTheme:
				case ThemeAndModifierTypes.Label_OverrideLabelModifier:
					// Need to select one column, otherwise we can not proceed it.
					if(RadioButtonList1.SelectedItem == null)
					{
						return false;
					}
					return true;
				case ThemeAndModifierTypes.BarTheme:
				case ThemeAndModifierTypes.PieTheme:
					// Need to select two columns.
					int selectedItemsSum = 0;
					// this is required to customer to select 2 cols.
					for(int index=0; index < CheckBoxList1.Items.Count; index++)
					{
						if(CheckBoxList1.Items[index].Selected)
						{
							selectedItemsSum++;
						}
					}
					if(selectedItemsSum != 2)
					{
						return false;
					}
					return true;
				case ThemeAndModifierTypes.FeatureOverrideStyleModifier:
					return true;
			}
			return false;
		}

		// Decide if the RadioButtonListVisible should be visible or not.
		private bool IsRadioButtonListVisible(string themeName)
		{
			ThemeAndModifierTypes themeType = ThemesAndModifiers.GetThemeAndModifierTypesByName(themeName);
			switch(themeType)
			{
				case ThemeAndModifierTypes.RangedTheme:
				case ThemeAndModifierTypes.IndividualValueTheme:
				case ThemeAndModifierTypes.DotDensityTheme:
				case ThemeAndModifierTypes.GraduatedSymbolTheme:
				case ThemeAndModifierTypes.Label_IndividualValueLabelTheme:
				case ThemeAndModifierTypes.Label_RangedLabelTheme:
				case ThemeAndModifierTypes.Label_OverrideLabelModifier:
					return true;
			}
			return false;
		}

		// Decide if the CheckBoxList should be visible or not.
		private bool IsCheckBoxListVisible(string themeName)
		{
			ThemeAndModifierTypes themeType = ThemesAndModifiers.GetThemeAndModifierTypesByName(themeName);
			switch(themeType)
			{
				case ThemeAndModifierTypes.BarTheme:
				case ThemeAndModifierTypes.PieTheme:
					return true;
			}
			return false;
		}

		// Decide if the note message should be displayed or not.
		private bool IsNoteLabelVisible(string themeName)
		{
			ThemeAndModifierTypes themeType = ThemesAndModifiers.GetThemeAndModifierTypesByName(themeName);
			switch(themeType)
			{
				case ThemeAndModifierTypes.BarTheme:
				case ThemeAndModifierTypes.PieTheme:
					return true;
			}
			return false;
		}

		// Decide what kind of infos shoud be displayed on the page.
		private void PrepareThemeAndModifierOptions(string themeName)
		{
			RadioButtonList1.Visible = this.IsRadioButtonListVisible(themeName);
			CheckBoxList1.Visible = this.IsCheckBoxListVisible(themeName);
			Label2.BackColor = Color.LightGray;
			Label2.Visible = this.IsNoteLabelVisible(themeName);
		}

		private string ConstructThemeAlias(ThemeAndModifierTypes themeType)
		{
			return themeType.ToString() + "_alias";
		}
		#endregion
	}
}
