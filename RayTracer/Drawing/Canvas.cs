using SkiaSharp;
using System.IO;
using System.Text;
using static System.Environment;

namespace RayTracer.Drawing
{
    public sealed class Canvas
    {
        public Vector2Int Size;

        private Color[,] _pixels;

        public Canvas(Vector2Int size)
        {
            Size = size;
            _pixels = new Color[size.X, size.Y];
            Fill(Color.Black);
        }

        public Canvas(int x, int y) : this(new Vector2Int(x, y)) { }

        public Color this[int x, int y]
        {
            get => _pixels[x, y];
            set => _pixels[x, y] = value;
        }

        public void Fill(Color c)
        {
            for (var y = 0; y < Size.Y; y++)
            {
                for (var x = 0; x < Size.X; x++)
                {
                    _pixels[x, y] = c;
                }
            }
        }

        /// <summary>
        /// returns ppm, file contents
        /// </summary>
        /// <returns></returns>
        public string ToPixmap()
        {
            var b = new StringBuilder();
            b.Append($"P3{NewLine}{Size.X} {Size.Y}{NewLine}255{NewLine}");
            for (var y = 0; y < Size.Y; y++)
            {
                var line = "";
                for (var x = 0; x < Size.X; x++)
                    AppendToLine(ref line, _pixels[x, y]);
                b.AppendLine(line.Trim());
            }
            return b.ToString();
        }

        private static void AppendToLine(ref string line, Color col)
        {
            //character limit = 70
            const int max = 70;
            foreach (var v in col.ToBytes())
            {
                var parts = line.Split(NewLine);
                if (parts[^1].Length + v.ToString().Length > max) line = line.Trim() + NewLine;
                line += $"{v} ";
            }
        }

        public void Save(string path = "D://test.jpg")
        {
            var info = new SKImageInfo(Size.X, Size.Y, SKImageInfo.PlatformColorType, SKAlphaType.Premul);
            var surface = SKSurface.Create(info);
            var canvas = surface.Canvas;
            for (var y = 0; y < Size.Y; y++)
            {
                for (var x = 0; x < Size.X; x++) canvas.DrawPoint(x, y, _pixels[x, y]);
            }
            using var image = surface.Snapshot().Encode(SKEncodedImageFormat.Jpeg, 100);
            using var s = File.Create(path);
            image.SaveTo(s);
        }
    }
}
