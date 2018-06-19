using System;
using System.Diagnostics.Contracts;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.TestPool;

namespace GraphLabs.Site.Models.TestPoolEntry
{
    /// <summary> Загрузчик моделей тестпулов </summary>
    internal sealed class TestPoolEntryModelLoader : AbstractModelLoader<TestPoolEntryModel, DomainModel.TestPoolEntry>
    {

        /// <summary> Загрузчик моделей тестпулов </summary>
        public TestPoolEntryModelLoader(
            IEntityQuery query
            ) : base(query)
        {
        }

        /// <summary> Загрузить по сущности-прототипу </summary>
        public override TestPoolEntryModel Load(DomainModel.TestPoolEntry testPoolEntry)
        {
            Contract.Requires<ArgumentNullException>(testPoolEntry != null);

            var model = new TestPoolEntryModel()
            {
                Id = testPoolEntry.Id,
                TestPool = testPoolEntry.TestPool,
                SubCategory = new SubCategoryModel()
                {
                    CategoryId = testPoolEntry.SubCategory.Category.Id,
                    CategoryName = testPoolEntry.SubCategory.Category.Name,
                    Id = testPoolEntry.SubCategory.Id,
                    Name = testPoolEntry.SubCategory.Name
                },
                QuestionsCount = testPoolEntry.QuestionsCount
            };

            return model;
        }
    }
}
