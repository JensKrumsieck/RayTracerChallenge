using RayTracer.Materials;
using RayTracer.Shapes;
using System.Collections.Generic;
using System.Numerics;
using static RayTracer.Extension.MatrixExtension;

namespace RayTracer.Environment
{
    public class World
    {
        public static World Default => new()
        {
            Lights = new List<PointLight> { PointLight.Default },
            Objects = new List<Entity>
            {
                new Sphere
                {
                    Material = new PhongMaterial(new Color(.8f, 1f, .6f)){Diffuse = .7f, Specular = .2f}
                },
                new Sphere(Scale(.5f))
            }
        };


        public List<PointLight> Lights = new();
        public List<Entity> Objects = new();

        public List<Intersection> Intersect(ref Ray ray)
        {
            var hits = new List<Intersection>();
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var entity in Objects)
            {
                entity.Intersect(ref ray, ref hits);
            }
            hits.Sort();
            return hits;
        }

        public Color ShadeHit(ref IntersectionState comps) => comps.Object.Material.Shade(Lights[0], in comps, InShadow(comps.OverPoint));

        public Color ColorAt(ref Ray ray)
        {
            var xs = Intersect(ref ray);
            var hit = Intersection.Hit(ref xs);
            if (hit == null)
                return Color.Black;
            var comp = IntersectionState.Prepare(ref hit, ref ray);
            return ShadeHit(ref comp);
        }

        public bool InShadow(Vector4 point)
        {
            var v = Lights[0].Position - point;
            var dis = v.Length();
            var dir = Vector4.Normalize(v);
            var r = new Ray(point, dir);
            var xs = Intersect(ref r);
            var hit = Intersection.Hit(ref xs);
            return hit != null && hit.Distance < dis;
        }
    }
}