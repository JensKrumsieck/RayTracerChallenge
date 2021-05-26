using System.Collections.Generic;
using System.Numerics;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Environment
{
    public struct PointLight : ILight
    {
        public static PointLight Default => new(Point(-10f, 10f, -10f), Color.White);

        public Vector4 Position { get; set; }

        public Color Intensity { get; set; }
        public int Samples { get; set; }

        public PointLight(Vector4 position, Color intensity) : this()
        {
            Position = position;
            Intensity = intensity;
            Samples = 1;
        }

        public readonly float IntensityAt(Vector4 point, World w) => w.InShadow(Position, point) ? 0f : 1f;
        
        public readonly IEnumerable<Vector4> GetSamples()
        {
            yield return Position;
        }
    }
}
