using System.Diagnostics.Contracts;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel.Repositories;
using JetBrains.Annotations;

namespace GraphLabs.Dal.Ef.Repositories
{
    /// <summary> Фабрика репозиториев </summary>
    [UsedImplicitly]
    public sealed class RepositoryFactory
    {
        private readonly GraphLabsContext _context;
        private readonly ISystemDateService _systemDateService;
        private readonly ITasksContext _tasksContext;

        /// <summary> Фабрика репозиториев </summary>
        public RepositoryFactory(GraphLabsContext context, ISystemDateService systemDateService, ITasksContext tasksContext)
        {
            Guard.IsNotNull(nameof(context), context); 
            _context = context;
            _systemDateService = systemDateService;
            _tasksContext = tasksContext;
        }

        /// <summary> Получить репозиторий с группами </summary>
        [NotNull]
        public IGroupRepository GetGroupRepository()
        {
            var groupRepository = new GroupRepository(_context);
            Guard.IsNotNull(nameof(groupRepository), groupRepository);
            return groupRepository;
        }

        /// <summary> Получить репозиторий с группами </summary>
        [NotNull]
        public IUserRepository GetUserRepository()
        {
            var userRepository = new UserRepository(_context);
            Guard.IsNotNull(nameof(userRepository), userRepository);
            return userRepository;
        }

        /// <summary> Получить репозиторий с группами </summary>
        [NotNull]
        public ISessionRepository GetSessionRepository()
        {
            var sessionRepository = new SessionRepository(_context, _systemDateService);
            Guard.IsNotNull(nameof(sessionRepository), sessionRepository);
            return sessionRepository;
        }

        /// <summary> Получить репозиторий с лабораторными работами </summary>
        [NotNull]
        public ILabRepository GetLabRepository()
        {
            var labRepository = new LabRepository(_context, _tasksContext);
            Guard.IsNotNull(nameof(labRepository), labRepository);
            return labRepository;
        }

		[NotNull]
		public ICategoryRepository GetCategoryRepository()
		{
            var categoryRepository = new CategoryRepository(_context);
            Guard.IsNotNull(nameof(categoryRepository), categoryRepository);
            return categoryRepository;
		}

        [NotNull]
        public ISurveyRepository GetSurveyRepository()
        {
            var surveyRepository = new SurveyRepository(_context);
            Guard.IsNotNull(nameof(surveyRepository), surveyRepository);
            return surveyRepository;
        }

        [NotNull]
        public ITestPoolRepository GetTestPoolRepository()
        {
            var testPoolRepository = new TestPoolRepository(_context);
            Guard.IsNotNull(nameof(testPoolRepository), testPoolRepository);
            return testPoolRepository;
        }
    }
}