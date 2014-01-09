using System.ServiceModel;
using GraphLabs.WcfServices.Data;

namespace GraphLabs.WcfServices
{
    /// <summary> Сервис для генераторов вариантов </summary>
    [ServiceContract]
    public interface IVariantGenService
    {
        /// <summary> Получает вариант задания по Id </summary>
        /// <param name="id"> Id варианта</param>
        [OperationContract]
        TaskVariantInfo GetVariant(long id);

        /// <summary> Регистрирует завершение выполнения задания </summary>
        /// <param name="info"> Новый вариант </param>
        /// <param name="taskId"> Id задания, для которого вариант. Слегка избыточен, но пусть будет. </param>
        /// <param name="updateExistent"> Обновить существующую версию? </param>
        /// <returns> True, если успешно сохранили. False, если вариант с таким номером уже есть в этом задании. </returns>
        [OperationContract]
        void SaveVariant(TaskVariantInfo info, long taskId, bool updateExistent = false);
    }
}
