using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vec3Color = RayTracer.Vec3;
namespace RayTracer
{
    abstract class Material
    {
        public virtual bool Scatter(Ray rayIn, HitRecord rec, ref Vec3 attenuation, ref Ray scattered)
        {
            return false;
        }
    }
    class Lambertian : Material
    {
        private Vec3Color albedo;
        public Lambertian(Vec3 albedo)
        {
            this.albedo = albedo;
        }
        public override bool Scatter(Ray rayIn, HitRecord rec, ref Vec3 attenuation, ref Ray scattered)
        {
            Vec3 scatterDirection = rec.Normal + Vec3.RandomUnitVector();
            if (scatterDirection.NearZero())
            {
                // If the scatter direction is near zero, use the normal as the scatter direction.
                scatterDirection = rec.Normal;
            }
            scattered = new Ray(rec.Point, scatterDirection);
            attenuation = albedo;
            return true;
        }
    }

    class Metal : Material
    {
        private Vec3Color albedo;
        private double fuzz;
        public Metal(Vec3Color albedo, double fuzz)
        {
            this.albedo = albedo;
            this.fuzz = fuzz < 1 ? fuzz : 1;
        }

        public override bool Scatter(Ray rayIn, HitRecord rec, ref Vec3Color attenuation, ref Ray scattered)
        {
            Vec3 reflected = Vec3.Reflect(rayIn.direction, rec.Normal);
            reflected = Vec3.UnitVector(reflected) + (fuzz * Vec3.RandomUnitVector());
            scattered = new Ray(rec.Point, reflected);
            attenuation = albedo;
            return (Vec3.Dot(scattered.direction, rec.Normal) > 0);
        }
    }

    class Dielectric : Material
    {
        private double refractionIndex;

        public Dielectric(double refractionIndex)
        {
            this.refractionIndex = refractionIndex;
        }
        public override bool Scatter(Ray rayIn, HitRecord rec, ref Vec3Color attenuation, ref Ray scattered)
        {
            attenuation = new Vec3Color(1.0, 1.0, 1.0);
            double ri = rec.FrontFace ? (1.0 / refractionIndex) : refractionIndex;
            Vec3 unitDirection = Vec3.UnitVector(rayIn.direction);
            Vec3 refracted = Vec3.Refract(unitDirection, rec.Normal, ri);

            scattered = new Ray(rec.Point, refracted);
            return true;
        }
    }
}
