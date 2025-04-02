using RayTracer;
using Vec3Color = RayTracer.Vec3;
using Vec3Point = RayTracer.Vec3;
// output file name

const string OutputFileName = "output.ppm";


static double HitSphere(Vec3Point center, double radius, Ray r)
{
    Vec3 oc = center - r.origin;
    double a = r.direction.LengthSquared;
    double h = Vec3.Dot(r.direction, oc);
    double c = oc .LengthSquared - radius * radius;
    double discriminant = h * h - a * c;

    if (discriminant < 0)
    {
        return -1.0;
    } else
    {
        return (h - Math.Sqrt(discriminant)) / a;
    }
}

static Vec3 RayColor (Ray r)
{
    double t = HitSphere(new Vec3Point(0, 0, -1), 0.5, r);
    if (t > 0)
    {
        Vec3 normal = Vec3.UnitVector(r.At(t) - new Vec3(0, 0, -1));
        return 0.5 * new Vec3Color(normal.X + 1, normal.Y + 1, normal.Z + 1);

    }
    Vec3 unitDirection = Vec3.UnitVector(r.direction);
    double a = 0.5 * (unitDirection.Y + 1.0);
    return ((1 - a) * new Vec3Color(1.0, 1.0, 1.0) + (a) * new Vec3Color(0.5, 0.7, 1.0));
}

// Image

double aspectRatio = 16.0 / 9.0;
int imageWidth = 600;
// Calculate image height, and ensure it is an integer

int imageHeight = (int)(imageWidth / aspectRatio);
imageHeight = imageHeight >= 1 ? imageHeight : 1;

// Camera 

double focalLength = 1.0;
double viewportHeight = 2.0;
double viewportWidth = aspectRatio * viewportHeight;
// Change the type of cameraCenter to Vec3 to match the type of other vectors
Vec3 cameraCenter = new Vec3Point(0, 0, 0);

// Calculate the vectors across the horizontal and down the vertical viewport edges.

Vec3 viewportU = new(viewportWidth, 0, 0);
Vec3 viewportV = new(0, -viewportHeight, 0);

// Calculate the horizontal and vertical delta vectors from pixel to pixel.

Vec3 pixelDeltaU = viewportU / (double)imageWidth;
Vec3 pixelDeltaV = viewportV / (double)imageHeight;


Vec3 focalVec = new(0, 0, focalLength);
// Calculate the location of the upper left pixel.
Vec3 viewportUpperLeft = cameraCenter - focalVec - viewportU / 2 - viewportV / 2;
Vec3 pixel00Loc = viewportUpperLeft + 0.5 * (pixelDeltaU + pixelDeltaV);

// Render

using (StreamWriter outputFile = new(OutputFileName, false))
{ 
    outputFile.WriteLine("P3\n" + imageWidth + " " + imageHeight + "\n255\n");
    for (int j = 0; j < imageHeight; j++)
    {
        Console.WriteLine("Scanlines remaining: " + (imageHeight - j));

        for (int i = 0; i < imageWidth; i++)
        {
            Vec3 pixelCenter = pixel00Loc + (i * pixelDeltaU) + (j * pixelDeltaV);
            Vec3 rayDirection = pixelCenter - cameraCenter;
            Ray r = new(cameraCenter, rayDirection);
            Vec3 pixelColor = RayColor(r);

            Utils.WriteColor(outputFile, pixelColor);

        }
    }
    Console.WriteLine("Done.                    ");
}