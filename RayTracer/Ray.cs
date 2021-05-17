using System;
using RayTracer.Extension;
using System.Numerics;

namespace RayTracer
{
    public struct Ray
    {
        public Vector4 Origin;
        public Vector4 Direction;

        public Ray(Vector4 origin, Vector4 direction)
        {
            Origin = origin;
            Direction = direction;
#if DEBUG
            if (!Origin.IsPoint() || !Direction.IsVector())
                throw new InvalidOperationException($"Ray Origin must be point, direction must be vector! Given: Origin: {Origin} & Direction: {Direction}");
#endif
        }

        public Ray(float ox, float oy, float oz, float dx, float dy, float dz)
            : this(new Vector4(ox, oy, oz, 1f), new Vector4(dx, dy, dz, 0f)) { }

        public readonly Vector4 PointByDistance(float distance) => Origin + Direction * distance;

        public readonly Ray Transform(Matrix4x4 m) => new(m.Multiply(Origin), m.Multiply(Direction));
    }
}
