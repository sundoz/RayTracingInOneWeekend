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

        public Sphere(Vec3 center, double radius)
        {
            this.center = center;
            this.radius = Math.Max(radius, 0);
        }

        public bool Hit(Ray r, double rayTMin, double rayTMax, ref Hit_record rec)
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
            if (root <= rayTMin || root >= rayTMax)
            {
                root = (h + sqrtd) / a;
                if  (root <= rayTMin || root >= rayTMax)
                {
                    return false;
                }
            }

            rec.T = root;
            rec.Point = r.At(rec.T);
            rec.Normal = (rec.Point - center) / radius;
            return true;
        }
    }
}
