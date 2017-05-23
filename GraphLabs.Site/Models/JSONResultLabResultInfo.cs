using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraphLabs.Dal.Ef;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Models
{
    public class TaskInfo
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Variant { get; set; }

        public int Result { get; set; }

        public TaskInfo(TaskResult[] tasks, int number)
        {
            Id = tasks[number].Id;
            Name = tasks[number].TaskVariant.Task.Name;
            Variant = "Вариант " + tasks[number].TaskVariant.Number;
            int? score = tasks[number].Score;
            if (score != null) Result = (int) score;
            else Result = -1;
        }
    }

    public class TestInfo : JsonResult
    {
        public string TestPoolName { get; set; }

        public TestResultInfo[] TestEntries { get; set; }

        public TestInfo(GraphLabsContext ctx, int resultId)
        {
            var result = ctx.Results.FirstOrDefault(e => e.Id == resultId);
            if (result == null) throw new Exception("Результат не найден!");
            var testResults = result
                .AbstractResultEntries
                .OfType<TestResult>()
                .ToArray();
            if (!testResults.Any()) throw new Exception("В тестпуле не оказалось заданий!");
            var testPool = testResults.First().TestPoolEntry.TestPool;

            TestPoolName = testPool.Name;
            TestEntries = new TestResultInfo[testResults.Length];
            for (int i = 0; i < testResults.Length; i++)
            {
                TestEntries.SetValue(new TestResultInfo
                {
                    QuestionName = testResults.ElementAt(i).TestPoolEntry.TestQuestion.Question,
                    Score = testResults.ElementAt(i).TestPoolEntry.Score,
                    Mark = testResults.ElementAt(i).Score
                }, i);
            }
        }
    }

    public class TestResultInfo
    {
        public string QuestionName { get; set; }

        public int Score { get; set; }

        public int Mark { get; set; }
    }

    public class JSONResultLabResultInfo
    {
        public long Result { get; set; }

        public string LabName { get; set; }

        public TaskInfo[] Tasks { get; set; }

        public string[] Problems { get; set; }

        public long StudentsNumber { get; set; }

        public int Place { get; set; }

        public JSONResultLabResultInfo(GraphLabsContext ctx, int id, long studentId)
        {
            //var labVar = ctx.LabVariants.Single(labvar => labvar.Id == id);
            var result = ctx.Results.Single(res => res.Id == id);
            var labVar = result.LabVariant.Id;
            var labName = result.LabVariant.LabWork.Name;
            var resultStudent = ctx.Results.Where(tr => tr.Student.Id == studentId).ToArray();
            var groupId = resultStudent[0].Student.Group.Id;
            Result = result.Id;
            LabName = labName;
            StudentsNumber = ctx.Groups.Count(tr => tr.Id == groupId);
            Place = GetPlace(ctx, labVar, studentId);
            Tasks = GetTaskInfo(ctx, id, studentId);
            Problems = GetProblems(ctx, id);
        }

    private TaskInfo[] GetTaskInfo(GraphLabsContext ctx, int id, long studentId)
    {
        var tasks = ctx.AbstractResultEntries.OfType<TaskResult>().Where(task=> task.Result.Student.Id == studentId && task.Result.Id == id).ToArray();
        var result = new TaskInfo[tasks.Length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = new TaskInfo(tasks, i);
        }
        return result;
    }

    private string[] GetProblems(GraphLabsContext ctx, int id)
    {
        string[] result;
        var problems =
            ctx.AbstractStudentActions.OfType<StudentAction>().Where(tr => tr.TaskResult.Result.Id == id && tr.Penalty != 0)
                .ToArray();
        if (problems.Length == 0)
        {
            result = new string[1];
            result[0] = "У вас нет проблемных зон.";
        }
        else
        {
            result = new string[0];
            for (int i = 0; i < problems.Length; i++)
            {
                bool flag = true;
                for (int j = 0; j < problems.Length && j != i; j++)
                {
                    if (problems[i].Description == problems[j].Description)
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    result = InsertDescription(problems[i].Description, result);
                }
            }
        }

        return result;
    }

        private string[] InsertDescription(string Description, string[] array)
        {
            var newLength = array.Length + 1;
            var result = new string[newLength];
            for (var i = 0; i < result.Length; i++)
            {
                if (i < array.Length)
                {
                    result[i] = array[i];
                }
                else
                {
                    result[i] = Description;
                }
            }
            return result;
        }

    private int GetPlace(GraphLabsContext ctx, long id, long studentId)
    {
        var students = ctx.Results.Where(tr => tr.LabVariant.Id == id).OrderBy(td => td.Score).ToArray();
        var place = 1;
        for (int i = 0; i < students.Length; i++)
        {
            if (students[i].Student.Id != studentId) place++;
            else return place;
        }
        return place;
    }
}

    public class JSONTaskResultInfo
    {
        public int Result { get; set; }

        public int Id { get; set; }

        public string TaskName { get; set; }

        public StudentActionInfo[] Actions { get; set; }

        public JSONTaskResultInfo(GraphLabsContext ctx, int id)
        {
            var taskResult = ctx.AbstractResultEntries.OfType<TaskResult>().Single(task => task.Id == id);
            Id = id;
            TaskName = taskResult.TaskVariant.Task.Name;
            Actions = GetStudentActionsInfo(ctx, id);
            Result = 0;
        }

        private StudentActionInfo[] GetStudentActionsInfo(GraphLabsContext ctx, int id)
        {
            var actions = ctx.AbstractStudentActions.OfType<StudentAction>().Where(action => action.TaskResult.Id == id).ToArray();
            var studentActions = new StudentActionInfo[actions.Length];
            for (int i = 0; i < actions.Length; i++)
            {
                studentActions[i] = new StudentActionInfo(actions[i]);
            }
            return studentActions;
        }
    }

    public class StudentActionInfo
    {
        public long Id;

        public string Description;

        public int Penalty;

        public DateTime Time;

        public StudentActionInfo(StudentAction action)
        {
            Id = action.Id;
            Description = action.Description;
            Penalty = action.Penalty;
            Time = action.Time;
        }
    }
}