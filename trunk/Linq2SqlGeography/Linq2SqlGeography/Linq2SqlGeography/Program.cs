using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linq2SqlGeography;
using Microsoft.SqlServer.Types;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace Linq2SqlGeography
{

    class Program
    {
        static void Main(string[] args)
        {
            test();
            getSectorCoverage();
            Console.ReadKey();
        }
        private static string crtab
            = @"

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CELLTRACING]') AND type in (N'U'))
DROP TABLE [dbo].[CELLTRACING]
;
CREATE TABLE [dbo].[CELLTRACING](
	[idx] [int] NULL,
	[SiteName] [varchar](30) NULL,
	[MI_STYLE] [varchar](254) NULL,
	[MI_PRINX] [int] IDENTITY(1,1) NOT NULL,
	[SP_GEOMETRY] [geometry] NULL,
PRIMARY KEY CLUSTERED 
(
	[MI_PRINX] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


";

        private static void getSectorCoverage()
        {
            DataClasses2DataContext dc = new DataClasses2DataContext();
            //dc.ExecuteCommand("delete  from  CELLTRACING");
            //HandleTable.CreateTable(typeof(CELLTRACING));
            dc.ExecuteCommand(crtab);
            //Console.ReadKey();

            Console.WriteLine(dc.SITE.Count());
            foreach (var site in dc.SITE)
            {
                CellCoverage cc = new CellCoverage();
                var sgeo = cc.MergePoint(site);
                SqlGeometry mgeo = SqlGeometry.STGeomFromWKB(sgeo.STAsBinary(), 4326).STConvexHull();

                Console.WriteLine("SqlGeometry   {0}        mgeo.STEnvelope();",
                    mgeo.STAsBinary());

                CELLTRACING ct = new CELLTRACING();
                ct.SiteName = site.cell_name;
                ct.MI_STYLE = "Red";
                ct.SP_GEOMETRY = mgeo;
                //ct.Latitude = site.Latitude;
                //ct.Longitude = site.Longitude;


                string sql = @" INSERT INTO [CELLTRACING]
           ([SiteName],[MI_STYLE],[SP_GEOMETRY]) VALUES  ('"
                    + ct.SiteName+ "','" + ct.MI_STYLE + "','" + ct.SP_GEOMETRY+ "')";

                dc.ExecuteCommand(sql);

                //dc.CELLTRACING.InsertOnSubmit(ct);
                // dc.SubmitChanges();
            }

        }

        private static void test()
        {
            var g = SqlGeometry.STGeomFromText(new SqlChars("MULTIPOINT(0 0, 13.5 2, 7 19)"), 0);
            Console.WriteLine(g.InstanceOf("GEOMETRYCOLLECTION"));
        }


    }
}
