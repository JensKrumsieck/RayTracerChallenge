using System.Collections;
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
        // ReSharper disable once UnusedParameter.Global
        #pragma warning disable IDE0060
        public static void ColorsAreEqual(this Assert a, Color expected, Color value)
        {
            Assert.IsTrue(expected == value, $"Colors did not match. Expected: {expected}, Got: {value}");
        }

        /// <summary>
        /// Checks whether all items are same
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="col"></param>
        /// <param name="item"></param>
        public static void AllItemsAre<T>(this CollectionAssert a, ICollection col, T item)
        {
            foreach (var i in col) Assert.AreEqual(i, item);
        }
        #pragma warning restore IDE0060
    }
}
