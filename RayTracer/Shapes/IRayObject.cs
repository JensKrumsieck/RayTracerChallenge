namespace RayTracer.Shapes
{
    public interface IRayObject
    {
        public Intersection[] Intersect(in Ray r);

        public Intersection? Hit(in Ray r);
    }
}
