using RayTracer.Extension;
using RayTracer.Shapes;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace RayTracer
{
    public readonly struct IntersectionState
    {
        public readonly Entity Object;
        public readonly float Distance;

        public readonly Vector4 Point;
        public readonly Vector4 Eye;
        public readonly Vector4 Reflect;
        public readonly Vector4 Normal;

        public readonly float N1;
        public readonly float N2;

        public Vector4 OverPoint => Point + Normal * Constants.Epsilon;
        public Vector4 FarOverPoint => Point + Normal * 5e-3f;
        public Vector4 UnderPoint => Point - Normal * Constants.Epsilon;

        public readonly bool IsInside;

        private IntersectionState(Entity o, float distance, Vector4 point, Vector4 eye, Vector4 normal, float n1, float n2)
        {
            Object = o;
            Distance = distance;
            Point = point;
            Eye = eye;
            IsInside = Vector4.Dot(normal, Eye) < 0;
            Normal = IsInside ? -normal : normal;
            Reflect = (-Eye).Reflect(Normal);
            N1 = n1;
            N2 = n2;
        }

        /// <summary>
        /// USE IN TESTS ONLY!
        /// </summary>
        /// <param name="point"></param>
        /// <param name="eye"></param>
        /// <param name="normal"></param>
        public IntersectionState(Vector4 point, Vector4 eye, Vector4 normal)
        {
            Point = point;
            Eye = eye;
            IsInside = Vector4.Dot(normal, Eye) < 0;
            Normal = IsInside ? -normal : normal;
            Object = null!;
            Distance = 0f;
            Reflect = (-Eye).Reflect(Normal);
            N1 = 0;
            N2 = 0;
        }

        public float Schlick()
        {
            var cos = Vector4.Dot(Eye, Normal);
            if (N1 > N2)
            {
                var n = N1 / N2;
                var sin2 = n * n * (1 - cos * cos);
                if (sin2 > 1.0f) return 1.0f;

                var cost = MathF.Sqrt(1f - sin2);
                cos = cost;
            }

            var tmp = (N1 - N2) / (N1 + N2);
            var r0 = tmp * tmp;

            return r0 + (1 - r0) * MathF.Pow(1 - cos, 5);
        }

        public static IntersectionState Prepare(ref Intersection i, ref Ray r) =>
            Prepare(ref i, ref r, new List<Intersection> { i });

        public static IntersectionState Prepare(ref Intersection i, ref Ray r, List<Intersection> xs)
        {
            var n1 = 0f;
            var n2 = 0f;
            var containers = new List<Entity>();
            foreach (var x in xs)
            {
                if (x.Equals(i)) n1 = containers.Count == 0 ? 1f : containers[^1].Material.IOR;

                if (containers.Contains(x.Object)) containers.Remove(x.Object);
                else containers.Add(x.Object);

                if (x.Equals(i)) n2 = containers.Count == 0 ? 1f : containers[^1].Material.IOR;
            }

            var point = r.PointByDistance(i.Distance);
            return new IntersectionState(i.Object, i.Distance, point, -r.Direction, i.Object.Normal(point), n1, n2);
        }
    }
}
