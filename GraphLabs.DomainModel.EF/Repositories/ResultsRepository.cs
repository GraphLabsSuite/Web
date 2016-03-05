using System.Linq;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.DomainModel.EF.Repositories
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
                .Where(result => result.Student.Id == student.Id && result.Grade == null)
                .ToArray();
        }
    }

}
