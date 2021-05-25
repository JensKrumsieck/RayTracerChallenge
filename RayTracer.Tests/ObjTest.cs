using Microsoft.VisualStudio.TestTools.UnitTesting;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests
{
    [TestClass]
    public class ObjTest
    {
        [TestMethod]
        public void IgnoreGibberish()
        {
            const string gibberisch = @"
Lorem ipsum, dolor sit amet consectetur adipisicing elit. 
Iusto id fuga accusamus quia! Alias quisquam ullam hic 
vero magnam sed.
";
            var p = new ObjParser(gibberisch);
            p.Parse();
            Assert.AreEqual(p.Vertices.Count, 0);
            Assert.AreEqual(p.Normals.Count, 0);
            Assert.AreEqual(p.Group.Count, 0);
        }

        [TestMethod]
        public void ParseVertices()
        {
            const string vertices = @"
v -1 1 0
v -1.000000  0.50000  0.000
v 1 0 0
v 1 1 0";
            var parser = new ObjParser(vertices);
            parser.Parse();
            Assert.AreEqual(parser.Vertices.Count, 4);
            Assert.AreEqual(parser.Vertices[0], Point(-1, 1, 0));
            Assert.AreEqual(parser.Vertices[1], Point(-1, .5f, 0));
            Assert.AreEqual(parser.Vertices[2], Point(1, 0, 0));
            Assert.AreEqual(parser.Vertices[3], Point(1, 1, 0));
        }

        [TestMethod]
        public void ParseFaces()
        {
            const string obj = @"
v -1 1 0
v -1 0 0
v 1 0 0
v 1 1  0

f 1 2 3
f 1 3 4";
            var parser = new ObjParser(obj);
            parser.Parse();

            Assert.AreEqual(parser.Vertices.Count, 4);
            Assert.AreEqual(parser.Group.Count, 2);
        }

        [TestMethod]
        public void TriangulateFaces()
        {
            const string obj = @"
v -1 1 0
v -1 0 0
v 1 0 0
v 1 1  0
v 0 2 0

f 1 2 3 4 5";
            var parser = new ObjParser(obj);
            parser.Parse();
            Assert.AreEqual(parser.Vertices.Count, 5);
            Assert.AreEqual(parser.Group.Count, 3);
        }

        [TestMethod]
        public void ParseGroups()
        {
            const string obj = @"
v -1 1 0
v -1 0 0
v 1 0 0
v 1 1  0

g FirstGroup
f 1 2 3
g SecondGroup
f 1 3 4";
            var parser = new ObjParser(obj);
            parser.Parse();

            Assert.AreEqual(parser.Vertices.Count, 4);
            Assert.AreEqual(parser.Group.Count, 2);
        }

        [TestMethod]
        public void ParseVertexNormals()
        {
            const string obj = @"
vn 0 0 1
vn 0.707 0 -0.707
vn 1 2 3 ";
            var parser = new ObjParser(obj);
            parser.Parse();
            Assert.AreEqual(parser.Normals[0], Direction(0, 0, 1));
            Assert.AreEqual(parser.Normals[1], Direction(0.707f, 0, -0.707f));
            Assert.AreEqual(parser.Normals[2], Direction(1, 2, 3));
        }
    }

}
