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

        public PhongMaterial(Color color, float ambient, float diffuse, float specular, float shininess)
        {
            BaseColor = color;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shininess;
        }

        public static PhongMaterial Default => new(Color.White, .1f, .9f, .9f, 200f);

        public readonly Color Shade(ILight light, Vector3 point, Vector3 eye, Vector3 normal)
        {
            var effectiveCol = BaseColor * light.Intensity;
            //get direction to light source
            var lightDir = Vector3.Normalize(light.Position - point);

            //calculate ambient
            var ambient = effectiveCol * Ambient;

            var lightNormal = Vector3.Dot(lightDir, normal);

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
                var reflect = Vector3.Reflect(-lightDir, normal);
                var reflectDotEye = Vector3.Dot(reflect, eye);
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
