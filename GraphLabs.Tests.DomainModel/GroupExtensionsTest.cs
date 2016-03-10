using System;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.EF;
using GraphLabs.DomainModel.EF.Extensions;
using GraphLabs.DomainModel.EF.Services;
using NUnit.Framework;
using Moq;

namespace GraphLabs.Tests.DomainModel
{
    [TestFixture]
    public class GroupExtensionsTest : GraphLabsTestBase
    {
        [Test]
        public void TestGetName()
        {
            const int CURRENT_YEAR = 2013;

            var systemDateMoq = new Mock<ISystemDateService>();
            systemDateMoq.Setup(o => o.GetDate()).Returns(new DateTime(CURRENT_YEAR, 01, 01));

            var group01 = GraphLabsContext.Groups.Create();
            group01.FirstYear = CURRENT_YEAR - 1;
            group01.Number = 100;
            Assert.AreEqual("К01-100", group01.GetName(systemDateMoq.Object));

            var group05 = GraphLabsContext.Groups.Create();
            group05.FirstYear = CURRENT_YEAR - 3;
            group05.Number = 101;
            Assert.AreEqual("К05-101", group05.GetName(systemDateMoq.Object));

            systemDateMoq.Setup(o => o.GetDate()).Returns(new DateTime(CURRENT_YEAR, 02, 01));
            var group02 = GraphLabsContext.Groups.Create();
            group02.FirstYear = CURRENT_YEAR - 1;
            group02.Number = 102;
            Assert.AreEqual("К02-102", group02.GetName(systemDateMoq.Object));

            var group08 = GraphLabsContext.Groups.Create();
            group08.FirstYear = CURRENT_YEAR - 4;
            group08.Number = 103;
            Assert.AreEqual("К08-103", group08.GetName(systemDateMoq.Object));

            systemDateMoq.Setup(o => o.GetDate()).Returns(new DateTime(CURRENT_YEAR, 11, 01));
            var group03 = GraphLabsContext.Groups.Create();
            group03.FirstYear = CURRENT_YEAR - 1;
            group03.Number = 104;
            Assert.AreEqual("К03-104", group03.GetName(systemDateMoq.Object));

            group01 = GraphLabsContext.Groups.Create();
            group01.FirstYear = CURRENT_YEAR;
            group01.Number = 105;
            Assert.AreEqual("К01-105", group01.GetName(systemDateMoq.Object));
        }
    }
}
