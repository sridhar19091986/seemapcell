using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
//using Linq2SqlGeography.LinqSql;
using System.Text.RegularExpressions;
//using Linq2SqlGeography.LinqSql.FromAbis;
//using Linq2SqlGeography.LinqSql.FromMap;
using Linq2SqlGeography.LinqSql.FromOSS;
using Linq2SqlGeography.LinqSql.ToMap;

namespace Linq2SqlGeography
{
    public class cellLocating
    {
        private static int pencolor = 0;
        private SqlGeometry sgeom = new SqlGeometry();
        private SqlGeography sgeog = new SqlGeography();
        private string sql = null;
        private Linq2SqlGeography.LinqSql.FromOSS.DataClasses1DataContext dc_oss = new Linq2SqlGeography.LinqSql.FromOSS.DataClasses1DataContext();
        private Linq2SqlGeography.LinqSql.ToMap.DataClasses1DataContext dc_tmap =new Linq2SqlGeography.LinqSql.ToMap.DataClasses1DataContext();

        public cellLocating()
        {
            getSectorCoverage();
        }

        private void getSectorCoverage()
        {

            dc_tmap.ExecuteCommand(HandleTable.createCellTracing);
            Console.WriteLine(dc_oss.SITE.Count());

            foreach (var site in dc_oss.SITE)
            {

                if (site.latitude == null) continue;

                CellCoverage cc = new CellCoverage();

                #region 这里的算法复杂度高，仿真的过程比较复杂

                cc.pre_rxlev = -94;

                #endregion

                sgeog = cc.MergePoint(site);
                sgeom = SqlGeometry.STGeomFromWKB(sgeog.STAsBinary(), 4326).STConvexHull();

                pencolor = HandleTable.getRandomPenColor(false, false, false);

                CellTracing ct = new CellTracing();
                ct.cell = site.cell;
                ct.MI_STYLE = "Pen (1, 60," + pencolor.ToString() + ")";
                ct.SP_GEOMETRY = sgeom;
                sql = @" INSERT INTO [CELLTRACING]([cell],[MI_STYLE],[SP_GEOMETRY]) VALUES  ('"
                    + ct.cell + "','" + ct.MI_STYLE + "','" + ct.SP_GEOMETRY + "')";
                dc_tmap.ExecuteCommand(sql);

            }
        }
    }
}
