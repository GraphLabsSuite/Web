using System.ComponentModel.DataAnnotations;

namespace GraphLabs.Site.Models
{
    public class AuthModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Вы не ввели логин")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Вы не ввели пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}