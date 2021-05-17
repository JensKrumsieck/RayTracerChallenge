﻿using RayTracer.Environment;
using RayTracer.Extension;
using RayTracer.Materials.Patterns;
using System;
using System.Numerics;

namespace RayTracer.Materials
{
    public class PhongMaterial : IEquatable<PhongMaterial>
    {
        public Color BaseColor;
        public float Ambient { get; set; }
        public float Diffuse;
        public float Specular;
        public float Shininess;
        public Pattern? Pattern;

        public static PhongMaterial Default => new(Color.White);

        public PhongMaterial(Color baseColor, float diffuse = .9f, float specular = .9f, float ambient = .1f, float shininess = 200f)
        {
            BaseColor = baseColor;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shininess;
        }

        public Color Shade(PointLight l, IntersectionState c, bool inShadow = false)
        {
            var effCol = (Pattern?.ColorAtEntity(c.Object, c.Point) ?? BaseColor) * l.Intensity;
            var lightV = Vector4.Normalize(l.Position - c.Point);
            var ambient = effCol * Ambient;

            var lightDotNormal = Vector4.Dot(lightV, c.Normal);
            if (lightDotNormal < 0 || inShadow) return ambient;

            var diffuse = effCol * Diffuse * lightDotNormal;
            var reflect = (-lightV).Reflect(c.Normal);
            var reflectDotEye = Vector4.Dot(reflect, c.Eye);
            if (reflectDotEye <= 0) return ambient + diffuse;

            var pow = MathF.Pow(reflectDotEye, Shininess);
            var specular = l.Intensity * Specular * pow;
            return ambient + diffuse + specular;
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
