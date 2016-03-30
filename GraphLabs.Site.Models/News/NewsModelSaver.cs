using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Logic.Security;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.News
{
    /// <summary> Сервис сохранение новостей </summary>
    sealed class NewsModelSaver : IEntityBasedModelSaver<NewsModel, DomainModel.News>
    {
        private readonly IGraphLabsPrincipal _currentUser;
        private readonly IOperationContextFactory<IGraphLabsContext> _operationContextFactory;

        /// <summary> Сервис сохранение новостей </summary>
        public NewsModelSaver(
            IGraphLabsPrincipal currentUser,
            IOperationContextFactory<IGraphLabsContext> operationContextFactory)
        {
            _currentUser = currentUser;
            _operationContextFactory = operationContextFactory;
        }

        public DomainModel.News CreateOrUpdate(NewsModel model)
        {
            using (var operation = _operationContextFactory.Create())
            {
                var user = operation.DataContext.Query
                    .OfEntities<User>()
                    .SingleOrDefault(u => u.Email == _currentUser.Identity.Name && !(u is Student));

                if (user == null)
                    return null;

                DomainModel.News news;
                if (model.Id == 0)
                {
                    news = operation.DataContext.Factory.Create<DomainModel.News>();
                }
                else
                {
                    news = operation.DataContext.Query.Get<DomainModel.News>(model.Id);
                    if (news.User != user && !user.Role.HasFlag(UserRole.Administrator))
                    {
                        return null;
                    }
                }

                news.Title = model.Title;
                news.Text = model.Text;
                news.User = user;

                operation.Complete();

                return news;
            }
        }
    }
}