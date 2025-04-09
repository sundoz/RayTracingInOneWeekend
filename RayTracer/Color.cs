using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
   
{
    partial class Color
    {
        public static double LinearToGamma(double linearComponent)
        {
            if (linearComponent > 0)
            {
                return Math.Sqrt(linearComponent);
            }
            return 0;
        }
        public static void WriteColor(StreamWriter streamWriter, Vec3 pixelColor)
        {
            Interval intensity = new(0.000, 0.999);

            int rbyte = (int)(256 * intensity.Clamp(LinearToGamma(pixelColor.X)));
            int gbyte = (int)(256 * intensity.Clamp(LinearToGamma(pixelColor.Y)));
            int bbyte = (int)(256 * intensity.Clamp(LinearToGamma(pixelColor.Z)));
            streamWriter.WriteLine(rbyte + " " + gbyte + " " + bbyte);
        }
    }
}
