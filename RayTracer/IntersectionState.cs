using RayTracer.Extension;
using RayTracer.Shapes;
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

        public Vector4 OverPoint => Point + Normal * Constants.Epsilon;
        public Vector4 FarOverPoint => Point + Normal * 5e-3f;
        public readonly bool IsInside;

        private IntersectionState(Entity o, float distance, Vector4 point, Vector4 eye, Vector4 normal)
        {
            Object = o;
            Distance = distance;
            Point = point;
            Eye = eye;
            IsInside = Vector4.Dot(normal, Eye) < 0;
            Normal = IsInside ? -normal : normal;
            Reflect = (-Eye).Reflect(Normal);
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
        }

        public static IntersectionState Prepare(ref Intersection i, ref Ray r)
        {
            var point = r.PointByDistance(i.Distance);
            return new IntersectionState(i.Object, i.Distance, point, -r.Direction, i.Object.Normal(point));
        }
    }
}
