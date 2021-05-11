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

        public Ray Transform(Matrix m) => new Ray(m * Origin, m * Direction);

        public static bool Intersect(Ray ray, Transform transform, out HitInfo[] hits)
        {
            hits = transform.Intersect(ray);
            return hits.Length != 0;
        }

        public static bool Intersect(Vector3 origin, Vector3 direction, Transform transform, out HitInfo[] hits) =>
            Intersect(new Ray(origin, direction), transform, out hits);
    }
}
