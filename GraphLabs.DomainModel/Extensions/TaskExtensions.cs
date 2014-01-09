using System.Data.Entity;
using System.IO;
using GraphLabs.DomainModel.Utils;

namespace GraphLabs.DomainModel.Extensions
{
    /// <summary> Расширения для Task </summary>
    public static class TaskExtensions
    {
        /// <summary> Создаёт экземпляр Task по xap'у </summary>
        /// <param name="stream">xap</param>
        /// <param name="set">DbSet, куда будет приаттачен новый экземпляр</param>
        /// <returns>Null, если xap был кривой</returns>
        public static Task CreateFromXap(this DbSet<Task> set, Stream stream)
        {
            var info = XapProcessor.Parse(stream);

            if (info == null)
                return null;

            var newTask = set.Create();
            newTask.Name = info.Name;
            newTask.Sections = info.Sections;
            newTask.VariantGenerator = null;
            newTask.Note = null;
            newTask.Version = info.Version;
            newTask.Xap = stream.ReadToEnd();
            
            return newTask;
        }
    }
}