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
        private readonly IEntityBasedModelLoader<TaskResultModel, TaskResult> _modelLoader;

        public ResultModelLoader(IEntityQuery query, IEntityBasedModelLoader<TaskResultModel, TaskResult> modelLoader)
            : base(query)
        {
            _modelLoader = modelLoader;
        }

        public override ResultModel Load(Result result)
        {
            Contract.Requires(result != null);

            var model = new ResultModel()
            {
                Id = result.Id,
                LabVariantNumber = result.LabVariant.Number,
                Mode = result.Mode,
                Score = result.Score,
                StartDateTime = result.StartDateTime,
                Student = result.Student,
                TaskResults = result.TaskResults.Select(x => _modelLoader.Load(x))
            };

            return model;
        }
    }
}
