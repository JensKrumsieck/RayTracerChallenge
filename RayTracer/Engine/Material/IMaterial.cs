using RayTracer.Engine.Lighting;

namespace RayTracer.Engine.Material
{
    public interface IMaterial
    {
        public Color BaseColor { get; set; }

        public Color Shade(ILight light, IntersectionPoint p);
    }
}
