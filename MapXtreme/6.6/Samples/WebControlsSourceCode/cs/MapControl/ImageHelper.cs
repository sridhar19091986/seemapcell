#region Copyright (c) 1989-2003, MAPINFO CORPORATION
/*****************************************************************************
*       Copyright (c) 1989-2003, MAPINFO CORPORATION
*       All  rights reserved.
*       Confidential Property of MAPINFO CORPORATION
*
* $Workfile: ImageHelper.cs $
* $Revision: 2 $
* $Modtime: 1/10/06 4:27p $
*
* $Log: /staging_MapXtreme/MapInfo.Net/WebControls/cs/MapControl/ImageHelper.cs $
 * 
 * 2     1/10/06 4:44p Sukumta
 * Xml comment changes to improve documentation
 * 
 * 1     11/14/05 8:41p Sukumta
 * 
 * 6     8/02/05 1:15p Sukumta
 * Renamed snippet name
 * 
 * 5     6/10/05 5:36p Chdougla
 * Merged the $/main teamline into $/staging_MapXtreme again, while
 * retaining all MapXtreme 6.2 merge changes.
 * 
 * 3     6/03/05 1:07p Tocitrin
 * Extracted any inline code samples and moved them to the snippets
 * solution.
* 
* 4     9/21/04 10:54a Nalindsa
* Backfill Project
* 
* 3     4/07/04 11:59a Josturma
* fixed bad XML
* 
* 2     04/04/06 5:44p Soouyang
* add sample code
* 
* 1     04/04/02 5:06p Soouyang
* new file
* 
*/
#endregion

using System;
using System.Web;

namespace MapInfo.WebControls
{
	///<summary>Helper class to save and get images from cache.</summary>
	///	<remarks>If you are NOT using MapInfo WebControls, this class contains method
	/// to help you save map or legend images into Cache and return a URL that you can use
	/// in href of your img tag to pick up the image. The following are some code examples:</remarks>
	/// <include file="../../Snippets/Dev/MapInfo.Web.cs.xml" path="CodeSamples/Snippets[@name='MapInfo_Web_ImageHelper']/*"/>
	public class ImageHelper
	{
		///			<summary>Constructor for stream handler.</summary>
		internal ImageHelper() {}

		///			<summary>Indicates the Path of the URL where we get map image from.</summary>
		///			<remarks>None.</remarks>
		public const string Path = "GetMapXtremeImage.aspx";
		///			<summary>Used to extract image stream from Cache.</summary>
		///			<remarks>None.</remarks>
		public const string StreamIDParameter = "StreamID";
		///			<summary>Indicates which format was used to export the image by map.</summary>
		///			<remarks>This is used to set the response content type.</remarks>
		public const string FormatParameter = "Format";

		///			<summary>Returns an unique ID that you can use as the key to the cached
		/// object.</summary>
		/// <remarks>None</remarks>
		///			<returns>Returns an unique ID, GUID.</returns>
		public static string GetUniqueID()
		{
			return Guid.NewGuid().ToString();
		}

		///			<summary>Formats the query string to be used to extract the stream out of
		/// Cache.</summary>
		/// <remarks>None</remarks>
		///			<param name="streamID">Stream ID.</param>
		///			<param name="imageFormat">Image format.</param>
		///			<returns>Returns a String containing the URL. You can put this string in href
		/// of the image tag.</returns>
		public static string GetImageURL(string streamID, string imageFormat)
		{
			return string.Format("{0}?{1}={2}&{3}={4}", 
				Path, StreamIDParameter, streamID, FormatParameter, imageFormat);
		}
		///			<summary>Formats the query string to be used to extract the stream out of
		/// Cache.</summary>
		/// <remarks>None</remarks>
		///			<param name="streamID">Stream ID.</param>
		///			<returns>Returns a String containing the query.</returns>
		public static string GetImageURL(string streamID)
		{
			return string.Format("{0}?{1}={2}", Path, StreamIDParameter, streamID);
		}

		///	<summary>Inserts the obj that is associated with the key in the Context.Cache.</summary>
		/// <remarks>None</remarks>
		/// <param name="key">An unique key to the obj.</param>
		///	<param name="obj">The object that need to be cached, normally this is a memory
		/// Stream of an image.</param>
		///	<param name="timeOutInMinutes">Auto-remove this obj from cache after this integer,
		/// timeOutInMinutes.</param>
		public static void SetImageToCache(string key, object obj, int timeOutInMinutes)
		{
			HttpContext.Current.Cache.Insert(key, obj, null, DateTime.Now.AddMinutes(timeOutInMinutes), TimeSpan.Zero);
		}

		///			<summary>Returns the obj that associated with the key from the Context.Cache
		/// and this object is removed from the Cache.</summary>
		/// <remarks>None</remarks>
		///			<param name="key">An unique key to the obj.</param>
		///			<returns>Returns an obj in the Cache.</returns>
		public static object GetImageFromCache(string key)
		{
			object obj = HttpContext.Current.Cache[key];
			HttpContext.Current.Cache.Remove(key);
			return obj;
		}

		///			<summary>Returns the obj that associated with the key from the Context.Cache
		/// and this object is removed from the Cache.</summary>
		/// <remarks>None</remarks>
		///			<param name="key">An unique key to the obj.</param>
		///			<param name="bRemove">If <c>true</c>, remove the obj from the cache.</param>
		///			<returns>Returns the obj in the cache.</returns>
		public static object GetImageFromCache(string key, bool bRemove)
		{
			object obj = HttpContext.Current.Cache[key];
			if (bRemove)
				HttpContext.Current.Cache.Remove(key);
			return obj;
		}
	}
}
