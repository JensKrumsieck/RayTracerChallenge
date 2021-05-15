using RayTracer.Engine.Lighting;
using System;
using System.Numerics;

namespace RayTracer.Engine.Material
{
    public struct PhongMaterial : IMaterial
    {
        public Color BaseColor { get; set; }
        public float Ambient;
        public float Diffuse;
        public float Specular;
        public float Shininess;

        public PhongMaterial(Color color, float ambient = .1f, float diffuse = .9f, float specular = .9f, float shininess = 200f)
        {
            BaseColor = color;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shininess;
        }

        public static PhongMaterial Default => new(Color.White);

        public readonly Color Shade(ILight light, IntersectionPoint p)
        {
            var effectiveCol = BaseColor * light.Intensity;
            //get direction to light source
            var lightDir = Vector3.Normalize(light.Position - p.HitPoint);

            //calculate ambient
            var ambient = effectiveCol * Ambient;

            var lightNormal = Vector3.Dot(lightDir, p.Normal);

            Color diffuse;
            Color specular;
            if (lightNormal < 0)
            {
                diffuse = Color.Black;
                specular = Color.Black;
            }
            else
            {
                diffuse = effectiveCol * Diffuse * lightNormal;
                var reflect = Vector3.Reflect(-lightDir, p.Normal);
                var reflectDotEye = Vector3.Dot(reflect, p.Eye);
                if (reflectDotEye <= 0) specular = Color.Black;
                else
                {
                    var factor = MathF.Pow(reflectDotEye, Shininess);
                    specular = light.Intensity * Specular * factor;
                }
            }
            return ambient + diffuse + specular;
        }

        public override readonly string ToString() =>
            $"PhongMaterial: {BaseColor}\n\tAmbient: {Ambient}\n\tDiffuse: {Diffuse}\n\tSpecular: {Specular}\n\tShininess: {Shininess}";
    }
}
