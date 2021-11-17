using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAssignment.Core;
using ProductAssignment.Core.Models;
using ProductAssignment.Core.Security;
using ProductAssignment.WebApi.Dtos.Auth;

namespace ProductAssignment.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    { 
        private readonly IUserAuthenticator _userAuthenticator;

        public AuthController(IUserAuthenticator userAuthenticator)
        {
            _userAuthenticator = userAuthenticator;
        }

        // POST: api/Login
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<TokenDto> Login([FromBody] LoginDto loginDto)
        {
            string userToken;
            if (_userAuthenticator.Login(loginDto.Username, loginDto.Password, out userToken))
            {
                //Authentication successful
                return Ok(new TokenDto{JwtToken = userToken});
            }
            return Unauthorized("Unknown username and password combination");
        }
        
    }
}