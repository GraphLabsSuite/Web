using System.ComponentModel.DataAnnotations;
using GraphLabs.Site.Utils;

namespace GraphLabs.Site.Models
{
    /// <summary> Модель для смены пароля </summary>
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Вы не ввели старый пароль")]
        [Display(Name = "Старый пароль")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Вы не ввели новый пароль")]
        [MinLength(SecurityExtensions.MIN_PASSWORD_LENGTH, ErrorMessage = "Минимальная длина 6 символов")]
        [MaxLength(20, ErrorMessage = "Максимальная длина 20 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Вы не ввели подтверждение")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтверждение")]
        public string ConfirmPassword { get; set; }
    }
}