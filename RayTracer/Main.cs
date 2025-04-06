using RayTracer;
using Vec3Color = RayTracer.Vec3;
using Vec3Point = RayTracer.Vec3;

const double PosInf = double.PositiveInfinity;

HittableList World = new();
World.Add(new Sphere(new Vec3Point(0, 0, -1), 0.5));
World.Add(new Sphere(new Vec3Point(0, -100.5, -1), 100));

Camera cam = new();
cam.AspectRatio = 16.0 / 9.0;
cam.ImageWidth = 400;
cam.SamplesForPixel = 100;
cam.MaxDepth = 50;

cam.Render(World);

