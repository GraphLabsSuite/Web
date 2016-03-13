using System.Collections.Generic;
using System.Linq;
using GraphLabs.Dal.Ef;
using GraphLabs.DomainModel.Infrastructure;

namespace GraphLabs.DomainModel
{
    /// <summary> Вариант задания </summary>
    public partial class TaskVariant : AbstractEntity
    {
        /// <summary> Перед сохранением новой сущности в базу </summary>
        public override void OnInsert()
        {
            base.OnInsert();

            Version = 1;
        }

        /// <summary> Валидация </summary>
        public override IEnumerable<EntityValidationError> OnEntityValidating()
        {
            if (string.IsNullOrWhiteSpace(Number))
                yield return new EntityValidationError("Number", ValidationErrors.TaskVariant_OnValidating_Необходимо_указать_номер_варианта_);
            if (string.IsNullOrWhiteSpace(GeneratorVersion))
                yield return new EntityValidationError("GeneratorVersion", ValidationErrors.TaskVariant_OnValidating_Необходимо_указать_версию_генератора_варианта_);
            if (Data == null || !Data.Any())
                yield return new EntityValidationError("Data", ValidationErrors.TaskVariant_OnValidating_Должны_быть_указаны_данные_варианта_);
            if (Version <= 0)
                yield return new EntityValidationError("Version", ValidationErrors.TaskVariant_OnValidating_Версия_должна_быть_больше_0_);
        }
    }
}
