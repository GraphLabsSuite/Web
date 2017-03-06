using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Groups;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.TestPool
{
    /// <summary> Загрузчик моделей тестпулов </summary>
    class TestPoolModelLoader : AbstractModelLoader<TestPoolModel, DomainModel.TestPool>
    {
        /// <summary> Загрузчик моделей тестпулов </summary>
        public TestPoolModelLoader(IEntityQuery query) : base(query)
        {
        }

        /// <summary> Загрузить по сущности-прототипу </summary>
        public override TestPoolModel Load(DomainModel.TestPool testPool)
        {
            Contract.Requires(testPool != null);

            var model = new TestPoolModel()
            {
                Id = testPool.Id,
                Name = testPool.Name,
                LabVariants = testPool.LabVariants
            };

            return model;
        }
    }
}
