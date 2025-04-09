using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    internal class Sphere : Hittable

    {
        private Vec3 center;
        private double radius;
        private Material material;

        public Sphere(Vec3 center, double radius, Material material)
        {
            this.center = center;
            this.radius = Math.Max(radius, 0);
            this.material = material;
        }

        public bool Hit(Ray r, Interval rayT, ref HitRecord rec)
        {
            Vec3 oc = center - r.origin;
            double a = r.direction.LengthSquared;
            double h = Vec3.Dot(r.direction, oc);
            double c = oc.LengthSquared - radius * radius;
            double discriminant = h * h - a * c;

            if (discriminant < 0)
            {
                return false;
            }
            
            double sqrtd = Math.Sqrt(discriminant);

            // Find the nearest root that lies in the acceptable range.

            double root = (h - sqrtd) / a;
            if (!rayT.Surrounds(root))
            {
                root = (h + sqrtd) / a;
                if  (!rayT.Surrounds(root))
                {
                    return false;
                }
            }

            rec.T = root;
            rec.Point = r.At(rec.T);
            Vec3 outwardNormal = (rec.Point - center) / radius;
            rec.SetFaceNormal(r, outwardNormal);
            rec.Material = material;

            return true;
        }
    }
}
