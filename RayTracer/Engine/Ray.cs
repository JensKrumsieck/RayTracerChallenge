namespace RayTracer.Engine
{
    public struct Ray
    {
        public Vector Origin;
        public Vector Direction;

        public Ray(Vector origin, Vector direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public readonly Vector PointByDistance(float d) => Origin + Direction * d;

        public readonly Ray Transform(Matrix m) => new(m * Origin, m * Direction);

        private static bool Intersect(Ray ray, Transform transform, out HitInfo[] hits)
        {
            hits = transform.Intersect(ray);
            return hits.Length != 0;
        }
        public static bool Intersect(Vector origin, Vector direction, Transform transform, out HitInfo[] hits) =>
            Intersect(new Ray(origin, direction), transform, out hits);

        public static HitInfo? Hit(Vector origin, Vector dir, Transform transform) => Hit(new Ray(origin, dir), transform);
        private static HitInfo? Hit(Ray ray, Transform transform) => transform.Hit(ray);
    }
}
