using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestRozetka.other
{
    [TestFixture]
    class NUnitAttributeTests
    {
        [TestCase(1, 1, 1)]
        [TestCase(5, 5, 25)]
        [TestCase(2, 10, 20)]
        [Category("attr")]
        public void Multiply1(int a, int b, int expected) {
            int actual = a * b;
            Assert.AreEqual(expected, actual);
        }

        // the same as Multiply but written differently
        [TestCase(1, 1, ExpectedResult = 1)]
        [TestCase(5, 5, ExpectedResult = 25)]
        [TestCase(2, 10, ExpectedResult = 20)]
        [Category("attr")]
        public int Multiply2(int a, int b)
        {
            int actual = a * b;
            return actual;
        }

        [TestCaseSource("trimInput")]
        [Category("attr")]
        public void Trim(string input, string expected) {
            string actual = input.Trim();
            Assert.AreEqual(expected, actual);
        }

        private static string[][] trimInput = {
            new string[] { "stop and ", "stop and" },
            new string[] { "   smell the roses    ", "smell the roses" }
        };

        [Test, MaxTime(500), Category("time-consuming")]
        public void MaxTime500() {
            Thread.Sleep(1000);
        }

        [Test, Timeout(500), Category("time-consuming")]
        public void Timeout500()
        {
            Thread.Sleep(1000);
        }

        [Test]
        [Category("attr")]
        public void TestEven([Values(6, 7, 8)] int n) {
            Assert.IsTrue(n % 2 == 0);
        }

        [Test]
        [Category("attr")]
        public void TestEven_2([ValueSource("evenValues")] int n)
        {
            Assert.IsTrue(n % 2 == 0);
        }

        private static List<int> evenValues = new List<int>() { 2, 4, 6, 7 };

        [Test]
        [Category("attr")]
        public void TestEven_3([Random(0, 10, 5)] int n) {
            Assert.IsTrue(n % 2 == 0);
        }

        [Test]
        [Platform("Windows7,Unix")]
        [Category("attr")]
        public void TestForWindows7() {
            Assert.IsTrue(true);
        }

        [Test]
        [Platform(Exclude = "Unix")]
        [Category("attr")]
        public void TestForOtherOS()
        {
            Assert.IsTrue(true);
        }

        [Test]
        [Pairwise]
        [Category("cars-pairwise")]
        // 35 test cases are generated
        public void TestPairwiseCars(
            [Values("Ford", "Mercedes", "BMW")] string manufacturer,
            [Values("Start", "Active", "Tourist", "Travel", "Business")] string equipment,
            [Range(2011, 2017)] int year)
        {
            Assert.IsTrue(true);
        }

        [Test]
        [Combinatorial]
        [Category("cars-combinatorial")]
        // 105 test cases are generated; exhaustive testing
        public void TestCombinatorialCars(
            [Values("Ford", "Mercedes", "BMW")] string manufacturer,
            [Values("Start", "Active", "Tourist", "Travel", "Business")] string equipment,
            [Range(2011, 2017)] int year)
        {
            Assert.IsTrue(true);
        }

        [Test]
        [Sequential]
        [Category("cars-sequential")]
        public void TestSequentialCars(
            [Values("Ford", "Mercedes", "BMW")] string manufacturer,
            [Values("Start", "Active", "Tourist", "Travel", "Business")] string equipment,
            [Range(2011, 2017)] int year)
        {
            Assert.IsTrue(true);
        }
    }
}
