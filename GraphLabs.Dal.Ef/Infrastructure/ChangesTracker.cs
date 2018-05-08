using System;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using GraphLabs.Dal.Ef;
using JetBrains.Annotations;

namespace GraphLabs.Dal.Ef.Infrastructure
{
    /// <summary> Менеджер изменений </summary>
    [UsedImplicitly]
    [Obsolete("Не использовать")]
    public class ChangesTracker : IChangesTracker
    {
        private readonly GraphLabsContext _context;

        /// <summary> Менеджер изменений </summary>
        public ChangesTracker(GraphLabsContext context)
        {
            Contract.Requires(context != null);
            Guard.Guard.IsNotNull(context, nameof(context) ); //*

            _context = context;
        }

        /// <summary> Проверяет, что нет несохранённых изменений </summary>
        public void CheckHasNoChanges()
        {
            if (_context.ChangeTracker.HasChanges())
                throw new InvalidOperationException("Обнаружены несохранённые изменения.");
        }

        /// <summary> Сохранить все изменения </summary>
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        /// <summary> Откатить все изменения. ЗАВЕРШАЕТ ТЕКУЩУЮ ТРАНЗАКЦИЮ, ЕСЛИ ОНА БЫЛА. </summary>
        public void DiscardChanges()
        {
            _context.RollbackChanges();
        }

        /// <summary> Проверяет, привязана ли сущность к текущему контексту </summary>
        public bool IsEntityAttached(object entity)
        {
            return _context.ChangeTracker.Entries().Any(e => e.Entity == entity && e.State != EntityState.Detached);
        }
    }
}
