using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace GraphLabs.DomainModel.EF
{
    /// <summary> Действие при выполнении ЛР </summary>
    public partial class Action : AbstractEntity
    {
        /// <summary> Валидация </summary>
        public override IEnumerable<DbValidationError> OnEntityValidating(DbEntityEntry entityEntry)
        {
            if (string.IsNullOrWhiteSpace(Description))
                yield return new DbValidationError("Description", ValidationErrors.Action_OnEntityValidating_Необходимо_указать_описание_действия_);

            if (Penalty < 0 || Penalty > 100)
                yield return new DbValidationError("Penalty", ValidationErrors.Action_OnEntityValidating_Штрафной_балл_может_находиться_только_в_диапазоне_от_0_до_100_);
        }
    }
}
