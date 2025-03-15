using System.Linq;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using ModelLayer.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RepositoryLayer.Hashing;
using RepositoryLayer.Interface;
using RepositoryLayer.Token;
using System.Net.Mail;
using System.Net;
using System.IdentityModel.Tokens.Jwt;


namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly GreetingDbContext _dbContext;
        private readonly Password_Hash _passwordHash;
        private readonly JwtToken _JwtToken;
        private readonly IConfiguration _config;
        private readonly EmailService _emailService;

        public UserRL(GreetingDbContext context, IConfiguration config, Password_Hash hash,JwtToken jwtToken, EmailService emailService)
        {
            _dbContext = context;
            _passwordHash = hash;
            _JwtToken = jwtToken;
            _config = config;
            _emailService = emailService;
        }

        public UserEntity RegisterUser(RegisterModel userRegistrationDto)
        {
            var existingUser = _dbContext.Users.FirstOrDefault(e => e.Email == userRegistrationDto.Email);

            if (existingUser == null)
            {
                var newUser = new UserEntity
                {
                    FirstName = userRegistrationDto.FirstName,
                    LastName = userRegistrationDto.LastName,
                    Email = userRegistrationDto.Email,
                    PasswordHash = _passwordHash.PasswordHashing(userRegistrationDto.Password)
                };

                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();

                return newUser;
            }
            return null;
        }

        public string LoginUser(LoginModel userLoginDto)
        {
            var validUser = _dbContext.Users.FirstOrDefault(e => e.Email == userLoginDto.Email);

            if (validUser != null)
            {
                if (_passwordHash.VerifyPassword(userLoginDto.Password, validUser.PasswordHash))
                {
                    // Debug logging
                    if (string.IsNullOrEmpty(validUser.Email))
                        throw new Exception("validUser.Email is null or empty");

                    if (string.IsNullOrEmpty(validUser.FirstName))
                        throw new Exception("validUser.FirstName is null or empty");

                    var token = _JwtToken.GenerateToken(validUser);
                    return token;
                }
            }

            return null;
        }

        public async Task<string> ForgetPassword(string email)
        {
            var validUser = _dbContext.Users.FirstOrDefault(e => e.Email == email);

            if (validUser != null)
            {

                var generatedToken = _JwtToken.GenerateTokenReset(validUser.Email, validUser.Id);

                var baseUrl = _config["ResetURL:ResetPasswordUrl"];
                var callbackUrl = $"{baseUrl}?token={generatedToken}";

                await _emailService.SendEmailAsync(email, "Reset Password", callbackUrl);

                return "Ok";
            }
            return null;
        }

        public bool ResetPassword(string newPassword, int userId)
        {

            var validUser = _dbContext.Users.FirstOrDefault(e => e.Id == userId);

            if (validUser != null)
            {
                validUser.PasswordHash = _passwordHash.PasswordHashing(newPassword);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

    }
}