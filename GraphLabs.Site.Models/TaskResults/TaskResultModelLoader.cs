using System.Diagnostics.Contracts;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.StudentActions;

namespace GraphLabs.Site.Models.TaskResults
{
    class TaskResultModelLoader : AbstractModelLoader<TaskResultModel, TaskResult>
    {
        public TaskResultModelLoader(IEntityQuery query) : base(query) { }

        public override TaskResultModel Load(TaskResult taskResult)
        {
            Contract.Requires(taskResult != null);

            var model = new TaskResultModel()
            {
                Id = taskResult.Id,
                TaskName = taskResult.TaskVariant.Task.Name,
                Status = ExecutionStatusToString(taskResult.Status),
                TaskVariantNumber = taskResult.TaskVariant.Number
            };

            return model;
        }

        private string ExecutionStatusToString(ExecutionStatus status)
        {
            switch (status)
            {
                case ExecutionStatus.Complete:
                    return "Закончена";
                case ExecutionStatus.Executing:
                    return "Выполняется";
                default:
                    return "Не определён";
            }
        }
    }
}

