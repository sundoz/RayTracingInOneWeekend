using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    internal class RWUtils
    {
        public static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        public static double RandomDouble()
        {
            return new Random().NextDouble();
        }

        public static double RandomDouble(double min, double max)
        {
            return min + (max - min) * RandomDouble();
        }
    }
}
