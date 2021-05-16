using RayTracer.Shapes;
using System;

namespace RayTracer
{
    public sealed class Intersection
    {
        public float Distance;
        public Entity Object;

        public Intersection(float distance, Entity hitObject)
        {
            Distance = distance;
            Object = hitObject;
        }

        public static Intersection? Hit(Intersection[] intersections)
        {
            if (intersections.Length == 0 || Array.TrueForAll(intersections, i => i.Distance < 0f)) return null;
            Intersection? lowest = null;
            foreach (var t in intersections)
            {
                if (t.Distance < 0f) continue;
                if (lowest == null || t.Distance < lowest.Distance) lowest = t;
            }
            return lowest;
        }
    }
}