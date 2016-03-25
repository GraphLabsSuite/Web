using System;

namespace GraphLabs.Site.Core.OperationContext
{
    /// <summary> �������� ������-�������� </summary>
    public interface IOperationContext<out TDataContext> : IDisposable
    {
        /// <summary> �������� ������� � ������ </summary>
        TDataContext DataContext { get; }

        /// <summary> ��������� ��������� (��������� ��������) </summary>
        void Complete();
    }
}