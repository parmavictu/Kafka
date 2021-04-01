using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Kafka.Domain.Entities;
using Kafka.Infra.CrossCutting.Identity.Authorization;
using Kafka.Infra.CrossCutting.Identity.Models;
using Kafka.Infra.CrossCutting.Identity.Models.AccountViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Kafka.Infra.CrossCutting.Identity.TokenConfigurations
{
    public class Token
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenDescriptor _tokenDescriptor;


        public Token(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, TokenDescriptor tokenDescriptor)
        {
            _userManager = userManager;
            _tokenDescriptor = new TokenDescriptor()
            {
                Audience = Environment.GetEnvironmentVariable("TOKEN_SERVER"),
                Issuer = Environment.GetEnvironmentVariable("URL_SERVER"),
                MinutesValid = int.Parse(Environment.GetEnvironmentVariable("MINUTES_VALID"))
            };
        }

        private static long ToUnixEpochDate(DateTime date)
     => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        public async Task<object> GenerateUserToken(LoginViewModel login, Student student)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            var userClaims = await _userManager.GetClaimsAsync(user);

            userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            // Necessário converver para IdentityClaims
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(userClaims);

            var handler = new JwtSecurityTokenHandler();
            var signingConf = new SigningCredentialsConfiguration();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenDescriptor.Issuer,
                Audience = _tokenDescriptor.Audience,
                SigningCredentials = signingConf.SigningCredentials,
                Subject = identityClaims,
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(_tokenDescriptor.MinutesValid)
            });

            var encodedJwt = handler.WriteToken(securityToken);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = DateTime.Now.AddMinutes(_tokenDescriptor.MinutesValid),
                user = new
                {
                    id = user.Id,
                    nome = student.Name,
                    email = student.Email,
                    claims = userClaims.Select(c => new { c.Type, c.Value })
                }
            };

            return response;
        }
    }
}
