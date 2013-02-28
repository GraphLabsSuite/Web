using System.ComponentModel.DataAnnotations;

namespace GraphLabs.Site.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
                        
        [Required(ErrorMessage = "Имя обязательно")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна")]
        public string SurName { get; set; }

        [Required(ErrorMessage = "Отчество обязательно")]
        public string FatherName { get; set; }

        [Required(ErrorMessage = "E-mail обязателен")]
        public string Email { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public bool Verify { get; set; }

        public int GroupID { get; set; }
        public virtual Group Group { get; set; }
    } 
}