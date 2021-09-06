using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NewsAPI.Models;

namespace NewsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _config;

        public TokenController(IConfiguration config)
        {
            _config = config;
        }

        //Post usato per la generazione del token di autorizzazione
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody]UserModel login)
        {    
            IActionResult response = Unauthorized();    
            var user = AuthenticateUser(login);    
    
            if (user != null)    
            {    
                var tokenString = GenerateJSONWebToken(user);    
                response = Ok(new { token = tokenString });    
            }    
    
            return response;  
        }
        
        //Metodo usato per la generazione del token      
        private string GenerateJSONWebToken(UserModel userInfo)    
        {    
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));    
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);    
    
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],    
                        _config["Jwt:Issuer"],    
                         null,    
                        expires: DateTime.Now.AddMinutes(120),    
                        signingCredentials: credentials);    
    
            return new JwtSecurityTokenHandler().WriteToken(token);    
        }  

        
        private UserModel AuthenticateUser(UserModel login)    
        {    
            UserModel user = null;    
    
            //Per comodita' ho inserito delle credenziali "standard" che servono
            //ad eseguire la generazione del token 

            if (login.Username == "admin" && login.Password == "admin1234" )    
            {    
                user = new UserModel { Username = "admin", Password = "admin1234" };    
            }
            
            return user;    
        }     
    }
}