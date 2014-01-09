using System.IO;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;
using NUnit.Framework;

namespace GraphLabs.Tests.DomainModel
{
    [TestFixture]
    public class TaskExtensionsTests : TestBase
    {
        [Test]
        [Repeat(2)]
        public void TestCreateFromXap()
        {
            Task task = null;
            using (var ms = new MemoryStream(Resources.GraphLabs_Tasks_SCC))
            {
                task = GraphLabsContext.Tasks.CreateFromXap(ms);
            }

            Assert.AreEqual("Поиск КСС и построение конденсата", task.Name);
            Assert.AreEqual("Какое-то описание", task.Sections);
            Assert.IsNull(task.VariantGenerator);
            Assert.IsNull(task.Note);
            Assert.AreEqual("1.0.0.39321", task.Version);
            Assert.AreEqual(Resources.GraphLabs_Tasks_SCC.LongLength, task.Xap.LongLength);
        }
    }
}
