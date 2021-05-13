﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;
using RayTracer.Primitives;
using System.Threading.Tasks;
using Color = RayTracer.Engine.Color;
using Vector = RayTracer.Engine.Vector;

namespace RayTracer.Tests
{
    [TestClass]
    public class TheChallenges
    {
        [TestMethod]
        public void ChapterV()
        {
            const float wallZ = 10f;
            const float wallSize = 7f;
            const int canvasSize = 100;
            const float pxSize = wallSize / canvasSize;
            const float half = wallSize / 2f;

            var canvas = new Viewport(canvasSize, canvasSize);
            var col = Color.Red;
            var s = new Sphere();
            var origin = Vector.Point(0f, 0f, -5f);
            Parallel.For(0, canvasSize, y =>
            {
                var worldY = half - pxSize * y;
                for (var x = 0; x < canvasSize; x++)
                {
                    var worldX = -half + pxSize * x;
                    if (Ray.Hit(origin, (Vector.Point(worldX, worldY, wallZ) - origin).Normalized, s) != null) canvas.SetPixel(x, y, col);
                }
            });
        }
    }
}
