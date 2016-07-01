using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Dal.Ef.Repositories
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
            //TODO: Заменить Score
            return Context.Results
                .Where(result => result.Student.Id == student.Id && result.Status == ExecutionStatus.Executing)
                .ToArray();
        }
    }

}
