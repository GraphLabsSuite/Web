using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using GraphLabs.DomainModel;
using GraphLabs.Dal.Ef;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Logic;
using GraphLabs.WcfServices.Data;

namespace GraphLabs.WcfServices
{
    /// <summary> Сервис для генераторов вариантов </summary>
    public class VariantGenService : IVariantGenService
    {
        private readonly IOperationContextFactory<IGraphLabsContext> _operationFactory;
        private const int INITIAL_VERSION = 1;

        /// <summary> Сервис для генераторов вариантов </summary>
        public VariantGenService(IOperationContextFactory<IGraphLabsContext> operationFactory)
        {
            _operationFactory = operationFactory;
        }

        /// <summary> Получает вариант задания по Id </summary>
        /// <param name="id"> Id варианта</param>
        public TaskVariantDto GetVariant(long id)
        {
            using (var op = _operationFactory.Create())
            {
                var variant = op.DataContext.Query.Find<TaskVariant>(id);
                if (variant == null)
                    throw new ArgumentException("Вариант с указанным Id не найден.");

                TaskVariantDto taskVariantDto = new TaskVariantDto
                {
                    Data = variant.Data,
                    GeneratorVersion = variant.GeneratorVersion,
                    Number = variant.Number,
                    Version = variant.Version,
                    Id = variant.Id
                };

                Guard.IsNotNull(taskVariantDto);
                return taskVariantDto;
            }
        }

        /// <summary> Регистрирует завершение выполнения задания </summary>
        /// <param name="info"> Новый вариант </param>
        /// <param name="taskId"> Id задания, для которого вариант. Слегка избыточен, но пусть будет. </param>
        /// <param name="updateExistent"> Обновить существующую версию? </param>
        /// <returns> True, если успешно сохранили. False, если вариант с таким номером уже есть в этом задании. </returns>
        public void SaveVariant(TaskVariantDto info, long taskId, bool updateExistent = false)
        {
            if (info == null)
                throw new ArgumentException("info");
            if (info.Version.HasValue)
                throw new ArgumentException("info.Version");

            using (var op = _operationFactory.Create())
            {
                var task = op.DataContext.Query.Find<Task>(taskId);
                if (task == null)
                    throw new Exception("Не удалось сохранить вариант: не найдено задание с полученным Id.");
                if (updateExistent)
                {
                    var existVariant = op.DataContext.Query.Find<TaskVariant>(info.Id);

                    if (existVariant == null)
                        throw new Exception("Не удалось сохранить вариант: не найден вариант с полученным Id.");
                    if (existVariant.Number != info.Number)
                        throw new Exception("Нельзя изменить номер варианта на обновлении.");
                    if (existVariant.Task.Id != taskId)
                        throw new Exception("Не удалось сохранить вариант: не совпал номер задания.");

                    existVariant.Data = info.Data;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(info.Number))
                        throw new Exception("Не удалось сохранить вариант: не указан номер задания.");
                    if (info.Data == null || !info.Data.Any())
                        throw new Exception("Не удалось сохранить вариант: отсутствуют данные для сохранения.");
                    if (op.QueryOf<TaskVariant>().Any(v => v.Number == info.Number))
                        throw new Exception(string.Format("Не удалось сохранить вариант: номер \"{0}\" уже занят.", info.Number));

                    var newVariant = op.DataContext.Factory.Create<TaskVariant>();
                    newVariant.Number = info.Number;
                    newVariant.GeneratorVersion = info.GeneratorVersion;
                    newVariant.Data = info.Data;
                    newVariant.Task = task;
                }

                op.Complete();
            }
        }
    }
}
