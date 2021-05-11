namespace RayTracer.Engine
{
    public struct Ray
    {
        public Vector3 Origin;
        public Vector3 Direction;

        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public readonly Vector3 PointByDistance(float d) => Origin + Direction * d;

        public readonly Ray Transform(Matrix m) => new(m * Origin, m * Direction);

        public static bool Intersect(Ray ray, Transform transform, out HitInfo[] hits)
        {
            hits = transform.Intersect(ray);
            return hits.Length != 0;
        }

        public static bool Intersect(Vector3 origin, Vector3 direction, Transform transform, out HitInfo[] hits) =>
            Intersect(new Ray(origin, direction), transform, out hits);
    }

    public struct NativeRay
    {
        public System.Numerics.Vector3 Origin;
        public System.Numerics.Vector3 Direction;

        public NativeRay(System.Numerics.Vector3 origin, System.Numerics.Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }
        public readonly System.Numerics.Vector3 PointByDistance(float d) => Origin + Direction * d;

        public readonly NativeRay Transform(System.Numerics.Matrix4x4 m) =>
            new(System.Numerics.Vector3.Transform(Origin, m), System.Numerics.Vector3.Transform(Direction, m));

        public static bool Intersect(NativeRay ray, NativeTransform transform, out NativeHitInfo[] hits)
        {
            hits = transform.Intersect(ray);
            return hits.Length != 0;
        }

        public static bool Intersect(System.Numerics.Vector3 origin, System.Numerics.Vector3 direction, NativeTransform transform, out NativeHitInfo[] hits) =>
            Intersect(new NativeRay(origin, direction), transform, out hits);

    }
}
