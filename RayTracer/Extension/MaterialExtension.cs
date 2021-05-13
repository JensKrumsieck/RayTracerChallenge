using RayTracer.Engine;
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
        public static PhongMaterial WithColor(this PhongMaterial m, Color col)
        {
            m.BaseColor = col;
            return m;
        }
        public static PhongMaterial WithDiffuse(this PhongMaterial m, float diffuse)
        {
            m.Diffuse = diffuse;
            return m;
        }
        public static PhongMaterial WithSpecular(this PhongMaterial m, float specular)
        {
            m.Diffuse = specular;
            return m;
        }
    }
}
