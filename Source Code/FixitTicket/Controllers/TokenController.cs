using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FixitTicket.Models;

namespace FixitTicket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly IConfiguration _config;

        public TokenController(IConfiguration config) 
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody] LoginModel login) 
        {
            IActionResult response = Unauthorized();
            var user = Authenticate(login);

            if (user != null) 
            {
                var tokenString = BuildToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string BuildToken(User user) 
        {

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserRole.ToString()),
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static User Authenticate(LoginModel login) 
        {
            User user = null;

            if (login.Username == "user" && login.Password == "web_dev") 
            {
                user = new User { Id = 989271487, Name = "Name", Email = "g_bick@u.pacific.edu", UserRole = UserRole.Resident};
            }
            else if (login.Username == "employee" && login.Password == "Arshita")
            {
                user = new User { Id = 989271234, Name = "Name", Email = "a_sandhiparthi@u.pacific.edu", UserRole = UserRole.Employee };
            }

            return user;
        }

        public class LoginModel 
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }


    }
}
