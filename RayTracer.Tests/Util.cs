using ChemSharp.Molecules;
using RayTracer.Materials;
using RayTracer.Shapes;
using System.Collections.Generic;
using System.Diagnostics;
using static RayTracer.Extension.MatrixExtension;

namespace RayTracer.Tests
{
    public static class Util
    {
        private static IEnumerable<Sphere> LoadAtoms(string file,  Dictionary<string, PhongMaterial> materials)
        {
            var mol = MoleculeFactory.Create(file);
            foreach (var a in mol.Atoms)
            {
                Debug.Assert(a.CovalentRadius != null, "a.CovalentRadius != null");
                yield return new Sphere(Translation(a.Location) * Scale(a.CovalentRadius.Value / 150f)) { Material = materials[a.Symbol] };
            }
        }

        public static IEnumerable<Sphere> LoadMesitaldehydeAtoms()
        {
            const string file = "files/mescho.xyz";
            var materials = new Dictionary<string, PhongMaterial>
            {
                ["O"] = new(FromHex(new Element("O").Color)),
                ["C"] = new(FromHex(new Element("C").Color)),
                ["H"] = new(FromHex(new Element("H").Color))
            };
            return LoadAtoms(file, materials);
        }

        public static Color FromHex(string hex)
        {
            var sysCol = System.Drawing.ColorTranslator.FromHtml(hex);
            return new Color(sysCol.R / 255f, sysCol.G / 255f, sysCol.B / 255f);
        }
    }
}
