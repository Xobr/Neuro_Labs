using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1Lub
{
    class Constatnts
    {
        public static Random r = new Random(); 
        public static double ActivationFunk(double y)
        {
            double alpha = 1.0;
            return Math.Pow(1 + Math.Exp(-alpha * y), -1);
        }

        public static double _Teta = 0.1;
        public static double _Epselon = 0.1;

        public static double[] Sigma = new double[] { 0.00078125, 0.00234375 };

        public static double GetDouble(int start,int end)
        {
            int n = 1000;
            int st = start * n;
            int fn = end * n; 
            double res = ((r.Next(st, fn) / (double)n)); 
            return res; 
        }

        public static double GetE(double[] X, double[] T)
        {
            double sum = 0;
            for (int i = 0; i < X.Length; i++)
            {
                sum += Math.Pow((X[i] - T[i]), 2); 
            }
            return sum / 2d; 
        }

    }
}
