using System.Data.Entity;
using System.Linq;


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
        public LabWork FindLabWorkById(long id)
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
        public LabVariant FindLabVariantById(long id)
        {
            CheckNotDisposed();

            return Context.LabVariants.SingleOrDefault(lv => lv.Id == id);
        }

        /// <summary> Получить задания лабораторной работы по какому-либо варианту лабораторной работы </summary>
        public Task[] FindEntryTasksByLabVarId(long labVarId)
        {
            CheckNotDisposed();

            long labWorkId = Context
                .LabVariants
                .Where(v => v.Id == labVarId)
                .Select(v => v.LabWork.Id)
                .Single();
                
            return Context
                .LabEntries
                .Where(e => e.LabWork.Id == labWorkId)
                .Select(e => e.Task)
                .ToArray();
        }

        /// <summary> Получить задания варианта лабораторной работы </summary>
        public Task[] FindTasksByLabVarId(long labVarId)
        {
            CheckNotDisposed();

            return Context
                .LabVariants
                .Where(v => v.Id == labVarId)
                .SelectMany(v => v.TaskVariants)
                .Select(v => v.Task)
                .ToArray();
        }

        /// <summary> Получить варианты заданий с заданиями варианта лабораторной работы </summary>
        public TaskVariant[] GetTaskVariantsByLabVarId(long labVarId)
        {
            CheckNotDisposed();

            return Context
                .LabVariants
                .Where(v => v.Id == labVarId)
                .SelectMany(v => v.TaskVariants)
                .Include(v => v.Task)
                .ToArray();
        }
    }
}
