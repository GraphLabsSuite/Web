using System.Data.Entity.Infrastructure;

namespace GraphLabs.DomainModel.EF.Extensions
{
    /// <summary> Расширения для DbEntityEntry </summary>
    internal static class DbEntityEntryExtensions
    {
        /// <summary> Свойство поменялось? </summary>
        public static bool PropertyChanged(this DbEntityEntry entityEntry, string propertyName)
        {
            return entityEntry.CurrentValues[propertyName] != entityEntry.OriginalValues[propertyName];
        }
    }
}
