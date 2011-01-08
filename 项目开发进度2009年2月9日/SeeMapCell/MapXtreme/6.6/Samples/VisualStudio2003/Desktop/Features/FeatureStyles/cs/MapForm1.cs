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
using MapInfo.Geometry;
using MapInfo.Text;
using MapInfo.Styles;
using MapInfo.Windows.Dialogs;

namespace Styles
{
	/// <summary>
	/// Summary description for MapForm1.
	/// </summary>
	public class MapForm1 : System.Windows.Forms.Form
	{
		private SimpleLineStyle _lineStyle = null;
		private SimpleInterior _fillStyle = null;
		private SimpleVectorPointStyle _vectorSymbol=null;
		private BitmapPointStyle _bitmapSymbol=null;
		private FontPointStyle _fontSymbol=null;
		private TextStyle _textStyle=null;
    private double _textAngle=0;
    private Alignment _textAlignment = Alignment.CenterCenter;
    private Spacing _textSpacing = Spacing.Single;
		private LineStyleDlg _lineStyleDlg = null; 
		private AreaStyleDlg _areaStyleDlg = null; 
		private TextStyleDlg _textStyleDlg = null;
		private SymbolStyleDlg _symbolStyleDlg = null;

		#region control declarations	

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabLines;
		private System.Windows.Forms.TabPage tabStock;
		private System.Windows.Forms.TabPage tabFills;
		private System.Windows.Forms.TabPage tabFontSymbols;
		private System.Windows.Forms.TabPage tabVectorSymbols;
		private System.Windows.Forms.TabPage tabBitmapSymbols;
		private System.Windows.Forms.TabPage tabText;
		private System.Windows.Forms.RadioButton radioBlackFillStyle;
		private System.Windows.Forms.RadioButton radioBlackLineStyle;
		private System.Windows.Forms.RadioButton radioBlueFillStyle;
		private System.Windows.Forms.RadioButton radioHollowFillStyle;
		private System.Windows.Forms.RadioButton radioRedFillStyle;
		private System.Windows.Forms.RadioButton radioWhiteFillStyle;
		private System.Windows.Forms.Button btnBackColor;
		private System.Windows.Forms.RadioButton radioBlueLineStyle;
		private System.Windows.Forms.RadioButton radioHollowLineStyle;
		private System.Windows.Forms.RadioButton radioRedLineStyle;
		private System.Windows.Forms.Button btnLineColor;
		private System.Windows.Forms.NumericUpDown numericUpDownPixelWidth;
		private System.Windows.Forms.CheckBox checkBoxLineInterleaved;
		private System.Windows.Forms.RadioButton radioPixelWidth;
		private System.Windows.Forms.NumericUpDown numericUpDownPointWidth;
		private System.Windows.Forms.RadioButton radioPointWidth;
		private System.Windows.Forms.Label labelLinePattern;
		private System.Windows.Forms.NumericUpDown numericUpDownLinePattern;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnFillForeColor;
		private System.Windows.Forms.NumericUpDown numericUpDownFillPattern;
		private System.Windows.Forms.Button btnFillBackColor;
		private System.Windows.Forms.CheckBox checkBoxFillTransparent;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnVectorColor;
		private System.Windows.Forms.NumericUpDown numericUpDownVectorCode;
		private System.Windows.Forms.NumericUpDown numericUpDownVectorPointSize;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox checkBoxBitmapShowBackground;
		private System.Windows.Forms.Button buttonBitmapColor;
		private System.Windows.Forms.NumericUpDown numericUpDownBitmapPointSize;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox checkBoxBitmapApplyColor;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ListBox listBoxBitmapNames;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown numericUpDownFontCode;
		private System.Windows.Forms.NumericUpDown numericUpDownFontPointSize;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown numericUpDownFontAngle;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.ComboBox comboBoxFontFamilies;
		private System.Windows.Forms.CheckBox checkBoxFontBold;
		private System.Windows.Forms.CheckBox checkBoxFontItalic;
		private System.Windows.Forms.CheckBox checkBoxFontUnderline;
		private System.Windows.Forms.CheckBox checkBoxFontStrikeout;
		private System.Windows.Forms.CheckBox checkBoxFontAllCaps;
		private System.Windows.Forms.CheckBox checkBoxFontShadow;
		private System.Windows.Forms.CheckBox checkBoxFontDoubleSpace;
		private System.Windows.Forms.Button buttonFontForeColor;
		private System.Windows.Forms.Button buttonFontBackColor;
		private System.Windows.Forms.ComboBox comboBoxTextFontFamily;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.NumericUpDown numericUpDownTextSize;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.CheckBox checkBoxTextBold;
		private System.Windows.Forms.CheckBox checkBoxTextItalic;
		private System.Windows.Forms.CheckBox checkBoxTextUnderline;
		private System.Windows.Forms.CheckBox checkBoxTextStrikeout;
		private System.Windows.Forms.CheckBox checkBoxTextAllCaps;
		private System.Windows.Forms.Button buttonTextForeColor;
		private System.Windows.Forms.Button buttonTextBackColor;
		private System.Windows.Forms.TextBox textBoxTextText;
		private System.Windows.Forms.CheckBox checkBoxTextShadow;
		private System.Windows.Forms.CheckBox checkBoxTextDoubleSpace;
		private System.Windows.Forms.TabPage tabStyleDialogs;
		private System.Windows.Forms.Button buttonLineStyleDialog;
		private System.Windows.Forms.Button buttonAreaStyleDialog;
		private System.Windows.Forms.Button buttonTextStyleDialog;
		private System.Windows.Forms.RadioButton radioButtonFontNoBackground;
		private System.Windows.Forms.RadioButton radioButtonFontHalo;
		private System.Windows.Forms.RadioButton radioButtonFontOpaque;
		private System.Windows.Forms.RadioButton radioButtonTextNoBackground;
		private System.Windows.Forms.RadioButton radioButtonTextHalo;
		private System.Windows.Forms.RadioButton radioButtonTextOpaque;
		private System.Windows.Forms.Button buttonSymbolStyleDialog;
		private MapInfo.Windows.Controls.MapControl mapControl1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown numericUpDownTextAngle;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.ComboBox comboBoxTextAlignment;
		private System.Windows.Forms.ComboBox comboBoxTextSpacing;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.GroupBox groupBoxTextProperties;
		private System.Windows.Forms.GroupBox groupBoxTextFontProperties;
		private System.ComponentModel.Container components = null;
		#endregion

		public MapForm1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Assign the Pan tool to the middle mouse button
			mapControl1.Tools.MiddleButtonTool = null;
	}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if (this._lineStyleDlg != null) 
			{
				this._lineStyleDlg.Dispose();
				this._lineStyleDlg = null; 
			}
			if (this._areaStyleDlg != null) 
			{
				this._areaStyleDlg.Dispose();
				this._areaStyleDlg = null; 
			}
			if (this._textStyleDlg != null) 
			{
				this._textStyleDlg.Dispose();
				this._textStyleDlg = null; 
			}
			if (this._symbolStyleDlg != null) 
			{
				this._symbolStyleDlg.Dispose();
				this._symbolStyleDlg = null; 
			}
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
            Session.Dispose();
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
			this.btnBackColor = new System.Windows.Forms.Button();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabStock = new System.Windows.Forms.TabPage();
			this.radioBlackFillStyle = new System.Windows.Forms.RadioButton();
			this.radioBlackLineStyle = new System.Windows.Forms.RadioButton();
			this.radioBlueFillStyle = new System.Windows.Forms.RadioButton();
			this.radioHollowFillStyle = new System.Windows.Forms.RadioButton();
			this.radioRedFillStyle = new System.Windows.Forms.RadioButton();
			this.radioWhiteFillStyle = new System.Windows.Forms.RadioButton();
			this.radioBlueLineStyle = new System.Windows.Forms.RadioButton();
			this.radioHollowLineStyle = new System.Windows.Forms.RadioButton();
			this.radioRedLineStyle = new System.Windows.Forms.RadioButton();
			this.tabLines = new System.Windows.Forms.TabPage();
			this.labelLinePattern = new System.Windows.Forms.Label();
			this.radioPixelWidth = new System.Windows.Forms.RadioButton();
			this.checkBoxLineInterleaved = new System.Windows.Forms.CheckBox();
			this.numericUpDownPixelWidth = new System.Windows.Forms.NumericUpDown();
			this.btnLineColor = new System.Windows.Forms.Button();
			this.numericUpDownPointWidth = new System.Windows.Forms.NumericUpDown();
			this.radioPointWidth = new System.Windows.Forms.RadioButton();
			this.numericUpDownLinePattern = new System.Windows.Forms.NumericUpDown();
			this.tabFontSymbols = new System.Windows.Forms.TabPage();
			this.radioButtonFontOpaque = new System.Windows.Forms.RadioButton();
			this.radioButtonFontHalo = new System.Windows.Forms.RadioButton();
			this.radioButtonFontNoBackground = new System.Windows.Forms.RadioButton();
			this.comboBoxFontFamilies = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.numericUpDownFontCode = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownFontPointSize = new System.Windows.Forms.NumericUpDown();
			this.label8 = new System.Windows.Forms.Label();
			this.numericUpDownFontAngle = new System.Windows.Forms.NumericUpDown();
			this.label9 = new System.Windows.Forms.Label();
			this.checkBoxFontBold = new System.Windows.Forms.CheckBox();
			this.checkBoxFontItalic = new System.Windows.Forms.CheckBox();
			this.checkBoxFontUnderline = new System.Windows.Forms.CheckBox();
			this.checkBoxFontStrikeout = new System.Windows.Forms.CheckBox();
			this.checkBoxFontAllCaps = new System.Windows.Forms.CheckBox();
			this.checkBoxFontShadow = new System.Windows.Forms.CheckBox();
			this.checkBoxFontDoubleSpace = new System.Windows.Forms.CheckBox();
			this.buttonFontForeColor = new System.Windows.Forms.Button();
			this.buttonFontBackColor = new System.Windows.Forms.Button();
			this.tabFills = new System.Windows.Forms.TabPage();
			this.checkBoxFillTransparent = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnFillForeColor = new System.Windows.Forms.Button();
			this.numericUpDownFillPattern = new System.Windows.Forms.NumericUpDown();
			this.btnFillBackColor = new System.Windows.Forms.Button();
			this.tabVectorSymbols = new System.Windows.Forms.TabPage();
			this.label2 = new System.Windows.Forms.Label();
			this.btnVectorColor = new System.Windows.Forms.Button();
			this.numericUpDownVectorCode = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownVectorPointSize = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.tabBitmapSymbols = new System.Windows.Forms.TabPage();
			this.listBoxBitmapNames = new System.Windows.Forms.ListBox();
			this.label6 = new System.Windows.Forms.Label();
			this.numericUpDownBitmapPointSize = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.checkBoxBitmapShowBackground = new System.Windows.Forms.CheckBox();
			this.buttonBitmapColor = new System.Windows.Forms.Button();
			this.checkBoxBitmapApplyColor = new System.Windows.Forms.CheckBox();
			this.tabText = new System.Windows.Forms.TabPage();
			this.label15 = new System.Windows.Forms.Label();
			this.comboBoxTextSpacing = new System.Windows.Forms.ComboBox();
			this.label14 = new System.Windows.Forms.Label();
			this.comboBoxTextAlignment = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.numericUpDownTextAngle = new System.Windows.Forms.NumericUpDown();
			this.radioButtonTextOpaque = new System.Windows.Forms.RadioButton();
			this.radioButtonTextHalo = new System.Windows.Forms.RadioButton();
			this.radioButtonTextNoBackground = new System.Windows.Forms.RadioButton();
			this.textBoxTextText = new System.Windows.Forms.TextBox();
			this.comboBoxTextFontFamily = new System.Windows.Forms.ComboBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.numericUpDownTextSize = new System.Windows.Forms.NumericUpDown();
			this.label13 = new System.Windows.Forms.Label();
			this.checkBoxTextBold = new System.Windows.Forms.CheckBox();
			this.checkBoxTextItalic = new System.Windows.Forms.CheckBox();
			this.checkBoxTextUnderline = new System.Windows.Forms.CheckBox();
			this.checkBoxTextStrikeout = new System.Windows.Forms.CheckBox();
			this.checkBoxTextAllCaps = new System.Windows.Forms.CheckBox();
			this.checkBoxTextShadow = new System.Windows.Forms.CheckBox();
			this.checkBoxTextDoubleSpace = new System.Windows.Forms.CheckBox();
			this.buttonTextForeColor = new System.Windows.Forms.Button();
			this.buttonTextBackColor = new System.Windows.Forms.Button();
			this.groupBoxTextProperties = new System.Windows.Forms.GroupBox();
			this.groupBoxTextFontProperties = new System.Windows.Forms.GroupBox();
			this.tabStyleDialogs = new System.Windows.Forms.TabPage();
			this.buttonSymbolStyleDialog = new System.Windows.Forms.Button();
			this.buttonTextStyleDialog = new System.Windows.Forms.Button();
			this.buttonAreaStyleDialog = new System.Windows.Forms.Button();
			this.buttonLineStyleDialog = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabStock.SuspendLayout();
			this.tabLines.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownPixelWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownPointWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownLinePattern)).BeginInit();
			this.tabFontSymbols.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownFontCode)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownFontPointSize)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownFontAngle)).BeginInit();
			this.tabFills.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownFillPattern)).BeginInit();
			this.tabVectorSymbols.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownVectorCode)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownVectorPointSize)).BeginInit();
			this.tabBitmapSymbols.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownBitmapPointSize)).BeginInit();
			this.tabText.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownTextAngle)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownTextSize)).BeginInit();
			this.tabStyleDialogs.SuspendLayout();
			this.SuspendLayout();
			// 
			// mapControl1
			// 
			this.mapControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mapControl1.Enabled = false;
			this.mapControl1.Location = new System.Drawing.Point(128, 200);
			this.mapControl1.Name = "mapControl1";
			this.mapControl1.Size = new System.Drawing.Size(224, 136);
			this.mapControl1.TabIndex = 0;
			this.mapControl1.Text = "mapControl1";
			this.mapControl1.Tools.LeftButtonTool = null;
			this.mapControl1.Tools.MiddleButtonTool = null;
			this.mapControl1.Tools.RightButtonTool = null;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Controls.Add(this.mapControl1);
			this.panel1.Controls.Add(this.btnBackColor);
			this.panel1.Location = new System.Drawing.Point(4, 38);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(488, 357);
			this.panel1.TabIndex = 1;
			// 
			// btnBackColor
			// 
			this.btnBackColor.Location = new System.Drawing.Point(8, 200);
			this.btnBackColor.Name = "btnBackColor";
			this.btnBackColor.Size = new System.Drawing.Size(80, 24);
			this.btnBackColor.TabIndex = 1;
			this.btnBackColor.Text = "Back Color";
			this.btnBackColor.Click += new System.EventHandler(this.btnBackColor_Click);
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 395);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(496, 19);
			this.statusBar1.TabIndex = 2;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabStock);
			this.tabControl1.Controls.Add(this.tabLines);
			this.tabControl1.Controls.Add(this.tabFontSymbols);
			this.tabControl1.Controls.Add(this.tabFills);
			this.tabControl1.Controls.Add(this.tabVectorSymbols);
			this.tabControl1.Controls.Add(this.tabBitmapSymbols);
			this.tabControl1.Controls.Add(this.tabText);
			this.tabControl1.Controls.Add(this.tabStyleDialogs);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(496, 232);
			this.tabControl1.TabIndex = 3;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabStock
			// 
			this.tabStock.Controls.Add(this.radioBlackFillStyle);
			this.tabStock.Controls.Add(this.radioBlackLineStyle);
			this.tabStock.Controls.Add(this.radioBlueFillStyle);
			this.tabStock.Controls.Add(this.radioHollowFillStyle);
			this.tabStock.Controls.Add(this.radioRedFillStyle);
			this.tabStock.Controls.Add(this.radioWhiteFillStyle);
			this.tabStock.Controls.Add(this.radioBlueLineStyle);
			this.tabStock.Controls.Add(this.radioHollowLineStyle);
			this.tabStock.Controls.Add(this.radioRedLineStyle);
			this.tabStock.Location = new System.Drawing.Point(4, 22);
			this.tabStock.Name = "tabStock";
			this.tabStock.Size = new System.Drawing.Size(488, 206);
			this.tabStock.TabIndex = 1;
			this.tabStock.Text = "Stock";
			// 
			// radioBlackFillStyle
			// 
			this.radioBlackFillStyle.Location = new System.Drawing.Point(8, 8);
			this.radioBlackFillStyle.Name = "radioBlackFillStyle";
			this.radioBlackFillStyle.TabIndex = 0;
			this.radioBlackFillStyle.Text = "Black Fill Style";
			this.radioBlackFillStyle.CheckedChanged += new System.EventHandler(this.radioBlackFillStyle_CheckedChanged);
			// 
			// radioBlackLineStyle
			// 
			this.radioBlackLineStyle.Location = new System.Drawing.Point(128, 8);
			this.radioBlackLineStyle.Name = "radioBlackLineStyle";
			this.radioBlackLineStyle.TabIndex = 0;
			this.radioBlackLineStyle.Text = "Black Line Style";
			this.radioBlackLineStyle.CheckedChanged += new System.EventHandler(this.radioBlackLineStyle_CheckedChanged);
			// 
			// radioBlueFillStyle
			// 
			this.radioBlueFillStyle.Location = new System.Drawing.Point(8, 36);
			this.radioBlueFillStyle.Name = "radioBlueFillStyle";
			this.radioBlueFillStyle.TabIndex = 0;
			this.radioBlueFillStyle.Text = "Blue Fill Style";
			this.radioBlueFillStyle.CheckedChanged += new System.EventHandler(this.radioBlueFillStyle_CheckedChanged);
			// 
			// radioHollowFillStyle
			// 
			this.radioHollowFillStyle.Location = new System.Drawing.Point(8, 64);
			this.radioHollowFillStyle.Name = "radioHollowFillStyle";
			this.radioHollowFillStyle.TabIndex = 0;
			this.radioHollowFillStyle.Text = "Hollow Fill Style";
			this.radioHollowFillStyle.CheckedChanged += new System.EventHandler(this.radioHollowFillStyle_CheckedChanged);
			// 
			// radioRedFillStyle
			// 
			this.radioRedFillStyle.Location = new System.Drawing.Point(8, 92);
			this.radioRedFillStyle.Name = "radioRedFillStyle";
			this.radioRedFillStyle.TabIndex = 0;
			this.radioRedFillStyle.Text = "Red Fill Style";
			this.radioRedFillStyle.CheckedChanged += new System.EventHandler(this.radioRedFillStyle_CheckedChanged);
			// 
			// radioWhiteFillStyle
			// 
			this.radioWhiteFillStyle.Location = new System.Drawing.Point(8, 120);
			this.radioWhiteFillStyle.Name = "radioWhiteFillStyle";
			this.radioWhiteFillStyle.TabIndex = 0;
			this.radioWhiteFillStyle.Text = "White Fill Style";
			this.radioWhiteFillStyle.CheckedChanged += new System.EventHandler(this.radioWhiteFillStyle_CheckedChanged);
			// 
			// radioBlueLineStyle
			// 
			this.radioBlueLineStyle.Location = new System.Drawing.Point(128, 36);
			this.radioBlueLineStyle.Name = "radioBlueLineStyle";
			this.radioBlueLineStyle.TabIndex = 0;
			this.radioBlueLineStyle.Text = "Blue Line Style";
			this.radioBlueLineStyle.CheckedChanged += new System.EventHandler(this.radioBlueLineStyle_CheckedChanged);
			// 
			// radioHollowLineStyle
			// 
			this.radioHollowLineStyle.Location = new System.Drawing.Point(128, 64);
			this.radioHollowLineStyle.Name = "radioHollowLineStyle";
			this.radioHollowLineStyle.Size = new System.Drawing.Size(112, 24);
			this.radioHollowLineStyle.TabIndex = 0;
			this.radioHollowLineStyle.Text = "Hollow Line Style";
			this.radioHollowLineStyle.CheckedChanged += new System.EventHandler(this.radioHollowLineStyle_CheckedChanged);
			// 
			// radioRedLineStyle
			// 
			this.radioRedLineStyle.Location = new System.Drawing.Point(128, 92);
			this.radioRedLineStyle.Name = "radioRedLineStyle";
			this.radioRedLineStyle.TabIndex = 0;
			this.radioRedLineStyle.Text = "Red Line Style";
			this.radioRedLineStyle.CheckedChanged += new System.EventHandler(this.radioRedLineStyle_CheckedChanged);
			// 
			// tabLines
			// 
			this.tabLines.Controls.Add(this.labelLinePattern);
			this.tabLines.Controls.Add(this.radioPixelWidth);
			this.tabLines.Controls.Add(this.checkBoxLineInterleaved);
			this.tabLines.Controls.Add(this.numericUpDownPixelWidth);
			this.tabLines.Controls.Add(this.btnLineColor);
			this.tabLines.Controls.Add(this.numericUpDownPointWidth);
			this.tabLines.Controls.Add(this.radioPointWidth);
			this.tabLines.Controls.Add(this.numericUpDownLinePattern);
			this.tabLines.Location = new System.Drawing.Point(4, 22);
			this.tabLines.Name = "tabLines";
			this.tabLines.Size = new System.Drawing.Size(488, 206);
			this.tabLines.TabIndex = 0;
			this.tabLines.Text = "Lines";
			// 
			// labelLinePattern
			// 
			this.labelLinePattern.Location = new System.Drawing.Point(16, 95);
			this.labelLinePattern.Name = "labelLinePattern";
			this.labelLinePattern.Size = new System.Drawing.Size(56, 23);
			this.labelLinePattern.TabIndex = 6;
			this.labelLinePattern.Text = "Pattern:";
			// 
			// radioPixelWidth
			// 
			this.radioPixelWidth.Location = new System.Drawing.Point(16, 38);
			this.radioPixelWidth.Name = "radioPixelWidth";
			this.radioPixelWidth.Size = new System.Drawing.Size(88, 24);
			this.radioPixelWidth.TabIndex = 5;
			this.radioPixelWidth.Text = "Pixel Width:";
			this.radioPixelWidth.CheckedChanged += new System.EventHandler(this.radioPixelWidth_CheckedChanged);
			// 
			// checkBoxLineInterleaved
			// 
			this.checkBoxLineInterleaved.Location = new System.Drawing.Point(16, 120);
			this.checkBoxLineInterleaved.Name = "checkBoxLineInterleaved";
			this.checkBoxLineInterleaved.TabIndex = 4;
			this.checkBoxLineInterleaved.Text = "Interleaved";
			this.checkBoxLineInterleaved.CheckedChanged += new System.EventHandler(this.checkBoxLineInterleaved_CheckedChanged);
			// 
			// numericUpDownPixelWidth
			// 
			this.numericUpDownPixelWidth.Location = new System.Drawing.Point(120, 40);
			this.numericUpDownPixelWidth.Name = "numericUpDownPixelWidth";
			this.numericUpDownPixelWidth.Size = new System.Drawing.Size(80, 20);
			this.numericUpDownPixelWidth.TabIndex = 2;
			this.numericUpDownPixelWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownPixelWidth.Value = new System.Decimal(new int[] {
																																					1,
																																					0,
																																					0,
																																					0});
			this.numericUpDownPixelWidth.ValueChanged += new System.EventHandler(this.numericUpDownPixelWidth_ValueChanged);
			// 
			// btnLineColor
			// 
			this.btnLineColor.Location = new System.Drawing.Point(16, 8);
			this.btnLineColor.Name = "btnLineColor";
			this.btnLineColor.Size = new System.Drawing.Size(112, 24);
			this.btnLineColor.TabIndex = 1;
			this.btnLineColor.Text = "Color";
			this.btnLineColor.Click += new System.EventHandler(this.btnLineColor_Click);
			// 
			// numericUpDownPointWidth
			// 
			this.numericUpDownPointWidth.Location = new System.Drawing.Point(120, 64);
			this.numericUpDownPointWidth.Name = "numericUpDownPointWidth";
			this.numericUpDownPointWidth.Size = new System.Drawing.Size(80, 20);
			this.numericUpDownPointWidth.TabIndex = 2;
			this.numericUpDownPointWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownPointWidth.Value = new System.Decimal(new int[] {
																																					1,
																																					0,
																																					0,
																																					0});
			this.numericUpDownPointWidth.ValueChanged += new System.EventHandler(this.numericUpDownPointWidth_ValueChanged);
			// 
			// radioPointWidth
			// 
			this.radioPointWidth.Location = new System.Drawing.Point(16, 62);
			this.radioPointWidth.Name = "radioPointWidth";
			this.radioPointWidth.Size = new System.Drawing.Size(88, 24);
			this.radioPointWidth.TabIndex = 5;
			this.radioPointWidth.Text = "Point Width:";
			this.radioPointWidth.CheckedChanged += new System.EventHandler(this.radioPointWidth_CheckedChanged);
			// 
			// numericUpDownLinePattern
			// 
			this.numericUpDownLinePattern.Location = new System.Drawing.Point(120, 96);
			this.numericUpDownLinePattern.Name = "numericUpDownLinePattern";
			this.numericUpDownLinePattern.Size = new System.Drawing.Size(80, 20);
			this.numericUpDownLinePattern.TabIndex = 2;
			this.numericUpDownLinePattern.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownLinePattern.Value = new System.Decimal(new int[] {
																																					 2,
																																					 0,
																																					 0,
																																					 0});
			this.numericUpDownLinePattern.ValueChanged += new System.EventHandler(this.numericUpDownLinePattern_ValueChanged);
			// 
			// tabFontSymbols
			// 
			this.tabFontSymbols.Controls.Add(this.radioButtonFontOpaque);
			this.tabFontSymbols.Controls.Add(this.radioButtonFontHalo);
			this.tabFontSymbols.Controls.Add(this.radioButtonFontNoBackground);
			this.tabFontSymbols.Controls.Add(this.comboBoxFontFamilies);
			this.tabFontSymbols.Controls.Add(this.label10);
			this.tabFontSymbols.Controls.Add(this.label7);
			this.tabFontSymbols.Controls.Add(this.numericUpDownFontCode);
			this.tabFontSymbols.Controls.Add(this.numericUpDownFontPointSize);
			this.tabFontSymbols.Controls.Add(this.label8);
			this.tabFontSymbols.Controls.Add(this.numericUpDownFontAngle);
			this.tabFontSymbols.Controls.Add(this.label9);
			this.tabFontSymbols.Controls.Add(this.checkBoxFontBold);
			this.tabFontSymbols.Controls.Add(this.checkBoxFontItalic);
			this.tabFontSymbols.Controls.Add(this.checkBoxFontUnderline);
			this.tabFontSymbols.Controls.Add(this.checkBoxFontStrikeout);
			this.tabFontSymbols.Controls.Add(this.checkBoxFontAllCaps);
			this.tabFontSymbols.Controls.Add(this.checkBoxFontShadow);
			this.tabFontSymbols.Controls.Add(this.checkBoxFontDoubleSpace);
			this.tabFontSymbols.Controls.Add(this.buttonFontForeColor);
			this.tabFontSymbols.Controls.Add(this.buttonFontBackColor);
			this.tabFontSymbols.Location = new System.Drawing.Point(4, 22);
			this.tabFontSymbols.Name = "tabFontSymbols";
			this.tabFontSymbols.Size = new System.Drawing.Size(488, 206);
			this.tabFontSymbols.TabIndex = 3;
			this.tabFontSymbols.Text = "Font Symbols";
			// 
			// radioButtonFontOpaque
			// 
			this.radioButtonFontOpaque.Location = new System.Drawing.Point(376, 80);
			this.radioButtonFontOpaque.Name = "radioButtonFontOpaque";
			this.radioButtonFontOpaque.TabIndex = 26;
			this.radioButtonFontOpaque.Text = "Box";
			this.radioButtonFontOpaque.CheckedChanged += new System.EventHandler(this.radioButtonFontOpaque_CheckedChanged);
			// 
			// radioButtonFontHalo
			// 
			this.radioButtonFontHalo.Location = new System.Drawing.Point(376, 56);
			this.radioButtonFontHalo.Name = "radioButtonFontHalo";
			this.radioButtonFontHalo.TabIndex = 25;
			this.radioButtonFontHalo.Text = "Halo";
			this.radioButtonFontHalo.CheckedChanged += new System.EventHandler(this.radioButtonFontHalo_CheckedChanged);
			// 
			// radioButtonFontNoBackground
			// 
			this.radioButtonFontNoBackground.Checked = true;
			this.radioButtonFontNoBackground.Location = new System.Drawing.Point(376, 32);
			this.radioButtonFontNoBackground.Name = "radioButtonFontNoBackground";
			this.radioButtonFontNoBackground.TabIndex = 24;
			this.radioButtonFontNoBackground.TabStop = true;
			this.radioButtonFontNoBackground.Text = "No Background";
			this.radioButtonFontNoBackground.CheckedChanged += new System.EventHandler(this.radioButtonFontNoBackground_CheckedChanged);
			// 
			// comboBoxFontFamilies
			// 
			this.comboBoxFontFamilies.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxFontFamilies.Location = new System.Drawing.Point(264, 8);
			this.comboBoxFontFamilies.Name = "comboBoxFontFamilies";
			this.comboBoxFontFamilies.Size = new System.Drawing.Size(144, 21);
			this.comboBoxFontFamilies.TabIndex = 22;
			this.comboBoxFontFamilies.SelectedIndexChanged += new System.EventHandler(this.comboBoxFontFamilies_SelectedIndexChanged);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(192, 8);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(64, 16);
			this.label10.TabIndex = 21;
			this.label10.Text = "Font Name:";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(16, 40);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(56, 23);
			this.label7.TabIndex = 18;
			this.label7.Text = "Code:";
			// 
			// numericUpDownFontCode
			// 
			this.numericUpDownFontCode.Location = new System.Drawing.Point(88, 40);
			this.numericUpDownFontCode.Name = "numericUpDownFontCode";
			this.numericUpDownFontCode.Size = new System.Drawing.Size(80, 20);
			this.numericUpDownFontCode.TabIndex = 16;
			this.numericUpDownFontCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownFontCode.Value = new System.Decimal(new int[] {
																																				34,
																																				0,
																																				0,
																																				0});
			this.numericUpDownFontCode.ValueChanged += new System.EventHandler(this.numericUpDownFontCode_ValueChanged);
			// 
			// numericUpDownFontPointSize
			// 
			this.numericUpDownFontPointSize.Location = new System.Drawing.Point(88, 64);
			this.numericUpDownFontPointSize.Name = "numericUpDownFontPointSize";
			this.numericUpDownFontPointSize.Size = new System.Drawing.Size(80, 20);
			this.numericUpDownFontPointSize.TabIndex = 14;
			this.numericUpDownFontPointSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownFontPointSize.Value = new System.Decimal(new int[] {
																																						 10,
																																						 0,
																																						 0,
																																						 0});
			this.numericUpDownFontPointSize.ValueChanged += new System.EventHandler(this.numericUpDownFontPointSize_ValueChanged);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(16, 64);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(64, 23);
			this.label8.TabIndex = 19;
			this.label8.Text = "Point Size:";
			// 
			// numericUpDownFontAngle
			// 
			this.numericUpDownFontAngle.DecimalPlaces = 1;
			this.numericUpDownFontAngle.Increment = new System.Decimal(new int[] {
																																						 1,
																																						 0,
																																						 0,
																																						 65536});
			this.numericUpDownFontAngle.Location = new System.Drawing.Point(88, 88);
			this.numericUpDownFontAngle.Maximum = new System.Decimal(new int[] {
																																					 360,
																																					 0,
																																					 0,
																																					 0});
			this.numericUpDownFontAngle.Name = "numericUpDownFontAngle";
			this.numericUpDownFontAngle.Size = new System.Drawing.Size(80, 20);
			this.numericUpDownFontAngle.TabIndex = 15;
			this.numericUpDownFontAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownFontAngle.ValueChanged += new System.EventHandler(this.numericUpDownFontAngle_ValueChanged);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(16, 88);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(56, 23);
			this.label9.TabIndex = 17;
			this.label9.Text = "Angle:";
			// 
			// checkBoxFontBold
			// 
			this.checkBoxFontBold.Location = new System.Drawing.Point(200, 32);
			this.checkBoxFontBold.Name = "checkBoxFontBold";
			this.checkBoxFontBold.Size = new System.Drawing.Size(72, 24);
			this.checkBoxFontBold.TabIndex = 23;
			this.checkBoxFontBold.Text = "Bold";
			this.checkBoxFontBold.CheckedChanged += new System.EventHandler(this.checkBoxFontBold_CheckedChanged);
			// 
			// checkBoxFontItalic
			// 
			this.checkBoxFontItalic.Location = new System.Drawing.Point(200, 56);
			this.checkBoxFontItalic.Name = "checkBoxFontItalic";
			this.checkBoxFontItalic.Size = new System.Drawing.Size(72, 24);
			this.checkBoxFontItalic.TabIndex = 23;
			this.checkBoxFontItalic.Text = "Italic";
			this.checkBoxFontItalic.CheckedChanged += new System.EventHandler(this.checkBoxFontItalic_CheckedChanged);
			// 
			// checkBoxFontUnderline
			// 
			this.checkBoxFontUnderline.Location = new System.Drawing.Point(200, 80);
			this.checkBoxFontUnderline.Name = "checkBoxFontUnderline";
			this.checkBoxFontUnderline.Size = new System.Drawing.Size(72, 24);
			this.checkBoxFontUnderline.TabIndex = 23;
			this.checkBoxFontUnderline.Text = "Underline";
			this.checkBoxFontUnderline.CheckedChanged += new System.EventHandler(this.checkBoxFontUnderline_CheckedChanged);
			// 
			// checkBoxFontStrikeout
			// 
			this.checkBoxFontStrikeout.Location = new System.Drawing.Point(200, 104);
			this.checkBoxFontStrikeout.Name = "checkBoxFontStrikeout";
			this.checkBoxFontStrikeout.Size = new System.Drawing.Size(72, 24);
			this.checkBoxFontStrikeout.TabIndex = 23;
			this.checkBoxFontStrikeout.Text = "Strikeout";
			this.checkBoxFontStrikeout.CheckedChanged += new System.EventHandler(this.checkBoxFontStrikeout_CheckedChanged);
			// 
			// checkBoxFontAllCaps
			// 
			this.checkBoxFontAllCaps.Location = new System.Drawing.Point(280, 32);
			this.checkBoxFontAllCaps.Name = "checkBoxFontAllCaps";
			this.checkBoxFontAllCaps.Size = new System.Drawing.Size(72, 24);
			this.checkBoxFontAllCaps.TabIndex = 23;
			this.checkBoxFontAllCaps.Text = "All Caps";
			this.checkBoxFontAllCaps.CheckedChanged += new System.EventHandler(this.checkBoxFontAllCaps_CheckedChanged);
			// 
			// checkBoxFontShadow
			// 
			this.checkBoxFontShadow.Location = new System.Drawing.Point(280, 80);
			this.checkBoxFontShadow.Name = "checkBoxFontShadow";
			this.checkBoxFontShadow.Size = new System.Drawing.Size(72, 24);
			this.checkBoxFontShadow.TabIndex = 23;
			this.checkBoxFontShadow.Text = "Shadow";
			this.checkBoxFontShadow.CheckedChanged += new System.EventHandler(this.checkBoxFontShadow_CheckedChanged);
			// 
			// checkBoxFontDoubleSpace
			// 
			this.checkBoxFontDoubleSpace.Location = new System.Drawing.Point(280, 56);
			this.checkBoxFontDoubleSpace.Name = "checkBoxFontDoubleSpace";
			this.checkBoxFontDoubleSpace.Size = new System.Drawing.Size(96, 24);
			this.checkBoxFontDoubleSpace.TabIndex = 23;
			this.checkBoxFontDoubleSpace.Text = "Double Space";
			this.checkBoxFontDoubleSpace.CheckedChanged += new System.EventHandler(this.checkBoxFontDoubleSpace_CheckedChanged);
			// 
			// buttonFontForeColor
			// 
			this.buttonFontForeColor.Location = new System.Drawing.Point(8, 120);
			this.buttonFontForeColor.Name = "buttonFontForeColor";
			this.buttonFontForeColor.Size = new System.Drawing.Size(64, 24);
			this.buttonFontForeColor.TabIndex = 13;
			this.buttonFontForeColor.Text = "Fore";
			this.buttonFontForeColor.Click += new System.EventHandler(this.buttonFontForeColor_Click);
			// 
			// buttonFontBackColor
			// 
			this.buttonFontBackColor.Location = new System.Drawing.Point(88, 120);
			this.buttonFontBackColor.Name = "buttonFontBackColor";
			this.buttonFontBackColor.Size = new System.Drawing.Size(64, 24);
			this.buttonFontBackColor.TabIndex = 13;
			this.buttonFontBackColor.Text = "Back";
			this.buttonFontBackColor.Click += new System.EventHandler(this.buttonFontBackColor_Click);
			// 
			// tabFills
			// 
			this.tabFills.Controls.Add(this.checkBoxFillTransparent);
			this.tabFills.Controls.Add(this.label1);
			this.tabFills.Controls.Add(this.btnFillForeColor);
			this.tabFills.Controls.Add(this.numericUpDownFillPattern);
			this.tabFills.Controls.Add(this.btnFillBackColor);
			this.tabFills.Location = new System.Drawing.Point(4, 22);
			this.tabFills.Name = "tabFills";
			this.tabFills.Size = new System.Drawing.Size(488, 206);
			this.tabFills.TabIndex = 2;
			this.tabFills.Text = "Fills";
			// 
			// checkBoxFillTransparent
			// 
			this.checkBoxFillTransparent.Location = new System.Drawing.Point(16, 112);
			this.checkBoxFillTransparent.Name = "checkBoxFillTransparent";
			this.checkBoxFillTransparent.TabIndex = 10;
			this.checkBoxFillTransparent.Text = "Transparent";
			this.checkBoxFillTransparent.CheckedChanged += new System.EventHandler(this.checkBoxFillTransparent_CheckedChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 80);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 23);
			this.label1.TabIndex = 9;
			this.label1.Text = "Pattern:";
			// 
			// btnFillForeColor
			// 
			this.btnFillForeColor.Location = new System.Drawing.Point(16, 8);
			this.btnFillForeColor.Name = "btnFillForeColor";
			this.btnFillForeColor.Size = new System.Drawing.Size(112, 24);
			this.btnFillForeColor.TabIndex = 7;
			this.btnFillForeColor.Text = "Fore Color";
			this.btnFillForeColor.Click += new System.EventHandler(this.btnFillForeColor_Click);
			// 
			// numericUpDownFillPattern
			// 
			this.numericUpDownFillPattern.Location = new System.Drawing.Point(88, 80);
			this.numericUpDownFillPattern.Name = "numericUpDownFillPattern";
			this.numericUpDownFillPattern.Size = new System.Drawing.Size(80, 20);
			this.numericUpDownFillPattern.TabIndex = 8;
			this.numericUpDownFillPattern.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownFillPattern.Value = new System.Decimal(new int[] {
																																					 2,
																																					 0,
																																					 0,
																																					 0});
			this.numericUpDownFillPattern.ValueChanged += new System.EventHandler(this.numericUpDownFillPattern_ValueChanged);
			// 
			// btnFillBackColor
			// 
			this.btnFillBackColor.Location = new System.Drawing.Point(16, 40);
			this.btnFillBackColor.Name = "btnFillBackColor";
			this.btnFillBackColor.Size = new System.Drawing.Size(112, 24);
			this.btnFillBackColor.TabIndex = 7;
			this.btnFillBackColor.Text = "Back Color";
			this.btnFillBackColor.Click += new System.EventHandler(this.btnFillBackColor_Click);
			// 
			// tabVectorSymbols
			// 
			this.tabVectorSymbols.Controls.Add(this.label2);
			this.tabVectorSymbols.Controls.Add(this.btnVectorColor);
			this.tabVectorSymbols.Controls.Add(this.numericUpDownVectorCode);
			this.tabVectorSymbols.Controls.Add(this.numericUpDownVectorPointSize);
			this.tabVectorSymbols.Controls.Add(this.label3);
			this.tabVectorSymbols.Location = new System.Drawing.Point(4, 22);
			this.tabVectorSymbols.Name = "tabVectorSymbols";
			this.tabVectorSymbols.Size = new System.Drawing.Size(488, 206);
			this.tabVectorSymbols.TabIndex = 4;
			this.tabVectorSymbols.Text = "Vector Symbols";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 23);
			this.label2.TabIndex = 12;
			this.label2.Text = "Code:";
			// 
			// btnVectorColor
			// 
			this.btnVectorColor.Location = new System.Drawing.Point(8, 8);
			this.btnVectorColor.Name = "btnVectorColor";
			this.btnVectorColor.Size = new System.Drawing.Size(112, 24);
			this.btnVectorColor.TabIndex = 10;
			this.btnVectorColor.Text = "Color";
			this.btnVectorColor.Click += new System.EventHandler(this.btnVectorColor_Click);
			// 
			// numericUpDownVectorCode
			// 
			this.numericUpDownVectorCode.Location = new System.Drawing.Point(80, 40);
			this.numericUpDownVectorCode.Name = "numericUpDownVectorCode";
			this.numericUpDownVectorCode.Size = new System.Drawing.Size(80, 20);
			this.numericUpDownVectorCode.TabIndex = 11;
			this.numericUpDownVectorCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownVectorCode.Value = new System.Decimal(new int[] {
																																					34,
																																					0,
																																					0,
																																					0});
			this.numericUpDownVectorCode.ValueChanged += new System.EventHandler(this.numericUpDownVectorCode_ValueChanged);
			// 
			// numericUpDownVectorPointSize
			// 
			this.numericUpDownVectorPointSize.Location = new System.Drawing.Point(80, 64);
			this.numericUpDownVectorPointSize.Name = "numericUpDownVectorPointSize";
			this.numericUpDownVectorPointSize.Size = new System.Drawing.Size(80, 20);
			this.numericUpDownVectorPointSize.TabIndex = 11;
			this.numericUpDownVectorPointSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownVectorPointSize.Value = new System.Decimal(new int[] {
																																							 10,
																																							 0,
																																							 0,
																																							 0});
			this.numericUpDownVectorPointSize.ValueChanged += new System.EventHandler(this.numericUpDownVectorPointSize_ValueChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 64);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 23);
			this.label3.TabIndex = 12;
			this.label3.Text = "Point Size:";
			// 
			// tabBitmapSymbols
			// 
			this.tabBitmapSymbols.Controls.Add(this.listBoxBitmapNames);
			this.tabBitmapSymbols.Controls.Add(this.label6);
			this.tabBitmapSymbols.Controls.Add(this.numericUpDownBitmapPointSize);
			this.tabBitmapSymbols.Controls.Add(this.label5);
			this.tabBitmapSymbols.Controls.Add(this.checkBoxBitmapShowBackground);
			this.tabBitmapSymbols.Controls.Add(this.buttonBitmapColor);
			this.tabBitmapSymbols.Controls.Add(this.checkBoxBitmapApplyColor);
			this.tabBitmapSymbols.Location = new System.Drawing.Point(4, 22);
			this.tabBitmapSymbols.Name = "tabBitmapSymbols";
			this.tabBitmapSymbols.Size = new System.Drawing.Size(488, 206);
			this.tabBitmapSymbols.TabIndex = 5;
			this.tabBitmapSymbols.Text = "Bitmap Symbols";
			// 
			// listBoxBitmapNames
			// 
			this.listBoxBitmapNames.Location = new System.Drawing.Point(232, 16);
			this.listBoxBitmapNames.Name = "listBoxBitmapNames";
			this.listBoxBitmapNames.Size = new System.Drawing.Size(120, 121);
			this.listBoxBitmapNames.TabIndex = 17;
			this.listBoxBitmapNames.SelectedIndexChanged += new System.EventHandler(this.listBoxBitmapNames_SelectedIndexChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(184, 16);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(48, 16);
			this.label6.TabIndex = 15;
			this.label6.Text = "Name:";
			// 
			// numericUpDownBitmapPointSize
			// 
			this.numericUpDownBitmapPointSize.Location = new System.Drawing.Point(88, 48);
			this.numericUpDownBitmapPointSize.Name = "numericUpDownBitmapPointSize";
			this.numericUpDownBitmapPointSize.Size = new System.Drawing.Size(80, 20);
			this.numericUpDownBitmapPointSize.TabIndex = 13;
			this.numericUpDownBitmapPointSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownBitmapPointSize.Value = new System.Decimal(new int[] {
																																							 10,
																																							 0,
																																							 0,
																																							 0});
			this.numericUpDownBitmapPointSize.ValueChanged += new System.EventHandler(this.numericUpDownBitmapPointSize_ValueChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 48);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 23);
			this.label5.TabIndex = 14;
			this.label5.Text = "Point Size:";
			// 
			// checkBoxBitmapShowBackground
			// 
			this.checkBoxBitmapShowBackground.Location = new System.Drawing.Point(16, 115);
			this.checkBoxBitmapShowBackground.Name = "checkBoxBitmapShowBackground";
			this.checkBoxBitmapShowBackground.Size = new System.Drawing.Size(120, 24);
			this.checkBoxBitmapShowBackground.TabIndex = 12;
			this.checkBoxBitmapShowBackground.Text = "Show Background";
			this.checkBoxBitmapShowBackground.CheckedChanged += new System.EventHandler(this.checkBoxBitmapShowBackground_CheckedChanged);
			// 
			// buttonBitmapColor
			// 
			this.buttonBitmapColor.Location = new System.Drawing.Point(16, 11);
			this.buttonBitmapColor.Name = "buttonBitmapColor";
			this.buttonBitmapColor.Size = new System.Drawing.Size(112, 24);
			this.buttonBitmapColor.TabIndex = 11;
			this.buttonBitmapColor.Text = "Color";
			this.buttonBitmapColor.Click += new System.EventHandler(this.buttonBitmapColor_Click);
			// 
			// checkBoxBitmapApplyColor
			// 
			this.checkBoxBitmapApplyColor.Location = new System.Drawing.Point(16, 80);
			this.checkBoxBitmapApplyColor.Name = "checkBoxBitmapApplyColor";
			this.checkBoxBitmapApplyColor.TabIndex = 12;
			this.checkBoxBitmapApplyColor.Text = "Apply Color";
			this.checkBoxBitmapApplyColor.CheckedChanged += new System.EventHandler(this.checkBoxBitmapApplyColor_CheckedChanged);
			// 
			// tabText
			// 
			this.tabText.Controls.Add(this.label15);
			this.tabText.Controls.Add(this.comboBoxTextSpacing);
			this.tabText.Controls.Add(this.label14);
			this.tabText.Controls.Add(this.comboBoxTextAlignment);
			this.tabText.Controls.Add(this.label4);
			this.tabText.Controls.Add(this.numericUpDownTextAngle);
			this.tabText.Controls.Add(this.radioButtonTextOpaque);
			this.tabText.Controls.Add(this.radioButtonTextHalo);
			this.tabText.Controls.Add(this.radioButtonTextNoBackground);
			this.tabText.Controls.Add(this.textBoxTextText);
			this.tabText.Controls.Add(this.comboBoxTextFontFamily);
			this.tabText.Controls.Add(this.label11);
			this.tabText.Controls.Add(this.label12);
			this.tabText.Controls.Add(this.numericUpDownTextSize);
			this.tabText.Controls.Add(this.label13);
			this.tabText.Controls.Add(this.checkBoxTextBold);
			this.tabText.Controls.Add(this.checkBoxTextItalic);
			this.tabText.Controls.Add(this.checkBoxTextUnderline);
			this.tabText.Controls.Add(this.checkBoxTextStrikeout);
			this.tabText.Controls.Add(this.checkBoxTextAllCaps);
			this.tabText.Controls.Add(this.checkBoxTextShadow);
			this.tabText.Controls.Add(this.checkBoxTextDoubleSpace);
			this.tabText.Controls.Add(this.buttonTextForeColor);
			this.tabText.Controls.Add(this.buttonTextBackColor);
			this.tabText.Controls.Add(this.groupBoxTextProperties);
			this.tabText.Controls.Add(this.groupBoxTextFontProperties);
			this.tabText.Location = new System.Drawing.Point(4, 22);
			this.tabText.Name = "tabText";
			this.tabText.Size = new System.Drawing.Size(488, 206);
			this.tabText.TabIndex = 6;
			this.tabText.Text = "Text";
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(16, 160);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(64, 23);
			this.label15.TabIndex = 50;
			this.label15.Text = "Spacing";
			// 
			// comboBoxTextSpacing
			// 
			this.comboBoxTextSpacing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxTextSpacing.Location = new System.Drawing.Point(88, 159);
			this.comboBoxTextSpacing.Name = "comboBoxTextSpacing";
			this.comboBoxTextSpacing.Size = new System.Drawing.Size(96, 21);
			this.comboBoxTextSpacing.TabIndex = 49;
			this.comboBoxTextSpacing.SelectedIndexChanged += new System.EventHandler(this.comboBoxTextSpacing_SelectedIndexChanged);
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(16, 128);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(64, 23);
			this.label14.TabIndex = 48;
			this.label14.Text = "Alignment";
			// 
			// comboBoxTextAlignment
			// 
			this.comboBoxTextAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxTextAlignment.Location = new System.Drawing.Point(88, 125);
			this.comboBoxTextAlignment.Name = "comboBoxTextAlignment";
			this.comboBoxTextAlignment.Size = new System.Drawing.Size(96, 21);
			this.comboBoxTextAlignment.TabIndex = 47;
			this.comboBoxTextAlignment.SelectedIndexChanged += new System.EventHandler(this.comboBoxTextAlignment_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 96);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(64, 23);
			this.label4.TabIndex = 46;
			this.label4.Text = "Angle";
			// 
			// numericUpDownTextAngle
			// 
			this.numericUpDownTextAngle.Location = new System.Drawing.Point(88, 92);
			this.numericUpDownTextAngle.Name = "numericUpDownTextAngle";
			this.numericUpDownTextAngle.Size = new System.Drawing.Size(96, 20);
			this.numericUpDownTextAngle.TabIndex = 45;
			this.numericUpDownTextAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownTextAngle.ValueChanged += new System.EventHandler(this.numericUpDownTextAngle_ValueChanged);
			// 
			// radioButtonTextOpaque
			// 
			this.radioButtonTextOpaque.Location = new System.Drawing.Point(376, 144);
			this.radioButtonTextOpaque.Name = "radioButtonTextOpaque";
			this.radioButtonTextOpaque.Size = new System.Drawing.Size(88, 24);
			this.radioButtonTextOpaque.TabIndex = 44;
			this.radioButtonTextOpaque.Text = "Box";
			this.radioButtonTextOpaque.CheckedChanged += new System.EventHandler(this.radioButtonTextOpaque_CheckedChanged);
			// 
			// radioButtonTextHalo
			// 
			this.radioButtonTextHalo.Location = new System.Drawing.Point(376, 120);
			this.radioButtonTextHalo.Name = "radioButtonTextHalo";
			this.radioButtonTextHalo.Size = new System.Drawing.Size(88, 24);
			this.radioButtonTextHalo.TabIndex = 43;
			this.radioButtonTextHalo.Text = "Halo";
			this.radioButtonTextHalo.CheckedChanged += new System.EventHandler(this.radioButtonTextHalo_CheckedChanged);
			// 
			// radioButtonTextNoBackground
			// 
			this.radioButtonTextNoBackground.Checked = true;
			this.radioButtonTextNoBackground.Location = new System.Drawing.Point(376, 96);
			this.radioButtonTextNoBackground.Name = "radioButtonTextNoBackground";
			this.radioButtonTextNoBackground.Size = new System.Drawing.Size(88, 24);
			this.radioButtonTextNoBackground.TabIndex = 42;
			this.radioButtonTextNoBackground.TabStop = true;
			this.radioButtonTextNoBackground.Text = "No Background";
			this.radioButtonTextNoBackground.CheckedChanged += new System.EventHandler(this.radioButtonTextNoBackground_CheckedChanged);
			// 
			// textBoxTextText
			// 
			this.textBoxTextText.AcceptsReturn = true;
			this.textBoxTextText.Location = new System.Drawing.Point(56, 40);
			this.textBoxTextText.Multiline = true;
			this.textBoxTextText.Name = "textBoxTextText";
			this.textBoxTextText.Size = new System.Drawing.Size(128, 40);
			this.textBoxTextText.TabIndex = 41;
			this.textBoxTextText.Text = "AaBbCcDdEeFfGgWw";
			this.textBoxTextText.WordWrap = false;
			this.textBoxTextText.TextChanged += new System.EventHandler(this.textBoxTextText_TextChanged);
			// 
			// comboBoxTextFontFamily
			// 
			this.comboBoxTextFontFamily.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxTextFontFamily.Location = new System.Drawing.Point(288, 32);
			this.comboBoxTextFontFamily.Name = "comboBoxTextFontFamily";
			this.comboBoxTextFontFamily.Size = new System.Drawing.Size(144, 21);
			this.comboBoxTextFontFamily.TabIndex = 30;
			this.comboBoxTextFontFamily.SelectedIndexChanged += new System.EventHandler(this.comboBoxTextFontFamily_SelectedIndexChanged);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(216, 32);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(64, 16);
			this.label11.TabIndex = 29;
			this.label11.Text = "Font Name:";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(16, 48);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(40, 16);
			this.label12.TabIndex = 27;
			this.label12.Text = "Text:";
			// 
			// numericUpDownTextSize
			// 
			this.numericUpDownTextSize.Location = new System.Drawing.Point(304, 64);
			this.numericUpDownTextSize.Name = "numericUpDownTextSize";
			this.numericUpDownTextSize.Size = new System.Drawing.Size(96, 20);
			this.numericUpDownTextSize.TabIndex = 26;
			this.numericUpDownTextSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownTextSize.Value = new System.Decimal(new int[] {
																																				10,
																																				0,
																																				0,
																																				0});
			this.numericUpDownTextSize.ValueChanged += new System.EventHandler(this.numericUpDownTextSize_ValueChanged);
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(232, 64);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(64, 23);
			this.label13.TabIndex = 28;
			this.label13.Text = "Point Size:";
			// 
			// checkBoxTextBold
			// 
			this.checkBoxTextBold.Location = new System.Drawing.Point(208, 96);
			this.checkBoxTextBold.Name = "checkBoxTextBold";
			this.checkBoxTextBold.Size = new System.Drawing.Size(72, 24);
			this.checkBoxTextBold.TabIndex = 37;
			this.checkBoxTextBold.Text = "Bold";
			this.checkBoxTextBold.CheckedChanged += new System.EventHandler(this.checkBoxTextBold_CheckedChanged);
			// 
			// checkBoxTextItalic
			// 
			this.checkBoxTextItalic.Location = new System.Drawing.Point(208, 120);
			this.checkBoxTextItalic.Name = "checkBoxTextItalic";
			this.checkBoxTextItalic.Size = new System.Drawing.Size(72, 24);
			this.checkBoxTextItalic.TabIndex = 36;
			this.checkBoxTextItalic.Text = "Italic";
			this.checkBoxTextItalic.CheckedChanged += new System.EventHandler(this.checkBoxTextItalic_CheckedChanged);
			// 
			// checkBoxTextUnderline
			// 
			this.checkBoxTextUnderline.Location = new System.Drawing.Point(208, 144);
			this.checkBoxTextUnderline.Name = "checkBoxTextUnderline";
			this.checkBoxTextUnderline.Size = new System.Drawing.Size(72, 24);
			this.checkBoxTextUnderline.TabIndex = 35;
			this.checkBoxTextUnderline.Text = "Underline";
			this.checkBoxTextUnderline.CheckedChanged += new System.EventHandler(this.checkBoxTextUnderline_CheckedChanged);
			// 
			// checkBoxTextStrikeout
			// 
			this.checkBoxTextStrikeout.Location = new System.Drawing.Point(208, 168);
			this.checkBoxTextStrikeout.Name = "checkBoxTextStrikeout";
			this.checkBoxTextStrikeout.Size = new System.Drawing.Size(72, 24);
			this.checkBoxTextStrikeout.TabIndex = 40;
			this.checkBoxTextStrikeout.Text = "Strikeout";
			this.checkBoxTextStrikeout.CheckedChanged += new System.EventHandler(this.checkBoxTextStrikeout_CheckedChanged);
			// 
			// checkBoxTextAllCaps
			// 
			this.checkBoxTextAllCaps.Location = new System.Drawing.Point(280, 96);
			this.checkBoxTextAllCaps.Name = "checkBoxTextAllCaps";
			this.checkBoxTextAllCaps.Size = new System.Drawing.Size(72, 24);
			this.checkBoxTextAllCaps.TabIndex = 32;
			this.checkBoxTextAllCaps.Text = "All Caps";
			this.checkBoxTextAllCaps.CheckedChanged += new System.EventHandler(this.checkBoxTextAllCaps_CheckedChanged);
			// 
			// checkBoxTextShadow
			// 
			this.checkBoxTextShadow.Location = new System.Drawing.Point(280, 144);
			this.checkBoxTextShadow.Name = "checkBoxTextShadow";
			this.checkBoxTextShadow.Size = new System.Drawing.Size(72, 24);
			this.checkBoxTextShadow.TabIndex = 31;
			this.checkBoxTextShadow.Text = "Shadow";
			this.checkBoxTextShadow.CheckedChanged += new System.EventHandler(this.checkBoxTextShadow_CheckedChanged);
			// 
			// checkBoxTextDoubleSpace
			// 
			this.checkBoxTextDoubleSpace.Location = new System.Drawing.Point(280, 120);
			this.checkBoxTextDoubleSpace.Name = "checkBoxTextDoubleSpace";
			this.checkBoxTextDoubleSpace.Size = new System.Drawing.Size(96, 24);
			this.checkBoxTextDoubleSpace.TabIndex = 33;
			this.checkBoxTextDoubleSpace.Text = "Double Space";
			this.checkBoxTextDoubleSpace.CheckedChanged += new System.EventHandler(this.checkBoxTextDoubleSpace_CheckedChanged);
			// 
			// buttonTextForeColor
			// 
			this.buttonTextForeColor.Location = new System.Drawing.Point(288, 168);
			this.buttonTextForeColor.Name = "buttonTextForeColor";
			this.buttonTextForeColor.Size = new System.Drawing.Size(64, 24);
			this.buttonTextForeColor.TabIndex = 25;
			this.buttonTextForeColor.Text = "Fore";
			this.buttonTextForeColor.Click += new System.EventHandler(this.buttonTextForeColor_Click);
			// 
			// buttonTextBackColor
			// 
			this.buttonTextBackColor.Location = new System.Drawing.Point(368, 168);
			this.buttonTextBackColor.Name = "buttonTextBackColor";
			this.buttonTextBackColor.Size = new System.Drawing.Size(64, 24);
			this.buttonTextBackColor.TabIndex = 24;
			this.buttonTextBackColor.Text = "Back";
			this.buttonTextBackColor.Click += new System.EventHandler(this.buttonTextBackColor_Click);
			// 
			// groupBoxTextProperties
			// 
			this.groupBoxTextProperties.Location = new System.Drawing.Point(8, 0);
			this.groupBoxTextProperties.Name = "groupBoxTextProperties";
			this.groupBoxTextProperties.Size = new System.Drawing.Size(184, 200);
			this.groupBoxTextProperties.TabIndex = 51;
			this.groupBoxTextProperties.TabStop = false;
			this.groupBoxTextProperties.Text = "LegacyText Properties";
			// 
			// groupBoxTextFontProperties
			// 
			this.groupBoxTextFontProperties.Location = new System.Drawing.Point(200, 0);
			this.groupBoxTextFontProperties.Name = "groupBoxTextFontProperties";
			this.groupBoxTextFontProperties.Size = new System.Drawing.Size(280, 200);
			this.groupBoxTextFontProperties.TabIndex = 52;
			this.groupBoxTextFontProperties.TabStop = false;
			this.groupBoxTextFontProperties.Text = "Font Properties";
			// 
			// tabStyleDialogs
			// 
			this.tabStyleDialogs.Controls.Add(this.buttonSymbolStyleDialog);
			this.tabStyleDialogs.Controls.Add(this.buttonTextStyleDialog);
			this.tabStyleDialogs.Controls.Add(this.buttonAreaStyleDialog);
			this.tabStyleDialogs.Controls.Add(this.buttonLineStyleDialog);
			this.tabStyleDialogs.Location = new System.Drawing.Point(4, 22);
			this.tabStyleDialogs.Name = "tabStyleDialogs";
			this.tabStyleDialogs.Size = new System.Drawing.Size(488, 206);
			this.tabStyleDialogs.TabIndex = 7;
			this.tabStyleDialogs.Text = "Style Dialogs";
			// 
			// buttonSymbolStyleDialog
			// 
			this.buttonSymbolStyleDialog.Location = new System.Drawing.Point(16, 112);
			this.buttonSymbolStyleDialog.Name = "buttonSymbolStyleDialog";
			this.buttonSymbolStyleDialog.Size = new System.Drawing.Size(128, 23);
			this.buttonSymbolStyleDialog.TabIndex = 3;
			this.buttonSymbolStyleDialog.Text = "&Symbol Style Dialog";
			this.buttonSymbolStyleDialog.Click += new System.EventHandler(this.buttonSymbolStyleDialog_Click);
			// 
			// buttonTextStyleDialog
			// 
			this.buttonTextStyleDialog.Location = new System.Drawing.Point(16, 80);
			this.buttonTextStyleDialog.Name = "buttonTextStyleDialog";
			this.buttonTextStyleDialog.Size = new System.Drawing.Size(128, 23);
			this.buttonTextStyleDialog.TabIndex = 2;
			this.buttonTextStyleDialog.Text = "&Text Style Dialog";
			this.buttonTextStyleDialog.Click += new System.EventHandler(this.buttonTextStyleDialog_Click);
			// 
			// buttonAreaStyleDialog
			// 
			this.buttonAreaStyleDialog.Location = new System.Drawing.Point(16, 40);
			this.buttonAreaStyleDialog.Name = "buttonAreaStyleDialog";
			this.buttonAreaStyleDialog.Size = new System.Drawing.Size(128, 24);
			this.buttonAreaStyleDialog.TabIndex = 1;
			this.buttonAreaStyleDialog.Text = "&Area Style Dialog...";
			this.buttonAreaStyleDialog.Click += new System.EventHandler(this.buttonAreaStyleDialog_Click);
			// 
			// buttonLineStyleDialog
			// 
			this.buttonLineStyleDialog.Location = new System.Drawing.Point(16, 8);
			this.buttonLineStyleDialog.Name = "buttonLineStyleDialog";
			this.buttonLineStyleDialog.Size = new System.Drawing.Size(128, 24);
			this.buttonLineStyleDialog.TabIndex = 0;
			this.buttonLineStyleDialog.Text = "&Line Style Dialog...";
			this.buttonLineStyleDialog.Click += new System.EventHandler(this.buttonLineStyleDialog_Click);
			// 
			// MapForm1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(496, 414);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.panel1);
			this.MinimumSize = new System.Drawing.Size(250, 200);
			this.Name = "MapForm1";
			this.Text = "Styles Sample";
			this.SizeChanged += new System.EventHandler(this.MapForm1_SizeChanged);
			this.Load += new System.EventHandler(this.MapForm1_Load);
			this.panel1.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabStock.ResumeLayout(false);
			this.tabLines.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownPixelWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownPointWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownLinePattern)).EndInit();
			this.tabFontSymbols.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownFontCode)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownFontPointSize)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownFontAngle)).EndInit();
			this.tabFills.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownFillPattern)).EndInit();
			this.tabVectorSymbols.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownVectorCode)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownVectorPointSize)).EndInit();
			this.tabBitmapSymbols.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownBitmapPointSize)).EndInit();
			this.tabText.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownTextAngle)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownTextSize)).EndInit();
			this.tabStyleDialogs.ResumeLayout(false);
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

		private void MapForm1_Load(object sender, System.EventArgs e)
		{
			//replace default Map created by mapcontrol with FeatureRenderer 
			CoordSys csys = Session.Current.CoordSysFactory.CreateCoordSys(CoordSysType.NonEarth, null, DistanceUnit.Meter, 0, 0, 0, 0, 0, 0, 0, 0, 0, new MapInfo.Geometry.DRect(0, 0, mapControl1.Map.Size.Width, mapControl1.Map.Size.Height), null);
			FeatureRenderer fr = Session.Current.MapFactory.CreateFeatureRenderer("stylesample", "stylesample", mapControl1.Map.Handle, mapControl1.Map.Size, csys);
			mapControl1.Map = fr;
						
			fr.SetView(csys.Bounds, csys);

			radioBlackFillStyle.Checked = true;
			
			// used as storage for line tab
			_lineStyle = new SimpleLineStyle();

			// set updown limits for line tab
			numericUpDownPixelWidth.Minimum = (decimal)LineWidth.MinPixel;
			numericUpDownPixelWidth.Maximum = (decimal)LineWidth.MaxPixel;
			numericUpDownPixelWidth.Value = (decimal)LineWidth.MinPixel;
			numericUpDownPointWidth.Minimum = (decimal)LineWidth.MinPoint;
			numericUpDownPointWidth.Maximum = (decimal)LineWidth.MaxPoint;
			numericUpDownPointWidth.Value = (decimal)LineWidth.MinPoint;
			if (_lineStyle.Width.Unit == LineWidthUnit.Pixel) 
			{
				numericUpDownPixelWidth.Value = (decimal)_lineStyle.Width.Value;
				radioPixelWidth.Checked = true;
			}
			else 
			{
				numericUpDownPointWidth.Value = (decimal)_lineStyle.Width.Value;
				radioPointWidth.Checked = true;
			}

			numericUpDownLinePattern.Minimum = SimpleLineStyle.MinLinePattern;
			numericUpDownLinePattern.Maximum = SimpleLineStyle.MaxLinePattern;
			numericUpDownLinePattern.Value = _lineStyle.Pattern;

			// for fill tab
			_fillStyle = new SimpleInterior();
			numericUpDownFillPattern.Minimum = SimpleInterior.MinFillPattern;
			numericUpDownFillPattern.Maximum = SimpleInterior.MaxFillPattern;
			numericUpDownLinePattern.Value = _fillStyle.Pattern;

			// vector symbols
			_vectorSymbol = new SimpleVectorPointStyle();
			numericUpDownVectorPointSize.Minimum = BasePointStyle.MinPointSize;
			numericUpDownVectorPointSize.Maximum = BasePointStyle.MaxPointSize;
			numericUpDownVectorPointSize.Value = (decimal)_vectorSymbol.PointSize;

			// mapinfo.fnt valid values
			numericUpDownVectorCode.Minimum = 31;
			numericUpDownVectorCode.Maximum = 67;

			// bitmap symbol
			_bitmapSymbol = new BitmapPointStyle();
			foreach (BitmapPointStyle b in Session.Current.StyleRepository.BitmapPointStyleRepository) 
			{ 
				listBoxBitmapNames.Items.Add(b.Name);
			}
			checkBoxBitmapShowBackground.Checked = _bitmapSymbol.ShowWhiteBackground;
			checkBoxBitmapApplyColor.Checked = _bitmapSymbol.ApplyColor;
			numericUpDownBitmapPointSize.Minimum = BasePointStyle.MinPointSize;
			numericUpDownBitmapPointSize.Maximum = BasePointStyle.MaxPointSize;
			numericUpDownBitmapPointSize.Value = (decimal)_bitmapSymbol.PointSize;
			// font symbol
			_fontSymbol = new FontPointStyle();
			numericUpDownFontPointSize.Minimum = BasePointStyle.MinPointSize;
			numericUpDownFontPointSize.Maximum = BasePointStyle.MaxPointSize;
			numericUpDownFontPointSize.Value = (decimal)_fontSymbol.PointSize;
			numericUpDownFontCode.Minimum = BasePointStyle.MinCode;
			numericUpDownFontCode.Maximum = BasePointStyle.MaxCode;
			numericUpDownFontCode.Value = _fontSymbol.Code;

			// text
			_textStyle = new TextStyle();
      _textAngle = 0.0;
			numericUpDownTextSize.Minimum = 1;
			numericUpDownTextSize.Maximum = 96;
      numericUpDownTextSize.Value = (decimal)_fontSymbol.PointSize;
      numericUpDownTextAngle.Minimum = -360;
      numericUpDownTextAngle.Maximum = 360;

			Graphics g = CreateGraphics();
			FontFamily[] families = FontFamily.GetFamilies(g);
			// Draw text using each of the font families.
			foreach (FontFamily family in families)
			{
				comboBoxFontFamilies.Items.Add(family.Name);
				comboBoxTextFontFamily.Items.Add(family.Name);
			}
			g.Dispose();

			// select first fontname in each list (note that _fontSymbol and _textStyle
			// have to be constructed first since this causes a SelectedIndexChanged event).
			comboBoxFontFamilies.SelectedIndex = 0;
			comboBoxTextFontFamily.SelectedIndex = 0;

			foreach (Alignment alignment in Enum.GetValues(typeof(Alignment))) {
			  comboBoxTextAlignment.Items.Add(alignment);
			}
      comboBoxTextAlignment.SelectedIndex = 0;

			foreach (Spacing spacing in Enum.GetValues(typeof(Spacing))) {
			  comboBoxTextSpacing.Items.Add(spacing);
			}
			comboBoxTextSpacing.SelectedIndex = 0;

			// force first sample to be drawn
			tabControl1.SelectedIndex = 0;
			tabControl1_SelectedIndexChanged(this, null);
		}

		private void SetStockStyle()
		{
			FeatureRenderer fr = mapControl1.Map as FeatureRenderer;
			Feature f = null;
			if (radioBlackFillStyle.Checked)
			{
				// get region 
				f = fr.AreaSample(10.0);
				// set style
				CompositeStyle cs = (CompositeStyle)f.Style;
				cs.AreaStyle.Interior = StockStyles.BlackFillStyle();
			}
			else if (radioBlueFillStyle.Checked)
			{
				// get region 
				f = fr.AreaSample(10.0);
				// set style
				CompositeStyle cs = (CompositeStyle)f.Style;
				cs.AreaStyle.Interior = StockStyles.BlueFillStyle();
			}
			else if (radioHollowFillStyle.Checked)
			{
				// get region 
				f = fr.AreaSample(10.0);
				// set style
				CompositeStyle cs = (CompositeStyle)f.Style;
				cs.AreaStyle.Interior = StockStyles.HollowFillStyle();
			}
			else if (radioRedFillStyle.Checked)
			{
				// get region 
				f = fr.AreaSample(10.0);
				// set style
				CompositeStyle cs = (CompositeStyle)f.Style;
				cs.AreaStyle.Interior = StockStyles.RedFillStyle();
			}
			else if (radioWhiteFillStyle.Checked)
			{
				// get region 
				f = fr.AreaSample(10.0);
				// set style
				CompositeStyle cs = (CompositeStyle)f.Style;
				cs.AreaStyle.Interior = StockStyles.WhiteFillStyle();
			}
			else if (radioBlackLineStyle.Checked) 
			{
				f = fr.LineSample(10.0, FeatureRenderer.LineSampleType.ForwardDiagonal);
				CompositeStyle cs = (CompositeStyle)f.Style;
				cs.LineStyle = StockStyles.BlackLineStyle();
			}
			else if (radioBlueLineStyle.Checked) 
			{
				f = fr.LineSample(10.0, FeatureRenderer.LineSampleType.ForwardDiagonal);
				CompositeStyle cs = (CompositeStyle)f.Style;
				cs.LineStyle = StockStyles.BlueLineStyle();
			}
			else if (radioHollowLineStyle.Checked) 
			{
				f = fr.LineSample(10.0, FeatureRenderer.LineSampleType.ForwardDiagonal);
				CompositeStyle cs = (CompositeStyle)f.Style;
				cs.LineStyle = StockStyles.HollowLineStyle();
			}
			else if (radioRedLineStyle.Checked) 
			{
				f = fr.LineSample(10.0, FeatureRenderer.LineSampleType.ForwardDiagonal);
				CompositeStyle cs = (CompositeStyle)f.Style;
				cs.LineStyle = StockStyles.RedLineStyle();
			}
			fr.Feature = f;
		}
		private void radioBlackFillStyle_CheckedChanged(object sender, System.EventArgs e)
		{
			SetStockStyle();
		}

		private void radioBlueFillStyle_CheckedChanged(object sender, System.EventArgs e)
		{
			SetStockStyle();
		}

		private void radioHollowFillStyle_CheckedChanged(object sender, System.EventArgs e)
		{
			SetStockStyle();
		}

		private void radioRedFillStyle_CheckedChanged(object sender, System.EventArgs e)
		{
			SetStockStyle();
		}

		private void radioWhiteFillStyle_CheckedChanged(object sender, System.EventArgs e)
		{
			SetStockStyle();
		}

		private void radioBlackLineStyle_CheckedChanged(object sender, System.EventArgs e)
		{		
			SetStockStyle();
		}

		private void radioBlueLineStyle_CheckedChanged(object sender, System.EventArgs e)
		{
			SetStockStyle();
		}

		private void radioHollowLineStyle_CheckedChanged(object sender, System.EventArgs e)
		{
			SetStockStyle();
		}

		private void radioRedLineStyle_CheckedChanged(object sender, System.EventArgs e)
		{
			SetStockStyle();
		}

		private void btnBackColor_Click(object sender, System.EventArgs e)
		{
			ColorDialog dlg = new ColorDialog();
			if(dlg.ShowDialog() == DialogResult.OK)
			{
				mapControl1.Map.BackgroundBrush = new SolidBrush(dlg.Color);
			}

		}

		private void SetLineSample()
		{
			FeatureRenderer fr = mapControl1.Map as FeatureRenderer;
			Feature f=null;
			if (Session.Current.StyleRepository.LineStyleRepository.CanInterleavePattern(_lineStyle.Pattern))
			{
				f = fr.LineSample(10.0, FeatureRenderer.LineSampleType.CrossedDiagonals);
			}
			else 
			{
				f = fr.LineSample(10.0, FeatureRenderer.LineSampleType.ForwardDiagonal);
			}
			CompositeStyle cs = (CompositeStyle)f.Style;
			cs.LineStyle = _lineStyle;
			fr.Feature = f;		
		}

		private void btnLineColor_Click(object sender, System.EventArgs e)
		{
			ColorDialog dlg = new ColorDialog();
			dlg.Color = _lineStyle.Color;
			if(dlg.ShowDialog() == DialogResult.OK)
			{
				_lineStyle.Color = dlg.Color;
				SetLineSample();
			}		
		}

		private void radioPixelWidth_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioPixelWidth.Checked) {
				_lineStyle.Width = new LineWidth((double)numericUpDownPixelWidth.Value, LineWidthUnit.Pixel);
				SetLineSample();
			}
		}

		private void radioPointWidth_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioPointWidth.Checked) 
			{
				_lineStyle.Width = new LineWidth((double)numericUpDownPointWidth.Value, LineWidthUnit.Point);
				SetLineSample();
			}		
		}
		private void numericUpDownPixelWidth_ValueChanged(object sender, System.EventArgs e)
		{
			if (radioPixelWidth.Checked) 
			{
				_lineStyle.Width = new LineWidth((double)numericUpDownPixelWidth.Value, LineWidthUnit.Pixel);
				SetLineSample();
			}
		}

		private void numericUpDownPointWidth_ValueChanged(object sender, System.EventArgs e)
		{
			if (radioPointWidth.Checked) 
			{
				_lineStyle.Width = new LineWidth((double)numericUpDownPointWidth.Value, LineWidthUnit.Point);
				SetLineSample();
			}
		}

		private void numericUpDownLinePattern_ValueChanged(object sender, System.EventArgs e)
		{
			try 
			{
				// LineStyleRepository will throw an exception when the pattern value is invalid or can't be found
				// in the repository.
				int pattern = (int)numericUpDownLinePattern.Value;
				checkBoxLineInterleaved.Enabled = Session.Current.StyleRepository.LineStyleRepository.CanInterleavePattern(pattern);

				_lineStyle.Pattern = pattern;
				SetLineSample();	
			} 
			catch (System.ArgumentOutOfRangeException)
			{
				checkBoxLineInterleaved.Enabled = false;
			}
		}

		private void checkBoxLineInterleaved_CheckedChanged(object sender, System.EventArgs e)
		{
			_lineStyle.Interleaved = checkBoxLineInterleaved.Checked;
			SetLineSample();		
		}

		private void MapForm1_SizeChanged(object sender, System.EventArgs e)
		{
			// size changed. so lets reset the render to get a nonearth coordsys 
			// at the new size. 
			FeatureRenderer fr = mapControl1.Map as FeatureRenderer;
			if(fr != null)
			{
				CoordSys csys = null;
				if(mapControl1.Map.Size.Height != 0 && mapControl1.Map.Size.Width != 0 )
				{
					csys = Session.Current.CoordSysFactory.CreateCoordSys(CoordSysType.NonEarth, null, DistanceUnit.Meter, 0, 0, 0, 0, 0, 0, 0, 0, 0, new MapInfo.Geometry.DRect(0, 0, mapControl1.Map.Size.Width, mapControl1.Map.Size.Height), null);
				}
				if(csys != null)
				{
					fr.Reset(mapControl1.Map.Size, csys);
					fr.SetView(csys.Bounds, csys);

					tabControl1_SelectedIndexChanged(this, null);
				}
			}
		}

		private void SetFillSample()
		{
			// use linestyle also
			FeatureRenderer fr = mapControl1.Map as FeatureRenderer;
			Feature f = fr.AreaSample(10.0);
			CompositeStyle cs = (CompositeStyle)f.Style;
			cs.AreaStyle.Border = _lineStyle;
			cs.AreaStyle.Interior = _fillStyle;
			fr.Feature = f;
		}

		private void btnFillForeColor_Click(object sender, System.EventArgs e)
		{
			ColorDialog dlg = new ColorDialog();
			dlg.Color = _fillStyle.ForeColor;
			if(dlg.ShowDialog() == DialogResult.OK)
			{
				_fillStyle.ForeColor = dlg.Color;
				SetFillSample();
			}		
		}

		private void btnFillBackColor_Click(object sender, System.EventArgs e)
		{
			ColorDialog dlg = new ColorDialog();
			dlg.Color = _fillStyle.BackColor;
			if(dlg.ShowDialog() == DialogResult.OK)
			{
				_fillStyle.BackColor = dlg.Color;
				SetFillSample();
			}		
		}

		private void numericUpDownFillPattern_ValueChanged(object sender, System.EventArgs e)
		{
			_fillStyle.Pattern = (int)numericUpDownFillPattern.Value;
			SetFillSample();
		}

		private void checkBoxFillTransparent_CheckedChanged(object sender, System.EventArgs e)
		{
			_fillStyle.Transparent = checkBoxFillTransparent.Checked;
			SetFillSample();		
		}

		private void SetVectorSymbolSample()
		{
			FeatureRenderer fr = mapControl1.Map as FeatureRenderer;
			Feature f = fr.VectorSymbolSample();
			CompositeStyle cs = (CompositeStyle)f.Style;
			cs.SymbolStyle = _vectorSymbol;
			fr.Feature = f;
		}

		private void btnVectorColor_Click(object sender, System.EventArgs e)
		{
			ColorDialog dlg = new ColorDialog();
			dlg.Color = _vectorSymbol.Color;
			if(dlg.ShowDialog() == DialogResult.OK)
			{
				_vectorSymbol.Color = dlg.Color;
				SetVectorSymbolSample();
			}		
		}

		private void numericUpDownVectorCode_ValueChanged(object sender, System.EventArgs e)
		{
			_vectorSymbol.Code = (short)numericUpDownVectorCode.Value;
			SetVectorSymbolSample();
		}

		private void numericUpDownVectorPointSize_ValueChanged(object sender, System.EventArgs e)
		{
			_vectorSymbol.PointSize = (short)numericUpDownVectorPointSize.Value;
			SetVectorSymbolSample();
		}

		private void SetBitmapSymbolSample()
		{
			FeatureRenderer fr = mapControl1.Map as FeatureRenderer;
			Feature f = fr.BitmapSymbolSample();
			CompositeStyle cs = (CompositeStyle)f.Style;
			cs.SymbolStyle = _bitmapSymbol;
			fr.Feature = f;
		}
		private void buttonBitmapColor_Click(object sender, System.EventArgs e)
		{
			ColorDialog dlg = new ColorDialog();
			dlg.Color = _bitmapSymbol.Color;
			if(dlg.ShowDialog() == DialogResult.OK)
			{
				_bitmapSymbol.Color = dlg.Color;
				SetBitmapSymbolSample();
			}			
		}

		private void numericUpDownBitmapPointSize_ValueChanged(object sender, System.EventArgs e)
		{
			_bitmapSymbol.PointSize = (short)(numericUpDownBitmapPointSize.Value);
			SetBitmapSymbolSample();
		}

		private void checkBoxBitmapApplyColor_CheckedChanged(object sender, System.EventArgs e)
		{
			_bitmapSymbol.ApplyColor = checkBoxBitmapApplyColor.Checked;
			SetBitmapSymbolSample();
		}

		private void checkBoxBitmapShowBackground_CheckedChanged(object sender, System.EventArgs e)
		{
			_bitmapSymbol.ShowWhiteBackground = checkBoxBitmapShowBackground.Checked;
			SetBitmapSymbolSample();		
		}

		private void listBoxBitmapNames_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			_bitmapSymbol.Name = listBoxBitmapNames.SelectedItem.ToString();
			SetBitmapSymbolSample();		
		}


		private void SetFontSymbolSample()
		{
			FeatureRenderer fr = mapControl1.Map as FeatureRenderer;
			Feature f = fr.VectorSymbolSample();
			CompositeStyle cs = (CompositeStyle)f.Style;
			cs.SymbolStyle = _fontSymbol;
			fr.Feature = f;
		}

		// when tab changes, show sample for current tab
		private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch(tabControl1.SelectedIndex)
			{
				case 0:
					SetStockStyle();
					break;
				case 1:
					SetFillSample();
					break;
				case 2:
					SetLineSample();
					break;
				case 3:
					SetFontSymbolSample();
					break;
				case 4:
					SetVectorSymbolSample();
					break;
				case 5:
					SetBitmapSymbolSample();
					break;
				case 6:
					SetTextSample();
					break;
			}
		
		}

		private void buttonFontColor_Click(object sender, System.EventArgs e)
		{
			ColorDialog dlg = new ColorDialog();
			dlg.Color = _fontSymbol.Color;
			if(dlg.ShowDialog() == DialogResult.OK)
			{
				_fontSymbol.Color = dlg.Color;
				SetFontSymbolSample();
			}	
		}

		private void numericUpDownFontCode_ValueChanged(object sender, System.EventArgs e)
		{
			_fontSymbol.Code = (short)(numericUpDownFontCode.Value);
			SetFontSymbolSample();		
		}

		private void numericUpDownFontPointSize_ValueChanged(object sender, System.EventArgs e)
		{
			_fontSymbol.PointSize = (short)(numericUpDownFontPointSize.Value);
			SetFontSymbolSample();		
		}

		private void numericUpDownFontAngle_ValueChanged(object sender, System.EventArgs e)
		{
			// FontPointStyle angles are marshalled as tenths of a degree
			short sAngle = 0;
			try 
			{
				// convert to tenths of a degree
				sAngle = Convert.ToInt16(Math.IEEERemainder((double)numericUpDownFontAngle.Value, 360.0) * 10);
			}
			catch (System.OverflowException) 
			{
				sAngle = 0;
			}
			_fontSymbol.Angle = sAngle;
			SetFontSymbolSample();
		}

		private void comboBoxFontFamilies_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string name = comboBoxFontFamilies.SelectedItem.ToString();
			MapInfo.Styles.Font f = _fontSymbol.Font;
			f.Name = name;
			SetFontSymbolSample();
		}

		private void checkBoxFontBold_CheckedChanged(object sender, System.EventArgs e)
		{
			MapInfo.Styles.Font f = _fontSymbol.Font;
			f.FontWeight = checkBoxFontBold.Checked ? FontWeight.Bold : FontWeight.Normal;
			SetFontSymbolSample();
		}

		private void checkBoxFontItalic_CheckedChanged(object sender, System.EventArgs e)
		{
			MapInfo.Styles.Font f = _fontSymbol.Font;
			f.FontFaceStyle = checkBoxFontItalic.Checked ? FontFaceStyle.Italic : FontFaceStyle.Normal;
			SetFontSymbolSample();
		}

		private void checkBoxFontUnderline_CheckedChanged(object sender, System.EventArgs e)
		{
			MapInfo.Styles.Font f = _fontSymbol.Font;
			f.TextDecoration |= checkBoxFontUnderline.Checked ? TextDecoration.Underline : TextDecoration.None;
			SetFontSymbolSample();
		}

		private void checkBoxFontStrikeout_CheckedChanged(object sender, System.EventArgs e)
		{
			MapInfo.Styles.Font f = _fontSymbol.Font;
			f.TextDecoration |= checkBoxFontStrikeout.Checked ? TextDecoration.Strikeout : TextDecoration.None;
			SetFontSymbolSample();
		}

		private void checkBoxFontAllCaps_CheckedChanged(object sender, System.EventArgs e)
		{
			MapInfo.Styles.Font f = _fontSymbol.Font;
			f.TextCase = checkBoxFontAllCaps.Checked ? TextCase.AllCaps : TextCase.Default;
			SetFontSymbolSample();
		}

		private void checkBoxFontDoubleSpace_CheckedChanged(object sender, System.EventArgs e)
		{
			MapInfo.Styles.Font f = _fontSymbol.Font;
			f.DblSpace = checkBoxFontDoubleSpace.Checked;
			SetFontSymbolSample();
		}

		private void checkBoxFontShadow_CheckedChanged(object sender, System.EventArgs e)
		{
			MapInfo.Styles.Font f = _fontSymbol.Font;
			f.Shadow = checkBoxFontShadow.Checked;
			SetFontSymbolSample();
		}

		private void buttonFontForeColor_Click(object sender, System.EventArgs e)
		{
			ColorDialog dlg = new ColorDialog();
			dlg.Color = _fontSymbol.Color;
			if(dlg.ShowDialog() == DialogResult.OK)
			{
				_fontSymbol.Color = dlg.Color;
				SetFontSymbolSample();
			}
		}

		private void buttonFontBackColor_Click(object sender, System.EventArgs e)
		{
			MapInfo.Styles.Font f = _fontSymbol.Font;
			ColorDialog dlg = new ColorDialog();
			dlg.Color = f.BackColor;
			if(dlg.ShowDialog() == DialogResult.OK)
			{
				f.BackColor = dlg.Color;
				SetFontSymbolSample();
			}			
		}

		private void SetTextSample()
		{
      String textBoxString = textBoxTextText.Text;
      string textString;
      if (textBoxString.Length == 0) {
        textString = " ";
      } else {
        textString = textBoxString.Replace(Environment.NewLine, "\n");
      }
			FeatureRenderer fr = mapControl1.Map as FeatureRenderer;
			Feature f = fr.TextSample(textString, _textAngle, _textAlignment, _textSpacing, _textStyle);
			CompositeStyle cs = (CompositeStyle)f.Style;
			cs.TextStyle = _textStyle;
			fr.Feature = f;
		}

		private void comboBoxTextFontFamily_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string name = comboBoxTextFontFamily.SelectedItem.ToString();
			MapInfo.Styles.Font f = _textStyle.Font;
			f.Name = name;
			_textStyle.Font = f;
			SetTextSample();
		}

		private void textBoxTextText_TextChanged(object sender, System.EventArgs e)
		{
			SetTextSample();
		}

		private void numericUpDownTextSize_ValueChanged(object sender, System.EventArgs e)
		{
			MapInfo.Styles.Font f = _textStyle.Font;
			f.Size = (short)numericUpDownTextSize.Value;
			_textStyle.Font = f;
			SetTextSample();
		}

		private void numericUpDownTextAngle_ValueChanged(object sender, System.EventArgs e)
		{
			_textAngle = (double)numericUpDownTextAngle.Value;
			SetTextSample();
		}

		private void comboBoxTextAlignment_SelectedIndexChanged(object sender, System.EventArgs e)
		{
      _textAlignment = (Alignment)comboBoxTextAlignment.SelectedIndex;
			SetTextSample();
		}

		private void comboBoxTextSpacing_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		  _textSpacing = (Spacing)comboBoxTextSpacing.SelectedIndex;
			SetTextSample();
		}

		private void buttonTextForeColor_Click(object sender, System.EventArgs e)
		{
			MapInfo.Styles.Font f = _textStyle.Font;
			ColorDialog dlg = new ColorDialog();
			dlg.Color = f.ForeColor;
			if(dlg.ShowDialog() == DialogResult.OK)
			{
				f.ForeColor = dlg.Color;
				_textStyle.Font = f;
				SetTextSample();
			}			
		}

		private void buttonTextBackColor_Click(object sender, System.EventArgs e)
		{
			MapInfo.Styles.Font f = _textStyle.Font;
			ColorDialog dlg = new ColorDialog();
			dlg.Color = f.BackColor;
			if(dlg.ShowDialog() == DialogResult.OK)
			{
				f.BackColor = dlg.Color;
				_textStyle.Font = f;
				SetTextSample();
			}			
		}

		private void checkBoxTextBold_CheckedChanged(object sender, System.EventArgs e)
		{
			MapInfo.Styles.Font f = _textStyle.Font;
			f.FontWeight = checkBoxTextBold.Checked ? FontWeight.Bold : FontWeight.Normal;
			_textStyle.Font = f;
			SetTextSample();
		}

		private void checkBoxTextItalic_CheckedChanged(object sender, System.EventArgs e)
		{
			MapInfo.Styles.Font f = _textStyle.Font;
			f.FontFaceStyle = checkBoxTextItalic.Checked ? FontFaceStyle.Italic : FontFaceStyle.Normal;
			_textStyle.Font = f;
			SetTextSample();
		}

		private void checkBoxTextUnderline_CheckedChanged(object sender, System.EventArgs e)
		{
			MapInfo.Styles.Font f = _textStyle.Font;
			f.TextDecoration |= checkBoxTextUnderline.Checked ? TextDecoration.Underline : TextDecoration.None;
			_textStyle.Font = f;
			SetTextSample();
		}

		private void checkBoxTextStrikeout_CheckedChanged(object sender, System.EventArgs e)
		{
			MapInfo.Styles.Font f = _textStyle.Font;
			f.TextDecoration |= checkBoxTextStrikeout.Checked ? TextDecoration.Strikeout : TextDecoration.None;
			_textStyle.Font = f;
			SetTextSample();
		}

		private void checkBoxTextAllCaps_CheckedChanged(object sender, System.EventArgs e)
		{
			MapInfo.Styles.Font f = _textStyle.Font;
			f.TextCase = checkBoxTextAllCaps.Checked ? TextCase.AllCaps : TextCase.Default;
			_textStyle.Font = f;
			SetTextSample();
		}

		private void checkBoxTextDoubleSpace_CheckedChanged(object sender, System.EventArgs e)
		{
			MapInfo.Styles.Font f = _textStyle.Font;
			f.DblSpace = checkBoxTextDoubleSpace.Checked;
			_textStyle.Font = f;
			SetTextSample();
		}

		private void checkBoxTextShadow_CheckedChanged(object sender, System.EventArgs e)
		{
			MapInfo.Styles.Font f = _textStyle.Font;
			f.Shadow = checkBoxTextShadow.Checked;
			_textStyle.Font = f;
			SetTextSample();
		}

		private void buttonLineStyleDialog_Click(object sender, System.EventArgs e)
		{
			if (_lineStyleDlg == null) 
			{
				_lineStyleDlg = new LineStyleDlg(); 
			}
			_lineStyleDlg.LineStyle = _lineStyle; 
			if (_lineStyleDlg.ShowDialog() == DialogResult.OK) 
			{
				BaseLineStyle ls = _lineStyleDlg.LineStyle; 
				if (ls is SimpleLineStyle) 
				{
					_lineStyle = (SimpleLineStyle)ls; 
					SetLineSample();
				} 
				else 
				{
					throw new System.NotImplementedException("New style type not handled.");
				}
			}
		}

		private void buttonAreaStyleDialog_Click(object sender, System.EventArgs e)
		{
			if (_areaStyleDlg == null) 
			{
				_areaStyleDlg = new AreaStyleDlg(); 
			}
			_areaStyleDlg.AreaStyle = new AreaStyle(_lineStyle, _fillStyle);
			if (_areaStyleDlg.ShowDialog() == DialogResult.OK) 
			{
				if (_areaStyleDlg.AreaStyle.Border is SimpleLineStyle) 
				{
					_lineStyle = (SimpleLineStyle)_areaStyleDlg.AreaStyle.Border; 
				} 
				else 
				{
					throw new System.NotImplementedException("New style type not handled.");
				}
				if (_areaStyleDlg.AreaStyle.Interior is SimpleInterior) 
				{
					_fillStyle = (SimpleInterior)_areaStyleDlg.AreaStyle.Interior; 
				} 
				else 
				{
					throw new System.NotImplementedException("New style type not handled.");
				}
				SetFillSample();
			}	
		}

		private void buttonTextStyleDialog_Click(object sender, System.EventArgs e)
		{
			if (_textStyleDlg == null) 
			{
				_textStyleDlg = new TextStyleDlg(); 
			}
			if (_textStyleDlg.ShowDialog() == DialogResult.OK) 
			{
				_textStyle.Font = _textStyleDlg.FontStyle;
				SetTextSample();
			}			
		}

		private void radioButtonFontNoBackground_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.radioButtonFontNoBackground.Checked) 
			{
				MapInfo.Styles.Font f = _fontSymbol.Font;
				f.TextEffect = TextEffect.None;
				SetFontSymbolSample();
			}
		}

		private void radioButtonFontHalo_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.radioButtonFontHalo.Checked) 
			{
				MapInfo.Styles.Font f = _fontSymbol.Font;
				f.TextEffect = TextEffect.Halo;
				SetFontSymbolSample();
			}
		}

		private void radioButtonFontOpaque_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.radioButtonFontOpaque.Checked) 
			{
				MapInfo.Styles.Font f = _fontSymbol.Font;
				f.TextEffect = TextEffect.Box;
				SetFontSymbolSample();
			}
		}

		private void radioButtonTextNoBackground_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.radioButtonTextNoBackground.Checked) 
			{
				MapInfo.Styles.Font f = _textStyle.Font;
				f.TextEffect = TextEffect.None;
				_textStyle.Font = f;
				SetTextSample();
			}
		}

		private void radioButtonTextHalo_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.radioButtonTextHalo.Checked) 
			{
				MapInfo.Styles.Font f = _textStyle.Font;
				f.TextEffect = TextEffect.Halo;
				_textStyle.Font = f;
				SetTextSample();
			}
		}

		private void radioButtonTextOpaque_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.radioButtonTextOpaque.Checked) 
			{
				MapInfo.Styles.Font f = _textStyle.Font;
				f.TextEffect = TextEffect.Box;
				_textStyle.Font = f;
				SetTextSample();
			}
		}

		private void buttonSymbolStyleDialog_Click(object sender, System.EventArgs e)
		{
			if (_symbolStyleDlg == null) 
			{
				_symbolStyleDlg = new SymbolStyleDlg();
				_symbolStyleDlg.SymbolStyle = _vectorSymbol;
			}
			if (_symbolStyleDlg.ShowDialog() == DialogResult.OK) 
			{
				MapInfo.Styles.BasePointStyle sym = _symbolStyleDlg.SymbolStyle;
				if (sym is MapInfo.Styles.BitmapPointStyle) 
				{
					_bitmapSymbol = sym as MapInfo.Styles.BitmapPointStyle;
					SetBitmapSymbolSample();
				} 
				else if (sym is MapInfo.Styles.FontPointStyle) 
				{
					_fontSymbol = sym as MapInfo.Styles.FontPointStyle;
					SetFontSymbolSample();
				} 
				else 
				{
					_vectorSymbol = sym as MapInfo.Styles.SimpleVectorPointStyle;
					SetVectorSymbolSample();
				}
			}		
		}
	}
}
