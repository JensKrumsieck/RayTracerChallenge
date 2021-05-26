using System.Collections.Generic;
using System.Numerics;

namespace RayTracer.Environment
{
    public interface ILight
    {
        public IEnumerable<Vector4> GetSamples();
        public Color Intensity { get; set; }
        public int Samples { get; set; }
        public float IntensityAt(Vector4 point, World w);
    }
}
