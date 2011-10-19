using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Linq2SqlGeography
{
    public class Text2Class
    {
        public List<CellBA> CellBaList;
        public Text2Class()
        {
            CellBaList = new List<CellBA>();
            StreamRead2Class();
        }

        private string linestring;

        private void StreamRead2Class()
        {
            StreamReader sr = new StreamReader(@"D:\RLMFP");
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (!line.StartsWith("CELL"))
                {
                    Regex r = new Regex(@"\s+");
                    linestring = r.Replace(line, @" ");
                    var ln = linestring.Split(new Char[] { ' ' });
                    CellBA cb = new CellBA();
                    cb.cell = ln[0];
                    cb.mode = ln[1];
                    for (int i = 2; i < ln.Length; i++)
                        if (ln[i].Trim().Length > 0)
                            cb.ba.Add(ln[i].Trim());
                    //Console.WriteLine(cb.cell);
                    CellBaList.Add(cb);
                }
            }
        }
    }
    public class CellBA
    {
        public string cell;
        public string mode;
        public List<string> ba =new List<string>();
    }
}

