using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Web;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.EF;
using GraphLabs.DomainModel.EF.Extensions;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Logic.Security;
using JetBrains.Annotations;

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

        /// <summary> Модель новости </summary>
        public NewsModel(News news)
        {
            Contract.Requires<ArgumentNullException>(news != null);
            
            Id = news.Id;
            Title = news.Title;
            Text = news.Text;
            Publisher = news.User.GetShortName();
            PublishDate = !news.LastModificationTime.HasValue 
                ? news.PublicationTime.ToShortDateString() 
                : string.Format("Обновлено {0}", news.LastModificationTime.Value.ToShortDateString());

            var currentUser = HttpContext.User;
            CanEdit = currentUser.Identity.Name == news.User.Email || currentUser.IsInRole(UserRole.Administrator);
        }
    }
}