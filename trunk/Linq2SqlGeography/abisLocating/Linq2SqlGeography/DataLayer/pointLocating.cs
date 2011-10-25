using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
using Linq2SqlGeography.LinqSql;
using System.Text.RegularExpressions;

namespace Linq2SqlGeography
{
    public class pointLocating
    {

        private int pencolor = 0;

        private int redindex = 1;
        private int greenindex = 1;      
        private int blueindex = 1;

        private IEnumerable<Abis_MR> abis_mrr;
        private string events;
        private string pen;
        //private SqlGeography sgeog;
        private SqlGeometry sgeom;
        private string sql;
        private DataClasses2DataContext dc = new DataClasses2DataContext();


        public pointLocating()
        {
            getLocating();
        }
        private void getLocating()
        {

            dc.ExecuteCommand(HandleTable.createEventLocating);

            HandleNeighbour handleNeigh = new HandleNeighbour();
            List<mrNeighbour> mrNeighsNew = new List<mrNeighbour>();

            abis_mrr = dc.Abis_MR.Where(e=>e.cell.Length>2).Where(e => e.bsic5 > 0);

            foreach (var mr in abis_mrr)
            {
                mrNeighsNew = handleNeigh.getNeighList(handleNeigh.setNeighList(mr), true);
                mrLocating mrlocating = new mrLocating();
                //pencolor = HandleTable.getRandomPenColor(false,false,false);
                SqlGeography tempgeog = new SqlGeography();

                //以时间点做索引比较妥当？
                if (mr.Measurement_Report_time != null)
                    events = mr.Measurement_Report_time.ToString();

                foreach (var n in mrNeighsNew)
                {
                    if (n.nBaIndex == -1 && n.nBSIC == -1)
                    {
                        //服务小区用红色标识吧！
                        // Red: 16711680
                        pen = "Pen (1, 2,16711680)";

                        tempgeog = mrlocating.sLocating(n.ServiceCell, n.rxLev, n.powerControl); //服务小区
                        insertLocatingGeo(events, pen, tempgeog);

                        Console.WriteLine("服务小区：{0}...{1}...{2}", n.ServiceCell, n.rxLev, n.powerControl);

                    }
                    else
                    {

                        greenindex++;
                        if (greenindex > 255) greenindex = 0;
                        pencolor = redindex * 65535 + greenindex * 256 + blueindex;
                        pen = "Pen (1, 2," + pencolor.ToString() + ")";

                        if (n.NeighCell != null)
                        {
                            tempgeog = mrlocating.nLocating(n.NeighCell, n.rxLev, n.powerControl); //邻小区
                            insertLocatingGeo(events, pen, tempgeog);
                        }
                        Console.WriteLine("邻小区：{0}...{1}...{2}", n.NeighCell, n.rxLev, n.powerControl);
                    }
                }

            }
        }

        private void insertLocatingGeo(string events, string pen, SqlGeography sgeog)
        {
            sgeom = SqlGeometry.STGeomFromWKB(sgeog.STAsBinary(), 4326);
            sql = @" INSERT INTO [EventLocating]([events],[MI_STYLE],[SP_GEOMETRY]) VALUES  ('"
                + events + "','" + pen + "','" + sgeom + "')";
            dc.ExecuteCommand(sql);
        }
    }
}
