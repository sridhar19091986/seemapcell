using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linq2SqlGeography;
using Microsoft.SqlServer.Types;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using Linq2SqlGeography.LinqSql;


namespace Linq2SqlGeography
{

    class Program
    {
        static void Main(string[] args)
        {

            string sql = "UPDATE [SqlSpatialJiangmeng].[dbo].[Abis_MR] SET [cell] = 'JMJZTG1'";
            DataClasses2DataContext dc = new DataClasses2DataContext();
            dc.ExecuteCommand(sql);
            
            //通过m-trix导入数据，生成弱覆盖路线图？？？？？

            cellLocating cl = new cellLocating();

            pointLocating pl = new pointLocating();


            lineLocating ll = new lineLocating();
         

            //getLocating();
            //getSectorCoverage();
            Console.ReadKey();
        }
    }
}
