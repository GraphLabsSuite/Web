using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace GraphLabs.DomainModel
{
    /// <summary> Задание </summary>
    public partial class Task : AbstractEntity
    {
        /// <summary> Валидация </summary>
        public override IEnumerable<DbValidationError> OnEntityValidating(DbEntityEntry entityEntry)
        {
            if (string.IsNullOrWhiteSpace(Name))
                yield return new DbValidationError("Name", ValidationErrors.Task_OnValidating_Необходимо_указать_название_задания_);

            if (Xap == null || !Xap.Any())
                yield return new DbValidationError("Xap", ValidationErrors.Task_OnValidating_Указан_пустой_исполняемый_файл_задания_);

            if (string.IsNullOrWhiteSpace(Version))
                yield return new DbValidationError("Version", ValidationErrors.Task_OnValidating_Необходимо_указать_версию_);
        }
    }
}
