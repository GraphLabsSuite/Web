using System.Data.Entity;

namespace GraphLabs.DomainModel.EF.Contexts
{
    /// <summary> Задания </summary>
    public interface ITasksContext
    {
        /// <summary> Задания </summary>
        DbSet<Task> Tasks { get; set; }

        /// <summary> Варианты заданий </summary>
        DbSet<TaskVariant> TaskVariants { get; set; }
    }
}