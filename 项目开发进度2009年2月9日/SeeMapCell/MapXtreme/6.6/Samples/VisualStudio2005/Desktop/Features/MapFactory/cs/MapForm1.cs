using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Reflection;
using MapInfo.Data;
using MapInfo.Mapping;
using MapInfo.Engine;
using MapInfo.Windows.Dialogs;

namespace MapFactory
{
	/// <summary>
	/// Summary description for MapForm1.
	/// </summary>
	public class MapForm1 : System.Windows.Forms.Form
	{
		private MapInfo.Windows.Controls.MapControl mapControl1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.ComboBox comboBoxMaps;
		private System.Windows.Forms.Button RemoveMap;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button NewEmptyMap;
		private System.Windows.Forms.Button BtnNewMap;
		private System.Windows.Forms.Button ClearMaps;
		private System.Windows.Forms.Button MapFromFile;
		private MapInfo.Windows.Controls.MapToolBar mapToolBar1;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonOpenTable;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonLayerControl;
		private System.Windows.Forms.ToolBarButton toolBarButtonSeparator;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonSelect;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomIn;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomOut;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonPan;
		private System.ComponentModel.Container components = null;

		public MapForm1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Listen to some map events
			mapControl1.Map.ViewChangedEvent += new ViewChangedEventHandler(Map_ViewChanged);

			// Extra setup
			UpdateMapComboBox();
	}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mapControl1 = new MapInfo.Windows.Controls.MapControl();
			this.panel1 = new System.Windows.Forms.Panel();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.comboBoxMaps = new System.Windows.Forms.ComboBox();
			this.RemoveMap = new System.Windows.Forms.Button();
			this.BtnNewMap = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.MapFromFile = new System.Windows.Forms.Button();
			this.ClearMaps = new System.Windows.Forms.Button();
			this.NewEmptyMap = new System.Windows.Forms.Button();
			this.mapToolBar1 = new MapInfo.Windows.Controls.MapToolBar();
			this.mapToolBarButtonOpenTable = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonLayerControl = new MapInfo.Windows.Controls.MapToolBarButton();
			this.toolBarButtonSeparator = new System.Windows.Forms.ToolBarButton();
			this.mapToolBarButtonSelect = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonZoomIn = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonZoomOut = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonPan = new MapInfo.Windows.Controls.MapToolBarButton();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mapControl1
			// 
			this.mapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mapControl1.Location = new System.Drawing.Point(0, 0);
			this.mapControl1.Name = "mapControl1";
			this.mapControl1.Size = new System.Drawing.Size(408, 284);
			this.mapControl1.TabIndex = 0;
			this.mapControl1.Text = "mapControl1";
			this.mapControl1.Tools.MiddleButtonTool = null;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Controls.Add(this.mapControl1);
			this.panel1.Location = new System.Drawing.Point(4, 32);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(412, 288);
			this.panel1.TabIndex = 1;
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 322);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(584, 19);
			this.statusBar1.TabIndex = 2;
			// 
			// comboBoxMaps
			// 
			this.comboBoxMaps.Location = new System.Drawing.Point(200, 8);
			this.comboBoxMaps.Name = "comboBoxMaps";
			this.comboBoxMaps.Size = new System.Drawing.Size(121, 21);
			this.comboBoxMaps.TabIndex = 4;
			this.comboBoxMaps.Text = "comboBoxMaps";
			this.comboBoxMaps.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// RemoveMap
			// 
			this.RemoveMap.Location = new System.Drawing.Point(328, 7);
			this.RemoveMap.Name = "RemoveMap";
			this.RemoveMap.Size = new System.Drawing.Size(80, 23);
			this.RemoveMap.TabIndex = 5;
			this.RemoveMap.Text = "Remove Map";
			this.RemoveMap.Click += new System.EventHandler(this.RemoveMap_Click);
			// 
			// BtnNewMap
			// 
			this.BtnNewMap.Location = new System.Drawing.Point(16, 32);
			this.BtnNewMap.Name = "BtnNewMap";
			this.BtnNewMap.Size = new System.Drawing.Size(120, 23);
			this.BtnNewMap.TabIndex = 6;
			this.BtnNewMap.Text = "New Map Dialog";
			this.BtnNewMap.Click += new System.EventHandler(this.button2_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.MapFromFile);
			this.groupBox1.Controls.Add(this.ClearMaps);
			this.groupBox1.Controls.Add(this.NewEmptyMap);
			this.groupBox1.Controls.Add(this.BtnNewMap);
			this.groupBox1.Location = new System.Drawing.Point(424, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(152, 312);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "MapFactory Methods";
			// 
			// MapFromFile
			// 
			this.MapFromFile.Location = new System.Drawing.Point(16, 112);
			this.MapFromFile.Name = "MapFromFile";
			this.MapFromFile.Size = new System.Drawing.Size(120, 23);
			this.MapFromFile.TabIndex = 9;
			this.MapFromFile.Text = "New Map From File";
			this.MapFromFile.Click += new System.EventHandler(this.MapFromFile_Click);
			// 
			// ClearMaps
			// 
			this.ClearMaps.Location = new System.Drawing.Point(16, 152);
			this.ClearMaps.Name = "ClearMaps";
			this.ClearMaps.Size = new System.Drawing.Size(120, 23);
			this.ClearMaps.TabIndex = 8;
			this.ClearMaps.Text = "Clear Maps";
			this.ClearMaps.Click += new System.EventHandler(this.ClearMaps_Click);
			// 
			// NewEmptyMap
			// 
			this.NewEmptyMap.Location = new System.Drawing.Point(16, 72);
			this.NewEmptyMap.Name = "NewEmptyMap";
			this.NewEmptyMap.Size = new System.Drawing.Size(120, 23);
			this.NewEmptyMap.TabIndex = 7;
			this.NewEmptyMap.Text = "New Empty Map";
			this.NewEmptyMap.Click += new System.EventHandler(this.NewEmptyMap_Click);
			// 
			// mapToolBar1
			// 
			this.mapToolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						   this.mapToolBarButtonOpenTable,
																						   this.mapToolBarButtonLayerControl,
																						   this.toolBarButtonSeparator,
																						   this.mapToolBarButtonSelect,
																						   this.mapToolBarButtonZoomIn,
																						   this.mapToolBarButtonZoomOut,
																						   this.mapToolBarButtonPan});
			this.mapToolBar1.Divider = false;
			this.mapToolBar1.Dock = System.Windows.Forms.DockStyle.None;
			this.mapToolBar1.DropDownArrows = true;
			this.mapToolBar1.Location = new System.Drawing.Point(6, 6);
			this.mapToolBar1.MapControl = this.mapControl1;
			this.mapToolBar1.Name = "mapToolBar1";
			this.mapToolBar1.ShowToolTips = true;
			this.mapToolBar1.Size = new System.Drawing.Size(187, 26);
			this.mapToolBar1.TabIndex = 9;
			// 
			// mapToolBarButtonOpenTable
			// 
			this.mapToolBarButtonOpenTable.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.OpenTable;
			this.mapToolBarButtonOpenTable.ToolTipText = "Open Table";
			// 
			// mapToolBarButtonLayerControl
			// 
			this.mapToolBarButtonLayerControl.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.LayerControl;
			this.mapToolBarButtonLayerControl.ToolTipText = "Layer Control";
			// 
			// toolBarButtonSeparator
			// 
			this.toolBarButtonSeparator.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// mapToolBarButtonSelect
			// 
			this.mapToolBarButtonSelect.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Select;
			this.mapToolBarButtonSelect.ToolTipText = "Select";
			// 
			// mapToolBarButtonZoomIn
			// 
			this.mapToolBarButtonZoomIn.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomIn;
			this.mapToolBarButtonZoomIn.ToolTipText = "Zoom-in";
			// 
			// mapToolBarButtonZoomOut
			// 
			this.mapToolBarButtonZoomOut.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomOut;
			this.mapToolBarButtonZoomOut.ToolTipText = "Zoom-out";
			// 
			// mapToolBarButtonPan
			// 
			this.mapToolBarButtonPan.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Pan;
			this.mapToolBarButtonPan.ToolTipText = "Pan";
			// 
			// MapForm1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(584, 341);
			this.Controls.Add(this.mapToolBar1);
			this.Controls.Add(this.RemoveMap);
			this.Controls.Add(this.comboBoxMaps);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.groupBox1);
			this.MinimumSize = new System.Drawing.Size(592, 248);
			this.Name = "MapForm1";
			this.Text = "MapForm1";
			this.panel1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MapForm1());
		}

		// Handler function called when the active map's view changes
		private void Map_ViewChanged(object o, ViewChangedEventArgs e) {
			// Get the map
			MapInfo.Mapping.Map map = (MapInfo.Mapping.Map) o;
			// Display the zoom level
			Double dblZoom = System.Convert.ToDouble(String.Format("{0:E2}", mapControl1.Map.Zoom.Value));
			statusBar1.Text = "Zoom: " + dblZoom.ToString() + " " + mapControl1.Map.Zoom.Unit.ToString();
		}

		private void UpdateMapComboBox()
		{
			comboBoxMaps.Items.Clear();
			foreach (Map m in Session.Current.MapFactory)
			{
				comboBoxMaps.Items.Add(m);
			}
			comboBoxMaps.SelectedItem = mapControl1.Map;
		}

		private void SetMap(Map map)
		{
			if (mapControl1.Map != null) 
			{
				mapControl1.Map.ViewChangedEvent -= new ViewChangedEventHandler(Map_ViewChanged);
			}

			mapControl1.Map = map;

			if (map != null) 
			{
				mapControl1.Map.ViewChangedEvent += new ViewChangedEventHandler(Map_ViewChanged);
				mapControl1.Enabled = true;
				mapControl1.Map.IncrementalDraw = new IncrementalDraw(200, 100);

				// hook up the toolbar to the current map
				mapToolBar1.MapControl=mapControl1;
			}
			else 
			{
				mapControl1.Enabled = false;
			}
		}

		private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
				SetMap(comboBoxMaps.SelectedItem as Map);
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			int n=0;
			Table []tables = null;
			ITableEnumerator tenum = Session.Current.Catalog.EnumerateTables(TableFilterFactory.FilterMappableTables());
			while (tenum.MoveNext())
			{
				n++;
			}
			tables = new Table[n];
			n=0;
			foreach (Table t in tenum)
			{
				tables[n++]=t;
			}

			NewMapDlg dlg;
			if (n > 0) 
			{
				dlg = new NewMapDlg(tables);
				if (dlg.ShowDialog() == DialogResult.OK) 
				{
					// build map using MapTableLoader
					tables = dlg.SelectedTables;
					n = dlg.SelectionCount;
				}
				else 
				{
					return;
				}
			}
			// now build map from tables if any
			Map m;
			if (n == 0)
			{
				m = Session.Current.MapFactory.CreateEmptyMap(null, null, IntPtr.Zero, mapControl1.Size);
			}
			else 
			{
				m = Session.Current.MapFactory.Create(IntPtr.Zero, mapControl1.Size, new MapTableLoader(tables));
			}
			SetMap(m);
			UpdateMapComboBox();
		}

		private void RemoveMap_Click(object sender, System.EventArgs e)
		{
			Map m = mapControl1.Map;
			int n=(Session.Current.MapFactory as IList).IndexOf(m);
			int nNew = n-1;
			if (nNew < 0) nNew = 1; // deleting 0, so new is 1.
			if (Session.Current.MapFactory.Count == 1) // deleting last map 
			{
				SetMap(Session.Current.MapFactory.CreateEmptyMap(null, null, (IntPtr)0, new Size(0,0)));
			}
			else 
			{
				SetMap(Session.Current.MapFactory[nNew]);
			}
			Session.Current.MapFactory.Remove(m);
			UpdateMapComboBox();
		}

		private void NewEmptyMap_Click(object sender, System.EventArgs e)
		{
			Map map = Session.Current.MapFactory.CreateEmptyMap(null, null, IntPtr.Zero, mapControl1.Size);
			SetMap(map);
			UpdateMapComboBox();
		}

		private void ClearMaps_Click(object sender, System.EventArgs e)
		{
			Session.Current.MapFactory.Clear();
			// create new empty map
			Map map = Session.Current.MapFactory.CreateEmptyMap(null, null, IntPtr.Zero, mapControl1.Size);
			SetMap(map);
			UpdateMapComboBox();
		}

		private void MapFromFile_Click(object sender, System.EventArgs e)
		{
			DoMapfromFile();
		}

		private void DoMapfromFile() 
		{
			System.Windows.Forms.OpenFileDialog openFileDialog1=new System.Windows.Forms.OpenFileDialog();
			openFileDialog1.Multiselect = true;
			openFileDialog1.CheckFileExists = true;
			openFileDialog1.DefaultExt = "TAB";
			openFileDialog1.Filter = 
				"MapInfo Tables (*.tab)|*.tab|" +
				"MapInfo Geoset (*.gst)|*.gst|" +
				"MapInfo WorkSpace (*.mws)|*.mws";
			string	strCantOpenList = null;          
			if(openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) 
			{
				foreach(string filename in openFileDialog1.FileNames)	
				{
					try 
					{
						Map map = Session.Current.MapFactory.CreateFromFile(filename, System.IntPtr.Zero, mapControl1.Size);
						SetMap(map);
						UpdateMapComboBox();
					} 
					catch(MapException me) 
					{
						if (strCantOpenList==null) 
						{
							strCantOpenList = me.Arg;
						} 
						else 
						{
							strCantOpenList = strCantOpenList + ", " + me.Arg;
						}
					}
				}
			}
			if (strCantOpenList != null) 
			{
				MessageBox.Show("The following failed to open: " + strCantOpenList);
			}
		}
	}
}
