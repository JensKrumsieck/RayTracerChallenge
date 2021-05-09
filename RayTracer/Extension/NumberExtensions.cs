using System;
using static RayTracer.Constants;

namespace RayTracer.Extension
{
    public static class NumberExtensions
    {
        public static bool Equal(this float me, float other) => MathF.Abs(me - other) <= Epsilon;

        public static byte ToByte(this float f) => (byte) Math.Round(Math.Clamp(f, 0, 1) * 255f, MidpointRounding.AwayFromZero);
    }
}
