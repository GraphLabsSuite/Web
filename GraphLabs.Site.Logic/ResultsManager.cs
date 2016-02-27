using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GraphLabs.DomainModel.EF;
using GraphLabs.DomainModel.EF.Repositories;

namespace GraphLabs.Site.Logic
{
    /// <summary> Менеджер результатов </summary>
    public class ResultsManager : IResultsManager
    {
        private readonly ILabRepository _labRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IResultsRepository _resultsRepository;

        /// <summary> Менеджер результатов </summary>
        public ResultsManager(
            ILabRepository labRepository,
            ISessionRepository sessionRepository,
            IResultsRepository resultsRepository)
        {
            _labRepository = labRepository;
            _sessionRepository = sessionRepository;
            _resultsRepository = resultsRepository;
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
                oldResult.Grade = Grade.Interrupted;
            }

            if (latestCurrentResult == null)
            {
                // Если не нашли, то заводим новый
                var result = new Result
                {
                    LabVariant = variant,
                    Mode =
                        variant.IntroducingVariant ? LabExecutionMode.IntroductoryMode : LabExecutionMode.TestMode,
                    Student = student
                };
                _resultsRepository.Insert(result);
            }
        }
    }
}
