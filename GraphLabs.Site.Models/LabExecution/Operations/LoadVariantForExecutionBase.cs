using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Tasks.Contract;

namespace GraphLabs.Site.Models.LabExecution.Operations
{
    internal abstract class LoadVariantForExecutionBase : AbstractOperation
    {
        private readonly IAuthenticationSavingService _authService;
        private readonly IInitParamsProvider _initParamsProvider;
        private readonly TaskExecutionModelLoader _taskModelLoader;

        protected LoadVariantForExecutionBase(
            IOperationContextFactory<IGraphLabsContext> operationFactory,
            IAuthenticationSavingService authService,
            IInitParamsProvider initParamsProvider,
            TaskExecutionModelLoader taskModelLoader)
            : base(operationFactory)
        {
            _authService = authService;
            _initParamsProvider = initParamsProvider;
            _taskModelLoader = taskModelLoader;
        }

        protected VariantExecutionModelBase LoadImpl(LabVariant variant, int? taskIndex, Uri taskCompleteRedirect)
        {
            
            var lab = variant.LabWork;

            var student = GetCurrentStudent();
            var resultsToInterrupt = FindResultsToInterrup(student);
            var latestCurrentResult = FindLatestCurrentResult(resultsToInterrupt, lab);

            // Если есть, то вместо начала нового выполнения, продолжим старое.
            if (latestCurrentResult != null)
            {
                resultsToInterrupt = resultsToInterrupt.Except(new[] { latestCurrentResult }).ToArray();
            }

            foreach (var oldResult in resultsToInterrupt)
            {
                oldResult.Score = null;
                oldResult.Status = ExecutionStatus.Interrupted;
            }

            Result result;
            if (latestCurrentResult == null)
            {
                // Если не нашли, то заводим новый
                result = Factory.Create<Result>();
                result.LabVariant = variant;
                result.Mode = variant.IntroducingVariant
                    ? LabExecutionMode.IntroductoryMode
                    : LabExecutionMode.TestMode;
                result.Student = student;

                foreach (var taskVariant in variant.TaskVariants)
                {
                    var taskResult = Factory.Create<TaskResult>();
                    taskResult.Status = ExecutionStatus.Executing;
                    taskResult.TaskVariant = taskVariant;
                    taskResult.Result = result;

                    result.AbstractResultEntries.Add(taskResult);
                }
            }
            else
            {
                result = latestCurrentResult;
            }

            var task = taskIndex.HasValue
                ? GetTaskByIndex(lab, taskIndex.Value)
                : GetFirstUnsolvedTask(result);

            var model = task == null
                ? CompleteVariant(result) :
                CreateTaskExecutionModel(taskCompleteRedirect, task, variant, lab, result);

            return model;
        }

        private VariantExecutionModelBase CreateTaskExecutionModel(Uri taskCompleteRedirect, Task task, LabVariant variant, LabWork lab, Result result)
        {
            var initParams = InitParams.ForDemoMode(
                _authService.GetSessionInfo().SessionGuid,
                task.Id,
                variant.Id,
                lab.Id,
                taskCompleteRedirect);

            var model = CreateModelHeader<TaskVariantExecutionModel>(result, task);
            model.TaskName = task.Name;
            model.TaskId = task.Id;
            model.InitParams = _initParamsProvider.GetInitParamsString(initParams);

            return model;
        }

        private IEnumerable<TaskListEntryModel> GetOtherTasksModels(LabWork lab, Result result, Task task)
        {
            var otherTasks = lab.LabEntries
                .Select(e =>
                {
                    var model = _taskModelLoader.Load(result, e);
                    if (e.Task.Id == task?.Id)
                    {
                        model.State = TaskExecutionState.CurrentlySolving;
                    }
                    return model;
                });

            return otherTasks;
        }

        private TModel CreateModelHeader<TModel>(Result result, Task currentTask)
            where TModel : VariantExecutionModelBase, new()
        {
            var variant = result.LabVariant;
            var lab = variant.LabWork;

            return new TModel
            {
                VariantId = variant.Id,
                LabName = lab.Name,
                OtherTasks = GetOtherTasksModels(lab, result, currentTask).ToArray()
            };
        }


        #region Завершение лабы

        private VariantExecutionModelBase CompleteVariant(Result result)
        {
            Contract.Assert(result.AbstractResultEntries.All(r => r.Status == ExecutionStatus.Complete));

            var mark = GetMark(result.AbstractResultEntries);
            result.Score = mark;
            result.Status = ExecutionStatus.Complete;

            var model = CreateModelHeader<VariantExecutionCompleteModel>(result, null);
            model.ResultMessage = $"Работа завершена! Ваш балл: {mark}.";

            return model;
        }

        private int GetMark(ICollection<AbstractResultEntry> taskResults)
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

        private int?[] GetTaskResultsScore(ICollection<AbstractResultEntry> taskResults)
        {
            var taskResultsArray = taskResults.ToArray();
            var result = new int?[taskResultsArray.Length];
            for (var i = 0; i < taskResultsArray.Length; i++)
            {
                result[i] = taskResultsArray[i].Score;
            }
            return result;
        }

        #endregion


        private Task GetFirstUnsolvedTask(Result labResult)
        {
            var solvedTasks = labResult
                .AbstractResultEntries
                .Where(r => r.Status == ExecutionStatus.Complete)
                .OfType<TaskResult>()
                .Select(r => r.TaskVariant.Task.Id)
                .ToArray();

            var searchResult = labResult
                .LabVariant
                .LabWork
                .LabEntries
                .OrderBy(e => e.Order)
                .FirstOrDefault(e => !solvedTasks.Contains(e.Task.Id));

            return searchResult?.Task;
        }

        private Task GetTaskByIndex(LabWork lab, int index)
        {
            var result = lab.LabEntries
                .OrderBy(e => e.Order)
                .Skip(index)
                .FirstOrDefault();

            if (result == null)
                throw new Exception("Задания с запрошенным номером не существует.");

            return result.Task;
        }

        private Student GetCurrentStudent()
        {
            var session = _authService.GetSessionInfo();

            var ip = HttpContext.Current.Request.UserHostAddress;
            var matchingSessions = Query.OfEntities<Session>()
                .Where(s => s.Guid == session.SessionGuid && s.IP == ip) //todo куда-то вытащить
                .ToArray();

            if (matchingSessions.Count() == 1)
            {
                var student = matchingSessions.Single().User as Student;
                if (student != null)
                    return student;
            }

            throw new Exception("Сессия студента не найдена.");
        }

        private Result[] FindResultsToInterrup(Student student)
        {
            //Найти неоконченные результаты выполнения
            return student.Results
                .Where(result => result.Status == ExecutionStatus.Executing)
                // .Include(i => i.)
                .ToArray();
        }

        private Result FindLatestCurrentResult(IEnumerable<Result> resultsToInterrupt, LabWork lab)
        {
            // Найдём результаты, относящиеся к ЛР, которую пытаемся начать выполнять
            var currentResults = resultsToInterrupt
                .Where(res => res.LabVariant.LabWork == lab)
                .OrderByDescending(res => res.StartDateTime);

            // Посмотрим, есть ли вообще такие. Если есть, берём самый свежий (теоретически, там больше 1 и не должно быть).
            return currentResults.FirstOrDefault();
        }
    }
}