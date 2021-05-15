#nullable enable
using RayTracer.Engine.Lighting;
using RayTracer.Engine.Material;
using RayTracer.Primitives;
using System.Collections.Concurrent;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace RayTracer.Engine
{
    public class World
    {
        public Transform[] Objects;

        public PointLight Light;

        public static World Default => new()
        {
            Light =  new PointLight(new Vector3(-10, 10, -10), Color.White),
            Objects = new Transform[]
            {
                new Sphere
                {
                    Material = new PhongMaterial(new Color(.8f, 1f, .6f), .1f, .7f, .2f, 200f)
                },
                new Sphere(.5f)
            }
        };

        public bool Intersections(Ray ray, out HitInfo[] hitInfo)
        {
            var hits = new ConcurrentBag<HitInfo>();
            Parallel.ForEach(Objects, o =>
            {
                var newHits = o.Intersect(ray);
                foreach (var hit in newHits) hits.Add(hit);
            });
            hitInfo = hits.OrderBy(s => s.Distance).ToArray();
            return hitInfo.Length != 0;
        }

        public bool Hit(Ray ray, out HitInfo? hit)
        {
            hit = null;
            if (!Intersections(ray, out var hitInfo)) return false;
            hit = HitInfo.DetermineHit(hitInfo);
            return hit != null;
        }

        public Color Shade(IntersectionPoint p)
        {
            var shadowed = ShadowCheck(p.OverPoint);
            return p.Object.Material.Shade(Light, p, shadowed);
        }

        public Color ColorAt(Ray ray) => !Hit(ray, out var hit) ? Color.Black : Shade(IntersectionPoint.Prepare(hit, ray));

        public bool ShadowCheck(Vector3 p)
        {
            var v = Light.Position - p;
            var dist = v.Length();
            var dir = Vector3.Normalize(v);
            var r = new Ray(p, dir);
            if (!Hit(r, out var hit)) return false;
            return hit?.Distance < dist;
        }
    }
}
