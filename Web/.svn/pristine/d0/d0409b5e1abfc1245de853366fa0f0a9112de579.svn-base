using System;

namespace GraphLabs.Site.Core.OperationContext
{
    /// <summary> Контекст бизнес-операции </summary>
    public interface IOperationContext<out TDataContext> : IDisposable
    {
        /// <summary> Контекст доступа к данным </summary>
        TDataContext DataContext { get; }

        /// <summary> Сохранить изменения (завершает операцию) </summary>
        void Complete();
    }
}