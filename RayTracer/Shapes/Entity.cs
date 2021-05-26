using RayTracer.Materials;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace RayTracer.Shapes
{
    public abstract class Entity : IEquatable<Entity>
    {
        public readonly Guid Id;
        protected Entity() : this(Transform.Identity) { }
        protected Entity(Transform transform)
        {
            Transform = transform;
            Material = PhongMaterial.Default;
            Id = Guid.NewGuid();
        }

        public Entity? Parent;

        public PhongMaterial Material { get; set; }

        public bool Shadow = true;

        private Transform _transform;

        public Transform Transform
        {
            get => _transform;
            set
            {
                _transform = value;
                //reset transformed box
                TransformedBoundingBox = BoundingBox.Transform(_transform);
            }
        }

        private Bounds _boundingBox;
        public Bounds BoundingBox
        {
            get => _boundingBox;
            set
            {
                _boundingBox = value;
                TransformedBoundingBox = _boundingBox.Transform(Transform);
            }
        }

        public Bounds TransformedBoundingBox { get; protected set; }

        public void Intersect(ref Ray r, ref List<Intersection> xs)
        {
            var localRay = r.Transform(Transform.Inverse);
            var items = IntersectLocal(ref localRay);
            xs.AddRange(items);
        }

        public abstract List<Intersection> IntersectLocal(ref Ray r);

        public Intersection? Hit(ref Ray r)
        {
            var xs = new List<Intersection>();
            Intersect(ref r, ref xs);
            return Intersection.Hit(ref xs);
        }
        public abstract Vector4 LocalNormal(Vector4 at, Intersection? i = null);

        public Vector4 Normal(Vector4 at, Intersection? i = null)
        {
            var obj = WorldToObject(at);
            var localNormal = LocalNormal(obj, i);
            var worldNormal = NormalToWorld(localNormal);
            worldNormal.W = 0f;
            return Vector4.Normalize(worldNormal);
        }

        public Vector4 WorldToObject(Vector4 point)
        {
            if (Parent != null) point = Parent.WorldToObject(point);
            return Transform.WorldToObject(point);
        }

        public Vector4 NormalToWorld(Vector4 point)
        {
            point = Transform.ObjectToWorld(point);
            point.W = 0;
            point = Vector4.Normalize(point);
            if (Parent != null) point = Parent.NormalToWorld(point);
            return point;
        }

        public virtual void Divide(int threshold = 1) { }

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
        public override int GetHashCode() => HashCode.Combine(Id);
    }
}
