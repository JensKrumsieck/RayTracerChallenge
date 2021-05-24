using ChemSharp.Molecules;
using RayTracer.Materials;
using RayTracer.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using static RayTracer.Extension.MatrixExtension;

namespace RayTracer.Tests
{
    public static class Util
    {
        private static IEnumerable<Entity> LoadAtoms(string file, Dictionary<string, PhongMaterial> materials)
        {
            var mol = MoleculeFactory.Create(file);
            foreach (var a in mol.Atoms)
            {
                Debug.Assert(a.CovalentRadius != null, "a.CovalentRadius != null");
                yield return new Sphere(Translation(a.Location) * Scale(a.CovalentRadius.Value / 150f)) { Material = materials[a.Symbol] };
            }

            foreach (var b in mol.Bonds)
            {
                var start = b.Atom1.Location;
                var end = b.Atom2.Location;
                var lineVec = end - start;
                var axis = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, lineVec));
                var angle = MathF.Acos(Vector3.Dot(Vector3.UnitY, Vector3.Normalize(lineVec)));
                var mat2 = Matrix4x4.Transpose(Matrix4x4.CreateFromAxisAngle(axis, angle));
                var transform = Translation(start) * mat2 * Scale(.1f, lineVec.Length(), .1f);

                var cy = new Cylinder(transform) { IsClosed = true, Maximum = 1, Minimum = 0, Material = new PhongMaterial(Util.FromHex("#cccccc")) };
                yield return cy;
            }
        }

        public static IEnumerable<Entity> LoadMesitaldehydeAtoms()
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
