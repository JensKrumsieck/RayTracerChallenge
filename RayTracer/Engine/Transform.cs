#nullable enable
using RayTracer.Engine.Material;
using RayTracer.Extension;
using System.Numerics;

namespace RayTracer.Engine
{
    public abstract class Transform
    {
        private Matrix4x4 _transformationMatrix;
        private Matrix4x4 _inverseTransform;

        public Vector3 Position { get; private set; }

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
            //TODO: Scale & Rot
            _inverseTransform = _transformationMatrix.Invert();
        }

        protected Transform() => TransformationMatrix = Matrix4x4.Identity;

        public abstract Vector3 Normal(Vector3 point);

        public abstract HitInfo[] Intersect(Ray ray);

        public HitInfo? Hit(Ray ray) => HitInfo.DetermineHit(Intersect(ray));

        public override string ToString() => GetType().Name + ":" + Position;

        public Vector3 WorldToObject(Vector3 worldPoint) => worldPoint.Multiply(_inverseTransform);

        public Vector3 ObjectToWorld(Vector3 objectPoint) => objectPoint.Multiply(Matrix4x4.Transpose(_inverseTransform));
    }
}
