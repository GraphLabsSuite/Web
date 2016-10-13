using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace GraphLabs.Site.Utils
{
    public static class EnumerableExtensions
    {
        /// <summary> Сравнивает два списка, порядок элементов значения не имеет </summary>
        public static bool ContainsSameSet<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
        {
            Contract.Requires<ArgumentNullException>(list1 != null && list2 != null);

            if (list1.Count() != list2.Count())
            {
                return false;
            }

            var set1 = GetItemsSet(list1);
            var set2 = GetItemsSet(list2);

            return set1.IsSubsetOf(set2) && set2.IsSubsetOf(set1);
        }

        private static HashSet<KeyValuePair<T, int>> GetItemsSet<T>(IEnumerable<T> list)
        {
            var dict = new Dictionary<T, int>();
            foreach (var item in list)
            {
                if (dict.ContainsKey(item))
                    dict[item] += 1;
                else
                {
                    dict[item] = 1;
                }
            }

            return new HashSet<KeyValuePair<T, int>>(dict);
        }
    }
}
