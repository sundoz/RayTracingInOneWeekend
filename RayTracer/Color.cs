using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
   
{
    partial class Color
    {
        public static void WriteColor(StreamWriter streamWriter, Vec3 pixelColor)
        {
            Interval intensity = new(0.000, 0.999);

            int rbyte = (int)(256 * intensity.Clamp(pixelColor.X));
            int gbyte = (int)(256 * intensity.Clamp(pixelColor.Y));
            int bbyte = (int)(256 * intensity.Clamp(pixelColor.Z));
            streamWriter.WriteLine(rbyte + " " + gbyte + " " + bbyte);
        }
    }
}
