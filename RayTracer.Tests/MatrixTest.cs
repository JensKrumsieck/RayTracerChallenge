using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;

namespace RayTracer.Tests
{
    [TestClass]
    public class MatrixTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var m = new Matrix(new[,]
            {
                {1f, 2f, 3f, 4f},
                {5.5f, 6.5f, 7.5f, 8.5f},
                {9f, 10f, 11f, 12f},
                {13.5f, 14.5f, 15.5f, 16.5f}
            });

            Assert.AreEqual(1, m[0, 0]);
            Assert.AreEqual(4, m[0, 3]);
            Assert.AreEqual(5.5, m[1, 0]);
            Assert.AreEqual(7.5, m[1, 2]);
            Assert.AreEqual(11, m[2, 2]);
            Assert.AreEqual(13.5, m[3, 0]);
            Assert.AreEqual(15.5, m[3, 2]);
        }

        [TestMethod]
        public void MatrixEqual()
        {
            var m1 = new Matrix(new[,]
            {
                {1f, 2f, 3f, 4f},
                {5f, 6f, 7f, 8f},
                {9f, 8f, 7f, 6f},
                {5f, 4f, 3f, 2f}
            });
            var m2 = new Matrix(new[,]
            {
                {1f, 2f, 3f, 4f},
                {5f, 6f, 7f, 8f},
                {9f, 8f, 7f, 6f},
                {5f, 4f, 3f, 2f}
            });
            Assert.AreEqual(m1, m2);
        }

        [TestMethod]
        public void MatrixNotEqual()
        {
            var m1 = new Matrix(new[,]
            {
                {1f, 2f, 3f, 4f},
                {5f, 6f, 7f, 8f},
                {9f, 8f, 7f, 6f},
                {5f, 4f, 3f, 2f}
            });
            var m2 = new Matrix(new[,]
            {
                {2f, 3f, 3f, 4f},
                {5f, 6f, 7f, 8f},
                {9f, 8f, 7f, 6f},
                {5f, 4f, 3f, 2f}
            });
            Assert.AreNotEqual(m1, m2);
        }

        [TestMethod]
        public void MatrixMultiply()
        {
            var m1 = new Matrix(new[,]
            {
                {1f, 2f, 3f, 4f},
                {5f, 6f, 7f, 8f},
                {9f, 8f, 7f, 6f},
                {5f, 4f, 3f, 2f}
            });
            var m2 = new Matrix(new[,]
            {
                {-2f, 1f, 2f, 3f},
                {3f, 2f, 1f, -1f},
                {4f, 3f, 6f, 5f},
                {1f, 2f, 7f, 8f}
            });
            var m3 = new Matrix(new[,]
            {
                {20f, 22f, 50f, 48f},
                {44f, 54f, 114f, 108f},
                {40f, 58f, 110f, 102f},
                {16f, 26f, 46f, 42f}
            });
            Assert.IsTrue(m1 * m2 == m3);
        }

        [TestMethod]
        public void MultiplyVector()
        {
            var m = new Matrix(new[,]
            {
                {1f, 2f, 3f, 4f},
                {2f, 4f, 4f, 2f},
                {8f, 6f, 4f, 1f},
                {0f, 0f, 0f, 1f}
            });
            var v = new Vector3(1f, 2f, 3f, 1f);
            Assert.IsTrue((m * v) == new Vector3(18, 24, 33, 1));
        }

        [TestMethod]
        public void MultiplyIdentity()
        {
            var m = new Matrix(new[,]
            {
                {0f, 1f, 2f, 4f},
                {1f, 2f, 4f, 8f},
                {2f, 4f, 8f, 16f},
                {4f, 8f, 16f, 32f}
            });
            Assert.IsTrue(m * Matrix.Identity(4, 4) == m);
        }

        [TestMethod]
        public void MatrixTranspose()
        {
            var m1 = new Matrix(new[,]
            {
                {0f, 9f, 3f, 0f},
                {9f, 8f, 0f, 8f},
                {1f, 8f, 5f, 3f},
                {0f, 0f, 5f, 8f}
            });
            var m2 = new Matrix(new[,]
            {
                {0f, 9f, 1f, 0f},
                {9f, 8f, 8f, 0f},
                {3f, 0f, 5f, 5f},
                {0f, 8f, 3f, 8f}
            });
            Assert.AreEqual(m1.Transpose(), m2);
        }

        [TestMethod]
        public void TransposeIdentity()
        {
            var i = Matrix.Identity(4, 4);
            Assert.AreEqual(i, i.Transpose());
        }

        [TestMethod]
        public void TestSubmatrix()
        {
            var m1 = new Matrix(new[,]
            {
                {1f, 5f, 0f},
                {-3f, 2f, 7f},
                {0f, 6f, -3f}
            });
            var subM1 = new Matrix(new[,]
            {
                {-3f, 2f},
                {0f, 6f}
            });
            Assert.AreEqual(m1.Submatrix(0, 2), subM1);
        }

        [TestMethod]
        public void TestSubmatrix_2()
        {
            var m1 = new Matrix(new[,]
            {
                {-6f, 1f, 1f, 6f},
                {-8f, 5f, 8f, 6},
                {-1f, 0f, 8f, 2f},
                {-7f, 1f, -1f, 1f}
            });
            var subM1 = new Matrix(new[,]
            {
                {-6f, 1f, 6f},
                {-8f, 8f, 6f},
                {-7f, -1f, 1f}
            });
            Assert.AreEqual(m1.Submatrix(2, 1), subM1);
        }

        [TestMethod]
        public void TestDeterminant()
        {
            var m1 = new Matrix(new[,]
            {
                {1f, 5f},
                {-3f, 2f}
            });
            Assert.AreEqual(m1.Determinant(), 17);
        }

        [TestMethod]
        public void TestMinor()
        {
            var m1 = new Matrix(new[,]
            {
                {3f, 5f, 0f},
                {2f, -1f, -7f},
                {6f, -1f, 5f}
            });
            Assert.AreEqual(m1.Minor(1, 0), 25);
        }

        [TestMethod]
        public void TestCofactor()
        {
            var m1 = new Matrix(new[,]
            {
                {3f, 5f, 0f},
                {2f, -1f, -7f},
                {6f, -1f, 5f}
            });
            Assert.AreEqual(m1.Minor(0, 0), -12);
            Assert.AreEqual(m1.Cofactor(0, 0), -12);
            Assert.AreEqual(m1.Minor(1, 0), 25);
            Assert.AreEqual(m1.Cofactor(1, 0), -25);
        }

        [TestMethod]
        public void Determinant3x3()
        {
            var m1 = new Matrix(new[,]
            {
                {1f, 2f, 6f},
                {-5f, 8f, -4f},
                {2f, 6f, 4f}
            });
            Assert.AreEqual(m1.Determinant(), -196);
        }

        [TestMethod]
        public void Determinant4x4()
        {
            var m1 = new Matrix(new[,]
            {
                {-2f, -8f, 3f, 5f},
                {-3f, 1f, 7f, 3f},
                {1f, 2f, -9f, 6f},
                {-6f, 7f, 7f, -9f}
            });
            Assert.IsTrue(m1.Invertible);
            Assert.AreEqual(m1.Determinant(), -4071);
        }

        [TestMethod]
        public void MatrixInverse()
        {
            var m1 = new Matrix(new[,]
            {
                {-5f, 2f, 6f, -8f},
                {1f, -5f, 1f, 8f},
                {7f, 7f, -6f, -7f},
                {1f, -3f, 7f, 4f}
            });
            var m2 = m1.Inverse();
            Assert.AreEqual(m1.Determinant(), 532);
            Assert.AreEqual(m1.Cofactor(2, 3), -160);
            Assert.AreEqual(m2[3, 2], -160f / 532f);
            Assert.AreEqual(m1.Cofactor(3, 2), 105);
            Assert.AreEqual(m2[2, 3], 105f / 532f);
            var m3 = new Matrix(new[,]
            {
                {0.21805f, 0.45113f, 0.24060f, -0.04511f},
                {-0.80827f, -1.45677f, -0.44361f, 0.52068f},
                {-0.07895f, -0.22368f, -0.05263f, 0.19737f},
                {-0.52256f, -0.81391f, -0.30075f, 0.30639f}
            });
            Assert.IsTrue(m2.Equals(m3, 1e5f));
        }
    }
}
