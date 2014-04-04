using GraphLabs.Utils;
using NUnit.Framework;
using System.Collections.Generic;

namespace GraphLabs.Tests.Utils
{
    /// <summary> Тесты EnumerableExtensions </summary>
    [TestFixture]
    class EnumerableExtensionsTests
    {
        [Test]
        [TestCase(new object[] { 1 }, new object[] { 2 }, false)]
        [TestCase(new object[] { 1 }, new object[] { 1 }, true)]
        [TestCase(new object[] { 1 }, new object[] { 1, 2 }, false)]
        [TestCase(new object[] { 1, 2 }, new object[] { 2, 1 }, true)]
        [TestCase(new object[] { 1, 1 }, new object[] { 1 }, false)]
        [TestCase(new object[] { 1, 1, 2, 2, 2 }, new object[] { 1, 1, 1, 2, 2 }, false)]
        [TestCase(new object[] { 1, 2, 5, 3 }, new object[] { 1, 3, 2, 5 }, true)]
        public void ComparingTest(IEnumerable<object> list1, IEnumerable<object> list2, bool result)
        {
            Assert.That(list1.ContainsSameSet(list2) == result && list2.ContainsSameSet(list1) == result);
        }

    }
}
