using System;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Tasks.Contract;
using GraphLabs.Site.Core;

namespace GraphLabs.Site.Models.Preview
{
    internal sealed class TaskVariantPreviewModelLoader : ITaskVariantPreviewModelLoader
    {
        private readonly IAuthenticationSavingService _authService;
        private readonly IInitParamsProvider _initParamsProvider;
        private readonly IEntityQuery _entityQuery;

        public TaskVariantPreviewModelLoader(
            IAuthenticationSavingService authService,
            IInitParamsProvider initParamsProvider,
            IEntityQuery entityQuery)
        {
            _authService = authService;
            _initParamsProvider = initParamsProvider;
            _entityQuery = entityQuery;
        }

        public TaskVariantPreviewModel Load(int taskId, int labWorkId)
        {
            var model = CreateTaskVariantPreviewModel(taskId, labWorkId);
            return model;
        }

        private TaskVariantPreviewModel CreateTaskVariantPreviewModel(int taskId, int labWorkId)
        {
            long variantId;
            try
            {
                variantId = _entityQuery.OfEntities<TaskVariant>().First(t => t.Task.Id == taskId).Id;
            }
            catch (InvalidOperationException ex)
            {
                throw new GraphLabsException(ex,
                    "Не удалось загрузить вариант для предпросмотра. Вероятно, для выбранного модуля в системе нет ни одного доступного варианта.");
            }

            var initParams = InitParams.ForDemoMode(
                _authService.GetSessionInfo().SessionGuid,
                taskId,
                variantId,
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

