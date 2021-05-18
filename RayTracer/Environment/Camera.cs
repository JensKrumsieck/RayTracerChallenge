using RayTracer.Drawing;
using RayTracer.Extension;
using System;
using System.Numerics;
using System.Threading.Tasks;
using static RayTracer.Extension.MatrixExtension;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Environment
{
    public struct Camera
    {
        public Vector2Int Resolution;
        public float FieldOfView;
        public Transform Transform;

        public readonly float Aspect => (float)Resolution.X / Resolution.Y;
        public readonly float HalfView => MathF.Tan(FieldOfView / 2f);
        public readonly float HalfWidth => Aspect >= 1 ? HalfView : HalfView * Aspect;
        public readonly float HalfHeight => Aspect >= 1 ? HalfView / Aspect : HalfView;
        public readonly float PixelSize => (HalfWidth * 2) / Resolution.X;

        public Camera(Vector2Int resolution, float fieldOfView)
        {
            Resolution = resolution;
            FieldOfView = fieldOfView;
            Transform = Matrix4x4.Identity;
        }


        public Camera(int width, int height, float fieldOfView) : this(new Vector2Int(width, height), fieldOfView) { }

        public Ray RayTo(int x, int y)
        {
            var xOff = (x + .5f) * PixelSize;
            var yOff = (y + .5f) * PixelSize;
            var worldX = HalfWidth - xOff;
            var worldY = HalfHeight - yOff;

            var px = Transform.Inverse.Multiply(Point(worldX, worldY, -1f));
            var origin = Transform.Inverse.Multiply(Vector4.UnitW);
            origin.W = 1f;
            var dir = Vector4.Normalize(px - origin);
            return new Ray(origin, dir);
        }

        public static Transform ViewTransform(Vector4 from, Vector4 to, Vector4 up)
        {
#if DEBUG
            if (!from.IsPoint() || !to.IsPoint() || !up.IsVector())
                throw new InvalidOperationException("from, to, up mismatched types");
#endif
            var forward = Vector4.Normalize(to - from);
            var upN = Vector4.Normalize(up);
            var left = forward.Cross(upN);
            var trueUp = left.Cross(forward);

            var orientation = new Matrix4x4(
                left.X, left.Y, left.Z, 0f,
                trueUp.X, trueUp.Y, trueUp.Z, 0f,
                -forward.X, -forward.Y, -forward.Z, 0f,
                0f, 0f, 0f, 1f);
            return orientation * Translation(-from.X, -from.Y, -from.Z);
        }

        public readonly Canvas Render(World world)
        {
            var c = new Canvas(Resolution);
            var self = this;
            Parallel.For(0, self.Resolution.Y, new ParallelOptions{ MaxDegreeOfParallelism = System.Environment.ProcessorCount}, y =>
            {
                for (var x = self.Resolution.X - 1; x >= 0; x--)
                {
                    var r = self.RayTo(x, y);
                    c[x, y] = world.ColorAt(ref r);
                }
            });
            return c;
        }
    }
}
