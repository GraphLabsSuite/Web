using System.ServiceModel;

namespace GraphLabs.WcfServices.DebugTaskUploader
{
    /// <summary> Вспомогательный сервис для отладки заданий на сайте </summary>
    [ServiceContract]
    public interface IDebugTaskUploader
    {
        /// <summary> Загрузить задание для отладки </summary>
        [OperationContract]
        DebugTaskData UploadDebugTask(byte[] taskData, byte[] variantData);
    }
}
