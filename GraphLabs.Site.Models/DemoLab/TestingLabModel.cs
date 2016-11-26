using System;

namespace GraphLabs.Site.Models.DemoLab
{
    /// <summary> Модель ЛР, доступной к контрольному выполнению </summary>
    public class TestingLabModel : AvailableLabModel
    {
        public DateTime AvailableTill { get; set; }
    }
}