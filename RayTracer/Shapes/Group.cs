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
        }

        public void AddChildren(params Entity[] entities)
        {
            foreach (var e in entities) AddChild(e);
        }

        public int Count => _children.Count;

        public bool Contains(Entity e) => _children.Contains(e);

        public override List<Intersection> IntersectLocal(ref Ray r)
        {
            var xs = new List<Intersection>();
            foreach (var e in _children)
            {
                e.Intersect(ref r, ref xs);
            }
            //do not sort in groups as world does already do that
            return xs;
        }

        public override Vector4 LocalNormal(Vector4 at) => throw new InvalidOperationException();
    }
}
