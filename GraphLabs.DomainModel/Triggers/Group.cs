using System.Collections.Generic;
using GraphLabs.Dal.Ef;
using GraphLabs.DomainModel.Infrastructure;

namespace GraphLabs.DomainModel
{
    /// <summary> Группа </summary>
    public partial class Group : AbstractEntity
    {
        /// <summary> Валидация </summary>
        public override IEnumerable<EntityValidationError> OnEntityValidating()
        {
            if (Number <= 0)
                yield return new EntityValidationError("Number", ValidationErrors.Group_OnValidating_Номер_группы_должен_быть_больше_0_);
        }
    }
}
