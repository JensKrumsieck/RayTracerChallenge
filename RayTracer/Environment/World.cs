using RayTracer.Materials;
using RayTracer.Shapes;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
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
                    Material = new PhongMaterial(new Color(.8f, 1f, .6f), .7f, .2f)
                },
                new Sphere(Scale(.5f))
            }
        };


        public List<PointLight> Lights = new();
        public List<Entity> Objects = new();

        public List<Intersection> Intersect(Ray ray)
        {
            var hits = new List<Intersection>();
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var entity in Objects)
            {
                var xs = entity.Intersect(ray);
                if (xs.Count > 0) hits.AddRange(xs);
            }
            hits.Sort();
            return hits;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color ShadeHit(IntersectionState comps) => comps.Object.Material.Shade(Lights[0], comps.Point, comps.Eye, comps.Normal, InShadow(comps.OverPoint));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color ColorAt(in Ray ray)
        {
            var xs = Intersect(ray);
            var hit = Intersection.Hit(xs);
            return hit == null ? Color.Black : ShadeHit(IntersectionState.Prepare(hit, ray));
        }

        public bool InShadow(Vector4 point)
        {
            var v = Lights[0].Position - point;
            var dis = v.Length();
            var dir = Vector4.Normalize(v);

            var xs = Intersect(new Ray(point, dir));
            var hit = Intersection.Hit(xs);
            return hit != null && hit.Distance < dis;
        }
    }
}