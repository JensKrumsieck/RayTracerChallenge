using RayTracer.Materials;
using System;
using System.Numerics;

namespace RayTracer.Shapes
{
    public abstract class Entity : IRayObject, IShadedObject, IEquatable<Sphere>
    {
        public abstract Intersection[] Intersect(in Ray r);

        public abstract Intersection? Hit(in Ray r);

        public abstract Vector4 LocalNormal(in Vector4 at);

        public abstract Vector4 Normal(in Vector4 at);

        public PhongMaterial Material { get; set; }

        public Transform Transform;

        public bool Equals(Sphere? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Transform.Equals(other.Transform) && Material.Equals(other.Material);
        }
        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Sphere other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Transform, Material);
    }
}
