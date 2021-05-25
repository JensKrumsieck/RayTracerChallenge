using RayTracer.Shapes;
using System;
using System.Collections.Generic;

namespace RayTracer
{
    public sealed class Intersection : IComparable<Intersection>
    {
        public readonly float Distance;
        public readonly Entity Object;
        public float U;
        public float V;

        public Intersection(float distance, Entity hitObject, float u = float.NaN, float v = float.NaN)
        {
            Distance = distance;
            Object = hitObject;
            U = u;
            V = v;
        }

        public static Intersection? Hit(ref List<Intersection> intersections)
        {
            if (intersections.Count == 0 || intersections.TrueForAll(i => i.Distance < 0f)) return null;
            Intersection? lowest = null;
            foreach (var t in intersections)
            {
                if (t.Distance < 0f) continue;
                if (lowest == null || t.Distance < lowest.Distance) lowest = t;
            }
            return lowest;
        }

        public int CompareTo(Intersection? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            return other is null ? 1 : Distance.CompareTo(other.Distance);
        }
    }
}