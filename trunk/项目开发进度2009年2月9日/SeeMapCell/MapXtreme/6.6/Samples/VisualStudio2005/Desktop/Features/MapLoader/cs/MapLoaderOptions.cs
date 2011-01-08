using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MapInfo.Samples.MapLoader
{
	/// <summary>
	/// Summary description for TableLoaderOptions.
	/// </summary>
	public class MapLoaderOptions : System.Windows.Forms.Form
	{
		private System.Windows.Forms.CheckBox cbEnableLayers;
		private System.Windows.Forms.CheckBox cbAutoPosition;
		private System.Windows.Forms.Label labelStartPosition;
		private System.Windows.Forms.TextBox textBoxStartPosition;
		private System.Windows.Forms.CheckBox cbClearMap;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox cbLayersOnly;
		private System.Windows.Forms.CheckBox cbSetMapName;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MapLoaderOptions()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			cbEnableLayers.Checked = true;
			cbAutoPosition.Checked = true;
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cbEnableLayers = new System.Windows.Forms.CheckBox();
			this.cbAutoPosition = new System.Windows.Forms.CheckBox();
			this.labelStartPosition = new System.Windows.Forms.Label();
			this.textBoxStartPosition = new System.Windows.Forms.TextBox();
			this.cbClearMap = new System.Windows.Forms.CheckBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.cbLayersOnly = new System.Windows.Forms.CheckBox();
			this.cbSetMapName = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.SuspendLayout();
			// 
			// cbEnableLayers
			// 
			this.cbEnableLayers.Location = new System.Drawing.Point(24, 32);
			this.cbEnableLayers.Name = "cbEnableLayers";
			this.cbEnableLayers.TabIndex = 0;
			this.cbEnableLayers.Text = "Enable Layers";
			// 
			// cbAutoPosition
			// 
			this.cbAutoPosition.Location = new System.Drawing.Point(24, 56);
			this.cbAutoPosition.Name = "cbAutoPosition";
			this.cbAutoPosition.TabIndex = 1;
			this.cbAutoPosition.Text = "Auto Position";
			// 
			// labelStartPosition
			// 
			this.labelStartPosition.Location = new System.Drawing.Point(24, 80);
			this.labelStartPosition.Name = "labelStartPosition";
			this.labelStartPosition.Size = new System.Drawing.Size(104, 23);
			this.labelStartPosition.TabIndex = 2;
			this.labelStartPosition.Text = "Start Position:";
			// 
			// textBoxStartPosition
			// 
			this.textBoxStartPosition.Location = new System.Drawing.Point(136, 72);
			this.textBoxStartPosition.Name = "textBoxStartPosition";
			this.textBoxStartPosition.Size = new System.Drawing.Size(32, 20);
			this.textBoxStartPosition.TabIndex = 3;
			this.textBoxStartPosition.Text = "1";
			this.textBoxStartPosition.WordWrap = false;
			// 
			// cbClearMap
			// 
			this.cbClearMap.Location = new System.Drawing.Point(16, 192);
			this.cbClearMap.Name = "cbClearMap";
			this.cbClearMap.TabIndex = 4;
			this.cbClearMap.Text = "Clear Map First";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(208, 16);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 5;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(208, 48);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			// 
			// cbLayersOnly
			// 
			this.cbLayersOnly.Location = new System.Drawing.Point(24, 128);
			this.cbLayersOnly.Name = "cbLayersOnly";
			this.cbLayersOnly.TabIndex = 4;
			this.cbLayersOnly.Text = "Layers Only";
			// 
			// cbSetMapName
			// 
			this.cbSetMapName.Location = new System.Drawing.Point(24, 152);
			this.cbSetMapName.Name = "cbSetMapName";
			this.cbSetMapName.TabIndex = 4;
			this.cbSetMapName.Text = "Set Map Name";
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(192, 96);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Map Loader Options";
			// 
			// groupBox2
			// 
			this.groupBox2.Location = new System.Drawing.Point(8, 112);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(192, 72);
			this.groupBox2.TabIndex = 8;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Geoset && Workspace Only";
			// 
			// MapLoaderOptions
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(288, 222);
			this.ControlBox = false;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.cbClearMap);
			this.Controls.Add(this.textBoxStartPosition);
			this.Controls.Add(this.labelStartPosition);
			this.Controls.Add(this.cbAutoPosition);
			this.Controls.Add(this.cbEnableLayers);
			this.Controls.Add(this.cbLayersOnly);
			this.Controls.Add(this.cbSetMapName);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MapLoaderOptions";
			this.Text = "Map Loader Options";
			this.ResumeLayout(false);

		}
		#endregion

		public bool EnableLayers
		{
			get
			{
				return cbEnableLayers.Checked;
			}
			set
			{
				cbEnableLayers.Checked = value;
			}
		}
		public bool AutoPosition
		{
			get
			{
				return cbAutoPosition.Checked;
			}
			set
			{
				cbAutoPosition.Checked = value;
			}
		}
		public bool ClearMapFirst
		{
			get
			{
				return cbClearMap.Checked;
			}
			set
			{
				cbClearMap.Checked = value;
			}
		}

		public new int StartPosition
		{
			get
			{
				return int.Parse(textBoxStartPosition.Text);
			}
			set
			{
				textBoxStartPosition.Text = value.ToString();
			}
		}

		public bool LayersOnly
		{
			get
			{
				return cbLayersOnly.Checked;
			}
			set
			{
				cbLayersOnly.Checked = value;
			}
		}

		public bool SetMapName
		{
			get
			{
				return cbSetMapName.Checked;
			}
			set
			{
				cbSetMapName.Checked = value;
			}
		}

	}
}
