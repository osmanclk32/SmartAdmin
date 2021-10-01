using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartAdmin.Api.Security
{

    /// <summary>
    ///  Classe utilizado na chamada de "/api/Login" para obtenção do Token
    /// </summary>
    public class AccessCredentials
    {
       
        public string UserName { get; set; }


        public string Email { get; set; }

        /// <summary>
        /// Senha de acesso
        /// </summary>
        public string AccessKey { get; set; }


        public bool RememberMe { get; set; }

        /// <summary>
        /// Token de atualização para se utilizado quando o Token principal expirar
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        ///  Forma de obtenção de acesso de Token para consumo da Api, podendo ser:
        ///  passaword (Token principal) e refresh_token(Token de atualização)
        /// </summary>
        public string GrantType { get; set; }

       
    }
}
