﻿using System;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.DomainModel.Services;
using Moq;
using NUnit.Framework;

namespace GraphLabs.Tests.DomainModel
{
    [TestFixture]
    public class SystemDateTests : TestBase
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