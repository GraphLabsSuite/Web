using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel.Repositories
{
    /// <summary> Репозиторий результатов </summary>
   // [ContractClass(typeof(ResultsRepositoryContracts))]
    [Obsolete("Использовать глобальный контекст IGraphLabsContext")]
    public interface IResultsRepository
    {
        /// <summary> Записать результат в БД </summary>
        void Insert(Result result);

        /// <summary> Найти неоконченные результаты выполнения </summary>
        [NotNull]
        Result[] FindNotFinishedResults(Student student);
    }

    /// <summary> Репозиторий результатов - контракты</summary>
    //[ContractClassFor(typeof(IResultsRepository))]
    //internal abstract class ResultsRepositoryContracts : IResultsRepository
    //{
    //    // ReSharper disable AssignNullToNotNullAttribute

    //    /// <summary> Записать результат в БД </summary>
    //    public void Insert(Result result)
    //    {
    //        Contract.Requires<ArgumentNullException>(result != null);
    //    }

    //    /// <summary> Найти неоконченные результаты выполнения </summary>
    //    public Result[] FindNotFinishedResults(Student student)
    //    {
    //        Contract.Requires<ArgumentNullException>(student != null);
    //        Contract.Ensures(Contract.Result<Result[]>() != null);

    //        return default(Result[]);
    //    }
    //}
}