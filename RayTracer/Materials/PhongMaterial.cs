using RayTracer.Environment;
using RayTracer.Extension;
using RayTracer.Materials.Patterns;
using System;
using System.Numerics;

namespace RayTracer.Materials
{
    public class PhongMaterial : IEquatable<PhongMaterial>
    {
        public Color BaseColor;
        public float Ambient = .1f;
        public float Diffuse = .9f;
        public float Specular = .9f;
        public float Shininess = 200f;
        public float Reflectivity = 0f;
        public float IOR = 1.0f;
        public float Transparency = 0f;
        public Pattern? Pattern;

        public static PhongMaterial Default => new(Color.White);

        public PhongMaterial(Color baseColor)
        {
            BaseColor = baseColor;
        }

        public Color Shade(in ILight l, ref IntersectionState c, float intensity)
        {
            var effCol = (Pattern?.ColorAtEntity(c.Object, c.Point) ?? BaseColor) * l.Intensity;
            var ambient = effCol * Ambient;
            var diffuseColor = effCol * Diffuse;
            var specularColor = l.Intensity * Specular;
            var diffuse = Color.Black;
            var specular = Color.Black;
            foreach (var sample in l.GetSamples())
            {
                var lightV = Vector4.Normalize(sample - c.Point);
                var lightDotNormal = Vector4.Dot(lightV, c.Normal);
                if (lightDotNormal < 0) continue;

                diffuse += diffuseColor * lightDotNormal * intensity;
                var reflect = (-lightV).Reflect(c.Normal);
                var reflectDotEye = Vector4.Dot(reflect, c.Eye);
                if (reflectDotEye <= 0) continue;
                var pow = MathF.Pow(reflectDotEye, Shininess);
                specular += specularColor * pow * intensity;
            }
            return ambient + diffuse / l.Samples + specular / l.Samples;
        }

        /// <inheritdoc />
        public bool Equals(PhongMaterial? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var patternCheck = (Pattern == null && other.Pattern == null) ||
                               Pattern != null && Pattern.Equals(other.Pattern);
            return BaseColor.Equals(other.BaseColor) &&
                   Diffuse.Equals(other.Diffuse) &&
                   Specular.Equals(other.Specular) &&
                   Shininess.Equals(other.Shininess) &&
                   patternCheck &&
                   Ambient.Equals(other.Ambient);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((PhongMaterial)obj);
        }

        /// <inheritdoc />
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        public override int GetHashCode() => HashCode.Combine(BaseColor, Diffuse, Specular, Shininess, Pattern, Ambient);
    }
}
