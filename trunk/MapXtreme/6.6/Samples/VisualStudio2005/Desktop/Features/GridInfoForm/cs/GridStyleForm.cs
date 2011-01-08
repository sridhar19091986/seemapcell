using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace GridForm
{
	/// <summary>
	/// Summary description for GridStyleForm.
	/// </summary>
	public class GridStyleForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label Label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lable1111;
		private System.Windows.Forms.Button ColorButton;
		private System.Windows.Forms.Button OKbutton;
		private System.Windows.Forms.Button Cancelbutton;
		
		public System.Windows.Forms.TextBox AlphaTextBox;
		public System.Windows.Forms.TextBox BrightnessBox;
		public System.Windows.Forms.TextBox ContrastBox;
		public System.Windows.Forms.CheckBox GrayScale;
		public System.Windows.Forms.CheckBox Transparency;
		public System.Windows.Forms.CheckBox DisplayHillshade;
		public System.Windows.Forms.Label TransparentColor;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GridStyleForm()
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
			this.label1 = new System.Windows.Forms.Label();
			this.AlphaTextBox = new System.Windows.Forms.TextBox();
			this.Label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.BrightnessBox = new System.Windows.Forms.TextBox();
			this.ContrastBox = new System.Windows.Forms.TextBox();
			this.GrayScale = new System.Windows.Forms.CheckBox();
			this.Transparency = new System.Windows.Forms.CheckBox();
			this.DisplayHillshade = new System.Windows.Forms.CheckBox();
			this.lable1111 = new System.Windows.Forms.Label();
			this.TransparentColor = new System.Windows.Forms.Label();
			this.ColorButton = new System.Windows.Forms.Button();
			this.OKbutton = new System.Windows.Forms.Button();
			this.Cancelbutton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(77, 28);
			this.label1.TabIndex = 0;
			this.label1.Text = "Alpha:";
			// 
			// AlphaTextBox
			// 
			this.AlphaTextBox.Location = new System.Drawing.Point(134, 18);
			this.AlphaTextBox.Name = "AlphaTextBox";
			this.AlphaTextBox.Size = new System.Drawing.Size(76, 22);
			this.AlphaTextBox.TabIndex = 1;
			this.AlphaTextBox.Text = "textBox1";
			// 
			// Label3
			// 
			this.Label3.Location = new System.Drawing.Point(10, 55);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(124, 27);
			this.Label3.TabIndex = 4;
			this.Label3.Text = "Brightness (0-100):";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(10, 92);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(120, 27);
			this.label4.TabIndex = 5;
			this.label4.Text = "Contrast (0-100):";
			// 
			// BrightnessBox
			// 
			this.BrightnessBox.Location = new System.Drawing.Point(134, 55);
			this.BrightnessBox.Name = "BrightnessBox";
			this.BrightnessBox.Size = new System.Drawing.Size(76, 22);
			this.BrightnessBox.TabIndex = 6;
			this.BrightnessBox.Text = "textBox1";
			// 
			// ContrastBox
			// 
			this.ContrastBox.Location = new System.Drawing.Point(134, 92);
			this.ContrastBox.Name = "ContrastBox";
			this.ContrastBox.Size = new System.Drawing.Size(76, 22);
			this.ContrastBox.TabIndex = 7;
			this.ContrastBox.Text = "textBox1";
			// 
			// GrayScale
			// 
			this.GrayScale.Location = new System.Drawing.Point(19, 129);
			this.GrayScale.Name = "GrayScale";
			this.GrayScale.Size = new System.Drawing.Size(96, 28);
			this.GrayScale.TabIndex = 8;
			this.GrayScale.Text = "Grayscale";
			// 
			// Transparency
			// 
			this.Transparency.Location = new System.Drawing.Point(144, 129);
			this.Transparency.Name = "Transparency";
			this.Transparency.Size = new System.Drawing.Size(125, 28);
			this.Transparency.TabIndex = 9;
			this.Transparency.Text = "Transparency";
			// 
			// DisplayHillshade
			// 
			this.DisplayHillshade.Location = new System.Drawing.Point(19, 157);
			this.DisplayHillshade.Name = "DisplayHillshade";
			this.DisplayHillshade.Size = new System.Drawing.Size(135, 28);
			this.DisplayHillshade.TabIndex = 10;
			this.DisplayHillshade.Text = "DisplayHillshade ";
			// 
			// lable1111
			// 
			this.lable1111.Location = new System.Drawing.Point(19, 194);
			this.lable1111.Name = "lable1111";
			this.lable1111.Size = new System.Drawing.Size(120, 26);
			this.lable1111.TabIndex = 12;
			this.lable1111.Text = "TransparentColor ";
			// 
			// TransparentColor
			// 
			this.TransparentColor.Location = new System.Drawing.Point(134, 194);
			this.TransparentColor.Name = "TransparentColor";
			this.TransparentColor.Size = new System.Drawing.Size(77, 26);
			this.TransparentColor.TabIndex = 13;
			// 
			// ColorButton
			// 
			this.ColorButton.Location = new System.Drawing.Point(221, 194);
			this.ColorButton.Name = "ColorButton";
			this.ColorButton.Size = new System.Drawing.Size(29, 26);
			this.ColorButton.TabIndex = 14;
			this.ColorButton.Text = "...";
			this.ColorButton.Click += new System.EventHandler(this.ColorButton_Click);
			// 
			// OKbutton
			// 
			this.OKbutton.Location = new System.Drawing.Point(67, 249);
			this.OKbutton.Name = "OKbutton";
			this.OKbutton.Size = new System.Drawing.Size(90, 27);
			this.OKbutton.TabIndex = 15;
			this.OKbutton.Text = "OK";
			this.OKbutton.Click += new System.EventHandler(this.OKbutton_Click);
			// 
			// Cancelbutton
			// 
			this.Cancelbutton.Location = new System.Drawing.Point(182, 249);
			this.Cancelbutton.Name = "Cancelbutton";
			this.Cancelbutton.Size = new System.Drawing.Size(90, 27);
			this.Cancelbutton.TabIndex = 16;
			this.Cancelbutton.Text = "Cancel";
			this.Cancelbutton.Click += new System.EventHandler(this.Cancelbutton_Click);
			// 
			// GridStyleForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(350, 306);
			this.Controls.Add(this.Cancelbutton);
			this.Controls.Add(this.OKbutton);
			this.Controls.Add(this.ColorButton);
			this.Controls.Add(this.TransparentColor);
			this.Controls.Add(this.lable1111);
			this.Controls.Add(this.DisplayHillshade);
			this.Controls.Add(this.Transparency);
			this.Controls.Add(this.GrayScale);
			this.Controls.Add(this.ContrastBox);
			this.Controls.Add(this.BrightnessBox);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.Label3);
			this.Controls.Add(this.AlphaTextBox);
			this.Controls.Add(this.label1);
			this.Name = "GridStyleForm";
			this.Text = "GridStyleForm";
			this.ResumeLayout(false);

		}
		#endregion

		private void ColorButton_Click(object sender, System.EventArgs e)
		{
			ColorDialog dialog = new ColorDialog();
			dialog.Color = TransparentColor.BackColor;
			dialog.ShowHelp = true;
			dialog.AllowFullOpen = true;

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				TransparentColor.BackColor = dialog.Color;
			}
		}

		private void OKbutton_Click(object sender, System.EventArgs e)
		{
			Close();
			DialogResult = DialogResult.OK;
		}

		private void Cancelbutton_Click(object sender, System.EventArgs e)
		{
			Close();	
			DialogResult = DialogResult.Cancel;
		}
	}
}
