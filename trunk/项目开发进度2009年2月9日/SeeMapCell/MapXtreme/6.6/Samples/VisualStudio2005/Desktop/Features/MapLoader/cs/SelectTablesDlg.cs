using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MapInfo.Data;

namespace MapInfo.Samples.MapLoader
{
	/// <summary>
	/// Summary description for Dlg.
	/// </summary>
	public class SelectTablesDlg : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonAccept;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label labelSelectTables;
		private System.Windows.Forms.ListBox listBoxTables;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Creates a new SelectTablesDlg object.
		/// </summary>
		/// <remarks>
		/// This constructor is only useful during design time.
		/// </remarks>
		public SelectTablesDlg()
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
		/// Creates a new SelectTablesDlg object using a list of tables as input.
		/// </summary>
		/// <remarks>None.</remarks>
		/// <param name="tables">List of open tables.</param>
		public SelectTablesDlg(ITableEnumerator tables) {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Populate the table list
			// Add tables to the listbox
			foreach(Table tab in tables) {
				listBoxTables.Items.Add(tab);
			}
			// Set initial selection (first item in list)
			if (listBoxTables.Items.Count > 0) 
			{
				listBoxTables.SelectedIndex = 0;
			}
		}


		/// <summary>
		/// Gets an array of UITables that were selected by the user.
		/// </summary>
		/// <remarks>
		/// This value is only valid when the dialog returns with 
		/// result of DialogResult.OK.  Otherwise the returned value is null.
		/// </remarks>
		public Table[] SelectedTables {
			get {return _selectedTables;}
		}


		/// <summary>
		/// Gets the count of UITables that were selected by the user.
		/// </summary>
		/// <remarks>
		/// This value is only valid when the dialog returns with 
		/// result of DialogResult.OK.  Otherwise the returned value is zero.
		/// </remarks>
		public int SelectionCount {
			get {
				if (_selectedTables != null)
					return _selectedTables.Length;
				else
					return 0;
			}
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SelectTablesDlg));
			this.buttonAccept = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.listBoxTables = new System.Windows.Forms.ListBox();
			this.labelSelectTables = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// buttonAccept
			// 
			this.buttonAccept.AccessibleDescription = resources.GetString("buttonAccept.AccessibleDescription");
			this.buttonAccept.AccessibleName = resources.GetString("buttonAccept.AccessibleName");
			this.buttonAccept.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("buttonAccept.Anchor")));
			this.buttonAccept.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonAccept.BackgroundImage")));
			this.buttonAccept.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("buttonAccept.Dock")));
			this.buttonAccept.Enabled = ((bool)(resources.GetObject("buttonAccept.Enabled")));
			this.buttonAccept.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("buttonAccept.FlatStyle")));
			this.buttonAccept.Font = ((System.Drawing.Font)(resources.GetObject("buttonAccept.Font")));
			this.buttonAccept.Image = ((System.Drawing.Image)(resources.GetObject("buttonAccept.Image")));
			this.buttonAccept.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("buttonAccept.ImageAlign")));
			this.buttonAccept.ImageIndex = ((int)(resources.GetObject("buttonAccept.ImageIndex")));
			this.buttonAccept.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("buttonAccept.ImeMode")));
			this.buttonAccept.Location = ((System.Drawing.Point)(resources.GetObject("buttonAccept.Location")));
			this.buttonAccept.Name = "buttonAccept";
			this.buttonAccept.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("buttonAccept.RightToLeft")));
			this.buttonAccept.Size = ((System.Drawing.Size)(resources.GetObject("buttonAccept.Size")));
			this.buttonAccept.TabIndex = ((int)(resources.GetObject("buttonAccept.TabIndex")));
			this.buttonAccept.Text = resources.GetString("buttonAccept.Text");
			this.buttonAccept.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("buttonAccept.TextAlign")));
			this.buttonAccept.Visible = ((bool)(resources.GetObject("buttonAccept.Visible")));
			this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.AccessibleDescription = resources.GetString("buttonCancel.AccessibleDescription");
			this.buttonCancel.AccessibleName = resources.GetString("buttonCancel.AccessibleName");
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("buttonCancel.Anchor")));
			this.buttonCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonCancel.BackgroundImage")));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("buttonCancel.Dock")));
			this.buttonCancel.Enabled = ((bool)(resources.GetObject("buttonCancel.Enabled")));
			this.buttonCancel.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("buttonCancel.FlatStyle")));
			this.buttonCancel.Font = ((System.Drawing.Font)(resources.GetObject("buttonCancel.Font")));
			this.buttonCancel.Image = ((System.Drawing.Image)(resources.GetObject("buttonCancel.Image")));
			this.buttonCancel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("buttonCancel.ImageAlign")));
			this.buttonCancel.ImageIndex = ((int)(resources.GetObject("buttonCancel.ImageIndex")));
			this.buttonCancel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("buttonCancel.ImeMode")));
			this.buttonCancel.Location = ((System.Drawing.Point)(resources.GetObject("buttonCancel.Location")));
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("buttonCancel.RightToLeft")));
			this.buttonCancel.Size = ((System.Drawing.Size)(resources.GetObject("buttonCancel.Size")));
			this.buttonCancel.TabIndex = ((int)(resources.GetObject("buttonCancel.TabIndex")));
			this.buttonCancel.Text = resources.GetString("buttonCancel.Text");
			this.buttonCancel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("buttonCancel.TextAlign")));
			this.buttonCancel.Visible = ((bool)(resources.GetObject("buttonCancel.Visible")));
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// listBoxTables
			// 
			this.listBoxTables.AccessibleDescription = resources.GetString("listBoxTables.AccessibleDescription");
			this.listBoxTables.AccessibleName = resources.GetString("listBoxTables.AccessibleName");
			this.listBoxTables.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("listBoxTables.Anchor")));
			this.listBoxTables.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("listBoxTables.BackgroundImage")));
			this.listBoxTables.ColumnWidth = ((int)(resources.GetObject("listBoxTables.ColumnWidth")));
			this.listBoxTables.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("listBoxTables.Dock")));
			this.listBoxTables.Enabled = ((bool)(resources.GetObject("listBoxTables.Enabled")));
			this.listBoxTables.Font = ((System.Drawing.Font)(resources.GetObject("listBoxTables.Font")));
			this.listBoxTables.HorizontalExtent = ((int)(resources.GetObject("listBoxTables.HorizontalExtent")));
			this.listBoxTables.HorizontalScrollbar = ((bool)(resources.GetObject("listBoxTables.HorizontalScrollbar")));
			this.listBoxTables.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("listBoxTables.ImeMode")));
			this.listBoxTables.IntegralHeight = ((bool)(resources.GetObject("listBoxTables.IntegralHeight")));
			this.listBoxTables.ItemHeight = ((int)(resources.GetObject("listBoxTables.ItemHeight")));
			this.listBoxTables.Location = ((System.Drawing.Point)(resources.GetObject("listBoxTables.Location")));
			this.listBoxTables.Name = "listBoxTables";
			this.listBoxTables.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("listBoxTables.RightToLeft")));
			this.listBoxTables.ScrollAlwaysVisible = ((bool)(resources.GetObject("listBoxTables.ScrollAlwaysVisible")));
			this.listBoxTables.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBoxTables.Size = ((System.Drawing.Size)(resources.GetObject("listBoxTables.Size")));
			this.listBoxTables.TabIndex = ((int)(resources.GetObject("listBoxTables.TabIndex")));
			this.listBoxTables.Visible = ((bool)(resources.GetObject("listBoxTables.Visible")));
			this.listBoxTables.DoubleClick += new System.EventHandler(this.listBoxTables_DoubleClick);
			// 
			// labelSelectTables
			// 
			this.labelSelectTables.AccessibleDescription = resources.GetString("labelSelectTables.AccessibleDescription");
			this.labelSelectTables.AccessibleName = resources.GetString("labelSelectTables.AccessibleName");
			this.labelSelectTables.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("labelSelectTables.Anchor")));
			this.labelSelectTables.AutoSize = ((bool)(resources.GetObject("labelSelectTables.AutoSize")));
			this.labelSelectTables.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("labelSelectTables.Dock")));
			this.labelSelectTables.Enabled = ((bool)(resources.GetObject("labelSelectTables.Enabled")));
			this.labelSelectTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelSelectTables.Font = ((System.Drawing.Font)(resources.GetObject("labelSelectTables.Font")));
			this.labelSelectTables.Image = ((System.Drawing.Image)(resources.GetObject("labelSelectTables.Image")));
			this.labelSelectTables.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("labelSelectTables.ImageAlign")));
			this.labelSelectTables.ImageIndex = ((int)(resources.GetObject("labelSelectTables.ImageIndex")));
			this.labelSelectTables.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("labelSelectTables.ImeMode")));
			this.labelSelectTables.Location = ((System.Drawing.Point)(resources.GetObject("labelSelectTables.Location")));
			this.labelSelectTables.Name = "labelSelectTables";
			this.labelSelectTables.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("labelSelectTables.RightToLeft")));
			this.labelSelectTables.Size = ((System.Drawing.Size)(resources.GetObject("labelSelectTables.Size")));
			this.labelSelectTables.TabIndex = ((int)(resources.GetObject("labelSelectTables.TabIndex")));
			this.labelSelectTables.Text = resources.GetString("labelSelectTables.Text");
			this.labelSelectTables.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("labelSelectTables.TextAlign")));
			this.labelSelectTables.Visible = ((bool)(resources.GetObject("labelSelectTables.Visible")));
			this.labelSelectTables.Click += new System.EventHandler(this.labelSelectTables_Click);
			// 
			// SelectTablesDlg
			// 
			this.AcceptButton = this.buttonAccept;
			this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
			this.AccessibleName = resources.GetString("$this.AccessibleName");
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.CancelButton = this.buttonCancel;
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.Controls.Add(this.listBoxTables);
			this.Controls.Add(this.labelSelectTables);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonAccept);
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
			this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
			this.Name = "SelectTablesDlg";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");
			this.Load += new System.EventHandler(this.SelectTablesDlg_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonAccept_Click(object sender, System.EventArgs e) {
			// Construct the selected table list
			if (listBoxTables.SelectedItems.Count > 0) {
				_selectedTables = new Table[listBoxTables.SelectedItems.Count];
				listBoxTables.SelectedItems.CopyTo(_selectedTables, 0);
			}
			// Set return value and close the dialog
			DialogResult = DialogResult.OK;
			this.Close();
		}

		private void buttonCancel_Click(object sender, System.EventArgs e) {
			// Set return value and close the dialog
			DialogResult = DialogResult.Cancel;
			this.Close();
		}

		// List of selected tables
		private Table[] _selectedTables = null;

		// Handles double-click event in the table listbox
		private void listBoxTables_DoubleClick(object sender, System.EventArgs e) {
			// Dismiss the dialog by clicking the accept button.
			buttonAccept.PerformClick();
		}

		private void labelSelectTables_Click(object sender, System.EventArgs e)
		{
		
		}

		private void SelectTablesDlg_Load(object sender, System.EventArgs e)
		{
		
		}
	}
}
