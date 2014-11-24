using System;

namespace GraphLabs.DomainModel
{
    /// <summary> Транзакция </summary>
    public interface ITransactionScope : IDisposable
    {
        /// <summary> Коммит </summary>
        void Commit();

        /// <summary> Откат </summary>
        void Rollback();
    }
}