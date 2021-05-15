using RayTracer.Extension;
using System;
using System.Numerics;
using System.Threading.Tasks;
using static RayTracer.Extension.Matrix;

namespace RayTracer.Engine.Camera
{
    public class Camera
    {
        public Vector2Int Resolution;
        public float FieldOfView;

        public float PixelSize => HalfWidth * 2f / Resolution.X;
        public float AspectRatio => (float)Resolution.X / Resolution.Y;
        private float HalfView => MathF.Tan(FieldOfView / 2f);
        public float HalfWidth => AspectRatio >= 1 ? HalfView : HalfView * AspectRatio;
        public float HalfHeight => AspectRatio >= 1 ? HalfView / AspectRatio : HalfView;

        private Matrix4x4 _transform;
        private Matrix4x4 _inverseTransform;
        public Matrix4x4 Transform
        {
            get => _transform;
            set
            {
                _transform = value;
                _inverseTransform = _transform.Invert();
            }
        }

        public Camera(int width, int height, float foV)
        {
            Resolution = new Vector2Int(width, height);
            FieldOfView = foV;
            Transform = Matrix4x4.Identity;
        }

        public Ray RayTo(int x, int y)
        {
            var wX = HalfWidth - (x + .5f) * PixelSize;
            var wY = HalfHeight - (y + .5f) * PixelSize;
            var px = new Vector3(wX, wY, -1f).Multiply(_inverseTransform);
            var origin = Vector3.Zero.Multiply(_inverseTransform);
            return new Ray(origin, Vector3.Normalize(px - origin));
        }

        public Viewport Render(World w)
        {
            var view = new Viewport(Resolution.X, Resolution.Y);
            Parallel.For(0, Resolution.Y, y =>
            {
                for (var x = 0; x < Resolution.X; x++)
                {
                    var r = RayTo(x, y);
                    view.SetPixel(x, y, w.ColorAt(r));
                }
            });
            return view;
        }

        public static Matrix4x4 ViewTransform(Vector3 from, Vector3 to, Vector3 up)
        {
            var forward = Vector3.Normalize(to - from);
            var upNormal = Vector3.Normalize(up);
            var left = Vector3.Cross(forward, upNormal);
            var trueUp = Vector3.Cross(left, forward);

            var orientation = new Matrix4x4(
                left.X, left.Y, left.Z, 0f,
                trueUp.X, trueUp.Y, trueUp.Z, 0f,
                -forward.X, -forward.Y, -forward.Z, 0f,
                0f, 0f, 0f, 1f);
            return orientation * TranslationMatrix(-from);
        }
    }
}
