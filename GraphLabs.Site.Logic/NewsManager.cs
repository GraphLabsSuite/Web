using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using JetBrains.Annotations;
using log4net;

namespace GraphLabs.Site.Logic
{
    /// <summary> Менеджер новостей </summary>
    [UsedImplicitly]
    public class NewsManager : INewsManager
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(INewsManager));

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
            {
                _log.WarnFormat("Неудачная попытка создания/редактирования новостей. Неизвестный пользователь. Email: \"{0}\".", authorEmail ?? string.Empty);
                return false;
            }
            if (id == 0)
            {
                _newsRepository.Create(title, text, user);

                _dbContextManager.Commit();
                _log.InfoFormat("Новость \"{0}\" создана. Email автора: \"{1}\".", title, authorEmail);
                return true;
            }
         
            var news = _newsRepository.GetById(id);

            if (news.User != user && !user.Role.HasFlag(UserRole.Administrator))
            {
                _log.WarnFormat("Неудачная попытка создания/редактирования новостей: недостаточно прав. Email: \"{0}\".", authorEmail);
                return false;
            }

            news.Text = text;
            news.Title = title;
            news.User = user;

            _dbContextManager.Commit();
            _log.InfoFormat("Новость \"{0}\" отредактирована. Email автора: \"{1}\".", title, authorEmail);
            return true;
        }
    }
}
