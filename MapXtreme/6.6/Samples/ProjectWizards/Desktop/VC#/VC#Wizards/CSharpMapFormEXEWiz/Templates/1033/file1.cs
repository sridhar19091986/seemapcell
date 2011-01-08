using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MapInfo.Mapping;

namespace [!output SAFE_NAMESPACE_NAME]
{
	/// <summary>
	/// Summary description for [!output SAFE_CLASS_NAME].
	/// </summary>
	public class [!output SAFE_CLASS_NAME] : System.Windows.Forms.Form
	{
		private MapInfo.Windows.Controls.MapControl mapControl1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.StatusBar statusBar1;
		private MapInfo.Windows.Controls.MapToolBar mapToolBar1;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonOpenTable;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonLayerControl;
		private System.Windows.Forms.ToolBarButton toolBarButtonSeparator;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonSelect;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomIn;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomOut;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonPan;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonCenter;
		private System.ComponentModel.Container components = null;

		public [!output SAFE_CLASS_NAME]()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Listen to a map event in order to update the status bar
			mapControl1.Map.ViewChangedEvent += new ViewChangedEventHandler(Map_ViewChanged);

			// Set the MessageBox.Caption property to specify the text 
			// that appears on the title bar of various system message boxes: 
			MapInfo.Windows.MessageBox.Caption = "[!output SAFE_NAMESPACE_NAME]"; 

			//
			// Add your custom code here
			//

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
			this.mapToolBar1 = new MapInfo.Windows.Controls.MapToolBar();
			this.mapToolBarButtonOpenTable = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonLayerControl = new MapInfo.Windows.Controls.MapToolBarButton();
			this.toolBarButtonSeparator = new System.Windows.Forms.ToolBarButton();
			this.mapToolBarButtonSelect = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonZoomIn = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonZoomOut = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonPan = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonCenter = new MapInfo.Windows.Controls.MapToolBarButton();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mapControl1
			// 
			this.mapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mapControl1.Location = new System.Drawing.Point(0, 0);
			this.mapControl1.Name = "mapControl1";
			this.mapControl1.Size = new System.Drawing.Size(394, 245);
			this.mapControl1.TabIndex = 0;
			this.mapControl1.Text = "地图控件1";
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Controls.Add(this.mapControl1);
			this.panel1.Location = new System.Drawing.Point(4, 40);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(398, 249);
			this.panel1.TabIndex = 1;
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 287);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(406, 19);
			this.statusBar1.TabIndex = 2;
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
																																									 this.mapToolBarButtonPan,
																																									 this.mapToolBarButtonCenter});
			this.mapToolBar1.Divider = false;
			this.mapToolBar1.Dock = System.Windows.Forms.DockStyle.None;
			this.mapToolBar1.DropDownArrows = true;
			this.mapToolBar1.Location = new System.Drawing.Point(4, 8);
			this.mapToolBar1.MapControl = this.mapControl1;
			this.mapToolBar1.Name = "mapToolBar1";
			this.mapToolBar1.ShowToolTips = true;
			this.mapToolBar1.Size = new System.Drawing.Size(407, 26);
			this.mapToolBar1.TabIndex = 3;
			// 
			// mapToolBarButtonOpenTable
			// 
			this.mapToolBarButtonOpenTable.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.OpenTable;
			this.mapToolBarButtonOpenTable.ToolTipText = "打开表";
			// 
			// mapToolBarButtonLayerControl
			// 
			this.mapToolBarButtonLayerControl.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.LayerControl;
			this.mapToolBarButtonLayerControl.ToolTipText = "图层控制";
			// 
			// toolBarButtonSeparator
			// 
			this.toolBarButtonSeparator.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// mapToolBarButtonSelect
			// 
			this.mapToolBarButtonSelect.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Select;
			this.mapToolBarButtonSelect.ToolTipText = "选择";
			// 
			// mapToolBarButtonZoomIn
			// 
			this.mapToolBarButtonZoomIn.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomIn;
			this.mapToolBarButtonZoomIn.ToolTipText = "放大";
			// 
			// mapToolBarButtonZoomOut
			// 
			this.mapToolBarButtonZoomOut.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomOut;
			this.mapToolBarButtonZoomOut.ToolTipText = "缩小";
			// 
			// mapToolBarButtonPan
			// 
			this.mapToolBarButtonPan.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Pan;
			this.mapToolBarButtonPan.ToolTipText = "平移";
			// 
			// mapToolBarButtonCenter
			// 
			this.mapToolBarButtonCenter.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Center;
			this.mapToolBarButtonCenter.ToolTipText = "中心";
			// 
			// [!output SAFE_CLASS_NAME]
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(406, 306);
			this.Controls.Add(this.mapToolBar1);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.panel1);
			this.MinimumSize = new System.Drawing.Size(250, 200);
			this.Name = "[!output SAFE_CLASS_NAME]";
			this.Text = "[!output SAFE_CLASS_NAME]";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new [!output SAFE_CLASS_NAME]());
		}

		
		// Handler function called when the active map's view changes
		private void Map_ViewChanged(object o, ViewChangedEventArgs e) {
			// Display the zoom level
			Double dblZoom = System.Convert.ToDouble(String.Format("{0:E2}", mapControl1.Map.Zoom.Value));
			statusBar1.Text = "缩放:  " + dblZoom.ToString() + " " + MapInfo.Geometry.CoordSys.DistanceUnitAbbreviation(mapControl1.Map.Zoom.Unit);
		}
	}
}
