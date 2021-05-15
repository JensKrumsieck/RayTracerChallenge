using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;
using RayTracer.Extension;
using System;
using System.Collections;
using System.Numerics;

namespace RayTracer.Tests
{
    public static class AssertExtensions
    {
        public static void ColorsAreEqual(this Assert a, Color expected, Color value)
        {
            Assert.IsTrue(expected == value, $"Colors did not match. Expected: {expected}, Got: {value}");
        }

        public static void ColorsAreEqual(this Assert a, Color expected, Color value, float threshold)
        {
            Assert.IsTrue(expected.Equals(value, threshold), $"Colors did not match. Expected: {expected}, Got: {value}, threshold {threshold}");
        }
        public static void VectorsAreEqual(this Assert a, Vector3 expected, Vector3 value, float threshold)
        {
            Assert.IsTrue(MathF.Abs(expected.X - value.X) < threshold, $"{expected} did not match {value} for X");
            Assert.IsTrue(MathF.Abs(expected.Y - value.Y) < threshold, $"{expected} did not match {value} for Y");
            Assert.IsTrue(MathF.Abs(expected.Z - value.Z) < threshold, $"{expected} did not match {value} for Z");
        }

        public static void VectorsAreEqual(this Assert a, Vector3 expected, Vector3 value) =>
            a.VectorsAreEqual(expected, value, Constants.Epsilon);

        public static void MatricesAreEqual(this Assert a, Matrix4x4 expected, Matrix4x4 value, float threshold)
        {
            var arrE = expected.ToArray();
            var arrV = value.ToArray();
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    Assert.IsTrue(arrE[i, j].Equal(arrV[i, j], threshold),
                        $"Matrix evaluation failed at {j},{i}. \nExpected: {expected} \nGiven: {value}\nLast Check: {arrE[i, j]} Equals{arrV[i, j]}");
                }
            }
        }

        public static void MatricesAreEqual(this Assert a, Matrix4x4 expected, Matrix4x4 value) =>
            a.MatricesAreEqual(expected, value, Constants.Epsilon);

        public static void AllItemsAre<T>(this CollectionAssert a, ICollection col, T item)
        {
            foreach (var i in col) Assert.AreEqual(i, item);
        }
    }
}
