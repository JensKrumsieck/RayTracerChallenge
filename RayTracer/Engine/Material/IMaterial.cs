using System.Numerics;
using RayTracer.Engine.Lighting;

namespace RayTracer.Engine.Material
{
    public interface IMaterial
    {
        public Color BaseColor { get; set; }

        public Color Lighten(ILight light, Vector3 point, Vector3 eye, Vector3 normal);
    }
}
