// ////////////////////////////////////////////////////// /////////////////////////////////////////////////////////
// AESCrypt - criptografia e descriptografia de chave simétrica usando algoritmo AES / Rijndael (128, 192 e 256)
// ////////////////////////////////////////////////////// /////////////////////////////////////////////////////////
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SmartAdmin.Infra.Security
{
    /// <summary>
    /// Esta classe usa um algoritmo de chave simétrica (AES) para criptografar e descriptografar dados. Contanto que seja inicializado com o mesmo construtor e
    /// parâmetros, a classe usará a mesma chave. Antes de realizar a criptografia, a classe pode acrescentar bytes aleatórios ao texto simples e gerar diferentes
    /// valores criptografados do mesmo texto simples, chave de criptografia, vetor de inicialização e outros parâmetros. Esta classe é segura para thread.
    /// </summary>
    public class AESCrypt
    {
        #region Private members
        // Esses membros serão usados ​​para realizar a criptografia e a descriptografia.
        private ICryptoTransform encryptor = null;
        private ICryptoTransform decryptor = null;

        private AESCryptOptions Options = null;

        #endregion

        #region Construtores

        public AESCrypt() : this(AesDefault.PassPhrase,AesDefault.Iv) //, new AESCryptOptions(AESPasswordHash.None) )
        {
        }

        /// <summary>
        /// Use este construtor para realizar a criptografia / descriptografia com as seguintes opções:
        /// - chave de 128/192/256 bits (dependendo do comprimento da senha longa em bits)
        /// - algoritmo de hash de senha SHA-1 com salt de hash de senha de 4 a 8 bytes e 1 iteração de senha
        /// - hashing sem salt
        /// - sem vetor de inicialização
        /// - modo de livro de código eletrônico (BCE)
        /// </summary>
        /// <param name="passPhrase">
        /// Passphrase (em formato de string) a partir da qual uma senha pseudo-aleatória será derivada. A senha derivada será usada para gerar a chave de criptografia.
        /// </param>
        /// <remarks>
        /// Este construtor NÃO é recomendado porque não usa o vetor de inicialização e usa o modo de cifra ECB, que é menos seguro que o modo CBC.
        /// </remarks>
        public AESCrypt(string passPhrase) : this(passPhrase, null)
        {
        }

        /// <summary>
        /// Use este construtor para realizar a criptografia / descriptografia com as seguintes opções:
        /// - chave de 128/192/256 bits (dependendo do comprimento da senha longa em bits)
        /// - algoritmo de hash de senha SHA-1 com sal de hash de senha de 4 a 8 bytes e 1 iteração de senha
        /// - hashing sem sal
        /// - modo de encadeamento de blocos de criptografia (CBC)
        /// </summary>
        /// <param name="passPhrase">
        /// Passphrase (em formato de string) a partir da qual uma senha pseudo-aleatória será derivada. A senha derivada será usada para gerar a chave de criptografia.
        /// </param>
        /// <param name="initVector">
        ///  Vetor de inicialização (IV). Este valor é necessário para criptografar o primeiro bloco de dados de texto simples. IV deve ter exatamente 16 caracteres ASCII.
        /// </param>
        public AESCrypt(string passPhrase,string initVector) : this(passPhrase, initVector, new AESCryptOptions())
        {
        }

        /// <summary>
        /// Use este construtor para executar a criptografia / descriptografia com opções personalizadas.
        /// Consulte a documentação AESCryptOptions para obter detalhes.
        /// </summary>
        /// <param name="passPhrase">
        /// Passphrase (em formato de string) a partir da qual uma senha pseudo-aleatória será derivada. A senha derivada será usada para gerar a chave de criptografia.
        /// </param>
        /// <param name="initVector">
        ///  Vetor de inicialização (IV). Este valor é necessário para criptografar o primeiro bloco de dados de texto simples. IV deve ter exatamente 16 caracteres ASCII.
        /// </param>
        /// <param name="options">
        /// Um conjunto de opções personalizadas (ou padrão) para usar para a criptografia / descriptografia: consulte a documentação AESCryptOptions para obter detalhes.
        /// </param>
        public AESCrypt(string passPhrase, string initVector, AESCryptOptions options)
        {
            // armazena o objeto de opções localmente.
            this.Options = options;

            //  Verifica o tamanho correto (ou nulo) da chave criptográfica.
            if (Options.FixedKeySize.HasValue
                && Options.FixedKeySize != 128
                && Options.FixedKeySize != 192
                && Options.FixedKeySize != 256)
                throw new NotSupportedException("ERROR: options.FixedKeySize must be NULL (for auto-detect) or have a value of 128, 192 or 256");

            // Vetor de inicialização convertido em uma matriz de bytes.
            byte[] initVectorBytes = null;

            // Salt usado para hashing de senha (para gerar a chave, não durante
            // criptografia) convertido em uma matriz de bytes.
            byte[] saltValueBytes = null;

            // Pega os bytes do vetor de inicialização.
            if (initVector == null) initVectorBytes = new byte[0];
            else initVectorBytes = Encoding.UTF8.GetBytes(initVector);

           
            int keySize = (Options.FixedKeySize.HasValue)
                ? Options.FixedKeySize.Value
                : GetAESKeySize(passPhrase);

            
            byte[] keyBytes = null;
            if (Options.PasswordHash == AESPasswordHash.None)
            {
                
                keyBytes = System.Text.Encoding.UTF8.GetBytes(passPhrase);
            }
            else
            {
               
                if (Options.PasswordHashSalt == null) saltValueBytes = new byte[0];
                else saltValueBytes = Encoding.UTF8.GetBytes(options.PasswordHashSalt);

                // Gere a senha, que será usada para derivar a chave.
                PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                           passPhrase,
                                                           saltValueBytes,
                                                           Options.PasswordHash.ToString().ToUpper().Replace("-", ""),
                                                           Options.PasswordHashIterations);

                // Converte a chave em uma matriz de bytes ajustando o tamanho de bits em bytes.
                keyBytes = password.GetBytes(keySize / 8);
            }

            //Inicializa o objeto-chave AES.
            AesManaged symmetricKey = new AesManaged();

            symmetricKey.Padding = Options.PaddingMode;

            // Use the unsafe ECB cypher mode (not recommended) if no IV has been provided, otherwise use the more secure CBC mode.
            symmetricKey.Mode = (initVectorBytes.Length == 0)
                ? CipherMode.ECB
                : CipherMode.CBC;

            // Use o modo cifrado ECB inseguro (não recomendado) se nenhum IV tiver sido fornecido, caso contrário, use o modo CBC mais seguro.
            encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
        }
        #endregion

        #region Rotinas de Encriptação
        /// <summary>
        /// Criptografa um valor de string gerando uma string codificada em base64.
        /// </summary>
        /// <param name="plainText">
        /// Cadeia de caracteres de texto simples a ser criptografada.
        /// </param>
        /// <returns>
        /// Texto cifrado formatado como uma string codificada em base64.
        /// </returns>
        public string Encrypt(string plainText)
        {
            return Encrypt(Encoding.UTF8.GetBytes(plainText));
        }

        /// <summary>
        /// Criptografa uma matriz de bytes, gerando uma string codificada em base64.
        /// </summary>
        /// <param name="plainTextBytes">
        /// Bytes de texto simples a serem criptografados.
        /// </param>
        /// <returns>
        /// Texto cifrado formatado como uma string codificada em base64.
        /// </returns>
        public string Encrypt(byte[] plainTextBytes)
        {
            return Convert.ToBase64String(EncryptToBytes(plainTextBytes));
        }

        /// <summary>
        /// Criptografa um valor de string gerando uma matriz de bytes de texto cifrado.
        /// </summary>
        /// <param name="plainText">
        /// Cadeia de caracteres de texto simples a ser criptografada.
        /// </param>
        /// <returns>
        /// Texto cifrado formatado como uma matriz de bytes.
        /// </returns>
        public byte[] EncryptToBytes(string plainText)
        {
            return EncryptToBytes(Encoding.UTF8.GetBytes(plainText));
        }

        /// <summary>
        /// Criptografa um valor de string gerando uma matriz de bytes de texto cifrado.
        /// </summary>
        /// <param name="plainTextBytes">
        /// Cadeia de caracteres de texto simples a ser criptografada.
        /// </param>
        /// <returns>
        /// exto cifrado formatado como uma matriz de bytes.
        /// </returns>
        public byte[] EncryptToBytes(byte[] plainTextBytes)
        {
            // Adicione salt no início dos bytes de texto simples (se necessário)
            byte[] plainTextBytesWithSalt = (UseSalt()) ? AddSalt(plainTextBytes) : plainTextBytes;

            byte[] cipherTextBytes = null;

            // Tornar as operações criptográficas seguras para thread.
            lock (this)
            {
                // A criptografia será executada usando fluxo de memória.
                using (MemoryStream memoryStream = new MemoryStream())
                {

                    
                    using (CryptoStream cryptoStream = new CryptoStream(
                                                       memoryStream,
                                                       encryptor,
                                                        CryptoStreamMode.Write))
                    {

                       
                        cryptoStream.Write(plainTextBytesWithSalt, 0, plainTextBytesWithSalt.Length);
                       
                        cryptoStream.FlushFinalBlock();
                        // Move os dados criptografados da memória para uma matriz de bytes.
                        cipherTextBytes = memoryStream.ToArray();
                        cryptoStream.Close();
                    }
                    memoryStream.Close();
                }
              
                return cipherTextBytes;
            }
        }
        #endregion

        #region Rotinas de descriptografia
        /// <summary>
        ///  Descriptografa um valor de texto cifrado codificado em base64, gerando um resultado de string.
        /// </summary>
        /// <param name="cipherText">
        /// Cifra de texto cifrado codificado em Base64 a ser descriptografado.
        /// </param>
        /// <returns>
        /// valor da string descriptografada.
        /// </returns>
        public string Decrypt(string cipherText)
        {
            return Decrypt(Convert.FromBase64String(cipherText));
        }

        /// <summary>
        /// Descriptografa uma matriz de bytes contendo valor de texto cifrado e gera um
        /// string result.
        /// </summary>
        /// <param name="cipherTextBytes">
        ///  Matriz de bytes contendo dados criptografados.
        /// </param>
        /// <returns>
        /// Valor da string descriptografada.
        /// </returns>
        public string Decrypt(byte[] cipherTextBytes)
        {
            return Encoding.UTF8.GetString(DecryptToBytes(cipherTextBytes));
        }

        /// <summary>
        /// Descriptografa um valor de texto cifrado codificado em base64 e gera uma matriz de bytes
        /// de dados de texto simples.
        /// </summary>
        /// <param name="cipherText">
        /// Cifra de texto cifrado codificado em Base64 a ser descriptografado.
        /// </param>
        /// <returns>
        /// Matriz de bytes contendo valor descriptografado.
        /// </returns>
        public byte[] DecryptToBytes(string cipherText)
        {
            return DecryptToBytes(Convert.FromBase64String(cipherText));
        }

        /// <summary>
        /// Descriptografa um valor de texto cifrado codificado em base64 e gera uma matriz de bytes
        /// de dados de texto simples.
        /// </summary>
        /// <param name="cipherTextBytes">
        /// Matriz de bytes contendo dados criptografados.
        /// </param>
        /// <returns>
        /// Byte array containing decrypted value.
        /// </returns>
        public byte[] DecryptToBytes(byte[] cipherTextBytes)
        {
            byte[] decryptedBytes = null;
            byte[] plainTextBytes = null;
            int decryptedByteCount = 0;
            int saltLen = 0;

            // Uma vez que não sabemos qual será o tamanho do valor descriptografado, use o mesmo
            // tamanho como texto cifrado. O texto cifrado é sempre maior do que o texto simples
            // (na criptografia de cifra de bloco), então usaremos apenas o número de
            // byte de dados descriptografado depois de sabermos o quão grande é.
            decryptedBytes = new byte[cipherTextBytes.Length];

            //Vamos tornar as operações criptográficas seguras para thread.
            lock (this)
            {
                using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                {
                    //Para realizar a descriptografia, devemos usar o modo de leitura.
                    using (CryptoStream cryptoStream = new CryptoStream(
                                                       memoryStream,
                                                       decryptor,
                                                       CryptoStreamMode.Read))
                    {

                        // Descriptografando dados e obtendo a contagem de bytes de texto simples.
                        decryptedByteCount = cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                        cryptoStream.Close();
                    }

                    memoryStream.Close();
                }
            }

            //Se estivermos usando salt, obtenha seu comprimento dos primeiros 4 bytes de dados de texto simples.
            if (UseSalt())
            {
                saltLen = (decryptedBytes[0] & 0x03) |
                            (decryptedBytes[1] & 0x0c) |
                            (decryptedBytes[2] & 0x30) |
                            (decryptedBytes[3] & 0xc0);
            }

            //Aloca a matriz de bytes para conter o texto simples original (sem salt).
            plainTextBytes = new byte[decryptedByteCount - saltLen];

            // Copie o texto simples original descartando o valor de sal se necessário.
            Array.Copy(decryptedBytes, saltLen, plainTextBytes,
                        0, decryptedByteCount - saltLen);

            return plainTextBytes;
        }
        #endregion

        #region funções auxiliares
        /// <summary>
        /// Obtém KeySize pelo comprimento da senha em bytes.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static int GetAESKeySize(string passPhrase)
        {
            switch (passPhrase.Length)
            {
                case 16:
                    return 128;
                case 24:
                    return 192;
                case 32:
                    return 256;
                default:
                    throw new NotSupportedException("ERROR: AES Password must be of 16, 24 or 32 bits length!");
            }
        }

        /// <summary>
        ///  Verifica se o salt deve ser usado ou não para a criptografia / descriptografia.
        /// </summary>
        /// <returns></returns>
        private bool UseSalt()
        {
            //Use salt se o valor máximo de sal for maior que 0 e igual ou maior que comprimento mínimo de salt.
            return (Options.MaxSaltLength > 0 && Options.MaxSaltLength >= Options.MinSaltLength);
        }


        /// <summary>
        /// Adiciona uma matriz de bytes gerados aleatoriamente no início do
        /// array contendo o valor original do texto simples.
        /// </summary>
        /// <param name="plainTextBytes">
        /// Matriz de bytes contendo o valor original do texto simples.
        /// </param>
        /// <returns>
        /// Array original de bytes de texto simples (se salt não for usado) ou um
        /// array modificado contendo um sal gerado aleatoriamente adicionado no
        /// início dos bytes de texto simples.
        /// </returns>
        private byte[] AddSalt(byte[] plainTextBytes)
        {
           
            if (!UseSalt()) return plainTextBytes;

         
            byte[] saltBytes = GenerateSalt(Options.MinSaltLength, Options.MaxSaltLength);

            // Alocar array que conterá bytes de salt e de texto simples.
            byte[] plainTextBytesWithSalt = new byte[plainTextBytes.Length + saltBytes.Length];
            // First, copy salt bytes.
            Array.Copy(saltBytes, plainTextBytesWithSalt, saltBytes.Length);

         
            Array.Copy(plainTextBytes, 0,
                        plainTextBytesWithSalt, saltBytes.Length,
                        plainTextBytes.Length);

            return plainTextBytesWithSalt;
        }

        /// <summary>
        /// Gera uma matriz contendo bytes criptograficamente fortes.
        /// </summary>
        /// <returns>
        /// Array of randomly generated bytes.
        /// </returns>
        /// <remarks>
        /// O tamanho do sal será definido aleatoriamente ou exatamente conforme especificado pelo Parâmetros 
        /// minSlatLen e maxSaltLen passados ​​para o construtor do objeto.
        /// Os primeiros quatro bytes do array salt conterão o comprimento do salt
        /// dividido em quatro partes de dois bits.
        /// </remarks>
        private byte[] GenerateSalt(int minSaltLen, int maxSaltLen)
        {
            int saltLen = 0;

            if (minSaltLen == maxSaltLen) saltLen = minSaltLen;
            else
                saltLen = GenerateRandomNumber(minSaltLen, maxSaltLen);

            byte[] salt = new byte[saltLen];

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            rng.GetNonZeroBytes(salt);

            //Divida o comprimento do salt (sempre um byte) em quatro partes de dois bits e
            //armazena essas peças nos primeiros quatro bytes do array salt.
            salt[0] = (byte)((salt[0] & 0xfc) | (saltLen & 0x03));
            salt[1] = (byte)((salt[1] & 0xf3) | (saltLen & 0x0c));
            salt[2] = (byte)((salt[2] & 0xcf) | (saltLen & 0x30));
            salt[3] = (byte)((salt[3] & 0x3f) | (saltLen & 0xc0));

            return salt;
        }

        private int GenerateRandomNumber(int minValue, int maxValue)
        {
     
            byte[] randomBytes = new byte[4];

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            int seed = ((randomBytes[0] & 0x7f) << 24) |
                        (randomBytes[1] << 16) |
                        (randomBytes[2] << 8) |
                        (randomBytes[3]);

          
            Random random = new Random(seed);

       
            return random.Next(minValue, maxValue + 1);
        }
        #endregion
    }
}
