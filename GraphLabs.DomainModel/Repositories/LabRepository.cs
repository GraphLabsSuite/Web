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
        public LabWork GetLabWorkById(long id)
        {
            CheckNotDisposed();

            return Context.LabWorks.Single(l => l.Id == id);
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

            return Context.LabVariants.Single(lv => lv.Id == id);
        }
    }
}
