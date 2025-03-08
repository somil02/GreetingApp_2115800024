using System;
using System.ComponentModel.DataAnnotations;

namespace RepositoryLayer.Entity
{
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash{ get; set; } // Hashed Password
    }
}
