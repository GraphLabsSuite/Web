using System;
using GraphLabs.DomainModel.EF;
using GraphLabs.DomainModel.Infrastructure;

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

        public override void OnChange(IEntityChange change)
        {
            base.OnChange(change);

            if ((DateTime)change.OriginalValues["SystemDate"] < (DateTime)change.CurrentValues["SystemDate"])
                throw new GraphLabsValidationException("SystemDate", ValidationErrors.Settings_OnValidating_Новое_значение_системного_времени_не_может_быть_меньше_старого_);
        }
    }
}
