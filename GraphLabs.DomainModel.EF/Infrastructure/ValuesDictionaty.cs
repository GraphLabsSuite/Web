using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace GraphLabs.DomainModel.EF
{
    /// <summary> Словарь значений </summary>
    class ValuesDictionaty : IReadOnlyDictionary<string, object>
    {
        private readonly DbPropertyValues _values;

        /// <summary> Словарь значений </summary>
        public ValuesDictionaty(DbPropertyValues values)
        {
            _values = values;
        }

        /// <summary> NotImplemented </summary>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary> NotImplemented </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary> Gets the number of elements in the collection. </summary>
        public int Count => _values.PropertyNames.Count();

        /// <summary> Determines whether the read-only dictionary contains an element that has the specified key. </summary>
        public bool ContainsKey(string key)
        {
            return _values.PropertyNames.Contains(key);
        }

        /// <summary> Gets the value that is associated with the specified key. </summary>
        public bool TryGetValue(string key, out object value)
        {
            if (!ContainsKey(key))
            {
                value = default(object);
                return false;
            }

            value = _values[key];
            return true;
        }

        /// <summary> Gets the element that has the specified key in the read-only dictionary. </summary>
        public object this[string key] => _values[key];

        /// <summary> Gets an enumerable collection that contains the keys in the read-only dictionary.  </summary>
        public IEnumerable<string> Keys => _values.PropertyNames;

        /// <summary> Gets an enumerable collection that contains the values in the read-only dictionary. </summary>
        public IEnumerable<object> Values => Keys.Select(key => _values[key]);
    }
}