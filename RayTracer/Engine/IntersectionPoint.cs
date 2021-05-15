using System.Numerics;

namespace RayTracer.Engine
{
    public struct IntersectionPoint
    {
        public Transform Object;
        public Vector3 HitPoint;
        public Vector3 Eye;
        public HitInfo Intersection;
        public readonly Vector3 OverPoint => HitPoint + Normal * Constants.Epsilon;

        private Vector3 _normal;
        /// <summary>
        /// Normal, responsive to IsInside
        /// </summary>
        public Vector3 Normal
        {
            readonly get => IsInside ? -_normal : _normal;
            set => _normal = value;
        }

        public static IntersectionPoint Prepare(HitInfo intersection, Ray ray)
        {
            var comp = new IntersectionPoint
            {
                Intersection = intersection,
                Object = intersection.HitObject,
                HitPoint = ray.PointByDistance(intersection.Distance),
                Eye = -ray.Direction
            };
            comp.Normal = comp.Object.Normal(comp.HitPoint);
            return comp;
        }

        public readonly bool IsInside => Vector3.Dot(_normal, Eye) < 0;
    }
}
