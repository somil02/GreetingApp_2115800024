using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        public UserEntity RegisterUser(RegisterModel userRegistration);
        public UserModel LoginUser(LoginModel userLoginDto);
        public Task<string> ForgetPassword(string email);
        public bool ResetPassword(string newPassword, int userId);
    }
}
