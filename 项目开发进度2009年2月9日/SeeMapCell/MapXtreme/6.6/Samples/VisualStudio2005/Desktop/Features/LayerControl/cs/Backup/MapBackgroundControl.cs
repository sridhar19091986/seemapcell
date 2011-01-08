using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using MapInfo.Windows.Controls; 

namespace MapInfo.Samples.LayerControl
{
	/// <summary>
	/// MapBackgroundControl  is a sample PropertiesUserControl.  
	/// PropertiesUserControl objects are special UserControls that are displayed
	/// on tabs in the LayerControl.  If you want to add your own custom
	/// tabs to LayerControl, you start by creating your own custom
	/// PropertiesUserControl classes.  (Tip: You can start by using 
	/// Visual Studio to create a UserControl class, and then later 
	/// modify the your class declaration so that instead of extending
	/// UserControl, your class extends PropertiesUserControl, as shown below.) 
	/// </summary>
	public class MapBackgroundControl : MapInfo.Windows.Controls.PropertiesUserControl
	{
		// Declare some private variables we will need later: 
		private Color _bgColor = Color.White; 
		private ColorDialog _colorDlg = null; 

		private System.Windows.Forms.Label labelBackground;
		private System.Windows.Forms.Panel panelSample;
		private System.Windows.Forms.Button buttonChoose;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		// Constructor that creates a MapBackgroundControl object. 
		public MapBackgroundControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Set the PropertiesUserControl.Category property,  which 
			// dictates what text will appear on the Tab.  
			this.Category = PropertiesCategory.Style; 
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// The Setup method initializes the control.  LayerControl will call this
		/// method automatically, whenever the user selects a node of the specified
		/// type in the layer tree.  Each node in the layer tree has a corresponding
		/// object in the MapXtreme 2004 object model; when the user select a node in
		/// the layer tree, that node's object is passed to the Setup method.  
		/// </summary>
		/// <param name="obj">For this class, we assume obj is a Map object</param>
		public override void Setup(Object obj) 
		{
			// the obj parameter should never be null, but just to be on the safe side:
			if (obj == null) 
			{
				return;
			}

			// If Layer Control has been set up correctly, obj should be a Map object.
			// Verify that obj is a Map, just to be on the safe side: 
			if (obj is MapInfo.Mapping.Map) 
			{
				// At this point, you could cast obj to an appropriate type of object.
				// For example, if you are writing a control to manage properties
				// of a FeatureLayer, you would cast obj to a FeatureLayer, like so:
				//
				//    _featureLayer = (FeatureLayer)obj; 
				//
				// You would then use the object to initialize the state of the control.
				//
				// But this control is for setting properties of a Map object, 
				// and Map object is a special case.  The PropertiesUserControl
				// class has a Map property which represents the Map object in use.
				// So we don't need to cast obj to a Map, since we already have a 
				// Map property.  
				Brush bgBrush = Map.BackgroundBrush; 
				if (bgBrush is SolidBrush) 
				{
					_bgColor = ((SolidBrush)bgBrush).Color; 
					panelSample.BackColor = _bgColor; 
				}
			}
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.labelBackground = new System.Windows.Forms.Label();
			this.panelSample = new System.Windows.Forms.Panel();
			this.buttonChoose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// labelBackground
			// 
			this.labelBackground.Location = new System.Drawing.Point(8, 8);
			this.labelBackground.Name = "labelBackground";
			this.labelBackground.Size = new System.Drawing.Size(160, 20);
			this.labelBackground.TabIndex = 0;
			this.labelBackground.Text = "Map Background Color:";
			this.labelBackground.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panelSample
			// 
			this.panelSample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelSample.Location = new System.Drawing.Point(8, 32);
			this.panelSample.Name = "panelSample";
			this.panelSample.Size = new System.Drawing.Size(112, 24);
			this.panelSample.TabIndex = 1;
			// 
			// buttonChoose
			// 
			this.buttonChoose.Location = new System.Drawing.Point(128, 32);
			this.buttonChoose.Name = "buttonChoose";
			this.buttonChoose.Size = new System.Drawing.Size(112, 24);
			this.buttonChoose.TabIndex = 2;
			this.buttonChoose.Text = "Choose C&olor...";
			this.buttonChoose.Click += new System.EventHandler(this.buttonChoose_Click);
			// 
			// MapBackgroundControl
			// 
			this.Controls.Add(this.buttonChoose);
			this.Controls.Add(this.panelSample);
			this.Controls.Add(this.labelBackground);
			this.Name = "MapBackgroundControl";
			this.Size = new System.Drawing.Size(256, 72);
			this.ResumeLayout(false);

		}
		#endregion

		// The method called when the user clicks the Choose Color button.
		private void buttonChoose_Click(object sender, System.EventArgs e)
		{
			if (_colorDlg == null) 
			{
				_colorDlg = new ColorDialog(); 
			}
			_colorDlg.Color = _bgColor; 
			if (_colorDlg.ShowDialog() == DialogResult.OK) 
			{
				// The user chose a color and clicked OK.  
				// Use the color as the map's background color. 
				_bgColor = _colorDlg.Color; 
				panelSample.BackColor = _bgColor; 
				this.Map.BackgroundBrush = new SolidBrush(_bgColor);
			}
		}
	}
}
