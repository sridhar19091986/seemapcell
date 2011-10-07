namespace Shape2Sql
{
    using SharpGIS.Gis.Data.SqlServer;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Forms;

    internal static class e
    {
        private static f a;

        private static bool a()
        {
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            if (commandLineArgs.Length <= 1)
            {
                return false;
            }
            c c = new c(commandLineArgs);
            if (c.a("help") != null)
            {
                string text = "Commandline options:\r\nshape2sql.exe\r\n  -shp=[shapefile]\r\n  -connstr=[connectionstring] \r\n  -table=tablename                 //defaults to shapefile name\r\n  -srid=[sridnumber]               //defaults to -1 (required for geography)\r\n  -oidname=[IDcolumnName]    //defaults to 'ID'\r\n  -geomname=[GeometryColumnName] //defaults to 'geom'\r\n  -append=[true|false]             //defaults to false (replace table)\r\n  -geography=true                  //defaults to false (geometry type)\r\n  -columns=col1,col2,col3       //defaults to all columns";
                MessageBox.Show(text);
                return true;
            }
            string str2 = c.a("shp");
            if (string.IsNullOrEmpty(str2))
            {
                return false;
            }
            if (File.Exists(str2))
            {
                string str3 = c.a("connstr");
                if (string.IsNullOrEmpty(str3))
                {
                    return false;
                }
                string name = c.a("table");
                if (string.IsNullOrEmpty(name))
                {
                    name = new FileInfo(str2).Name;
                    name = name.Substring(0, name.LastIndexOf('.'));
                }
                string s = c.a("srid");
                int result = -1;
                bool flag = int.TryParse(s, out result);
                bool flag2 = c.a("geography") == "true";
                string str6 = c.a("oidname") ?? "ID";
                string str7 = c.a("geomname") ?? "geom";
                bool flag3 = true;
                bool flag4 = c.a("append") == null;
                string[] strArray2 = null;
                string str8 = c.a("columns");
                if (str8 != null)
                {
                    strArray2 = str8.Split(new char[] { ',' });
                }
                SharpGIS.Gis.Data.SqlServer.a a = new SharpGIS.Gis.Data.SqlServer.a(str3, name, str2, flag, result, flag2, str6, str7, flag3, flag4, strArray2);
                e.a = new f();
                e.a.Show();
                e.a.b(string.Format("Inserting file '{0}' into table '{1}'...", str2, name));
                e.a.b(string.Format("SRID = {0}", result));
                e.a.b(string.Format("Insert as Geography = {0}", flag2));
                e.a.b(string.Format("ID column name = {0}", str6));
                e.a.b(string.Format("Geometry column name = {0}", str7));
                e.a.b(string.Format("Replace existing table = {0}", flag4));
                e.a.b(string.Format("Columns = {0}", str8 ?? "[all]"));
                a.a(new RunWorkerCompletedEventHandler(e.a));
                a.b(new ProgressChangedEventHandler(e.a));
                a.a(new SharpGIS.Gis.Data.SqlServer.a.a(e.a));
                a.a();
                Application.Run();
            }
            return true;
        }

        private static void a(object A_0, SharpGIS.Gis.Data.SqlServer.a.b A_1)
        {
            string str = string.Format("Could not insert row #{0}\r\n\t{1}: {2}", A_1.b(), A_1.c().GetType(), A_1.c().Message);
            a.b(str);
        }

        private static void a(object A_0, ProgressChangedEventArgs A_1)
        {
            a.a(string.Format("{0} ({1}%)", (string) A_1.UserState, A_1.ProgressPercentage));
        }

        private static void a(object A_0, RunWorkerCompletedEventArgs A_1)
        {
            a.b("\r\nComplete!");
            a.Dispose();
            Application.Exit();
        }

        [STAThread]
        private static void b()
        {
            if (!a())
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new d());
            }
        }
    }
}

