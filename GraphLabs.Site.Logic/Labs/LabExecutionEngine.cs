using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.DomainModel.Services;
using System.Diagnostics.Contracts;
using System.Linq;

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

        /// <summary> Получить лабораторные работы, у которых сейчас ознакомительный период </summary>
        public string GetLabName(long id)
        {
            var lab = _labRepository.GetLabWorkById(id);
            if (lab != null)
            {
                return _labRepository.GetLabWorkById(id).Name;
            }
            else
            {
                return "";
            }
        }
    }
}
