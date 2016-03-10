using System.IO;
using JetBrains.Annotations;

namespace GraphLabs.Site.Logic.XapParsing
{
    /// <summary> Обработчик Xap </summary>
    public interface IXapProcessor
    {
        /// <summary> По xap-файлу создаёт сущность Task (в базу не пишет) </summary>
        /// <returns> null, если во время обработки произошла ошибка; иначе - новую сущность </returns>
        IXapInfo Parse([NotNull]Stream stream);
    }
}