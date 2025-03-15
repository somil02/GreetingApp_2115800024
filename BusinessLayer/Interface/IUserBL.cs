using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        public UserModel RegisterUser(RegisterModel userRegistration);
        public string LoginUser(LoginModel userLoginDto);
        Task<string> ForgetPassword(string email);
        bool ResetPassword(string token, string newPassword);
    }
}
