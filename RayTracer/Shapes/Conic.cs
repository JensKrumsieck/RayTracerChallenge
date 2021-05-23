namespace RayTracer.Shapes
{
    public abstract class Conic : Entity
    {
        protected Conic(Transform transform) : base(transform) { }
        protected Conic() { }
        public float Maximum = float.PositiveInfinity;
        public float Minimum = float.NegativeInfinity;
        public bool IsClosed = false;

        protected static bool CheckCap(ref Ray ray, float t)
        {
            var x = ray.Origin.X + t * ray.Direction.X;
            var z = ray.Origin.Z + t * ray.Direction.Z;

            return x * x + z * z <= 1 + Constants.Epsilon;
        }
    }
}
