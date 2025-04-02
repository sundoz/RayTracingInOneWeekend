using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vec3Point = RayTracer.Vec3;
namespace RayTracer
{
    class Hit_record
    {
        public Vec3Point Point;
        public Vec3 Normal;
        public double T;
    }
    interface Hittable
    {
        public bool Hit(Ray r, double t_min, double t_max, ref Hit_record rec);
    }
}
