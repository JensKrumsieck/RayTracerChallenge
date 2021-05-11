using System;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer.Engine
{
    public class Transform
    {
        public Vector3 Position => Vector3.Point(TransformationMatrix[0, 3], TransformationMatrix[1, 3], TransformationMatrix[2, 3]);
        public Vector3 Scale => Vector3.Point(TransformationMatrix[0, 0], TransformationMatrix[1, 1], TransformationMatrix[2, 2]);

        public Matrix TransformationMatrix;

        protected Transform(Vector3 position)
        {
            TransformationMatrix = Matrix.Translation(position.X, position.Y, position.Z);
        }

        protected Transform() => TransformationMatrix = Matrix.Identity(4, 4);

        public virtual Vector3 Normal(Vector3 point) => Vector3.Vector(0f, 0f, 0f);

        public virtual HitInfo[] Intersect(Ray ray) => Array.Empty<HitInfo>();

        public static HitInfo? Hit(IEnumerable<HitInfo> intersections) =>
            intersections.Where(s => s.Distance >= 0).OrderBy(s => s.Distance).FirstOrDefault();

        public override string ToString() => GetType().Name + ":" + Position;
    }

    public class NativeTransform
    {
        public System.Numerics.Vector3 Position => new(TransformationMatrix.M41, TransformationMatrix.M42, TransformationMatrix.M43);
        public System.Numerics.Vector3 Scale => new(TransformationMatrix.M11, TransformationMatrix.M22, TransformationMatrix.M33);

        public System.Numerics.Matrix4x4 TransformationMatrix;

        protected NativeTransform(System.Numerics.Vector3 position)
        {
            TransformationMatrix = System.Numerics.Matrix4x4.CreateTranslation(position);
        }

        protected NativeTransform() => TransformationMatrix = System.Numerics.Matrix4x4.Identity;

        public virtual System.Numerics.Vector3 Normal(Vector3 point) => System.Numerics.Vector3.Zero;
        public virtual NativeHitInfo[] Intersect(NativeRay ray) => Array.Empty<NativeHitInfo>();

        public static NativeHitInfo? Hit(IEnumerable<NativeHitInfo> intersections) =>
            intersections.Where(s => s.Distance >= 0).OrderBy(s => s.Distance).FirstOrDefault();

        public override string ToString() => GetType().Name + ":" + Position;
    }
}
