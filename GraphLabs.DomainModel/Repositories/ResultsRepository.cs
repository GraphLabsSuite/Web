using System.Linq;

namespace GraphLabs.DomainModel.Repositories
{
    /// <summary> Репозиторий результатов </summary>
    public class ResultsRepository : RepositoryBase, IResultsRepository
    {
        /// <summary> Репозиторий результатов </summary>
        public ResultsRepository(GraphLabsContext context) : base(context)
        {
        }

        /// <summary> Записать результат в БД </summary>
        public void Insert(Result result)
        {
            Context.Results.Add(result);
        }

        /// <summary> Найти неоконченные результаты выполнения </summary>
        public Result[] FindNotFinishedResults(Student student)
        {
            return Context.Results
                .Where(result => result.Student == student && result.Grade == null)
                .ToArray();
        }
    }

}
