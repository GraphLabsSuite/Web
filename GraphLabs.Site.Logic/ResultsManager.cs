using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GraphLabs.Dal.Ef;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Repositories;
using Microsoft.Practices.ObjectBuilder2;

namespace GraphLabs.Site.Logic
{
    /// <summary> Менеджер результатов </summary>
    public class ResultsManager : IResultsManager
    {
        private readonly IReportsContext _reportsContext;
        private readonly ILabRepository _labRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IResultsRepository _resultsRepository;
        private readonly IChangesTracker _changesTracker;

        /// <summary> Менеджер результатов </summary>
        public ResultsManager(
            IReportsContext reportsContext,
            ILabRepository labRepository,
            ISessionRepository sessionRepository,
            IResultsRepository resultsRepository,
            IChangesTracker changesTracker)
        {
            _reportsContext = reportsContext;
            _labRepository = labRepository;
            _sessionRepository = sessionRepository;
            _resultsRepository = resultsRepository;
            _changesTracker = changesTracker;
        }

        private Student GetCurrentStudent(Guid sessionGuid)
        {
            var session = _sessionRepository.FindByGuid(sessionGuid);
            if (session == null || !(session.User is Student))
            {
                throw new HttpException(404, "Не найдена сессия _студента_.");
            }

            return (Student)session.User;
        }

        private LabVariant GetLabVariant(long variantId)
        {
            var variant = _labRepository.FindLabVariantById(variantId);
            if (variant == null)
            {
                throw new HttpException(404, "Не найден вариант запрашиваемой ЛР.");
            }
            return variant;
        }

        /// <summary> Зафиксировать начало выполнения ЛР (создаёт заголовок результата) </summary>
        public void StartLabExecution(long variantId, Guid sessionGuid)
        {
            var student = GetCurrentStudent(sessionGuid);

            //Найти неоконченные результаты выполнения
            IEnumerable<Result> resultsToInterrupt = _resultsRepository.FindNotFinishedResults(student);
            var variant = GetLabVariant(variantId);

            // Найдём результаты, относящиеся к варианту ЛР, который пытаемся начать выполнять
            var currentResults = resultsToInterrupt
                .Where(res => res.LabVariant == variant)
                .OrderByDescending(res => res.StartDateTime)
                .ToArray();

            // Посмотрим, есть ли вообще такие. Если есть, берём самый свежий (теоретически, там больше 1 и не должно быть).
            var latestCurrentResult = currentResults.FirstOrDefault();
            // Если есть, то вместо начала нового выполнения, продолжим старое.
            if (latestCurrentResult != null)
            {
                resultsToInterrupt = resultsToInterrupt.Except(new[] {latestCurrentResult});
            }

            foreach (var oldResult in resultsToInterrupt)
            {
                //TODO: Заменить Score
                oldResult.Score = null;
                oldResult.Status = ExecutionStatus.Complete;
            }

            if (latestCurrentResult == null)
            {
                // Если не нашли, то заводим новый
                var result = _reportsContext.Results.CreateNew();
                result.LabVariant = variant;
                result.Mode = variant.IntroducingVariant
                    ? LabExecutionMode.IntroductoryMode
                    : LabExecutionMode.TestMode;
                result.Student = student;

                foreach (var taskVariant in variant.TaskVariants)
                {
                    var taskResult = _reportsContext.TaskResults.CreateNew();
                    taskResult.Status = ExecutionStatus.Executing;;
                    taskResult.StudentActions = new List<StudentAction>();
                    taskResult.TaskVariant = taskVariant;
                    taskResult.Result = result;

                    result.TaskResults.Add(taskResult);
                }
            }

            _changesTracker.SaveChanges();
        }
    }
}
