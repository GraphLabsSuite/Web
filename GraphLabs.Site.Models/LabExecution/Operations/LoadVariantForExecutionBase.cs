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

namespace GraphLabs.Site.Models.LabExecution.Operations
{
    internal abstract class LoadVariantForExecutionBase : AbstractOperation
    {
        private readonly IAuthenticationSavingService _authService;
        private readonly IInitParamsProvider _initParamsProvider;
        private readonly TaskExecutionModelLoader _taskModelLoader;
        private readonly TestExecutionModelLoader _testModelLoader;

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
        }

        protected VariantExecutionModelBase LoadImpl(LabVariant variant, int? taskIndex, int? testIndex, Uri taskCompleteRedirect)
        {
            
            var lab = variant.LabWork;

            var student = GetCurrentStudent();
            var resultsToInterrupt = FindResultsToInterrupt(student);
            var latestCurrentResult = FindLatestCurrentResult(resultsToInterrupt, lab);

            // ���� ����, �� ������ ������ ������ ����������, ��������� ������.
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
                // ���� �� �����, �� ������� �����
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
                var randomArray = new Randomizer[variant.TestPool.TestPoolEntries.Count];
                randomArray = Randomizer.InitializeArray(randomArray);
                var randomer = new Random(variant.TestPool.TestPoolEntries.Count);
                foreach (var testQuestion in variant.TestPool.TestPoolEntries)
                {
                    var testResult = Factory.Create<TestResult>();
                    testResult.TestPoolEntry = testQuestion;
                    var number = Randomizer.GetNewValue(randomArray, randomer);
                    testResult.Index = number.ToString();
                    randomArray = Randomizer.ChoseNumber(randomArray, number);
                    testResult.StudentAnswers = new List<StudentAnswer>();
                    result.AbstractResultEntries.Add(testResult);
                }
            }
            else
            {
                result = latestCurrentResult;
            }

            var task = taskIndex.HasValue
                ? GetTaskByIndex(lab, taskIndex.Value)
                : GetFirstUnsolvedTask(result);

            DomainModel.TestPoolEntry test = GetTestPoolEntry(variant, testIndex);

            var model = task == null 
                ? testIndex == null ? CompleteVariant(result) : CreateTestExecutionModel(taskCompleteRedirect, test, variant, lab, result)
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
                    array[i].Number = i;
                    array[i].Chosen = false;
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
                var next = randomer.Next();
                while (CheckChosen(array, next))
                {
                    next = randomer.Next();
                }
                return next;
            }
        }

        private VariantExecutionModelBase CreateTestExecutionModel(Uri taskCompleteRedirect, DomainModel.TestPoolEntry test, LabVariant variant, LabWork lab, Result result)
        {
            var model = new TestExecutionModel();
            model.Name = test.TestPool.Name;
            model.Question = test.TestQuestion.Question;
            model.Answers = test.TestQuestion.AnswerVariants;
            model.QuestionId = test.Id;
            model.OtherTasks = GetOtherTasksModels(lab, result, null, test).ToArray();
            model.VariantId = variant.Id;
            model.LabName = lab.Name;
            return model;
        }

        private bool IsTestPoolCompleted(LabVariant variant, int? testIndex)
        {
            // TODO: �������� ����������� ���������� �������� � �������, ���� TestPool - ����������� (��� ���������� ��������� ����������� � ��)
            return testIndex != null;
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

        private IEnumerable<BaseListEntryModel> GetOtherTasksModels(LabWork lab, Result result, Task task, DomainModel.TestPoolEntry testPoolEntry)
        {
            var otherTasks = result.AbstractResultEntries.Select(e =>
            {
                var testResult = e as TestResult;
                var entry = testResult.TestPoolEntry;
                var model = _testModelLoader.Load(result, entry);
                if (testResult.TestPoolEntry.Id == testPoolEntry?.Id)
                {
                    model.State = TaskExecutionState.CurrentlySolving;
                }
                return model as BaseListEntryModel;
            });
            var addTasks = lab.LabEntries.Select(e =>
            {
                var model = _taskModelLoader.Load(result, e);
                if (e.Task.Id == task?.Id)
                {
                    model.State = TaskExecutionState.CurrentlySolving;
                }
                return model as BaseListEntryModel;
            });
            return otherTasks.Concat(addTasks);
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


        #region ���������� ����

        private VariantExecutionModelBase CompleteVariant(Result result)
        {
            Contract.Assert(result.AbstractResultEntries.All(r => r.Status == ExecutionStatus.Complete));

            var mark = GetMark(result.AbstractResultEntries);
            result.Score = mark;
            result.Status = ExecutionStatus.Complete;

            var model = CreateModelHeader<VariantExecutionCompleteModel>(result, null);
            model.ResultMessage = $"������ ���������! ��� ����: {mark}.";

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

        private DomainModel.TestPoolEntry GetTestPoolEntry(LabVariant lab, int? testIndex)
        {
            if (testIndex == null) return null;
            return lab.TestPool.TestPoolEntries.SingleOrDefault(e => e.Id == testIndex.Value);
        }

        private Task GetTaskByIndex(LabWork lab, int index)
        {
            var result = lab.LabEntries
                .OrderBy(e => e.Order)
                .Skip(index)
                .FirstOrDefault();

            if (result == null)
                throw new Exception("������� � ����������� ������� �� ����������.");

            return result.Task;
        }

        private Student GetCurrentStudent()
        {
            var session = _authService.GetSessionInfo();

            var ip = HttpContext.Current.Request.UserHostAddress;
            var matchingSessions = Query.OfEntities<Session>()
                .Where(s => s.Guid == session.SessionGuid && s.IP == ip) //todo ����-�� ��������
                .ToArray();

            if (matchingSessions.Count() == 1)
            {
                var student = matchingSessions.Single().User as Student;
                if (student != null)
                    return student;
            }

            throw new Exception("������ �������� �� �������.");
        }

        private Result[] FindResultsToInterrupt(Student student)
        {
            //����� ������������ ���������� ����������
            return student.Results
                .Where(result => result.Status == ExecutionStatus.Executing)
                // .Include(i => i.)
                .ToArray();
        }

        private Result FindLatestCurrentResult(IEnumerable<Result> resultsToInterrupt, LabWork lab)
        {
            // ����� ����������, ����������� � ��, ������� �������� ������ ���������
            var currentResults = resultsToInterrupt
                .Where(res => res.LabVariant.LabWork == lab)
                .OrderByDescending(res => res.StartDateTime);

            // ���������, ���� �� ������ �����. ���� ����, ���� ����� ������ (������������, ��� ������ 1 � �� ������ ����).
            return currentResults.FirstOrDefault();
        }
    }
}