using System.Numerics;

namespace RayTracer.Lights
{
    public struct PointLight
    {
        public Vector4 Position;
        public Color Intensity;

        public PointLight(Vector4 position, Color intensity)
        {
            Position = position;
            Intensity = intensity;
        }
    }
}
