using RayTracer.Shapes;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace RayTracer
{
    public struct IntersectionState
    {
        public IRayObject Object;
        public float Distance;

        public Vector4 Point;
        public Vector4 Eye;

        private Vector4 _normal;
        public Vector4 Normal
        {
            get => IsInside ? -_normal : _normal;
            set => _normal = value;
        }

        public readonly bool IsInside => Vector4.Dot(_normal, Eye) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntersectionState Prepare(Intersection i, in Ray r)
        {
            var point = r.PointByDistance(i.Distance);
            return new IntersectionState
            {
                Distance = i.Distance,
                Object = i.Object,
                Eye = -r.Direction,
                _normal = i.Object.Normal(point),
                Point = point
            };
        }
    }
}
