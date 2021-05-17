using RayTracer.Environment;

namespace RayTracer.Materials
{
    public interface IMaterial
    {
        public Color Shade(in PointLight l, in IntersectionState c, bool inShadow = false);
    }
}
