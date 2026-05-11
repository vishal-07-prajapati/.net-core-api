using API.DTO;
using API.Interface;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwt;
        private readonly IAuthService _auth;
        public AuthController(IJwtService jwt, IAuthService auth) 
        {
            _jwt = jwt;
            _auth = auth;
        }

        [HttpGet("Test")]
        public IActionResult Test() => Ok(new { message1 = "TEST TESRT" });

        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthDTO user)
        {
            var successModel = _auth.GetUser(user);
            if (successModel.Success && successModel.Data != null)
            {
                var model = successModel.Data;
                var token = _jwt.GenerateToken(model);
                
                if(string.IsNullOrWhiteSpace(token))
                {
                    successModel.Success = false;
                    successModel.Message = "Token not generated!";
                }
                else
                {
                    successModel.Message += $" Here is your access Token: \"{token}\"";
                    successModel.Data?.AccessToken = token;
                }
            }
            
            return successModel.Success ? Ok(successModel) : Unauthorized(successModel);
        }
    }
}
