using RayTracer.Shapes;
using System;

#nullable enable
namespace RayTracer
{
    public struct Intersection
    {
        public float Distance;
        public IRayObject Object;

        public Intersection(float distance, IRayObject hitObject)
        {
            Distance = distance;
            Object = hitObject;
        }

        public static Intersection? Hit(Intersection[] intersections)
        {
            if (intersections.Length == 0 || Array.TrueForAll(intersections, i => i.Distance < 0f)) return null;
            Intersection? lowest = null;
            for (var i = 0; i < intersections.Length; i++)
            {
                if (intersections[i].Distance < 0f) continue;
                if (lowest == null || intersections[i].Distance < lowest?.Distance) lowest = intersections[i];
            }
            return lowest;
        }
    }
}