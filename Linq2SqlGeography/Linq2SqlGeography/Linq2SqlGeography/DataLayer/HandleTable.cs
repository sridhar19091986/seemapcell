using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Reflection;

namespace Linq2SqlGeography
{
    public static class HandleTable
    {
        public static bool CreateTable(Type linqTableClass)
        {
            //handleTable.CreateTable(typeof(OpCiPDCH));

            bool suc = true;
            string createtable = linqTableClass.Name;
            //MessageBox.Show(createtable);
            //混淆以后反射名称被改变出现问题
            using (DataClasses2DataContext localdb = new DataClasses2DataContext())
            {
                try
                {
                    var metaTable = localdb.Mapping.GetTable(linqTableClass);
                    var typeName = "System.Data.Linq.SqlClient.SqlBuilder";
                    var type = typeof(DataContext).Assembly.GetType(typeName);
                    var bf = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod;
                    var sql = type.InvokeMember("GetCreateTableCommand", bf, null, null, new[] { metaTable });
                    string delSql = @"if exists (select 1 from  sysobjects where  id = object_id('dbo." + createtable + @"') and   type = 'U')
                            drop table dbo." + createtable;
                    localdb.ExecuteCommand(delSql.ToString());
                    localdb.ExecuteCommand(sql.ToString());
                }
                catch (Exception ex)
                {
                    suc = false;
                    Console.WriteLine(ex.ToString());
                    //MessageBox.Show(ex.ToString());
                }
            }
            return suc;
        }
    }
}
