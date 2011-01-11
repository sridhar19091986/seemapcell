using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using GALib;
//using Voronoi.Data;
//using Voronoi.Mathematics;
using Voronoi.Data;
using Voronoi.Mathematics;
using TemperatureVoronoi;
using System.Collections;

namespace TSPApp
{
	/// <summary>
	/// Summary description for TSPForm.
	/// </summary>
	struct Point
	{
		public int x ;
		public int y ;
	}
	public class TSPForm : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;
        private System.Collections.Generic.HashSet<Vector> points = new HashSet<Vector>();
        private List<TemperatureLocation> temps = new List<TemperatureLocation>();

		//protected System.Collections.ArrayList Points = new ArrayList();
		protected System.Collections.ArrayList Points = new ArrayList();
		private System.Windows.Forms.Panel ControlsPanel;
		private System.Windows.Forms.Button Randamize;
		private System.Windows.Forms.Label lbl_Generations;
		private System.Windows.Forms.Label lbl_pop;
		private System.Windows.Forms.Label lbl_Mutation;
		private System.Windows.Forms.NumericUpDown num_Gnr;
		private System.Windows.Forms.NumericUpDown num_PopSiz;
		private System.Windows.Forms.NumericUpDown num_Mutation;
		private System.Windows.Forms.Label lbl_CrossOver;
		private System.Windows.Forms.Label lbl_Points;
		private System.Windows.Forms.NumericUpDown num_Cities;
		private System.Windows.Forms.Button Clear;
		private System.Windows.Forms.Panel CitiesPanel;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button btn_Run;
		private System.Windows.Forms.ToolBarButton tb_BrwsBtn;
		private System.Windows.Forms.ToolBar Tool_MainBar;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.NumericUpDown numCO;
		private System.Windows.Forms.ToolBarButton tb_About;
		private System.Windows.Forms.ImageList tb_ImageList;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		protected System.Random RndObj  = new Random();
		protected GAChromosome m_chro	= null;

		public TSPForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TSPForm));
            this.Randamize = new System.Windows.Forms.Button();
            this.ControlsPanel = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btn_Run = new System.Windows.Forms.Button();
            this.Clear = new System.Windows.Forms.Button();
            this.num_Cities = new System.Windows.Forms.NumericUpDown();
            this.lbl_Points = new System.Windows.Forms.Label();
            this.numCO = new System.Windows.Forms.NumericUpDown();
            this.lbl_CrossOver = new System.Windows.Forms.Label();
            this.num_Mutation = new System.Windows.Forms.NumericUpDown();
            this.lbl_Mutation = new System.Windows.Forms.Label();
            this.lbl_pop = new System.Windows.Forms.Label();
            this.lbl_Generations = new System.Windows.Forms.Label();
            this.num_Gnr = new System.Windows.Forms.NumericUpDown();
            this.num_PopSiz = new System.Windows.Forms.NumericUpDown();
            this.CitiesPanel = new System.Windows.Forms.Panel();
            this.Tool_MainBar = new System.Windows.Forms.ToolBar();
            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.tb_BrwsBtn = new System.Windows.Forms.ToolBarButton();
            this.tb_About = new System.Windows.Forms.ToolBarButton();
            this.tb_ImageList = new System.Windows.Forms.ImageList(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ControlsPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_Cities)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Mutation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Gnr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_PopSiz)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Randamize
            // 
            this.Randamize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Randamize.Location = new System.Drawing.Point(19, 26);
            this.Randamize.Name = "Randamize";
            this.Randamize.Size = new System.Drawing.Size(115, 52);
            this.Randamize.TabIndex = 0;
            this.Randamize.Text = "Randamize";
            this.Randamize.Click += new System.EventHandler(this.Randamize_Click);
            // 
            // ControlsPanel
            // 
            this.ControlsPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ControlsPanel.Controls.Add(this.panel3);
            this.ControlsPanel.Controls.Add(this.num_Cities);
            this.ControlsPanel.Controls.Add(this.lbl_Points);
            this.ControlsPanel.Controls.Add(this.numCO);
            this.ControlsPanel.Controls.Add(this.lbl_CrossOver);
            this.ControlsPanel.Controls.Add(this.num_Mutation);
            this.ControlsPanel.Controls.Add(this.lbl_Mutation);
            this.ControlsPanel.Controls.Add(this.lbl_pop);
            this.ControlsPanel.Controls.Add(this.lbl_Generations);
            this.ControlsPanel.Controls.Add(this.num_Gnr);
            this.ControlsPanel.Controls.Add(this.num_PopSiz);
            this.ControlsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ControlsPanel.Location = new System.Drawing.Point(729, 0);
            this.ControlsPanel.Name = "ControlsPanel";
            this.ControlsPanel.Size = new System.Drawing.Size(153, 591);
            this.ControlsPanel.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btn_Run);
            this.panel3.Controls.Add(this.Clear);
            this.panel3.Controls.Add(this.Randamize);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 363);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(149, 224);
            this.panel3.TabIndex = 12;
            // 
            // btn_Run
            // 
            this.btn_Run.Location = new System.Drawing.Point(19, 164);
            this.btn_Run.Name = "btn_Run";
            this.btn_Run.Size = new System.Drawing.Size(115, 51);
            this.btn_Run.TabIndex = 13;
            this.btn_Run.Text = "Run";
            this.btn_Run.Click += new System.EventHandler(this.btn_Run_Click);
            // 
            // Clear
            // 
            this.Clear.Location = new System.Drawing.Point(19, 95);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(115, 51);
            this.Clear.TabIndex = 12;
            this.Clear.Text = "Clear";
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // num_Cities
            // 
            this.num_Cities.Location = new System.Drawing.Point(19, 95);
            this.num_Cities.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.num_Cities.Name = "num_Cities";
            this.num_Cities.Size = new System.Drawing.Size(115, 21);
            this.num_Cities.TabIndex = 11;
            this.num_Cities.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.num_Cities.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // lbl_Points
            // 
            this.lbl_Points.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Points.ForeColor = System.Drawing.Color.Black;
            this.lbl_Points.Location = new System.Drawing.Point(19, 69);
            this.lbl_Points.Name = "lbl_Points";
            this.lbl_Points.Size = new System.Drawing.Size(115, 25);
            this.lbl_Points.TabIndex = 10;
            this.lbl_Points.Text = "Cities";
            // 
            // numCO
            // 
            this.numCO.DecimalPlaces = 2;
            this.numCO.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numCO.Location = new System.Drawing.Point(19, 370);
            this.numCO.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCO.Name = "numCO";
            this.numCO.Size = new System.Drawing.Size(115, 21);
            this.numCO.TabIndex = 9;
            this.numCO.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numCO.Value = new decimal(new int[] {
            75,
            0,
            0,
            131072});
            // 
            // lbl_CrossOver
            // 
            this.lbl_CrossOver.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_CrossOver.ForeColor = System.Drawing.Color.Black;
            this.lbl_CrossOver.Location = new System.Drawing.Point(19, 345);
            this.lbl_CrossOver.Name = "lbl_CrossOver";
            this.lbl_CrossOver.Size = new System.Drawing.Size(87, 25);
            this.lbl_CrossOver.TabIndex = 8;
            this.lbl_CrossOver.Text = "Crossover";
            // 
            // num_Mutation
            // 
            this.num_Mutation.DecimalPlaces = 2;
            this.num_Mutation.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_Mutation.Location = new System.Drawing.Point(19, 310);
            this.num_Mutation.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_Mutation.Name = "num_Mutation";
            this.num_Mutation.Size = new System.Drawing.Size(115, 21);
            this.num_Mutation.TabIndex = 7;
            this.num_Mutation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.num_Mutation.Value = new decimal(new int[] {
            7,
            0,
            0,
            131072});
            // 
            // lbl_Mutation
            // 
            this.lbl_Mutation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Mutation.ForeColor = System.Drawing.Color.Black;
            this.lbl_Mutation.Location = new System.Drawing.Point(19, 276);
            this.lbl_Mutation.Name = "lbl_Mutation";
            this.lbl_Mutation.Size = new System.Drawing.Size(115, 26);
            this.lbl_Mutation.TabIndex = 6;
            this.lbl_Mutation.Text = "Pm";
            // 
            // lbl_pop
            // 
            this.lbl_pop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_pop.ForeColor = System.Drawing.Color.Black;
            this.lbl_pop.Location = new System.Drawing.Point(19, 198);
            this.lbl_pop.Name = "lbl_pop";
            this.lbl_pop.Size = new System.Drawing.Size(115, 26);
            this.lbl_pop.TabIndex = 4;
            this.lbl_pop.Text = "Population Size";
            // 
            // lbl_Generations
            // 
            this.lbl_Generations.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Generations.ForeColor = System.Drawing.Color.Black;
            this.lbl_Generations.Location = new System.Drawing.Point(19, 129);
            this.lbl_Generations.Name = "lbl_Generations";
            this.lbl_Generations.Size = new System.Drawing.Size(115, 26);
            this.lbl_Generations.TabIndex = 3;
            this.lbl_Generations.Text = "Generations";
            // 
            // num_Gnr
            // 
            this.num_Gnr.Location = new System.Drawing.Point(19, 164);
            this.num_Gnr.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.num_Gnr.Name = "num_Gnr";
            this.num_Gnr.Size = new System.Drawing.Size(115, 21);
            this.num_Gnr.TabIndex = 2;
            this.num_Gnr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.num_Gnr.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // num_PopSiz
            // 
            this.num_PopSiz.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.num_PopSiz.Location = new System.Drawing.Point(19, 233);
            this.num_PopSiz.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.num_PopSiz.Name = "num_PopSiz";
            this.num_PopSiz.Size = new System.Drawing.Size(115, 21);
            this.num_PopSiz.TabIndex = 5;
            this.num_PopSiz.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.num_PopSiz.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // CitiesPanel
            // 
            this.CitiesPanel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.CitiesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CitiesPanel.Location = new System.Drawing.Point(0, 0);
            this.CitiesPanel.Name = "CitiesPanel";
            this.CitiesPanel.Size = new System.Drawing.Size(729, 591);
            this.CitiesPanel.TabIndex = 2;
            this.CitiesPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.CitiesPanel_Paint);
            this.CitiesPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CitiesPanel_MouseDown);
            // 
            // Tool_MainBar
            // 
            this.Tool_MainBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Tool_MainBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButton1,
            this.tb_BrwsBtn,
            this.tb_About});
            this.Tool_MainBar.ButtonSize = new System.Drawing.Size(40, 40);
            this.Tool_MainBar.DropDownArrows = true;
            this.Tool_MainBar.ImageList = this.tb_ImageList;
            this.Tool_MainBar.Location = new System.Drawing.Point(0, 0);
            this.Tool_MainBar.Name = "Tool_MainBar";
            this.Tool_MainBar.ShowToolTips = true;
            this.Tool_MainBar.Size = new System.Drawing.Size(882, 53);
            this.Tool_MainBar.TabIndex = 4;
            this.Tool_MainBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.Tool_MainBar_ButtonClick);
            // 
            // toolBarButton1
            // 
            this.toolBarButton1.Name = "toolBarButton1";
            this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tb_BrwsBtn
            // 
            this.tb_BrwsBtn.ImageIndex = 1;
            this.tb_BrwsBtn.Name = "tb_BrwsBtn";
            this.tb_BrwsBtn.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.tb_BrwsBtn.ToolTipText = "log info";
            // 
            // tb_About
            // 
            this.tb_About.ImageIndex = 0;
            this.tb_About.Name = "tb_About";
            this.tb_About.ToolTipText = "About TSP";
            // 
            // tb_ImageList
            // 
            this.tb_ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("tb_ImageList.ImageStream")));
            this.tb_ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.tb_ImageList.Images.SetKeyName(0, "");
            this.tb_ImageList.Images.SetKeyName(1, "");
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.CitiesPanel);
            this.panel2.Controls.Add(this.ControlsPanel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(882, 591);
            this.panel2.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 566);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(729, 25);
            this.panel1.TabIndex = 3;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.Location = new System.Drawing.Point(0, 0);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(729, 25);
            this.progressBar1.TabIndex = 0;
            // 
            // openFileDialog
            // 
            this.openFileDialog.CheckFileExists = false;
            this.openFileDialog.DefaultExt = "txt";
            this.openFileDialog.FileName = "TspLog";
            this.openFileDialog.Filter = "Text files|*.txt";
            // 
            // TSPForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(882, 591);
            this.Controls.Add(this.Tool_MainBar);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TSPForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TSP";
            this.Load += new System.EventHandler(this.TSPForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TSPForm_Paint);
            this.ControlsPanel.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.num_Cities)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Mutation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Gnr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_PopSiz)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new TSPForm());

		}

		private void Randamize_Click(object sender, System.EventArgs e)
		{
			Clear_Click(null, null);
			System.Random RndObj = new Random();
			
			//Generating points										
			int iCities = int.Parse(this.num_Cities.Value.ToString());
			for (int i = 0; i < iCities; i++)
			{
				Point newPoint	=  new Point();
				newPoint.x		=  RndObj.Next(50,CitiesPanel.ClientRectangle.Right-50);
				newPoint.y		=  RndObj.Next(50,CitiesPanel.Bottom-50);
				Points.Add(newPoint);
			}
			DrawPoints();
		}

        private void DrawPoints()
        {
            System.Drawing.Graphics objGraph = this.CitiesPanel.CreateGraphics();

            for (int i = 0; i < Points.Count; i++)
            {
                PointF newPoint = new PointF(((Point)Points[i]).x, ((Point)Points[i]).y);
                //按照角度生成voronoi图+话务量分布图
                HashSet<PointF> hs_voronoiPoint = new HashSet<PointF>();
                hs_voronoiPoint.Add(AngleToPoint(newPoint, 360 - 30, 1));
                hs_voronoiPoint.Add(AngleToPoint(newPoint, 360 - 30 - 120, 1));
                hs_voronoiPoint.Add(AngleToPoint(newPoint, 360 - 30 - 240, 1));
                foreach (PointF voronoiPoint in hs_voronoiPoint)
                {
                    points.Add(new Vector(voronoiPoint.X, voronoiPoint.Y));
                    temps.Add(new TemperatureLocation(voronoiPoint.X, voronoiPoint.Y, i));
                }    
            }
            //话务量小区分割图
            VoronoiTemparature vt = new VoronoiTemparature(temps);
            vt.ColdColor = Color.Orange;
            vt.HotColor = Color.Red;
            Bitmap bmp = vt.GetMapTemperature(this.Width, this.Height);
            objGraph.DrawImage(bmp, new PointF(0, 0));
            //覆盖小区分割图
            bmp = Fortune.GetVoronoyMap(this.Width, this.Height, points);
            
            objGraph.DrawImage(bmp, new PointF(0, 0));

            for (int i = 0; i < Points.Count; i++)
            {
                PointF newPoint = new PointF(((Point)Points[i]).x, ((Point)Points[i]).y);
                objGraph.DrawEllipse(new System.Drawing.Pen(System.Drawing.Color.Black), newPoint.X, newPoint.Y, 6, 6);
                //按照角度生成  直线+箭头
                HashSet<PointF> hs_endPoint = new HashSet<PointF>();
                Pen p = new Pen(Color.Blue, 3);
                p.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                hs_endPoint.Add(AngleToPoint(newPoint, 360 - 30, 20));
                hs_endPoint.Add(AngleToPoint(newPoint, 360 - 30 - 120, 20));
                hs_endPoint.Add(AngleToPoint(newPoint, 360 - 30 - 240, 20));
                //小区覆盖方向角
                foreach (PointF endPoint in hs_endPoint)
                    objGraph.DrawLine(p, newPoint, endPoint);
                //写上标识
                objGraph.DrawString((i + 1).ToString(), 
                    new System.Drawing.Font("Arial", 10), System.Drawing.Brushes.Brown, newPoint.X + 8, newPoint.Y);
            }
        }

        //按照角度生成点
        private PointF AngleToPoint(PointF  center, int angle, int radius)
        {
            PointF c = new PointF();
            c.X = (int )(center.X + radius * Math.Cos(3.1415 * angle/180));
            c.Y  = (int)(center.Y + radius * Math.Sin(3.1415 * angle/180));
            return c;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="chromose"></param>
		private void Initializer(GAChromosome chromose)
		{
			bool []PointSel = new bool[Points.Count];
			bool bStop = false;
			do
			{
				int iSel = RndObj.Next(0,Points.Count);
				if (!PointSel[iSel])
				{
					PointSel[iSel] = true;
					chromose.AddGene(new GAGene(iSel.ToString()));

				}
				
				for(int i=0; i < Points.Count ;i++)
				{
					if (!PointSel[i])
						break;
					if (i == Points.Count -1)
						bStop = true;
				}
			}while(!bStop);
		}
		/// <summary>
		///  Based On 2opt Algo
		/// </summary>
		/// <param name="chromose"></param>
		private void ChromoseCompraror(GAChromosome chromosome)
		{
			
			double [,] NighborMatrix = new double[Points.Count , Points.Count]; 
			for (int i = 0; i < Points.Count ; i++)
				NighborMatrix[i,i] = -1; //distance diagonal
			
			//start filling the Connection Matrix with cities and distances between each other
			for (int i = 0 ; i < Points.Count - 1 ; i++)
			{
				int City1Index = int.Parse(((GAGene)chromosome[i]).Value);
				for (int j = i + 1; j < Points.Count ; j++)
				{
					int City2Index  = int.Parse(((GAGene)chromosome[j]).Value);
					double distance = Distance((Point)Points[City1Index],(Point)Points[City2Index]);
					NighborMatrix[City1Index, City2Index] = distance ;
					NighborMatrix[City2Index, City1Index] = distance ;
				}
			}
			
			GAChromosome newChromosome = new GAChromosome();
			//Start By Random Selection					
			int iCurrentSel		=  RndObj.Next(0, Points.Count);
			GAGene Gene			= (GAGene)chromosome[iCurrentSel];
			newChromosome.AddGene(new GAGene(Gene.Value));
			//left cities for visiting 
			int iLeftPoints		= Points.Count - 1;
			int iCurrentCitySel = int.Parse(Gene.Value);
			do
			{
				int iNearstNeighbor = GetNearstNeighbor(NighborMatrix, iCurrentCitySel);	
				newChromosome.AddGene(new GAGene(iNearstNeighbor.ToString()));
				iLeftPoints--;
				iCurrentCitySel = iNearstNeighbor;
						
			}while(iLeftPoints > 0);
			 chromosome.CopyChromosome(newChromosome);
		}
		private int GetNearstNeighbor(double [,] NighborMatrix, int iCurrentCitySel)	
		{
			int Index = 0 ;
			double distance = 1000000000;
			for (int i = 0; i < Points.Count ;i++) 
			{
				//not diagonal or selected element before
				if ( NighborMatrix[iCurrentCitySel,i]!= -1 
					&& distance > NighborMatrix[iCurrentCitySel,i])
				{
					distance = NighborMatrix[iCurrentCitySel,i];
					Index    = i;
				}
			}
			//Unavailable cells
			for (int i = 0; i < Points.Count ;i++) 
			{
				NighborMatrix[i , iCurrentCitySel] = -1;    //entire column of nearst city	no one can go to this city
				NighborMatrix[i , Index] = -1;				//entire column of nearst city	no one can go to this city
			}
			return Index;
		}
		
		/// <summary>
		/// Mutation is done By Swapping elements from Chromosome
		/// </summary>
		/// <param name="chromose"></param>
		private void Mutator(GAChromosome chromose)
		{
			System.Random rnd = new Random();
			
			int ChromoLen	= chromose.Capacity;
			
            int iSelection1 = rnd.Next(0, ChromoLen);
			int iSelection2 = iSelection1;
			
			while(iSelection2 == iSelection1) 
				iSelection2 = rnd.Next(0, ChromoLen);
			
			GAGene Gene1 = (GAGene)chromose[iSelection1];
			GAGene Gene2 = (GAGene)chromose[iSelection2];
			
			chromose.RemoveAt(iSelection1);
			chromose.Insert(iSelection1,Gene2);
			
			chromose.RemoveAt(iSelection2);
			chromose.Insert(iSelection1,Gene1);
 
		}
		private double Distance(Point CityA, Point CityB)
		{
			double xDiff = CityA.x - CityB.x ;
			double yDiff = CityA.y - CityB.y ;
			return (Math.Sqrt(Math.Pow(xDiff,2) + Math.Pow(yDiff,2)));
		}
 		/// <summary>
		/// 
		/// </summary>
		/// <param name="chromose"></param>
		private void Fitness(GAChromosome chromosome)
		{
			double fitness = 0;
			
			int CityIndex1 = 0;
			int CityIndex2 = 1;

			while (CityIndex2 < chromosome.GeneLength)
			{
				string strGene1 = ((GAGene)chromosome[CityIndex1]).Value ;
				int Pointindex1 = int.Parse(strGene1);
				string strGene2 = ((GAGene)chromosome[CityIndex2]).Value ;
				int PointIndex2 = int.Parse(strGene2);
				fitness += Distance((Point)Points[Pointindex1],(Point)Points[PointIndex2]);
				CityIndex1++;
				CityIndex2++;
			}
			//Fitness increases when distance decreases
			chromosome.Fitness = 1000/fitness;
		}
		/// <summary>
		/// Steps 
		///			1- locate any gene locus randomaly in dad
		///			2- locate this gene index in mum
		///			3- Start Copying new genes from dad direction right
		///			4- Start Copying new genes from mum direction left 
		///			5- When exit loop copy the rest of genes
		/// </summary>
		/// <param name="Dad"></param>
		/// <param name="Mum"></param>
		/// <param name="child1"></param>
		/// <param name="child2"></param>
		private void CrossOver
		(	
			  GAChromosome Dad
			, GAChromosome Mum
			, ref GAChromosome child1
			, ref GAChromosome child2
		)
		{
			GreedyCrossOver(Dad, Mum, ref child1);
			GreedyCrossOver(Mum, Dad, ref child2);
		}
		/// <summary>
		/// // 
		/// </summary>
		/// <param name="Dad"></param>
		/// <param name="Mum"></param>
		/// <param name="child1"></param>
		private void GreedyCrossOver
		(
			  GAChromosome Dad
			, GAChromosome Mum
			, ref GAChromosome child
		)
		{
			
			int length		= Dad.GeneLength;
			int MumIndex	= -1;
			int DadIndex	= RndObj.Next(0,length); 

			GAGene DadGene = (GAGene)Dad[DadIndex];
			MumIndex = Mum.HasThisGene(DadGene);
			
			if (MumIndex < 0 )
				throw new Exception("Gene not found in mum");
			
			child.Add(new GAGene(DadGene.Value));
			
			bool bDadAdded = true;
			bool bMumAdded = true;
			do
			{
				//As long as I can add from dad
				GAGene obMumGene = null;
				GAGene obDadGene = null;
				if (bDadAdded )
				{
					if(DadIndex > 0)
                        DadIndex = DadIndex - 1 ;
					else
						DadIndex = length - 1 ;
					
					obDadGene = (GAGene)Dad[DadIndex];
				}
				else
				{
					bDadAdded = false;
				}
				//As long as I can add from mum
				if (bMumAdded)
				{
					if(MumIndex < length-1)
						MumIndex  = MumIndex + 1 ;
					else
						MumIndex  = 0 ;
					obMumGene = (GAGene)Mum[MumIndex];	
				}
				else
				{
					bMumAdded = false;
				}
				if(bDadAdded  && child.HasThisGene(obDadGene)< 0)
				{
					//Add to head Dad gene
					child.Insert(0,obDadGene);
				}
				else
					bDadAdded = false;
				
				if (bMumAdded && child.HasThisGene(obMumGene) < 0)
				{
					//Add to Tail Mum gene
					child.AddGene(obMumGene);
				}
				else
					bMumAdded = false;

			}while(bDadAdded || bMumAdded)	;

			// Add rest of genes by Random Selection
			while (child.GeneLength < length)
			{
				bool bDone = false;
				do
				{
					int iRandom = this.RndObj.Next(0, length);
					if (child.HasThisGene(new GAGene(iRandom.ToString()))< 0)
					{
						child.Add(new GAGene(iRandom.ToString()));
						bDone = true;
					}
				}while(!bDone);
			}
		}

		private void Clear_Click(object sender, System.EventArgs e)
		{
			m_chro = null ;
			this.Refresh();
			Points.Clear();
            //新加的部分
            points.Clear();
            temps.Clear();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
		
		}

		private void TSPForm_Load(object sender, System.EventArgs e)
		{
		
		}

		private void Tool_MainBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			switch(Tool_MainBar.Buttons.IndexOf(e.Button))
			{
				case 1:AllowLogging();break;
				case 2:ShowAboutBox();break;
				default:break;
			}
		}
		private void AllowLogging()
		{
			if(Tool_MainBar.Buttons[1].Pushed)
				openFileDialog.ShowDialog();
		}
		private void ShowAboutBox()
		{
			About AboutBox  = new About();
			AboutBox.ShowDialog();
		}


		private void CitiesPanel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			System.Drawing.Graphics objGraph= CitiesPanel.CreateGraphics();
			objGraph.DrawEllipse(new System.Drawing.Pen( System.Drawing.Color.Black),e.X,e.Y, 6,6);	
			Point newPoint = new Point();
			
			newPoint.x	   =  e.X;
			newPoint.y	   =  e.Y;
			Points.Add(newPoint);
			objGraph.DrawString((Points.Count).ToString(), new System.Drawing.Font("Arial",10),System.Drawing.Brushes.Brown,newPoint.x + 8,newPoint.y );	
		}

		private void btn_Run_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (Points.Count < 2)
				{
					MessageBox.Show("No Enough cities");
					return;
				}
				this.m_chro = null;
				Refresh();
                //新加的部分
                points.Clear();
                temps.Clear();
				DrawPoints();
				GALib.Initializer newItializer = new GALib.Initializer(this.Initializer);
				GALib.Mutate	  mutater	   = new GALib.Mutate(this. ChromoseCompraror);	
				GALib.Fitness	  fitmethod	   = new GALib.Fitness(this.Fitness);	
				GALib.CrossOver	  CrossMethod  = new GALib.CrossOver(this.CrossOver);		
			
				GALib.GA GAAlgo			= new GA(newItializer,fitmethod,mutater,CrossMethod);
				GAAlgo.Generations		= long.Parse(this.num_Gnr.Value.ToString());
				GAAlgo.PopulationSize	= ushort.Parse(this.num_PopSiz.Value.ToString());
				GAAlgo.Mutation			= double.Parse(this.num_Mutation.Value.ToString());
				GAAlgo.CrossOver		= double.Parse(this.numCO.Value.ToString());
			
				if(Tool_MainBar.Buttons[1].Pushed)
				{
					GAAlgo.EnableLogging = true;
					GAAlgo.LogFilePath   = this.openFileDialog.FileName; 

				}
				
				GAAlgo.Initialize();

				while (!GAAlgo.IsDone())
					GAAlgo.CreateNextGeneration();
			
				m_chro = GAAlgo.GetBestChromosome();
				DrawCitiesPath();
			}
			catch(System.FormatException exp)
			{
				MessageBox.Show("Please check your Input Parameters "+exp);
			}
		}

		private void DrawCitiesPath()
		{
			if (m_chro == null)
				return;
			
			string [] citiesArray = m_chro.ToString().Split(',');
			
			System.Drawing.Graphics objGraph= CitiesPanel.CreateGraphics();
			
			for (int i = 1; i < citiesArray.Length ;i++)
			{
				int lPoint1 = int.Parse(citiesArray[i-1]);
				int lPoint2 = int.Parse(citiesArray[i]);
				System.Drawing.Point Point1 = new System.Drawing.Point(((Point)Points[lPoint1]).x,((Point)Points[lPoint1]).y);
				System.Drawing.Point Point2 = new System.Drawing.Point(((Point)Points[lPoint2]).x,((Point)Points[lPoint2]).y);
				objGraph.DrawLine( new System.Drawing.Pen( System.Drawing.Color.Navy), Point1,Point2 );	

			}

		}

		private void TSPForm_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			
		}

		private void CitiesPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
            if (m_chro == null)
                return;
            this.DrawPoints();
            DrawPoints();
			DrawCitiesPath();
		}
	}
}
