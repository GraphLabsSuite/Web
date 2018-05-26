using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Logic.Security;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.News
{
    /// <summary> Загрузчик моделей групп </summary>
    class NewsModelLoader : AbstractModelLoader<NewsModel, DomainModel.News>
    {
        private readonly IGraphLabsPrincipal _currentUser;

        /// <summary> Загрузчик моделей групп </summary>
        public NewsModelLoader(IEntityQuery query, IGraphLabsPrincipal currentUser)
            : base(query)
        {
            _currentUser = currentUser;
        }

        /// <summary> Загрузить по сущности-прототипу </summary>
        public override NewsModel Load(DomainModel.News news)
        {
            var model = new NewsModel
            {
                Id = news.Id,
                Title = news.Title,
                Text = news.Text,
                Publisher = news.User.GetShortName(),
                PublishDate = !news.LastModificationTime.HasValue
                    ? news.PublicationTime.ToShortDateString()
                    : $"Обновлено {news.LastModificationTime.Value.ToShortDateString()}",

                CanEdit = _currentUser.Identity.Name == news.User.Email || _currentUser.IsInRole(UserRole.Administrator)
            };

            return model;
        }
    }
}