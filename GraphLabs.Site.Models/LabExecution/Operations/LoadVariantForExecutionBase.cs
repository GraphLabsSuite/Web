using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Tasks.Contract;
using Microsoft.Practices.ObjectBuilder2;

namespace GraphLabs.Site.Models.LabExecution.Operations
{
    internal abstract class LoadVariantForExecutionBase : AbstractOperation
    {
        private readonly IAuthenticationSavingService _authService;
        private readonly IInitParamsProvider _initParamsProvider;
        private readonly TaskExecutionModelLoader _taskModelLoader;
        private readonly TestExecutionModelLoader _testModelLoader;
        private readonly IOperationContextFactory<IGraphLabsContext> _operationFactory;

        protected LoadVariantForExecutionBase(
            IOperationContextFactory<IGraphLabsContext> operationFactory,
            IAuthenticationSavingService authService,
            IInitParamsProvider initParamsProvider,
            TaskExecutionModelLoader taskModelLoader,
            TestExecutionModelLoader testModelLoader)
            : base(operationFactory)
        {
            _testModelLoader = testModelLoader;
            _authService = authService;
            _initParamsProvider = initParamsProvider;
            _taskModelLoader = taskModelLoader;
            _operationFactory = operationFactory;
        }

        protected VariantExecutionModelBase LoadImpl(DomainModel.LabVariant variant, int? taskIndex, int? testIndex, Uri taskCompleteRedirect)
        {
            
            var lab = variant.LabWork;

            var student = GetCurrentStudent();
            var resultsToInterrupt = FindResultsToInterrupt(student);
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

                if (variant.TestPool != null)
                {
                    var randomArray = new Randomizer[variant.TestPool.TestPoolEntries.Count];
                    randomArray = Randomizer.InitializeArray(randomArray);
                    var randomer = new Random(variant.TestPool.TestPoolEntries.Count);
                    foreach (var testQuestion in variant.TestPool.TestPoolEntries)
                    {
                        var testResult = Factory.Create<TestResult>();
                        testResult.TestPoolEntry = testQuestion;
                        var number = Randomizer.GetNewValue(randomArray, randomer);
                        testResult.Index = number.ToString();
                        testResult.Result = result;
                        randomArray = Randomizer.ChoseNumber(randomArray, number);
                        testResult.StudentAnswers = new List<DomainModel.StudentAnswer>();
                        result.AbstractResultEntries.Add(testResult);
                    }
                }
            }
            else
            {
                result = latestCurrentResult;
            }

            var task = TryGetTaskByIndex(result, lab, taskIndex) 
                ?? GetFirstUnsolvedTask(result);

            var test = TryGetTestPoolEntry(result, variant, testIndex)
                ?? GetFirstUnsolvedTest(result);

            var model = task == null 
                ? test == null ? CompleteVariant(result) : CreateTestExecutionModel(taskCompleteRedirect, test, variant, lab, result)
                : IsTestPoolCompleted(variant, testIndex) ? CreateTestExecutionModel(taskCompleteRedirect, test, variant, lab, result) : CreateTaskExecutionModel(taskCompleteRedirect, task, variant, lab, result);

            return model;
        }

        private class Randomizer
        {
            public int Number;
            public bool Chosen;

            public Randomizer(int number, bool chosen)
            {
                this.Number = number;
                this.Chosen = chosen;
            }

            public static Randomizer[] InitializeArray(Randomizer[] array)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = new Randomizer(i, false);
                }
                return array;
            }

            public static Randomizer[] ChoseNumber(Randomizer[] array, int number)
            {
                array[number].Chosen = true;
                return array;
            }

            private static bool CheckChosen(Randomizer[] array, int number)
            {
                return array[number].Chosen;
            }

            public static int GetNewValue(Randomizer[] array, Random randomer)
            {
                var next = randomer.Next(array.Length);
                while (CheckChosen(array, next))
                {
                    next = randomer.Next();
                }
                return next;
            }
        }

        private VariantExecutionModelBase CreateTestExecutionModel(Uri taskCompleteRedirect, DomainModel.TestPoolEntry test, DomainModel.LabVariant variant, LabWork lab, Result result)
        {
            var model = new TestExecutionModel();
            model.Name = test.TestPool.Name;
            model.Question = test.TestQuestion.Question;
            model.Answers = test.TestQuestion.AnswerVariants;
            model.QuestionId = test.Id;
            model.OtherTasks = GetOtherTasksModels(lab, result, null, test).ToArray();
            model.VariantId = variant.Id;
            model.LabName = lab.Name;
            var operation = _operationFactory.Create();
            var testResult = operation.DataContext.Query.OfEntities<TestResult>().FirstOrDefault(e => e.TestPoolEntry.Id == test.Id & e.Result.Id == result.Id);
            operation.Complete();
            model.TestResult = testResult.Id;
            return model;
        }

        private bool IsTestPoolCompleted(DomainModel.LabVariant variant, int? testIndex)
        {
            // TODO: Добавить организацию запрещения перехода к заданию, если TestPool - блокирующий (При добавлении подобного функционала в БД)
            return testIndex != null;
        }

        private VariantExecutionModelBase CreateTaskExecutionModel(Uri taskCompleteRedirect, Task task, DomainModel.LabVariant variant, LabWork lab, Result result)
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

        private IEnumerable<BaseListEntryModel> GetOtherTasksModels(LabWork lab, Result result, Task task, DomainModel.TestPoolEntry testPoolEntry)
        {
            var otherTasks = result.AbstractResultEntries.Where(e => e is TestResult).Select(e =>
            {
                var testResult = (TestResult) e;
                var entry = testResult.TestPoolEntry;
                var model = _testModelLoader.Load(result, entry);
                if (testResult.TestPoolEntry.Id == testPoolEntry?.Id)
                {
                    model.State = TaskExecutionState.CurrentlySolving;
                }
                return (BaseListEntryModel) model;
            });
            var addTasks = lab.LabEntries.Select(e =>
            {
                var model = _taskModelLoader.Load(result, e);
                if (e.Task.Id == task?.Id)
                {
                    model.State = TaskExecutionState.CurrentlySolving;
                }
                return (BaseListEntryModel) model;
            }).ToArray();
            return otherTasks.Union(addTasks);
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
                OtherTasks = GetOtherTasksModels(lab, result, currentTask, null).ToArray()
            };
        }


        #region Завершение лабы

        private VariantExecutionModelBase CompleteVariant(Result result)
        {
            Guard.IsTrueAssertion(result.AbstractResultEntries.All(r => r.Status == ExecutionStatus.Complete));

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
            var test = GetTestResultScore(taskResults) ?? 0;
            sum = sum + test;
            return sum / (scores.Length + 1);
        }

        private int?[] GetTaskResultsScore(ICollection<AbstractResultEntry> taskResults)
        {
            var taskResultsArray = taskResults.OfType<TaskResult>().ToArray();
            var result = new int?[taskResultsArray.Length];
            for (var i = 0; i < taskResultsArray.Length; i++)
            {
                result[i] = taskResultsArray[i].Score;
            }
            return result;
        }

        private int? GetTestResultScore(ICollection<AbstractResultEntry> testResults)
        {
            var testResult = testResults.OfType<TestResult>().ToArray();
            if (testResult.Length == 0) return null;
            var realSum = 0;
            var maxSum = 0;
            for (var i = 0; i < testResult.Length; i++)
            {
                realSum = realSum + testResult[i].Score;
                maxSum = maxSum + testResult[i].TestPoolEntry.Score;
            }
            var result = (int) 100*realSum/maxSum;
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

        private DomainModel.TestPoolEntry GetFirstUnsolvedTest(Result result)
        {
            var searchResult = result
                .AbstractResultEntries
                .Where(r => r.Status != ExecutionStatus.Complete)
                .OfType<TestResult>()
                .OrderBy(r => r.Index)
                .Select(r => r.TestPoolEntry)
                .FirstOrDefault();

            return searchResult;
        }

        private DomainModel.TestPoolEntry TryGetTestPoolEntry(Result result, DomainModel.LabVariant lab, int? testIndex)
        {
            if (!testIndex.HasValue)
                return null;

            var testEntry = lab.TestPool.TestPoolEntries.SingleOrDefault(e => e.Id == testIndex.Value);
            if (testEntry == null)
                return null;

            var testResult = result
                .AbstractResultEntries
                .OfType<TestResult>()
                .SingleOrDefault(tr => tr.TestPoolEntry.Id == testEntry.Id
                                       && tr.Status == ExecutionStatus.Complete);
            if (testResult?.Status == ExecutionStatus.Complete)
                return null;

            return testEntry;
        }

        private Task TryGetTaskByIndex(Result labResult, LabWork lab, int? index)
        {
            if (!index.HasValue)
                return null;

            var taskEntry = lab.LabEntries.FirstOrDefault(e => e.Task.Id == index);
            if (taskEntry == null)
                return null;

            var taskResult = labResult
                .AbstractResultEntries
                .OfType<TaskResult>()
                .SingleOrDefault(tr => tr.TaskVariant.Task.Id == taskEntry.Id && 
                                       tr.Status == ExecutionStatus.Complete);

            if (taskResult?.Status == ExecutionStatus.Complete)
                return null;

            return taskEntry.Task;
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

        private Result[] FindResultsToInterrupt(Student student)
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