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

            //string sql = "UPDATE [SqlSpatialJiangmeng].[dbo].[Abis_MR] SET [cell] = 'JMJZTG1'";
            //DataClasses1DataContext dc = new DataClasses1DataContext();
            //dc.ExecuteCommand(sql);

            //起始小区？

            UpdateServiceCell usc = new UpdateServiceCell("JMJDEY1");

            //UpdateServiceCell usc = new UpdateServiceCell("JMJJHJ1");

            //通过m-trix导入数据，生成弱覆盖路线图？？？？？

            //小区覆盖分析


           // cellLocating cl = new cellLocating();

            //mr服务小区和邻小区分析
            pointLocating pl = new pointLocating();

            //mr位置分析
            lineLocating ll = new lineLocating();


            //getLocating();
            //getSectorCoverage();
            Console.ReadKey();

            //主要存在的问题，ba丢失，定位不准，从si消息中获取ba表，但是m没解码？？？？？？

            //天线高度和经纬度的更新的问题？？？？？？

        }
    }
}
