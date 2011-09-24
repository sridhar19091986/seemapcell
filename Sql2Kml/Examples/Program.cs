using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using Google.KML;

namespace Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            string exampleFileName = "";
            string exampleFileExt = "kml";
            geKML kml;

            Console.Write("Please type the number of example that you would like to run: ");
            string example = Console.ReadLine();
            Console.Write("Would you like to use compression? (.kmz): [y/n]");
            string kmz = Console.ReadLine();
            

            if (kmz.ToUpper() == "Y")
            {
                exampleFileExt = "kmz";
            }

            exampleFileName = "C:\\ge-kml_Example-" + example + "." + exampleFileExt;

            switch (example)
            {
                case "1":
                    kml = Example1.RunExample(exampleFileName);
                    break;
                case "2":
                    kml = Example2.RunExample(exampleFileName);
                    break;
                case "3":
                    kml = Example3.RunExample(exampleFileName);
                    break;
                case "4":
                    kml = Example4.RunExample(exampleFileName);
                    break;
                case "5":
                    kml = Example5.RunExample(exampleFileName);
                    break;
                case "6":
                    kml = Example6.RunExample(exampleFileName);
                    break;
                default:
                    throw new Exception("Invalid Example Number");
            }

            if (exampleFileExt == "kml")
            {
                File.WriteAllBytes(exampleFileName, kml.ToKML());
                ProcessStartInfo psInfNotepad = new ProcessStartInfo("notepad.exe", exampleFileName);
                Process.Start(psInfNotepad);
            }
            else
            {
                File.WriteAllBytes(exampleFileName,kml.ToKMZ());
            }


            ProcessStartInfo psInfGE = new ProcessStartInfo(@"C:\Program Files\Google\Google Earth\client\googleearth.exe", exampleFileName);
            
            Process.Start(psInfGE);
   

        }
    }
}
