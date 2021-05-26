using System.Numerics;

namespace RayTracer.Environment
{
    public interface ILight
    {
        public Vector4 Position { get; set; }
        public Color Intensity { get; set; }

        public float IntensityAt(Vector4 point, World w);
    }
}
