using System.IO;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Logic.Tasks;
using GraphLabs.Site.Utils.XapProcessor;
using Moq;
using NUnit.Framework;

namespace GraphLabs.Tests.Site.Logic
{
    [TestFixture]
    public class TaskManagerTests
    {
        /// <remarks>
        /// Два раза, потому что когда-то падало при повторной загрузке сборки. 
        /// Теперь для чтения атрибутов сборок используется Mono.Cecil, но всё равно.
        /// </remarks>
        [Test]
        [Repeat(2)]
        public void TestCreateFromXap()
        {
            var contextManagerMock = Mock.Of<IDbContextManager>();
            var taskRepositoryMock = Mock.Of<ITaskRepository>();

            var taskManager = new TaskManager(contextManagerMock, taskRepositoryMock, new XapProcessor());
            Task task;
            using (var ms = new MemoryStream(Resources.GraphLabs_Tasks_SCC))
            {
                task = taskManager.UploadTask(ms);
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
