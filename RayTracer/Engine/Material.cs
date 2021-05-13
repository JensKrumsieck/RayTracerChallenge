using System;
using System.Numerics;
using RayTracer.Engine.Lighting;

namespace RayTracer.Engine
{
    public struct Material
    {
        public Color BaseColor;
        public float Ambient;
        public float Diffuse;
        public float Specular;
        public float Shininess;

        public Material(Color color, float ambient, float diffuse, float specular, float shininess)
        {
            BaseColor = color;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shininess;
        }

        public static readonly Material DefaultMaterial = new(Color.White, .1f, .9f, .9f, 200f);

        public readonly Color Lighten(ILight light, Vector3 point, Vector3 eye, Vector3 normal)
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
                if(reflectDotEye <= 0) specular = Color.Black;
                else
                {
                    var factor = MathF.Pow(reflectDotEye, Shininess);
                    specular = light.Intensity * Specular * factor;
                }
            }
            return ambient + diffuse + specular;
        }
    }
}
