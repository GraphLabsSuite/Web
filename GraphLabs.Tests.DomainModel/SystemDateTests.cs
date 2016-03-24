using System;
using GraphLabs.Dal.Ef.Extensions;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel;
using GraphLabs.Dal.Ef;
using Moq;
using NUnit.Framework;

namespace GraphLabs.Tests.DomainModel
{
    [TestFixture]
    public class SystemDateTests : GraphLabsTestBase
    {
        [Test]
        public void TestGetTerm()
        {
            var systemDateMoq = new Mock<ISystemDateService>();
            
            systemDateMoq.Setup(o => o.GetDate()).Returns(new DateTime(2013, 01, 01));
            Assert.AreEqual(Term.Autumn, systemDateMoq.Object.GetTerm());

            systemDateMoq.Setup(o => o.GetDate()).Returns(new DateTime(2013, 02, 01));
            Assert.AreEqual(Term.Spring, systemDateMoq.Object.GetTerm());

            systemDateMoq.Setup(o => o.GetDate()).Returns(new DateTime(2013, 05, 01));
            Assert.AreEqual(Term.Spring, systemDateMoq.Object.GetTerm());

            systemDateMoq.Setup(o => o.GetDate()).Returns(new DateTime(2013, 08, 01));
            Assert.AreEqual(Term.Spring, systemDateMoq.Object.GetTerm());

            systemDateMoq.Setup(o => o.GetDate()).Returns(new DateTime(2013, 09, 01));
            Assert.AreEqual(Term.Autumn, systemDateMoq.Object.GetTerm());

            systemDateMoq.Setup(o => o.GetDate()).Returns(new DateTime(2013, 12, 01));
            Assert.AreEqual(Term.Autumn, systemDateMoq.Object.GetTerm());
        }

        [Test]
        public void TestGetDate()
        {
            var dateService = new SystemDateService(GraphLabsContext);
            Assert.DoesNotThrow(() => dateService.GetDate());
        }
    }
}
