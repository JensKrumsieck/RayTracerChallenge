using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

namespace RayTracer.Tests
{
    public static class AssertExtension
    {
        public static void FloatsAreEqual(this Assert a, float expected, float value, float threshold = Constants.Epsilon, string message = "")
        {
            Assert.IsTrue(MathF.Abs(value - expected) < threshold, 
                message + $"Float comparison failed for {value} and {expected} with Threshold {threshold}");
        }

        public static void VectorsAreEqual(this Assert a, Vector4 expected, Vector4 value, float threshold = Constants.Epsilon)
        {
            a.FloatsAreEqual(expected.X, value.X, threshold, $"Vectors {expected} & {value} are not Equal in X Component: \n");
            a.FloatsAreEqual(expected.Y, value.Y, threshold, $"Vectors {expected} & {value} are not Equal in Y Component: \n");
            a.FloatsAreEqual(expected.Z, value.Z, threshold, $"Vectors {expected} & {value} are not Equal in Z Component: \n");
            a.FloatsAreEqual(expected.W, value.W, threshold, $"Vectors {expected} & {value} are not Equal in X Component: \n");
        }

        public static void MatricesAreEqual(this Assert a, Matrix4x4 expected, Matrix4x4 value, float threshold = Constants.Epsilon)
        {
            a.FloatsAreEqual(expected.M11, value.M11, threshold, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M11 Component: \n");
            a.FloatsAreEqual(expected.M12, value.M12, threshold, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M12 Component: \n");
            a.FloatsAreEqual(expected.M13, value.M13, threshold, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M13 Component: \n");
            a.FloatsAreEqual(expected.M14, value.M14, threshold, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M14 Component: \n");

            a.FloatsAreEqual(expected.M21, value.M21, threshold, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M21 Component: \n");
            a.FloatsAreEqual(expected.M22, value.M22, threshold, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M22 Component: \n");
            a.FloatsAreEqual(expected.M23, value.M23, threshold, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M23 Component: \n");
            a.FloatsAreEqual(expected.M24, value.M24, threshold, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M14 Component: \n");

            a.FloatsAreEqual(expected.M31, value.M31, threshold, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M31 Component: \n");
            a.FloatsAreEqual(expected.M32, value.M32, threshold, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M32 Component: \n");
            a.FloatsAreEqual(expected.M33, value.M33, threshold, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M33 Component: \n");
            a.FloatsAreEqual(expected.M34, value.M34, threshold, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M34 Component: \n");

            a.FloatsAreEqual(expected.M41, value.M41, threshold, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M41 Component: \n");
            a.FloatsAreEqual(expected.M42, value.M42, threshold, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M42 Component: \n");
            a.FloatsAreEqual(expected.M43, value.M43, threshold, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M43 Component: \n");
            a.FloatsAreEqual(expected.M44, value.M44, threshold, $"Matrices \n{expected}\n&\n{value} \nare not Equal in M44 Component: \n");
        }
    }
}
