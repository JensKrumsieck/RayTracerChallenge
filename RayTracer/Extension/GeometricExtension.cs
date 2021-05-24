using System;

namespace RayTracer.Extension
{
    public static class GeometricExtension
    {
        public static (float, float) CheckAxis(float origin, float direction, float min = -1f, float max = 1f)
        {
            var tMinNum = min - origin;
            var tMaxNum = max - origin;
            float tMin;
            float tMax;
            if (MathF.Abs(direction) >= Constants.Epsilon)
            {
                tMin = tMinNum / direction;
                tMax = tMaxNum / direction;
            }
            else
            {
                tMin = tMinNum * float.PositiveInfinity;
                tMax = tMaxNum * float.PositiveInfinity;
            }

            return tMin > tMax ? (tMax, tMin) : (tMin, tMax);
        }
    }
}
