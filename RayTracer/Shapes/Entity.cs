using RayTracer.Materials;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace RayTracer.Shapes
{
    public abstract class Entity : IEquatable<Entity>
    {
        protected Entity(Transform transform)
        {
            Transform = transform;
            Material = PhongMaterial.Default;
        }
        protected Entity() : this(Transform.Identity) { }

        public void Intersect(ref Ray r, ref List<Intersection> xs)
        {
            var localRay = r.Transform(Transform.Inverse);
            var items = IntersectLocal(localRay);
            xs.AddRange(items);
        }

        public abstract List<Intersection> IntersectLocal(in Ray r);

        public Intersection? Hit(ref Ray r)
        {
            var xs = new List<Intersection>();
            Intersect(ref r, ref xs);
            return Intersection.Hit(ref xs);
        }
        public abstract Vector4 LocalNormal(Vector4 at);

        public Vector4 Normal(Vector4 at)
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
        public bool Equals(Entity? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Transform.Equals(other.Transform) && Material.Equals(other.Material);
        }
        /// <inheritdoc />
        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Entity other && Equals(other);
        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(Transform, Material);
    }
}
