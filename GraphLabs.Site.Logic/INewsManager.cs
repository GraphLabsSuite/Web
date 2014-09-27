using System;
using System.Diagnostics.Contracts;

namespace GraphLabs.Site.Logic
{
    /// <summary> Менеджер новостей </summary>
    [ContractClass(typeof(NewsManagerContracts))]
    public interface INewsManager
    {
        /// <summary> Создать или редактировать запись </summary>
        bool CreateOrEditNews(long id, string title, string text, string authorEmail);
    }

    /// <summary> Менеджер новостей - контракты </summary>
    [ContractClassFor(typeof(INewsManager))]
    internal abstract class NewsManagerContracts : INewsManager
    {
        /// <summary> Создать или редактировать запись </summary>
        public bool CreateOrEditNews(long id, string title, string text, string authorEmail)
        {
            Contract.Requires<ArgumentException>(id >= 0);
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(title));
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(text));
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(authorEmail));

            return default(bool);
        }
    }
}