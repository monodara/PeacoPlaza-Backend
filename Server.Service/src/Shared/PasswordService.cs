using System.Security.Cryptography;
using Server.Service.src.ServiceAbstract.AuthServiceAbstract;

namespace Server.Service.src.Shared
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password, out byte[] salt)
        {
            salt = GenerateSalt();
            byte[] hashedPasswordBytes = Pbkdf2(password, salt, 10000, HashAlgorithmName.SHA256, 32);
            return Convert.ToBase64String(hashedPasswordBytes);
        }

        public bool VerifyPassword(string password, string passwordHash, byte[] salt)
        {
            byte[] hashedPasswordBytes = Convert.FromBase64String(passwordHash);
            byte[] newHashedPasswordBytes = Pbkdf2(password, salt, 10000, HashAlgorithmName.SHA256, 32);
            return SlowEquals(hashedPasswordBytes, newHashedPasswordBytes);
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private byte[] Pbkdf2(string password, byte[] salt, int iterations, HashAlgorithmName hashAlgorithm, int outputLength)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations, hashAlgorithm))
            {
                return deriveBytes.GetBytes(outputLength);
            }
        }

        // Function to compare two byte arrays in length-constant time
        private bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }
    }
}
