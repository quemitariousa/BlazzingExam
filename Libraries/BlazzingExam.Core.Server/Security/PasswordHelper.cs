using System;
using System.Security.Cryptography;
using System.Text;

namespace BlazzingExam.Core.Server.Security
{
    public static class PasswordHelper
    {
        public static string EncodePasswordMd5(string pass) //Encrypt using MD5   
        {
            var provider = MD5.Create();
            string salt = SECURITYCODES.PasswordsSalt;
            byte[] bytes = provider.ComputeHash(Encoding.UTF32.GetBytes(salt + pass));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}