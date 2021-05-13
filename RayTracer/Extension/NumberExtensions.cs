using System;
using static RayTracer.Constants;

namespace RayTracer.Extension
{
    public static class NumberExtensions
    {
        public static bool Equal(this float me, float other, float threshold = Epsilon) => MathF.Abs(me - other) <= threshold;

        public static byte ToByte(this float f) => (byte)Math.Round(Math.Clamp(f, 0, 1) * 255f, MidpointRounding.AwayFromZero);
    }
}
