using System.ComponentModel.DataAnnotations;
using GraphLabs.DomainModel;
using GraphLabs.Site.Utils;

namespace GraphLabs.Site.Models
{
    /// <summary> Данные для регистрации </summary>
    public class RegistrationModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "E-mail обязателен")]
        [MaxLength(40, ErrorMessage = "Максимальная длина 40 символов")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Необходимо подтвердить E-mail")]
        [Compare("Email", ErrorMessage = "Указанные E-mail не совпадают")]
        [MaxLength(40, ErrorMessage = "Максимальная длина 40 символов")]
        [Display(Name = "E-mail ещё раз")]
        public string ConfirmEmail { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [MinLength(SecurityExtensions.MIN_PASSWORD_LENGTH, ErrorMessage = "Минимальная длина 6 символов")]
        [MaxLength(20, ErrorMessage = "Максимальная длина 20 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтверждение")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        [MaxLength(20, ErrorMessage = "Максимальная длина 20 символов")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна")]
        [MaxLength(20, ErrorMessage = "Максимальная длина 20 символов")]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Отчество обязательно")]
        [MaxLength(20, ErrorMessage = "Максимальная длина 20 символов")]
        [Display(Name = "Отчество")]
        public string FatherName { get; set; }

        [Required(ErrorMessage = "Необходимо указать группу.")]
        [Display(Name = "Группа")]
        public int IdGroup { get; set; }
    }
}