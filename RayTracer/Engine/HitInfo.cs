using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace RayTracer.Engine
{
    public sealed class HitInfo
    {
        public readonly float Distance;
        public readonly Transform HitObject;

        public HitInfo(float distance, Transform hitObject)
        {
            Distance = distance;
            HitObject = hitObject;
        }

        public override string ToString() => $"HitInfo: \n\tObject: {HitObject}\n\tDistance: {Distance}";

        public static HitInfo? DetermineHit(IEnumerable<HitInfo> xs) => xs.FirstOrDefault(i => i.Distance > 0);
    }
}
