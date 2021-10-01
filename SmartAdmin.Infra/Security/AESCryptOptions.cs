//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// AESCrypt - criptografia e descriptografia de chave simétrica usando algoritmo AES / Rijndael (128, 192 e 256)
// 
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Security.Cryptography;

namespace SmartAdmin.Infra.Security
{
    public class AESCryptOptions
    {
        #region Private members
        // Não permita que o salt tenha mais de 255 bytes, porque temos apenas
        // 1 byte para armazenar seu comprimento.
        private static int MAX_ALLOWED_SALT_LEN = 255;

        // Não permita que o salt seja menor que 4 bytes, porque usamos o primeiro
        // 4 bytes de salt para armazenar seu comprimento.
        private static int MIN_ALLOWED_SALT_LEN = 4;

        // O valor de salt aleatório terá entre 4 e 8 bytes de comprimento.
        private static int DEFAULT_MIN_SALT_LEN = MIN_ALLOWED_SALT_LEN;
        private static int DEFAULT_MAX_SALT_LEN = 8;

        // Esses membros serão usados ​​para realizar a criptografia e a descriptografia.
        private ICryptoTransform encryptor = null;
        private ICryptoTransform decryptor = null;

        #endregion

        #region Constructor
        public AESCryptOptions()
        {
            PasswordHash = AESPasswordHash.SHA1;
            PasswordHashIterations = 1;
            MinSaltLength = 0;
            MaxSaltLength = 0;
            FixedKeySize = null;
            PaddingMode = PaddingMode.PKCS7;
        }

        public AESCryptOptions(AESPasswordHash passwordHash)
        {
            PasswordHash = passwordHash;
            PasswordHashIterations = 1;
            MinSaltLength = 0;
            MaxSaltLength = 0;
            FixedKeySize = null;
            PaddingMode = PaddingMode.PKCS7;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Tamanho da chave: normalmente é 128, 192 ou 256, dependendo do tamanho da senha em bits (16, 24 ou 32 respectivamente).
        /// O padrão é NULL, o que significa que será calculado rapidamente usando o comprimento de bits da senha.
        /// </summary>
        public int? FixedKeySize { get; set; }

        /// <summary>
        // Modo de hash da senha: Nenhum, MD5 ou SHA1.
        /// SHA1 é o modo recomendado para a maioria dos cenários.
        /// </summary>
        public AESPasswordHash PasswordHash { get; set; }

        /// <summary>
        /// iterações de senha - não usado quando [PasswordHash] é definido como [AESPasswordHash.None]
        /// </summary>
        public int PasswordHashIterations { get; set; }

        /// <summary>
        /// Comprimento mínimo do salt: deve ser igual ou menor que MaxSaltLength.
        /// O padrão é 0.
        /// </summary>
        public int MinSaltLength { get; set; }

        /// <summary>
        /// Comprimento máximo do salt: deve ser igual ou maior que MinSaltLength.
        /// O padrão é 0, o que significa que nenhum sal será usado.
        /// </summary>
        public int MaxSaltLength { get; set; }

        /// <summary>
        /// Valor de salt usado para hashing de senha durante a geração de chave.
        /// NOTA: Este não é o mesmo que o salt que usaremos durante a criptografia.
        /// Este parâmetro pode ser qualquer string (defina-o como NULL para nenhum salt de hash de senha): o padrão é NULL.
        /// </summary>
        public string PasswordHashSalt { get; set; }

        /// <summary>
        /// Define o modo de preenchimento (o padrão é PaddingMode.PKCS7)
        /// </summary>
        public PaddingMode PaddingMode { get; set; }
        #endregion

    }
}
