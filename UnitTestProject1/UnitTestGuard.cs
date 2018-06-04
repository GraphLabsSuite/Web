using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GraphLabs;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTestGuard
    {
        private class A { }
        private class B : A { };
        private class C { };
        [TestMethod]
        public void TestMethodIsNotNull()
        {
            List<int> a = new List<int>() { 1, 2, 3 };
            Guard.IsNotNull(a);

            Exception exception = null;
            List<int> b = null;
            try
            {
                Guard.IsNotNull(b);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public void TestAreAssignedTypes()
        {
            Guard.AreAssignedTypes(typeof(A), typeof(B));
            Exception exception = null;
            try
            {
                Guard.AreAssignedTypes(typeof(B), typeof(A));
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);

            exception = null;
            try
            {
                Guard.AreAssignedTypes(typeof(C), typeof(A));
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);

        }

        [TestMethod]
        public void TestAreEqual()
        {
            String str1 = "a";
            String str2 = "a";
            String str3 = "b";

            Guard.AreEqual(str1, str2, nameof(str1), nameof(str2));
            Exception exception = null;
            try
            {
                Guard.AreEqual(str1, str3, nameof(str1), nameof(str3));
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public void TestIsNotNullOrWhiteSpace()
        {
            String str1 = "a";
            String str2 = "";
            String str3 = "   ";
            String str4 = null;

            Guard.IsNotNullOrWhiteSpace(str1);
            Exception exception = null;
            try
            {
                Guard.IsNotNullOrWhiteSpace(str2);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);

            exception = null;
            try
            {
                Guard.IsNotNullOrWhiteSpace(str3);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);

            exception = null;
            try
            {
                Guard.IsNotNullOrWhiteSpace(str4);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);

        }

        [TestMethod]
        public void TestIsIsPositive()
        {
            int intp = 5;
            int intn = -1;
            long longp = 10;
            long longn = -3;

            Guard.IsPositive(intp, nameof(intp));
            Guard.IsPositive(longp, nameof(longp));

            Exception exception = null;
            try
            {
                Guard.IsPositive(intn, nameof(intn));
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);

            exception = null;
            try
            {
                Guard.IsPositive(longn, nameof(longn));
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);

        }


        [TestMethod]
        public void TestIsNotEmpty()
        {
            String str1 = "a";
            String str2 = null;
            String str3 = "";

            Guard.IsNotEmpty(str1, nameof(str1));
            Guard.IsNotEmpty(str1, nameof(str1));

            Exception exception = null;
            try
            {
                Guard.IsNotEmpty(str3, nameof(str3));
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);

        }

        [TestMethod]
        public void TestIsTrueAssertion()
        {
            bool a = true;
            bool b = false;

            Guard.IsTrueAssertion(a);
            Exception exception = null;
            try
            {
                Guard.IsTrueAssertion(b);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
        }
    }
}
