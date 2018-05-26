using System;
using System.Collections.Generic;
using GraphLabs.DomainModel.Infrastructure;

namespace GraphLabs.DomainModel
{
    /// <summary> Абстрактная запись расписания лаб </summary>
    partial class AbstractLabSchedule : AbstractEntity
    {
        /// <summary> Валидация </summary>
        public override IEnumerable<EntityValidationError> OnEntityValidating()
        {
            if (DateFrom > DateTill)
                yield return new EntityValidationError("DateFrom", ValidationErrors.AbstractLabSchedule_OnEntityValidating_Дата_с_должна_быть_больше_даты_по);
        }
    }
}
