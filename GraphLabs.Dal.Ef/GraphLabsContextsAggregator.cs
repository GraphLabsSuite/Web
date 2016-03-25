using System;
using System.Diagnostics.Contracts;
using System.Linq;
using GraphLabs.Dal.Ef.Infrastructure;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Infrastructure;
using JetBrains.Annotations;

namespace GraphLabs.Dal.Ef
{
    /// <summary> Обёртка EF-контекста </summary>
    [UsedImplicitly]
    sealed class GraphLabsContextsAggregator :
        INewsContext,
        IUsersContext,
        ISessionsContext,
        IReportsContext,
        ITestsContext,
        ILabWorksContext,
        ITasksContext,
        ISystemContext
    {
        private readonly GraphLabsContext _ctx;

        public GraphLabsContextsAggregator(GraphLabsContext ctx)
        {
            _ctx = ctx;

            _users = new Lazy<IEntitySet<User>>(() => new EntitySet<User>(ctx));
            _groups = new Lazy<IEntitySet<Group>>(() => new EntitySet<Group>(ctx));
            _news = new Lazy<IEntitySet<News>>(() => new EntitySet<News>(ctx));
            _sessions = new Lazy<IEntitySet<Session>>(() => new EntitySet<Session>(ctx));
            _results = new Lazy<IEntitySet<Result>>(() => new EntitySet<Result>(ctx));
            _actions = new Lazy<IEntitySet<StudentAction>>(() => new EntitySet<StudentAction>(ctx));
            _testQuestions = new Lazy<IEntitySet<TestQuestion>>(() => new EntitySet<TestQuestion>(ctx));
            _answerVariants = new Lazy<IEntitySet<AnswerVariant>>(() => new EntitySet<AnswerVariant>(ctx));
            _categories = new Lazy<IEntitySet<Category>>(() => new EntitySet<Category>(ctx));
            _labWorks = new Lazy<IEntitySet<LabWork>>(() => new EntitySet<LabWork>(ctx));
            _labVariants = new Lazy<IEntitySet<LabVariant>>(() => new EntitySet<LabVariant>(ctx));
            _labEntries = new Lazy<IEntitySet<LabEntry>>(() => new EntitySet<LabEntry>(ctx));
            _tasks = new Lazy<IEntitySet<Task>>(() => new EntitySet<Task>(ctx));
            _taskVariants = new Lazy<IEntitySet<TaskVariant>>(() => new EntitySet<TaskVariant>(ctx));
            _settings = new Lazy<IEntitySet<Settings>>(() => new EntitySet<Settings>(ctx));
        }


        private readonly Lazy<IEntitySet<User>> _users;
        private readonly Lazy<IEntitySet<Group>> _groups;
        private readonly Lazy<IEntitySet<News>> _news;
        private readonly Lazy<IEntitySet<Session>> _sessions;
        private readonly Lazy<IEntitySet<Result>> _results;
        private readonly Lazy<IEntitySet<StudentAction>> _actions;
        private readonly Lazy<IEntitySet<TestQuestion>> _testQuestions;
        private readonly Lazy<IEntitySet<AnswerVariant>> _answerVariants;
        private readonly Lazy<IEntitySet<Category>> _categories;
        private readonly Lazy<IEntitySet<LabWork>> _labWorks;
        private readonly Lazy<IEntitySet<LabVariant>> _labVariants;
        private readonly Lazy<IEntitySet<LabEntry>> _labEntries;
        private readonly Lazy<IEntitySet<Task>> _tasks;
        private readonly Lazy<IEntitySet<TaskVariant>> _taskVariants;
        private readonly Lazy<IEntitySet<Settings>> _settings;

        /// <summary> Пользователи </summary>
        public IEntitySet<User> Users => _users.Value;

        /// <summary> Группы </summary>
        public IEntitySet<Group> Groups => _groups.Value;

        /// <summary> Новости </summary>
        public IEntitySet<News> News => _news.Value;

        /// <summary> Сессии </summary>
        public IEntitySet<Session> Sessions => _sessions.Value;
        
        /// <summary> Результаты </summary>
        public IEntitySet<Result> Results => _results.Value;

        /// <summary> Действия </summary>
        public IEntitySet<StudentAction> Actions => _actions.Value;

        /// <summary> Тестовые вопросы </summary>
        public IEntitySet<TestQuestion> TestQuestions => _testQuestions.Value;

        /// <summary> Варианты ответов </summary>
        public IEntitySet<AnswerVariant> AnswerVariants => _answerVariants.Value;

        /// <summary> Категории </summary>
        public IEntitySet<Category> Categories => _categories.Value;

        /// <summary> Лабораторные работы </summary>
        public IEntitySet<LabWork> LabWorks => _labWorks.Value;

        /// <summary> Варианты ЛР </summary>
        public IEntitySet<LabVariant> LabVariants => _labVariants.Value;

        /// <summary> Связь "Задание входит в ЛР" </summary>
        public IEntitySet<LabEntry> LabEntries => _labEntries.Value;

        /// <summary> Задания </summary>
        public IEntitySet<Task> Tasks => _tasks.Value;

        /// <summary> Варианты заданий </summary>
        public IEntitySet<TaskVariant> TaskVariants => _taskVariants.Value;

        /// <summary> Системные настройки </summary>
        public IEntitySet<Settings> Settings => _settings.Value;
    }
}
