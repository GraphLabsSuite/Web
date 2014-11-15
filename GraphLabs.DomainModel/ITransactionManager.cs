using System;

namespace GraphLabs.DomainModel
{
    /// <summary> Менеджер транзакций </summary>
    public interface ITransactionManager : IDisposable
    {
        /// <summary> Есть активная транзакция? </summary>
        bool HasActiveTransaction { get; }

        /// <summary> Начать транзакцию </summary>
        IDisposable BeginTransaction();

        /// <summary> Проверяет, что нет несохранённых изменений </summary>
        void CheckHasNoChanges();

        /// <summary> Сохранить и зафиксировать изменения. ЗАВЕРШАЕТ ТЕКУЩУЮ ТРАНЗАКЦИЮ, ЕСЛИ ОНА БЫЛА. </summary>
        void Commit();

        /// <summary> Сохранить изменения, начать новую транзакцию. Имеет смысл только в контексте BeginTransaction </summary>
        void IntermediateCommit();

        /// <summary> Откатить все изменения. ЗАВЕРШАЕТ ТЕКУЩУЮ ТРАНЗАКЦИЮ, ЕСЛИ ОНА БЫЛА. </summary>
        void Rollback();

        /// <summary> Откатить все изменения и начать новую транзакцию. Имеет смысл только в контексте BeginTransaction </summary>
        void IntermediateRollback();

        /// <summary> Проверяет, привязана ли сущность к текущему контексту </summary>
        bool IsEntityAttached(object entity);
    }
}