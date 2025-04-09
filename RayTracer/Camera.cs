

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
        private Vec3 _u;
        private Vec3 _v;
        private Vec3 _w;
        private Vec3 _defocusDiskU;
        private Vec3 _defocusDiskV;

        public double AspectRatio { get; set; } = 16.0 / 9.0;
        public int ImageWidth { get; set; } = 600;
        public int SamplesForPixel { get; set; } = 10; // Number of samples per pixel

        public int MaxDepth { get; set; } = 10; // Maximum depth of recursion for ray tracing
        public double VFov { get; set; } = 90;
        public Vec3Point LookFrom { get; set; } = new Vec3Point(0, 0, 0); // Camera position
        public Vec3Point LookAt { get; set; } = new Vec3Point(0, 0, -1); // Point the camera is looking at
        public Vec3 VUP { get; set; } = new Vec3Point(0, 1, 0); // Up vector for the camera
        public double DefocusAngle = 0; // Variation angle of rays through each pixel
        public double FocusDistance = 10; // distance from camera lookfrom point to plane of perfect focus

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

            
            double theta = RWUtils.DegreesToRadians(VFov);
            double h = Math.Tan(theta / 2);
            double viewportHeight = 2.0 * h * FocusDistance;
            double viewportWidth = AspectRatio * viewportHeight;

            // Change the type of cameraCenter to Vec3 to match the type of other vectors
            _cameraCenter = LookFrom;

            // Calculate the camera basis vectors
            _w = Vec3.UnitVector(LookFrom - LookAt);
            _u = Vec3.UnitVector(Vec3.Cross(VUP, _w));
            _v = Vec3.Cross(_w, _u);
            // Calculate the vectors across the horizontal and down the vertical viewport edges.

            Vec3 viewportU = viewportWidth * _u;
            Vec3 viewportV = viewportHeight * -_v;

            // Calculate the horizontal and vertical delta vectors from pixel to pixel.

            _pixelDeltaU = viewportU / (double)ImageWidth;
            _pixelDeltaV = viewportV / (double)_imageHeight;


            
            // Calculate the location of the upper left pixel.
            Vec3 viewportUpperLeft = _cameraCenter - (FocusDistance * _w) - viewportU / 2 - viewportV / 2;
            _pixel00Loc = viewportUpperLeft + 0.5 * (_pixelDeltaU + _pixelDeltaV);
            // Calculate the defocus disk vectors
            double defocusDiskRadius = FocusDistance * Math.Tan(RWUtils.DegreesToRadians(DefocusAngle) / 2);

            _defocusDiskU = _u * defocusDiskRadius;
            _defocusDiskV = _v * defocusDiskRadius;
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

            Vec3Point rayOrigin = (DefocusAngle <= 0) ? _cameraCenter : DefocusDiskSample();
            Vec3 rayDirection = pixelSample - rayOrigin;

            return new Ray(rayOrigin, rayDirection);
        }

        private Vec3 SampleSquare()
        {
            return new Vec3(RWUtils.RandomDouble() - 0.5, RWUtils.RandomDouble() - 0.5, 0);
        }

        private Vec3Point DefocusDiskSample()
        {
            Vec3 p = Vec3.RandomInUnitDisk();
            return _cameraCenter + (p.X * _defocusDiskU)  + (p.Y * _defocusDiskV);
        }
    }
}
