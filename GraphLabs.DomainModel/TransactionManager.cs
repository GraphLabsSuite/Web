using System;
using System.Data;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel
{
    /// <summary> Менеджер транзакций </summary>
    [UsedImplicitly]
    public class TransactionManager : ITransactionManager
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

        /// <summary> Начать транзакцию (забагованная версия) </summary>
        public IDisposable BeginTransaction_BUGGED()
        {
            StartTransaction();
            return new UnitOfWork(this);
        }

        private void StartTransaction()
        {
            CheckActiveTransactionDoesNotExist();
            CheckHasNoChanges();

            _activeTransaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary> Проверяет, что нет несохранённых изменений </summary>
        public void CheckHasNoChanges()
        {
            if (_context.ChangeTracker.HasChanges())
                throw new InvalidOperationException("Обнаружены несохранённые изменения.");
        }

        /// <summary> Сохранить и зафиксировать изменения. ЗАВЕРШАЕТ ТЕКУЩУЮ ТРАНЗАКЦИЮ, ЕСЛИ ОНА БЫЛА. </summary>
        public void Commit()
        {
            _context.SaveChanges();
            if (_activeTransaction != null)
            {
                _activeTransaction.Commit();
                DisposeActiveTransaction();
            }
        }

        /// <summary> Сохранить изменения, начать новую транзакцию. Имеет смысл только в контексте BeginTransaction </summary>
        public void IntermediateCommit()
        {
            CheckActiveTransactionExists();

            Commit();
            StartTransaction();
        }

        /// <summary> Откатить все изменения. ЗАВЕРШАЕТ ТЕКУЩУЮ ТРАНЗАКЦИЮ, ЕСЛИ ОНА БЫЛА. </summary>
        public void Rollback()
        {
            _context.RollbackChanges();
            if (_activeTransaction != null)
            {
                _activeTransaction.Rollback();
                DisposeActiveTransaction();
            }
        }

        /// <summary> Откатить все изменения и начать новую транзакцию. Имеет смысл только в контексте BeginTransaction </summary>
        public void IntermediateRollback()
        {
            CheckActiveTransactionExists();

            Rollback();
            StartTransaction();
        }

        /// <summary> Проверяет, привязана ли сущность к текущему контексту </summary>
        public bool IsEntityAttached(object entity)
        {
            return _context.ChangeTracker.Entries().Any(e => e.Entity == entity && e.State != EntityState.Detached);
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


        private class UnitOfWork : IDisposable
        {
            private readonly TransactionManager _transactionManager;

            public UnitOfWork(TransactionManager transactionManager)
            {
                Contract.Requires<ArgumentNullException>(transactionManager != null);

                _transactionManager = transactionManager;
            }

            private bool _disposed = false;
            public void Dispose()
            {
                if (_disposed)
                {
                    return;
                }

                var hasChanges = _transactionManager._context.ChangeTracker.HasChanges();
                var hasTransaction = _transactionManager.HasActiveTransaction;
                if (hasChanges && hasTransaction)
                {
                    _transactionManager.Commit();
                }
                else if (hasChanges)
                {
                    throw new InvalidOperationException("Выход из контекста BeginTransaction c имеющимися изменениями, но без транзакции. Перепутали Commit и IntermediateCommit?");
                }
                else if (hasTransaction)
                {
                    _transactionManager.Rollback();
                }

                _disposed = true;
            }
        }
    }
}
