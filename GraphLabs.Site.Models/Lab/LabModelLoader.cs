using System.Diagnostics.Contracts;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Lab
{
    /// <summary> Загрузчик моделей лабораторных работ </summary>
    sealed class LabModelLoader : AbstractModelLoader<LabModel, LabWork>
    {
        /// <summary> Загрузчик моделей лабораторных работ </summary>
        public LabModelLoader(IEntityQuery query) : base(query) { }

        /// <summary> Загрузить по сущности-прототипу </summary>
        public override LabModel Load(LabWork labWork)
        {
            Guard.IsNotNull(nameof(labWork), labWork);

            var model = new LabModel
            {
                Id = labWork.Id,
                Name = labWork.Name
            };

            return model;
        }
    }
}
