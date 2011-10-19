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
            
            //通过m-trix导入数据，生成弱覆盖路线图？？？？？

           
            pointLocating pl = new pointLocating();
            lineLocating ll = new lineLocating();
            cellLocating cl = new cellLocating();

            //getLocating();
            //getSectorCoverage();
            Console.ReadKey();
        }
    }
}
