using System.Diagnostics.Contracts;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.TaskResults;
using GraphLabs.Site.Models.StudentActions;
using GraphLabs.Site.Models.TaskResultsWithActions;
using System.Collections.Generic;
using GraphLabs.Site.Models.Groups;
using GraphLabs.DomainModel.Extensions;

namespace GraphLabs.Site.Models.ResultsWithTaskInfo
{
    class ResultWithTaskInfoModelLoader : AbstractModelLoader<ResultWithTaskInfoModel, Result>
    {
        private readonly IEntityBasedModelLoader<TaskResultModel, TaskResult> _modelLoader;
        private readonly IEntityBasedModelLoader<StudentActionModel, StudentAction> _modelLoader_StudentAction;
        private readonly IEntityBasedModelLoader<GroupModel, Group> _modelLoader_Group;
        public ResultWithTaskInfoModelLoader(IEntityQuery query,
            IEntityBasedModelLoader<TaskResultModel, TaskResult> modelLoader,
            IEntityBasedModelLoader<StudentActionModel, StudentAction> modelLoader_StudentAction,
            IEntityBasedModelLoader<GroupModel, Group> modelLoader_Group) : base(query)
        {
            _modelLoader = modelLoader;
            _modelLoader_StudentAction =  modelLoader_StudentAction;
            _modelLoader_Group = modelLoader_Group;
        }

        public override ResultWithTaskInfoModel Load(Result result)
        {
            Guard.IsNotNull(nameof(result), result);
            var name = result.Student.GetShortName();
            var model = new ResultWithTaskInfoModel()
            {
                Id = result.Id,
                LabVariantNumber = result.LabVariant.Number,
                Mode = LabExecutionModeToString(result.Mode),
                Score = result.Score,
                Status = ExecutionStatusToString(result.Status),
                StartDateTime = result.StartDateTime,
                LabWorkName = result.LabVariant.LabWork.Name,
                StudentId = result.Student.Id,
                StudentActions = result.AbstractResultEntries.OfType<TaskResult>().SelectMany(r => r.StudentActions).Select(x => _modelLoader_StudentAction.Load(x)).ToArray(),
                StudentName = result.Student.GetShortName()
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
