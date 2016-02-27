using System.Data.Entity;

namespace GraphLabs.DomainModel.EF.Contexts
{
    /// <summary> Журнал выполнения заданий </summary>
    public interface IReportsContext
    {
        /// <summary> Результаты </summary>
        DbSet<Result> Results { get; }

        /// <summary> Журнал действий </summary>
        DbSet<Action> Actions { get; }
    }
}