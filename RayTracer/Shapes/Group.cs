using System;
using System.Collections.Generic;
using System.Numerics;

namespace RayTracer.Shapes
{
    public sealed class Group : Entity
    {
        public Group(Transform t) : base(t) { }
        public Group() { }

        private readonly HashSet<Entity> _children = new();

        public void AddChild(Entity e)
        {
            _children.Add(e);
            e.Parent = this;
            BoundingBox = ComputeBounds();
        }

        public void AddChildren(params Entity[] entities)
        {
            foreach (var e in entities) AddChild(e);
        }

        public int Count => _children.Count;

        public bool Contains(Entity e) => _children.Contains(e);

        public override Bounds BoundingBox { get; set; }

        public override List<Intersection> IntersectLocal(ref Ray r)
        {
            var xs = new List<Intersection>();
            if (!BoundingBox.IntersectLocal(ref r)) return xs;
            foreach (var e in _children)
            {
                e.Intersect(ref r, ref xs);
            }
            //do not sort in groups as world does already do that
            return xs;
        }

        public override Vector4 LocalNormal(Vector4 at) => throw new InvalidOperationException();

        public Bounds ComputeBounds()
        {
            var b = Bounds.Empty;
            foreach (var e in _children)
            {
                b.Add(e.TransformedBoundingBox);
            }
            return b;
        }
    }
}
