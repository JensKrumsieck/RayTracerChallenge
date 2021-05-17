using RayTracer.Shapes;
using System.Collections.Generic;
using System.Numerics;

namespace RayTracer.Tests.TestObjects
{
    public sealed class TestShape : Entity
    {
        public TestShape(Transform tr) : base(tr) { }
        public TestShape() { }

        /// <summary>
        /// The Last Ray fired onto TestSpheres IntersectLocal Method
        /// </summary>
        public Ray SavedRay;

        /// <inheritdoc />
        public override List<Intersection> IntersectLocal(in Ray r)
        {
            SavedRay = r;
            return new List<Intersection>();
        }

        /// <inheritdoc />
        public override Vector4 LocalNormal(in Vector4 at) => new(at.X, at.Y, at.Z, 0f);
    }
}
