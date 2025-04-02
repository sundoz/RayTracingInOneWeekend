using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
   
{
    partial class Utils
    {
        public static void WriteColor(StreamWriter streamWriter, Vec3 pixelColor)
        {
            int ir = (int)(255.999 * pixelColor.X);
            int ig = (int)(255.999 * pixelColor.Y);
            int ib = (int)(255.999 * pixelColor.Z);
            streamWriter.WriteLine(ir + " " + ig + " " + ib);
        }
    }
}
