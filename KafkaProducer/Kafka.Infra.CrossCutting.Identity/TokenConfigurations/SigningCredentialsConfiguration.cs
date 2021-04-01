using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Kafka.Infra.CrossCutting.Identity.Authorization
{
    public class SigningCredentialsConfiguration
    {
        private const string SecretKey = "TesteP&r$0n@lM@n@g3r@lima.com.br";
        public static readonly SymmetricSecurityKey Key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        public SigningCredentials SigningCredentials { get; }

        public SigningCredentialsConfiguration()
        {
            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
        }
    }
}