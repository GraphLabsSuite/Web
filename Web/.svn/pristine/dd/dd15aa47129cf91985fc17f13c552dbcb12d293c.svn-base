using System.Collections.Generic;
using GraphLabs.Dal.Ef;
using GraphLabs.DomainModel.Infrastructure;

namespace GraphLabs.DomainModel
{
    public partial class LabVariant : AbstractEntity
    {
        public override IEnumerable<EntityValidationError> OnEntityValidating()
        {
            if (string.IsNullOrEmpty(Number))
                yield return new EntityValidationError("Number", ValidationErrors.LabVariant_OnEntityValidating_Номер_варианта_не_может_быть_пустым_);

            if (Version <= 0)
                yield return new EntityValidationError("Version", ValidationErrors.LabVariant_OnEntityValidating_Версия_варианта_должна_быть_больше_0_);
        }
    }
}
