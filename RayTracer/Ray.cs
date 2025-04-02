using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    internal class Ray
    {
        public Vec3 origin { get; }
        public Vec3 direction { get; }
        public Ray() { }
        public Ray(Vec3 origin, Vec3 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }
  
        public Vec3 At(double t) => origin + t * direction;
    }
}
