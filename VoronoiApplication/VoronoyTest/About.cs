using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace TSPApp
{
	/// <summary>
	/// Summary description for About.
	/// </summary>
	public class About : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public About()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(About));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 32);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(192, 176);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(216, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(168, 40);
			this.label1.TabIndex = 1;
			this.label1.Text = "Travelling Sales Man Problem";
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button1.Location = new System.Drawing.Point(320, 232);
			this.button1.Name = "button1";
			this.button1.TabIndex = 2;
			this.button1.Text = "Close";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(216, 80);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(176, 32);
			this.label2.TabIndex = 3;
			this.label2.Text = "Author : Sameh Mohamed Samir";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(216, 120);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(176, 23);
			this.label3.TabIndex = 4;
			this.label3.Text = "Version : 1.0.0.0";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(216, 152);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(176, 23);
			this.label4.TabIndex = 5;
			this.label4.Text = "Arab Academy ";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(216, 184);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(176, 23);
			this.label5.TabIndex = 6;
			this.label5.Text = "Dr Mohamed Razkaa";
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(208, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(192, 216);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Location = new System.Drawing.Point(0, 8);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(208, 216);
			this.groupBox2.TabIndex = 8;
			this.groupBox2.TabStop = false;
			// 
			// About
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.button1;
			this.ClientSize = new System.Drawing.Size(408, 261);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(416, 288);
			this.MinimizeBox = false;
			this.Name = "About";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About Tsp";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
