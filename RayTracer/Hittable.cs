using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vec3Point = RayTracer.Vec3;
namespace RayTracer
{
    class HitRecord
    {
        public Vec3Point Point;
        public Vec3 Normal;
        public double T;
        public bool FrontFace;

        public void SetFaceNormal(Ray r, Vec3 outwardNormal)
        {
            this.FrontFace = Vec3.Dot(r.direction, outwardNormal) < 0;
            this.Normal = FrontFace ? outwardNormal : -outwardNormal;
        }
    }
    interface Hittable
    {
        public bool Hit(Ray r, Interval rayT, ref HitRecord rec);

    }
}
