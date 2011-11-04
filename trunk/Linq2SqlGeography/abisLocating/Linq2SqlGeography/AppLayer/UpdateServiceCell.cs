using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
using Linq2SqlGeography.LinqSql.FromAbis;
using System.Text.RegularExpressions;

namespace Linq2SqlGeography
{
    public class UpdateServiceCell
    {
        //private ILookup<string, MCOMCARRIER> Carriers;
        private DataClasses1DataContext dc = new DataClasses1DataContext();
        private string ServiceCell = "";
        private string TargetCell = "";
        private string sql = "";
        private IEnumerable<Abis_Ho> abisho;
        private IEnumerable<Abis_MR> abismr;
        private Abis_Ho targetho;
        //private MCOMCARRIER carr;
        private List<DateTime?> hotimes;
        private int timeslen;
        private IEnumerable<int?> filenums;
        private string neighcell;

        public UpdateServiceCell(string ServiceCell)
        {
            this.ServiceCell = ServiceCell;
            //Carriers = dc.MCOMCARRIER.ToLookup(e => e.BCCH.Value.ToString() + "-" + e.BSIC.Trim());
            updateServiceCell();
        }

        public string getServiceCell(string servicecell, string bcch, string BSIC)
        {
            HandleNeighbour hn = new HandleNeighbour();
            neighcell = hn.getNeighCell(servicecell, bcch, BSIC);
            //carr = Carriers[bcch + "-" + BSIC].FirstOrDefault();
            return neighcell;
        }

        public void updateServiceCell()
        {
            filenums = dc.Abis_MR.Select(e => e.FileNum).Distinct().OrderBy(e=>e.Value);
            foreach (int f in filenums)
            {
                updateServiceCell(f); 
                Console.WriteLine(f); Console.WriteLine("....................................");
            }
        }

        public void updateServiceCell(int filenum)
        {
            abisho = dc.Abis_Ho.Where(e => e.FileNum == filenum);
            abismr = dc.Abis_MR.Where(e => e.FileNum == filenum);
            hotimes = dc.Abis_Ho.Where(e => e.FileNum == filenum).Select(e => e.PacketTime).Distinct().OrderBy(e => e.Value).ToList();
            hotimes.Add(DateTime.Now.AddDays(100));
            timeslen = hotimes.Count();
           
            if (timeslen == 1)
            {
                sql = "UPDATE [SqlSpatialJiangmeng].[dbo].[Abis_MR] SET [cell] = '" + ServiceCell + "'"
                      + "  where [FileNum] = " + filenum;
                dc.ExecuteCommand(sql);
                Console.WriteLine("没有发生切换...{0}", sql);
            }
            else
            {
                sql = @"UPDATE [SqlSpatialJiangmeng].[dbo].[Abis_MR] SET [cell] = '" + ServiceCell
                        + "'  where [FileNum] = " + filenum
                        + "   and   [PacketTime] <=  '" + hotimes[0] + "'";
                dc.ExecuteCommand(sql);
                for (int i = 0; i < timeslen - 1; i++)
                {
                    targetho = abisho.Where(e => e.PacketTime == hotimes[i]).FirstOrDefault();
                    TargetCell = getServiceCell(ServiceCell, targetho.bcch_arfcn, targetho.ncc + targetho.bcc);
                    Console.WriteLine("测试邻小区...{0}...{1}....{2}", ServiceCell, TargetCell, filenum);
                    if (TargetCell == null) continue;

                    if (timeslen == 2)
                    {
                        sql = @"UPDATE [SqlSpatialJiangmeng].[dbo].[Abis_MR] SET [cell] = '" + TargetCell
                               + "'  where [FileNum] = " + filenum
                               + "   and   [PacketTime] >= '" + hotimes[i] + "'";
                        dc.ExecuteCommand(sql);
                    }
                    else
                    {
                        sql = @"UPDATE [SqlSpatialJiangmeng].[dbo].[Abis_MR] SET [cell] = '" + TargetCell
                                + "'  where [FileNum] = " + filenum
                                + "   and   [PacketTime] >= '" + hotimes[i] + "'"
                                + "   and   [PacketTime]  <= '" + hotimes[i + 1] + "'";
                        dc.ExecuteCommand(sql);
                        Console.WriteLine(sql);
                        Console.WriteLine("发生切换...{0}...{1}....{2}", ServiceCell, TargetCell,filenum);
                        ServiceCell = TargetCell ;
                    }   
                }
            }
        }
    }
}
