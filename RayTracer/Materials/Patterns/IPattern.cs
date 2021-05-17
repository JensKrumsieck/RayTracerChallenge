using System.Numerics;

namespace RayTracer.Materials.Patterns
{
    public interface IPattern
    {
        /// <summary>
        /// Gets the color at specific point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Color At(Vector4 point);

        /// <summary>
        /// Transformation for Pattern
        /// </summary>
        public Transform Transform { get; set; }
    }
}
