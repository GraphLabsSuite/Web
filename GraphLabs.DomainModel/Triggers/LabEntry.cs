using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace GraphLabs.DomainModel
{
    /// <summary> Элемент ЛР </summary>
    public partial class LabEntry : AbstractEntity
    {
        /// <summary> Валидация </summary>
        public override IEnumerable<DbValidationError> OnEntityValidating(DbEntityEntry entityEntry)
        {
            if (Order < 0)
                yield return new DbValidationError("Order", "Порядковый номер задания должен быть больше 0.");
        }
    }
}
