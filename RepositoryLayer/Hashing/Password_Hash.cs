using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Hashing
{
    public class Password_Hash
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int Iterations = 10000;

        public Password_Hash()
        {
        }

        public string PasswordHashing(string userPass)
        {
            byte[] salt = new byte[SaltSize];
            new RNGCryptoServiceProvider().GetBytes(salt);
            var pbkdf2 = new Rfc2898DeriveBytes(userPass, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);
            byte[] hashByte = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashByte, 0, SaltSize);
            Array.Copy(hash, 0, hashByte, SaltSize, HashSize);
            return Convert.ToBase64String(hashByte);
        }

        public bool VerifyPassword(string userPass, string storedHashPass)
        {
            byte[] hashByte = Convert.FromBase64String(storedHashPass);
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashByte, 0, salt, 0, SaltSize);
            var pbkdf2 = new Rfc2898DeriveBytes(userPass, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            for (int i = 0; i < HashSize; i++)
            {
                if (hashByte[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
