using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLabs.DomainModel.Utils
{
    public static class Extensions
    {
        /// <summary> Сравнивает два списка, порядок элементов значения не имеет </summary>
        public static bool Compare<T>(this List<T> list1, List<T> list2)
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }

            bool result = true;

            foreach (var el1 in list1)
            {
                result = result && list2.Contains(el1);
            }

            return result;
        }
    }
}
