namespace GraphLabs.Site.Models.Error
{
    /// <summary> Какая-то неизвестная ошибка </summary>
    public class OopsModel
    {
        /// <summary> Краткое описание </summary>
        public string ShortDescription { get; set; }

        /// <summary> Исключение </summary>
        public string Exception { get; set; }
    }
}