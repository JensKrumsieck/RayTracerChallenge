using RayTracer.Extension;
using RayTracer.Shapes;
using System.Numerics;

namespace RayTracer.Materials.Patterns
{
    public abstract class Pattern : IPattern
    {
        public abstract Color At(Vector4 point);

        public Color ColorAtEntity(Entity obj, Vector4 point)
        {
            var objectPoint = obj?.Transform.Inverse.Multiply(point) ?? point;
            var patternPoint = Transform.Inverse.Multiply(objectPoint);
            return At(patternPoint);
        }

        public Transform Transform { get; set; } = Transform.Identity;
    }
}
