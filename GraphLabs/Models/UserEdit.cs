using System.ComponentModel.DataAnnotations;

namespace GraphLabs.Site.Models
{
    public class UserEdit
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        [MaxLength(20, ErrorMessage = "Максимальная длина 20 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна")]
        [MaxLength(20, ErrorMessage = "Максимальная длина 20 символов")]
        public string SurName { get; set; }

        [Required(ErrorMessage = "Отчество обязательно")]
        [MaxLength(20, ErrorMessage = "Максимальная длина 20 символов")]
        public string FatherName { get; set; }

        [Required(ErrorMessage = "E-mail обязателен")]
        [MaxLength(40, ErrorMessage = "Максимальная длина 40 символов")]
        public string Email { get; set; }

        public bool Verify { get; set; }

        public int GroupID { get; set; }

        public Group Group { get; set; }
    }
}