using System;

namespace ThematicsWeb
{
	/// <summary>
	/// Summary description for SampleConstants.
	/// </summary>
	public class SampleConstants
	{
		// eword is a mdb table.
		public static string EWorldAlias = "eworld";
		// eworld.tab wrapped eword.mdb table.
		public static string EWorldTabFileName = "eworld.tab";
		// ThemeTableAlias is the table on which the sample will create. 
		public static string ThemeTableAlias = "world";
		// ThemeLayerAlias is the FeatureLayer on which the sample will create.
		public static string ThemeLayerAlias = "world";
		// new LabelLayer alias for this sample.
		public static string NewLabelLayerAlias = "LabelAliasForBoundData";
		// alias name for a group layer which contents are going to be changed frenquently.
		public static string GroupLayerAlias = "tempGroupLayerAlias";
		// Bound data columns, these columns come from eworld table.
		public static string[] BoundDataColumns = new string[]{"Pop_1994", "Pop_Male", "Pop_Fem"};
		// Will be used in table's AddColumns method.
		public static string SouceMatchColumn = "Country";
		// Will be used in table's AddColumns method.
		public static string TableMatchColumn = "Country";

		private SampleConstants(){}
	}
}
