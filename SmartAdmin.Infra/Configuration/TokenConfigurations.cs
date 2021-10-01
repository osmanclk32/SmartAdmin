using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartAdmin.Infra.Configuration
{
    public class TokenConfigurations
    {
        public string Audience { get; set; }

        /// <summary>
        /// Emitente do Token
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Validade em horas do Token
        /// </summary>
        public int Hours { get; set; }


        /// <summary>
        /// Validade em minutos do Token
        /// </summary>
        public int Minutes { get; set; }


        /// <summary>
        /// Validade em segundos do Token
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// Tempo máximo de validade do refresh token
        /// </summary>
        public int FinalExpiration { get; set; }
    }
}
