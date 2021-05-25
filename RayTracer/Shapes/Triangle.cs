using RayTracer.Extension;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace RayTracer.Shapes
{
    public sealed class Triangle : Entity
    {
        public readonly Vector4 V1;
        public readonly Vector4 V2;
        public readonly Vector4 V3;

        public readonly Vector4 N1;
        public readonly Vector4 N2;
        public readonly Vector4 N3;

        public readonly Vector4 E1;
        public readonly Vector4 E2;
        private readonly Vector4 _normal;
        private readonly bool _isSmooth;

        public Triangle(Vector4 v1, Vector4 v2, Vector4 v3)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
            E1 = V2 - V1;
            E2 = V3 - V1;
            _normal = Vector4.Normalize(E2.Cross(E1));
            SetUpBounds();
        }

        public Triangle(Vector4 v1, Vector4 v2, Vector4 v3, Vector4 n1, Vector4 n2, Vector4 n3)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
            N1 = n1;
            N2 = n2;
            N3 = n3;
            E1 = V2 - V1;
            E2 = V3 - V1;
            _isSmooth = true;
            SetUpBounds();
        }

        private void SetUpBounds()
        {
            var bounds = Bounds.Empty;
            bounds.Add(V1, V2, V3);
            BoundingBox = bounds;
        }

        public override List<Intersection> IntersectLocal(ref Ray r)
        {
            var dirCrossE2 = r.Direction.Cross(E2);
            var det = Vector4.Dot(E1, dirCrossE2);
            if (MathF.Abs(det) < Constants.Epsilon) return new List<Intersection>();

            var f = 1f / det;
            var p1ToOr = r.Origin - V1;
            var u = Vector4.Dot(p1ToOr, dirCrossE2) * f;
            if (u is < 0 or > 1) return new List<Intersection>();
            var orCrossE1 = p1ToOr.Cross(E1);
            var v = f * Vector4.Dot(r.Direction, orCrossE1);
            if (v < 0 || u + v > 1) return new List<Intersection>();

            var t = f * Vector4.Dot(E2, orCrossE1);

            return new List<Intersection> { new(t, this, u, v) };
        }

        public override Vector4 LocalNormal(Vector4 at, Intersection? i = null) => i == null || !_isSmooth ? _normal : N2 * i.U + N3 * i.V + N1 * (1 - i.U - i.V);
    }
}
