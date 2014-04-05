using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Site.Logic
{
    /// <summary> Менеджер новостей </summary>
    public class NewsManager : INewsManager
    {
        private readonly IUserRepository _userRepository;
        private readonly INewsRepository _newsRepository;
        private readonly DbContextManager _dbContextManager;

        public NewsManager(
            IUserRepository userRepository,
            INewsRepository newsRepository,
            DbContextManager dbContextManager)
        {
            _userRepository = userRepository;
            _newsRepository = newsRepository;
            _dbContextManager = dbContextManager;
        }

        /// <summary> Создать или редактировать запись </summary>
        public bool CreateOrEditNews(long id, string title, string text, string authorEmail)
        {
            var user = _userRepository.FindActiveUserByEmail(authorEmail);
            if (user == null)
                return false;

            if (id == 0)
            {
                _newsRepository.Create(title, text, user);

                _dbContextManager.Commit();
                return true;
            }
            else
            {
                var news = _newsRepository.GetById(id);
                news.Text = text;
                news.Title = title;
                news.User = user;

                _dbContextManager.Commit();
                return true;
            }
        }
    }
}
