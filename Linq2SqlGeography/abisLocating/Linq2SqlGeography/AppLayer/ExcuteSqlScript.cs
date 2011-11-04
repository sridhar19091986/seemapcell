using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
//using Linq2SqlGeography.LinqSql;
using System.Text.RegularExpressions;
//using Linq2SqlGeography.LinqSql.FromAbis;
//using Linq2SqlGeography.LinqSql.FromMap;
//using Linq2SqlGeography.LinqSql.FromOSS;
using Linq2SqlGeography.LinqSql.ToMap;

namespace Linq2SqlGeography
{
    public class ExcuteSqlScript
    {
        private DataClasses1DataContext dc = new DataClasses1DataContext();

        public ExcuteSqlScript()
        {
        }

        public void insertSqltableIntoMapinfo(string tablename)
        {
            string sql = @"
    
INSERT INTO [MAPINFO].[MAPINFO_MAPCATALOG] 
    ([SPATIALTYPE], [TABLENAME], [OWNERNAME], [SPATIALCOLUMN], [DB_X_LL], [DB_Y_LL], 
    [DB_X_UR], [DB_Y_UR], [VIEW_X_LL], [VIEW_Y_LL], [VIEW_X_UR], [VIEW_Y_UR], [COORDINATESYSTEM],
    [SYMBOL], [XCOLUMNNAME], [YCOLUMNNAME], [RENDITIONTYPE], [RENDITIONCOLUMN], [RENDITIONTABLE], [NUMBER_ROWS])
VALUES (17.1, N'" + tablename + @"', N'dbo', N'SP_GEOMETRY', 112.033017, 21.595985, 113.250913, 22.855645, 112.695674, 
    22.338406, 112.719924, 22.350238, N'Earth Projection 1, 104', N'Pen (1, 60, 0)', N'NO_COLUMN', N'NO_COLUMN',
    1, N'MI_STYLE', N'', NULL)
";
            dc.ExecuteCommand(sql);
        }

        public void createLocatingTable(string tablename)
        {
            string sql = @"

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + tablename + @"]') AND type in (N'U'))
DROP TABLE [dbo].[" + tablename + @"]
;
CREATE TABLE [dbo].[" + tablename + @"](
	[events] [varchar](30) NULL,
	[MI_STYLE] [varchar](254) NULL,
	[MI_PRINX] [int] IDENTITY(1,1) NOT NULL,
	[SP_GEOMETRY] [geometry] NULL,
PRIMARY KEY CLUSTERED 
(
	[MI_PRINX] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
";
            dc.ExecuteCommand(sql);
        }
    }
}
