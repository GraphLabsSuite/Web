using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphLabs.Site.Utils.Results
{
    /// <summary>Результат работы метода в виде лога сообщений</summary>
    /// <remarks>
    /// <p>
    /// Позволяет методу возвращать признак успешности выполнения, с множеством сообщений.
    /// Каждое сообщение может быть информационным, либо сообщением об ошибке.
    /// При наличии по крайней мере одного сообщения об ошибке, весь результат
    /// считается ошибкой.
    /// </p>
    /// <p>
    /// Для реализации используется именно структура, чтобы исключить возможность вернуть null
    /// как результат работы метода. Что в свою очередь призвано сделать обработку результата
    /// единообразной, без лишних проверок на null.
    /// </p>
    /// <p>
    /// Наличие ошибки можно проверить используя <see cref="IsFailure"/> или <see cref="IsSuccess"/>,
    /// как удобнее по смыслу.
    /// </p>
    /// <p>
    /// Метод может вернуть признак успешности выполнения без сообщений, используя <see cref="Success"/>
    /// </p>
    /// <p>
    /// Метод может вернуть или создать лог с одним информационным сообщением, используя <see cref="Info"/>
    /// </p>
    /// <p>
    /// Метод может вернуть или создать лог с одним сообщением об ошибке, используя <see cref="Failure"/>
    /// </p>
    /// </remarks>
    public struct ResultMessagesLog : IEnumerable<ResultMessage>
    {
        /// <summary>Успешный резль</summary>
        public static ResultMessagesLog Success = new ResultMessagesLog();

        /// <summary>Результат содержит ошибку</summary>
        public bool IsFailure
        {
            get { return _entries != null && _entries.Any(e => e.IsFailure); }
        }

        /// <summary>Результат не содержит ошибки</summary>
        public bool IsSuccess
        {
            get { return !IsFailure; }
        }

        private List<ResultMessage> _entries;

        /// <summary>Добавить запись в результат</summary>
        public void Add(ResultMessage entry)
        {
            if (_entries == null)
            {
                _entries = new List<ResultMessage>();
            }
            _entries.Add(entry);
        }

        /// <summary>Добавить информационное сообщение</summary>
        public void AddInfo(string messageFormat, params object[] args)
        {
            Add(ResultMessage.Info(string.Format(messageFormat, args)));
        }

        /// <summary>Добавить сообщение об ошибке</summary>
        public void AddError(string errorFormat, params object[] args)
        {
            Add(ResultMessage.Failure(string.Format(errorFormat, args)));
        }

        /// <summary>Создает результат работы метода без ошибки, с информационным сообщением</summary>
        public static ResultMessagesLog Info(string messageFormat, params object[] args)
        {
            var log = new ResultMessagesLog();
            log.AddInfo(messageFormat, args);
            return log;
        }

        /// <summary>Создает результат работы метода с сообщением об ошибке</summary>
        public static ResultMessagesLog Failure(string errorFormat, params object[] args)
        {
            var log = new ResultMessagesLog();
            log.AddError(errorFormat, args);
            return log;
        }

        #region IEnumerable<ResultMessage>

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<ResultMessage> GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}