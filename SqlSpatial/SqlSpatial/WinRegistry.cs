namespace SqlSpatial
{
    using Microsoft.Win32;
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;

    internal class WinRegistry
    {
        private const string RegistryPath = @"Software\SharpGIS\SqlSpatialQuery";

        private static RegistryKey GetRoot(bool writable)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SharpGIS\SqlSpatialQuery", writable);
            if (key == null)
            {
                key = Registry.CurrentUser.CreateSubKey(@"Software\SharpGIS\SqlSpatialQuery");
            }
            return key;
        }

        public static T GetValue<T>(string name) where T: ISerializable
        {
            RegistryKey root = GetRoot(false);
            object obj2 = root.GetValue(name);
            T local = default(T);
            if (obj2 is byte[])
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    MemoryStream serializationStream = new MemoryStream(obj2 as byte[]);
                    local = (T) formatter.Deserialize(serializationStream);
                }
                catch
                {
                }
            }
            root.Close();
            return local;
        }

        public static object GetValue(string name)
        {
            RegistryKey root = GetRoot(false);
            object obj2 = root.GetValue(name);
            root.Close();
            return obj2;
        }

        public static void SetValue(string name, object value)
        {
            RegistryKey root = GetRoot(true);
            if (value is ISerializable)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream serializationStream = new MemoryStream();
                formatter.Serialize(serializationStream, value as ISerializable);
                root.SetValue(name, serializationStream.ToArray());
            }
            else
            {
                root.SetValue(name, value);
            }
            root.Close();
        }
    }
}

