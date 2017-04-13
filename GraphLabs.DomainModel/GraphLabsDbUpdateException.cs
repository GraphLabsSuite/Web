using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.Site.Core;

namespace GraphLabs.DomainModel
{
    public sealed class GraphLabsDbUpdateException : GraphLabsException
    {
        /// <summary> Ошибка валидации сущности </summary>
        public GraphLabsDbUpdateException(string property, string message)
            : base(ValidationErrors.DB_Update_Существуют_FK_, property, message)
        {
        }

        /// <summary> Ошибка валидации </summary>
        public GraphLabsDbUpdateException(Exception innerException)
            : base(innerException, GetMessage(innerException))
        {
        }

        public static string GetMessage(Exception exception)
        {
            if (exception.HResult == -2146233088)
                // Это код ошибки наличия внешних ключей на данный элемент в базе данных (вроде как)
            {
                return ValidationErrors.DB_Update_Существуют_FK_;
            }
            return ValidationErrors.DB_Update_Неизвестная_ошибка_;
        }
    }
}
