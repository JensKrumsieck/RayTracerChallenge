using RayTracer.Materials.Patterns;
using System.Numerics;

namespace RayTracer.Tests.TestObjects
{
    public sealed class TestPattern : Pattern
    {
        public override Color At(Vector4 point) => point;
    }
}
