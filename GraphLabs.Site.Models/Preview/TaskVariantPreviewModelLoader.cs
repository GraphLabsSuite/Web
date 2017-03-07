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
using GraphLabs.Site.Models.Preview.Operations;
using GraphLabs.Site.Models.LabExecution;

namespace GraphLabs.Site.Models.Preview
{
    class TaskVariantPreviewModelLoader : ITaskVariantPreviewModelLoader
    {
        private readonly IAuthenticationSavingService _authService;
        private readonly IInitParamsProvider _initParamsProvider;
        private readonly IOperationContextFactory<IGraphLabsContext> _operationFactory;
        private readonly TaskExecutionModelLoader _taskModelLoader;

        public TaskVariantPreviewModelLoader(
            IAuthenticationSavingService authService,
            IInitParamsProvider initParamsProvider,
            IOperationContextFactory<IGraphLabsContext> operationFactory,
            TaskExecutionModelLoader taskModelLoader)
        {
            _authService = authService;
            _initParamsProvider = initParamsProvider;
            _operationFactory = operationFactory;
            _taskModelLoader = taskModelLoader;
        }

        public TaskVariantPreviewModel Load(int taskId, int labWorkId)
        {
            using (var operation = new LoadTaskVariantForPreview(_operationFactory, _authService, _initParamsProvider, _taskModelLoader))
            {
                return operation.Load(taskId, labWorkId);
            }
        }
    }
}
