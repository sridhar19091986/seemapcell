namespace Shape2Sql
{
    using Microsoft.Win32;
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;

    internal class b
    {
        private const string a = @"Software\SharpGIS\Shape2Sql";

        private static RegistryKey a(bool A_0)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SharpGIS\Shape2Sql", A_0);
            if (key == null)
            {
                key = Registry.CurrentUser.CreateSubKey(@"Software\SharpGIS\Shape2Sql");
            }
            return key;
        }

        public static object a(string A_0)
        {
            RegistryKey key = a(false);
            object obj2 = key.GetValue(A_0);
            key.Close();
            return obj2;
        }

        public static void a(string A_0, object A_1)
        {
            RegistryKey key = a(true);
            if (A_1 is ISerializable)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream serializationStream = new MemoryStream();
                formatter.Serialize(serializationStream, A_1 as ISerializable);
                key.SetValue(A_0, serializationStream.ToArray());
            }
            else
            {
                key.SetValue(A_0, A_1);
            }
            key.Close();
        }

        public static T GetValue<T>(string name) where T: ISerializable
        {
            RegistryKey key = a(false);
            object obj2 = key.GetValue(name);
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
            key.Close();
            return local;
        }
    }
}

