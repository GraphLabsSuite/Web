using System;

namespace GraphLabs.Site.Logic
{
    /// <summary> Менеджер результатов </summary>
    public interface IResultsManager
    {
        /// <summary> Зафиксировать начало выполнения ЛР (создаёт заголовок результата) </summary>
        void StartLabExecution(long variantId, Guid sessionGuid);
    }
}