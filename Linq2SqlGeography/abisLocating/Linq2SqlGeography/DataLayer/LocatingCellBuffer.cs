using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
using Linq2SqlGeography.LinqSql.ToMap;
using Linq2SqlGeography.LinqSql.FromOSS;
using System.Text.RegularExpressions;

namespace Linq2SqlGeography
{
    class LocatingCellBuffer
    {
        private Linq2SqlGeography.LinqSql.FromOSS.DataClasses1DataContext dc_oss = new Linq2SqlGeography.LinqSql.FromOSS.DataClasses1DataContext();
        private Linq2SqlGeography.LinqSql.ToMap.DataClasses1DataContext dc_tmap = new Linq2SqlGeography.LinqSql.ToMap.DataClasses1DataContext();
        private OkumuraHata okumh = new OkumuraHata();
        private double mobileHight = 1.5;    //移动台高度

        private string sCell { get; set; }
        private double sRxlev { get; set; }
        private double sPowerControl { get; set; }

        private double sPowerN { get; set; }
        private double sAntGain;
        private double sHeight;
        private double sPathLoss { get; set; }

        private SqlGeography sitesgeog;
        //private SqlGeometry mgeo;
        private SqlGeography okumbuffersgeog;
        private SITE site;
        private CellTracing celltracing;
        private SqlGeometry celltracingsgeom;
        private SqlGeography celltracingsgeog;

        public LocatingCellBuffer(string cell, double rxlev, double powerControl)
        {
            this.sCell = cell;
            this.sRxlev = rxlev;
            this.sPowerControl = powerControl;
        }
        public SqlGeography getCellGeo()
        {
            site = dc_oss.SITE.Where(e => e.cell == this.sCell).FirstOrDefault();
            if (site == null) return SqlGeography.Point(0, 0, 4236);
            sitesgeog = SqlGeography.Point((double)site.latitude, (double)site.longitude, 4326);
            double.TryParse(site.ant_gain, out sAntGain);
            double.TryParse(site.height, out sHeight);
            sPathLoss = okumh.PathLoss2Distance((double)site.band, (double)sHeight, (double)mobileHight, (double)(sPowerN + sAntGain - sPowerControl - sRxlev));

            Console.WriteLine(sPathLoss * 1000);

            okumbuffersgeog = sitesgeog.STBuffer(sPathLoss * 1000);

            celltracing = dc_tmap.CellTracing.Where(e => e.cell == sCell).FirstOrDefault();

            //if (abc == null) return ngeo;

            celltracingsgeom = celltracing.SP_GEOMETRY;

            celltracingsgeog = SqlGeography.STGeomFromWKB(celltracingsgeom.STAsBinary(), 4326);

            Console.WriteLine(okumbuffersgeog.STArea());
            Console.WriteLine(celltracingsgeog.STArea());

            if (celltracingsgeog.STArea() > 0)
                return okumbuffersgeog.STIntersection(celltracingsgeog);   // 用圆和扇形相交
            else
                return okumbuffersgeog;    //返回圆

        }
    }
}
