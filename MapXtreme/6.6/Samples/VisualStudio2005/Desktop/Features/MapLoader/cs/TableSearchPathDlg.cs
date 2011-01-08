using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MapInfo.Samples.MapLoader
{
	/// <summary>
	/// Summary description for TableSearchPathDialog.
	/// </summary>
	public class TableSearchPathDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label labelPath;
		private System.Windows.Forms.TextBox textBoxTableSearchPath;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TableSearchPathDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.labelPath = new System.Windows.Forms.Label();
			this.textBoxTableSearchPath = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// labelPath
			// 
			this.labelPath.Location = new System.Drawing.Point(16, 8);
			this.labelPath.Name = "labelPath";
			this.labelPath.Size = new System.Drawing.Size(104, 16);
			this.labelPath.TabIndex = 0;
			this.labelPath.Text = "Table Search Path:";
			// 
			// textBoxTableSearchPath
			// 
			this.textBoxTableSearchPath.Location = new System.Drawing.Point(136, 8);
			this.textBoxTableSearchPath.Name = "textBoxTableSearchPath";
			this.textBoxTableSearchPath.Size = new System.Drawing.Size(312, 20);
			this.textBoxTableSearchPath.TabIndex = 1;
			this.textBoxTableSearchPath.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(192, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(168, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Separate Paths with \';\'";
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.Location = new System.Drawing.Point(143, 72);
			this.button1.Name = "button1";
			this.button1.TabIndex = 3;
			this.button1.Text = "OK";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Location = new System.Drawing.Point(263, 72);
			this.button2.Name = "button2";
			this.button2.TabIndex = 4;
			this.button2.Text = "Cancel";
			// 
			// TableSearchPathDialog
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.button2;
			this.ClientSize = new System.Drawing.Size(480, 110);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBoxTableSearchPath);
			this.Controls.Add(this.labelPath);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TableSearchPathDialog";
			this.ShowInTaskbar = false;
			this.Text = "Table Search Path";
			this.Load += new System.EventHandler(this.TableSearchPathDialog_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void TableSearchPathDialog_Load(object sender, System.EventArgs e)
		{
		
		}

		public string Path
		{
			get
			{
				return textBoxTableSearchPath.Text;
			}
			set
			{
				textBoxTableSearchPath.Text = value;
			}
		}
	}
}
