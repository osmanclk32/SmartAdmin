using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAdmin.Infra.Security
{
    /// <summary>
    /// Hash de senha AES: defina "Nenhum" para nenhum hash.
    /// </summary>
    public enum AESPasswordHash : int
    {
        None = 0,
        MD5 = 1,
        SHA1 = 2
    }

    internal static class AesDefault
    {
        public static string PassPhrase = "54321678901234567890123456789012";

        // Initialization Vector (16 bit length)
        public static string Iv = "4321567890123456";
    }
        
}
