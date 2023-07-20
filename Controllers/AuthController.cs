using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWTDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IConfiguration configuration;

        
        //public AuthController() { }
        [JsonConstructor]
        public AuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
       
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Auth(User user)
        {
             IActionResult response = Unauthorized();
            if (user != null)
            {
                if(user.UserName.Equals("test@email.com")&& user.Password.Equals("testpass"))
                {
                    var issuer = configuration["Jwt:Issuer"];
                    var audience = configuration["Jwt:Audience"];
                    var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
                    var signingCredentials =new SigningCredentials( 
                        new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha512Signature);

                    var subject=new ClaimsIdentity(new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                            new Claim(JwtRegisteredClaimNames.Email,user.Password)
                    });
                    var expires=DateTime.UtcNow.AddMinutes(10);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = subject,
                        Expires = expires,
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = signingCredentials
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var jwtToken=tokenHandler.WriteToken(token);
                    return Ok(jwtToken);
                }

            }
            return response;
        }

        // PUT api/<AuthController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
