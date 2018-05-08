using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirSimApp
{
    public static class Mod
    {
        public static int CanonicalModulo(int dividend, int divisor)
        {
            int temp = dividend % divisor;
            return temp < 0 ? temp + divisor : temp;
        }

        public static float CanonicalModulo(float dividend, float divisor)
        {
            float temp = dividend % divisor;
            return temp < 0 ? temp + divisor : temp;
        }

        public static double CanonicalModulo(double dividend, double divisor)
        {
            double temp = dividend % divisor;
            return temp < 0 ? temp + divisor : temp;
        }
    }
}
