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

        private readonly Vector4 _normal;
        public Vector4 Normal => IsInside ? -_normal : _normal;

        public Vector4 OverPoint => Point + Normal * 5e-3f;
        public bool IsInside => Vector4.Dot(_normal, Eye) < 0;

        private IntersectionState(Entity o, float distance, Vector4 point, Vector4 eye, Vector4 normal)
        {
            Object = o;
            Distance = distance;
            Point = point;
            Eye = eye;
            _normal = normal;
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
            _normal = normal;
            Object = null!;
            Distance = 0f;
        }

        public static IntersectionState Prepare(ref Intersection i, ref Ray r)
        {
            var point = r.PointByDistance(i.Distance);
            return new IntersectionState(i.Object, i.Distance, point, -r.Direction, i.Object.Normal(point));
        }
    }
}
