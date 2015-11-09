using System.IO;
using System.ServiceModel;

namespace GraphLabs.WcfServices
{
    /// <summary> Вспомогательный сервис для отладки заданий на сайте </summary>
    [ServiceContract]
    public interface IDebugTaskUploader
    {
        /// <summary> Загрузить задание для отладки </summary>
        [OperationContract]
        long UploadDebugTask(byte[] taskData, byte[] variantData);
    }
}
