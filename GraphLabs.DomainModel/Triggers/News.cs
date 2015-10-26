using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace GraphLabs.DomainModel
{
    /// <summary> Новости </summary>
    public partial class News : AbstractEntity
    {
        /// <summary> Перед сохранением новой сущности в базу </summary>
        public override void OnInsert()
        {
            base.OnInsert();

            PublicationTime = DateTime.Now;
            LastModificationTime = null;
        }

        /// <summary> Перед сохранением изменённой сущности в базу </summary>
        public override void OnChange(DbEntityEntry entry)
        {
            base.OnChange(entry);

            LastModificationTime = DateTime.Now;
        }

        /// <summary> Валидация </summary>
        public override IEnumerable<DbValidationError> OnEntityValidating(DbEntityEntry entityEntry)
        {
            if (string.IsNullOrWhiteSpace(Title))
                yield return new DbValidationError("Title", ValidationErrors.News_OnValidating_Заголовок_новости_не_может_быть_пустым_);

            if (string.IsNullOrWhiteSpace(Text))
                yield return new DbValidationError("Text", ValidationErrors.News_OnValidating_Текст_новости_не_может_быть_пустым_);
        }
    }
}
