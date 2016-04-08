using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Utils;

namespace GraphLabs.Site.Models.DemoLab
{
    /// <summary> Загрузчик моделей демонстрационных лабораторных работ </summary>
    sealed class DemoLabModelLoader : AbstractModelLoader<DemoLabModel, LabWork>
    {
        /// <summary> Загрузчик моделей демонстрационных лабораторных работ </summary>
        public DemoLabModelLoader(IEntityQuery query) : base(query) { }

        /// <summary> Загрузить по сущности-прототипу </summary>
        public override DemoLabModel Load(LabWork labWork)
        {
            Contract.Requires(labWork != null);

            var model = new DemoLabModel
            {
                Id = labWork.Id,
                Name = labWork.Name,
                AcquaintanceTill = (DateTime)labWork.AcquaintanceTill,
                Variants = labWork.LabVariants
                    .Where(lv => lv.IntroducingVariant)
                    .ToArray()
                    .Where(lv => VerifyCompleteVariant(lv.Id))
                    .ToArray()
                    .Select(lv => new KeyValuePair<long, string>(lv.Id, lv.Number))
                    .ToArray()
        };

            return model;
        }

        /// <summary> Проверка соответствия варианта лабораторной работы содержанию работы </summary>
        private bool VerifyCompleteVariant(long variantId)
        {
            long labWorkId = _query.OfEntities<LabVariant>()
                .Where(v => v.Id == variantId)
                .Select(v => v.LabWork.Id)
                .Single();

            long[] labEntry = _query.OfEntities<LabEntry>()
                .Where(e => e.LabWork.Id == labWorkId)
                .Select(e => e.Task.Id)
                .ToArray();

            long[] currentVariantEntry = _query.OfEntities<LabVariant>()
                .Where(l => l.Id == variantId)
                .SelectMany(t => t.TaskVariants)
                .Select(t => t.Task.Id)
                .ToArray();

            return labEntry.ContainsSameSet(currentVariantEntry);
        }
    }
}
