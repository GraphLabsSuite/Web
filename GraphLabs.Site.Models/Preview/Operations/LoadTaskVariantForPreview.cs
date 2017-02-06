using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Tasks.Contract;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Models.LabExecution;

namespace GraphLabs.Site.Models.Preview.Operations
{
    class LoadTaskVariantForPreview : AbstractOperation
    {
        private readonly IAuthenticationSavingService _authService;
        private readonly IInitParamsProvider _initParamsProvider;
        private readonly TaskExecutionModelLoader _taskModelLoader;

        public LoadTaskVariantForPreview(
            IOperationContextFactory<IGraphLabsContext> operationFactory,
            IAuthenticationSavingService authService,
            IInitParamsProvider initParamsProvider,
            TaskExecutionModelLoader taskModelLoader)
            : base(operationFactory)
        {
            _authService = authService;
            _initParamsProvider = initParamsProvider;
            _taskModelLoader = taskModelLoader;
        }

        public TaskVariantPreviewModel Load(int taskId, int labWorkId)
        {

            var labWork = Query.Get<LabWork>(labWorkId);

            var model = CreateTaskVariantPreviewModel(taskId, 0, labWorkId);

            return model;
        }

        private TaskVariantPreviewModel CreateTaskVariantPreviewModel(int taskId, int labVariantId, int labWorkId)
        {
            var initParams = InitParams.ForDemoMode(
                _authService.GetSessionInfo().SessionGuid,
                taskId,
                53,
                labWorkId,
                null);

            var model = new TaskVariantPreviewModel
            {
                TaskId = taskId,
                InitParams = _initParamsProvider.GetInitParamsString(initParams)
            };

            return model;
        }
    }
}
