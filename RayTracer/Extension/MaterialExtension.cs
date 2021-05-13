using RayTracer.Engine.Material;

namespace RayTracer.Extension
{
    public static class MaterialExtension
    {
        public static PhongMaterial WithAmbient(this PhongMaterial m, float ambient)
        {
            m.Ambient = ambient;
            return m;
        }
    }
}
