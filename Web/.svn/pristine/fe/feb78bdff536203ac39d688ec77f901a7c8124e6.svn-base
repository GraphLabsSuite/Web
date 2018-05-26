using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Dal.Ef.Repositories
{
    /// <summary> Репозиторий с тестпулами </summary>
    public class TestPoolRepository : RepositoryBase, ITestPoolRepository
    {
        /// <summary> Репозиторий с тестпулами </summary>
        public TestPoolRepository(GraphLabsContext context)
            : base(context)
        {
        }

        public TestPool GetTestPoolById(long id)
        {
            CheckNotDisposed();
            return Context.TestPools.SingleOrDefault(l => l.Id == id);
        }
    }
}