#region Library
using Booking.Model.Interface;
using Database.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
#endregion

namespace Booking.Model.Services
{
    public class TokenService : ItokenService
    {
        #region Dependance Injection and create security key
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration _config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));
        }
        #endregion

        public string GetToken(Users user)
        {
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
            };

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(5),
                SigningCredentials = creds
            };

            var tokenhandeler = new JwtSecurityTokenHandler();

            var token = tokenhandeler.CreateToken(tokenDescriptor);

            return tokenhandeler.WriteToken(token);
        }
    }
}
