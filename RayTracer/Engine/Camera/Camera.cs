using RayTracer.Extension;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace RayTracer.Engine.Camera
{
    public class Camera : Transform
    {
        public Vector2Int Resolution;
        public float FieldOfView;

        public float PixelSize => HalfWidth * 2f / Resolution.X;
        public float AspectRatio => Resolution.X / (float)Resolution.Y;
        private float HalfView => MathF.Tan(FieldOfView / 2f);
        public float HalfWidth => AspectRatio >= 1 ? HalfView : HalfView * AspectRatio;
        public float HalfHeight => AspectRatio >= 1 ? HalfView / AspectRatio : HalfView;

        public Camera(int width, int height, float foV)
        {
            Resolution = new Vector2Int(width, height);
            FieldOfView = foV;
        }

        public Ray RayTo(int x, int y)
        {
            var wX = HalfWidth - (x + .5f) * PixelSize;
            var wY = HalfHeight - (y + .5f) * PixelSize;
            var inv = TransformationMatrix.Invert();
            var px = Vector3.Transform(new Vector3(wX, wY, -1f), inv);
            var origin = Vector3.Transform(Vector3.Zero, inv);
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

        public static Matrix4x4 ViewTransform(Vector3 from, Vector3 to, Vector3 up) => Matrix4x4.CreateLookAt(from, to, up);
        //var zaxis = Vector3.Normalize(to - from);
        //var xaxis = Vector3.Cross(zaxis, Vector3.Normalize(up));
        //var yaxis = Vector3.Cross(xaxis, zaxis);
        //Matrix4x4 result;
        //result.M11 = xaxis.X;
        //result.M12 = xaxis.Y;
        //result.M13 = xaxis.Z;
        //result.M14 = 0.0f;
        //result.M21 = yaxis.X;
        //result.M22 = yaxis.Y;
        //result.M23 = yaxis.Z;
        //result.M24 = 0.0f;
        //result.M31 = -zaxis.X;
        //result.M32 = -zaxis.Y;
        //result.M33 = -zaxis.Z;
        //result.M34 = 0.0f;
        //result.M41 = 0.0f;
        //result.M42 = 0.0f;
        //result.M43 = 0.0f;
        //result.M44 = 1.0f;
        //return Matrix4x4.Multiply(result, Matrix4x4.Transpose(Matrix4x4.CreateTranslation(-from)));
    }
}
