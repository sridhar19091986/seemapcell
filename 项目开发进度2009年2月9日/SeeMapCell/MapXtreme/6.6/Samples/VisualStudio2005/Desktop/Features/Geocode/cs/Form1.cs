using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Data;
//MapInfo references
using MapInfo.Geocoding;

namespace MapInfo.Samples.GeocodeSampleCS
{
	/// <summary>
	/// Sample to demonstate Geocoding and Geocode Constraints.
	/// </summary>
	public class FormGeocodeSample : System.Windows.Forms.Form
	{
		private MapInfo.Windows.Controls.GroupBox gbServerInfo;
		private MapInfo.Windows.Controls.RadioButton rbMMServer;
		private MapInfo.Windows.Controls.RadioButton rbMiAware;
		private MapInfo.Windows.Controls.Label lblMMUrl;
		private MapInfo.Windows.Controls.Label lblMiAwareURl;
		private MapInfo.Windows.Controls.TextBox tbMMUrl;
		private MapInfo.Windows.Controls.TextBox tbMiAwareUrl;
		private MapInfo.Windows.Controls.GroupBox gbAddress;
		private MapInfo.Windows.Controls.TextBox tbStreet;
		private MapInfo.Windows.Controls.Label lblStreet;
		private MapInfo.Windows.Controls.TextBox tbCity;
		private MapInfo.Windows.Controls.Label lblCity;
		private MapInfo.Windows.Controls.TextBox tbState;
		private MapInfo.Windows.Controls.Label lblState;
		private MapInfo.Windows.Controls.TextBox tbPostalCode;
		private MapInfo.Windows.Controls.Label lblPostalCode;
		private MapInfo.Windows.Controls.GroupBox gbConstraints;
		private MapInfo.Windows.Controls.CheckBox cbCloseMatchesOnly;
		private MapInfo.Windows.Controls.CheckBox cbFallbackPostal;
		private MapInfo.Windows.Controls.CheckBox cbFallbackGeographic;
		private MapInfo.Windows.Controls.GroupBox gbMustMatch;
		private MapInfo.Windows.Controls.CheckBox cbMatchAddrNumber;
		private MapInfo.Windows.Controls.CheckBox cbMatchMainAddr;
		private MapInfo.Windows.Controls.CheckBox cbMatchInput;
		private MapInfo.Windows.Controls.CheckBox cbMatchPostal;
		private MapInfo.Windows.Controls.CheckBox cbMatchState;
		private MapInfo.Windows.Controls.CheckBox cbMatchCity;
		private MapInfo.Windows.Controls.GroupBox gbResults;
		private MapInfo.Windows.Controls.RichTextBox rtbResults;
		private MapInfo.Windows.Controls.Button btnGeocode;
		private MapInfo.Windows.Controls.Button btnExit;
		private MapInfo.Windows.Controls.Button btnConnect;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		// the geocode client
		private IGeocodeClient _geocodeClient = null;
		private MapInfo.Windows.Controls.TextBox tbCountry;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown maxCandidates;
		private System.Windows.Forms.NumericUpDown maxRanges;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown maxRangeUnits;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox streetOffset;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox streetOffsetUnits;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox cornerOffset;
		private System.Windows.Forms.ComboBox cornerOffsetUnits;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox geocodeType;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox dictionaryUsage;
		private System.Windows.Forms.TextBox tbPlace;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.CheckBox cbMatchCountry;
		private MapInfo.Windows.Controls.Label lblCountry;
	
		public FormGeocodeSample()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
      // These seem to get wiped out if they are placed in InitializeComponent() and changes are made
      // in DevStudio designer mode.
      this.streetOffsetUnits.Items.Add(MapInfo.Geometry.CoordSys.DistanceUnitKeyword(MapInfo.Geometry.DistanceUnit.Foot));
		  this.streetOffsetUnits.Items.Add(MapInfo.Geometry.CoordSys.DistanceUnitKeyword(MapInfo.Geometry.DistanceUnit.Mile));
		  this.streetOffsetUnits.Items.Add(MapInfo.Geometry.CoordSys.DistanceUnitKeyword(MapInfo.Geometry.DistanceUnit.Meter));
		  this.streetOffsetUnits.Items.Add(MapInfo.Geometry.CoordSys.DistanceUnitKeyword(MapInfo.Geometry.DistanceUnit.Kilometer));
		  this.cornerOffsetUnits.Items.Add(MapInfo.Geometry.CoordSys.DistanceUnitKeyword(MapInfo.Geometry.DistanceUnit.Foot));
		  this.cornerOffsetUnits.Items.Add(MapInfo.Geometry.CoordSys.DistanceUnitKeyword(MapInfo.Geometry.DistanceUnit.Mile));
		  this.cornerOffsetUnits.Items.Add(MapInfo.Geometry.CoordSys.DistanceUnitKeyword(MapInfo.Geometry.DistanceUnit.Meter));
		  this.cornerOffsetUnits.Items.Add(MapInfo.Geometry.CoordSys.DistanceUnitKeyword(MapInfo.Geometry.DistanceUnit.Kilometer));
      this.geocodeType.Items.Add(GeocodeType.MATCH_ADDRESS.ToString());
		  this.geocodeType.Items.Add(GeocodeType.MATCH_POSTCODE.ToString());
		  this.geocodeType.Items.Add(GeocodeType.MATCH_POI.ToString());
		  this.geocodeType.Items.Add(GeocodeType.MATCH_CITIES.ToString());
		  this.geocodeType.Items.Add(GeocodeType.MATCH_GEOGRAPHIC.ToString());
		  this.geocodeType.Items.Add(GeocodeType.MATCH_STANDARDIZE_ONLY.ToString());
		  this.geocodeType.Items.Add(GeocodeType.MATCH_BROWSE.ToString());
      this.dictionaryUsage.Items.Add(DictionaryUsage.AD_AND_UD.ToString());
		  this.dictionaryUsage.Items.Add(DictionaryUsage.AD_ONLY.ToString());
		  this.dictionaryUsage.Items.Add(DictionaryUsage.PREFER_AD.ToString());
		  this.dictionaryUsage.Items.Add(DictionaryUsage.PREFER_UD.ToString());
		  this.dictionaryUsage.Items.Add(DictionaryUsage.UD_ONLY.ToString());
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
			this.gbServerInfo = new MapInfo.Windows.Controls.GroupBox();
			this.tbMiAwareUrl = new MapInfo.Windows.Controls.TextBox();
			this.tbMMUrl = new MapInfo.Windows.Controls.TextBox();
			this.lblMiAwareURl = new MapInfo.Windows.Controls.Label();
			this.lblMMUrl = new MapInfo.Windows.Controls.Label();
			this.rbMiAware = new MapInfo.Windows.Controls.RadioButton();
			this.rbMMServer = new MapInfo.Windows.Controls.RadioButton();
			this.gbAddress = new MapInfo.Windows.Controls.GroupBox();
			this.label10 = new System.Windows.Forms.Label();
			this.tbPlace = new System.Windows.Forms.TextBox();
			this.tbCountry = new MapInfo.Windows.Controls.TextBox();
			this.lblCountry = new MapInfo.Windows.Controls.Label();
			this.tbPostalCode = new MapInfo.Windows.Controls.TextBox();
			this.lblPostalCode = new MapInfo.Windows.Controls.Label();
			this.tbState = new MapInfo.Windows.Controls.TextBox();
			this.lblState = new MapInfo.Windows.Controls.Label();
			this.tbCity = new MapInfo.Windows.Controls.TextBox();
			this.lblCity = new MapInfo.Windows.Controls.Label();
			this.tbStreet = new MapInfo.Windows.Controls.TextBox();
			this.lblStreet = new MapInfo.Windows.Controls.Label();
			this.gbConstraints = new MapInfo.Windows.Controls.GroupBox();
			this.dictionaryUsage = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.geocodeType = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.cornerOffsetUnits = new System.Windows.Forms.ComboBox();
			this.cornerOffset = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.streetOffsetUnits = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.streetOffset = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.maxRangeUnits = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.maxRanges = new System.Windows.Forms.NumericUpDown();
			this.maxCandidates = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.gbMustMatch = new MapInfo.Windows.Controls.GroupBox();
			this.cbMatchCountry = new System.Windows.Forms.CheckBox();
			this.cbMatchCity = new MapInfo.Windows.Controls.CheckBox();
			this.cbMatchState = new MapInfo.Windows.Controls.CheckBox();
			this.cbMatchPostal = new MapInfo.Windows.Controls.CheckBox();
			this.cbMatchInput = new MapInfo.Windows.Controls.CheckBox();
			this.cbMatchMainAddr = new MapInfo.Windows.Controls.CheckBox();
			this.cbMatchAddrNumber = new MapInfo.Windows.Controls.CheckBox();
			this.cbFallbackGeographic = new MapInfo.Windows.Controls.CheckBox();
			this.cbFallbackPostal = new MapInfo.Windows.Controls.CheckBox();
			this.cbCloseMatchesOnly = new MapInfo.Windows.Controls.CheckBox();
			this.gbResults = new MapInfo.Windows.Controls.GroupBox();
			this.rtbResults = new MapInfo.Windows.Controls.RichTextBox();
			this.btnGeocode = new MapInfo.Windows.Controls.Button();
			this.btnExit = new MapInfo.Windows.Controls.Button();
			this.btnConnect = new MapInfo.Windows.Controls.Button();
			this.gbServerInfo.SuspendLayout();
			this.gbAddress.SuspendLayout();
			this.gbConstraints.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.maxRangeUnits)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.maxRanges)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.maxCandidates)).BeginInit();
			this.gbMustMatch.SuspendLayout();
			this.gbResults.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbServerInfo
			// 
			this.gbServerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.gbServerInfo.Controls.Add(this.tbMiAwareUrl);
			this.gbServerInfo.Controls.Add(this.tbMMUrl);
			this.gbServerInfo.Controls.Add(this.lblMiAwareURl);
			this.gbServerInfo.Controls.Add(this.lblMMUrl);
			this.gbServerInfo.Controls.Add(this.rbMiAware);
			this.gbServerInfo.Controls.Add(this.rbMMServer);
			this.gbServerInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.gbServerInfo.Location = new System.Drawing.Point(16, 8);
			this.gbServerInfo.Name = "gbServerInfo";
			this.gbServerInfo.Size = new System.Drawing.Size(560, 88);
			this.gbServerInfo.TabIndex = 0;
			this.gbServerInfo.TabStop = false;
			this.gbServerInfo.Text = "Server Information:";
			// 
			// tbMiAwareUrl
			// 
			this.tbMiAwareUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tbMiAwareUrl.Location = new System.Drawing.Point(168, 16);
			this.tbMiAwareUrl.Name = "tbMiAwareUrl";
			this.tbMiAwareUrl.Size = new System.Drawing.Size(384, 20);
			this.tbMiAwareUrl.TabIndex = 60;
			this.tbMiAwareUrl.Text = "http://ats-envinsag:8066/LocationUtility/services/LocationUtility";
			this.tbMiAwareUrl.TextChanged += new System.EventHandler(this.rbUrlChanged);
			// 
			// tbMMUrl
			// 
			this.tbMMUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tbMMUrl.Enabled = false;
			this.tbMMUrl.Location = new System.Drawing.Point(168, 56);
			this.tbMMUrl.Name = "tbMMUrl";
			this.tbMMUrl.Size = new System.Drawing.Size(384, 20);
			this.tbMMUrl.TabIndex = 30;
			this.tbMMUrl.Text = "http://ats-vader:8097/mapmarker40/servlet/mapmarker";
			this.tbMMUrl.TextChanged += new System.EventHandler(this.rbUrlChanged);
			// 
			// lblMiAwareURl
			// 
			this.lblMiAwareURl.Location = new System.Drawing.Point(136, 16);
			this.lblMiAwareURl.Name = "lblMiAwareURl";
			this.lblMiAwareURl.Size = new System.Drawing.Size(32, 20);
			this.lblMiAwareURl.TabIndex = 50;
			this.lblMiAwareURl.Text = "URL:";
			this.lblMiAwareURl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblMMUrl
			// 
			this.lblMMUrl.Location = new System.Drawing.Point(136, 56);
			this.lblMMUrl.Name = "lblMMUrl";
			this.lblMMUrl.Size = new System.Drawing.Size(32, 20);
			this.lblMMUrl.TabIndex = 20;
			this.lblMMUrl.Text = "URL:";
			this.lblMMUrl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// rbMiAware
			// 
			this.rbMiAware.Checked = true;
			this.rbMiAware.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbMiAware.Location = new System.Drawing.Point(8, 24);
			this.rbMiAware.Name = "rbMiAware";
			this.rbMiAware.Size = new System.Drawing.Size(104, 16);
			this.rbMiAware.TabIndex = 40;
			this.rbMiAware.TabStop = true;
			this.rbMiAware.Text = "miAware Service";
			this.rbMiAware.CheckedChanged += new System.EventHandler(this.rbMiAware_CheckedChanged);
			// 
			// rbMMServer
			// 
			this.rbMMServer.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rbMMServer.Location = new System.Drawing.Point(8, 56);
			this.rbMMServer.Name = "rbMMServer";
			this.rbMMServer.Size = new System.Drawing.Size(112, 16);
			this.rbMMServer.TabIndex = 10;
			this.rbMMServer.Text = "MapMarker Server";
			this.rbMMServer.CheckedChanged += new System.EventHandler(this.rbMMServer_CheckedChanged);
			// 
			// gbAddress
			// 
			this.gbAddress.Controls.Add(this.label10);
			this.gbAddress.Controls.Add(this.tbPlace);
			this.gbAddress.Controls.Add(this.tbCountry);
			this.gbAddress.Controls.Add(this.lblCountry);
			this.gbAddress.Controls.Add(this.tbPostalCode);
			this.gbAddress.Controls.Add(this.lblPostalCode);
			this.gbAddress.Controls.Add(this.tbState);
			this.gbAddress.Controls.Add(this.lblState);
			this.gbAddress.Controls.Add(this.tbCity);
			this.gbAddress.Controls.Add(this.lblCity);
			this.gbAddress.Controls.Add(this.tbStreet);
			this.gbAddress.Controls.Add(this.lblStreet);
			this.gbAddress.Enabled = false;
			this.gbAddress.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.gbAddress.Location = new System.Drawing.Point(16, 104);
			this.gbAddress.Name = "gbAddress";
			this.gbAddress.Size = new System.Drawing.Size(432, 152);
			this.gbAddress.TabIndex = 70;
			this.gbAddress.TabStop = false;
			this.gbAddress.Text = "Input Address:";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(16, 24);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(56, 32);
			this.label10.TabIndex = 174;
			this.label10.Text = "Place Name";
			// 
			// tbPlace
			// 
			this.tbPlace.Location = new System.Drawing.Point(80, 24);
			this.tbPlace.Name = "tbPlace";
			this.tbPlace.Size = new System.Drawing.Size(344, 20);
			this.tbPlace.TabIndex = 173;
			this.tbPlace.Text = "MapInfo";
			// 
			// tbCountry
			// 
			this.tbCountry.Location = new System.Drawing.Point(312, 120);
			this.tbCountry.Name = "tbCountry";
			this.tbCountry.Size = new System.Drawing.Size(40, 20);
			this.tbCountry.TabIndex = 172;
			this.tbCountry.Text = "USA";
			// 
			// lblCountry
			// 
			this.lblCountry.Location = new System.Drawing.Point(256, 120);
			this.lblCountry.Name = "lblCountry";
			this.lblCountry.Size = new System.Drawing.Size(48, 16);
			this.lblCountry.TabIndex = 171;
			this.lblCountry.Text = "Country:";
			// 
			// tbPostalCode
			// 
			this.tbPostalCode.Location = new System.Drawing.Point(80, 120);
			this.tbPostalCode.Name = "tbPostalCode";
			this.tbPostalCode.Size = new System.Drawing.Size(160, 20);
			this.tbPostalCode.TabIndex = 170;
			this.tbPostalCode.Text = "12180";
			// 
			// lblPostalCode
			// 
			this.lblPostalCode.Location = new System.Drawing.Point(16, 112);
			this.lblPostalCode.Name = "lblPostalCode";
			this.lblPostalCode.Size = new System.Drawing.Size(56, 32);
			this.lblPostalCode.TabIndex = 160;
			this.lblPostalCode.Text = "Postal Code:";
			// 
			// tbState
			// 
			this.tbState.Location = new System.Drawing.Point(312, 88);
			this.tbState.Name = "tbState";
			this.tbState.Size = new System.Drawing.Size(88, 20);
			this.tbState.TabIndex = 150;
			this.tbState.Text = "NY";
			// 
			// lblState
			// 
			this.lblState.Location = new System.Drawing.Point(256, 88);
			this.lblState.Name = "lblState";
			this.lblState.Size = new System.Drawing.Size(40, 16);
			this.lblState.TabIndex = 140;
			this.lblState.Text = "State:";
			// 
			// tbCity
			// 
			this.tbCity.Location = new System.Drawing.Point(80, 88);
			this.tbCity.Name = "tbCity";
			this.tbCity.Size = new System.Drawing.Size(160, 20);
			this.tbCity.TabIndex = 130;
			this.tbCity.Text = "Troy";
			// 
			// lblCity
			// 
			this.lblCity.Location = new System.Drawing.Point(16, 88);
			this.lblCity.Name = "lblCity";
			this.lblCity.Size = new System.Drawing.Size(48, 16);
			this.lblCity.TabIndex = 120;
			this.lblCity.Text = "City:";
			// 
			// tbStreet
			// 
			this.tbStreet.Location = new System.Drawing.Point(80, 56);
			this.tbStreet.Name = "tbStreet";
			this.tbStreet.Size = new System.Drawing.Size(344, 20);
			this.tbStreet.TabIndex = 110;
			this.tbStreet.Text = "1 Global View";
			// 
			// lblStreet
			// 
			this.lblStreet.Location = new System.Drawing.Point(16, 64);
			this.lblStreet.Name = "lblStreet";
			this.lblStreet.Size = new System.Drawing.Size(48, 16);
			this.lblStreet.TabIndex = 100;
			this.lblStreet.Text = "Street:";
			// 
			// gbConstraints
			// 
			this.gbConstraints.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.gbConstraints.Controls.Add(this.dictionaryUsage);
			this.gbConstraints.Controls.Add(this.label9);
			this.gbConstraints.Controls.Add(this.geocodeType);
			this.gbConstraints.Controls.Add(this.label8);
			this.gbConstraints.Controls.Add(this.label7);
			this.gbConstraints.Controls.Add(this.cornerOffsetUnits);
			this.gbConstraints.Controls.Add(this.cornerOffset);
			this.gbConstraints.Controls.Add(this.label6);
			this.gbConstraints.Controls.Add(this.streetOffsetUnits);
			this.gbConstraints.Controls.Add(this.label5);
			this.gbConstraints.Controls.Add(this.streetOffset);
			this.gbConstraints.Controls.Add(this.label4);
			this.gbConstraints.Controls.Add(this.label3);
			this.gbConstraints.Controls.Add(this.maxRangeUnits);
			this.gbConstraints.Controls.Add(this.label2);
			this.gbConstraints.Controls.Add(this.maxRanges);
			this.gbConstraints.Controls.Add(this.maxCandidates);
			this.gbConstraints.Controls.Add(this.label1);
			this.gbConstraints.Controls.Add(this.gbMustMatch);
			this.gbConstraints.Controls.Add(this.cbFallbackGeographic);
			this.gbConstraints.Controls.Add(this.cbFallbackPostal);
			this.gbConstraints.Controls.Add(this.cbCloseMatchesOnly);
			this.gbConstraints.Enabled = false;
			this.gbConstraints.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.gbConstraints.Location = new System.Drawing.Point(16, 264);
			this.gbConstraints.Name = "gbConstraints";
			this.gbConstraints.Size = new System.Drawing.Size(560, 216);
			this.gbConstraints.TabIndex = 190;
			this.gbConstraints.TabStop = false;
			this.gbConstraints.Text = "Geocode Constraints:";
			// 
			// dictionaryUsage
			// 
			this.dictionaryUsage.Location = new System.Drawing.Point(296, 144);
			this.dictionaryUsage.Name = "dictionaryUsage";
			this.dictionaryUsage.Size = new System.Drawing.Size(128, 21);
			this.dictionaryUsage.TabIndex = 250;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(200, 144);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(104, 16);
			this.label9.TabIndex = 249;
			this.label9.Text = "Dictionary Usage";
			// 
			// geocodeType
			// 
			this.geocodeType.Location = new System.Drawing.Point(296, 104);
			this.geocodeType.Name = "geocodeType";
			this.geocodeType.Size = new System.Drawing.Size(128, 21);
			this.geocodeType.TabIndex = 248;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(200, 104);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(88, 16);
			this.label8.TabIndex = 247;
			this.label8.Text = "Geocode Type:";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(392, 64);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(32, 23);
			this.label7.TabIndex = 246;
			this.label7.Text = "Units";
			// 
			// cornerOffsetUnits
			// 
			this.cornerOffsetUnits.Location = new System.Drawing.Point(336, 64);
			this.cornerOffsetUnits.Name = "cornerOffsetUnits";
			this.cornerOffsetUnits.Size = new System.Drawing.Size(56, 21);
			this.cornerOffsetUnits.TabIndex = 245;
			// 
			// cornerOffset
			// 
			this.cornerOffset.Location = new System.Drawing.Point(280, 64);
			this.cornerOffset.Name = "cornerOffset";
			this.cornerOffset.Size = new System.Drawing.Size(48, 20);
			this.cornerOffset.TabIndex = 244;
			this.cornerOffset.Text = "25";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(200, 64);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(80, 23);
			this.label6.TabIndex = 243;
			this.label6.Text = "Corner Offset:";
			// 
			// streetOffsetUnits
			// 
			this.streetOffsetUnits.Location = new System.Drawing.Point(336, 24);
			this.streetOffsetUnits.Name = "streetOffsetUnits";
			this.streetOffsetUnits.Size = new System.Drawing.Size(56, 21);
			this.streetOffsetUnits.TabIndex = 242;
			this.streetOffsetUnits.SelectedIndexChanged += new System.EventHandler(this.streetOffsetUnits_SelectedIndexChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(392, 24);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(32, 16);
			this.label5.TabIndex = 241;
			this.label5.Text = "Units";
			// 
			// streetOffset
			// 
			this.streetOffset.Location = new System.Drawing.Point(280, 24);
			this.streetOffset.Name = "streetOffset";
			this.streetOffset.Size = new System.Drawing.Size(48, 20);
			this.streetOffset.TabIndex = 239;
			this.streetOffset.Text = "25";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(200, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 16);
			this.label4.TabIndex = 238;
			this.label4.Text = "Street Offset: ";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 160);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 16);
			this.label3.TabIndex = 237;
			this.label3.Text = "Max Range Units:";
			// 
			// maxRangeUnits
			// 
			this.maxRangeUnits.Location = new System.Drawing.Point(120, 158);
			this.maxRangeUnits.Minimum = new System.Decimal(new int[] {
																																	1,
																																	0,
																																	0,
																																	-2147483648});
			this.maxRangeUnits.Name = "maxRangeUnits";
			this.maxRangeUnits.Size = new System.Drawing.Size(48, 20);
			this.maxRangeUnits.TabIndex = 236;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 128);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 16);
			this.label2.TabIndex = 235;
			this.label2.Text = "Max Ranges:";
			// 
			// maxRanges
			// 
			this.maxRanges.Location = new System.Drawing.Point(120, 128);
			this.maxRanges.Minimum = new System.Decimal(new int[] {
																															1,
																															0,
																															0,
																															-2147483648});
			this.maxRanges.Name = "maxRanges";
			this.maxRanges.Size = new System.Drawing.Size(48, 20);
			this.maxRanges.TabIndex = 234;
			// 
			// maxCandidates
			// 
			this.maxCandidates.Location = new System.Drawing.Point(120, 96);
			this.maxCandidates.Minimum = new System.Decimal(new int[] {
																																	1,
																																	0,
																																	0,
																																	-2147483648});
			this.maxCandidates.Name = "maxCandidates";
			this.maxCandidates.Size = new System.Drawing.Size(48, 20);
			this.maxCandidates.TabIndex = 233;
			this.maxCandidates.Value = new System.Decimal(new int[] {
																																1,
																																0,
																																0,
																																0});
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 98);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 232;
			this.label1.Text = "Max Candidates:";
			this.label1.Click += new System.EventHandler(this.label1_Click);
			// 
			// gbMustMatch
			// 
			this.gbMustMatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.gbMustMatch.Controls.Add(this.cbMatchCountry);
			this.gbMustMatch.Controls.Add(this.cbMatchCity);
			this.gbMustMatch.Controls.Add(this.cbMatchState);
			this.gbMustMatch.Controls.Add(this.cbMatchPostal);
			this.gbMustMatch.Controls.Add(this.cbMatchInput);
			this.gbMustMatch.Controls.Add(this.cbMatchMainAddr);
			this.gbMustMatch.Controls.Add(this.cbMatchAddrNumber);
			this.gbMustMatch.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.gbMustMatch.Location = new System.Drawing.Point(432, 16);
			this.gbMustMatch.Name = "gbMustMatch";
			this.gbMustMatch.Size = new System.Drawing.Size(120, 192);
			this.gbMustMatch.TabIndex = 230;
			this.gbMustMatch.TabStop = false;
			this.gbMustMatch.Text = "Must Match On:";
			// 
			// cbMatchCountry
			// 
			this.cbMatchCountry.Location = new System.Drawing.Point(8, 160);
			this.cbMatchCountry.Name = "cbMatchCountry";
			this.cbMatchCountry.TabIndex = 291;
			this.cbMatchCountry.Text = "Country";
			// 
			// cbMatchCity
			// 
			this.cbMatchCity.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cbMatchCity.Location = new System.Drawing.Point(8, 88);
			this.cbMatchCity.Name = "cbMatchCity";
			this.cbMatchCity.Size = new System.Drawing.Size(104, 16);
			this.cbMatchCity.TabIndex = 270;
			this.cbMatchCity.Text = "City";
			// 
			// cbMatchState
			// 
			this.cbMatchState.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cbMatchState.Location = new System.Drawing.Point(8, 112);
			this.cbMatchState.Name = "cbMatchState";
			this.cbMatchState.Size = new System.Drawing.Size(104, 16);
			this.cbMatchState.TabIndex = 280;
			this.cbMatchState.Text = "State";
			// 
			// cbMatchPostal
			// 
			this.cbMatchPostal.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cbMatchPostal.Location = new System.Drawing.Point(8, 136);
			this.cbMatchPostal.Name = "cbMatchPostal";
			this.cbMatchPostal.Size = new System.Drawing.Size(104, 16);
			this.cbMatchPostal.TabIndex = 290;
			this.cbMatchPostal.Text = "Postal Code";
			// 
			// cbMatchInput
			// 
			this.cbMatchInput.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cbMatchInput.Location = new System.Drawing.Point(8, 64);
			this.cbMatchInput.Name = "cbMatchInput";
			this.cbMatchInput.Size = new System.Drawing.Size(104, 16);
			this.cbMatchInput.TabIndex = 260;
			this.cbMatchInput.Text = "Input";
			// 
			// cbMatchMainAddr
			// 
			this.cbMatchMainAddr.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cbMatchMainAddr.Location = new System.Drawing.Point(8, 40);
			this.cbMatchMainAddr.Name = "cbMatchMainAddr";
			this.cbMatchMainAddr.Size = new System.Drawing.Size(104, 16);
			this.cbMatchMainAddr.TabIndex = 250;
			this.cbMatchMainAddr.Text = "Main Address";
			// 
			// cbMatchAddrNumber
			// 
			this.cbMatchAddrNumber.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cbMatchAddrNumber.Location = new System.Drawing.Point(8, 16);
			this.cbMatchAddrNumber.Name = "cbMatchAddrNumber";
			this.cbMatchAddrNumber.Size = new System.Drawing.Size(104, 16);
			this.cbMatchAddrNumber.TabIndex = 240;
			this.cbMatchAddrNumber.Text = "Address Number";
			// 
			// cbFallbackGeographic
			// 
			this.cbFallbackGeographic.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cbFallbackGeographic.Location = new System.Drawing.Point(16, 70);
			this.cbFallbackGeographic.Name = "cbFallbackGeographic";
			this.cbFallbackGeographic.Size = new System.Drawing.Size(176, 16);
			this.cbFallbackGeographic.TabIndex = 220;
			this.cbFallbackGeographic.Text = "Fallback to Geographic Centroid";
			// 
			// cbFallbackPostal
			// 
			this.cbFallbackPostal.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cbFallbackPostal.Location = new System.Drawing.Point(16, 47);
			this.cbFallbackPostal.Name = "cbFallbackPostal";
			this.cbFallbackPostal.Size = new System.Drawing.Size(160, 16);
			this.cbFallbackPostal.TabIndex = 210;
			this.cbFallbackPostal.Text = "Fallback to Postal Centroid";
			// 
			// cbCloseMatchesOnly
			// 
			this.cbCloseMatchesOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cbCloseMatchesOnly.Location = new System.Drawing.Point(16, 24);
			this.cbCloseMatchesOnly.Name = "cbCloseMatchesOnly";
			this.cbCloseMatchesOnly.Size = new System.Drawing.Size(136, 16);
			this.cbCloseMatchesOnly.TabIndex = 200;
			this.cbCloseMatchesOnly.Text = "Close Matches Only";
			// 
			// gbResults
			// 
			this.gbResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.gbResults.Controls.Add(this.rtbResults);
			this.gbResults.Enabled = false;
			this.gbResults.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.gbResults.Location = new System.Drawing.Point(16, 488);
			this.gbResults.Name = "gbResults";
			this.gbResults.Size = new System.Drawing.Size(552, 136);
			this.gbResults.TabIndex = 32;
			this.gbResults.TabStop = false;
			this.gbResults.Text = "Results:";
			// 
			// rtbResults
			// 
			this.rtbResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.rtbResults.Location = new System.Drawing.Point(8, 16);
			this.rtbResults.Name = "rtbResults";
			this.rtbResults.Size = new System.Drawing.Size(536, 112);
			this.rtbResults.TabIndex = 330;
			this.rtbResults.Text = "";
			// 
			// btnGeocode
			// 
			this.btnGeocode.Enabled = false;
			this.btnGeocode.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnGeocode.Location = new System.Drawing.Point(464, 144);
			this.btnGeocode.Name = "btnGeocode";
			this.btnGeocode.Size = new System.Drawing.Size(80, 32);
			this.btnGeocode.TabIndex = 300;
			this.btnGeocode.Text = "Geocode";
			this.btnGeocode.Click += new System.EventHandler(this.btnGeocode_Click);
			// 
			// btnExit
			// 
			this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnExit.Location = new System.Drawing.Point(464, 184);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(80, 32);
			this.btnExit.TabIndex = 310;
			this.btnExit.Text = "Exit";
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// btnConnect
			// 
			this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnConnect.Location = new System.Drawing.Point(464, 104);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(80, 32);
			this.btnConnect.TabIndex = 311;
			this.btnConnect.Text = "Connect to Server";
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// FormGeocodeSample
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(584, 630);
			this.Controls.Add(this.btnConnect);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.btnGeocode);
			this.Controls.Add(this.gbResults);
			this.Controls.Add(this.gbConstraints);
			this.Controls.Add(this.gbAddress);
			this.Controls.Add(this.gbServerInfo);
			this.Name = "FormGeocodeSample";
			this.Text = "Geocode Sample";
			this.gbServerInfo.ResumeLayout(false);
			this.gbAddress.ResumeLayout(false);
			this.gbConstraints.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.maxRangeUnits)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.maxRanges)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.maxCandidates)).EndInit();
			this.gbMustMatch.ResumeLayout(false);
			this.gbResults.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new FormGeocodeSample());
		}
		
		#region Control Event Handlers
		private void btnConnect_Click(object sender, System.EventArgs e)
		{
      try 
      {
			  // create the appropriate geocode client
			  if (rbMMServer.Checked)
				  _geocodeClient = GeocodeClientFactory.GetMmjHttpClient(tbMMUrl.Text);
			  else
				  _geocodeClient = GeocodeClientFactory.GetEnvinsaLocationUtilityService(tbMiAwareUrl.Text);

			  SetDefaultConstraints();
        gbAddress.Enabled = true;
        gbConstraints.Enabled = true;
        gbResults.Enabled = true;
        btnGeocode.Enabled = true;
      }  
      catch (Exception exception)
      {
      	System.Windows.Forms.MessageBox.Show("Exception geocoding: " + exception.Message);
      }

		}

		private void rbMMServer_CheckedChanged(object sender, System.EventArgs e)
		{
			rbMiAware.Checked = !rbMMServer.Checked;
			tbMiAwareUrl.Enabled = !rbMMServer.Checked;
			tbMMUrl.Enabled = rbMMServer.Checked;			
		  gbAddress.Enabled = false;
		  gbConstraints.Enabled = false;
		  rtbResults.Text = "";
		  gbResults.Enabled = false;
		  btnGeocode.Enabled = false;
		}

		private void rbMiAware_CheckedChanged(object sender, System.EventArgs e)
		{
			rbMMServer.Checked = !rbMiAware.Checked;
			tbMMUrl.Enabled = !rbMiAware.Checked;
			tbMiAwareUrl.Enabled = rbMiAware.Checked;
		  gbAddress.Enabled = false;
		  gbConstraints.Enabled = false;
      rtbResults.Text = "";
      gbResults.Enabled = false;
		  btnGeocode.Enabled = false;
		}

		private void btnExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void btnGeocode_Click(object sender, System.EventArgs e)
		{			
			this.Cursor = Cursors.WaitCursor;
			rtbResults.Text = "";
			PerformGeocode();			
			this.Cursor = Cursors.Default;
		}	
		#endregion

		#region Geocoding Code
		private void PerformGeocode()
		{
			try
			{
				Address[] addrList = new Address[1];

				// set up the address
				addrList[0] = BuildAddress();

				// set up the request
				GeocodeRequest req = new GeocodeRequest(addrList);

				// set the geocoding constraints
				req.GeocodeConstraints = BuildConstraints();

				// geocode
				GeocodeResponse res = _geocodeClient.Geocode(req);

				//process the result
				ProcessResponse(res);
			}
			catch (Exception e)
			{
				System.Windows.Forms.MessageBox.Show("Exception geocoding: " + e.Message);
			}
		}
		
		private Address BuildAddress()
		{
      StreetAddress streetAddress = null;
      // country is required
      string country = tbCountry.Text.ToUpper();
      Address result = new Address(country);
      if (tbStreet.Text != null && !tbStreet.Text.Equals(string.Empty)) {
			  Street street = new Street(tbStreet.Text);
        if (tbPlace.Text != null && !tbPlace.Text.Equals(string.Empty))  {
          Building building = new Building();
          building.BuildingName = tbPlace.Text;
          streetAddress = new StreetAddress(street, building);
        } else {
          streetAddress = new StreetAddress(street);
        }
        result.StreetAddress = streetAddress;
      }

      int numPlaces = 0;
      if (tbCity.Text != null && !tbCity.Text.Equals(string.Empty)) numPlaces++;
			if (tbState.Text != null && !tbState.Text.Equals(string.Empty)) numPlaces++;
      if (numPlaces > 0) {
        int iPlace = 0;
			  Place[] cityState = new Place[numPlaces];
			  if (tbCity.Text != null && !tbCity.Text.Equals(string.Empty)) {
			    cityState[iPlace] = new Place(tbCity.Text, NamedPlaceClassification.Municipality);
          iPlace++;
        }  
			  if (tbState.Text != null && !tbState.Text.Equals(string.Empty)) {
			    cityState[iPlace] = new Place(tbState.Text, NamedPlaceClassification.CountrySubdivision);
        }  
			  result.PlaceList = cityState;
      }  
      if (tbPostalCode.Text != null && !tbPostalCode.Text.Equals(string.Empty)) {
			  result.PrimaryPostalCode = tbPostalCode.Text;
      }  
			return result;
		}
    
    private void SetDefaultConstraints() 
    {
      GeocodeConstraints constraints;
      try {
        constraints = _geocodeClient.DefaultGeocodeConstraints;
      }
      catch (GeocodingException ex) {
        constraints = new GeocodeConstraints();
      }  
      
      cbCloseMatchesOnly.Checked = constraints.CloseMatchesOnly;
      cbFallbackPostal.Checked = constraints.FallbackToPostal;
      cbFallbackGeographic.Checked= constraints.FallbackToGeographic;
      cbMatchAddrNumber.Checked = constraints.MustMatchAddressNumber;
      cbMatchMainAddr.Checked = constraints.MustMatchMainAddress;
      cbMatchInput.Checked = constraints.MustMatchInput;
      cbMatchCity.Checked = constraints.MustMatchMunicipality;
      cbMatchState.Checked = constraints.MustMatchCountrySubdivision;
      cbMatchPostal.Checked = constraints.MustMatchPostalCode;
      cbMatchCountry.Checked = constraints.MustMatchCountry;
      maxCandidates.Value = constraints.MaxCandidates;
      maxRanges.Value = constraints.MaxRanges;
      maxRangeUnits.Value = constraints.MaxRangeUnits;
      streetOffset.Text = constraints.OffsetFromStreet.Value.ToString();
      streetOffsetUnits.Text = MapInfo.Geometry.CoordSys.DistanceUnitKeyword(constraints.OffsetFromStreet.Unit);
      cornerOffset.Text = constraints.OffsetFromCorner.Value.ToString();
      cornerOffsetUnits.Text = MapInfo.Geometry.CoordSys.DistanceUnitKeyword(constraints.OffsetFromCorner.Unit);
      geocodeType.Text = constraints.GeocodeType.ToString();
      dictionaryUsage.Text = constraints.DictionaryUsage.ToString();
    }

		private GeocodeConstraints BuildConstraints()
		{
			GeocodeConstraints result = new GeocodeConstraints();
			result.CloseMatchesOnly = cbCloseMatchesOnly.Checked;
			result.FallbackToPostal = cbFallbackPostal.Checked;
			result.FallbackToGeographic = cbFallbackGeographic.Checked;
			result.MustMatchAddressNumber = cbMatchAddrNumber.Checked;
			result.MustMatchMainAddress = cbMatchMainAddr.Checked;
			result.MustMatchInput = cbMatchInput.Checked;
			result.MustMatchMunicipality = cbMatchCity.Checked;
			result.MustMatchCountrySubdivision = cbMatchState.Checked;
			result.MustMatchPostalCode = cbMatchPostal.Checked;
      result.MustMatchCountry = cbMatchCountry.Checked;
      result.MaxCandidates = (int) maxCandidates.Value;
      result.MaxRanges = (int) maxRanges.Value;
      result.MaxRangeUnits = (int) maxRangeUnits.Value;
      System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
      MapInfo.Geometry.Distance offset = new MapInfo.Geometry.Distance(25.0, MapInfo.Geometry.DistanceUnit.Foot);
      offset.Value = Convert.ToDouble(streetOffset.Text, culture);
      offset.Unit = MapInfo.Geometry.CoordSys.DistanceUnitFromKeyword(streetOffsetUnits.Text);
      result.OffsetFromStreet = offset;
			offset.Value = Convert.ToDouble(cornerOffset.Text, culture);
			offset.Unit = MapInfo.Geometry.CoordSys.DistanceUnitFromKeyword(cornerOffsetUnits.Text);
			result.OffsetFromCorner = offset;
      result.GeocodeType = (MapInfo.Geocoding.GeocodeType) Enum.Parse(typeof(MapInfo.Geocoding.GeocodeType), geocodeType.Text);
      result.DictionaryUsage = (MapInfo.Geocoding.DictionaryUsage) Enum.Parse(typeof(MapInfo.Geocoding.DictionaryUsage), dictionaryUsage.Text);
			return result;
		}
		
		private void ProcessResponse(GeocodeResponse res)
		{
			StringBuilder buffer = new StringBuilder();
			int i = 0;
			IEnumerator enumer = res.GetEnumerator();				
			while (enumer.MoveNext())
			{
				AddressCandidates responses = (AddressCandidates)enumer.Current;
				System.Collections.IEnumerator respEnumer = responses.GetEnumerator();				
				while (respEnumer.MoveNext())
				{
					i +=1;
					buffer.Append("**** Candidate " + i + " ****\n");
					
					CandidateAddress geoAddr = (CandidateAddress)respEnumer.Current;
					Address addr = geoAddr.Address;			
          ProcessAddress(addr, buffer);

					
					buffer.Append("Coordinates: " + geoAddr.Point.X + " ,  " + geoAddr.Point.Y);
					buffer.Append("\n");
					buffer.Append("ResponseCode/Match Type :" + geoAddr.GeocodeMatchCode.ResultCode);
					buffer.Append("\n\n");
				}				
			}

			if (buffer.Length != 0)
				rtbResults.Text = buffer.ToString();
			else
				rtbResults.Text = "No candidates returned";	
		}
    
    private void ProcessAddress(Address addr, StringBuilder buffer) 
    {
      StreetAddress streetAddress = addr.StreetAddress;
      if (streetAddress != null)
      {
    		
    	  Building outBuilding = streetAddress.Building;
    	  if (outBuilding != null)
    	  {	
    		  AddInfoToResults("Building Name", outBuilding.BuildingName, buffer, "\n");								
    		  AddInfoToResults("Building Number", outBuilding.Number, buffer, "\n");								
    	  }

    	  Street returnedStreet = streetAddress.Street;						
    	  AddInfoToResults("Street Type Prefix: ", returnedStreet.TypePrefix, buffer, "\n");
    	  AddInfoToResults("Street Directional Prefix: ", returnedStreet.DirectionalPrefix, buffer, "\n");
   		  AddInfoToResults("Street PreAddress", returnedStreet.PreAddress, buffer, "\n");
    	  AddInfoToResults("Street Official Name", returnedStreet.OfficialName, buffer, "\n");
  		  AddInfoToResults("Street PostAddress", returnedStreet.PostAddress, buffer, "\n");
    	  AddInfoToResults("Street Type Suffix: ", returnedStreet.TypeSuffix, buffer,"\n");
    	  AddInfoToResults("Street Directional Suffix: ", returnedStreet.DirectionalSuffix, buffer, "\n");
        if (streetAddress.Unit != null) {
          AddInfoToResults("Unit Type: ", streetAddress.Unit.Type, buffer, "\t");
          AddInfoToResults("Value: ", streetAddress.Unit.Value, buffer, "\n");
        }  
      }
      AddInfoToResults("Country Code: ", addr.CountryCode, buffer, "\n");
      AddInfoToResults("Primary Postal Code: ", addr.PrimaryPostalCode, buffer, "\n");
      AddInfoToResults("Secondary Postal Code: ", addr.SecondaryPostalCode, buffer, "\n");
      if (addr.StreetIntersection != null) {
        AddInfoToResults("Street Intersection: ", addr.StreetIntersection.ToString(), buffer, "\n");
      }  
      if (addr.PlaceList!= null)
      {
      	System.Collections.IEnumerator placeList = addr.PlaceList.GetEnumerator();
      	while (placeList.MoveNext())
      	{
      		Place place = (Place)placeList.Current;
      		buffer.Append(place.Type + ": " + place.Name);
      		buffer.Append("\n");
      	}
      }

      if ((streetAddress != null) && (streetAddress.AddressRanges != null) && (streetAddress.AddressRanges.Length > 0)) {
        IEnumerator rangeEnum = streetAddress.GetRangeEnumerator();
        int iRange = 0;
        while (rangeEnum.MoveNext()) {
          AddressRange range = (AddressRange) rangeEnum.Current;
          iRange +=1;
          buffer.Append("Range " + iRange + ":\t");
          buffer.Append("\n");
          AddInfoToResults("\tStreetSide: ", range.StreetSide.ToString(), buffer, "\t");
          AddInfoToResults("Parity: ", range.Parity.ToString(), buffer, "\t");
          buffer.Append("\n");							
          AddInfoToResults("\tLow Address Number: ", range.LowAddressNumber, buffer, "\t");
          AddInfoToResults("High Address Number: ", range.HighAddressNumber, buffer, "\t");
          buffer.Append("\n");
          // printing the Range Address Information doesn't seem to add much additional information
          // and just adds clutter to the output, so it is currently ignored. 
          // Remove the #if...#endif below to include the Range Address information.
          #if false
          if (range.AddressSpecified) {
            buffer.Append("\tRange Address Information====>\n");
            ProcessAddress(range.Address, buffer);
            buffer.Append("\t<======\n");
          }
          #endif
          if (range.RangeUnits != null && range.RangeUnits.Length > 0) {
            IEnumerator ruEnum = range.GetRangeUnitEnumerator();
            int iRangeUnit = 0;
            while (ruEnum.MoveNext()) {
              AddressRangeUnit rangeUnit = (AddressRangeUnit) ruEnum.Current;
              iRangeUnit += 1;
              buffer.Append("\tRangeUnit " + iRangeUnit + ":\t");
              buffer.Append("\n");
              AddInfoToResults("\t\tLow Unit: ", rangeUnit.LowUnit, buffer, "\t");
              AddInfoToResults("High Unit: ", rangeUnit.HighUnit, buffer, "");
              buffer.Append("\n");
              // printing the Range Unit Address Information doesn't seem to add much additional information
              // and just adds clutter to the output, so it is currently ignored. 
              // Remove the #if...#endif below to include the Range Unit Address information.
              #if false
              if (rangeUnit.AddressSpecified) {
                buffer.Append("\tRange Unit Address Information====>\n");
                ProcessAddress(rangeUnit.Address, buffer);
                buffer.Append("\t<======\n");
              }
              #endif                  
            }
          }
        }
      }

      if (addr.CensusBlock != null) {
        AddInfoToResults("Census Block: ", addr.CensusBlock, buffer, "\n");
      }
    }
	

		private void AddInfoToResults(String description, String info,
									  StringBuilder buffer, String spacer)
		{

			if ((info != null) && (info.Length != 0))
			{
				buffer.Append(description + ": " + info);				
				buffer.Append(spacer);
			}
		}
    
    private void rbUrlChanged(object sender, System.EventArgs e) 
    {
      gbAddress.Enabled = false;
      gbConstraints.Enabled = false;
      rtbResults.Text = "";
      gbResults.Enabled = false;
      btnGeocode.Enabled = false;
    }
		#endregion

		private void textBox1_TextChanged(object sender, System.EventArgs e) {
		
		}

		private void label1_Click(object sender, System.EventArgs e) {
		
		}

		private void streetOffsetUnits_SelectedIndexChanged(object sender, System.EventArgs e) {
		
		}
	}
}
