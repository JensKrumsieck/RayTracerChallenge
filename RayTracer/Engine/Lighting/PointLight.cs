using System.Numerics;

namespace RayTracer.Engine.Lighting
{
    public struct PointLight : ILight
    {
        public Vector3 Position { get; }
        public Color Intensity { get; }

        public PointLight(Vector3 position, Color intensity)
        {
            Position = position;
            Intensity = intensity;
        }
    }
}
