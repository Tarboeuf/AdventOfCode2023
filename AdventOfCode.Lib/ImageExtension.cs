using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

#pragma warning disable CA1416

namespace AdventOfCode.Lib;

public static class ImageExtension
{
    public static void ExportImage(this bool[][] lines, Func<bool, Color> getColor, string filePath)
    {
        var imageHeight = lines.Length;
        var imageWidth = lines[0].Length;
        var bitmap = new Bitmap(imageWidth, imageHeight);

        for (var y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            for (var x = 0; x < line.Length; x++)
            {
                var c = line[x];
                bitmap.SetPixel(x, y, getColor(c));
            }
        }
        
        bitmap.Save(filePath);
    }

    public static void ExportImage(this IEnumerable<(int x, int y)> path, int width, int height, string filePath)
    {
        var imageHeight = height;
        var imageWidth = width;
        var bitmap = new Bitmap(imageWidth, imageHeight);
        var hash = path.ToHashSet();

        for (var y = 0; y < imageHeight; y++)
        {
            for (var x = 0; x < imageWidth; x++)
            {
                bitmap.SetPixel(x, y, hash.Contains((x, y)) ? Color.Black : Color.White);
            }
        }

        bitmap.Save(filePath);
    }

    public static int ExportImageByPath(this IEnumerable<(int x, int y)> path, int width, int height, string filePath)
    {
        var imageHeight = height;
        var imageWidth = width;
        var bitmap = new Bitmap(imageWidth, imageHeight);
        var list = path.ToList();


        using (var g = Graphics.FromImage(bitmap))
        {
            g.FillPolygon(Brushes.Blue, list.Append(list.First()).Select(p => new Point(p.x, p.y)).ToArray(), FillMode.Winding);
            g.DrawPolygon(new Pen(Brushes.Black), list.Append(list.First()).Select(p => new Point(p.x, p.y)).ToArray());
        }
        
        File.Delete(filePath);
        bitmap.Save(filePath);
        
        return bitmap.GetColors().Count(c => c.Name == "ff0000ff");
    }

    public static IEnumerable<Color> GetColors(this Bitmap bitmap)
    {
        for (var y = 0; y < bitmap.Height; y++)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                yield return bitmap.GetPixel(x, y);
            }
        }
    }   
}