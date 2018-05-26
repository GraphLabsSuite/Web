using System.Collections.Generic;
using GraphLabs.DomainModel.Infrastructure;

namespace GraphLabs.DomainModel
{
    /// <summary> Группа </summary>
    public partial class Group : AbstractEntity
    {
        /// <summary> Валидация </summary>
        public override IEnumerable<EntityValidationError> OnEntityValidating()
        {
            if (string.IsNullOrWhiteSpace(Name))
                yield return new EntityValidationError("Number", ValidationErrors.Group_OnEntityValidating_Необходимо_указать_непустое_название_группы);
        }
    }
}
