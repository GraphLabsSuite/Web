namespace GraphLabs.Site.Core.OperationContext
{
    /// <summary> Фабрика контекста операций </summary>
    public interface IOperationContextFactory<out TDataContext>
    {
        /// <summary> Создать контекст </summary>
        IOperationContext<TDataContext> Create();
    }
}