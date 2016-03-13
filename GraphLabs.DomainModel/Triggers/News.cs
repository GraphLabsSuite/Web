using System;
using System.Collections.Generic;
using GraphLabs.Dal.Ef;
using GraphLabs.DomainModel.Infrastructure;

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
        public override void OnChange(IEntityChange change)
        {
            base.OnChange(change);

            LastModificationTime = DateTime.Now;
        }

        /// <summary> Валидация </summary>
        public override IEnumerable<EntityValidationError> OnEntityValidating()
        {
            if (string.IsNullOrWhiteSpace(Title))
                yield return new EntityValidationError("Title", ValidationErrors.News_OnValidating_Заголовок_новости_не_может_быть_пустым_);

            if (string.IsNullOrWhiteSpace(Text))
                yield return new EntityValidationError("Text", ValidationErrors.News_OnValidating_Текст_новости_не_может_быть_пустым_);
        }
    }
}
