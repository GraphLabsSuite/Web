using System.Diagnostics.Contracts;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.LabExecution;
using GraphLabs.Site.Models.StudentActions;


namespace GraphLabs.Site.Models.TaskResultsWithActions
{
    class TaskResultWithActionsModelLoader : AbstractModelLoader<TaskResultWithActionsModel, TaskResult>
    {
        private readonly IEntityBasedModelLoader<StudentActionModel, StudentAction> _modelLoader;

        public TaskResultWithActionsModelLoader(IEntityQuery query,
            IEntityBasedModelLoader<StudentActionModel, StudentAction> modelLoader) : base(query)
        {
            _modelLoader = modelLoader;
        }

        public override TaskResultWithActionsModel Load(TaskResult taskResult)
        {
            Contract.Requires(taskResult != null);

            var model = new TaskResultWithActionsModel()
            {
                Id = taskResult.Id,
                TaskName = taskResult.TaskVariant.Task.Name,
                Status = ExecutionStatusToString(taskResult.Status),
                TaskVariantNumber = taskResult.TaskVariant.Number,
                StudentActions = taskResult.StudentActions.Select(x => _modelLoader.Load(x)).ToArray(),
                Result = taskResult.Result
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

