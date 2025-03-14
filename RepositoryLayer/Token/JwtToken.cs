﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Entity;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Token
{
    public class JwtToken
    {
        private readonly IConfiguration _config;
        public JwtToken(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(UserEntity user)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Use standard claim type
                  new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                  new Claim(ClaimTypes.Email, user.Email)
                }),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}