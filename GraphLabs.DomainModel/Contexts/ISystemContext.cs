using System.Data.Entity;

namespace GraphLabs.DomainModel.EF.Contexts
{
    /// <summary> Системные настройки </summary>
    public interface ISystemContext
    {
        /// <summary> Системные настройки </summary>
        DbSet<Settings> Settings { get; set; }
    }
}