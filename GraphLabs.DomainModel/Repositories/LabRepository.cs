using System.Data.Entity;
using System.Linq;
using System;
using GraphLabs.Utils;


namespace GraphLabs.DomainModel.Repositories
{
    /// <summary> Репозиторий с группами </summary>
    internal class LabRepository : RepositoryBase, ILabRepository
    {
        /// <summary> Репозиторий с лабораторными работами </summary>
        public LabRepository(GraphLabsContext context)
            : base(context)
        {
        }

        #region Получение массивов лабораторных работ

        /// <summary> Получить лабораторные работы </summary>
        public LabWork[] GetLabWorks()
        {
            CheckNotDisposed();

            return Context.LabWorks.ToArray();
        }

        /// <summary> Получить лабораторные работы, доступные в данный момент в ознакомительном режиме </summary>
        public LabWork[] GetDemoLabs(DateTime currentDate)
        {
            CheckNotDisposed();

            return GetLabWorks()
                .Where(l => l.AcquaintanceFrom.HasValue && l.AcquaintanceTill.HasValue)
                .Where(l => currentDate.CompareTo(l.AcquaintanceFrom) >= 0 && currentDate.CompareTo(l.AcquaintanceTill) <= 0)
                .ToArray();
        }

        #endregion

        #region Получение массива вариантов лабораторной работы

        /// <summary> Получить варианты лабораторной работы по id лабораторной работы </summary>
        public LabVariant[] GetLabVariantsByLabWorkId(long id)
        {
            CheckNotDisposed();

            return Context.LabVariants
                .Where(lv => lv.LabWork.Id == id)
                .ToArray();
        }

        /// <summary> Получить ознакомительные варианты лабораторной работы по id лабораторной работы </summary>
        public LabVariant[] GetDemoLabVariantsByLabWorkId(long labId)
        {
            CheckNotDisposed();

            return Context.LabVariants
                .Where(lv => lv.LabWork.Id == labId)
                .Where(lv => lv.IntroducingVariant)
                .ToArray();
        }

        /// <summary> Получить готовые ознакомительные варианты лабораторной работы по id лабораторной работы </summary>
        public LabVariant[] GetCompleteDemoLabVariantsByLabWorkId(long labId)
        {
            CheckNotDisposed();

            return GetDemoLabVariantsByLabWorkId(labId)
                .Where(lv => VerifyCompleteVariant(lv.Id))
                .ToArray();
        }

        #endregion

        #region Проверки

        /// <summary> Проверить существование лабораторной работы </summary>
        public bool CheckLabWorkExist(long id)
        {
            CheckNotDisposed();

            LabWork lab = Context.LabWorks.SingleOrDefault(l => l.Id == id);

            return (lab == null ? false : true);
        }

        /// <summary> Проверить существование варианта лабораторной работы </summary>
        public bool CheckLabVariantExist(long id)
        {
            CheckNotDisposed();

            LabVariant labVariant = Context.LabVariants.SingleOrDefault(l => l.Id == id);

            return (labVariant == null ? false : true);
        }

        /// <summary> Проверить принадлежность варианта л.р. лабораторной работе </summary>
        public bool CheckLabVariantBelongLabWork(long labId, long labVarId)
        {
            CheckNotDisposed();

            LabVariant labVariant = Context.LabVariants.Where(lv => lv.Id == labVarId).SingleOrDefault(lv => lv.LabWork.Id == labId);

            return (labVariant == null ? false : true);
        }

        /// <summary> Проверка соответствия варианта лабораторной работы содержанию работы </summary>
        public bool VerifyCompleteVariant(long variantId)
        {
            CheckNotDisposed();

            long labWorkId = Context.LabVariants
                .Where(v => v.Id == variantId)
                .Select(v => v.LabWork.Id)
                .Single();

            long[] labEntry = Context.LabEntries
                .Where(e => e.LabWork.Id == labWorkId)
                .Select(e => e.Task.Id)
                .ToArray();

            long[] currentVariantEntry = Context.LabVariants
                .Where(l => l.Id == variantId)
                .SelectMany(t => t.TaskVariants)
                .Select(t => t.Task.Id)
                .ToArray();

            return labEntry.ContainsSameSet(currentVariantEntry);
        }

        #endregion

        /// <summary> Получить лабораторную работу по id </summary>
        public LabWork GetLabWorkById(long id)
        {
            CheckNotDisposed();

            return Context.LabWorks.SingleOrDefault(l => l.Id == id);
        }

        /// <summary> Найти вариант лабораторной работы по id </summary>
        public LabVariant FindLabVariantById(long id)
        {
            CheckNotDisposed();

            return Context.LabVariants.SingleOrDefault(lv => lv.Id == id);
        }

        /// <summary> Получить варианты заданий с заданиями варианта лабораторной работы </summary>
        public TaskVariant[] GetTaskVariantsByLabVarId(long labVarId)
        {
            CheckNotDisposed();

            return Context.LabVariants
                .Where(v => v.Id == labVarId)
                .SelectMany(v => v.TaskVariants)
                .Include(v => v.Task)
                .ToArray();
        }
    }
}
