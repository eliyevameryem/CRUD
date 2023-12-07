using Microsoft.Build.Framework;

namespace Pronia.ViewsModels
{
    public class LoginVM
    {
        [Required]
        public string UsernameOrEmail { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe {  get; set; }   

    }
}
