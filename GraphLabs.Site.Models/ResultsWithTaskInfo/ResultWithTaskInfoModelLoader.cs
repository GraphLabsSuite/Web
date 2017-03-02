using System.Diagnostics.Contracts;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.TaskResults;

namespace GraphLabs.Site.Models.ResultsWithTaskInfo
{
    class ResultWithTaskInfoModelLoader : AbstractModelLoader<ResultWithTaskInfoModel, Result>
    {
        private readonly IEntityBasedModelLoader<TaskResultModel, TaskResult> _modelLoader;

        public ResultWithTaskInfoModelLoader(IEntityQuery query,
            IEntityBasedModelLoader<TaskResultModel, TaskResult> modelLoader) : base(query)
        {
            _modelLoader = modelLoader;
        }

        public override ResultWithTaskInfoModel Load(Result result)
        {
            Contract.Requires(result != null);

            var model = new ResultWithTaskInfoModel()
            {
                Id = result.Id,
                LabVariantNumber = result.LabVariant.Number,
                Mode = LabExecutionModeToString(result.Mode),
                Score = result.Score,
                Status = ExecutionStatusToString(result.Status),
                StartDateTime = result.StartDateTime,
                LabWorkName = result.LabVariant.LabWork.Name,
                TaskResults = result.AbstractResultEntries.OfType<TaskResult>().Select(x => _modelLoader.Load(x)).ToArray(),
                StudentId = result.Student.Id
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

        private string LabExecutionModeToString(LabExecutionMode mode)
        {
            switch (mode)
            {
                case LabExecutionMode.IntroductoryMode:
                    return "Ознакомительный";
                case LabExecutionMode.TestMode:
                    return "Тестовый";
                default:
                    return "Не определён";
            }
        }
    }
}
