using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Middleware.ExceptionHandler;
using ModelLayer.Model;
using System.Threading.Tasks;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserBL _userBL;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserBL userBL, ILogger<UserController> logger)
        {
            _userBL = userBL;
            _logger = logger;
        }

        /// <summary>
        ///  Registers a new user.
        /// </summary>
        /// <param name="registrationModel"></param>
        /// <returns>first name, last name, email</returns>

        [HttpPost]
        public IActionResult Registration(RegisterModel registrationModel)
        {
            var result = _userBL.RegisterUser(registrationModel);
            if (result != null)
            {
                var response = new ResponseModel<UserModel>
                {
                    Success = true,
                    Message = "User Registered Successfully",
                    Data = result
                };
                return Ok(response);
            }

            return BadRequest(new ResponseModel<UserModel>
            {
                Success = false,
                Message = "Could not register"
            });
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns>response model</returns>
        [HttpPost("login")]
        public IActionResult Login(LoginModel loginModel)
        {
            var token = _userBL.LoginUser(loginModel);
            if (!string.IsNullOrEmpty(token))
            {
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Login successful",
                    Data = token
                });
            }

            return Unauthorized(new ResponseModel<string>
            {
                Success = false,
                Message = "Login failed",
                Data = null
            });
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel passwordModel)
        {
            throw new System.NotImplementedException();
        }

        [HttpPatch("resetpassword")]
        public IActionResult ResetPassword([FromQuery] int userId, ResetPasswordModel resetModel)
        {
            throw new System.NotImplementedException();
        }
    }
}
