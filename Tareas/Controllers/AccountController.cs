using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tareas.Application.Services;
using Tareas.Domain.Entitites;
using Tareas.Jwt;

namespace Tareas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly TokenService _tokenService;

        public AccountController(AccountService accountService, TokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }


        [HttpPost("create")]
        [Authorize]
        public async Task<ActionResult<string>> PostCreateAccount(User user) {

            if (user == null) throw new ArgumentNullException("Required fields");

            if (user.Password != user.PasswordConfirmed) return "401: Password are not the same.";

            try
            {
               var response = await _accountService.CreateAccountAsync(user);

                if (response != "200") throw new Exception(response);

                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> PostLoginAccount(string email, string password) {
            try
            {
                if (email == null || password == null) throw new ArgumentNullException("404: Required fields");

                var response = await _accountService.GetAccountAsync(email, password);

                if(response == null) throw new Exception();

                var token =  _tokenService.GenerateJwtToken(response);
                Console.WriteLine(token);

                return Ok(token);
            }
            catch(ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception) {
                return BadRequest("Invalid credentials");
            }
        }
        //// Método para generar el JWT token
        //private string GenerateJwtToken(User user)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key_here")); // Clave secreta
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Puedes incluir otros datos del usuario
        //        new Claim(ClaimTypes.Email, user.Email),
        //        new Claim(ClaimTypes.Name, user.Name),
        //        // Agregar otros claims si es necesario
        //    };

        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(claims),
        //        Expires = DateTime.Now.AddHours(1), // Expiración del token
        //        SigningCredentials = credentials
        //    };

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token); // Devuelve el token como un string
        //}
    }
}
