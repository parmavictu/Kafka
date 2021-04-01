using System.ComponentModel.DataAnnotations;

namespace Kafka.Infra.CrossCutting.Identity.Models.AccountViewModels
{
    public class LoginThirdPartyViewModel
    {
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
