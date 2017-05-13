using System;
using System.Linq;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Models.LabExecution
{
    /// <summary> Фабрика моделей заданий в лабе </summary>
    internal sealed class TaskExecutionModelLoader
    {
        public TaskListEntryModel Load(Result currentResult, LabEntry entryToLoad)
        {
            var resultForEntry = currentResult
                .AbstractResultEntries
                .OfType<TaskResult>()
                .SingleOrDefault(result => result.TaskVariant.Task.Id == entryToLoad.Task.Id);

            var taskState = resultForEntry?.Status == ExecutionStatus.Complete
                ? TaskListEntryModel.TaskExecutionState.Solved
                : TaskListEntryModel.TaskExecutionState.New;

            return new TaskListEntryModel()
            {
                TaskId = entryToLoad.Task.Id,
                TaskName = entryToLoad.Task.Name,
                State = taskState
            };
        }
    }
}