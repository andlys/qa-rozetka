using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.other
{
    [TestFixture]
    class NUnitAttributes
    {
        [TestCase(1, 1, 1)]
        [TestCase(5, 5, 25)]
        [TestCase(2, 10, 20)]
        [Category("attr")]
        public void Multiply1(int a, int b, int expected) {
            int actual = a*b;
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
    }
}
