using System;
using System.Collections.Generic;
using System.Text;

namespace SQLServerCLRTest
{
    public class CLRFunctions
    {
        public static string HelloWorld(string Name)
        {
            return ("Hello " + Name);
        }


        /*   在使用MySql时会遇到中文乱码的问题就此写下面两个函数   
                *   在写入数据库和从数据库读出时将编码改变   
                *   author：alice   
                *   date       ：2006/1/25   
              */
        //写入数据库时进行转换   
        public static string GB2312_ISO8859(string write)
        {
            //声明字符集   
            System.Text.Encoding iso8859, gb2312;
            //iso8859   
            iso8859 = System.Text.Encoding.GetEncoding("iso8859-1");
            //国标2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            byte[] gb;
            gb = gb2312.GetBytes(write);
            //返回转换后的字符   
            return iso8859.GetString(gb);
        }

        //读出时进行转换   
        public static string ISO8859_GB2312(string read)
        {
            //声明字符集   
            System.Text.Encoding iso8859, gb2312;
            //iso8859   
            iso8859 = System.Text.Encoding.GetEncoding("iso8859-1");
            //国标2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            byte[] iso;
            iso = iso8859.GetBytes(read);
            //返回转换后的字符   
            return gb2312.GetString(iso);
        }
    }

}
