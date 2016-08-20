using System.ComponentModel.DataAnnotations;

namespace QForum.Web.Models
{
    public class RegisterModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Food { get; set; }
        
        public string Other { get; set; }
    }
}