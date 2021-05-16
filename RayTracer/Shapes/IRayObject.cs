using System.Numerics;

namespace RayTracer.Shapes
{
    public interface IRayObject
    {
        /// <summary>
        /// Returns all Intersections with Ray r
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public Intersection[] Intersect(in Ray r);

        /// <summary>
        /// Returns nearest Hit with Ray r
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public Intersection? Hit(in Ray r);

        /// <summary>
        /// Normal in Local Coordinates
        /// </summary>
        /// <param name="at"></param>
        /// <returns></returns>
        public Vector4 LocalNormal(in Vector4 at);

        /// <summary>
        /// Normal in World Coordinates
        /// </summary>
        /// <param name="at"></param>
        /// <returns></returns>
        public Vector4 Normal(in Vector4 at);
    }
}
