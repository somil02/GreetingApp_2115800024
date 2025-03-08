using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        public UserModel RegisterUser(RegisterModel userRegistration);
        public UserModel LoginUser(LoginModel userLoginDto);
        public Task<string> ForgetPassword(string email);
        public bool ResetPassword(string newPassword, string token);
    }
}
