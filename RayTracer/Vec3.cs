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
        public double Length { get; }
        public double LengthSquared { get; }
        
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

        public static Vec3 operator -(Vec3 e) => new Vec3(-e.e0, -e.e1, -e.e2);

        public static Vec3 operator -(Vec3 a, Vec3 b) => new Vec3(a.e0 - b.e0, a.e1 - b.e1, a.e2 - b.e2);

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

        public double LengthSquared => e0 * e0 + e1 * e1 + e2 * e2;

        public double Length => Math.Sqrt(LengthSquared);

        public static double Dot(Vec3 u, Vec3 v) => u.e0 * v.e0 + u.e1 * v.e1 + u.e2 * v.e2;

        public static Vec3 Cross(Vec3 u, Vec3 v)
        {
            return new Vec3(u.e1 * v.e2 - u.e2 * v.e1,
                            u.e2 * v.e0 - u.e0 * v.e2,
                            u.e0 * v.e1 - u.e1 * v.e0);
        }

        public static Vec3 UnitVector(Vec3 v) => v / v.Length;

        public override string ToString()
        {
            return $"{e0} {e1} {e2}";
        }

        public static Vec3 Random()
        {
            return new Vec3(RWUtils.RandomDouble(), RWUtils.RandomDouble(), RWUtils.RandomDouble());
        }

        public static Vec3 Random(double min, double max)
        {
            return new Vec3(RWUtils.RandomDouble(min, max), RWUtils.RandomDouble(min, max), RWUtils.RandomDouble(min, max));
        }

        public static Vec3 RandomUnitVector()
        {
            while (true)
            {
                Vec3 p = Vec3.Random(-1, 1);
                double lensq = p.LengthSquared;
                if (1e-160 < lensq && lensq >= 1)
                {
                    return p / Math.Sqrt(lensq);
                }
            }
        }

        public static Vec3 RanodomOnHemisphere(Vec3 normal)
        {
            Vec3 onUnitSphere = Vec3.RandomUnitVector();

            if (Vec3.Dot(onUnitSphere, normal) > 0)
            {
                return onUnitSphere;
            }
            else
            {
                return -onUnitSphere;
            }
        }
    }

}


