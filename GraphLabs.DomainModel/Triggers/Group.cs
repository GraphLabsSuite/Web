using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace GraphLabs.DomainModel
{
    /// <summary> Группа </summary>
    public partial class Group : AbstractEntity
    {
        /// <summary> Валидация </summary>
        public override IEnumerable<DbValidationError> OnEntityValidating(DbEntityEntry entityEntry)
        {
            if (Number <= 0)
                yield return new DbValidationError("Number", ValidationErrors.Group_OnValidating_Номер_группы_должен_быть_больше_0_);
        }
    }
}
