using agendaBackEnd.Models;
using agendaBackEnd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace agendaBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Auth : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public static UsersModel user = new UsersModel();
        public Auth(IConfiguration configuration, IAuthService authService)
        {
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UsersModel>> Register([FromBody] UserRegister request)
        {
            CreatePasswordHash(request.password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Id = request.Id;
            user.nombre = request.nombre;
            user.apellido = request.apellido;
            user.email = request.email;
            user.password = request.password;
            user.passwordHash = passwordHash;
            user.passwordSalt = passwordSalt;

            await _authService.SignUp(user);
           
            return Ok(user);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login([FromBody] User userRequest)
        {

            var userBd = _authService.GetUserForSignIn(userRequest.Email);

            if(userBd is null)
            {
                return BadRequest("User not found");
            }

            if(!VerifyPasswordHash(userRequest.Password, userBd.passwordHash, userBd.passwordSalt))
            {
                return BadRequest("Wrong password");
            }


            string token = CreateToken(userBd);
            return Ok(token);
        }

        private string CreateToken(UsersModel user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.nombre),
                new Claim(ClaimTypes.Email, user.email)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            } 
        }
    }
}
