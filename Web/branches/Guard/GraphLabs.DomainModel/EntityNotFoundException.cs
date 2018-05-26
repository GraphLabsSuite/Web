using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.Site.Core;

namespace GraphLabs.DomainModel
{
    /// <summary> Запись в БД не найдена </summary>
    public sealed class EntityNotFoundException : GraphLabsException
    {
    /// <summary> Запись в БД не найдена </summary>
        public EntityNotFoundException(Type entityType, object[] key)
            : base(FormatMessage(entityType, key))
        {
            
        }

        private static string FormatMessage(Type entityType, object[] key)
        {
            return $"В БД не удалось найти запись [{entityType.Name}] с ключом [{string.Join(", ", key)}].";
        }
    }
}
