using System;
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

            Id = 1; // таким образом гарантируем, что в таблице лежит ровно одна запись.
        }
    } 
}
