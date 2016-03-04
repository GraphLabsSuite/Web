using System.Collections.Generic;

namespace GraphLabs.DomainModel.Infrastructure
{
    /// <summary> Интерфейс, описывающий изменение сущности </summary>
    public interface IEntityChange
    {
        /// <summary> Изменилось ли свойство? </summary>
        bool PropertyChanged(string propertyName);

        /// <summary> Исходные значения свойств </summary>
        IReadOnlyDictionary<string, object> OriginalValues { get; }

        /// <summary> Текущие значения свойств </summary>
        IReadOnlyDictionary<string, object> CurrentValues { get; }
    }
}