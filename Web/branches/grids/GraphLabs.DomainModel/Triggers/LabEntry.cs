using System.Collections.Generic;
using GraphLabs.Dal.Ef;
using GraphLabs.DomainModel.Infrastructure;

namespace GraphLabs.DomainModel
{
    /// <summary> Элемент ЛР </summary>
    public partial class LabEntry : AbstractEntity
    {
        /// <summary> Валидация </summary>
        public override IEnumerable<EntityValidationError> OnEntityValidating()
        {
            if (Order < 0)
                yield return new EntityValidationError("Order", "Порядковый номер задания должен быть больше 0.");
        }
    }
}
