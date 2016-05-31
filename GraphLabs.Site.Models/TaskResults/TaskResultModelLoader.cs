using System.Diagnostics.Contracts;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.StudentActions;

namespace GraphLabs.Site.Models.TaskResults
{
    class TaskResultModelLoader : AbstractModelLoader<TaskResultModel, TaskResult>
    {
        private readonly IEntityBasedModelLoader<StudentActionModel, StudentAction> _modelLoader;

        public TaskResultModelLoader(IEntityQuery query, IEntityBasedModelLoader<StudentActionModel, StudentAction> modelLoader) : base(query)
        {
            _modelLoader = modelLoader;
        }

        public override TaskResultModel Load(TaskResult taskResult)
        {
            Contract.Requires(taskResult != null);

            var model = new TaskResultModel()
            {
                Id = taskResult.Id,
                Status = taskResult.Status,
                StudentActions = taskResult.StudentActions.Select(x => _modelLoader.Load(x)),
                TaskVariantNumber = taskResult.TaskVariant.Number
            };

            return model;
        }
    }
}
