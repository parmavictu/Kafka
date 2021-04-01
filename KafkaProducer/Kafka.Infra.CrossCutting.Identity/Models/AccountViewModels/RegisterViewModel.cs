using System.ComponentModel.DataAnnotations;

namespace Kafka.Infra.CrossCutting.Identity.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "A {0} deve ter ao menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get;  set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme a senha")]
        [Compare("Password", ErrorMessage = "A senha e confirmar senha não são iguais.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "O e-mail é requerido")]
        [EmailAddress(ErrorMessage = "E-mail em formato inválido")]
        public string Email { get;  set; }

        [Required(ErrorMessage = "O Cpf é requerido")]
        public string Cpf { get;  set; }

        [Required(ErrorMessage = "O Nome é requerido")]
        public string Name { get; set; }
    }
}
