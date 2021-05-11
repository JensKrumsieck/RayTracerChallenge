#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer.Engine
{
    public class Transform
    {
        public Vector Position => Vector.Point(TransformationMatrix[0, 3], TransformationMatrix[1, 3], TransformationMatrix[2, 3]);
        public Vector Scale => Vector.Point(TransformationMatrix[0, 0], TransformationMatrix[1, 1], TransformationMatrix[2, 2]);

        public Matrix TransformationMatrix;

        protected Transform(Vector position)
        {
            TransformationMatrix = Matrix.Translation(position.X, position.Y, position.Z);
        }

        protected Transform() => TransformationMatrix = Matrix.Identity;

        public virtual Vector Normal(Vector point) => new(0f, 0f, 0f);

        public virtual HitInfo[] Intersect(Ray ray) => Array.Empty<HitInfo>();

        public static HitInfo? Hit(IEnumerable<HitInfo> intersections) =>
            intersections.Where(s => s.Distance >= 0).OrderBy(s => s.Distance).FirstOrDefault();

        public override string ToString() => GetType().Name + ":" + Position;
    }
}
