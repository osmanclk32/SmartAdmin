using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SmartAdmin.Infra.Security
{
    public static class HashManager
    {

        /// <summary>
        /// Gera Hash em SHA1
        /// </summary>
        /// <param name="text"></param>
        /// <param name="enc"></param>
        /// <returns></returns>
        public static string GenerateHashSHA1(string text, Encoding enc)
        {
            string hash;

            try
            {
                byte[] buffer = enc.GetBytes(text);

                var cryptoTransformSha1 = new SHA1CryptoServiceProvider();

                hash = BitConverter.ToString(cryptoTransformSha1.ComputeHash(buffer)).Replace("-", "").ToLower();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return hash.Trim();
        }

        public static string GeranateHashMD5(string texto)
        {

            var md5 = MD5.Create();

            byte[] inputBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(texto);
            byte[] hash = md5.ComputeHash(inputBytes);

            var sb = new System.Text.StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }


        public static string GetRandomPassword(int length)
        {
            char[] chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&".ToCharArray();
            var password = string.Empty;

            var random = new Random();

            for (var i = 0; i < length; i++)
            {
                int x = random.Next(1, chars.Length);

                if (!password.Contains(chars.GetValue(x).ToString()))
                    password += chars.GetValue(x);
                else
                    i--;
            }

            return password;
        }
    }
}
