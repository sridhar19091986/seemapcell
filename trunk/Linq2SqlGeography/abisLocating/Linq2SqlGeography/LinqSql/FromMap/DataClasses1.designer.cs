﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.488
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Linq2SqlGeography.LinqSql.FromMap
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="SqlSpatialJiangmeng")]
	public partial class DataClasses1DataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void Insert城市内部主要交通干道(城市内部主要交通干道 instance);
    partial void Update城市内部主要交通干道(城市内部主要交通干道 instance);
    partial void Delete城市内部主要交通干道(城市内部主要交通干道 instance);
    partial void Insert城市内部主要交通干道abc(城市内部主要交通干道abc instance);
    partial void Update城市内部主要交通干道abc(城市内部主要交通干道abc instance);
    partial void Delete城市内部主要交通干道abc(城市内部主要交通干道abc instance);
    #endregion
		
		public DataClasses1DataContext() : 
				base(global::Linq2SqlGeography.Properties.Settings.Default.SqlSpatialTestConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<城市内部主要交通干道> 城市内部主要交通干道
		{
			get
			{
				return this.GetTable<城市内部主要交通干道>();
			}
		}
		
		public System.Data.Linq.Table<城市内部主要交通干道abc> 城市内部主要交通干道abc
		{
			get
			{
				return this.GetTable<城市内部主要交通干道abc>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.城市内部主要交通干道")]
	public partial class 城市内部主要交通干道 : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _编码;
		
		private string _MI_STYLE;
		
		private int _MI_PRINX;
		
		private Microsoft.SqlServer.Types.SqlGeometry _SP_GEOMETRY;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void On编码Changing(string value);
    partial void On编码Changed();
    partial void OnMI_STYLEChanging(string value);
    partial void OnMI_STYLEChanged();
    partial void OnMI_PRINXChanging(int value);
    partial void OnMI_PRINXChanged();
    partial void OnSP_GEOMETRYChanging(Microsoft.SqlServer.Types.SqlGeometry value);
    partial void OnSP_GEOMETRYChanged();
    #endregion
		
		public 城市内部主要交通干道()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_编码", DbType="VarChar(4)")]
		public string 编码
		{
			get
			{
				return this._编码;
			}
			set
			{
				if ((this._编码 != value))
				{
					this.On编码Changing(value);
					this.SendPropertyChanging();
					this._编码 = value;
					this.SendPropertyChanged("编码");
					this.On编码Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MI_STYLE", DbType="VarChar(254)")]
		public string MI_STYLE
		{
			get
			{
				return this._MI_STYLE;
			}
			set
			{
				if ((this._MI_STYLE != value))
				{
					this.OnMI_STYLEChanging(value);
					this.SendPropertyChanging();
					this._MI_STYLE = value;
					this.SendPropertyChanged("MI_STYLE");
					this.OnMI_STYLEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MI_PRINX", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int MI_PRINX
		{
			get
			{
				return this._MI_PRINX;
			}
			set
			{
				if ((this._MI_PRINX != value))
				{
					this.OnMI_PRINXChanging(value);
					this.SendPropertyChanging();
					this._MI_PRINX = value;
					this.SendPropertyChanged("MI_PRINX");
					this.OnMI_PRINXChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SP_GEOMETRY", UpdateCheck=UpdateCheck.Never)]
		public Microsoft.SqlServer.Types.SqlGeometry SP_GEOMETRY
		{
			get
			{
				return this._SP_GEOMETRY;
			}
			set
			{
				if ((this._SP_GEOMETRY != value))
				{
					this.OnSP_GEOMETRYChanging(value);
					this.SendPropertyChanging();
					this._SP_GEOMETRY = value;
					this.SendPropertyChanged("SP_GEOMETRY");
					this.OnSP_GEOMETRYChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.城市内部主要交通干道abc")]
	public partial class 城市内部主要交通干道abc : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _编码;
		
		private string _MI_STYLE;
		
		private int _MI_PRINX;
		
		private System.Data.Linq.Binary _SP_GEOMETRY;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void On编码Changing(string value);
    partial void On编码Changed();
    partial void OnMI_STYLEChanging(string value);
    partial void OnMI_STYLEChanged();
    partial void OnMI_PRINXChanging(int value);
    partial void OnMI_PRINXChanged();
    partial void OnSP_GEOMETRYChanging(System.Data.Linq.Binary value);
    partial void OnSP_GEOMETRYChanged();
    #endregion
		
		public 城市内部主要交通干道abc()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_编码", DbType="VarChar(4)")]
		public string 编码
		{
			get
			{
				return this._编码;
			}
			set
			{
				if ((this._编码 != value))
				{
					this.On编码Changing(value);
					this.SendPropertyChanging();
					this._编码 = value;
					this.SendPropertyChanged("编码");
					this.On编码Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MI_STYLE", DbType="VarChar(254)")]
		public string MI_STYLE
		{
			get
			{
				return this._MI_STYLE;
			}
			set
			{
				if ((this._MI_STYLE != value))
				{
					this.OnMI_STYLEChanging(value);
					this.SendPropertyChanging();
					this._MI_STYLE = value;
					this.SendPropertyChanged("MI_STYLE");
					this.OnMI_STYLEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MI_PRINX", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int MI_PRINX
		{
			get
			{
				return this._MI_PRINX;
			}
			set
			{
				if ((this._MI_PRINX != value))
				{
					this.OnMI_PRINXChanging(value);
					this.SendPropertyChanging();
					this._MI_PRINX = value;
					this.SendPropertyChanged("MI_PRINX");
					this.OnMI_PRINXChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SP_GEOMETRY", DbType="VarBinary(1)", UpdateCheck=UpdateCheck.Never)]
		public System.Data.Linq.Binary SP_GEOMETRY
		{
			get
			{
				return this._SP_GEOMETRY;
			}
			set
			{
				if ((this._SP_GEOMETRY != value))
				{
					this.OnSP_GEOMETRYChanging(value);
					this.SendPropertyChanging();
					this._SP_GEOMETRY = value;
					this.SendPropertyChanged("SP_GEOMETRY");
					this.OnSP_GEOMETRYChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
