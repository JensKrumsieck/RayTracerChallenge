using RayTracer.Materials;
using RayTracer.Shapes;
using System;
using System.Collections.Generic;
using System.Numerics;
using static RayTracer.Extension.MatrixExtension;

namespace RayTracer.Environment
{
    public class World
    {
        public static World Default => new()
        {
            Lights = new List<ILight> { PointLight.Default },
            Objects = new List<Entity>
            {
                new Sphere
                {
                    Material = new PhongMaterial(new Color(.8f, 1f, .6f)){Diffuse = .7f, Specular = .2f}
                },
                new Sphere(Scale(.5f))
            }
        };

        public List<ILight> Lights = new();
        public List<Entity> Objects = new();

        public void Intersect(ref Ray ray, ref List<Intersection> hits)
        {
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var entity in Objects)
            {
                entity.Intersect(ref ray, ref hits);
            }
            hits.Sort();
        }

        public Color ShadeHit(ref IntersectionState comps, int limit = 0)
        {
            var intensity = Lights[0].IntensityAt(comps.OverPoint, this);
            var surfaceColor = comps.Object.Material.Shade(Lights[0], ref comps, intensity);
            var reflectedColor = ReflectedColor(ref comps, limit);
            var refractedColor = RefractedColor(ref comps, limit);

            var material = comps.Object.Material;
            if (material.Reflectivity <= 0 || material.Transparency <= 0)
                return surfaceColor + reflectedColor + refractedColor;

            var reflectance = comps.Schlick();
            return surfaceColor + reflectedColor * reflectance + refractedColor * (1 - reflectance);
        }

        public Color ColorAt(ref Ray ray, int limit = 0)
        {
            var xs = new List<Intersection>();
            Intersect(ref ray, ref xs);
            var hit = Intersection.Hit(ref xs);
            if (hit == null)
                return Color.Black;
            var comp = IntersectionState.Prepare(ref hit, ref ray, xs);
            return ShadeHit(ref comp, limit);
        }

        public bool InShadow(Vector4 lightPosition, Vector4 point)
        {
            var xs = new List<Intersection>();
            var v = lightPosition - point;
            var dir = Vector4.Normalize(v);
            var dis = v.LengthSquared();
            var r = new Ray(point, dir);
            foreach (var obj in Objects)
            {
                xs.Clear();
                obj.Intersect(ref r, ref xs);
                var hit = Intersection.Hit(ref xs);
                if (hit != null && hit.Distance * hit.Distance < dis) return true;
            }
            return false;
        }

        public Color ReflectedColor(ref IntersectionState comps, int limit = 0)
        {
            if (comps.Object.Material.Reflectivity == 0f || limit == 0) return Color.Black;
            var refRay = new Ray(comps.OverPoint, comps.Reflect);
            var col = ColorAt(ref refRay, limit - 1);
            return col * comps.Object.Material.Reflectivity;
        }

        public Color RefractedColor(ref IntersectionState comps, int limit)
        {
            if (comps.Object.Material.Transparency == 0 || limit == 0) return Color.Black;

            var ratio = comps.N1 / comps.N2;
            var cos = Vector4.Dot(comps.Eye, comps.Normal);
            var sin2 = ratio * ratio * (1 - cos * cos);
            if (sin2 > 1f) return Color.Black;

            var cost = MathF.Sqrt(1.0f - sin2);
            var dir = comps.Normal * (ratio * cos - cost) - comps.Eye * ratio;
            var ray = new Ray(comps.UnderPoint, dir);

            return ColorAt(ref ray, limit - 1) * comps.Object.Material.Transparency;
        }
    }
}