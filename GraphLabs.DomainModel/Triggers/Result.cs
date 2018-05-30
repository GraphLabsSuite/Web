using System;
using System.Collections.Generic;
using GraphLabs.Dal.Ef;
using GraphLabs.DomainModel.Infrastructure;

namespace GraphLabs.DomainModel
{
    /// <summary> Результат выполнения работы </summary>
    public partial class Result : AbstractEntity
    {
        /// <summary> Перед сохранением новой сущности в базу </summary>
        public override void OnInsert()
        {
            base.OnInsert();

            StartDateTime = DateTime.Now;
            Score = null;
        }

        /// <summary> Валидация </summary>
        public override IEnumerable<EntityValidationError> OnEntityValidating()
        {
            yield break;
        }
        public virtual ICollection<StudentAction> StudentActions { get; set; }
        public virtual TaskVariant TaskVariant { get; set; }
    }
}
