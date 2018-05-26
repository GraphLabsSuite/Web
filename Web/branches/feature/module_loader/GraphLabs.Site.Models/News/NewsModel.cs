using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Security.Principal;
using System.Web;
using GraphLabs.Site.Models.Infrastructure;
using JetBrains.Annotations;

namespace GraphLabs.Site.Models.News
{
    /// <summary> Модель новости </summary>
    public class NewsModel : IEntityBasedModel<DomainModel.News>
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
        [Display(Name = "Содержание")]
        public string Text { get; set; }

        /// <summary> Опубликовал </summary>
        public string Publisher { get; set; }

        /// <summary> Дата публикации </summary>
        public string PublishDate { get; set; }

        /// <summary> Может редактироваться текущим пользователем? </summary>
        public bool CanEdit { get; set; }

        /// <summary> Модель новости </summary>
        [UsedImplicitly]
        public NewsModel()
        {
        }

        private HttpContext HttpContext
        {
            get
            {
                return HttpContext.Current;
            }
        }
    }
}