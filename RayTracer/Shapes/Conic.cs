using RayTracer.Extension;
using System.Collections.Generic;

namespace RayTracer.Shapes
{
    public abstract class Conic : Entity
    {
        protected Conic(Transform transform) : base(transform) { }
        protected Conic() { }
        public float Maximum = float.PositiveInfinity;
        public float Minimum = float.NegativeInfinity;
        public bool IsClosed = false;

        protected static bool CheckCap(ref Ray ray, float t, float rad = 1f)
        {
            var x = ray.Origin.X + t * ray.Direction.X;
            var z = ray.Origin.Z + t * ray.Direction.Z;

            return x * x + z * z <= rad * rad + Constants.Epsilon;
        }

        protected void IntersectCaps(ref Ray r, ref List<Intersection> xs, bool useRadius = false)
        {
            if (!IsClosed || r.Direction.Y.Equal(0)) return;
            var t = (Minimum - r.Origin.Y) / r.Direction.Y;
            if (CheckCap(ref r, t, useRadius ? Minimum : 1)) xs.Add(new Intersection(t, this));
            t = (Maximum - r.Origin.Y) / r.Direction.Y;
            if (CheckCap(ref r, t, useRadius ? Maximum : 1)) xs.Add(new Intersection(t, this));
        }
    }
}
