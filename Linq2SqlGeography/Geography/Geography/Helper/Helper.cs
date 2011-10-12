///-----------------------------------
///作者: xgluxv@hotmail.com
///日期: 2010-8-12
///-----------------------------------

namespace Geography.CoordinateSystem
{

    ///-------------------------------------------
    ///两个辅助类 在内部使用
    ///-------------------------------------------
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    class LatZones
    {
        private char[] letters = { 'A', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K',
        'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Z' };

        private int[] degrees = { -90, -84, -72, -64, -56, -48, -40, -32, -24, -16,
        -8, 0, 8, 16, 24, 32, 40, 48, 56, 64, 72, 84 };

        private char[] negLetters = { 'A', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K',
        'L', 'M' };

        private int[] negDegrees = { -90, -84, -72, -64, -56, -48, -40, -32, -24,
        -16, -8 };

        private char[] posLetters = { 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W',
        'X', 'Z' };

        private int[] posDegrees = { 0, 8, 16, 24, 32, 40, 48, 56, 64, 72, 84 };

        private int arrayLength = 22;

        public LatZones()
        {
        }

        public int getLatZoneDegree(string letter)
        {
            char ltr = letter[0];
            for (int i = 0; i < arrayLength; i++)
            {
                if (letters[i] == ltr)
                {
                    return degrees[i];
                }
            }
            return -100;
        }

        public String getLatZone(double latitude)
        {
            int latIndex = -2;
            int lat = (int)latitude;

            if (lat >= 0)
            {
                int len = posLetters.Length;
                for (int i = 0; i < len; i++)
                {
                    if (lat == posDegrees[i])
                    {
                        latIndex = i;
                        break;
                    }

                    if (lat > posDegrees[i])
                    {
                        continue;
                    }
                    else
                    {
                        latIndex = i - 1;
                        break;
                    }
                }
            }
            else
            {
                int len = negLetters.Length;
                for (int i = 0; i < len; i++)
                {
                    if (lat == negDegrees[i])
                    {
                        latIndex = i;
                        break;
                    }

                    if (lat < negDegrees[i])
                    {
                        latIndex = i - 1;
                        break;
                    }
                    else
                    {
                        continue;
                    }

                }

            }

            if (latIndex == -1)
            {
                latIndex = 0;
            }
            if (lat >= 0)
            {
                if (latIndex == -2)
                {
                    latIndex = posLetters.Length - 1;
                }
                return Convert.ToString(posLetters[latIndex]);
            }
            else
            {
                if (latIndex == -2)
                {
                    latIndex = negLetters.Length - 1;
                }
                return Convert.ToString(negLetters[latIndex]);

            }
        }

    }
    class Digraphs
    {
        private IDictionary<int,string> digraph1;
        private IDictionary<int,string> digraph2 ;

        private string[] digraph1Array = { "A", "B", "C", "D", "E", "F", "G", "H",
        "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X",
        "Y", "Z" };

        private string[] digraph2Array = { "V", "A", "B", "C", "D", "E", "F", "G",
        "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V" };

        public Digraphs()
        {
            digraph1 = new Dictionary<int,string>();
            digraph2 = new Dictionary<int, string>();
            digraph1.Add(1, "A");
            digraph1.Add(2, "B");
            digraph1.Add(3, "C");
            digraph1.Add(4, "D");
            digraph1.Add(5, "E");
            digraph1.Add(6, "F");
            digraph1.Add(7, "G");
            digraph1.Add(8, "H");
            digraph1.Add(9, "J");
            digraph1.Add(10, "K");
            digraph1.Add(11, "L");
            digraph1.Add(12, "M");
            digraph1.Add(13, "N");
            digraph1.Add(14, "P");
            digraph1.Add(15, "Q");
            digraph1.Add(16, "R");
            digraph1.Add(17, "S");
            digraph1.Add(18, "T");
            digraph1.Add(19, "U");
            digraph1.Add(20, "V");
            digraph1.Add(21, "W");
            digraph1.Add(22, "X");
            digraph1.Add(23, "Y");
            digraph1.Add(24, "Z");

            digraph2.Add(0, "V");
            digraph2.Add(1, "A");
            digraph2.Add(2, "B");
            digraph2.Add(3, "C");
            digraph2.Add(4, "D");
            digraph2.Add(5, "E");
            digraph2.Add(6, "F");
            digraph2.Add(7, "G");
            digraph2.Add(8, "H");
            digraph2.Add(9, "J");
            digraph2.Add(10, "K");
            digraph2.Add(11, "L");
            digraph2.Add(12, "M");
            digraph2.Add(13, "N");
            digraph2.Add(14, "P");
            digraph2.Add(15, "Q");
            digraph2.Add(16, "R");
            digraph2.Add(17, "S");
            digraph2.Add(18, "T");
            digraph2.Add(19, "U");
            digraph2.Add(20, "V");

        }

        public int getDigraph1Index(string letter)
        {
            for (int i = 0; i < digraph1Array.Length; i++)
            {
                if (digraph1Array[i].Equals(letter))
                {
                    return i + 1;
                }
            }
            return -1;
        }

        public int getDigraph2Index(string letter)
        {
            for (int i = 0; i < digraph2Array.Length; i++)
            {
                if (digraph2Array[i].Equals(letter))
                {
                    return i;
                }
            }

            return -1;
        }

        public string getDigraph1(int longZone, double easting)
        {
            int a1 = longZone;
            double a2 = 8 * ((a1 - 1) % 3) + 1;

            double a3 = easting;
            double a4 = a2 + ((int)(a3 / 100000)) - 1;
            return digraph1[(int)Math.Floor(a4)];
        }

        public string getDigraph2(int longZone, double northing)
        {
            int a1 = longZone;
            double a2 = 1 + 5 * ((a1 - 1) % 2);
            double a3 = northing;
            double a4 = (a2 + ((int)(a3 / 100000)));
            a4 = (a2 + ((int)(a3 / 100000.0))) % 20;
            a4 = Math.Floor(a4);
            if (a4 < 0)
            {
                a4 = a4 + 19;
            }
            return digraph2[(int)Math.Floor(a4)];

        }

    }

}
