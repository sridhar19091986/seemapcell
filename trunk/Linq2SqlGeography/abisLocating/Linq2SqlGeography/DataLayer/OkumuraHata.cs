using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linq2SqlGeography
{
    public class OkumuraHata
    {
        public bool CityType = true;
        private double frequency;       
        //f(MHz):工作频率,  hb(m):基站天线有效高度, 
        //hm(m):移动台天线高度, d(km):收发天线之间的距离 

        //逆向计算，采用迭代算法
        public double PathLoss2Distance(double f, double hb, double hm, double pl)
        {
            double distance = 0;
            double lb = 0;
            while (lb < pl)
            {
                lb = City_Lb(f, hb, hm, distance);
                distance += 0.01;
                //Console.WriteLine("{0}...", distance);
            }
            return distance;
        }

        //正向计算，oh模型
        public double City_Lb(double f, double hb, double hm, double d)//市区传播损耗中值函数，单位为dB 
        {
            this.frequency = f;  //在这个中间函数给频率赋值
            return 69.55 + 26.16 * Math.Log10(f) - 13.82 * Math.Log10(hb) 
                - Modi_Factor(hm) + (44.9 - 6.55 * Math.Log10(hb)) * Math.Log10(d);
        }
        public double Suburb_Lb(double f, double hb, double hm, double d)//郊区传播损耗函数 
        {
            return City_Lb(f, hb, hm, d) - 2 * Math.Pow(Math.Log10(f / 28), 2) - 5.4;
        }
        public double Width_Lb(double f, double hb, double hm, double d)//开阔去传播损耗函数 
        {
            return City_Lb(f, hb, hm, d) - 4.78 * Math.Pow(Math.Log10(f), 2) - 18.33 * Math.Log10(f) - 40.98;
        }
        double Modi_Factor(double hm)//移动台天线高度校正因子，单位（dB） 
        {
            if (CityType)  //大城市 
            {
                if (frequency < 300)
                {
                    return 8.29 * Math.Pow(Math.Log10(1.54 * hm), 2) - 1.1;
                }
                else
                    return 3.2 * Math.Pow(Math.Log10(11.75 * hm), 2) - 4.97;
            }
            else
                return (1.1 * Math.Log10(frequency) - 0.7) * hm - 1.56 * Math.Log10(frequency) + 0.8;
        }
    }
}
