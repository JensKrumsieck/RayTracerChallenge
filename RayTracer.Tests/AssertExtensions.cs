using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;

namespace RayTracer.Tests
{
    public static class AssertExtensions
    {
        /// <summary>
        /// Color Assertions
        /// </summary>
        /// <param name="a"></param>
        /// <param name="expected"></param>
        /// <param name="value"></param>
        public static void ColorsAreEqual(this Assert a, Color expected, Color value)
        {
            Assert.IsTrue(expected == value, $"Colors did not match. Expected: {expected}, Got: {value}");
        }
    }
}
