using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using MapInfo.Mapping;
using MapInfo.Styles;
using MapInfo.Windows.Dialogs;

namespace ChangeStyles
{
	/// <summary>
	/// This sample app demonstrates how to use FeatureOverrideStyleModifier and layer FeatureStyleModifiers to change styles of
	/// various features within a map
	/// </summary>
	public class Form1 : System.Windows.Forms.Form {
		private System.Windows.Forms.Button button1;


		private LineStyleDlg _lineStyleDlg = null;
		private AreaStyleDlg _areaStyleDlg = null;
		private TextStyleDlg _textStyleDlg = null;
		private SymbolStyleDlg _symbolStyleDlg = null;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private MapInfo.Windows.Controls.MapControl mapControl1;
		private System.Windows.Forms.Button button5;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			Microsoft.Win32.RegistryKey key =
				Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\MapInfo\MapXtreme\6.6");
			string s = (string)key.GetValue("SampleDataSearchPath");
			if (s != null && s.Length > 0) {
				if (s.EndsWith("\\")==false) {
					s += "\\";
				}
			}
			else {
				s = Environment.CurrentDirectory;
			}
			key.Close();
	
			MapInfo.Engine.Session.Current.TableSearchPath.Path = s;
			string geoSetName = "world.gst";
			try {
				mapControl1.Map.Load(new MapGeosetLoader(s + geoSetName));			
			}
			catch(Exception) {
				MessageBox.Show("Geoset " + geoSetName + " not found.");
			}	
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
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
		private void InitializeComponent() {
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.mapControl1 = new MapInfo.Windows.Controls.MapControl();
			this.button5 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			//
			// button1
			//
			this.button1.Location = new System.Drawing.Point(48, 272);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(104, 23);
			this.button1.TabIndex = 11;
			this.button1.Text = "Change AreaStyle";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			//
			// button2
			//
			this.button2.Location = new System.Drawing.Point(184, 272);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(104, 24);
			this.button2.TabIndex = 13;
			this.button2.Text = "Change LineStyle";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			//
			// button3
			//
			this.button3.Location = new System.Drawing.Point(320, 272);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(104, 24);
			this.button3.TabIndex = 14;
			this.button3.Text = "Change TextStyle";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			//
			// button4
			//
			this.button4.Location = new System.Drawing.Point(456, 272);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(112, 32);
			this.button4.TabIndex = 15;
			this.button4.Text = "Change SymbolStyle";
			this.button4.Click += new System.EventHandler(this.button4_Click);
			//
			// mapControl1
			//
			this.mapControl1.Location = new System.Drawing.Point(24, 8);
			this.mapControl1.Name = "mapControl1";
			this.mapControl1.Size = new System.Drawing.Size(584, 248);
			this.mapControl1.TabIndex = 16;
			this.mapControl1.Text = "mapControl1";
			this.mapControl1.Tools.LeftButtonTool = "ZoomIn";
			//
			// button5
			//
			this.button5.Location = new System.Drawing.Point(48, 320);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(120, 23);
			this.button5.TabIndex = 17;
			this.button5.Text = "Change Sparse Style";
			this.button5.Click += new System.EventHandler(this.button5_Click);
			//
			// Form1
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(656, 359);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.mapControl1);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new Form1());
		}

		// Change Area style of all regions in the region layer
		private void button1_Click(object sender, System.EventArgs e) {
			//Get the layer we want
			FeatureLayer _lyr = this.mapControl1.Map.Layers["world"] as FeatureLayer;
			//Create and show the style dialog
			if (_areaStyleDlg == null) {
				_areaStyleDlg = new AreaStyleDlg();
			}
			// After getting style from dialog, create and apply the featureoverridestylemodifier object to layer
			if (_areaStyleDlg.ShowDialog() == DialogResult.OK) {
				FeatureOverrideStyleModifier fsm = new FeatureOverrideStyleModifier(null, new MapInfo.Styles.CompositeStyle(_areaStyleDlg.AreaStyle));
				_lyr.Modifiers.Append(fsm);
				this.mapControl1.Map.Zoom = new MapInfo.Geometry.Distance(25000, MapInfo.Geometry.DistanceUnit.Mile);
			}
		}

		// Change Area style of all regions in the region layer
		private void button2_Click(object sender, System.EventArgs e) {
			FeatureLayer _lyr = this.mapControl1.Map.Layers["grid15"] as FeatureLayer;
			if (_lineStyleDlg == null) {
				_lineStyleDlg = new LineStyleDlg();
			}
			if (_lineStyleDlg.ShowDialog() == DialogResult.OK) {
				FeatureOverrideStyleModifier fsm = new FeatureOverrideStyleModifier(null, new MapInfo.Styles.CompositeStyle(_lineStyleDlg.LineStyle));
				_lyr.Modifiers.Append(fsm);
				this.mapControl1.Map.Zoom = new MapInfo.Geometry.Distance(25000, MapInfo.Geometry.DistanceUnit.Mile);
			}
		}

		private void button3_Click(object sender, System.EventArgs e) {
			LabelLayer _lyr = this.mapControl1.Map.Layers["worldlabels"] as LabelLayer;
			if (_textStyleDlg == null) {
				_textStyleDlg = new TextStyleDlg();
			}
			if (_textStyleDlg.ShowDialog() == DialogResult.OK) {
				_lyr.Sources["world"].DefaultLabelProperties.Style = new TextStyle(_textStyleDlg.FontStyle, _lyr.Sources["world"].DefaultLabelProperties.Style.CalloutLine);
				_lyr.Sources["worldcap"].DefaultLabelProperties.Style = new TextStyle(_textStyleDlg.FontStyle, _lyr.Sources["worldcap"].DefaultLabelProperties.Style.CalloutLine);
				_lyr.Sources["wldcty25"].DefaultLabelProperties.Style = new TextStyle(_textStyleDlg.FontStyle, _lyr.Sources["wldcty25"].DefaultLabelProperties.Style.CalloutLine);
				this.mapControl1.Map.Zoom = new MapInfo.Geometry.Distance(6250, MapInfo.Geometry.DistanceUnit.Mile);
			}
		}

		private void button4_Click(object sender, System.EventArgs e) {
			FeatureLayer _lyr = this.mapControl1.Map.Layers["worldcap"] as FeatureLayer;
			if (_symbolStyleDlg == null) {
				_symbolStyleDlg = new SymbolStyleDlg();
			}
			if (_symbolStyleDlg.ShowDialog() == DialogResult.OK) {
				FeatureOverrideStyleModifier fsm = new FeatureOverrideStyleModifier(null, new MapInfo.Styles.CompositeStyle(_symbolStyleDlg.SymbolStyle));
				_lyr.Modifiers.Append(fsm);
				this.mapControl1.Map.Zoom = new MapInfo.Geometry.Distance(6250, MapInfo.Geometry.DistanceUnit.Mile);
			}
		}

		private void button5_Click(object sender, System.EventArgs e) {
			// Get the layer we want
			FeatureLayer _lyr = this.mapControl1.Map.Layers["worldcap"] as FeatureLayer;

			//Create a sparse point style
			MapInfo.Styles.SimpleVectorPointStyle vs = new SimpleVectorPointStyle();

			//Just change the color and code and attributes flag to indicate that
			vs.Code = 55;
			vs.PointSize = 25;
			
			vs.Color = System.Drawing.Color.Red;
			// vs.Attributes = StyleAttributes.PointAttributes.Color | StyleAttributes.PointAttributes.VectorCode;
			
			

			// And apply to the layer
			FeatureOverrideStyleModifier fsm = new FeatureOverrideStyleModifier(null, new MapInfo.Styles.CompositeStyle(vs));
			_lyr.Modifiers.Append(fsm);
			this.mapControl1.Map.Zoom = new MapInfo.Geometry.Distance(6250, MapInfo.Geometry.DistanceUnit.Mile);
		}
	}
}
