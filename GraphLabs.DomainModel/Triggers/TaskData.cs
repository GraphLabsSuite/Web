using System.Collections.Generic;
using System.Linq;
using GraphLabs.DomainModel.Infrastructure;

namespace GraphLabs.DomainModel
{
    /// <summary> Бинарник задания </summary>
    public partial class TaskData : AbstractEntity
    {
        /// <summary> Валидация </summary>
        public override IEnumerable<EntityValidationError> OnEntityValidating()
        {
            if (Xap == null || !Xap.Any())
                yield return new EntityValidationError("Xap", ValidationErrors.Task_OnValidating_Указан_пустой_исполняемый_файл_задания_);
        }
    }
}