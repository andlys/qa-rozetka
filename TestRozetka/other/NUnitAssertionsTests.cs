using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRozetka.other
{
    [TestFixture]
    class NUnitAssertionsTests
    {
        [Test, Category("assertions")]
        public void SimpleAssertionHasTest([ValueSource("list")] List<int> numbers) {
            Assert.That(numbers, Has.Exactly(2).EqualTo(4));
        }

        private static List<List<int>> list = new List<List<int>>() {
            new List<int>() { 4, 13, 4, 5, 16 }
        };

        [Test, Category("assertions")]
        public void SimpleAssertionHasTest_2([ValueSource("list")] List<int> numbers)
        {
            Assert.That(numbers, Has.Exactly(3).LessThanOrEqualTo(10));
        }

        [Test, Category("assertions")]
        public void SimpleAssertionIsTest([Random(0,1,1)] int n) {
            Assert.That(n, Is.EqualTo(0));
        }

    }
}
