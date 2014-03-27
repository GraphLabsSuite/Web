using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GraphLabs.Site.Logic.Security;
using NUnit.Framework;

namespace GraphLabs.Tests.Site.Logic
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void Test()
        {
            var hashCalculator = new BCryptCalculator();
            var hash = hashCalculator.Crypt("123");

            Debug.WriteLine(hash);
        }
    }
}
