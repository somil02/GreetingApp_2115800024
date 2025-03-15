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
            var result = await _userBL.ForgetPassword(passwordModel.Email);
            var response = new ResponseModel<string>();
            if (result != null)
            {
                response.Success = true;
                response.Message = $"Reset password link sent successfully to your email address {result}";
                return Ok(response);
            }
            response.Success = false;
            response.Message = $"User is not present with email id ={passwordModel.Email}";
            return BadRequest(response);
        }

        [HttpPatch("resetpassword")]
        public IActionResult ResetPassword([FromQuery] string token,[FromBody] ResetPasswordModel resetModel)
        {

            var result = _userBL.ResetPassword(resetModel.NewPassword, token);
            var response = new ResponseModel<bool>();
            if (result)
            {

                response.Success = true;
                response.Message = "Password reset successful";
                response.Data = result;
                return Ok(response);
            }
            response.Success = false;
            response.Message = "Error occurred resetting password. Please try again.";
            return BadRequest(response);
        }
    }
}
