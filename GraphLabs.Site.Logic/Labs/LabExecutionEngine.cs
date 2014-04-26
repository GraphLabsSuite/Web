using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.DomainModel.Services;
using System.Diagnostics.Contracts;
using System.Linq;
using System;
using System.Collections.Generic;
using GraphLabs.Utils;

namespace GraphLabs.Site.Logic.Labs
{
    public sealed class LabExecutionEngine : ILabExecutionEngine
    {
        private readonly ILabRepository _labRepository;

        public LabExecutionEngine(
            ILabRepository labRepository)
        {
            _labRepository = labRepository;
        }

        /// <summary> Получить имя лабораторной работы </summary>
        public string GetLabName(long id)
        {
            var lab = _labRepository.FindLabWorkById(id);
            if (lab != null)
            {
                return _labRepository.FindLabWorkById(id).Name;
            }
            else
            {
                throw new Exception("Лабораторная работа не найдена");
            }
        }

        /// <summary> Проверяет, соответствует ли вариант содержанию лабораторной работы </summary>
        public bool IsLabVariantCorrect(long labVarId)
        {
            var labVar = _labRepository.FindLabVariantById(labVarId);
            if (labVar == null)
            {
                return false;
            }
            List<long> tasksId = new List<long>();
            foreach (var e in _labRepository.FindEntryTasksByLabVarId(labVarId))
            {
                tasksId.Add(e.Id);
            }
            List<long> tasksIdAlt = new List<long>();
            foreach (var t in _labRepository.FindTasksByLabVarId(labVarId))
            {
                tasksIdAlt.Add(t.Id);
            }

            return tasksId.ContainsSameSet(tasksIdAlt);
        }
    }
}
