using SkiaSharp;
using System;
using System.IO;

namespace RayTracer.Engine
{
    public sealed class Viewport
    {
        public int Width { get; }
        public int Height { get; }

        private Color[,] _pixels;

        public Viewport(int w, int h)
        {
            Width = w;
            Height = h;
            Initialize();
        }

        public void SetPixel(int i, int j, Color col) => _pixels[i, j] = col;
        public Color PixelAt(int i, int j) => _pixels[i, j];

        private void Initialize()
        {
            _pixels = new Color[Width, Height];
            SetAll(Color.Black);
        }

        public void SetAll(Color col)
        {
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                    _pixels[i, j] = col;
            }
        }

        /// <summary>
        /// For Test purposes only!
        /// </summary>
        /// <returns></returns>
        public Color[,] GetPixels() => _pixels;

        public string ToPixmap()
        {
            const string variant = "P3";
            const byte max = 255;
            var content = $"{variant}{Environment.NewLine}{Width} {Height}{Environment.NewLine}{max}{Environment.NewLine}";
            for (var i = 0; i < Height; i++)
            {
                var line = "";
                for (var j = 0; j < Width; j++) AppendToLine(ref line, _pixels[j, i]);
                content += line.Trim() + Environment.NewLine;
            }
            return content;
        }

        private static void AppendToLine(ref string line, Color col)
        {
            //character limit = 70
            const int max = 70;
            foreach (var v in col.ToBytes())
            {
                var parts = line.Split(Environment.NewLine);
                if (parts[^1].Length + v.ToString().Length > max) line = line.Trim() + Environment.NewLine;
                line += $"{v} ";
            }
        }

        /// <summary>
        /// Uses SkiaSharp to produce an image
        /// path default to D:/ is for debugging purpose and will be removed
        /// </summary>
        /// <param name="path"></param>
        public void Render(string path = "D://test.jpg")
        {
            var info = new SKImageInfo(Width, Height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);
            var surface = SKSurface.Create(info);
            var canvas = surface.Canvas;
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++) canvas.DrawPoint(i, j, _pixels[i, j].ToSKColor());
            }

            using var image = surface.Snapshot().Encode(SKEncodedImageFormat.Jpeg, 100);
            using var s = File.Create(path);
            image.SaveTo(s);
        }

        public void RenderPPM(string path = "D://test.ppm") => File.WriteAllText(path, ToPixmap());

        /// <inheritdoc/>
        public override string ToString() => ToPixmap();
    }
}
