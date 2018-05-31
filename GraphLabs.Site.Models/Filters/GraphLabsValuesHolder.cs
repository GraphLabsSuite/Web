using System;
using System.Collections.Concurrent;

namespace GraphLabs.Site.Core.Filters
{
    public class GraphLabsValuesHolder
    {
        private static readonly Random r = new Random();
        private static readonly ConcurrentDictionary<string, object> _container =
            new ConcurrentDictionary<string, object>();

        //return accesssor string to type
        public static string registerValue(object value)
        {
            while (true)
            {
                var key = r.Next().ToString();
                if (!_container.ContainsKey(key))
                {
                    lock (_container)
                    {
                        if (!_container.ContainsKey(key))
                        {
                            _container[key] = value;
                            return key;
                        }
                    }
                }
            }
            
        }

        //gets and remove value
        public static object getAndRemove(string key)
        {
            var o = _container[key];
            _container.TryRemove(key, out o);
            return o;
        }
    }
}