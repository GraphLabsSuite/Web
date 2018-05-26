using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.TestPoolEntry
{
    /// <summary>
    /// Модель редактирования вопросов в тест-пуле
    /// </summary>
    public class AuxTestPoolEntryModel
    {
        /// <summary>
        /// Идентификатор вопроса в тестпуле
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Тип изменяемого значения
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Новое значение изменяемого поля
        /// </summary>
        public int Value { get; set; }
    }
}