using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
using SqlServerToKml;
using System.Xml;

namespace TestSqlKml
{
    class Program
    {
        static void Main(string[] args)
        {
            // Simple console app test
            SqlGeography geog = SqlGeography.Parse("POINT EMPTY");
            string s = geog.AsKML("Name", "Description");
            Console.WriteLine(s);
        }
    }
}
