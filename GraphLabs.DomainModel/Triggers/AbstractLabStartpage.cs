using System;
using System.Collections.Generic;
using GraphLabs.DomainModel.Infrastructure;
using System.Linq;

namespace GraphLabs.DomainModel
{
    /// <summary> Абстрактная запись расписания лаб </summary>
    partial class AbstractLabStartpage : AbstractEntity
    {
        /// <summary> Валидация </summary>
        public override IEnumerable<EntityValidationError> OnEntityValidating()
        {
            return Enumerable.Empty<EntityValidationError>();
        }
    }
}
