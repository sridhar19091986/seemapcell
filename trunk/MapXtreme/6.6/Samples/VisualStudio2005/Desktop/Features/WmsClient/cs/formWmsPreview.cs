using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using MapInfo.Data;
using MapInfo.Engine;
using MapInfo.Geometry;
using MapInfo.Windows.Controls;
using MapInfo.Wms;

namespace WmsPreview
{
	/// <summary>
	/// Summary description for formWmsPreview.
	/// </summary>
	public class formWmsPreview : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ComboBox comboBoxServers;
		private System.Windows.Forms.TextBox textBoxURL;
		private System.Windows.Forms.TextBox textBoxVersion;
		private System.Windows.Forms.ComboBox comboBoxImageFormat;
		private System.Windows.Forms.Label labelImageFormat;
		private System.Windows.Forms.Label labelServerURL;
		private System.Windows.Forms.Label labelWMSServer;
		private System.Windows.Forms.GroupBox groupBoxCRS;
		private System.Windows.Forms.ComboBox comboBoxProjection;
		private System.Windows.Forms.GroupBox groupBoxImageBackground;
		private System.Windows.Forms.PictureBox pictureBoxColor;
		private System.Windows.Forms.CheckBox checkBoxTransparent;
		private System.Windows.Forms.GroupBox groupBoxWMSLayers;
		private System.Windows.Forms.TreeView treeViewServerLayers;
		private System.Windows.Forms.ListBox listBoxClientLayers;
		private System.Windows.Forms.TextBox textBoxStyle;
		private System.Windows.Forms.Label labelStyle;
		private System.Windows.Forms.ComboBox comboBoxStyle;
		private System.Windows.Forms.Button buttonMoveDown;
		private System.Windows.Forms.Button buttonMoveUp;
		private System.Windows.Forms.Button buttonRemove;
		private System.Windows.Forms.Button buttonAdd;
		private System.Windows.Forms.GroupBox groupBoxWmsClient;
		private System.Windows.Forms.ComboBox comboBoxLayerStyle;
		private System.Windows.Forms.Label labelLayerStyle;
		private System.Windows.Forms.TextBox textBoxLayerStyle;
		private System.Windows.Forms.Button buttonViewEntire;
		private System.Windows.Forms.ToolBar toolBarMap;
		private System.Windows.Forms.ToolBarButton toolBarButtonZoomIn;
		private System.Windows.Forms.ToolBarButton toolBarButtonPan;
		private System.Windows.Forms.ToolBarButton toolBarButtonZoomOut;
		private System.Windows.Forms.ImageList imageListTools;

		private MapInfo.Windows.Controls.MapControl mapControlWms;

		private System.ComponentModel.IContainer components;

		private ToolBarButton _currentToolButton = null;
		private WmsServerInfo _serverInfo = null;
		private Cursor _currentCursor = Cursor.Current;
		private WmsCapabilitiesState _wmsCapabilitiesState;
		private WmsClientState _wmsClientState;
		private FeatureLayerState _featureLayerState;
		private ServerList _serverList;

		private WmsStyleState _capabilitiesStyleState;
		private WmsStyleState _clientStyleState;
		private WmsLayerState _capabilitiesLayerState;
		private WmsLayerState _clientLayerState;
		private System.Windows.Forms.TextBox textBoxSrsDescription;
		private WmsSrsState _srsState;

		public formWmsPreview()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			MapInfo.Engine.Session.Current.CoordSysFactory.LoadDefaultProjectionFile();

			_capabilitiesStyleState = new WmsStyleState(textBoxStyle);
			_clientStyleState = new WmsStyleState(textBoxLayerStyle);

			_capabilitiesLayerState = new WmsLayerState(null, comboBoxStyle, _capabilitiesStyleState);
			_clientLayerState = new WmsLayerState(null, comboBoxLayerStyle, _clientStyleState);

			_srsState = new WmsSrsState(textBoxSrsDescription);

			_wmsCapabilitiesState = new WmsCapabilitiesState(textBoxURL, textBoxVersion, treeViewServerLayers, comboBoxImageFormat, _capabilitiesLayerState);
			_wmsClientState = new WmsClientState(comboBoxImageFormat, checkBoxTransparent, pictureBoxColor, comboBoxProjection, _srsState, listBoxClientLayers, _clientLayerState);
			_featureLayerState = new FeatureLayerState(mapControlWms);
			_serverList = new ServerList(comboBoxServers);

			_currentToolButton = toolBarButtonZoomIn;
			_currentToolButton.Pushed = true;
			mapControlWms.Tools.LeftButtonTool = _currentToolButton.Tag as string;
			mapControlWms.Map.DrawEvent += new MapInfo.Mapping.MapDrawEventHandler(Map_DrawEvent);
			mapControlWms.PaintException += new PaintExceptionEventHandler(mapControlWms_PaintException);

			ToolBarSetup();
			_serverList.Set();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
				mapControlWms.Map.DrawEvent -= new MapInfo.Mapping.MapDrawEventHandler(Map_DrawEvent);
				mapControlWms.PaintException -= new PaintExceptionEventHandler(mapControlWms_PaintException);
				mapControlWms.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(formWmsPreview));
			this.textBoxURL = new System.Windows.Forms.TextBox();
			this.comboBoxImageFormat = new System.Windows.Forms.ComboBox();
			this.labelImageFormat = new System.Windows.Forms.Label();
			this.comboBoxServers = new System.Windows.Forms.ComboBox();
			this.labelServerURL = new System.Windows.Forms.Label();
			this.labelWMSServer = new System.Windows.Forms.Label();
			this.groupBoxCRS = new System.Windows.Forms.GroupBox();
			this.comboBoxProjection = new System.Windows.Forms.ComboBox();
			this.textBoxSrsDescription = new System.Windows.Forms.TextBox();
			this.groupBoxImageBackground = new System.Windows.Forms.GroupBox();
			this.pictureBoxColor = new System.Windows.Forms.PictureBox();
			this.checkBoxTransparent = new System.Windows.Forms.CheckBox();
			this.groupBoxWMSLayers = new System.Windows.Forms.GroupBox();
			this.treeViewServerLayers = new System.Windows.Forms.TreeView();
			this.textBoxStyle = new System.Windows.Forms.TextBox();
			this.labelStyle = new System.Windows.Forms.Label();
			this.comboBoxStyle = new System.Windows.Forms.ComboBox();
			this.buttonAdd = new System.Windows.Forms.Button();
			this.listBoxClientLayers = new System.Windows.Forms.ListBox();
			this.buttonMoveDown = new System.Windows.Forms.Button();
			this.buttonMoveUp = new System.Windows.Forms.Button();
			this.buttonRemove = new System.Windows.Forms.Button();
			this.mapControlWms = new MapInfo.Windows.Controls.MapControl();
			this.groupBoxWmsClient = new System.Windows.Forms.GroupBox();
			this.comboBoxLayerStyle = new System.Windows.Forms.ComboBox();
			this.labelLayerStyle = new System.Windows.Forms.Label();
			this.textBoxLayerStyle = new System.Windows.Forms.TextBox();
			this.textBoxVersion = new System.Windows.Forms.TextBox();
			this.toolBarMap = new System.Windows.Forms.ToolBar();
			this.toolBarButtonZoomIn = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonZoomOut = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonPan = new System.Windows.Forms.ToolBarButton();
			this.imageListTools = new System.Windows.Forms.ImageList(this.components);
			this.buttonViewEntire = new System.Windows.Forms.Button();
			this.groupBoxCRS.SuspendLayout();
			this.groupBoxImageBackground.SuspendLayout();
			this.groupBoxWMSLayers.SuspendLayout();
			this.groupBoxWmsClient.SuspendLayout();
			this.SuspendLayout();
			// 
			// textBoxURL
			// 
			this.textBoxURL.Location = new System.Drawing.Point(112, 48);
			this.textBoxURL.Name = "textBoxURL";
			this.textBoxURL.ReadOnly = true;
			this.textBoxURL.Size = new System.Drawing.Size(600, 20);
			this.textBoxURL.TabIndex = 23;
			this.textBoxURL.Text = "";
			// 
			// comboBoxImageFormat
			// 
			this.comboBoxImageFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxImageFormat.Location = new System.Drawing.Point(384, 432);
			this.comboBoxImageFormat.Name = "comboBoxImageFormat";
			this.comboBoxImageFormat.Size = new System.Drawing.Size(121, 21);
			this.comboBoxImageFormat.Sorted = true;
			this.comboBoxImageFormat.TabIndex = 28;
			this.comboBoxImageFormat.SelectedIndexChanged += new System.EventHandler(this.comboBoxImageFormat_SelectedIndexChanged);
			// 
			// labelImageFormat
			// 
			this.labelImageFormat.Location = new System.Drawing.Point(296, 432);
			this.labelImageFormat.Name = "labelImageFormat";
			this.labelImageFormat.Size = new System.Drawing.Size(88, 23);
			this.labelImageFormat.TabIndex = 27;
			this.labelImageFormat.Text = "&Image Format:";
			// 
			// comboBoxServers
			// 
			this.comboBoxServers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxServers.Location = new System.Drawing.Point(112, 16);
			this.comboBoxServers.Name = "comboBoxServers";
			this.comboBoxServers.Size = new System.Drawing.Size(600, 21);
			this.comboBoxServers.Sorted = true;
			this.comboBoxServers.TabIndex = 22;
			this.comboBoxServers.SelectedIndexChanged += new System.EventHandler(this.comboBoxServers_SelectedIndexChanged);
			// 
			// labelServerURL
			// 
			this.labelServerURL.Location = new System.Drawing.Point(16, 48);
			this.labelServerURL.Name = "labelServerURL";
			this.labelServerURL.Size = new System.Drawing.Size(88, 23);
			this.labelServerURL.TabIndex = 18;
			this.labelServerURL.Text = "Ser&ver URL:";
			// 
			// labelWMSServer
			// 
			this.labelWMSServer.Location = new System.Drawing.Point(16, 16);
			this.labelWMSServer.Name = "labelWMSServer";
			this.labelWMSServer.Size = new System.Drawing.Size(88, 23);
			this.labelWMSServer.TabIndex = 17;
			this.labelWMSServer.Text = "&WMS Server:";
			// 
			// groupBoxCRS
			// 
			this.groupBoxCRS.Controls.Add(this.comboBoxProjection);
			this.groupBoxCRS.Controls.Add(this.textBoxSrsDescription);
			this.groupBoxCRS.Location = new System.Drawing.Point(286, 536);
			this.groupBoxCRS.Name = "groupBoxCRS";
			this.groupBoxCRS.Size = new System.Drawing.Size(300, 96);
			this.groupBoxCRS.TabIndex = 30;
			this.groupBoxCRS.TabStop = false;
			this.groupBoxCRS.Text = "Coordinate Reference System:";
			// 
			// comboBoxProjection
			// 
			this.comboBoxProjection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxProjection.Location = new System.Drawing.Point(16, 24);
			this.comboBoxProjection.Name = "comboBoxProjection";
			this.comboBoxProjection.Size = new System.Drawing.Size(224, 21);
			this.comboBoxProjection.Sorted = true;
			this.comboBoxProjection.TabIndex = 0;
			this.comboBoxProjection.SelectedIndexChanged += new System.EventHandler(this.comboBoxProjection_SelectedIndexChanged);
			// 
			// textBoxSrsDescription
			// 
			this.textBoxSrsDescription.Location = new System.Drawing.Point(8, 64);
			this.textBoxSrsDescription.Name = "textBoxSrsDescription";
			this.textBoxSrsDescription.ReadOnly = true;
			this.textBoxSrsDescription.Size = new System.Drawing.Size(280, 20);
			this.textBoxSrsDescription.TabIndex = 3;
			this.textBoxSrsDescription.Text = "";
			// 
			// groupBoxImageBackground
			// 
			this.groupBoxImageBackground.Controls.Add(this.pictureBoxColor);
			this.groupBoxImageBackground.Controls.Add(this.checkBoxTransparent);
			this.groupBoxImageBackground.Location = new System.Drawing.Point(288, 464);
			this.groupBoxImageBackground.Name = "groupBoxImageBackground";
			this.groupBoxImageBackground.Size = new System.Drawing.Size(168, 64);
			this.groupBoxImageBackground.TabIndex = 29;
			this.groupBoxImageBackground.TabStop = false;
			this.groupBoxImageBackground.Text = "Image Background:";
			// 
			// pictureBoxColor
			// 
			this.pictureBoxColor.Location = new System.Drawing.Point(120, 16);
			this.pictureBoxColor.Name = "pictureBoxColor";
			this.pictureBoxColor.Size = new System.Drawing.Size(32, 32);
			this.pictureBoxColor.TabIndex = 2;
			this.pictureBoxColor.TabStop = false;
			this.pictureBoxColor.Click += new System.EventHandler(this.pictureBoxColor_Click);
			// 
			// checkBoxTransparent
			// 
			this.checkBoxTransparent.Checked = true;
			this.checkBoxTransparent.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxTransparent.Location = new System.Drawing.Point(8, 24);
			this.checkBoxTransparent.Name = "checkBoxTransparent";
			this.checkBoxTransparent.Size = new System.Drawing.Size(96, 24);
			this.checkBoxTransparent.TabIndex = 0;
			this.checkBoxTransparent.Text = "&Transparent";
			this.checkBoxTransparent.CheckedChanged += new System.EventHandler(this.checkBoxTransparent_CheckedChanged);
			// 
			// groupBoxWMSLayers
			// 
			this.groupBoxWMSLayers.Controls.Add(this.treeViewServerLayers);
			this.groupBoxWMSLayers.Controls.Add(this.textBoxStyle);
			this.groupBoxWMSLayers.Controls.Add(this.labelStyle);
			this.groupBoxWMSLayers.Controls.Add(this.comboBoxStyle);
			this.groupBoxWMSLayers.Controls.Add(this.buttonAdd);
			this.groupBoxWMSLayers.Location = new System.Drawing.Point(8, 88);
			this.groupBoxWMSLayers.Name = "groupBoxWMSLayers";
			this.groupBoxWMSLayers.Size = new System.Drawing.Size(264, 488);
			this.groupBoxWMSLayers.TabIndex = 26;
			this.groupBoxWMSLayers.TabStop = false;
			this.groupBoxWMSLayers.Text = "Available WMS Layers";
			// 
			// treeViewServerLayers
			// 
			this.treeViewServerLayers.HideSelection = false;
			this.treeViewServerLayers.ImageIndex = -1;
			this.treeViewServerLayers.Location = new System.Drawing.Point(8, 24);
			this.treeViewServerLayers.Name = "treeViewServerLayers";
			this.treeViewServerLayers.SelectedImageIndex = -1;
			this.treeViewServerLayers.Size = new System.Drawing.Size(250, 336);
			this.treeViewServerLayers.TabIndex = 20;
			this.treeViewServerLayers.EnabledChanged += new System.EventHandler(this.treeViewServerLayers_EnabledChanged);
			this.treeViewServerLayers.DoubleClick += new System.EventHandler(this.treeViewServerLayers_DoubleClick);
			this.treeViewServerLayers.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewServerLayers_AfterSelect);
			// 
			// textBoxStyle
			// 
			this.textBoxStyle.Location = new System.Drawing.Point(8, 448);
			this.textBoxStyle.Name = "textBoxStyle";
			this.textBoxStyle.ReadOnly = true;
			this.textBoxStyle.Size = new System.Drawing.Size(248, 20);
			this.textBoxStyle.TabIndex = 17;
			this.textBoxStyle.Text = "";
			// 
			// labelStyle
			// 
			this.labelStyle.Location = new System.Drawing.Point(16, 408);
			this.labelStyle.Name = "labelStyle";
			this.labelStyle.Size = new System.Drawing.Size(48, 23);
			this.labelStyle.TabIndex = 16;
			this.labelStyle.Text = "St&yle:";
			// 
			// comboBoxStyle
			// 
			this.comboBoxStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxStyle.Location = new System.Drawing.Point(72, 408);
			this.comboBoxStyle.Name = "comboBoxStyle";
			this.comboBoxStyle.Size = new System.Drawing.Size(176, 21);
			this.comboBoxStyle.TabIndex = 15;
			// 
			// buttonAdd
			// 
			this.buttonAdd.Enabled = false;
			this.buttonAdd.Location = new System.Drawing.Point(72, 376);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.TabIndex = 11;
			this.buttonAdd.Text = "&Add>>";
			this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
			// 
			// listBoxClientLayers
			// 
			this.listBoxClientLayers.AllowDrop = true;
			this.listBoxClientLayers.Location = new System.Drawing.Point(608, 112);
			this.listBoxClientLayers.Name = "listBoxClientLayers";
			this.listBoxClientLayers.Size = new System.Drawing.Size(250, 225);
			this.listBoxClientLayers.TabIndex = 19;
			this.listBoxClientLayers.EnabledChanged += new System.EventHandler(this.listBoxTableLayers_EnabledChanged);
			this.listBoxClientLayers.SelectedIndexChanged += new System.EventHandler(this.listBoxTableLayers_SelectedIndexChanged);
			// 
			// buttonMoveDown
			// 
			this.buttonMoveDown.Enabled = false;
			this.buttonMoveDown.Location = new System.Drawing.Point(696, 488);
			this.buttonMoveDown.Name = "buttonMoveDown";
			this.buttonMoveDown.TabIndex = 14;
			this.buttonMoveDown.Text = "Move &Down";
			this.buttonMoveDown.Click += new System.EventHandler(this.buttonMoveDown_Click);
			// 
			// buttonMoveUp
			// 
			this.buttonMoveUp.Enabled = false;
			this.buttonMoveUp.Location = new System.Drawing.Point(696, 456);
			this.buttonMoveUp.Name = "buttonMoveUp";
			this.buttonMoveUp.TabIndex = 13;
			this.buttonMoveUp.Text = "Move &Up";
			this.buttonMoveUp.Click += new System.EventHandler(this.buttonMoveUp_Click);
			// 
			// buttonRemove
			// 
			this.buttonRemove.Enabled = false;
			this.buttonRemove.Location = new System.Drawing.Point(696, 424);
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.TabIndex = 12;
			this.buttonRemove.Text = "&Remove<<";
			this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
			// 
			// mapControlWms
			// 
			this.mapControlWms.Location = new System.Drawing.Point(288, 120);
			this.mapControlWms.Name = "mapControlWms";
			this.mapControlWms.Size = new System.Drawing.Size(300, 300);
			this.mapControlWms.TabIndex = 31;
			this.mapControlWms.TabStop = false;
			this.mapControlWms.Text = "mapControlWms";
			this.mapControlWms.Tools.LeftButtonTool = "Arrow";
			// 
			// groupBoxWmsClient
			// 
			this.groupBoxWmsClient.Controls.Add(this.comboBoxLayerStyle);
			this.groupBoxWmsClient.Controls.Add(this.labelLayerStyle);
			this.groupBoxWmsClient.Controls.Add(this.textBoxLayerStyle);
			this.groupBoxWmsClient.Location = new System.Drawing.Point(600, 88);
			this.groupBoxWmsClient.Name = "groupBoxWmsClient";
			this.groupBoxWmsClient.Size = new System.Drawing.Size(264, 448);
			this.groupBoxWmsClient.TabIndex = 32;
			this.groupBoxWmsClient.TabStop = false;
			this.groupBoxWmsClient.Text = "Layer Control";
			// 
			// comboBoxLayerStyle
			// 
			this.comboBoxLayerStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxLayerStyle.Location = new System.Drawing.Point(80, 264);
			this.comboBoxLayerStyle.Name = "comboBoxLayerStyle";
			this.comboBoxLayerStyle.Size = new System.Drawing.Size(121, 21);
			this.comboBoxLayerStyle.TabIndex = 33;
			this.comboBoxLayerStyle.SelectedIndexChanged += new System.EventHandler(this.comboBoxLayerStyle_SelectedIndexChanged);
			// 
			// labelLayerStyle
			// 
			this.labelLayerStyle.Location = new System.Drawing.Point(16, 264);
			this.labelLayerStyle.Name = "labelLayerStyle";
			this.labelLayerStyle.Size = new System.Drawing.Size(48, 23);
			this.labelLayerStyle.TabIndex = 33;
			this.labelLayerStyle.Text = "St&yle:";
			// 
			// textBoxLayerStyle
			// 
			this.textBoxLayerStyle.Location = new System.Drawing.Point(8, 296);
			this.textBoxLayerStyle.Name = "textBoxLayerStyle";
			this.textBoxLayerStyle.ReadOnly = true;
			this.textBoxLayerStyle.Size = new System.Drawing.Size(248, 20);
			this.textBoxLayerStyle.TabIndex = 33;
			this.textBoxLayerStyle.Text = "";
			// 
			// textBoxVersion
			// 
			this.textBoxVersion.Location = new System.Drawing.Point(728, 48);
			this.textBoxVersion.Name = "textBoxVersion";
			this.textBoxVersion.ReadOnly = true;
			this.textBoxVersion.TabIndex = 33;
			this.textBoxVersion.Text = "";
			// 
			// toolBarMap
			// 
			this.toolBarMap.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																																									this.toolBarButtonZoomIn,
																																									this.toolBarButtonZoomOut,
																																									this.toolBarButtonPan});
			this.toolBarMap.Dock = System.Windows.Forms.DockStyle.None;
			this.toolBarMap.DropDownArrows = true;
			this.toolBarMap.ImageList = this.imageListTools;
			this.toolBarMap.Location = new System.Drawing.Point(288, 80);
			this.toolBarMap.Name = "toolBarMap";
			this.toolBarMap.ShowToolTips = true;
			this.toolBarMap.Size = new System.Drawing.Size(296, 28);
			this.toolBarMap.TabIndex = 34;
			this.toolBarMap.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarMap_ButtonClick);
			// 
			// toolBarButtonZoomIn
			// 
			this.toolBarButtonZoomIn.Pushed = true;
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
			// imageListTools
			// 
			this.imageListTools.ColorDepth = System.Windows.Forms.ColorDepth.Depth4Bit;
			this.imageListTools.ImageSize = new System.Drawing.Size(18, 16);
			this.imageListTools.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTools.ImageStream")));
			this.imageListTools.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// buttonViewEntire
			// 
			this.buttonViewEntire.Location = new System.Drawing.Point(480, 480);
			this.buttonViewEntire.Name = "buttonViewEntire";
			this.buttonViewEntire.TabIndex = 34;
			this.buttonViewEntire.Text = "View Entire";
			this.buttonViewEntire.Click += new System.EventHandler(this.buttonViewEntire_Click);
			// 
			// formWmsPreview
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(872, 646);
			this.Controls.Add(this.toolBarMap);
			this.Controls.Add(this.textBoxVersion);
			this.Controls.Add(this.mapControlWms);
			this.Controls.Add(this.textBoxURL);
			this.Controls.Add(this.comboBoxImageFormat);
			this.Controls.Add(this.labelImageFormat);
			this.Controls.Add(this.comboBoxServers);
			this.Controls.Add(this.labelServerURL);
			this.Controls.Add(this.labelWMSServer);
			this.Controls.Add(this.groupBoxCRS);
			this.Controls.Add(this.groupBoxImageBackground);
			this.Controls.Add(this.groupBoxWMSLayers);
			this.Controls.Add(this.buttonRemove);
			this.Controls.Add(this.buttonMoveUp);
			this.Controls.Add(this.buttonMoveDown);
			this.Controls.Add(this.listBoxClientLayers);
			this.Controls.Add(this.groupBoxWmsClient);
			this.Controls.Add(this.buttonViewEntire);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "formWmsPreview";
			this.Text = "WMS Preview";
			this.groupBoxCRS.ResumeLayout(false);
			this.groupBoxImageBackground.ResumeLayout(false);
			this.groupBoxWMSLayers.ResumeLayout(false);
			this.groupBoxWmsClient.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			try
			{
				Application.Run(new formWmsPreview());
			}
			catch (ApplicationException ex)
			{
				MessageBox.Show(ex.Message);
				Application.Exit();
			}
		}

		// This method reads creates a bitmap object from resources
		private Bitmap GetBitmapResource(string bitmapName)
		{
			return new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType(), bitmapName));
		}

		// This method creates an imagelist from bitmap resources
		private ImageList CreateImageList()
		{
			// Create the list
			ImageList imageList = new ImageList();
			imageList.ImageSize = new Size(18, 16);
			imageList.ColorDepth = ColorDepth.Depth4Bit;
			imageList.TransparentColor = Color.FromArgb(192, 192, 192);

			// Add bitmaps
			imageList.Images.AddStrip(GetBitmapResource("standlo1.bmp"));

			return imageList;
		}

		// This methods associates the imagelist with the toolbar
		// and assigns images to each tool button.
		private void ToolBarSetup()
		{
			toolBarMap.ImageList = CreateImageList();
			toolBarButtonZoomIn.ImageIndex = 4;
			toolBarButtonZoomOut.ImageIndex = 5;
			toolBarButtonPan.ImageIndex = 7;

			// Make the select tool active by default
			toolBarButtonZoomIn.Pushed = true;
			CheckToolButton(mapControlWms.Tools.LeftButtonTool);
		}

		// Helper function to check one tool button and uncheck all the rest
		private void CheckToolButton(string toolName)
		{
			foreach (ToolBarButton tbb in toolBarMap.Buttons)
			{
				if (tbb.Style == ToolBarButtonStyle.ToggleButton)
				{
					if ((string)tbb.Tag == toolName)
					{
						tbb.Pushed = true;
					}
					else
					{
						tbb.Pushed = false;
					}
				}
			}
		}

		private static void showError(string strMessage)
		{
			System.Windows.Forms.MessageBox.Show(formWmsPreview.ActiveForm, strMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void comboBoxServers_SelectedIndexChanged(object sender, EventArgs e)
		{
			_currentCursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			_wmsCapabilitiesState.Clear();
			_wmsClientState.Clear();
			_featureLayerState.Clear();
			this.Refresh();
			_serverInfo = comboBoxServers.SelectedItem as WmsServerInfo;
			try
			{
				_wmsCapabilitiesState.Set(_serverInfo.Capabilities);
				_wmsClientState.Set(_serverInfo.Client);
				_featureLayerState.Set(_serverInfo.FeatureLayer);
			}
			catch (System.Net.WebException x)
			{
				showError(x.Message);
			}
			catch (System.ApplicationException x)
			{
				showError(x.Message);
			}
			Cursor.Current = _currentCursor;
		}

		private void treeViewServerLayers_AfterSelect(object sender, TreeViewEventArgs e)
		{
			IWmsLayer layer = e.Node.Tag as IWmsLayer;
			buttonAdd.Enabled = layer.Name != null;
		}

		private void treeViewServerLayers_EnabledChanged(object sender, EventArgs e)
		{
			if (!(sender as System.Windows.Forms.TreeView).Enabled)
			{
				buttonAdd.Enabled = false;
			}
		}

		private void listBoxTableLayers_SelectedIndexChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.ListBox listBox = sender as System.Windows.Forms.ListBox;
			buttonRemove.Enabled = listBox.SelectedIndex >= 0;
			buttonMoveUp.Enabled = listBox.SelectedIndex > 0;
			buttonMoveDown.Enabled = listBox.SelectedIndex >= 0 && listBox.SelectedIndex < listBox.Items.Count - 1;
		}

		private void treeViewServerLayers_DoubleClick(object sender, EventArgs e)
		{
			if (treeViewServerLayers.SelectedNode.FirstNode == null)
			{
				AddSelectedLayer();
			}
		}

		private void listBoxTableLayers_EnabledChanged(object sender, EventArgs e)
		{
			if (!(sender as System.Windows.Forms.ListBox).Enabled)
			{
				buttonRemove.Enabled = false;
				buttonMoveUp.Enabled = false;
				buttonMoveDown.Enabled = false;
			}
		}

		private void buttonAdd_Click(object sender, EventArgs e)
		{
			AddSelectedLayer();
		}

		private void AddSelectedLayer()
		{
			string srs = _serverInfo.Client.Srs;
			_serverInfo.Client.AddLayer((treeViewServerLayers.SelectedNode.Tag as IWmsLayer).Name, SelectedStyle(comboBoxStyle));
			srs = _serverInfo.Client.Srs;
		}

		private void pictureBoxColor_Click(object sender, EventArgs e)
		{
			ColorDialog colorDialog = new ColorDialog();
			colorDialog.Color = _serverInfo.Client.BGColor;
			if (colorDialog.ShowDialog() == DialogResult.OK)
			{
				pictureBoxColor.BackColor = _serverInfo.Client.BGColor = colorDialog.Color;
			}
		}

		private void comboBoxLayerStyle_SelectedIndexChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.ComboBox comboBox = sender as System.Windows.Forms.ComboBox;
			_serverInfo.Client.ChangeStyle(listBoxClientLayers.SelectedIndex, SelectedStyle(comboBox));
		}

		private void comboBoxImageFormat_SelectedIndexChanged(object sender, EventArgs e)
		{
			_serverInfo.Client.MimeType = comboBoxImageFormat.SelectedItem as string;
		}

		private void checkBoxTransparent_CheckedChanged(object sender, EventArgs e)
		{
			_serverInfo.Client.Transparent = (sender as System.Windows.Forms.CheckBox).Checked;
		}

		private void buttonMoveDown_Click(object sender, EventArgs e)
		{
			_serverInfo.Client.MoveLayerDown(listBoxClientLayers.SelectedIndex);
		}

		private void buttonMoveUp_Click(object sender, EventArgs e)
		{
			_serverInfo.Client.MoveLayerUp(listBoxClientLayers.SelectedIndex);
		}

		private void buttonRemove_Click(object sender, EventArgs e)
		{
			_serverInfo.Client.RemoveLayer(listBoxClientLayers.SelectedIndex);
		}

		private void toolBarMap_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			_currentToolButton.Pushed = false;
			_currentToolButton = e.Button;
			if (object.ReferenceEquals(e.Button, this.toolBarButtonZoomIn) == true)
			{
				mapControlWms.Tools.LeftButtonTool = "ZoomIn";
			}
			else if (object.ReferenceEquals(e.Button, this.toolBarButtonZoomOut) == true)
			{
				mapControlWms.Tools.LeftButtonTool = "ZoomOut";
			}
			else if (object.ReferenceEquals(e.Button, this.toolBarButtonPan) == true)
			{
				mapControlWms.Tools.LeftButtonTool = "Pan";
			}
		}

		private void buttonViewEntire_Click(object sender, EventArgs e)
		{
			mapControlWms.Map.Bounds = _featureLayerState.FeatureLayer.Bounds;
		}

		private void comboBoxProjection_SelectedIndexChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.ComboBox comboBox = sender as System.Windows.Forms.ComboBox;
			_serverInfo.Client.Srs = comboBox.SelectedItem as String;
		}

		private void Map_DrawEvent(object sender, MapInfo.Mapping.MapDrawEventArgs e)
		{
			if (e.Status == MapInfo.Mapping.DrawStatus.BeginDraw)
			{
				_currentCursor = Cursor.Current;
				Cursor.Current = Cursors.WaitCursor;
			}
			else
			{
				Cursor.Current = _currentCursor;
			}
		}

		private void mapControlWms_PaintException(object sender, PaintExceptionEventArgs e)
		{
			showError(e.Exception.Message);
		}

		private static string SelectedStyle(System.Windows.Forms.ComboBox comboBox)
		{
			return comboBox.SelectedIndex <= 0 ? null : (comboBox.SelectedItem as IWmsStyle).Name;
		}
	}
}