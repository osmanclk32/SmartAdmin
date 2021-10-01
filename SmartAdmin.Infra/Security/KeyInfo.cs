using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SmartAdmin.Infra.Security
{
    public class KeyInfo
    {
        public byte[] Key { get; }
        public byte[] Iv { get; }

        private string _defaultKey = "46PFO2yoJkvBwz99kBEMlNkxvL40vUSGaqr/WBu3+zV=";
        private string _defaultIv = "Ou3fn+I9SVicGWMLkFEgZV==";

        public string KeyString => Convert.ToBase64String(Key);
        public string IVString => Convert.ToBase64String(Iv);

        public KeyInfo()
        {
            //using (var myAes = Aes.Create())
            //{
            //    Key = myAes.Key;
            //    Iv = myAes.IV;
            //}

            Key = Convert.FromBase64String(_defaultKey);
            Iv = Convert.FromBase64String(_defaultIv);
        }

        public KeyInfo(string key, string iv)
        {
            Key = Convert.FromBase64String(key);
            Iv = Convert.FromBase64String(iv);
        }

        public KeyInfo(byte[] key, byte[] iv)
        {
            Key = key;
            Iv = iv;
        }

        public bool HasValues()
        {
            return !string.IsNullOrWhiteSpace(KeyString) && !string.IsNullOrWhiteSpace(IVString);
        }
    }
}
