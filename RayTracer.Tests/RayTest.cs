﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;

namespace RayTracer.Tests
{
    [TestClass]
    public class RayTest
    {
        [TestMethod]
        public void CreateRay()
        {
            var ray = new Ray(Vector.Point(1f, 2f, 3f), new Vector(4f, 5f, 6f));
            Assert.AreEqual(Vector.Point(1f, 2f, 3f), ray.Origin);
            Assert.AreEqual(new Vector(4f, 5f, 6f), ray.Direction);
        }

        [TestMethod]
        public void PointByDistance()
        {
            var ray = new Ray(Vector.Point(2f, 3f, 4f), new Vector(1f, 0f, 0f));
            Assert.AreEqual(Vector.Point(2f, 3f, 4f), ray.PointByDistance(0f));
            Assert.AreEqual(Vector.Point(3f, 3f, 4f), ray.PointByDistance(1f));
            Assert.AreEqual(Vector.Point(1f, 3f, 4f), ray.PointByDistance(-1f));
            Assert.AreEqual(Vector.Point(4.5f, 3f, 4f), ray.PointByDistance(2.5f));
        }
    }
}