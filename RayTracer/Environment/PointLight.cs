using System.Numerics;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Environment
{
    public struct PointLight
    {
        public static PointLight Default => new(Point(-10f, 10f, -10f), Color.White);

        public Vector4 Position;
        public Color Intensity;

        public PointLight(Vector4 position, Color intensity)
        {
            Position = position;
            Intensity = intensity;
        }
    }
}
