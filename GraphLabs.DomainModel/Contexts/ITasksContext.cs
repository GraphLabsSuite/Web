namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Задания </summary>
    public interface ITasksContext
    {
        /// <summary> Задания </summary>
        IEntitySet<Task> Tasks { get; set; }

        /// <summary> Варианты заданий </summary>
        IEntitySet<TaskVariant> TaskVariants { get; set; }
    }
}