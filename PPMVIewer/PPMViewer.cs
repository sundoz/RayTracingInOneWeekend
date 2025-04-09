using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

public class PPMViewer : Form
{
    private Bitmap image;

    public PPMViewer(string filePath)
    {
        this.Text = "PPM Viewer";
        this.Load += (sender, e) => LoadPPM(filePath);
        this.Paint += (sender, e) =>
        {
            if (image != null)
                e.Graphics.DrawImage(image, 0, 0);
        };
    }

    private void LoadPPM(string filePath)
    {
        Console.WriteLine(filePath);
        var lines = File.ReadAllLines(filePath)
                        .Where(line => !line.StartsWith("#")) // пропускаем комментарии
                        .ToArray();

        if (lines[0] != "P3")
        {
            MessageBox.Show("Поддерживается только формат P3 (ASCII).", "Ошибка");
            return;
        }

        string[] dimensions = lines[1].Split(' ');
        int width = int.Parse(dimensions[0]);
        int height = int.Parse(dimensions[1]);

        int maxColor = int.Parse(lines[2]);

        var colorValues = lines.Skip(3)
                               .SelectMany(line => line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries))
                               .Select(int.Parse)
                               .ToArray();

        image = new Bitmap(width, height);

        int i = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int r = colorValues[i++];
                int g = colorValues[i++];
                int b = colorValues[i++];
                image.SetPixel(x, y, Color.FromArgb(r, g, b));
            }
        }

        this.ClientSize = new Size(width, height);
        this.Invalidate(); // перерисовать окно
    }

    [STAThread]
    public static void Main()
    {
        string ppmPath = "C:\\Users\\rogac\\source\\repos\\RayTracingInOneWeekend\\RayTracer\\bin\\Debug\\net9.0\\output.ppm"; // замените на путь к вашему PPM-файлу
        System.Windows.Forms.Application.EnableVisualStyles();
        System.Windows.Forms.Application.Run(new PPMViewer(ppmPath));
    }
}