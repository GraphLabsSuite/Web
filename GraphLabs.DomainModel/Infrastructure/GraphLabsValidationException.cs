using System;
using GraphLabs.Site.Core;

namespace GraphLabs.DomainModel.Infrastructure
{
    /// <summary> Ошибка валидации сущности </summary>
    public sealed class GraphLabsValidationException : GraphLabsException
    {
        /// <summary> Ошибка валидации сущности </summary>
        public GraphLabsValidationException(string property, string message)
            : base(ValidationErrors.GraphLabsValidationException_Ошибка_валидации_значения_0_1, property, message)
        {
        }

        /// <summary> Ошибка валидации </summary>
        public GraphLabsValidationException(Exception innerException, string message)
            : base(innerException, ValidationErrors.GraphLabsValidationException_Ошибка_валидации_0, message)
        {
        }
    }
}
