using System;

namespace SmartAdmin.Api.Dtos
{
    public class UserApp
    {
        public int IdTenant { get; set; }
        
        public int? IdColaborador { get; set; }

        public string NomeUsuario { get; set; }
        
        public string Email { get; set; }
        
        public string Senha { get; set; }

        public string Telefone { get; set; }

        public bool PrimeiroAcesso { get; set; }

        public int IdSituacaoCadastral { get; set; }

        public bool TelefoneConfirmado { get; set; }

        public bool EmailConfirmado { get; set; }

        /// <summary>
        /// Se preenchido, o usuário é bloqueado até a DataHoraFimBloqueio
        /// </summary>
        public bool Bloqueado { get; set; }
        
        
        
    }
}
