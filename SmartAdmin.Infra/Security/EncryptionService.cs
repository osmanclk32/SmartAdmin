
using SmartAdmin.Infra.Extensions;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SmartAdmin.Infra.Security
{

    public class EncryptionService
    { 
        private RSACryptoServiceProvider rsa;

        private static string _encryptionKey = "46PFO2yoJkvBwz99kBEMlNkxvL40vUSGaqzV";


        private bool isPrivateKeyLoaded = false;
        private bool isPublicKeyLoaded = false;        
        private readonly KeyInfo _keyInfo;

        private const string _privateKeyFileName = "\\PrivateKey.xml";
        private const string _publicKeyFileName = "\\PublicKey.xml";

        // Properties
        public bool IsPrivateKeyLoaded
        { get { return isPrivateKeyLoaded; } }

        public bool IsPublicKeyLoaded
        { get { return isPublicKeyLoaded; } }

        public EncryptionService()
        {

        }
        
        public EncryptionService(KeyInfo AeskeyInfo = null)
        {
            _keyInfo = AeskeyInfo;
        }


        /// <summary>
        /// Cria um arquivo contendo uma chave privada e outro contendo uma chave public
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool CreateKeyPairs(string path)
        {
            bool result = false;

            try
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(512))
                {

                    string privateKey = rsa.ToXmlString(true);

                    File.WriteAllText(path + _privateKeyFileName, privateKey);

                    string publicKey = rsa.ToXmlString(false);

                    File.WriteAllText(path + _publicKeyFileName, publicKey);

                    result = true;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return result;

        }

        /// <summary>
        /// Criptografa usando a chave privada
        /// </summary>
        /// <param name="textToEncrypt"></param>
        /// <param name="fullPathPrivateKey"></param>
        /// <returns></returns>
        public string PrivateEncrypt(string textToEncrypt,string fullPathPrivateKey)
        {
            string encryptedText = string.Empty;

            try
            {
                string key = GetPrivateKeyFromXml(fullPathPrivateKey);

                using (var rsa = new RSACryptoServiceProvider(512))
                {
                    rsa.FromXmlString(key);

                    isPublicKeyLoaded = false;
                    isPrivateKeyLoaded = true;

                    // Converte a string em array de byte
                    byte[] secretData = Encoding.UTF8.GetBytes(textToEncrypt);

                    // Criptografa usando a chave privada
                    byte[] encrypted = rsa.PrivateEncryption(secretData);

                    encryptedText = Convert.ToBase64String(encrypted);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return encryptedText;
        }

        /// <summary>
        /// Criptografa usando uma chave publica
        /// </summary>
        /// <param name="textToEncrypt"></param>
        /// <param name="fullPathPublicKey"></param>
        /// <returns></returns>
        public string Encrypt(string textToEncrypt, string fullPathPublicKey)
        {
            var bytesToEncrypt = Encoding.UTF8.GetBytes(textToEncrypt);

            using (var rsa = new RSACryptoServiceProvider())
            {
                try
                {
                    string publicKey = GetPublicKeyFromXml(fullPathPublicKey);

                    rsa.FromXmlString(publicKey);

                    var encryptedData = rsa.Encrypt(bytesToEncrypt, true);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);

                    return base64Encrypted;
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        /// <summary>
        /// Descriptografa usando a chave privada
        /// </summary>
        /// <param name="textToDecrypt"></param>
        /// <param name="fullPathPrivateKey"></param>
        /// <returns></returns>
        public string Decrypt(string textToDecrypt, string fullPathPrivateKey)
        {
            var bytesToDescrypt = Encoding.UTF8.GetBytes(textToDecrypt);

            using (var rsa = new RSACryptoServiceProvider(512))
            {
                try
                {
                    string privateKey = GetPrivateKeyFromXml(fullPathPrivateKey);

                    // server decrypting data with private key                    
                    rsa.FromXmlString(privateKey);

                    var resultBytes = Convert.FromBase64String(textToDecrypt);
                    var decryptedBytes = rsa.Decrypt(resultBytes, true);
                    var decryptedData = Encoding.UTF8.GetString(decryptedBytes);

                    return decryptedData.ToString();
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        /// <summary>
        /// Descriptografa usando a chave publica
        /// </summary>
        /// <param name="textToDecrypt"></param>
        /// <param name="fullPathPublicKey"></param>
        /// <returns></returns>
        public string PublicDecrypt(string textToDecrypt,string fullPathPublicKey)
        {
            string dencryptedText = string.Empty;

            try
            {
                string publicKey = GetPublicKeyFromXml(fullPathPublicKey);

                rsa.FromXmlString(publicKey);

                isPublicKeyLoaded = true;
                isPrivateKeyLoaded = false;

                byte[] encData = Encoding.UTF8.GetBytes(textToDecrypt);

                byte[]  decipher = rsa.PublicDecryption(encData);

                dencryptedText = Convert.ToBase64String(decipher);

                rsa.Dispose();
            }
            catch (Exception)
            {

                throw;
            }

            return dencryptedText;

        }

        private string GetPrivateKeyFromXml(string fullPathPrivateKey)
        {
            string privateKey = string.Empty;

            if (!File.Exists(fullPathPrivateKey + _privateKeyFileName))
            {
                throw new FileNotFoundException("Arquivo não encontrado: " + fullPathPrivateKey + _privateKeyFileName);
            }

            try
            {
                privateKey = File.ReadAllText(fullPathPrivateKey);

                
            }
            catch (Exception)
            {

                throw;
            }

            return privateKey;
        }

        private string GetPublicKeyFromXml(string publicPath)
        {
            string publicKey = string.Empty;

            if (!File.Exists(publicPath+ _publicKeyFileName))
            {
                throw new FileNotFoundException("Arquivo não encontrado: " + publicPath+ _publicKeyFileName);
            }
            
            try
            {
                publicKey = File.ReadAllText(publicPath + _publicKeyFileName);
            }
            catch (Exception)
            {

                throw;
            }

            return publicKey;
        }


        #region Metodos Aes

        public string EncryptAes(string input)
        {
            var enc = EncryptStringToBytes_Aes(input, _keyInfo.Key, _keyInfo.Iv);

            return Convert.ToBase64String(enc);
        }

        public string DecryptAes(string cipherText)
        {
            var cipherBytes = Convert.FromBase64String(cipherText);

            return DecryptStringFromBytes_Aes(cipherBytes, _keyInfo.Key, _keyInfo.Iv);
        }

        private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] key, byte[] iv)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException(nameof(plainText));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv));
            byte[] encrypted;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        private static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] key, byte[] iv)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv));

            string plaintext;
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        } 
                    }
                }

            }

            return plaintext;
        }

        //public void Dispose()
        //{
        //    if ( rsa != null )
        //    {
        //        rsa.Clear();
        //        rsa.Dispose();
        //    }
        //}


        public static string Encrypt(string clearText)
        {

            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        //cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            cipherText = cipherText.Replace(" ", "+");

            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        //cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }

    #endregion

}
