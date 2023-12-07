using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewsModels
{
    public class RegisterVM
    {
        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        [MaxLength(30)]
        public string Surname { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [MaxLength(30)]
        public string Username { get; set; }
        [Required]
          
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)] 
        public string ConfirmPassword { get; set; }


    }
}
