using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.TaskResults;

namespace GraphLabs.Site.Models.Results
{
    class ResultModelLoader : AbstractModelLoader<ResultModel, Result>
    {
        public ResultModelLoader(IEntityQuery query): base(query) { }

        public override ResultModel Load(Result result)
        {
            Contract.Requires(result != null);

            var model = new ResultModel()
            {
                Id = result.Id,
                LabVariantNumber = result.LabVariant.Number,
                Mode = LabExecutionModeToString(result.Mode),
                Score = result.Score,
                Status = ExecutionStatusToString(result.Status),
                StartDateTime = result.StartDateTime,
                LabWorkName = result.LabVariant.LabWork.Name
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
