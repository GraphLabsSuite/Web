using System.ComponentModel.DataAnnotations;

namespace GraphLabs.Site.Models
{
    /// <summary> Данные для авторизации </summary>
    public class AuthModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Вы не ввели e-mail")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Вы не ввели пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}