using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace GraphLabs.DomainModel
{
    /// <summary> Лабораторная работа </summary>
    public partial class LabWork : AbstractEntity
    {
        public override IEnumerable<DbValidationError> OnEntityValidating(DbEntityEntry entityEntry)
        {
            if (string.IsNullOrWhiteSpace(Name))
                yield return new DbValidationError("Name", ValidationErrors.LabWork_OnValidating_Название_лабораторной_работы_не_может_быть_пустым_);

            if (AcquaintanceFrom >= AcquaintanceTill)
                yield return new DbValidationError("AcquaintanceTill", ValidationErrors.LabWork_OnValidating_Указан_некорректный_период_ознакомления_);
        }
    }
}
