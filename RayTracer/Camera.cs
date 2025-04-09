

namespace RayTracer
{
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using Vec3Color = RayTracer.Vec3;
    using Vec3Point = RayTracer.Vec3;
    internal class Camera
    {

        private int _imageHeight; // Render height in pixels
        private Vec3Point? _cameraCenter; // Camera center
        private Vec3Point? _pixel00Loc; // Location of 0, 0 pixel
        private Vec3? _pixelDeltaU; // Offset from pixel to pixel in the U(right) direction
        private Vec3? _pixelDeltaV; // Offset from pixel to pixel in the V(down) direction
        private double _pixelSamplesScale; // Scale for pixel samples
        public double AspectRatio { get; set; } = 16.0 / 9.0;
        public int ImageWidth { get; set; } = 600;
        public int SamplesForPixel { get; set; } = 10; // Number of samples per pixel

        public int MaxDepth { get; set; } = 10; // Maximum depth of recursion for ray tracing
        // output file name
        public string OutputFileName { get; set; } = "output.ppm";

        public void Render(Hittable world)
        {
            Initialize();
            using (StreamWriter outputFile = new(OutputFileName, false))
            {
                outputFile.WriteLine("P3\n" + ImageWidth + " " + _imageHeight + "\n255\n");
                for (int j = 0; j < _imageHeight; j++)
                {
                    Console.WriteLine("Scanlines remaining: " + (_imageHeight - j));

                    for (int i = 0; i < ImageWidth; i++)
                    {
                        Vec3Color pixelColor = new(0, 0, 0);
                        for (int sample = 0; sample < SamplesForPixel; sample++)
                        {
                            Ray r = GetRay(i, j);
                            pixelColor += RayColor(r, MaxDepth, world);
                        }

                        Color.WriteColor(outputFile, _pixelSamplesScale * pixelColor);

                    }
                }
                Console.WriteLine("Done.                    ");
            }
        }
        private void Initialize()
        {
            _imageHeight = (int)(ImageWidth / AspectRatio);
            _imageHeight = _imageHeight >= 1 ? _imageHeight : 1;

            _pixelSamplesScale = 1.0 / SamplesForPixel;
            // Camera 

            double focalLength = 1.0;
            double viewportHeight = 2.0;
            double viewportWidth = AspectRatio * viewportHeight;
            // Change the type of cameraCenter to Vec3 to match the type of other vectors
            _cameraCenter = new Vec3Point(0, 0, 0);

            // Calculate the vectors across the horizontal and down the vertical viewport edges.

            Vec3 viewportU = new(viewportWidth, 0, 0);
            Vec3 viewportV = new(0, -viewportHeight, 0);

            // Calculate the horizontal and vertical delta vectors from pixel to pixel.

            _pixelDeltaU = viewportU / (double)ImageWidth;
            _pixelDeltaV = viewportV / (double)_imageHeight;


            Vec3 focalVec = new(0, 0, focalLength);
            // Calculate the location of the upper left pixel.
            Vec3 viewportUpperLeft = _cameraCenter - focalVec - viewportU / 2 - viewportV / 2;
            _pixel00Loc = viewportUpperLeft + 0.5 * (_pixelDeltaU + _pixelDeltaV);
        }

        private Vec3Color RayColor(Ray ray, int depth, Hittable world)
        {
            if (depth <= 0)
            {
                return new Vec3Color(0, 0, 0);
            }

            HitRecord rec = new HitRecord();
            if (world.Hit(ray, new Interval(0.001, double.PositiveInfinity), ref rec))
            {
                Ray scattered = new Ray();
                Vec3Color attenuation = new Vec3Color(0, 0, 0);
                if (rec.Material.Scatter(ray, rec, ref attenuation, ref scattered))
                    return attenuation * RayColor(scattered, depth - 1, world);
                return new Vec3Color(0, 0, 0);
            }
            Vec3 unitDirection = Vec3.UnitVector(ray.direction);
            double a = 0.5 * (unitDirection.Y + 1.0);
            return ((1 - a) * new Vec3Color(1.0, 1.0, 1.0) + (a) * new Vec3Color(0.5, 0.7, 1.0));
        }

        private Ray GetRay(int i, int j)
        {
            Vec3 offset = SampleSquare();
            Vec3 pixelSample = _pixel00Loc 
                + ((i + offset.X) * _pixelDeltaU) 
                + ((j + offset.Y) * _pixelDeltaV);

            Vec3Point rayOrigin = _cameraCenter;
            Vec3 rayDirection = pixelSample - _cameraCenter;

            return new Ray(rayOrigin, rayDirection);
        }

        private Vec3 SampleSquare()
        {
            return new Vec3(RWUtils.RandomDouble() - 0.5, RWUtils.RandomDouble() - 0.5, 0);
        }


    }
}
