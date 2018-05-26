using System;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Tasks.Contract;

namespace GraphLabs.Site.Models.LabExecution.Operations
{
    internal class LoadTestingVariantForExecution : LoadVariantForExecutionBase
    {
        public LoadTestingVariantForExecution(
            IOperationContextFactory<IGraphLabsContext> operationFactory,
            IAuthenticationSavingService authService,
            IInitParamsProvider initParamsProvider,
            TaskExecutionModelLoader taskModelLoader,
            TestExecutionModelLoader testExecutionModelLoader)
            : base(operationFactory, authService, initParamsProvider, taskModelLoader, testExecutionModelLoader)
        {
        }

        public VariantExecutionModelBase Load(int? taskIndex, Uri taskCompleteRedirect)
        {
            throw new NotImplementedException();
        }
    }
}