using System;
using System.Security.Cryptography;

using HomeBudget.Identity.Domain.Constants;
using HomeBudget.Identity.Domain.Interfaces;

namespace HomeBudget.Identity.Domain
{
    public class Encryptor : IEncryptor
    {
        public string GetSalt()
        {
            var saltBytes = new byte[EncryptorOptions.SaltSize];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);

            return Convert.ToBase64String(saltBytes);
        }

        public string GetHash(string value, string salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(
                value,
                GetBytes(salt),
                EncryptorOptions.IterationsCount,
                HashAlgorithmName.SHA256);

            return Convert.ToBase64String(pbkdf2.GetBytes(EncryptorOptions.SaltSize));
        }

        private static byte[] GetBytes(string value)
        {
            var bytes = new byte[value.Length + sizeof(char)];
            Buffer.BlockCopy(value.ToCharArray(), 0, bytes, 0, bytes.Length);

            return bytes;
        }
    }
}
