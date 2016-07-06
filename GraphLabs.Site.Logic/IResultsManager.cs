using System;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Logic
{
    /// <summary> Менеджер результатов </summary>
    public interface IResultsManager
    {
        /// <summary> Зафиксировать начало выполнения ЛР (создаёт заголовок результата) </summary>
        void StartLabExecution(long variantId, Guid sessionGuid);

        /// <summary>
        /// Зафиксировать конец выполнения ЛР (высчитывает и обновляет результат всей лабы)
        /// </summary>
        void EndLabExecution(long labVarId, Guid sessionGuid);
    }
}