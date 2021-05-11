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
    }

    public sealed class NativeHitInfo
    {
        public readonly float Distance;
        public readonly NativeTransform HitObject;

        public NativeHitInfo(float distance, NativeTransform hitObject)
        {
            Distance = distance;
            HitObject = hitObject;
        }

        public override string ToString() => $"HitInfo: \n\tObject: {HitObject}\n\tDistance: {Distance}";
    }
}
