﻿using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Utils;

namespace GraphLabs.Site.Models.AvailableLab
{
    /// <summary> Загрузчик моделей демонстрационных лабораторных работ </summary>
    sealed class DemoLabModelLoader : AbstractModelLoader<DemoLabModel, AbstractLabSchedule>
    {
        /// <summary> Загрузчик моделей демонстрационных лабораторных работ </summary>
        public DemoLabModelLoader(IEntityQuery query) : base(query) { }

        /// <summary> Загрузить по сущности-прототипу </summary>
        public override DemoLabModel Load(AbstractLabSchedule sch)
        {
            Contract.Requires(sch != null);
            Contract.Requires(sch.Mode == LabExecutionMode.IntroductoryMode);

            var model = new DemoLabModel
            {
                Id = sch.Id,
                Name = sch.LabWork.Name,
                AcquaintanceTill = sch.DateTill,
                Variants = sch.LabWork.LabVariants
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
            long labWorkId = _query.OfEntities<DomainModel.LabVariant>()
                .Where(v => v.Id == variantId)
                .Select(v => v.LabWork.Id)
                .Single();

            long[] labEntry = _query.OfEntities<LabEntry>()
                .Where(e => e.LabWork.Id == labWorkId)
                .Select(e => e.Task.Id)
                .ToArray();

            long[] currentVariantEntry = _query.OfEntities<DomainModel.LabVariant>()
                .Where(l => l.Id == variantId)
                .SelectMany(t => t.TaskVariants)
                .Select(t => t.Task.Id)
                .ToArray();

            return labEntry.ContainsSameSet(currentVariantEntry);
        }
    }
}
