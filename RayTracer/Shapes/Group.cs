using RayTracer.Materials;
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

        public void AddChild(Entity e, bool computeBounds = true)
        {
            _children.Add(e);
            e.Parent = this;
            if (computeBounds) BoundingBox = ComputeBounds();
        }

        public void AddChildrenWithoutRefresh(params Entity[] entities)
        {
            foreach (var e in entities) AddChild(e, false);
        }

        public void AddChildren(params Entity[] entities)
        {
            foreach (var e in entities) AddChild(e, false);
            BoundingBox = ComputeBounds();
        }

        public int Count => _children.Count;

        public bool Contains(Entity e) => _children.Contains(e);

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
            foreach (var e in _children) b.Add(e.TransformedBoundingBox);
            return b;
        }

        public (Group left, Group right) Partition()
        {
            var gLeft = new Group();
            var gRight = new Group();
            var (left, right) = BoundingBox.Split();
            var remaining = new List<Entity>();
            foreach (var child in _children)
            {
                if (left.Contains(child.TransformedBoundingBox)) gLeft.AddChild(child, false);
                else if (right.Contains(child.TransformedBoundingBox)) gRight.AddChild(child, false);
                else remaining.Add(child);
            }
            _children.Clear();
            AddChildrenWithoutRefresh(remaining.ToArray());
            return (gLeft, gRight);
        }

        public override void Divide(int threshold = 1)
        {
            if (threshold < Count)
            {
                var (left, right) = Partition();
                if (left.Count != 0) AddChild(left, false);
                if (right.Count != 0) AddChild(right, false);
            }
            foreach (var c in _children) c.Divide(threshold);
            BoundingBox = ComputeBounds();
        }

        public void Fill(PhongMaterial material)
        {
            foreach (var c in _children)
            {
                if (c is Group g) g.Fill(material);
                else c.Material = material;
            }
        }
    }
}
