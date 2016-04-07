using System;
using System.Diagnostics.Contracts;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.LabVariants
{
    /// <summary> Загрузчик моделей вариантов лабораторных работ </summary>
    sealed class LabVariantModelLoader : AbstractModelLoader<LabVariantModel, LabVariant>
    {
        /// <summary> Загрузчик моделей вариантов лабораторных работ </summary>
        public LabVariantModelLoader(IEntityQuery query) : base(query) { }

        /// <summary> Загрузить по сущности-прототипу </summary>
        public override LabVariantModel Load(LabVariant labVariant)
        {
            Contract.Requires(labVariant != null);

            var model = new LabVariantModel
            {
                Id = labVariant.Id,
                IntroducingVariant = labVariant.IntroducingVariant,
                LabWorkId = labVariant.LabWork.Id,
                Number = labVariant.Number,
                Results = labVariant.Results,
                TaskVariants = labVariant.TaskVariants,
                TestQuestions = labVariant.TestQuestions,
                Version = labVariant.Version
            };

            return model;
        }
    }
}