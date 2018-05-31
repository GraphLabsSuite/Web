using System.Diagnostics.Contracts;

namespace GraphLabs.Site.Utils.Results
{
    /// <summary>Сообщение о результате работы метода</summary>
    /// <remarks>
    /// <p>
    /// Позволяет трактовать результат как успех, информационное сообщение, либо как сообщение об ошибке.
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
    /// Метод может вернуть признак успешности выполнения, используя <see cref="Success"/>
    /// </p>
    /// <p>
    /// Метод может вернуть информационное сообщение, используя <see cref="Info"/>
    /// </p>
    /// <p>
    /// Метод может вернуть наличие ошибки, используя <see cref="Failure"/>
    /// </p>
    /// </remarks>
    public struct ResultMessage
    {
        /// <summary>Результат работы метода без ошибки</summary>
        public static readonly ResultMessage Success = new ResultMessage();

        /// <summary>Результат содержит ошибку</summary>
        public bool IsFailure
        {
            get { return _isFailure; }
        }

        /// <summary>Результат не содержит ошибки</summary>
        public bool IsSuccess
        {
            get { return !_isFailure; }
        }

        /// <summary>Содержит ли результат сообщение</summary>
        public bool ContainsMessage
        {
            get { return _message != null; }
        }

        private readonly string _message;
        private readonly bool _isFailure;

        private ResultMessage(string message, bool isFailure)
        {
            _message = message;
            _isFailure = isFailure;
        }

        /// <summary>Создает результат работы метода без ошибки, с информационным сообщением</summary>
        public static ResultMessage Info(string messageFormat, params object[] args)
        {
            Guard.IsNotNullOrWhiteSpace(messageFormat);

            return new ResultMessage(string.Format(messageFormat, args), false);
        }

        /// <summary>Создает результат работы метода с сообщением об ошибке</summary>
        public static ResultMessage Failure(string errorFormat, params object[] args)
        {
            Guard.IsNotNullOrWhiteSpace(errorFormat);

            return new ResultMessage(string.Format(errorFormat, args), true);
        }

        /// <summary>Преобразование к строке</summary>
        public override string ToString()
        {
            return ContainsMessage ? _message : string.Empty;
        }
    }
}