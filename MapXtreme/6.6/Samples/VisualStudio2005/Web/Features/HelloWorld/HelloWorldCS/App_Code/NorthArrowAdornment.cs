using System;
using System.Drawing;
using System.Runtime.Serialization;
using MapInfo.Mapping;

namespace HelloWorldWeb
{
	[Serializable]
	public sealed class NorthArrowAdornment : MapInfo.Mapping.IAdornment, ISerializable
	{
		private System.Drawing.Size _size;
		private System.Drawing.Point _location;
		private System.String _name;
		private System.String _alias;
		private System.String _mapAlias;
		private System.IO.MemoryStream _imageStream;
		private event ViewChangedEventHandler _viewChangedEvent=null;

		private bool _blockViewChangedEvent = false;

		public NorthArrowAdornment(string mapAlias, Size size, string adornmentAlias, string name, string fileName)
		{
			_size = size;
			_alias = adornmentAlias;
			_name = name;
			_location = new System.Drawing.Point(0, 0);
			_mapAlias = mapAlias;

			_imageStream = new System.IO.MemoryStream();
			System.Drawing.Image image = System.Drawing.Imaging.Metafile.FromFile(fileName);
			image.Save(_imageStream, image.RawFormat);
			_imageStream.Position = 0;
			image.Dispose();
		}

		/// <summary>
		/// Internal use only.
		/// </summary>
		/// <param name="adornmentAlias">alias name for this adornment.</param>
		/// <param name="mapAlias">Map alias name.</param>
		public NorthArrowAdornment(string adornmentAlias, string mapAlias)
		{
			_alias = adornmentAlias;
			_mapAlias = mapAlias;
		}

		#region IAdornment Members
		public System.Drawing.Size Size
		{
			get
			{ 
				return _size;
			}
			set
			{
				if (_size != value) 
				{
					_size = value;
					OnViewSizeChanged();
				}
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		public System.Drawing.Point Location
		{
			get
			{
				return _location;
			}
			set
			{
				if(this.BlockViewChangedEvent == false)
				{
					if(_location != value)
					{
						_location = value;
						OnViewLocationChanged();
					}
				}
			}
		}

		public string Alias
		{
			get
			{
				return _alias;
			}
			set
			{
				if (this.Map != null) 
				{
					// check if setting alias back to same value
					// or if alias is not unique
					IAdornment a = this.Map.Adornments[value];
					if (a != null && !object.ReferenceEquals(this, a))
					{
						throw new ArgumentException("AliasNotUnique!!!");
					}
				}
				_alias = value;
			}
		}

		public MapInfo.Mapping.Map Map
		{
			get
			{
				return MapInfo.Engine.Session.Current.MapFactory[_mapAlias];
			}
		}

		public void Draw(System.Drawing.Graphics graphics, System.Drawing.Rectangle updateArea, System.Drawing.Point drawPnt)
		{
			_imageStream.Position = 0;
			System.Drawing.Image image = System.Drawing.Imaging.Metafile.FromStream(_imageStream);
			graphics.DrawImage(image, drawPnt);
			image.Dispose();
		}

		public event ViewChangedEventHandler ViewChangedEvent
		{
			add
			{
				_viewChangedEvent += value;
			}
			remove
			{
				_viewChangedEvent -= value;
			}
		}
		#endregion

		public bool BlockViewChangedEvent
		{
			get
			{
				return _blockViewChangedEvent;
			}
			set
			{
				_blockViewChangedEvent = value;
			}
		}

		private void OnViewChanged(ViewChangedEventArgs e) 
		{
			if(!BlockViewChangedEvent)
			{
				_viewChangedEvent(this, e);
			}
		}

		private void OnViewChanged() 
		{
			ViewChangedEventArgs e=new ViewChangedEventArgs();
			OnViewChanged(e);
		}

		private void OnViewSizeChanged() 
		{
			ViewChangedEventArgs e=new ViewChangedEventArgs();
			e.SizeChange = true;
			OnViewChanged(e);
		}

		private void OnViewLocationChanged() 
		{
			ViewChangedEventArgs e=new ViewChangedEventArgs();
			e.LocationChange = true;
			OnViewChanged(e);
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_alias", _alias);
			info.AddValue("_name", _name);
			info.AddValue("_size", _size);
			info.AddValue("_mapAlias", _mapAlias);
			info.AddValue("_location", _location);
			info.AddValue("_imageStream", _imageStream);
			info.SetType(typeof(NorthArrowAdornmentDeserializer));
		}

		public void SetObjectData(SerializationInfo info, StreamingContext context)
		{
			_name = info.GetString("_name");
			_size = (System.Drawing.Size)info.GetValue("_size", typeof(System.Drawing.Size));
			_alias = info.GetString("_alias");
			_mapAlias = info.GetString("_mapAlias");
			_location = (System.Drawing.Point)info.GetValue("_location", typeof(System.Drawing.Point));
			_imageStream = (System.IO.MemoryStream)info.GetValue("_imageStream", typeof(System.IO.MemoryStream));
		}
	}
	/// <summary>Implements deserialization of a NorthArrowAdornment instance.</summary>
	/// <remarks>See the article at 
	/// http://msdn.microsoft.com/msdnmag/issues/02/07/net/print.asp.
	/// See also the "Object References" section of the article at
	/// http://community.borland.com/article/0,1410,29787,00.html.
	/// </remarks>
	[Serializable]
	public sealed class NorthArrowAdornmentDeserializer : IObjectReference, ISerializable
	{
		NorthArrowAdornment _adornment = null;

		private NorthArrowAdornmentDeserializer(SerializationInfo info, StreamingContext context)
		{
			string alias = info.GetString("_alias");
			string mapAlias = info.GetString("_mapAlias");

			// See if the "same" map exists.
			MapInfo.Mapping.Map map = MapInfo.Engine.Session.Current.MapFactory[mapAlias];
			if (map == null)
			{
				throw new SerializationException(MapInfo.Engine.Session.Current.Resources.GetString("MapInfo.Serialization.AdornmentMapNotFound", alias, mapAlias));
			}
			
			IAdornment adornment = map.Adornments[alias];
			if (adornment != null)
			{
				if (adornment is NorthArrowAdornment) 
				{
					_adornment = (NorthArrowAdornment)adornment;
				}
				else
				{
					map.Adornments.Remove(alias);
					_adornment=null;
					adornment=null;
				}
			}
			// The "same" adornment doesn't exist. Create it from scratch.
			if (adornment == null ) 
			{
				_adornment = new NorthArrowAdornment(alias, mapAlias);
				//adornment = _adornment;
				map.Adornments.Append(_adornment);
			}

			_adornment.SetObjectData(info, context);
		}
		Object IObjectReference.GetRealObject(StreamingContext context)
		{
			// NOTE: This method is called twice for each deserialization.
			// According to the http://community.borland.com/article/0,1410,29787,00.html
			// article, this was known by Microsoft as a bug in the 1.1 beta.
			// There is a work around, but as long as
			// there's no processing done here, no harm is done.
			return _adornment;
		}
		///	<summary>Required by the ISerializable interface, but this one is never called.</summary>
		///	<param name="info">The SerializationInfo to populate with data.</param>
		///	<param name="context">The destination for this serialization.</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) { }
	}
}
