using System.Collections.Generic;
using System.Linq;
using GraphLabs.DomainModel.EF;
using GraphLabs.DomainModel.Infrastructure;

namespace GraphLabs.DomainModel
{
    /// <summary> Задание </summary>
    public partial class Task : AbstractEntity
    {
        /// <summary> Валидация </summary>
        public override IEnumerable<EntityValidationError> OnEntityValidating()
        {
            if (string.IsNullOrWhiteSpace(Name))
                yield return new EntityValidationError("Name", ValidationErrors.Task_OnValidating_Необходимо_указать_название_задания_);

            if (Xap == null || !Xap.Any())
                yield return new EntityValidationError("Xap", ValidationErrors.Task_OnValidating_Указан_пустой_исполняемый_файл_задания_);

            if (string.IsNullOrWhiteSpace(Version))
                yield return new EntityValidationError("Version", ValidationErrors.Task_OnValidating_Необходимо_указать_версию_);
        }
    }
}
