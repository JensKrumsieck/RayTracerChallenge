using RayTracer.Extension;
using RayTracer.Lights;
using System;
using System.Numerics;

namespace RayTracer.Materials
{
    public struct PhongMaterial
    {
        public Color BaseColor;
        public float Ambient { get; set; }
        public float Diffuse;
        public float Specular;
        public float Shininess;

        public static PhongMaterial Default => new(Color.White);

        public PhongMaterial(Color baseColor, float diffuse = .9f, float specular = .9f, float ambient = .1f, float shininess = 200f)
        {
            BaseColor = baseColor;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shininess;
        }

        public Color Shade(PointLight l, Vector4 point, Vector4 eye, Vector4 normal)
        {
            var effCol = BaseColor * l.Intensity;
            var lightV = Vector4.Normalize(l.Position - point);
            var ambient = effCol * Ambient;

            var lightDotNormal = Vector4.Dot(lightV, normal);
            if (lightDotNormal < 0) return ambient;
            var diffuse = effCol * Diffuse * lightDotNormal;
            var reflect = (-lightV).Reflect(normal);
            var reflectDotEye = Vector4.Dot(reflect, eye);
            if (reflectDotEye <= 0) return ambient + diffuse;
            var pow = MathF.Pow(reflectDotEye, Shininess);
            var specular = l.Intensity * Specular * pow;
            return ambient + diffuse + specular;
        }
    }
}
