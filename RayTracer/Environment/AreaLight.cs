using System.Numerics;

namespace RayTracer.Environment
{
    public struct AreaLight : ILight
    {

        public Vector4 Corner;
        public Vector4 UVec;
        public Vector4 VVec;
        public int USteps;
        public int VSteps;
        public Vector4 Position { get; set; }
        public Color Intensity { get; set; }
        public float Samples;

        public AreaLight(Vector4 corner, Vector4 uVec, int uSteps, Vector4 vVec, int vSteps, Color intensity) : this()
        {
            Corner = corner;
            UVec = uVec / uSteps;
            VVec = vVec / vSteps;
            USteps = uSteps;
            VSteps = vSteps;
            Intensity = intensity;
            Samples = USteps * VSteps;
            Position = (uVec / 2f + vVec / 2f);
        }

        public readonly Vector4 PointAt(int u, int v) => Corner + UVec * (u + .5f) + VVec * (v + .5f);

        public float IntensityAt(Vector4 point, World w)
        {
            var total = 0f;
            for (var v = 0; v < VSteps; v++)
            {
                for (var u = 0; u < USteps; u++)
                {
                    var pos = PointAt(u, v);
                    if (!w.InShadow(pos, point)) total += 1f;
                }
            }

            return total / Samples;
        }

    }
}
