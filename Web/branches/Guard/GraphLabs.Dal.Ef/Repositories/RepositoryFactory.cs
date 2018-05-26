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
            Contract.Requires(context != null);
            Guard.Guard.IsNotNull(context, nameof(context)); //*
            _context = context;
            _systemDateService = systemDateService;
            _tasksContext = tasksContext;
        }

        /// <summary> Получить репозиторий с группами </summary>
        [NotNull]
        public IGroupRepository GetGroupRepository()
        {
            Contract.Ensures(Contract.Result<IGroupRepository>() != null);
            GroupRepository groupRepository = new GroupRepository(_context);
            Guard.Guard.IsNotNull(groupRepository, nameof(groupRepository));
            return groupRepository;
        }

        /// <summary> Получить репозиторий с группами </summary>
        [NotNull]
        public IUserRepository GetUserRepository()
        {
            Contract.Ensures(Contract.Result<IUserRepository>() != null);
            UserRepository userRepository = new UserRepository(_context);
            Guard.Guard.IsNotNull(userRepository, nameof(userRepository));
            return userRepository;
        }

        /// <summary> Получить репозиторий с группами </summary>
        [NotNull]
        public ISessionRepository GetSessionRepository()
        {
            Contract.Ensures(Contract.Result<ISessionRepository>() != null);
            SessionRepository sessionRepository = new SessionRepository(_context, _systemDateService);
            Guard.Guard.IsNotNull(sessionRepository, nameof(sessionRepository));
            return sessionRepository;
        }

        /// <summary> Получить репозиторий с лабораторными работами </summary>
        [NotNull]
        public ILabRepository GetLabRepository()
        {
            Contract.Ensures(Contract.Result<ILabRepository>() != null);
            LabRepository labRepository = new LabRepository(_context, _tasksContext);
            Guard.Guard.IsNotNull(labRepository, nameof(labRepository));
            return labRepository;
        }

		[NotNull]
		public ICategoryRepository GetCategoryRepository()
		{
			Contract.Ensures(Contract.Result<ICategoryRepository>() != null);
            CategoryRepository categoryRepository = new CategoryRepository(_context);
            Guard.Guard.IsNotNull(categoryRepository, nameof(categoryRepository));
            return categoryRepository;
		}

        [NotNull]
        public ISurveyRepository GetSurveyRepository()
        {
            Contract.Ensures(Contract.Result<ISurveyRepository>() != null);
            SurveyRepository surveyRepository = new SurveyRepository(_context);
            Guard.Guard.IsNotNull(surveyRepository, nameof(surveyRepository));
            return surveyRepository;
        }

        [NotNull]
        public ITestPoolRepository GetTestPoolRepository()
        {
            Contract.Ensures(Contract.Result<ITestPoolRepository>() != null);
            TestPoolRepository testPoolRepository = new TestPoolRepository(_context);
            Guard.Guard.IsNotNull(testPoolRepository, nameof(testPoolRepository));
            return testPoolRepository;
        }
    }
}