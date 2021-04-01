using System.ComponentModel.DataAnnotations;

namespace Kafka.Infra.CrossCutting.Identity.Models.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage ="E-mail obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Senha")]
        [StringLength(100, ErrorMessage = "A senha deve ter no minimo 6 digitos.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmação de senha")]
        [Compare("Password", ErrorMessage = "A senha informada e a de confirmação estão diferentes.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage ="Token de recuperação de senha não foi informado.")]
        public string Code { get; set; }
    }
}
