using System.Web;
using Microsoft.Practices.Unity;

namespace GraphLabs.Site
{
    /// <summary> Сохраняет объекты в текущем HttpContext'е, в результате
    /// чего они доступны строго в рамках одного запроса </summary>
    public class PerRequestLifetimeManager : LifetimeManager
    {
        private readonly object _key = new object();

        /// <summary> Retrieve a value from the backing store associated with this Lifetime policy. </summary>
        /// <returns> the object desired, or null if no such object is currently stored. </returns>
        public override object GetValue()
        {
            if (HttpContext.Current == null || !HttpContext.Current.Items.Contains(_key))
            {
                return null;
            }

            return HttpContext.Current.Items[_key];
        }

        /// <summary> Remove the given object from backing store. </summary>
        public override void RemoveValue()
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items.Remove(_key);
            }
        }

        /// <summary> Stores the given value into backing store for retrieval later. </summary>
        /// <param name="newValue">The object being stored.</param>
        public override void SetValue(object newValue)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items[_key] = newValue;
            }
        }
    }
}