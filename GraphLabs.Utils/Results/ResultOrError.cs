using System.Diagnostics.Contracts;

namespace GraphLabs.Site.Utils.Results
{
    /// <summary>Результат работы метода либо сообщение об ошибке</summary>
    /// <remarks>
    /// Вспомогательный класс, для <see cref="ResultOrError"/>,
    /// для автоматического вывода типа в <see cref="Success{TResult}"/>
    /// </remarks>
    public static class ResultOrError
    {
        /// <summary>Создает результат работы метода без ошибки</summary>
        public static ResultOrError<TResult> Success<TResult>(TResult result)
        {
            return ResultOrError<TResult>.Success(result);
        }

        /// <summary>Создает результат работы метода c ошибкой</summary>
        public static ResultOrError<TResult> Failure<TResult>(string errorFormat, params object[] args)
        {
            return ResultOrError<TResult>.Failure(errorFormat, args);
        }
    }

    /// <summary>Результат работы метода либо сообщение об ошибке</summary>
    /// <remarks>
    /// <p>
    /// Позволяет возвращать результат работы метода типа <see cref="TResult"/> либо сообщение об ошибке.
    /// Результат и сообщение об ошибке являются взаимоисключающими
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
    /// Метод может вернуть отсутствие ошибки, используя <see cref="Success"/>
    /// </p>
    /// <p>
    /// Метод может вернуть наличие ошибки, используя <see cref="Failure"/>
    /// </p>
    /// </remarks>
    public struct ResultOrError<TResult>
    {
        /// <summary>Результат</summary>
        public TResult Result
        {
            get { return _result; }
        }

        /// <summary>Сообщение об ошибке</summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
        }

        /// <summary>Результат содержит ошибку</summary>
        public bool IsFailure
        {
            get { return ErrorMessage != null; }
        }

        /// <summary>Результат не содержит ошибки</summary>
        public bool IsSuccess
        {
            get { return ErrorMessage == null; }
        }

        private readonly TResult _result;
        private readonly string _errorMessage;

        private ResultOrError(string errorMessage)
        {
            _errorMessage = errorMessage;
            _result = default(TResult);
        }

        private ResultOrError(TResult result)
        {
            _result = result;
            _errorMessage = null;
        }

        /// <summary>Создает результат работы метода без ошибки</summary>
        public static ResultOrError<TResult> Success(TResult result)
        {
            return new ResultOrError<TResult>(result);
        }

        /// <summary>Создает результат работы метода с ошибкой</summary>
        public static ResultOrError<TResult> Failure(string errorFormat, params object[] args)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(errorFormat));
            Guard.Guard.IsNotNullOrWhiteSpace(errorFormat);

            return new ResultOrError<TResult>(string.Format(errorFormat, args));
        }
    }
}
