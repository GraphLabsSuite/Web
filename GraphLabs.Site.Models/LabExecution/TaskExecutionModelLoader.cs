using System;
using System.Linq;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Models.LabExecution
{
    /// <summary> Фабрика моделей заданий в лабе </summary>
    internal sealed class TaskExecutionModelLoader
    {
        public TaskExecutionModel Load(Result currentResult, LabEntry entryToLoad)
        {
            var resultForEntry = currentResult
                .TaskResults
                .SingleOrDefault(result => result.TaskVariant.Task.Id == entryToLoad.Task.Id);

            var taskState = resultForEntry?.Status == ExecutionStatus.Complete
                ? TaskExecutionState.Solved
                : TaskExecutionState.New;

            return new TaskExecutionModel()
            {
                TaskId = entryToLoad.Task.Id,
                TaskName = entryToLoad.Task.Name,
                State = taskState
            };
        }
    }
}