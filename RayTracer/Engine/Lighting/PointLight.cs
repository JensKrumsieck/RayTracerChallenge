using System.Numerics;

namespace RayTracer.Engine.Lighting
{
    public struct PointLight : ILight
    {
        public Vector3 Position { get; set; }
        public Color Intensity { get; set; }

        public PointLight(Vector3 position, Color intensity)
        {
            Position = position;
            Intensity = intensity;
        }

        public override string ToString() => $"PointLight at {Position} with Intensity {Intensity}";
    }
}
