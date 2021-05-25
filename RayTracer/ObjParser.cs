#nullable disable
using RayTracer.Extension;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using Group = RayTracer.Shapes.Group;

namespace RayTracer
{
    public class ObjParser
    {
        private readonly IEnumerable<string> _lines;

        public List<Vector4> Vertices;
        public List<Vector4> Normals;
        public Group Group;
        private Group _currentGroup;

        public ObjParser(string dataOrPath)
        {
            var content = Path.GetExtension(dataOrPath) == ".obj" ? File.ReadAllText(dataOrPath) : dataOrPath;
            _lines = content.Split(System.Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        }

        public void Parse()
        {
            Vertices = new List<Vector4>();
            Normals = new List<Vector4>();
            Group = new Group();
            _currentGroup = Group; //set current to root
            ReadData();
        }

        private void ReadData()
        {
            foreach (var l in _lines)
            {
                var items = l.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (items[0] == "vn") ParseNormal(items);
                if (items[0] == "v") ParseVertex(items);
                if (items[0] == "g")
                {
                    var newGroup = new Group();
                    Group.AddChild(newGroup);
                    _currentGroup = new Group();
                }
                if (items[0] != "f") continue;
                var res = ParsePolygons(items);
                List<Vector4> polygon = new();
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var index in res) polygon.Add(Vertices[index - 1]);
                var tris = polygon.FanTriangulation();
                // ReSharper disable once CoVariantArrayConversion
                if (tris != null) _currentGroup.AddChildrenWithoutRefresh(tris);
            }
            Group.BoundingBox = Group.ComputeBounds();
        }

        private void ParseVertex(string[] items) => Vertices.Add(ParseVector(items, 1f));
        private void ParseNormal(string[] items) => Normals.Add(ParseVector(items, 0f));


        private static Vector4 ParseVector(IReadOnlyList<string> items, float w)
        {
            var x = float.Parse(items[1], CultureInfo.InvariantCulture);
            var y = float.Parse(items[2], CultureInfo.InvariantCulture);
            var z = float.Parse(items[3], CultureInfo.InvariantCulture);
            return new Vector4(x, y, z, w);
        }

        private static int[] ParsePolygons(string[] items) => items.Skip(1).Select(int.Parse).ToArray();
    }
}
