using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;

namespace GraphLabs.Site.Models
{
    /// <summary> Модель новости </summary>
    public class NewsModel
    {
        /// <summary> Id </summary>
        public long Id { get; set; }

        /// <summary> Заголовок </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать заголовок новости")]
        [MinLength(3, ErrorMessage = "Заголовок слишком короткий!")]
        [MaxLength(50, ErrorMessage = "Заголовок слишком длинный!")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        /// <summary> Содержание </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать содержание новости")]
        [MinLength(3, ErrorMessage = "Cодержание слишком короткое!")]
        [MaxLength(500, ErrorMessage = "Содержание слишком длинное!")]
        [Display(Name = "Содержание")]
        public string Text { get; set; }

        /// <summary> Опубликовал </summary>
        public string Publisher { get; set; }

        /// <summary> Дата публикации или модификации </summary>
        public DateTime Date { get; set; }

        /// <summary> Модификация была? </summary>
        public bool Modificated { get; set; }

        /// <summary> Модель соответсвует новой (отсутствующей в бд) новости? </summary>
        public bool IsNew { get; set; }

        /// <summary> Модель новости </summary>
        public NewsModel()
        {
            IsNew = true;
        }

        /// <summary> Модель новости </summary>
        public NewsModel(News news)
        {
            Contract.Requires<ArgumentNullException>(news != null);

            Id = news.Id;
            Title = news.Title;
            Text = news.Text;
            Publisher = news.User.GetShortName();
            Modificated = news.LastModificationTime.HasValue;
            Date = news.LastModificationTime ?? news.PublicationTime;
            IsNew = false;
        }
    }
}