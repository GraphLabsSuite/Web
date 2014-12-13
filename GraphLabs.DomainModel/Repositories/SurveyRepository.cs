using System.Linq;
using System.Data.Entity;
using System;

namespace GraphLabs.DomainModel.Repositories
{
    /// <summary> Репозиторий с группами </summary>
    internal class SurveyRepository : RepositoryBase, ISurveyRepository
    {
        /// <summary> Репозиторий с группами </summary>
        public SurveyRepository(GraphLabsContext context)
            : base(context)
        {
        }

    }
}
