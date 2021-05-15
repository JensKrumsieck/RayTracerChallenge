#nullable enable
using RayTracer.Engine.Material;
using RayTracer.Extension;
using System;
using System.Numerics;

namespace RayTracer.Engine
{
    public class Transform
    {
        private Matrix4x4 _transformationMatrix;

        public Vector3 Position;

        public IMaterial Material = PhongMaterial.Default;

        public Matrix4x4 TransformationMatrix
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
            Position = new Vector3(TransformationMatrix.M14, TransformationMatrix.M24, TransformationMatrix.M34);
        }

        protected Transform() => TransformationMatrix = Matrix4x4.Identity;

        public virtual Vector3 Normal(Vector3 point) => new(0f, 0f, 0f);

        public virtual HitInfo[] Intersect(Ray ray, bool hit = false) => Array.Empty<HitInfo>();

        public HitInfo? Hit(Ray ray)
        {
            var xs = Intersect(ray, true);
            return xs.Length == 0 ? null : xs[0];
        }

        public override string ToString() => GetType().Name + ":" + Position;

        public Vector3 WorldToObject(Vector3 worldPoint) => worldPoint.Multiply(TransformationMatrix.Invert());

        public Vector3 ObjectToWorld(Vector3 objectPoint) => objectPoint.Multiply(Matrix4x4.Transpose(TransformationMatrix.Invert()));
    }
}
