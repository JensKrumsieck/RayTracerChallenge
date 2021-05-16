using RayTracer.Materials;
using System;
using System.Numerics;

namespace RayTracer.Shapes
{
    public abstract class Entity : IRayObject, IShadedObject, IEquatable<Sphere>
    {/// <inheritdoc />
        public abstract Intersection[] Intersect(in Ray r);
        /// <inheritdoc />
        public abstract Intersection? Hit(in Ray r);
        /// <inheritdoc />
        public abstract Vector4 LocalNormal(in Vector4 at);
        /// <inheritdoc />
        public abstract Vector4 Normal(in Vector4 at);

        public PhongMaterial Material { get; set; }

        public Transform Transform;

        /// <inheritdoc />
        public bool Equals(Sphere? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Transform.Equals(other.Transform) && Material.Equals(other.Material);
        }
        /// <inheritdoc />
        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Sphere other && Equals(other);
        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(Transform, Material);
    }
}
