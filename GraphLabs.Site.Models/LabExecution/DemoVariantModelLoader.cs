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
        private readonly TestExecutionModelLoader _testExecutionModelLoader;

        public DemoVariantModelLoader(
            IAuthenticationSavingService authService,
            IInitParamsProvider initParamsProvider,
            IOperationContextFactory<IGraphLabsContext> operationFactory,
            TaskExecutionModelLoader taskModelLoader,
            TestExecutionModelLoader testExecutionModelLoader)
        {
            _authService = authService;
            _initParamsProvider = initParamsProvider;
            _operationFactory = operationFactory;
            _taskModelLoader = taskModelLoader;
            _testExecutionModelLoader = testExecutionModelLoader;
        }

        public VariantExecutionModelBase Load(long labVariantId, int? taskIndex, int? testIndex, Uri taskCompleteRedirect)
        {
            using (var operation = new LoadDemoVariantForExecution(_operationFactory, _authService, _initParamsProvider, _taskModelLoader, _testExecutionModelLoader))
            {
                return operation.Load(labVariantId, taskIndex, testIndex, taskCompleteRedirect);
            }
        }
    }
}