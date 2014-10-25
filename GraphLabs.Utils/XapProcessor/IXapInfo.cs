namespace GraphLabs.DomainModel.Utils
{
    /// <summary> Информация о Xap </summary>
    public interface IXapInfo
    {
        /// <summary> Название </summary>
        string Name { get; }

        /// <summary> Тэги </summary>
        string Sections { get; }

        /// <summary> Версия </summary>
        string Version { get; }
    }
}