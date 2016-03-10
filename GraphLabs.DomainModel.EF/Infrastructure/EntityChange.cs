using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.DomainModel.EF.Extensions;
using GraphLabs.DomainModel.Infrastructure;

namespace GraphLabs.DomainModel.EF
{
    /// <summary> Описывает изменение сущности в текущей транзакции </summary>
    class EntityChange : IEntityChange
    {
        private readonly DbEntityEntry _entry;
        private readonly Lazy<IReadOnlyDictionary<string, object>> _originalValues;
        private readonly Lazy<IReadOnlyDictionary<string, object>> _currentValues;

        /// <summary> Описывает изменение сущности в текущей транзакции </summary>
        public EntityChange(DbEntityEntry entry)
        {
            _entry = entry;

            _originalValues = new Lazy<IReadOnlyDictionary<string, object>>(
                () => new ValuesDictionaty(_entry.OriginalValues));

            _currentValues = new Lazy<IReadOnlyDictionary<string, object>>(
                () => new ValuesDictionaty(_entry.CurrentValues));
        }

        /// <summary> Изменилось ли свойство? </summary>
        public bool PropertyChanged(string propertyName)
        {
            return _entry.CurrentValues[propertyName] != _entry.OriginalValues[propertyName];
        }

        public IReadOnlyDictionary<string, object> OriginalValues
        {
            get { return _originalValues.Value; }
        }

        public IReadOnlyDictionary<string, object> CurrentValues
        {
            get { return _currentValues.Value; }
        }
    }
}
