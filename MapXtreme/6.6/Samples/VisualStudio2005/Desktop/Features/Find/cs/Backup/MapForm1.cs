using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace FindCS
{
	/// <summary>
	/// Summary description for MapForm1.
	/// </summary>
	public class MapForm1 : System.Windows.Forms.Form
	{
		private MapInfo.Windows.Controls.MapControl mapControl1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MapForm1(MapInfo.Mapping.Map map)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			mapControl1.Map = map;
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
			this.mapControl1 = new MapInfo.Windows.Controls.MapControl();
			this.SuspendLayout();
			// 
			// mapControl1
			// 
			this.mapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mapControl1.Location = new System.Drawing.Point(0, 0);
			this.mapControl1.Name = "mapControl1";
			this.mapControl1.Size = new System.Drawing.Size(292, 266);
			this.mapControl1.TabIndex = 0;
			this.mapControl1.Text = "mapControl1";
			// 
			// MapForm1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.mapControl1);
			this.Name = "MapForm1";
			this.Text = "MapForm1";
			this.Load += new System.EventHandler(this.MapForm1_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void MapForm1_Load(object sender, System.EventArgs e)
		{
			mapControl1.Tools.InfoTipsEnabled=true;
		}
	}
}
