namespace GraphLabs.Site.Core.OperationContext
{
    /// <summary> ������� ��������� �������� </summary>
    public interface IOperationContextFactory<out TDataContext>
    {
        /// <summary> ������� �������� </summary>
        IOperationContext<TDataContext> Create();
    }
}