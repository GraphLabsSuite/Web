using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

    public class JSONResultLabResultInfo
    {
        public int Result { get; set; }

        public string LabName { get; set; }

        public TaskInfo[] Tasks { get; set; }

        public string[] Problems { get; set; }

        public long StudentsNumber { get; set; }

        public int Place { get; set; }

        public JSONResultLabResultInfo(GraphLabsContext ctx, int id, long studentId)
        {
            var labName = ctx.LabVariants.Single(labvar => labvar.Id == id).LabWork.Name;
            var resultStudent = ctx.Results.Where(tr => tr.Student.Id == studentId).ToArray();
            var groupId = resultStudent[0].Student.Group.Id;
            Result = 0;
            LabName = labName;
            StudentsNumber = ctx.Groups.Count(tr => tr.Id == groupId);
            Place = GetPlace(ctx, id, studentId);
            Tasks = GetTaskInfo(ctx, id, studentId);
            Problems = GetProblems(ctx, id, studentId);
        }

    private TaskInfo[] GetTaskInfo(GraphLabsContext ctx, int lab, long studentId)
    {
        var tasks = ctx.TaskResults.Where(task => task.Result.Student.Id == studentId && task.Result.LabVariant.Id == lab).ToArray();
        var result = new TaskInfo[tasks.Length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = new TaskInfo(tasks, i);
        }
        return result;
    }

    private string[] GetProblems(GraphLabsContext ctx, int id, long studentId)
    {
        string[] result;
        var problems =
            ctx.StudentActions.Where(tr => tr.TaskResult.Result.LabVariant.Id == id && tr.TaskResult.Result.Student.Id == studentId && tr.Penalty != 0)
                .ToArray();
        if (problems.Length == 0)
        {
            result = new string[1];
            result[0] = "У вас нет проблемных зон.";
        }
        else
        {
            result = new string[problems.Length];
            for (int i = 0; i < problems.Length; i++)
            {
                result[i] = problems[i].Description;
            }
        }
        return result;
    }

    private int GetPlace(GraphLabsContext ctx, int id, long studentId)
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
}