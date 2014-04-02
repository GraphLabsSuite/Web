using System.Linq;
using System.Collections.Generic;
using GraphLabs.DomainModel.Utils;

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

        /// <summary> Получить лабораторные работы </summary>
        public LabWork[] GetLabWorks()
        {
            CheckNotDisposed();

            return Context.LabWorks.ToArray();
        }

        /// <summary> Получить лабораторную работу по id </summary>
        public LabWork GetLabWorkById(long id)
        {
            CheckNotDisposed();

            return Context.LabWorks.SingleOrDefault(l => l.Id == id);
        }

        /// <summary> Получить варианты лабораторной работы по id лабораторной работы </summary>
        public LabVariant[] GetLabVariantsByLabWorkId(long id)
        {
            CheckNotDisposed();

            return Context.LabVariants.Where(lv => lv.LabWork.Id == id).ToArray();
        }

        /// <summary> Получить вариант лабораторной работы по id </summary>
        public LabVariant GetLabVariantById(long id)
        {
            CheckNotDisposed();

            return Context.LabVariants.SingleOrDefault(lv => lv.Id == id);
        }

        /// <summary> Проверяет, соответствует ли вариант содержанию лабораторной работы </summary>
        public bool IsLabVariantCorrect(long labVarId)
        {
            CheckNotDisposed();

            var labVar = Context.LabVariants.SingleOrDefault(lv => lv.Id == labVarId);
            if (labVar == null)
            {
                return false;
            }
            List<long> tasksId = new List<long>();
            foreach (var e in labVar.LabWork.LabEntries)
            {
                tasksId.Add(e.Task.Id);
            }
            List<long> tasksIdAlt = new List<long>();
            foreach (var t in labVar.TaskVariants)
            {
                tasksIdAlt.Add(t.Task.Id);
            }

            return tasksId.Compare(tasksIdAlt);
        }
    }
}
