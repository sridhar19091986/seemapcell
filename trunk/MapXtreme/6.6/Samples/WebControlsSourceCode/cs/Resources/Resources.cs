using System;
using System.IO;
using System.Reflection;
using System.Resources;

namespace MapInfo.WebControls
{
	///<summary>Manages access to application resources.</summary>
	///<remarks>Used to obtain objects stored as resources (bitmaps, icons, cursors and strings).</remarks>
	internal class Resources {	
		private static string _resourceFolder = "/MapXtremeWebResources 6_6";
		private static string _resouceFolder80 = "MapXtremeWebResources";

		/// <summary>
		/// Property to get and set the location of the resources such as javascript files  and images.
		/// </summary>
		/// <remarks>None</remarks>
		public static string ResourceFolder
		{
			get
			{
				if (System.Environment.Version.Major > 1) {
					return _resouceFolder80;
				} else {
					return _resourceFolder;
				}
			}
			set {_resourceFolder = value;}
		}

		// Object for accessing resources.
		private static MapInfo.L10N.Resources _resources;

		// Property to return the resources object; creates it if ness.
		private static MapInfo.L10N.Resources ResourceManager {
			get {
				if (_resources == null) {
					_resources = new MapInfo.L10N.Resources(Assembly.GetExecutingAssembly(), "MapInfo.WebControls.Resources");
				}
				return _resources;
			}
		}

		///			<summary>Retrieves a String object from resources.</summary>
		///			<remarks>The String returned is from the Strings resource table.</remarks>
		///			<param name="resName">The name of the String to get.</param>
		///			<returns>Returns a String object.</returns>
		///			<exception cref="T:System.ArgumentNullException"/>
		///			<exception cref="T:System.Resources.MissingManifestResourceException"/>
		public static string GetString(string resName) {
			return _resources.GetString(resName);
		}

		///	<summary>Retrieves a file from resources in the form of a stream.</summary>
		///	<remarks>The object returned is from the Files resource table.</remarks>
		///	<param name="resName">The name of the file resource to get.</param>
		///	<returns>Returns a Stream object.</returns>
		///	<exception cref="T:System.ArgumentNullException"/>
		///	<exception cref="T:System.Resources.MissingManifestResourceException"/>
		public static Stream GetFile(string resName) {
			return _resources.GetFile(resName);
		}
	}

	/// <summary>
	/// A class providing access to localization utilities.
	/// </summary>
	internal class L10NUtils {
		private static MapInfo.L10N.Resources _resources;

		/// <summary>
		/// Gets a MapInfo.L10N.Resources objects.
		/// </summary>
		internal static MapInfo.L10N.Resources Resources {
			get {
				if (_resources == null) {
					_resources = new MapInfo.L10N.Resources(Assembly.GetExecutingAssembly(), "MapInfo.WebControls.Resources");
				}
				return _resources;
			}
		}
	}
}
