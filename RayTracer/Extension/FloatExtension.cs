using System;

namespace RayTracer.Extension
{
    public static class FloatExtension
    {
        public static byte ToByte(this float f) => (byte)Math.Round(Math.Clamp(f, 0, 1) * 255f, MidpointRounding.AwayFromZero);

        public static bool Equal(this float f, float l) => MathF.Abs(f - l) < Constants.Epsilon;
    }
}
