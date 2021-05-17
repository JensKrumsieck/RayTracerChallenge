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
        public static IEnumerable<Sphere> LoadMesitaldehydeAtoms()
        {
            const string file = "files/mescho.xyz";
            var mol = MoleculeFactory.Create(file);
            var materials = new Dictionary<string, PhongMaterial>
            {
                ["O"] = new(FromHex(new Element("O").Color)),
                ["C"] = new(FromHex(new Element("C").Color)),
                ["H"] = new(FromHex(new Element("H").Color))
            };
            foreach (var a in mol.Atoms)
            {
                Debug.Assert(a.CovalentRadius != null, "a.CovalentRadius != null");
                yield return new Sphere(Translation(a.Location) * Scale(a.CovalentRadius.Value / 150f)) { Material = materials[a.Symbol] };
            }
        }

        public static Color FromHex(string hex)
        {
            var sysCol = System.Drawing.ColorTranslator.FromHtml(hex);
            return new Color(sysCol.R / 255f, sysCol.G / 255f, sysCol.B / 255f);
        }
    }
}
