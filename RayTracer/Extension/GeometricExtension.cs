using RayTracer.Shapes;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace RayTracer.Extension
{
    public static class GeometricExtension
    {
        public static (float, float) CheckAxis(float origin, float direction, float min = -1f, float max = 1f)
        {
            var tMinNum = min - origin;
            var tMaxNum = max - origin;
            float tMin;
            float tMax;
            if (MathF.Abs(direction) >= Constants.Epsilon)
            {
                tMin = tMinNum / direction;
                tMax = tMaxNum / direction;
            }
            else
            {
                tMin = tMinNum * float.PositiveInfinity;
                tMax = tMaxNum * float.PositiveInfinity;
            }

            return tMin > tMax ? (tMax, tMin) : (tMin, tMax);
        }

        public static Triangle[] FanTriangulation(this List<Vector4> vertices, List<Vector4> normals)
        {
            var tris = new Triangle[vertices.Count - 2];
            for (var i = 2; i < vertices.Count; i++)
            {
                if (normals.Count == 0) tris[i - 2] = new Triangle(vertices[0], vertices[i], vertices[i - 1]);
                else
                    tris[i - 2] = new Triangle(vertices[0], vertices[i], vertices[i - 1], normals[0], normals[i],
                        normals[i - 1]);
            }
            return tris;
        }
    }
}
