#nullable enable
using System;
using System.Numerics;

namespace RayTracer.Engine
{
    public class Transform
    {
        private Matrix _transformationMatrix;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;

        public Matrix TransformationMatrix
        {
            get => _transformationMatrix;
            set
            {
                _transformationMatrix = value;
                Decompose();
            }
        }

        private void Decompose()
        {
            if (!Matrix4x4.Decompose(TransformationMatrix, out var scale, out var rotation, out var pos)) return;
            Position = pos;
            Rotation = rotation;
            Scale = scale;
        }

        protected Transform(Vector3 position) => TransformationMatrix = Matrix.Translation(position.X, position.Y, position.Z);

        protected Transform() => TransformationMatrix = Matrix4x4.Identity;

        public virtual Vector3 Normal(Vector3 point) => new(0f, 0f, 0f);

        public virtual HitInfo[] Intersect(Ray ray, bool hit = false) => Array.Empty<HitInfo>();

        public HitInfo? Hit(Ray ray)
        {
            var xs = Intersect(ray, true);
            return xs.Length == 0 ? null : xs[0];
        }

        public override string ToString() => GetType().Name + ":" + Position;

        public Vector3 WorldToObject(Vector3 worldPoint) => Vector3.Transform(worldPoint, TransformationMatrix.Inverse());

        public Vector3 ObjectToWorld(Vector3 objectPoint) => Vector3.Transform(objectPoint, TransformationMatrix.Inverse().Transpose());
    }
}
