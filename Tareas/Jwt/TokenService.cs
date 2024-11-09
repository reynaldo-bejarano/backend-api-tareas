using Jose;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tareas.Domain.Entitites;
namespace Tareas.Jwt
{
    public class TokenService
    {
        private readonly IOptions<JwtSettings> _jwtSettings;

        public TokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        // Método para generar el JWT token
        public string GenerateJwtToken(User user)
        {
            // Leer la clave secreta desde la configuración
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.SecretKey)); // Usar la clave desde la configuración
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Puedes incluir otros datos del usuario
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            
            // Agregar otros claims si es necesario
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.Value.ExpiryDurationInHours),
                NotBefore = DateTime.UtcNow,  // El token no será válido hasta este momento
                SigningCredentials = credentials,
                Issuer = _jwtSettings.Value.Issuer,   // Usar el emisor desde la configuración
                Audience = _jwtSettings.Value.Audience // Usar la audiencia desde la configuración
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token); // Devuelve el token como un string
        }
    }
}
