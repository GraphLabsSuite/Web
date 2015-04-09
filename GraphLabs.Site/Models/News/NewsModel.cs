using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.DomainModel.Services;
using GraphLabs.Site.Logic.Security;
using JetBrains.Annotations;
using log4net;
using Microsoft.Practices.Unity;

namespace GraphLabs.Site.Models.News
{
    /// <summary> DTO новости </summary>
    //TODO разделить на две модели для редактирования и просмотра
    public sealed class NewsModel : GraphLabsModel
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(NewsModel));

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
        public string Publisher { get; private set; }

        /// <summary> Дата публикации </summary>
        public string PublishDate { get; private set; }

        /// <summary> Может редактироваться текущим пользователем? </summary>
        public bool CanEdit { get; private set; }

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
        public NewsModel(DomainModel.News news)
        {
            Contract.Requires<ArgumentNullException>(news != null);
            InitializeWith(news);
        }

        private void InitializeWith(DomainModel.News news)
        {
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


        #region Нарушим SRP. Часть для редактирования

        private readonly INewsContext _context;
        private readonly ISystemDateService _dateService;

        [InjectionConstructor]
        public NewsModel(INewsContext context, ISystemDateService dateService)
        {
            _context = context;
            _dateService = dateService;
        }

        /// <summary> Загрузить из базы </summary>
        public void Load(long id)
        {
            Contract.Assert(_context != null);

            var news = _context.News.Where(n => n.Id == id).Include(n => n.User).Single();
            InitializeWith(news);
        }

        /// <summary> Сохранить </summary>
        public bool Save(string authorEmail)
        {
            Contract.Assert(_context != null);

            var user =
                _context.Users.SingleOrDefault(
                    u => u.Email == authorEmail && new[] {UserRole.Teacher, UserRole.Administrator}.Contains(u.Role));
            if (user == null)
            {
                _log.WarnFormat("Неудачная попытка создания/редактирования новостей. Неизвестный пользователь. Email: \"{0}\".", authorEmail ?? string.Empty);
                return false;
            }
            DomainModel.News news;
            if (Id == 0)
            {
                // создаём новую новость
                news = new DomainModel.News
                {
                    Title = Title,
                    Text = Text,
                    User = user,
                    PublicationTime = _dateService.Now()
                };
                _context.News.Add(news);

                _log.InfoFormat("Новость \"{0}\" создана. Email автора: \"{1}\".", Title, authorEmail);
                return true;
            }
            else
            {
                // редактируем новость
                news = _context.News.Single(n => n.Id == Id);

                if (news.User != user && !user.Role.HasFlag(UserRole.Administrator))
                {
                    _log.WarnFormat(
                        "Неудачная попытка создания/редактирования новостей: недостаточно прав. Email: \"{0}\".",
                        authorEmail);
                    return false;
                }

                news.Text = Text;
                news.Title = Title;
                news.User = user;
                news.LastModificationTime = _dateService.Now();

                _log.InfoFormat("Новость \"{0}\" отредактирована. Email автора: \"{1}\".", Title, authorEmail);
                return true;
            }
        }

        #endregion
    }
}