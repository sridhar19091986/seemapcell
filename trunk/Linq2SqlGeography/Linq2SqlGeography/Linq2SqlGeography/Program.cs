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
            getLocating();
            getSectorCoverage();
            Console.ReadKey();
        }

  
        private static int pencolor = 0;
    

        private static void getSectorCoverage()
        {
            DataClasses2DataContext dc = new DataClasses2DataContext();
            dc.ExecuteCommand(HandleTable.crcelltracing);
            Console.WriteLine(dc.SITE.Count());
            foreach (var site in dc.SITE)
            {
                if (site.latitude == null) continue;
                CellCoverage cc = new CellCoverage();

                #region 这里的算法复杂度高，仿真的过程比较复杂

                cc.pre_rxlev = -94;

                #endregion

                var sgeo = cc.MergePoint(site);
                SqlGeometry mgeo = SqlGeometry.STGeomFromWKB(sgeo.STAsBinary(), 4326).STConvexHull();

                pencolor = HandleTable.getRandomPenColor();

                CELLTRACING ct = new CELLTRACING();
                ct.cell = site.cell;
                ct.MI_STYLE = "Pen (1, 60," +pencolor.ToString() + ")";
                ct.SP_GEOMETRY = mgeo;
                string sql = @" INSERT INTO [CELLTRACING]([cell],[MI_STYLE],[SP_GEOMETRY]) VALUES  ('"
                    + ct.cell + "','" + ct.MI_STYLE + "','" + ct.SP_GEOMETRY + "')";
                dc.ExecuteCommand(sql);
            }
        }

        private static void getLocating()
        {
            DataClasses2DataContext dc = new DataClasses2DataContext();
            dc.ExecuteCommand(HandleTable.creventlocating);

            mrLocating mrl = new mrLocating();
            mrl.sLocating("", 0, 0);
            mrl.nLocating("", 0, 0);
            mrl.nLocating("", 0, 0);
            mrl.nLocating("", 0, 0);
            mrl.nLocating("", 0, 0);
            mrl.nLocating("", 0, 0);
            mrl.getLocating();

            Console.WriteLine("{0}...{1}",mrl.sgeo.Long,mrl.sgeo.Lat);
            Console.ReadKey();

            string events="";
            string pen = "Pen (6, 60," + 65536 + ")";
            SqlGeometry mgeo = SqlGeometry.STGeomFromWKB(mrl.sgeo.STAsBinary(), 4326).STConvexHull();
            string sql = @" INSERT INTO [EventLocating]([events],[MI_STYLE],[SP_GEOMETRY]) VALUES  ('"
                + events + "','" + pen + "','" + mgeo + "')";

            dc.ExecuteCommand(sql);

        }
    }
}
