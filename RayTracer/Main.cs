using RayTracer;
using Vec3Color = RayTracer.Vec3;
using Vec3Point = RayTracer.Vec3;

const double PosInf = double.PositiveInfinity;

HittableList World = new();

Material materialGround = new Lambertian(new Vec3Color(0.8, 0.8, 0));
Material materialCenter = new Lambertian(new Vec3Color(0.1, 0.2, 0.5));
Material materialLeft = new Dielectric(1.50);
Material materialRight = new Metal(new Vec3Color(0.8, 0.6, 0.2), 1);

World.Add(new Sphere(new Vec3Point(0, -100.5, -1), 100, materialGround));
World.Add(new Sphere(new Vec3Point(0, 0, -1), 0.5, materialCenter));
World.Add(new Sphere(new Vec3Point(-1, 0, -1), 0.5, materialLeft));
World.Add(new Sphere(new Vec3Point(1, 0, -1), 0.5, materialRight));

Camera cam = new();
cam.AspectRatio = 16.0 / 9.0;
cam.ImageWidth = 600;
cam.SamplesForPixel = 50;
cam.MaxDepth = 10;

cam.Render(World);



