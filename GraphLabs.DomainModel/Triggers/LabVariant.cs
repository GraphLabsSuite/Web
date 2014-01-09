using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace GraphLabs.DomainModel
{
    public partial class LabVariant : AbstractEntity
    {
        public override IEnumerable<DbValidationError> OnEntityValidating(DbEntityEntry entityEntry)
        {
            if (string.IsNullOrEmpty(Number))
                yield return new DbValidationError("Number", ValidationErrors.LabVariant_OnEntityValidating_Номер_варианта_не_может_быть_пустым_);

            if (Version <= 0)
                yield return new DbValidationError("Version", ValidationErrors.LabVariant_OnEntityValidating_Версия_варианта_должна_быть_больше_0_);
        }
    }
}
