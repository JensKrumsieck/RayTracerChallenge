using RayTracer.Materials;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace RayTracer.Shapes
{
    public abstract class Entity : IEquatable<Sphere>
    {
        protected Entity(Transform transform)
        {
            Transform = transform;
            Material = PhongMaterial.Default;
        }
        protected Entity() : this(Transform.Identity) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<Intersection> Intersect(in Ray r)
        {
            var localRay = r.Transform(Transform.Inverse);
            return IntersectLocal(localRay);
        }

        public abstract List<Intersection> IntersectLocal(in Ray r);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Intersection? Hit(in Ray r)
        {
            var xs = Intersect(r);
            return Intersection.Hit(xs);
        }
        public abstract Vector4 LocalNormal(in Vector4 at);

        public Vector4 Normal(in Vector4 at)
        {
            var obj = Transform.WorldToObject(at);
            var localNormal = LocalNormal(obj);
            var worldNormal = Transform.ObjectToWorld(localNormal);
            worldNormal.W = 0f;
            return Vector4.Normalize(worldNormal);
        }

        public PhongMaterial Material { get; set; }

        public Transform Transform;

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
