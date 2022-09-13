using System;
using System.Security.Cryptography;
using System.Text;

namespace ScaleAddon
{
    public static class Utils
    {
        public static string Hash512(string Password, string Salt)
        {
            var convertedToBytes = Encoding.UTF8.GetBytes(Password + Salt);
            HashAlgorithm hashType = new SHA512Managed();
            byte[] hashBytes = hashType.ComputeHash(convertedToBytes);
            string hashedResult = Convert.ToBase64String(hashBytes);
            return hashedResult;
        }


    }
}