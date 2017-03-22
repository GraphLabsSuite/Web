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
using GraphLabs.Site.Models.LabExecution;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Core;

namespace GraphLabs.Site.Models.Preview
{
    internal sealed class TaskVariantPreviewModelLoader : ITaskVariantPreviewModelLoader
    {
        private readonly IAuthenticationSavingService _authService;
        private readonly IInitParamsProvider _initParamsProvider;
        private readonly IEntityQuery _entityQuery;
        private readonly TaskExecutionModelLoader _taskModelLoader;

        public TaskVariantPreviewModelLoader(
            IAuthenticationSavingService authService,
            IInitParamsProvider initParamsProvider,
            IEntityQuery entityQuery,
            TaskExecutionModelLoader taskModelLoader)
        {
            _authService = authService;
            _initParamsProvider = initParamsProvider;
            _entityQuery = entityQuery;
            _taskModelLoader = taskModelLoader;
        }

        public TaskVariantPreviewModel Load(int taskId, int labWorkId)
        {
            var labWork = _entityQuery.Find<LabWork>(labWorkId);

            var model = CreateTaskVariantPreviewModel(taskId, labWorkId);

            return model;
        }
        private TaskVariantPreviewModel CreateTaskVariantPreviewModel(int taskId,  int labWorkId)
        {
            try
            {
                var variantId = _entityQuery.OfEntities<TaskVariant>().First(t => t.Task.Id == taskId).Id;
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
            catch (InvalidOperationException ex)
            { throw new GraphLabsException(ex, "Не удалось загрузить вариант для предпросмотра. Вероятно, для выбранного модуля в системе нет ни одного доступного варианта.");
            }

        }
    }
        
    }

