using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace GraphLabs.DomainModel
{
    /// <summary> Сессия пользователя </summary>
    public partial class Settings : AbstractEntity
    {
        /// <summary> Перед сохранением новой сущности в базу </summary>
        public override void OnInsert()
        {
            base.OnInsert();

            Id = 0; // таким образом гарантируем, что в таблице лежит ровно одна запись.
        }

        /// <summary> Валидация </summary>
        public override IEnumerable<DbValidationError> OnEntityValidating(DbEntityEntry entityEntry)
        {
            if ((DateTime)entityEntry.OriginalValues["SystemDate"] < (DateTime)entityEntry.CurrentValues["SystemDate"])
                yield return new DbValidationError("SystemDate", ValidationErrors.Settings_OnValidating_Новое_значение_системного_времени_не_может_быть_меньше_старого_);
        }
    }
}
