using System;
using System.Collections.Generic;

namespace GraphLabs.Site.Models.DemoLab
{
    /// <summary> Модель ЛР, доступной к ознакомительному выполнению </summary>
    public class DemoLabModel : AvailableLabModel
    {
        public ICollection<KeyValuePair<long, string>> Variants { get; set; }

        public DateTime AcquaintanceTill { get; set; }
    }
}