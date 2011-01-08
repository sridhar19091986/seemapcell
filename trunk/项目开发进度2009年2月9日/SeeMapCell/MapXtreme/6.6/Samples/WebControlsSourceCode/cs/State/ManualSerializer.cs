using System;
using System.Web;

namespace MapInfo.WebControls
{
	/// <summary>
	/// A utility class used to serialize an object to its binary format or restore an object from its binary format.
	/// </summary>
	/// <remarks>This class allows users to serialize and deserialize as a stream. The objects which are ISerializable get deserialized automatically
	/// by ASP.NET. If you want to prevent this, and do your own deserialization, this class can be used.</remarks>
	[Serializable]
	public class ManualSerializer
	{
		/// <summary>
		/// Uses the System formatters to save the MapXtreme objects in the session state as a binary blob.
		/// </summary>
		/// <param name="ser">A serializable object.</param>
		/// <remarks>If you simply send it to the Session state it will automatically extract itself the next time the user accesses the site. This allows you to deserialize certain objects when you want them. This method takes an object and saves it's binary stream.</remarks>
		/// <returns>A byte array to hold binary format version of object you passed in.</returns>
		public static byte[] BinaryStreamFromObject(object ser)
		{
			System.IO.MemoryStream memStr = new System.IO.MemoryStream();
			System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			formatter.Serialize(memStr, ser);
			return memStr.GetBuffer();
		}
		/// <summary>
		/// Uses the System formatters to save the MapXtreme objects in the session state as a binary blob.
		/// </summary>
		/// <param name="bits">A byte array to hold a object binary format.</param>
		/// <remarks>If you simply send it to the Session state it will automatically extract itself the next time the user accesses the site. This allows you to deserialize certain objects when you want them. This method takes a binary stream and returns an object. Casting happens later.</remarks>
		/// <returns>A object restored from its binary format.</returns>
		public static object ObjectFromBinaryStream(byte[] bits)
		{
			object ret = null;
			if(bits != null)
			{
				System.IO.MemoryStream memStr = new System.IO.MemoryStream(bits);
				System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				ret = formatter.Deserialize(memStr);
			}
			return ret;
		}

		/// <summary>
		/// Gets or sets a serializable object with the specified key.
		/// </summary>
		/// <param name="hashName">The string key used by HttpContext.Current.Session to store binary format of the object, or restore the object from its binary format.</param>
		/// <remarks>None.</remarks>
		public object this[string hashName]
		{
			get
			{
				return ObjectFromBinaryStream(HttpContext.Current.Session[hashName] as byte[]);
			}
			set
			{
				HttpContext.Current.Session[hashName] = BinaryStreamFromObject(value);
			}
		}

		/// <summary>
		/// Saves MapXtreme object into HttpSessionState
		/// </summary>
		/// <param name="o">MapXtreme object</param>
		/// <param name="name">Name to be used as key</param>
		/// <remarks>This function uses BinaryFormatter to save stream of bytes into HttpSessionState. 
		/// The error handling for this method has to be taken care of by users in their application.
		/// </remarks>
		public static void SaveMapXtremeObjectIntoHttpSession(object o, string name) {
			HttpContext.Current.Session[name] = ManualSerializer.BinaryStreamFromObject(o);
		}

		/// <summary>
		/// Restores MapXtreme from HttpSessionState into MapXtreme Session.
		/// </summary>
		/// <param name="name">Name used as key to save the object</param>
		/// <remarks>This function restores MapXtreme object from HttpSessionState into MapXtreme session.
		/// 	The error handling for this method has to be taken care of by users in their application.
		/// </remarks>
		public static void RestoreMapXtremeObjectFromHttpSession(string name) {
			if (HttpContext.Current.Session[name] != null) {
				byte[] bits = HttpContext.Current.Session[name] as byte[];
				object o = ManualSerializer.ObjectFromBinaryStream(bits);
			}
		}
	}
}

