using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

namespace RayTracer.Tests
{
    public static class AssertExtension
    {
        public static void FloatsAreEqual(this Assert a, float expected, float value, string message = "")
        {
            Assert.IsTrue(MathF.Abs(value - expected) < Constants.Epsilon, message + $"Float comparison failed for {value} and {expected}");
        }

        public static void VectorsAreEqual(this Assert a, Vector4 expected, Vector4 value)
        {
            a.FloatsAreEqual(expected.X, value.X, $"Vectors {expected} & {value} are not Equal in X Component: \n");
            a.FloatsAreEqual(expected.Y, value.Y, $"Vectors {expected} & {value} are not Equal in Y Component: \n");
            a.FloatsAreEqual(expected.Z, value.Z, $"Vectors {expected} & {value} are not Equal in Z Component: \n");
            a.FloatsAreEqual(expected.W, value.W, $"Vectors {expected} & {value} are not Equal in X Component: \n");
        }

        public static void MatricesAreEqual(this Assert a, Matrix4x4 expected, Matrix4x4 value)
        {
            a.FloatsAreEqual(expected.M11, value.M11, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M11 Component: \n");
            a.FloatsAreEqual(expected.M12, value.M12, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M12 Component: \n");
            a.FloatsAreEqual(expected.M13, value.M13, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M13 Component: \n");
            a.FloatsAreEqual(expected.M14, value.M14, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M14 Component: \n");

            a.FloatsAreEqual(expected.M21, value.M21, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M21 Component: \n");
            a.FloatsAreEqual(expected.M22, value.M22, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M22 Component: \n");
            a.FloatsAreEqual(expected.M23, value.M23, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M23 Component: \n");
            a.FloatsAreEqual(expected.M24, value.M24, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M14 Component: \n");

            a.FloatsAreEqual(expected.M31, value.M31, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M31 Component: \n");
            a.FloatsAreEqual(expected.M32, value.M32, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M32 Component: \n");
            a.FloatsAreEqual(expected.M33, value.M33, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M33 Component: \n");
            a.FloatsAreEqual(expected.M34, value.M34, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M34 Component: \n");

            a.FloatsAreEqual(expected.M41, value.M41, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M41 Component: \n");
            a.FloatsAreEqual(expected.M42, value.M42, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M42 Component: \n");
            a.FloatsAreEqual(expected.M43, value.M43, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M43 Component: \n");
            a.FloatsAreEqual(expected.M44, value.M44, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M44 Component: \n");
        }
    }
}
