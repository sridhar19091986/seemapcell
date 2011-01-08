using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using MapInfo.Geometry;
using MapInfo.Engine;

namespace CoordSysCS
{
	/// <summary>
	/// Sample CoordSys application to demostrate how to use the CoordSys class.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		/// <summary>
		/// The MICommand variable is used to generate cursors against open tables.
		/// </summary>
		private System.Windows.Forms.Button ExitButton;
		private System.Windows.Forms.Button TestCoordSys;
		private System.Windows.Forms.RichTextBox outputTextBox;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.components = new System.ComponentModel.Container();
			Session.Current.CoordSysFactory.LoadDefaultProjectionFile();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			this.ExitButton = new System.Windows.Forms.Button();
			this.TestCoordSys = new System.Windows.Forms.Button();
			this.outputTextBox = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// ExitButton
			// 
			this.ExitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ExitButton.Location = new System.Drawing.Point(640, 400);
			this.ExitButton.Name = "ExitButton";
			this.ExitButton.Size = new System.Drawing.Size(88, 32);
			this.ExitButton.TabIndex = 0;
			this.ExitButton.Text = "Exit";
			this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
			// 
			// TestCoordSys
			// 
			this.TestCoordSys.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.TestCoordSys.Location = new System.Drawing.Point(504, 400);
			this.TestCoordSys.Name = "TestCoordSys";
			this.TestCoordSys.Size = new System.Drawing.Size(104, 32);
			this.TestCoordSys.TabIndex = 1;
			this.TestCoordSys.Text = "Test Coord Sys";
			this.TestCoordSys.Click += new System.EventHandler(this.TestCoordSys_Click);
			// 
			// outputTextBox
			// 
			this.outputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.outputTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.outputTextBox.Location = new System.Drawing.Point(8, 8);
			this.outputTextBox.Name = "outputTextBox";
			this.outputTextBox.Size = new System.Drawing.Size(720, 384);
			this.outputTextBox.TabIndex = 2;
			this.outputTextBox.Text = "";
			this.outputTextBox.WordWrap = false;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(744, 446);
			this.Controls.Add(this.outputTextBox);
			this.Controls.Add(this.TestCoordSys);
			this.Controls.Add(this.ExitButton);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void ExitButton_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void CreateCoordSys()
		{
			MapInfo.Geometry.CoordSys csys = Session.Current.CoordSysFactory.CreateFromPrjString("1, 56");
			this.outputTextBox.AppendText("Created CoordSys: " + csys.MapBasicString + "\n");
			csys = Session.Current.CoordSysFactory.CreateFromPrjString("\"test\\p45678\", 1, 56\"");
			this.outputTextBox.AppendText("Created CoordSys: " + csys.MapBasicString + "\n");
			csys = Session.Current.CoordSysFactory.CreateLongLat(MapInfo.Geometry.DatumID.AstroBeaconE);
			this.outputTextBox.AppendText("Created CoordSys: " + csys.MapBasicString + "\n");
			csys = Session.Current.CoordSysFactory.CreateFromPrjString("30, 47, 7, 103.853, 1.287639, 30000, 30000");
			this.outputTextBox.AppendText("Created CoordSys: " + csys.MapBasicString + "\n");
			csys = Session.Current.CoordSysFactory.CreateFromPrjString("30, 1000, 7, 13.62720367, 52.41864828, 40000, 10000");
			this.outputTextBox.AppendText("Created CoordSys: " + csys.MapBasicString + "\n");
			csys = Session.Current.CoordSysFactory.CreateFromMapBasicString(@"CoordSys Earth Projection 4, 62, ""m"", 0, 90, 90
");
			this.outputTextBox.AppendText("Created CoordSys: " + csys.MapBasicString + "\n");
		}

		/// <summary>
		/// We use the Math.Round method since none of the transforms are exact.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		private void ComparePoints(string msg, DPoint p1, DPoint p2)
		{
			int prec = 5;
			if (Math.Round(p1.x, prec) == Math.Round(p2.x, prec) &&
				Math.Round(p1.y, prec) == Math.Round(p2.y, prec))
			{
				outputTextBox.AppendText(msg + " points are equal: (" + Math.Round(p1.x, prec) + ", " + Math.Round(p1.y, prec) + ") == (" + Math.Round(p2.x, prec) + ", " + Math.Round(p2.y, prec) + ")\n");
			}
			else 
			{
				outputTextBox.AppendText(msg + " points are NOT equal: (" + Math.Round(p1.x, prec) + ", " + Math.Round(p1.y, prec) + ") != (" + Math.Round(p2.x, prec) + ", " + Math.Round(p2.y, prec) + ")\n");
			}
		}

		/// <summary>
		/// We are going to use the CoordSys transform to transform a point from one coordsys to another then back
		/// again to see if the round trip produces the correct results. Then test the array version of the transform.
		/// </summary>
		private void UseCoordinateTransform()
		{
			// create LongLat projection
			CoordSys csys = Session.Current.CoordSysFactory.CreateFromPrjString("1, 56");
			// Create Robinson projection
			CoordSys csys1 = Session.Current.CoordSysFactory.CreateFromPrjString("12, 62, 7, 0");
			if (csys == csys1)
			{
				outputTextBox.AppendText("Oops, coordsys's are equal, this is bad\n");
			}
			CoordinateTransform coordTransform = Session.Current.CoordSysFactory.CreateCoordinateTransform(csys, csys1);
			DPoint pntSrc = new DPoint(0, 0);
			DPoint pntDest = coordTransform.CoordSys1ToCoordSys2(pntSrc);
			DPoint pntSrc1 = new DPoint(1, 1);
			DPoint pntDest1 = coordTransform.CoordSys1ToCoordSys2(pntSrc1);
			DPoint pntBackToSrc = coordTransform.CoordSys2ToCoordSys1(pntDest1);
			ComparePoints("Round trip", pntSrc1, pntBackToSrc);
			// compare the result with arrays
			DPoint[] pnt = new DPoint[2];
			pnt[0].x = 0;
			pnt[0].y = 0;
			pnt[1].x = 1;
			pnt[1].y = 1;
			coordTransform.CoordSys1ToCoordSys2(pnt, out pnt);
			ComparePoints("Array converted", pntDest, pnt[0]);
			ComparePoints("Array converted", pntDest1, pnt[1]);
		}

		private void EnumCoordSysFactory()
		{
			CoordSysInfoEnumerator coordSysEnum = Session.Current.CoordSysFactory.GetCoordSysInfoEnumerator();

			coordSysEnum.Reset();
			outputTextBox.AppendText("Enumerate CoordSysFactory: \n");
			while (coordSysEnum.MoveNext()) 
			{
				outputTextBox.AppendText(" \t " + coordSysEnum.Current.CoordSys.MapBasicString + "\n");
			}
		}

		private void TestCoordSys_Click(object sender, System.EventArgs e)
		{
			CreateCoordSys();
			UseCoordinateTransform();
			EnumCoordSysFactory();
		}
	}
}
