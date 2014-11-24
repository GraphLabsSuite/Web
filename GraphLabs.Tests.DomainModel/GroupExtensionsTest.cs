using System;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.DomainModel.Services;
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
            
            var group01 = new Group()
                {
                    FirstYear = CURRENT_YEAR - 1,
                    Number = 100
                };
            Assert.AreEqual("К01-100", group01.GetName(systemDateMoq.Object));

            var group05 = new Group()
            {
                FirstYear = CURRENT_YEAR-3,
                Number = 101
            };
            Assert.AreEqual("К05-101", group05.GetName(systemDateMoq.Object));

            systemDateMoq.Setup(o => o.GetDate()).Returns(new DateTime(CURRENT_YEAR, 02, 01));
            var group02 = new Group()
            {
                FirstYear = CURRENT_YEAR - 1,
                Number = 102
            };
            Assert.AreEqual("К02-102", group02.GetName(systemDateMoq.Object));

            var group08 = new Group()
            {
                FirstYear = CURRENT_YEAR - 4,
                Number = 103
            };
            Assert.AreEqual("К08-103", group08.GetName(systemDateMoq.Object));

            systemDateMoq.Setup(o => o.GetDate()).Returns(new DateTime(CURRENT_YEAR, 11, 01));
            var group03 = new Group()
            {
                FirstYear = CURRENT_YEAR - 1,
                Number = 104
            };
            Assert.AreEqual("К03-104", group03.GetName(systemDateMoq.Object));

            group01 = new Group()
            {
                FirstYear = CURRENT_YEAR,
                Number = 105
            };
            Assert.AreEqual("К01-105", group01.GetName(systemDateMoq.Object));
        }
    }
}
