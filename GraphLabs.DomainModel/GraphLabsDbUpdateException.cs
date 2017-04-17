using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.Site.Core;

namespace GraphLabs.DomainModel
{
    public enum DbUpgradeFailure
    {
        FkViolated,
        Unknown
    }

    public sealed class GraphLabsDbUpdateException : GraphLabsException
    {
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public override string Message { get; }

        /// <summary>
        /// Тип ошибки
        /// </summary>
        public DbUpgradeFailure ExceptionFailure { get;  }

        /// <summary> Ошибка валидации сущности </summary>
        public GraphLabsDbUpdateException(string property, string message)
            : base(ValidationErrors.DB_Update_Существуют_FK_, property, message)
        {
            Message = message;
            ExceptionFailure = DbUpgradeFailure.Unknown;
        }

        /// <summary> Ошибка валидации </summary>
        public GraphLabsDbUpdateException(Exception innerException)
            : base(innerException)
        {
            if (HResult == -2146233088)
            // Это код ошибки наличия внешних ключей на данный элемент в базе данных (вроде как)
            {
                ExceptionFailure = DbUpgradeFailure.FkViolated;
                Message = ValidationErrors.DB_Update_Существуют_FK_;
            }
            else
            {
                Message = ValidationErrors.DB_Update_Неизвестная_ошибка_;
                ExceptionFailure = DbUpgradeFailure.Unknown;
            }
        }
    }
}
