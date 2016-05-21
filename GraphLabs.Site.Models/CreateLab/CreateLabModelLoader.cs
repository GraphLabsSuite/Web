using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.CreateLab
{
    /// <summary> Загрузчик моделей создания лабораторных работ </summary>
    sealed class CreateLabModelLoader : AbstractModelLoader<CreateLabModel, LabWork>
    {
        /// <summary> Загрузчик моделей создания лабораторных работ </summary>
        public CreateLabModelLoader(IEntityQuery query) : base(query) { }

        /// <summary> Загрузить по сущности-прототипу </summary>
        public override CreateLabModel Load(LabWork labWork)
        {
            Contract.Requires(labWork != null);

            var model = new CreateLabModel
            {
                Id = labWork.Id,
                Name = labWork.Name,
                AcquaintanceFrom = (DateTime)labWork.AcquaintanceFrom,
                AcquaintanceTill = (DateTime)labWork.AcquaintanceTill,
                Tasks = MakeListFromTasks(labWork.LabEntries.Select(e => e.Task).ToArray())
            };

            return model;
        }

        private List<KeyValuePair<long, string>> MakeListFromTasks(Task[] tasks)
        {
            var result = new List<KeyValuePair<long, string>>();
            foreach (var t in tasks)
            {
                result.Add(new KeyValuePair<long, string>(t.Id, t.Name));
            }
            return result;
        }
    }
}
