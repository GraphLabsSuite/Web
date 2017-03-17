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
        public GraphLabsDbUpdateException(Exception innerException, string message)
            : base(innerException, ValidationErrors.DB_Update_Существуют_FK_, message)
        {
        }
    }
}
