using System.Numerics;

namespace RayTracer.Engine.Lighting
{
    public interface ILight
    {
        public Vector3 Position { get; set; }

        public Color Intensity { get; set; }
    }
}
