using System.IO;
using System.ServiceModel;

namespace GraphLabs.WcfServices
{
    /// <summary> Вспомогательный сервис для отладки заданий на сайте </summary>
    [ServiceContract]
    public interface ITaskDebugHelper
    {
        /// <summary> Загрузить задание для отладки </summary>
        [OperationContract]
        int UploadDebugTask(byte[] taskData, byte[] variantData, string email, string password);
    }
}
