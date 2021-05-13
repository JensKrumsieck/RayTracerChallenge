using RayTracer.Engine.Lighting;
using System.Numerics;

namespace RayTracer.Engine.Material
{
    public interface IMaterial
    {
        public Color BaseColor { get; set; }

        public Color Shade(ILight light, Vector3 point, Vector3 eye, Vector3 normal);
    }
}
