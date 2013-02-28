using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraphLabs.Site.Models
{
    public class Group
    {
        public int GroupID { get; set; }

        [Required(ErrorMessage = "Название обязательно")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Год обучения обязателен")]
        public int Year { get; set; }
                
        public bool IsActive { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}