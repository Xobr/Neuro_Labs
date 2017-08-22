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
            double alpha = 1.0;
            return Math.Pow(1 + Math.Exp(-alpha * y), -1);
        }

        public static double _Teta = 0.03;
        public static double _Epselon = 0.1249;
    }
}
