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
    public class LiJiaoSite
    {
        public 立交ABC lj { get; set; }
        public MCOMSITE site { get; set; }
    }
}
