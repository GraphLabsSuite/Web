using System;
using System.ComponentModel.DataAnnotations;

namespace GraphLabs.Site.Models
{
    public class News
    {
        public int NewsID { get; set; }

        [Required]
        [Display(Name = "Заголовок")]
        public string Header { get; set; }

        [Required]
        [Display(Name = "Текст новости")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public DateTime TimeOfPublic { get; set; }
    }

    public class NewsPubl
    {
        public int NewsID { get; set; }

        [Required]
        [Display(Name = "Заголовок")]
        public string Header { get; set; }

        [Required]
        [Display(Name = "Текст новости")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [Required]
        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Required]
        [Display(Name = "Дата")]
        public string Date { get; set; }
    }
}