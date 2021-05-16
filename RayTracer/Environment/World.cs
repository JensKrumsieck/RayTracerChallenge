using RayTracer.Materials;
using RayTracer.Shapes;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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

        public Intersection[] Intersect(Ray ray)
        {
            var hits = new ConcurrentStack<Intersection>();
            Parallel.ForEach(Objects, s =>
            {
                var xs = s.Intersect(ray);
                if (xs.Length > 0) hits.PushRange(xs);
            });
            return hits.OrderBy(s => s.Distance).ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color ShadeHit(IntersectionState comps) =>
            comps.Object is not IShadedObject shaded ?
                Color.Black :
                shaded.Material.Shade(Lights[0], comps.Point, comps.Eye, comps.Normal);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color ColorAt(in Ray ray)
        {
            var xs = Intersect(ray);
            var hit = Intersection.Hit(xs);
            return hit == null ? Color.Black : ShadeHit(IntersectionState.Prepare(hit, ray));
        }
    }
}