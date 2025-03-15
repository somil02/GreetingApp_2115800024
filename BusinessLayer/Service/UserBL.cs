using System;
using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using RepositoryLayer.Token;
using System.Security.Claims;


namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private IUserRL _userRl;
        private readonly JwtToken _jwtToken;

        public UserBL(IUserRL userRl, JwtToken jwtToken)
        {
            _userRl = userRl;
            _jwtToken = jwtToken;
        }

        public UserModel RegisterUser(RegisterModel registerModel)
        {
            var user = _userRl.RegisterUser(registerModel);
            if (user != null)
            {
                return new UserModel()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                };
            }
            return null;
        }

        public string LoginUser(LoginModel userLoginDto)
        {
            return _userRl.LoginUser(userLoginDto);
        }

        public async Task<string> ForgetPassword(string email)
        {
            return await _userRl.ForgetPassword(email);
        }
        public bool ResetPassword(string newPassword, string token)
        {
            var principal = _jwtToken.GetTokenValidation(token);
            var userId = Convert.ToInt32(principal.FindFirstValue("UserId"));
            return _userRl.ResetPassword(newPassword, userId);
        }
    }
}

