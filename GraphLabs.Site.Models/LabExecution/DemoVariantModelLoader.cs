using System;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Models.LabExecution.Operations;
using GraphLabs.Tasks.Contract;

namespace GraphLabs.Site.Models.LabExecution
{
    internal class DemoVariantModelLoader : IDemoVariantModelLoader
    {
        private readonly IAuthenticationSavingService _authService;
        private readonly IInitParamsProvider _initParamsProvider;
        private readonly IOperationContextFactory<IGraphLabsContext> _operationFactory;
        private readonly TaskExecutionModelLoader _taskModelLoader;

        public DemoVariantModelLoader(
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

        public VariantExecutionModelBase Load(long labVariantId, int? taskIndex, Uri taskCompleteRedirect)
        {
            using (var operation = new LoadDemoVariantForExecution(_operationFactory, _authService, _initParamsProvider, _taskModelLoader))
            {
                return operation.Load(labVariantId, taskIndex, taskCompleteRedirect);
            }
        }
    }
}