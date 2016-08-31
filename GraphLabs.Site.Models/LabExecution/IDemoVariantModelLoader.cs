using System;

namespace GraphLabs.Site.Models.LabExecution
{
    /// <summary> ��������� ������� ��� ���������� � ��������������� ������ </summary>
    public interface IDemoVariantModelLoader
    {
        VariantExecutionModelBase Load(long labVariantId, int? taskIndex, Uri taskCompleteRedirect);
    }
}