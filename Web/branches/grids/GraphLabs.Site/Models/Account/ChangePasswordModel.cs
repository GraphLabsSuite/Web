using System.ComponentModel.DataAnnotations;
using GraphLabs.Site.Utils;

namespace GraphLabs.Site.Models.Account
{
    /// <summary> Модель для смены пароля </summary>
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Необходимо указать старый пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Старый пароль")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Необходимо указать новый пароль")]
        [MinLength(SecurityExtensions.MIN_PASSWORD_LENGTH, ErrorMessage = "Минимальная длина 6 символов")]
        [MaxLength(20, ErrorMessage = "Максимальная длина 20 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Необходимо указать подтверждение")]
        [MinLength(SecurityExtensions.MIN_PASSWORD_LENGTH, ErrorMessage = "Минимальная длина 6 символов")]
        [MaxLength(20, ErrorMessage = "Максимальная длина 20 символов")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтверждение")]
        public string ConfirmPassword { get; set; }
    }
}