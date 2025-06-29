using System;
using System.Security.Cryptography;
using System.Text;

namespace HB.AbpFundation.Helpers
{
    public class PasswordHelper
    {
        public static string GetPasswordHash(string password, string salt)
        {
            salt = Convert.ToBase64String(Encoding.UTF8.GetBytes(salt));
            var buffer = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            return BitConverter.ToString(buffer).Replace("-", "");
        }
    }
}
