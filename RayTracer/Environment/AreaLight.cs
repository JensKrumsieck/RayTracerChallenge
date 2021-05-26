﻿using System;
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
        public bool Jitter;
        private readonly Random rnd;

        public AreaLight(Vector4 corner, Vector4 uVec, int uSteps, Vector4 vVec, int vSteps, Color intensity) : this()
        {
            Corner = corner;
            UVec = uVec / uSteps;
            VVec = vVec / vSteps;
            USteps = uSteps;
            VSteps = vSteps;
            Intensity = intensity;
            Samples = USteps * VSteps;
            Position = uVec / 2f + vVec / 2f;
            Jitter = true;
            rnd = new Random(int.MaxValue/2);
        }

        public readonly Vector4 PointAt(int u, int v)
        {
            var jit1 = (float)(Jitter ? rnd.NextDouble() : .5);
            var jit2 = (float)(Jitter ? rnd.NextDouble() : .5);
            return Corner + UVec * (u + jit1 ) + VVec * (v + jit2);
        }

        public readonly float IntensityAt(Vector4 point, World w)
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
