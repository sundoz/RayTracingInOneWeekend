using RayTracer;
using Vec3Color = RayTracer.Vec3;
using Vec3Point = RayTracer.Vec3;

const double PosInf = double.PositiveInfinity;

HittableList World = new();


void AddSpheres()
{
    Material groundMaterial = new Lambertian(new Vec3(0.5, 0.5, 0.5));
    World.Add(new Sphere(new Vec3Point(0, -1000, 0), 1000, groundMaterial));

    for(int a = -11; a < 11; a++)
    {
        for( int b = -11; b< 11; b++)
        {
            double chooseMaterial = RWUtils.RandomDouble();
            Vec3Point center = new Vec3Point(a + 0.9 * RWUtils.RandomDouble(), 0.2, b + 0.9 * RWUtils.RandomDouble());
            if ((center - new Vec3Point(4, 0.2, 0)).Length > 0.9)
            {
                Material sphereMaterial;
                if (chooseMaterial < 0.8)
                {
                    // diffuse
                    Vec3 albedo = Vec3.Random() * Vec3.Random();
                    sphereMaterial = new Lambertian(albedo);
                    World.Add(new Sphere(center, 0.2, sphereMaterial));
                }
                else if (chooseMaterial < 0.95)
                {
                    // metal
                    Vec3 albedo = Vec3.Random(0.5, 1);
                    double fuzz = RWUtils.RandomDouble(0, 0.5);
                    sphereMaterial = new Metal(albedo, fuzz);
                    World.Add(new Sphere(center, 0.2, sphereMaterial));
                }
                else
                {
                    // glass
                    sphereMaterial = new Dielectric(1.5);
                    World.Add(new Sphere(center, 0.2, sphereMaterial));
                }
            }
        }
        Material material1 = new Dielectric(1.5);
        Material material2 = new Metal(new Vec3(0.7, 0.6, 0.5), 0);
        Material material3 = new Lambertian(new Vec3(0.4, 0.2, 0.1));
        World.Add(new Sphere(new Vec3Point(0, 1, 0), 1.0, material1));
        World.Add(new Sphere(new Vec3Point(-4, 1, 0), 1.0, material3));
        World.Add(new Sphere(new Vec3Point(4, 1, 0), 1.0, material2));
    }
}
AddSpheres();
Camera cam = new();
cam.AspectRatio = 16.0 / 9.0;
cam.ImageWidth = 600;
cam.SamplesForPixel = 150;
cam.MaxDepth = 50;

cam.VFov = 35;
cam.LookFrom = new Vec3(13, 5, 3);
cam.LookAt = new Vec3(0, 0, 0);
cam.VUP = new Vec3(0, 5, 0);

cam.DefocusAngle = 0.6;
cam.FocusDistance = 10;

cam.Render(World);



