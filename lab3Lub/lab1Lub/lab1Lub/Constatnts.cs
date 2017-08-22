using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1Lub
{
    class Constatnts
    {
        public static double ActivationFunk(double y)
        {
            double alpha = 0.5;
            //return Math.Pow(1 + Math.Exp(-alpha * y), -1);
            return (2 * Math.Pow(1 + Math.Exp(-y * alpha), -1)) - 1; 
        }
        public static double F1(double x)
        {
            return Math.Exp(x); 
        } 
        public static lab1Lub.Point[] GetArray(double start, double end, double step)
        {
            var res = new List<lab1Lub.Point>();
            double current = start; 
            while (current <= end)
            {
                res.Add(new lab1Lub.Point(current,F1(current)));
                current += step; 
            } 
            return res.ToArray(); 
        }

        public static double TransformerAlong(double x)
        {
            return ((x - _XMin) * (b - a) / (_XMax - _XMin)) + a; 
        }
        public static double TransformerBack(double x)
        {
            return ((x - a) * (_XMax - _XMin) / (b - a)) + _XMin; 
        }
        public static double GetPercError(double p, double real)
        {
            return Math.Abs(p - real) / real; 
        }
        private static double a = 0;
        public static double b = 1; 
        public static double _Teta = 0.01;
        public static double _Epselon = 0.01;
        public static double _XMax;
        public static double _XMin; 
    }
}
