using CoLending.Core.Constants;
using CoLending.Core.Exceptions;
using CoLending.Core.RequestModels;
using CoLending.Core.ResponseModels;
using CoLending.Infrastructure.HttpService;
using CoLending.Manager.Interfaces;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace CoLending.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginManager _loginManager;
        private readonly ITokenManager _tokenManager;

        public LoginController(ILoginManager loginManager, ITokenManager tokenManager)
        {
            _loginManager = loginManager;
            _tokenManager = tokenManager;
        }

        [HttpPost]
        [ProducesDefaultResponseType(typeof(LoginResponse))]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> Login1([FromBody] LoginRequest loginRequest)
        {
            LoginResponse loginResponse = await _loginManager.Login(loginRequest);

            if (loginResponse != null && string.IsNullOrEmpty(loginResponse.UserId))
            {
                return BadRequest(new ErrorResponse { Message = ValidationMessages.ValidationError });
            }
            TokenClaims claimPram = new TokenClaims()
            {
                UserId = loginResponse.UserId.ToString(),
                //MobileNo = employeeLoginResponse.MobileNo,
                //EmailID = employeeLoginResponse.EmailID,
               // Role = employeeLoginResponse.Role.ToUpper()
            };
            var tokenToReturn = _tokenManager.AccesToken(claimPram);
           
            string token = tokenToReturn.ToString();

            dynamic User = new ExpandoObject();

            User.employeeLoginResponse = loginResponse;
            User.Token = tokenToReturn;

            return CreatedAtAction(nameof(Login1), new { loginResponse.UserId }, User);
        }

    }
}
