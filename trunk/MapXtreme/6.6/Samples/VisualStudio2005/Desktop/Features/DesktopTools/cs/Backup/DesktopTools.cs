using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using MapInfo.Data;
using MapInfo.Mapping;
using MapInfo.Engine;
using MapInfo.Windows.Dialogs;
using MapInfo.Geometry;
using MapInfo.Styles;
using MapInfo.Tools;
using MapInfo.Windows.Controls;

namespace ToolsAppCS
{
	/// <summary>
	/// Summary description for MapForm1.
	/// </summary>
	public class MapForm1 : System.Windows.Forms.Form
	{
		private MapInfo.Windows.Controls.MapControl mapControl1;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.ToolBarButton toolBarButtonAddPoint;
		private System.Windows.Forms.ToolBarButton toolBarButtonAddLine;
		private System.Windows.Forms.ToolBarButton toolBarButtonAddPolyline;
		private System.Windows.Forms.ToolBarButton toolBarButtonAddPolygon;
		private System.Windows.Forms.ToolBarButton toolBarButtonAddCircle;
		private System.Windows.Forms.ToolBarButton toolBarButtonAddRectangle;
		private System.Windows.Forms.ToolBarButton toolBarButtonAddEllipse;
		private System.Windows.Forms.ToolBarButton toolBarButtonAddText;
		private System.Windows.Forms.ToolBarButton toolBarButtonSepCustomTools;
		private System.Windows.Forms.ToolBarButton toolBarButtonArrow;
		private System.Windows.Forms.ToolBarButton toolBarButtonCustomPoint;
		private System.Windows.Forms.ToolBarButton toolBarButtonCustomLine;
		private System.Windows.Forms.ToolBarButton toolBarButtonCustomArc;
		private System.Windows.Forms.ToolBarButton toolBarButtonCustomPolyline;
		private System.Windows.Forms.ToolBarButton toolBarButtonCustomPolygon;
		private System.Windows.Forms.ToolBarButton toolBarButtonCustomCircle;
		private System.Windows.Forms.ToolBarButton toolBarButtonCustomRectangle;
		private System.Windows.Forms.ToolBarButton toolBarButtonCustomEllipse;
		private System.Windows.Forms.ToolBarButton toolBarButtonSepPushButtons;
		private System.Windows.Forms.ToolBarButton toolBarButtonFileOpen;
		private System.Windows.Forms.ToolBarButton toolBarButtonLayerControl;
		private System.Windows.Forms.ToolBarButton toolBarButtonSepViewTools;
		private System.Windows.Forms.ToolBarButton toolBarButtonZoomIn;
		private System.Windows.Forms.ToolBarButton toolBarButtonZoomOut;
		private System.Windows.Forms.ToolBarButton toolBarButtonPan;
		private System.Windows.Forms.ToolBarButton toolBarButtonCenter;
		private System.Windows.Forms.ToolBarButton toolBarButtonLabel;
		private System.Windows.Forms.ToolBarButton toolBarButtonSepSelectTools;
		private System.Windows.Forms.ToolBarButton toolBarButtonSelect;
		private System.Windows.Forms.ToolBarButton toolBarButtonSelectPolygon;
		private System.Windows.Forms.ToolBarButton toolBarButtonSelectRadius;
		private System.Windows.Forms.ToolBarButton toolBarButtonSelectRect;
		private System.Windows.Forms.ToolBarButton toolBarButtonSelectRegion;
		private System.Windows.Forms.Label lblEventsFired;
		private System.Windows.Forms.Label lblCancelActivation;
		private System.Windows.Forms.Label lblCancelUse;
		private System.Windows.Forms.Label lblNumObjects;
		private System.Windows.Forms.Label lblNumSelected;
		private System.Windows.Forms.Label lblWheelMode;
		private System.Windows.Forms.Button btnRegions;
		private System.Windows.Forms.Button btnPoints;
		private System.Windows.Forms.Button btnClearMap;
		private ToolsAppCS.LayerFilterStatusBar statusBar1;
		private System.Windows.Forms.TextBox txtNumObjects;
		private System.Windows.Forms.TextBox txtNumSelected;
		private System.Windows.Forms.TextBox txtResults;
		private System.Windows.Forms.CheckBox chkCancelSelect;
		private System.Windows.Forms.CheckBox chkCancelNodeEdit;
		private System.Windows.Forms.CheckBox chkCancelObjChanges;
		private System.Windows.Forms.CheckBox chkCancelObjAdd;
		private System.Windows.Forms.CheckBox chkCancelLabel;
		private System.Windows.Forms.CheckBox chkEventCtrls;
		private System.Windows.Forms.ComboBox comboCancelActivation;
		private System.Windows.Forms.ComboBox comboCancelToolEnding;
		private System.Windows.Forms.ComboBox comboWheelMode;
		private System.Windows.Forms.Panel panelMapBorder;
		private System.Windows.Forms.StatusBarPanel statusBarPanelZoom;

		private LayerControlDlg _layerDlg = null;
		private System.Windows.Forms.Button btnLines;

		// module-level variable used to ensure the map has the focus whenever
		//  the mouse is hovering over it -- allows better mouse-wheel behavior
		//  as well as correct tool icons, without requiring a starting map click.
		private Control lastFocus = null;

		[DllImport("user32.dll")]
		public static extern IntPtr GetFocus();


		public MapForm1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// add standard instances of stock Custom tools to the map's Tools collection
			mapControl1.Tools.Add("CustomPoint", new CustomPointMapTool(true, mapControl1.Tools.FeatureViewer, 
				mapControl1.Handle.ToInt32(), mapControl1.Tools,
				mapControl1.Tools.MouseToolProperties, mapControl1.Tools.MapToolProperties));

			mapControl1.Tools.Add("CustomLine", new CustomLineMapTool(true, true, true, 
				mapControl1.Viewer, mapControl1.Handle.ToInt32(), mapControl1.Tools,
				mapControl1.Tools.MouseToolProperties, mapControl1.Tools.MapToolProperties));

			mapControl1.Tools.Add("CustomPolyline", new CustomPolylineMapTool(true, true, true, 
				mapControl1.Viewer, mapControl1.Handle.ToInt32(), mapControl1.Tools,
				mapControl1.Tools.MouseToolProperties, mapControl1.Tools.MapToolProperties));

			mapControl1.Tools.Add("CustomPolygon", new CustomPolygonMapTool(true, true, true, 
				mapControl1.Viewer, mapControl1.Handle.ToInt32(), mapControl1.Tools,
				mapControl1.Tools.MouseToolProperties, mapControl1.Tools.MapToolProperties));

			mapControl1.Tools.Add("CustomRectangle", new CustomRectangleMapTool(true, true, true, 
				mapControl1.Viewer, mapControl1.Handle.ToInt32(), mapControl1.Tools,
				mapControl1.Tools.MouseToolProperties, mapControl1.Tools.MapToolProperties));

			mapControl1.Tools.Add("CustomEllipse", new CustomEllipseMapTool(true, true, true, 
				mapControl1.Viewer, mapControl1.Handle.ToInt32(), mapControl1.Tools,
				mapControl1.Tools.MouseToolProperties, mapControl1.Tools.MapToolProperties));

			mapControl1.Tools.Add("CustomCircle", new CustomCircleMapTool(true, true, true, 
				mapControl1.Viewer, mapControl1.Handle.ToInt32(), mapControl1.Tools,
				mapControl1.Tools.MouseToolProperties, mapControl1.Tools.MapToolProperties));

			mapControl1.Tools.Add("CustomArc", new CustomArcMapTool(true, true, true, 
				mapControl1.Viewer, mapControl1.Handle.ToInt32(), mapControl1.Tools,
				mapControl1.Tools.MouseToolProperties, mapControl1.Tools.MapToolProperties));

			// Listen to some map events
			mapControl1.Map.ViewChangedEvent += new ViewChangedEventHandler(Map_ViewChanged);
			mapControl1.Map.Layers.Added += new CollectionEventHandler(Layers_CountChanged);
			mapControl1.Map.Layers.Removed += new CollectionEventHandler(Layers_CountChanged);

			// Listen to some tool events
			mapControl1.Tools.Activating += new ToolActivatingEventHandler(ToolActivating);
			mapControl1.Tools.Activated += new ToolActivatedEventHandler(ToolActivatedFired);
			mapControl1.Tools.Used += new ToolUsedEventHandler(ToolUsed);
			mapControl1.Tools.Ending += new ToolEndingEventHandler(ToolEnding);
			mapControl1.Tools.FeatureAdding += new FeatureAddingEventHandler(FeatureAdding);
			mapControl1.Tools.FeatureAdded += new FeatureAddedEventHandler(FeatureAdded);
			mapControl1.Tools.FeatureSelecting += new FeatureSelectingEventHandler(FeatureSelecting);
			mapControl1.Tools.FeatureSelected += new FeatureSelectedEventHandler(FeatureSelected);
			mapControl1.Tools.FeatureChanging += new FeatureChangingEventHandler(FeatureChanging);
			mapControl1.Tools.FeatureChanged += new MapInfo.Tools.FeatureChangedEventHandler(FeatureChanged);
			mapControl1.Tools.NodeChanging += new NodeChangingEventHandler(NodeChanging);
			mapControl1.Tools.NodeChanged += new NodeChangedEventHandler(NodeChanged);
			mapControl1.Tools.LabelAdding += new LabelAddingEventHandler(LabelAdding);
			mapControl1.Tools.LabelAdded += new LabelAddedEventHandler(LabelAdded);

			mapControl1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.mapControl1_MouseWheel);

			// only create a new LayerControlDialog _once_:
			_layerDlg = new LayerControlDlg();
			_layerDlg.Map=mapControl1.Map;
			_layerDlg.LayerControl.SelectedTab=PropertiesCategory.Editing;

			// Extra toolbar setup
			ToolBarSetup();

			// Set up StatusBar to show which layers allow select/edit/draw operations, 
			// and divide available space evenly between StatusBar panels
			statusBar1.Layers = mapControl1.Map.Layers; 
			statusBar1.SelectableLayerFilter = mapControl1.Tools.SelectMapToolProperties.SelectableLayerFilter; 
			statusBar1.EditableLayerFilter = mapControl1.Tools.SelectMapToolProperties.EditableLayerFilter; 
			statusBar1.InsertionLayerFilter = mapControl1.Tools.AddMapToolProperties.InsertionLayerFilter; 		
			foreach (StatusBarPanel p in statusBar1.Panels) p.Width=174;
			UpdateAllControls();

			// deal with optional display of advanced controls related to Cancel events
			comboWheelMode.SelectedIndex=2;
			comboCancelActivation.SelectedIndex=0;
			comboCancelToolEnding.SelectedIndex=0;
			panelMapBorder.Location=new System.Drawing.Point(panelMapBorder.Location.X, chkEventCtrls.Checked ? 128 : 56);
			panelMapBorder.Height=txtResults.Bottom - panelMapBorder.Location.Y;

			// make standard call to clear the map and add a temporary table for editing
			ReloadMap("<None>");
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
				if (components != null) components.Dispose();
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
			this.panelMapBorder = new System.Windows.Forms.Panel();
			this.txtResults = new System.Windows.Forms.TextBox();
			this.toolBar1 = new System.Windows.Forms.ToolBar();
			this.toolBarButtonAddPoint = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonAddLine = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonAddPolyline = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonAddPolygon = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonAddCircle = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonAddRectangle = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonAddEllipse = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonAddText = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSepCustomTools = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonArrow = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonCustomPoint = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonCustomLine = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonCustomArc = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonCustomPolyline = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonCustomPolygon = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonCustomCircle = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonCustomRectangle = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonCustomEllipse = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSepPushButtons = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonFileOpen = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonLayerControl = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSepViewTools = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonZoomIn = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonZoomOut = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonPan = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonCenter = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonLabel = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSepSelectTools = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSelect = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSelectRect = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSelectRadius = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSelectPolygon = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSelectRegion = new System.Windows.Forms.ToolBarButton();
			this.lblEventsFired = new System.Windows.Forms.Label();
			this.comboCancelActivation = new System.Windows.Forms.ComboBox();
			this.lblCancelActivation = new System.Windows.Forms.Label();
			this.comboCancelToolEnding = new System.Windows.Forms.ComboBox();
			this.lblCancelUse = new System.Windows.Forms.Label();
			this.btnRegions = new System.Windows.Forms.Button();
			this.btnPoints = new System.Windows.Forms.Button();
			this.btnLines = new System.Windows.Forms.Button();
			this.btnClearMap = new System.Windows.Forms.Button();
			this.statusBar1 = new ToolsAppCS.LayerFilterStatusBar();
			this.statusBarPanelZoom = new System.Windows.Forms.StatusBarPanel();
			this.lblNumObjects = new System.Windows.Forms.Label();
			this.txtNumObjects = new System.Windows.Forms.TextBox();
			this.chkCancelSelect = new System.Windows.Forms.CheckBox();
			this.chkCancelNodeEdit = new System.Windows.Forms.CheckBox();
			this.lblNumSelected = new System.Windows.Forms.Label();
			this.txtNumSelected = new System.Windows.Forms.TextBox();
			this.chkCancelObjChanges = new System.Windows.Forms.CheckBox();
			this.chkCancelObjAdd = new System.Windows.Forms.CheckBox();
			this.comboWheelMode = new System.Windows.Forms.ComboBox();
			this.chkCancelLabel = new System.Windows.Forms.CheckBox();
			this.chkEventCtrls = new System.Windows.Forms.CheckBox();
			this.lblWheelMode = new System.Windows.Forms.Label();
			this.panelMapBorder.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanelZoom)).BeginInit();
			this.SuspendLayout();
			// 
			// mapControl1
			// 
			this.mapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mapControl1.Location = new System.Drawing.Point(0, 0);
			this.mapControl1.Name = "mapControl1";
			this.mapControl1.Size = new System.Drawing.Size(440, 289);
			this.mapControl1.TabIndex = 8;
			this.mapControl1.Text = "mapControl1";
			this.mapControl1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MapForm1_KeyPress);
			this.mapControl1.MouseEnter += new System.EventHandler(this.mapControl1_MouseEnter);
			this.mapControl1.MouseLeave += new System.EventHandler(this.mapControl1_MouseLeave);
			// 
			// panelMapBorder
			// 
			this.panelMapBorder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.panelMapBorder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panelMapBorder.Controls.Add(this.mapControl1);
			this.panelMapBorder.Location = new System.Drawing.Point(4, 128);
			this.panelMapBorder.Name = "panelMapBorder";
			this.panelMapBorder.Size = new System.Drawing.Size(444, 293);
			this.panelMapBorder.TabIndex = 14;
			// 
			// txtResults
			// 
			this.txtResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtResults.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtResults.Location = new System.Drawing.Point(456, 128);
			this.txtResults.Multiline = true;
			this.txtResults.Name = "txtResults";
			this.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtResults.Size = new System.Drawing.Size(223, 293);
			this.txtResults.TabIndex = 9;
			this.txtResults.TabStop = false;
			this.txtResults.Text = "";
			// 
			// toolBar1
			// 
			this.toolBar1.AutoSize = false;
			this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						this.toolBarButtonAddPoint,
																						this.toolBarButtonAddLine,
																						this.toolBarButtonAddPolyline,
																						this.toolBarButtonAddPolygon,
																						this.toolBarButtonAddCircle,
																						this.toolBarButtonAddRectangle,
																						this.toolBarButtonAddEllipse,
																						this.toolBarButtonAddText,
																						this.toolBarButtonSepCustomTools,
																						this.toolBarButtonArrow,
																						this.toolBarButtonCustomPoint,
																						this.toolBarButtonCustomLine,
																						this.toolBarButtonCustomArc,
																						this.toolBarButtonCustomPolyline,
																						this.toolBarButtonCustomPolygon,
																						this.toolBarButtonCustomCircle,
																						this.toolBarButtonCustomRectangle,
																						this.toolBarButtonCustomEllipse,
																						this.toolBarButtonSepPushButtons,
																						this.toolBarButtonFileOpen,
																						this.toolBarButtonLayerControl,
																						this.toolBarButtonSepViewTools,
																						this.toolBarButtonZoomIn,
																						this.toolBarButtonZoomOut,
																						this.toolBarButtonPan,
																						this.toolBarButtonCenter,
																						this.toolBarButtonLabel,
																						this.toolBarButtonSepSelectTools,
																						this.toolBarButtonSelect,
																						this.toolBarButtonSelectRect,
																						this.toolBarButtonSelectRadius,
																						this.toolBarButtonSelectPolygon,
																						this.toolBarButtonSelectRegion});
			this.toolBar1.ButtonSize = new System.Drawing.Size(25, 22);
			this.toolBar1.Divider = false;
			this.toolBar1.Dock = System.Windows.Forms.DockStyle.None;
			this.toolBar1.DropDownArrows = true;
			this.toolBar1.Location = new System.Drawing.Point(8, 0);
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.ShowToolTips = true;
			this.toolBar1.Size = new System.Drawing.Size(448, 56);
			this.toolBar1.TabIndex = 1;
			this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
			// 
			// toolBarButtonAddPoint
			// 
			this.toolBarButtonAddPoint.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonAddPoint.Tag = "AddPoint";
			this.toolBarButtonAddPoint.ToolTipText = "Add Point";
			// 
			// toolBarButtonAddLine
			// 
			this.toolBarButtonAddLine.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonAddLine.Tag = "AddLine";
			this.toolBarButtonAddLine.ToolTipText = "Add Line";
			// 
			// toolBarButtonAddPolyline
			// 
			this.toolBarButtonAddPolyline.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonAddPolyline.Tag = "AddPolyline";
			this.toolBarButtonAddPolyline.ToolTipText = "Add Polyline";
			// 
			// toolBarButtonAddPolygon
			// 
			this.toolBarButtonAddPolygon.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonAddPolygon.Tag = "AddPolygon";
			this.toolBarButtonAddPolygon.ToolTipText = "Add Polygon";
			// 
			// toolBarButtonAddCircle
			// 
			this.toolBarButtonAddCircle.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonAddCircle.Tag = "AddCircle";
			this.toolBarButtonAddCircle.ToolTipText = "Add Circle";
			// 
			// toolBarButtonAddRectangle
			// 
			this.toolBarButtonAddRectangle.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonAddRectangle.Tag = "AddRectangle";
			this.toolBarButtonAddRectangle.ToolTipText = "Add Rectangle";
			// 
			// toolBarButtonAddEllipse
			// 
			this.toolBarButtonAddEllipse.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonAddEllipse.Tag = "AddEllipse";
			this.toolBarButtonAddEllipse.ToolTipText = "Add Ellipse";
			// 
			// toolBarButtonAddText
			// 
			this.toolBarButtonAddText.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonAddText.Tag = "AddText";
			this.toolBarButtonAddText.ToolTipText = "Add Text";
			// 
			// toolBarButtonSepCustomTools
			// 
			this.toolBarButtonSepCustomTools.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonArrow
			// 
			this.toolBarButtonArrow.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonArrow.Tag = "Arrow";
			this.toolBarButtonArrow.ToolTipText = "Arrow";
			// 
			// toolBarButtonCustomPoint
			// 
			this.toolBarButtonCustomPoint.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonCustomPoint.Tag = "CustomPoint";
			this.toolBarButtonCustomPoint.ToolTipText = "Custom Point";
			// 
			// toolBarButtonCustomLine
			// 
			this.toolBarButtonCustomLine.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonCustomLine.Tag = "CustomLine";
			this.toolBarButtonCustomLine.ToolTipText = "Custom Line";
			// 
			// toolBarButtonCustomArc
			// 
			this.toolBarButtonCustomArc.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonCustomArc.Tag = "CustomArc";
			this.toolBarButtonCustomArc.ToolTipText = "Custom Arc";
			// 
			// toolBarButtonCustomPolyline
			// 
			this.toolBarButtonCustomPolyline.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonCustomPolyline.Tag = "CustomPolyline";
			this.toolBarButtonCustomPolyline.ToolTipText = "Custom Polyline";
			// 
			// toolBarButtonCustomPolygon
			// 
			this.toolBarButtonCustomPolygon.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonCustomPolygon.Tag = "CustomPolygon";
			this.toolBarButtonCustomPolygon.ToolTipText = "Custom Polygon";
			// 
			// toolBarButtonCustomCircle
			// 
			this.toolBarButtonCustomCircle.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonCustomCircle.Tag = "CustomCircle";
			this.toolBarButtonCustomCircle.ToolTipText = "Custom Circle";
			// 
			// toolBarButtonCustomRectangle
			// 
			this.toolBarButtonCustomRectangle.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonCustomRectangle.Tag = "CustomRectangle";
			this.toolBarButtonCustomRectangle.ToolTipText = "Custom Rectangle";
			// 
			// toolBarButtonCustomEllipse
			// 
			this.toolBarButtonCustomEllipse.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonCustomEllipse.Tag = "CustomEllipse";
			this.toolBarButtonCustomEllipse.ToolTipText = "Custom Ellipse";
			// 
			// toolBarButtonSepPushButtons
			// 
			this.toolBarButtonSepPushButtons.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonFileOpen
			// 
			this.toolBarButtonFileOpen.ToolTipText = "Open Table";
			// 
			// toolBarButtonLayerControl
			// 
			this.toolBarButtonLayerControl.Enabled = false;
			this.toolBarButtonLayerControl.ToolTipText = "Layer Control";
			// 
			// toolBarButtonSepViewTools
			// 
			this.toolBarButtonSepViewTools.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonZoomIn
			// 
			this.toolBarButtonZoomIn.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonZoomIn.Tag = "ZoomIn";
			this.toolBarButtonZoomIn.ToolTipText = "Zoom In";
			// 
			// toolBarButtonZoomOut
			// 
			this.toolBarButtonZoomOut.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonZoomOut.Tag = "ZoomOut";
			this.toolBarButtonZoomOut.ToolTipText = "Zoom Out";
			// 
			// toolBarButtonPan
			// 
			this.toolBarButtonPan.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonPan.Tag = "Pan";
			this.toolBarButtonPan.ToolTipText = "Pan";
			// 
			// toolBarButtonCenter
			// 
			this.toolBarButtonCenter.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonCenter.Tag = "Center";
			this.toolBarButtonCenter.ToolTipText = "Center Tool";
			// 
			// toolBarButtonLabel
			// 
			this.toolBarButtonLabel.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonLabel.Tag = "Label";
			this.toolBarButtonLabel.ToolTipText = "Label Tool";
			// 
			// toolBarButtonSepSelectTools
			// 
			this.toolBarButtonSepSelectTools.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonSelect
			// 
			this.toolBarButtonSelect.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonSelect.Tag = "Select";
			this.toolBarButtonSelect.ToolTipText = "Select";
			// 
			// toolBarButtonSelectRect
			// 
			this.toolBarButtonSelectRect.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonSelectRect.Tag = "SelectRect";
			this.toolBarButtonSelectRect.ToolTipText = "Select Rectangle";
			// 
			// toolBarButtonSelectRadius
			// 
			this.toolBarButtonSelectRadius.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonSelectRadius.Tag = "SelectRadius";
			this.toolBarButtonSelectRadius.ToolTipText = "Select Radius";
			// 
			// toolBarButtonSelectPolygon
			// 
			this.toolBarButtonSelectPolygon.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonSelectPolygon.Tag = "SelectPolygon";
			this.toolBarButtonSelectPolygon.ToolTipText = "Select Polygon";
			// 
			// toolBarButtonSelectRegion
			// 
			this.toolBarButtonSelectRegion.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.toolBarButtonSelectRegion.Tag = "SelectRegion";
			this.toolBarButtonSelectRegion.ToolTipText = "Select Region";
			// 
			// lblEventsFired
			// 
			this.lblEventsFired.Location = new System.Drawing.Point(456, 112);
			this.lblEventsFired.Name = "lblEventsFired";
			this.lblEventsFired.Size = new System.Drawing.Size(80, 16);
			this.lblEventsFired.TabIndex = 13;
			this.lblEventsFired.Text = "Events fired:";
			// 
			// comboCancelActivation
			// 
			this.comboCancelActivation.DisplayMember = "<None>";
			this.comboCancelActivation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCancelActivation.Items.AddRange(new object[] {
																	   "<None>",
																	   "ZoomIn",
																	   "ZoomOut",
																	   "Pan",
																	   "Center",
																	   "Label",
																	   "Select",
																	   "SelectPolygon",
																	   "SelectRadius",
																	   "SelectRect",
																	   "SelectRegion",
																	   "AddPoint",
																	   "AddLine",
																	   "AddPolyline",
																	   "AddPolygon",
																	   "AddCircle",
																	   "AddRectangle",
																	   "AddEllipse",
																	   "AddText",
																	   "Arrow",
																	   "CustomPoint",
																	   "CustomLine",
																	   "CustomArc",
																	   "CustomPolyline",
																	   "CustomPolygon",
																	   "CustomCircle",
																	   "CustomRectangle",
																	   "CustomEllipse",
																	   "CustomizedSelectRect",
																	   "AddLineDistance",
																	   "LocationTool"});
			this.comboCancelActivation.Location = new System.Drawing.Point(112, 80);
			this.comboCancelActivation.Name = "comboCancelActivation";
			this.comboCancelActivation.Size = new System.Drawing.Size(88, 21);
			this.comboCancelActivation.TabIndex = 3;
			this.comboCancelActivation.ValueMember = "<None>";
			this.comboCancelActivation.Visible = false;
			// 
			// lblCancelActivation
			// 
			this.lblCancelActivation.Location = new System.Drawing.Point(8, 80);
			this.lblCancelActivation.Name = "lblCancelActivation";
			this.lblCancelActivation.Size = new System.Drawing.Size(128, 20);
			this.lblCancelActivation.TabIndex = 11;
			this.lblCancelActivation.Text = "Cancel activation for:";
			this.lblCancelActivation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblCancelActivation.Visible = false;
			// 
			// comboCancelToolEnding
			// 
			this.comboCancelToolEnding.DisplayMember = "<None>";
			this.comboCancelToolEnding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCancelToolEnding.Items.AddRange(new object[] {
																	   "<None>",
																	   "ZoomIn",
																	   "ZoomOut",
																	   "Pan",
																	   "Center",
																	   "Label",
																	   "Select",
																	   "SelectPolygon",
																	   "SelectRadius",
																	   "SelectRect",
																	   "SelectRegion",
																	   "AddPoint",
																	   "AddLine",
																	   "AddPolyline",
																	   "AddPolygon",
																	   "AddCircle",
																	   "AddRectangle",
																	   "AddEllipse",
																	   "AddText",
																	   "Arrow",
																	   "CustomPoint",
																	   "CustomLine",
																	   "CustomArc",
																	   "CustomPolyline",
																	   "CustomPolygon",
																	   "CustomCircle",
																	   "CustomRectangle",
																	   "CustomEllipse",
																	   "CustomizedSelectRect",
																	   "AddLineDistance",
																	   "LocationTool"});
			this.comboCancelToolEnding.Location = new System.Drawing.Point(112, 104);
			this.comboCancelToolEnding.Name = "comboCancelToolEnding";
			this.comboCancelToolEnding.Size = new System.Drawing.Size(88, 21);
			this.comboCancelToolEnding.TabIndex = 8;
			this.comboCancelToolEnding.ValueMember = "<None>";
			this.comboCancelToolEnding.Visible = false;
			// 
			// lblCancelUse
			// 
			this.lblCancelUse.Location = new System.Drawing.Point(8, 104);
			this.lblCancelUse.Name = "lblCancelUse";
			this.lblCancelUse.Size = new System.Drawing.Size(88, 20);
			this.lblCancelUse.TabIndex = 12;
			this.lblCancelUse.Text = "Cancel use for:";
			this.lblCancelUse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblCancelUse.Visible = false;
			// 
			// btnRegions
			// 
			this.btnRegions.Location = new System.Drawing.Point(560, 8);
			this.btnRegions.Name = "btnRegions";
			this.btnRegions.Size = new System.Drawing.Size(120, 20);
			this.btnRegions.TabIndex = 4;
			this.btnRegions.Text = "Add Regions Table";
			this.btnRegions.Click += new System.EventHandler(this.btnRegions_Click);
			// 
			// btnPoints
			// 
			this.btnPoints.Location = new System.Drawing.Point(560, 56);
			this.btnPoints.Name = "btnPoints";
			this.btnPoints.Size = new System.Drawing.Size(120, 20);
			this.btnPoints.TabIndex = 5;
			this.btnPoints.Text = "Add Points Table";
			this.btnPoints.Click += new System.EventHandler(this.btnPoints_Click);
			// 
			// btnLines
			// 
			this.btnLines.Location = new System.Drawing.Point(560, 32);
			this.btnLines.Name = "btnLines";
			this.btnLines.Size = new System.Drawing.Size(120, 20);
			this.btnLines.TabIndex = 6;
			this.btnLines.Text = "Add Lines Table";
			this.btnLines.Click += new System.EventHandler(this.btnLines_Click);
			// 
			// btnClearMap
			// 
			this.btnClearMap.Location = new System.Drawing.Point(456, 56);
			this.btnClearMap.Name = "btnClearMap";
			this.btnClearMap.Size = new System.Drawing.Size(96, 20);
			this.btnClearMap.TabIndex = 2;
			this.btnClearMap.Text = "Clear Map";
			this.btnClearMap.Click += new System.EventHandler(this.btnClearMap_Click);
			// 
			// statusBar1
			// 
			this.statusBar1.EditableLayerFilter = null;
			this.statusBar1.EditPanelWidth = 150;
			this.statusBar1.InsertionLayerFilter = null;
			this.statusBar1.InsertionPanelWidth = 150;
			this.statusBar1.Layers = null;
			this.statusBar1.Location = new System.Drawing.Point(0, 423);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						  this.statusBarPanelZoom});
			this.statusBar1.SelectableLayerFilter = null;
			this.statusBar1.SelectPanelWidth = 150;
			this.statusBar1.ShowPanels = true;
			this.statusBar1.Size = new System.Drawing.Size(688, 22);
			this.statusBar1.TabIndex = 10;
			this.statusBar1.Text = "statusBar1";
			// 
			// statusBarPanelZoom
			// 
			this.statusBarPanelZoom.Width = 180;
			// 
			// lblNumObjects
			// 
			this.lblNumObjects.Location = new System.Drawing.Point(528, 80);
			this.lblNumObjects.Name = "lblNumObjects";
			this.lblNumObjects.Size = new System.Drawing.Size(120, 20);
			this.lblNumObjects.TabIndex = 15;
			this.lblNumObjects.Text = "Objects in Temp table:";
			this.lblNumObjects.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtNumObjects
			// 
			this.txtNumObjects.Location = new System.Drawing.Point(640, 80);
			this.txtNumObjects.Name = "txtNumObjects";
			this.txtNumObjects.ReadOnly = true;
			this.txtNumObjects.Size = new System.Drawing.Size(40, 20);
			this.txtNumObjects.TabIndex = 16;
			this.txtNumObjects.Text = "";
			// 
			// chkCancelSelect
			// 
			this.chkCancelSelect.Location = new System.Drawing.Point(208, 104);
			this.chkCancelSelect.Name = "chkCancelSelect";
			this.chkCancelSelect.Size = new System.Drawing.Size(112, 20);
			this.chkCancelSelect.TabIndex = 17;
			this.chkCancelSelect.Text = "Cancel selection";
			this.chkCancelSelect.Visible = false;
			// 
			// chkCancelNodeEdit
			// 
			this.chkCancelNodeEdit.Location = new System.Drawing.Point(208, 56);
			this.chkCancelNodeEdit.Name = "chkCancelNodeEdit";
			this.chkCancelNodeEdit.Size = new System.Drawing.Size(120, 20);
			this.chkCancelNodeEdit.TabIndex = 18;
			this.chkCancelNodeEdit.Text = "Cancel node edits";
			this.chkCancelNodeEdit.Visible = false;
			// 
			// lblNumSelected
			// 
			this.lblNumSelected.Location = new System.Drawing.Point(528, 104);
			this.lblNumSelected.Name = "lblNumSelected";
			this.lblNumSelected.Size = new System.Drawing.Size(112, 20);
			this.lblNumSelected.TabIndex = 19;
			this.lblNumSelected.Text = "Objects in selection:";
			this.lblNumSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtNumSelected
			// 
			this.txtNumSelected.Location = new System.Drawing.Point(640, 104);
			this.txtNumSelected.Name = "txtNumSelected";
			this.txtNumSelected.ReadOnly = true;
			this.txtNumSelected.Size = new System.Drawing.Size(40, 20);
			this.txtNumSelected.TabIndex = 20;
			this.txtNumSelected.Text = "";
			// 
			// chkCancelObjChanges
			// 
			this.chkCancelObjChanges.Location = new System.Drawing.Point(208, 80);
			this.chkCancelObjChanges.Name = "chkCancelObjChanges";
			this.chkCancelObjChanges.Size = new System.Drawing.Size(144, 20);
			this.chkCancelObjChanges.TabIndex = 21;
			this.chkCancelObjChanges.Text = "Cancel obj changes";
			this.chkCancelObjChanges.Visible = false;
			// 
			// chkCancelObjAdd
			// 
			this.chkCancelObjAdd.Location = new System.Drawing.Point(336, 80);
			this.chkCancelObjAdd.Name = "chkCancelObjAdd";
			this.chkCancelObjAdd.Size = new System.Drawing.Size(120, 20);
			this.chkCancelObjAdd.TabIndex = 22;
			this.chkCancelObjAdd.Text = "Cancel object add";
			this.chkCancelObjAdd.Visible = false;
			// 
			// comboWheelMode
			// 
			this.comboWheelMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboWheelMode.Items.AddRange(new object[] {
																"Disabled",
																"Zoom Only",
																"Zoom and Scroll",
																"Custom Settings"});
			this.comboWheelMode.Location = new System.Drawing.Point(96, 56);
			this.comboWheelMode.Name = "comboWheelMode";
			this.comboWheelMode.Size = new System.Drawing.Size(104, 21);
			this.comboWheelMode.TabIndex = 23;
			this.comboWheelMode.Visible = false;
			this.comboWheelMode.SelectedIndexChanged += new System.EventHandler(this.comboWheelMode_SelectedIndexChanged);
			// 
			// chkCancelLabel
			// 
			this.chkCancelLabel.Location = new System.Drawing.Point(336, 56);
			this.chkCancelLabel.Name = "chkCancelLabel";
			this.chkCancelLabel.Size = new System.Drawing.Size(120, 20);
			this.chkCancelLabel.TabIndex = 24;
			this.chkCancelLabel.Text = "Cancel label add";
			this.chkCancelLabel.Visible = false;
			// 
			// chkEventCtrls
			// 
			this.chkEventCtrls.Location = new System.Drawing.Point(366, 32);
			this.chkEventCtrls.Name = "chkEventCtrls";
			this.chkEventCtrls.Size = new System.Drawing.Size(184, 16);
			this.chkEventCtrls.TabIndex = 25;
			this.chkEventCtrls.Text = "Show Advanced Event Settings";
			this.chkEventCtrls.CheckedChanged += new System.EventHandler(this.chkEventCtrls_CheckedChanged);
			// 
			// lblWheelMode
			// 
			this.lblWheelMode.Location = new System.Drawing.Point(8, 56);
			this.lblWheelMode.Name = "lblWheelMode";
			this.lblWheelMode.Size = new System.Drawing.Size(80, 16);
			this.lblWheelMode.TabIndex = 26;
			this.lblWheelMode.Text = "MouseWheel:";
			this.lblWheelMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblWheelMode.Visible = false;
			// 
			// MapForm1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(688, 445);
			this.Controls.Add(this.chkEventCtrls);
			this.Controls.Add(this.chkCancelLabel);
			this.Controls.Add(this.comboWheelMode);
			this.Controls.Add(this.chkCancelObjAdd);
			this.Controls.Add(this.txtNumSelected);
			this.Controls.Add(this.txtNumObjects);
			this.Controls.Add(this.txtResults);
			this.Controls.Add(this.lblNumSelected);
			this.Controls.Add(this.chkCancelNodeEdit);
			this.Controls.Add(this.chkCancelSelect);
			this.Controls.Add(this.lblNumObjects);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.btnClearMap);
			this.Controls.Add(this.btnLines);
			this.Controls.Add(this.btnPoints);
			this.Controls.Add(this.btnRegions);
			this.Controls.Add(this.comboCancelToolEnding);
			this.Controls.Add(this.lblCancelUse);
			this.Controls.Add(this.toolBar1);
			this.Controls.Add(this.comboCancelActivation);
			this.Controls.Add(this.lblCancelActivation);
			this.Controls.Add(this.panelMapBorder);
			this.Controls.Add(this.lblEventsFired);
			this.Controls.Add(this.chkCancelObjChanges);
			this.Controls.Add(this.lblWheelMode);
			this.MinimumSize = new System.Drawing.Size(696, 350);
			this.Name = "MapForm1";
			this.Text = "Desktop Tools Sample";
			this.Resize += new System.EventHandler(this.MapForm1_Resize);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MapForm1_KeyPress);
			this.panelMapBorder.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanelZoom)).EndInit();
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

		#region Map and toolbar initialization and handling code
		// This method creates an image list from a bitmap resource,
		// associates the imagelist with the toolbar,
		// and assigns images to each tool button.
		private void ToolBarSetup() 
		{
			// Create the ImageList
			ImageList imList = new ImageList();
			imList.ImageSize = new Size(18, 16);
			imList.ColorDepth = ColorDepth.Depth4Bit;
			imList.TransparentColor = Color.FromArgb(192, 192, 192);

			// adds the bitmap
			imList.Images.AddStrip(new Bitmap(
				Assembly.GetExecutingAssembly().GetManifestResourceStream(
				this.GetType(), "buttons.bmp")));

			toolBar1.ImageList = imList;
			toolBarButtonFileOpen.ImageIndex = 1;
			toolBarButtonLayerControl.ImageIndex = 30;

			// Select tools
			toolBarButtonSelect.ImageIndex = 10;
			toolBarButtonSelectRect.ImageIndex = 11;
			toolBarButtonSelectRadius.ImageIndex=12;
			toolBarButtonSelectPolygon.ImageIndex = 13;
			toolBarButtonSelectRegion.ImageIndex = 14;
			toolBarButtonLabel.ImageIndex=29;

			// Map tools
			toolBarButtonZoomIn.ImageIndex = 15;
			toolBarButtonZoomOut.ImageIndex = 16;
			toolBarButtonPan.ImageIndex = 18;
			toolBarButtonCenter.ImageIndex=19;

			// Add tools
			toolBarButtonAddPoint.ImageIndex = 20;
			toolBarButtonAddLine.ImageIndex = 21;
			toolBarButtonAddPolyline.ImageIndex = 22;
			toolBarButtonAddPolygon.ImageIndex = 23;
			toolBarButtonAddCircle.ImageIndex = 24;
			toolBarButtonAddRectangle.ImageIndex = 25;
			toolBarButtonAddEllipse.ImageIndex = 26;
			toolBarButtonAddText.ImageIndex = 27;

			// Custom tools
			toolBarButtonArrow.ImageIndex=9;
			toolBarButtonCustomPoint.ImageIndex = 48;
			toolBarButtonCustomLine.ImageIndex = 49;
			toolBarButtonCustomPolyline.ImageIndex = 50;
			toolBarButtonCustomArc.ImageIndex = 51;
			toolBarButtonCustomPolygon.ImageIndex = 52;
			toolBarButtonCustomCircle.ImageIndex = 53;
			toolBarButtonCustomRectangle.ImageIndex = 54;
			toolBarButtonCustomEllipse.ImageIndex = 55;

			// Make the select tool active by default
			mapControl1.Tools.LeftButtonTool="Select";
			CheckToolButton("Select");

			toolBarButtonFileOpen.Enabled=true;
			toolBarButtonLayerControl.Enabled=true;

			EnableToolButtons(!mapControl1.Map.Empty);
		}

		// This method displays an Open File dialog using the Load Map Wizard
		private void DisplayOpenFileDialog() 
		{
			// the four commented lines below can replace the rest of the code in this
			//  subroutine.  Other sample apps all use the LoadMapWizard in this way.
			//  An exception is made here to show how the same task can be accomplished
			//  with lower-level code -- which can then be customized if necessary.
			//
			//     LoadMapWizard loadMapWizard = new LoadMapWizard();
			//     loadMapWizard.ShowDbms = true;
			//     loadMapWizard.Run(this, mapControl1.Map);
			//     UpdateAllControls();

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
				foreach(string filename in openFileDialog1.FileNames)	
					try 
					{
						if (filename.ToLower().EndsWith(MapLoader.FileExtensionGST))  
							mapControl1.Map.Load(new MapGeosetLoader(filename)); // add geoset
						else if (filename.ToLower().EndsWith(MapLoader.FileExtensionWOR)) 
						{
							mapControl1.Map.Load(new MapWorkSpaceLoader(filename));  // add workspace
							mapControl1.Map.Size = mapControl1.Size;
						}	
						else mapControl1.Map.Load(new MapTableLoader(filename));  // add table
					} 
					catch(MapException me) 
					{
						if (strCantOpenList==null) strCantOpenList = me.Arg; 
						else strCantOpenList = strCantOpenList + ", " + me.Arg;
					}
			if (strCantOpenList != null) 
				MessageBox.Show("The following failed to open: " + strCantOpenList);

			// Newly opened tables are generally selectable and can be added to,
			// so refresh the status bar to show the latest settings. 
			UpdateAllControls();
		}

		// Display the layer control dialog
		private void DisplayLayerControlDialog()
		{
			// the following must be done every time Layer Control is opened,
			//  so that the Layer Control knows about any updates to the map
			_layerDlg.Map = mapControl1.Map;

			_layerDlg.LayerControl.Tools=mapControl1.Tools;
			_layerDlg.ShowDialog(this);  

			// The user might have changed the Selectable checkboxes, etc.,
			// so refresh the status bar to show the latest settings. 
			UpdateAllControls();
		}

		// Helper function to check one tool button and uncheck all the rest
		private void CheckToolButton(string toolName) 
		{
			foreach (ToolBarButton tbb in toolBar1.Buttons) 
				if (tbb.Style == ToolBarButtonStyle.ToggleButton) 
					tbb.Pushed = ((string)tbb.Tag == toolName);
		}

		// Helper function to enable/disable all the tool buttons on the tool bar
		private void EnableToolButtons(bool enable) 
		{
			foreach (ToolBarButton tbb in toolBar1.Buttons)
				if (tbb.Style == ToolBarButtonStyle.ToggleButton)
					tbb.Enabled = enable;
		}
		
		// Handler function called when the active map's view changes
		private void Map_ViewChanged(object o, ViewChangedEventArgs e) 
		{
			// Get the map
			MapInfo.Mapping.Map map = (MapInfo.Mapping.Map) o;
			// Display the zoom level
			Double dblZoom = System.Convert.ToDouble(String.Format("{0:E2}", mapControl1.Map.Zoom.Value));
			statusBarPanelZoom.Text = "Zoom: " + dblZoom.ToString() + " " + mapControl1.Map.Zoom.Unit.ToString();
		}

		// This method makes sure the tool buttons are enabled only if
		//  the map contains at least one layer.
		private void Layers_CountChanged(object o, CollectionEventArgs e) 
		{
			EnableToolButtons(mapControl1.Map.Layers.Count > 0);
		}

		// This method handles the toolbar's button click event (for all buttons on the bar).
		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (object.ReferenceEquals(e.Button, toolBarButtonFileOpen))
				DisplayOpenFileDialog();
			else if (object.ReferenceEquals(e.Button, toolBarButtonLayerControl))
				DisplayLayerControlDialog();
			else 
			{
				// MapTool name stored in button tag.
				mapControl1.Tools.LeftButtonTool = (string) e.Button.Tag;
				
				// Choice of tool may have been canceled in code, so now
				//  set the toolbar to match the current LeftButton tool
				CheckToolButton(mapControl1.Tools.LeftButtonTool);
			}
		}
#endregion

		#region Code handling events for other controls on the form
		private void chkEventCtrls_CheckedChanged(object sender, System.EventArgs e)
		{
			comboWheelMode.Visible=chkEventCtrls.Checked;
			comboCancelActivation.Visible=chkEventCtrls.Checked;
			comboCancelToolEnding.Visible=chkEventCtrls.Checked;
			lblWheelMode.Visible=chkEventCtrls.Checked;
			lblCancelActivation.Visible=chkEventCtrls.Checked;
			lblCancelUse.Visible=chkEventCtrls.Checked;
			chkCancelNodeEdit.Visible=chkEventCtrls.Checked;
			chkCancelSelect.Visible=chkEventCtrls.Checked;
			chkCancelObjAdd.Visible=chkEventCtrls.Checked;
			chkCancelObjChanges.Visible=chkEventCtrls.Checked;
			chkCancelLabel.Visible=chkEventCtrls.Checked;

			panelMapBorder.Location=new System.Drawing.Point(panelMapBorder.Location.X, chkEventCtrls.Checked ? 128 : 56);
			panelMapBorder.Height=txtResults.Bottom - panelMapBorder.Location.Y;
		}
		
		private void comboWheelMode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch (comboWheelMode.Text)
			{
				case "Disabled":
					mapControl1.MouseWheelSupport=new MouseWheelSupport(
						MouseWheelBehavior.None,1,0);
					break;
				case "Zoom Only":
					mapControl1.MouseWheelSupport=new MouseWheelSupport(
						MouseWheelBehavior.Zoom,2,0);
					break;
				case "Zoom and Scroll":
					mapControl1.MouseWheelSupport=new MouseWheelSupport(
						MouseWheelBehavior.ZoomScroll,2,.1);
					break;
				case "Custom Settings":
					mapControl1.MouseWheelSupport=new MouseWheelSupport(
						MouseWheelBehavior.ZoomScroll,1.05,.01);
					break;
			}
		}

		private void btnRegions_Click(object sender, System.EventArgs e)
		{
			ReloadMap("US_CNTY.TAB");
		}

		private void btnLines_Click(object sender, System.EventArgs e)
		{
			ReloadMap("US_HIWAY.TAB");
		}

		private void btnPoints_Click(object sender, System.EventArgs e)
		{
			ReloadMap("USCTY_1K.TAB");
		}

		private void btnClearMap_Click(object sender, System.EventArgs e)
		{
			ReloadMap("<None>");
		}
		#endregion

		#region Code customizing details of map and form behavior
		private void MapForm1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{  // toggle snap mode if user presses the 'S' key
			if (e.KeyChar=='s')
				mapControl1.Tools.MapToolProperties.SnapEnabled 
					= !mapControl1.Tools.MapToolProperties.SnapEnabled;
		}

		// **************************************************************************
		// The following section contains event code to allow mouse wheel and cursors
		//  to operate correctly whenever the mouse pointer is hovering over the map:

		private void mapControl1_MouseEnter(object sender, System.EventArgs e) 
		{
			// in the case of the AddText tool, it is necessary to make an exception
			//  and turn off the autofocus behavior.  AddText creates a new child window
			//  on the map.  If the exception is not made, the editing process will be
			//  automatically finalized as soon as the mouse moves, because the focus
			//  will be returned to the map control window instead of the child window!
			if ((this.mapControl1.Focused == false) && (this.mapControl1.Tools.LeftButtonTool != "AddText"))
			{
				lastFocus = Control.FromHandle(GetFocus());
				this.mapControl1.Focus();
			}
		}

		private void mapControl1_MouseLeave(object sender, System.EventArgs e) 
		{
			if ((lastFocus != null) && (this.mapControl1.Tools.LeftButtonTool != "AddText"))
				lastFocus.Focus();
		}

		private void MapForm1_Resize(object sender, System.EventArgs e)
		{
			// WindowsKey+M or the Show Desktop button on the Quick Launch toolbar sets the
			//  ActiveForm to null, which would cause an exception in the code following this line:
			if (MapForm1.ActiveForm==null) return;

			// divide available space appropriately between StatusBar panels
			int formWidth=MapForm1.ActiveForm.Width;
			if (formWidth<=720)
				foreach (StatusBarPanel p in statusBar1.Panels)	p.Width = formWidth / 4;
			else
			{
				foreach (StatusBarPanel p in statusBar1.Panels)
				{
					if (p.Text.StartsWith("Zoom:")||p.Text.StartsWith("Edit:")) p.Width=180;
					else p.Width = (formWidth-360) / 2; // Zoom and Edit panels stop growing
				}
			}
		}
		#endregion

		#region Code reporting relevant map, tool, and mouse events to user
		// ***************************************************************************
		// The following section contains the event code used to report details of the
		//  use of the various tools, in the textbox readout to the right of the map

		private void ToolActivating(object sender, ToolActivatingEventArgs e) 
		{
			// if Activation combo-box is set to cancel this tool, cancel it!
			if (e.ToolName == comboCancelActivation.Text) e.Cancel = true;

			txtResults.Text = "ToolActivating: " + e.ToolName + " " + e.ButtonTool
				+ " " + (e.Cancel ? "Cancel: " + e.ToString() : e.ToString());
			txtResults.Refresh();
		}

		private void ToolActivatedFired(object sender, ToolActivatedEventArgs e) 
		{
			txtResults.Text = txtResults.Text + "\r\nToolActivated: " + e.ToolName + " " + e.ButtonTool;
			txtResults.Refresh();
		}

		private void ToolUsed(object sender, ToolUsedEventArgs e) 
		{
			// when starting to use a tool, clear out the Events textbox
			if (e.ToolStatus==ToolStatus.Start) txtResults.Text="";

			txtResults.Text = txtResults.Text + "\r\nToolUsed: " + e.ToString();
			txtResults.Refresh();
		}
	
		private void ToolEnding(object sender, ToolEndingEventArgs e) 
		{
			// if ToolEnding combo-box is set to cancel this tool... cancel it!
			if (e.ToolName == comboCancelToolEnding.Text) e.Cancel = true;

			txtResults.Text = txtResults.Text + "\r\n" + (e.Cancel ? "ToolEnding:  Cancel" : "ToolEnding:");
			txtResults.Refresh();
		}

		private void FeatureAdding(object sender, FeatureAddingEventArgs e) 
		{
			if (chkCancelObjAdd.Checked) e.Cancel=true;
			
			if (e.Feature != null) 
			{
				txtResults.Text = txtResults.Text + "\r\nFeatureAdding: " + (e.Cancel ? "Cancel: " + e.ToString() : e.ToString());
				txtResults.Refresh();
			}
		}

		private void FeatureAdded(object sender, FeatureAddedEventArgs e) 
		{
			if (e.Feature != null) 
			{
				txtResults.Text = txtResults.Text + "\r\nFeatureAdded: " + e.ToString();
				txtResults.Refresh();

				UpdateObjCount();
			}
		}

		private void FeatureSelecting(object sender, FeatureSelectingEventArgs e)
		{
			if (chkCancelSelect.Checked) e.Cancel=true;

			txtResults.Text = txtResults.Text + "\r\nFeatureSelecting:  " + (e.Cancel ? "Cancel" : e.ToString());
			txtResults.Refresh();
		}

		private void FeatureSelected(object sender, FeatureSelectedEventArgs e) 
		{
			txtResults.Text = txtResults.Text + "\r\nFeatureSelected:  " + e.ToString();
			txtResults.Refresh();

			UpdateSelCount();
		}

		private void FeatureChanging(object sender, FeatureChangingEventArgs e) 
		{
			if (chkCancelObjChanges.Checked) e.Cancel=true;

			txtResults.Text = txtResults.Text + "\r\nFeatureChanging:  " + (e.Cancel ? "Cancel" : e.ToString());
			txtResults.Refresh();
		}

		private void FeatureChanged(object sender, MapInfo.Tools.FeatureChangedEventArgs e) 
		{
			txtResults.Text = txtResults.Text + "\r\nFeatureChanged:  " + e.ToString();
			txtResults.Refresh();

			UpdateObjCount();
		}

		private void NodeChanging(object sender, NodeChangingEventArgs e) 
		{
			if (chkCancelNodeEdit.Checked) e.Cancel=true;

			txtResults.Text = txtResults.Text + "\r\nNodeChanging:  " + (e.Cancel ? "Cancel" : e.ToString());
			txtResults.Refresh();
		}

		private void NodeChanged(object sender, NodeChangedEventArgs e) 
		{
			txtResults.Text = txtResults.Text + "\r\nNodeChanged:  " + e.ToString();
			txtResults.Refresh();
		}

		private void LabelAdding(object sender, LabelAddingEventArgs e) 
		{
			if (chkCancelLabel.Checked) e.Cancel=true;

			txtResults.Text = txtResults.Text + "\r\nLabelAdding:  " + (e.Cancel ? "Cancel" : e.ToString());
			txtResults.Refresh();
		}

		private void LabelAdded(object sender, LabelAddedEventArgs e) 
		{
			txtResults.Text = txtResults.Text + "\r\nLabelAdded:  " + e.ToString();
			txtResults.Refresh();
		}

		private void mapControl1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			txtResults.Text=txtResults.Text + "\r\nMouseWheel:  Delta=" + e.Delta
				+ ", X=" + e.X + ", Y=" +e.Y;
			txtResults.Refresh();
		}
		#endregion 

		#region ReloadMap(), UpdateAllControls(), UpdateObjCount(), UpdateSelCount() subs
		// Loads requested table, if any, and [re-]creates in-memory Temp table
		private void ReloadMap(string strFName)
		{
			MapInfo.Geometry.DPoint ctr=mapControl1.Map.Center;
			MapInfo.Geometry.Distance z=mapControl1.Map.Zoom;
			txtResults.Text="";

			if (strFName == "<None>")
			{
				// first close the temporary table -- an easy way to clean it up
				// (otherwise the table will remain open after the map is cleared)
				MapInfo.Data.Table tblTemp=Session.Current.Catalog.GetTable("Temp");
				if (tblTemp!=null) tblTemp.Close();
				mapControl1.Map.Clear();

				// create a temporary table and add a featurelayer for it
				FeatureLayer layerTemp=new FeatureLayer(
					Session.Current.Catalog.CreateTable(
					TableInfoFactory.CreateTemp("Temp"), new TableSessionInfo()));
				mapControl1.Map.Layers.Add(layerTemp);
				
				// set the insertion and edit filters to allow all tools to work on Temp
				ToolFilter toolFilter = 
					(ToolFilter) mapControl1.Tools.AddMapToolProperties.InsertionLayerFilter ;
				if (toolFilter != null && !toolFilter.IncludeLayer(layerTemp) ) 
					toolFilter.SetExplicitInclude(layerTemp, true); 
				toolFilter=(ToolFilter) mapControl1.Tools.SelectMapToolProperties.EditableLayerFilter;
				if (toolFilter != null && !toolFilter.IncludeLayer(layerTemp) ) 
					toolFilter.SetExplicitInclude(layerTemp, true); 

				txtResults.Text="A new in-memory temporary table (named 'Temp') "
					+ "has been added to the map.\r\n\r\nObjects can be added to this table with the "
					+ "Add tools; the objects can then be edited with the Select tool.";
			
				// this sample will automatically load point, line, and region tables
				//  in North America -- so set an appropriate view:
				mapControl1.Map.SetView(new DPoint(-100,30),
					Session.Current.CoordSysFactory.CreateLongLat(DatumID.WGS84),
					60000000);
			}
			else
			{
				// figure out what path to use for data
				// check the .csproj directory, two levels up from the .EXE

				string strPath=Application.StartupPath;
				strPath=strPath.Substring(0,strPath.LastIndexOf(Path.DirectorySeparatorChar)); // get rid of \Debug
				strPath=strPath.Substring(0,strPath.LastIndexOf(Path.DirectorySeparatorChar)+1) // get rid of \bin
					+ "Data" + Path.DirectorySeparatorChar;
				if (File.Exists(strPath + strFName)==false)
				{
					// check the registry for a known sample data search path
					Microsoft.Win32.RegistryKey keySamp = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\MapInfo\MapXtreme\6.6");
					if (keySamp != null)
					{
						string s = (string)keySamp.GetValue("SampleDataSearchPath");
						keySamp.Close();

						if (s == null) s = Environment.CurrentDirectory;
						else if (s.EndsWith(Path.DirectorySeparatorChar.ToString())==false)
							s += Path.DirectorySeparatorChar;
						strPath=s;
					}
				}

				if (File.Exists(strPath+strFName))
				{
					mapControl1.Map.Load(new MapTableLoader(strPath + strFName));
					mapControl1.Map.Center=ctr;
					mapControl1.Map.Zoom=z;
				}
				else
				{
					txtResults.Text="Couldn't find table: \r\n"
						+ strPath + strFName
						+ "\r\n\r\nTo enable this function, copy the relevant files "
						+ "to the above location, or reinstall the sample data.\r\n\r\n";
				}
			}
			UpdateAllControls();
		}

		private void UpdateAllControls()
		{
			UpdateObjCount();
			UpdateSelCount();

			// the following are defined in LayerFilterStatusBar.cs
			statusBar1.UpdateSelectText(); 
			statusBar1.UpdateEditText(); 
			statusBar1.UpdateInsertText(); 
		}	
			
		private void UpdateObjCount()
		{
			FeatureLayer lyrTemp=(FeatureLayer) mapControl1.Map.Layers["Temp"];
			if (lyrTemp!=null)
			{
				IFeatureEnumerator fen = (lyrTemp.Table as IFeatureCollection).GetFeatureEnumerator();

				int n=0;
				while (fen.MoveNext()) n++;
				txtNumObjects.Text=n.ToString();
			}
		}

		private void UpdateSelCount()
		{
			IEnumerator se = MapInfo.Engine.Session.Current.Selections.GetEnumerator();
			int i=0;
			while (se.MoveNext()) i+= ((Selection)se.Current).TotalCount;
			txtNumSelected.Text=i.ToString();
		}
		#endregion
	}
}