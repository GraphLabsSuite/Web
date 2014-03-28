using System;
using System.Data;
using System.Data.Entity;
using System.Diagnostics.Contracts;

namespace GraphLabs.DomainModel
{
    /// <summary> Менеджер транзакций </summary>
    public class TransactionManager : IDisposable
    {
        private readonly GraphLabsContext _context;

        /// <summary> Активная транзакция </summary>
        private DbContextTransaction _activeTransaction;
        
        /// <summary> Есть активная транзакция? </summary>
        public bool HasActiveTransaction
        {
            get { return _activeTransaction != null; }
        }

        /// <summary> Менеджер транзакций </summary>
        public TransactionManager(GraphLabsContext context)
        {
            Contract.Requires(context != null);

            _context = context;
        }

        /// <summary> Начать транзакцию </summary>
        public void BeginTransaction()
        {
            CheckActiveTransactionDoesNotExist();

            _activeTransaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary> Сохранить и зафиксировать изменения </summary>
        public void Commit()
        {
            CheckActiveTransactionExists();

            _context.SaveChanges();
            _activeTransaction.Commit();
            DisposeActiveTransaction();
        }

        /// <summary> Сохранить и зафиксировать изменения, после чего начать новую транзакцию </summary>
        public void IntermediateCommit()
        {
            Commit();
            BeginTransaction();
        }

        /// <summary> Откатить все изменения </summary>
        public void Rollback()
        {
            CheckActiveTransactionExists();

            _activeTransaction.Rollback();
            DisposeActiveTransaction();
        }

        /// <summary> Откатить все изменения и начать новую транзакцию </summary>
        public void IntermediateRollback()
        {
            Rollback();
            BeginTransaction();
        }


        #region Вспомогательное

        private void DisposeActiveTransaction()
        {
            _activeTransaction.Dispose();
            _activeTransaction = null;
        }

        private void CheckActiveTransactionDoesNotExist()
        {
            if (_activeTransaction != null)
                throw new InvalidOperationException("Транзакция уже начата.");
        }

        private void CheckActiveTransactionExists()
        {
            if (_activeTransaction == null)
                throw new InvalidOperationException("Активная транзакция отсутствует.");
        }

        #endregion


        #region IDisposable

        /// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
        public void Dispose()
        {
            if (_activeTransaction != null)
            {
                throw new InvalidOperationException("Обнаружена незафиксированная транзакция.");
            }
        }

        #endregion
    }
}
