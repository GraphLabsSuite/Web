using System;

namespace GraphLabs.DomainModel.EF
{
    /// <summary> Менеджер изменение </summary>
    public interface IChangesTracker
    {
        /// <summary> Сохранить все изменения </summary>
        void SaveChanges();

        /// <summary> Проверяет, что изменений нет. В противном случае бросает исключение </summary>
        void CheckHasNoChanges();

        /// <summary> Откатить имеющиеся изменения </summary>
        void DiscardChanges();
    }
}