using System.Collections.Generic;
using GraphLabs.Dal.Ef;
using GraphLabs.DomainModel.Infrastructure;

namespace GraphLabs.DomainModel
{
    /// <summary> Лабораторная работа </summary>
    public partial class LabWork : AbstractEntity
    {
        public override IEnumerable<EntityValidationError> OnEntityValidating()
        {
            if (string.IsNullOrWhiteSpace(Name))
                yield return new EntityValidationError("Name", ValidationErrors.LabWork_OnValidating_Название_лабораторной_работы_не_может_быть_пустым_);
        }
    }
}
