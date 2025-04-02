using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer;

namespace RayTracer
{

    interface IVec3
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }
        public double length { get; }
        public double lengthSquared { get; }
        
    }
    internal class Vec3 : IVec3
    {
        public double e0, e1, e2;
        public Vec3() => e0 = e1 = e2 = 0;
        public Vec3(double e0, double e1, double e2)
        {
            this.e0 = e0;
            this.e1 = e1;
            this.e2 = e2;
        }

        public double X => e0;
        public double Y => e1;
        public double Z => e2;

        public static Vec3 operator -(Vec3 e) => new Vec3(-e.X, -e.Y, -e.Z);

        public static Vec3 operator -(Vec3 a, Vec3 b) => new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public double this[int i]
        {
            get
            {
                return i switch
                {
                    0 => e0,
                    1 => e1,
                    2 => e2,
                    _ => throw new IndexOutOfRangeException("Invalid index")
                };
            }
        }

        public static Vec3 operator +(Vec3 a, Vec3 b)
        { 
            return new Vec3(a.e0 + b.e0, a.e1 + b.e1, a.e2 + b.e2);
        }

        public static Vec3 operator *(Vec3 a, double t)
        { 
            return new Vec3(a.e0 * t, a.e1 * t, a.e2 * t);
        }

        public static Vec3 operator *(Vec3 a, Vec3 b)
        {
            return new Vec3(a.e0 * b.e0, a.e1 * b.e1, a.e2 * b.e2);
        }
        public static Vec3 operator *(double t, Vec3 a) => a * t;

        public static Vec3 operator /(Vec3 a, double t) => a * (1 / t);

        public double lengthSquared => e0 * e0 + e1 * e1 + e2 * e2;

        public double length => Math.Sqrt(lengthSquared);

        public double dot(Vec3 u, Vec3 v) => u.e0 * v.e0 + u.e1 * v.e1 + u.e2 * v.e2;

        public Vec3 cross(Vec3 u, Vec3 v)
        {
            return new Vec3(u.e1 * v.e2 - u.e2 * v.e1,
                            u.e2 * v.e0 - u.e0 * v.e2,
                            u.e0 * v.e1 - u.e1 * v.e0);
        }

        public static Vec3 unitVector(Vec3 v) => v / v.length;

    }

}


