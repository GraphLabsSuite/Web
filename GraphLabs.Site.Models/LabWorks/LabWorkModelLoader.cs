﻿using System;
using System.Diagnostics.Contracts;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.LabWorks
{
    /// <summary> Загрузчик моделей лабораторных работ </summary>
    sealed class LabWorkModelLoader : AbstractModelLoader<LabWorkModel, LabWork>
    {
        /// <summary> Загрузчик моделей лабораторных работ </summary>
        public LabWorkModelLoader(IEntityQuery query) : base(query) { }

        /// <summary> Загрузить по сущности-прототипу </summary>
        public override LabWorkModel Load(LabWork labWork)
        {
            Contract.Requires(labWork != null);

            var model = new LabWorkModel
            {
                Id = labWork.Id,
                Name = labWork.Name,
                AcquaintanceFrom = labWork.AcquaintanceFrom,
                AcquaintanceTill = labWork.AcquaintanceTill,
                LabVariants = labWork.LabVariants
            };

            return model;
        }
    }
}