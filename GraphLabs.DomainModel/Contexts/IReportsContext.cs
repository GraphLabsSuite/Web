namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Журнал выполнения заданий </summary>
    public interface IReportsContext
    {
        /// <summary> Результаты </summary>
        IEntitySet<Result> Results { get; }

        /// <summary> Журнал действий </summary>
        IEntitySet<Action> Actions { get; }
    }
}