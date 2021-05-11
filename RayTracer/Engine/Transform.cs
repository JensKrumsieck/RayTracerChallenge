using System;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer.Engine
{
    public class Transform
    {
        public Vector3 Position;

        public Transform(Vector3 position)
        {
            Position = position;
        }

        public virtual HitInfo[] Intersect(Ray ray) => Array.Empty<HitInfo>();

        public static HitInfo? Hit(IEnumerable<HitInfo> intersections) =>
            intersections.Where(s => s.Distance >= 0).OrderBy(s => s.Distance).FirstOrDefault();

        public override string ToString() => GetType().Name + ":" + Position;
    }
}
