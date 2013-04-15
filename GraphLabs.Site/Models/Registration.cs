using System.ComponentModel.DataAnnotations;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Models
{
    /// <summary> Данные для регистрации </summary>
    public class Registration
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Логин обязателен")]
        [MaxLength(20, ErrorMessage = "Максимальная длина 20 символов")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [MinLength(6, ErrorMessage = "Минимальная длина 6 символов")]
        [MaxLength(20, ErrorMessage = "Максимальная длина 20 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Повторите пароль")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        [MaxLength(20, ErrorMessage = "Максимальная длина 20 символов")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна")]
        [MaxLength(20, ErrorMessage = "Максимальная длина 20 символов")]
        [Display(Name = "Фамилия")]
        public string SurName { get; set; }

        [Required(ErrorMessage = "Отчество обязательно")]
        [MaxLength(20, ErrorMessage = "Максимальная длина 20 символов")]
        [Display(Name = "Отчество")]
        public string FatherName { get; set; }

        [Required(ErrorMessage = "E-mail обязателен")]
        [MaxLength(40, ErrorMessage = "Максимальная длина 40 символов")]
        [Display(Name = "E-mail адрес")]
        public string Email { get; set; }

        public int ID_Group { get; set; }
        public Group Group { get; set; }
    }
}