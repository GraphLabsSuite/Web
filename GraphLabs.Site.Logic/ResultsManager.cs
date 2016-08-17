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
        public long StartLabExecution(long variantId, Guid sessionGuid)
        {
            var variant = GetLabVariant(variantId);
            var student = GetCurrentStudent(sessionGuid);
            var resultsToInterrupt = FindResultsToInterrupt(sessionGuid);
            var latestCurrentResult = FindLatestCurrentResult(resultsToInterrupt, variantId);
            var unsolvedTask = 0;
            // Если есть, то вместо начала нового выполнения, продолжим старое.
            if (latestCurrentResult != null)
            {
                resultsToInterrupt = resultsToInterrupt.Except(new[] { latestCurrentResult });
            }

            foreach (var oldResult in resultsToInterrupt)
            {
                //TODO: Заменить Score
                oldResult.Score = null;
                oldResult.Status = ExecutionStatus.Interrupted;
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
                result.Status = ExecutionStatus.Executing;
                result.Score = null;
                foreach (var taskVariant in variant.TaskVariants)
                {
                    var taskResult = _reportsContext.TaskResults.CreateNew();
                    taskResult.Status = ExecutionStatus.Executing;
                    ;
                    taskResult.StudentActions = new List<StudentAction>();
                    taskResult.TaskVariant = taskVariant;
                    taskResult.Result = result;

                    result.TaskResults.Add(taskResult);
                }
            }
            else
            {
                foreach (var taskResult in latestCurrentResult.TaskResults)
                {
                    if (taskResult.Status == ExecutionStatus.Executing)
                    {
                        return taskResult.TaskVariant.Task.Id;
                    }
                }
            }

            _changesTracker.SaveChanges();
            return unsolvedTask;
        }

        public void EndLabExecution(long labVarId, Guid sessionGuid)
        {
            var resultsToInterrupt = FindResultsToInterrupt(sessionGuid);
            var latestCurrentResult = FindLatestCurrentResult(resultsToInterrupt, labVarId);
            var taskResults = latestCurrentResult.TaskResults;
            var mark = GetMark(taskResults);
            latestCurrentResult.Score = mark;
            latestCurrentResult.Status = ExecutionStatus.Complete;
            _changesTracker.SaveChanges();
        }

        private IEnumerable<Result> FindResultsToInterrupt(Guid sessionGuid)
        {
            var student = GetCurrentStudent(sessionGuid);

            //Найти неоконченные результаты выполнения
            return _resultsRepository.FindNotFinishedResults(student);
        }

        private Result FindLatestCurrentResult(IEnumerable<Result> resultsToInterrupt, long variantId)
        {

            var variant = GetLabVariant(variantId);
            // Найдём результаты, относящиеся к варианту ЛР, который пытаемся начать выполнять
            var currentResults = resultsToInterrupt
                .Where(res => res.LabVariant == variant)
                .OrderByDescending(res => res.StartDateTime)
                .ToArray();

            // Посмотрим, есть ли вообще такие. Если есть, берём самый свежий (теоретически, там больше 1 и не должно быть).
            return currentResults.FirstOrDefault();
        }

        private int?[] GetTaskResultsScore(ICollection<TaskResult> taskResults)
        {
            var taskResultsArray = taskResults.ToArray();
            var result = new int?[taskResultsArray.Length];
            for (var i = 0; i < taskResultsArray.Length; i++)
            {
                result[i] = taskResultsArray[i].Score;
            }
            return result;
        }

        private int GetMark(ICollection<TaskResult> taskResults)
        {
            var scores = GetTaskResultsScore(taskResults);
            var sum = 0;
            for (int i = 0; i < scores.Length; i++)
            {
                if (scores[i] == null) return -1;
                sum = (int)scores[i] + sum;

            }
            return sum / scores.Length;
        }
    }
}
