using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using SqlKata;


namespace SmartAdmin.Identity.Models
{
   // [Table("cta_usuario")]
    public class ApplicationUser
    {
        
        [Key("id_usuario")]
        [Ignore]
        public int IdUsuario { get; set; }

        [Column("primeiro_acesso")]
        public bool PrimeiroAcesso { get; set; }

        [Column("id_situacao_cadastral")]
        public int IdSituacaoCadastral { get; set; }

        [Column("telefone_confirmado")]
        public bool TelefoneConfirmado { get; set; }

        /// <summary>
        /// Se preenchido, o usuário é bloqueado até a DataHoraFimBloqueio
        /// </summary>
        [Column("bloqueado")]
        public bool Bloqueado { get; set; }

        [Column("data_hora_fim_bloqueio")]
        public DateTimeOffset? DataHoraFimBloqueio { get; set; }

        [Column("qtde_falhas_acesso")]
        public int QtdeFalhasAcesso { get; set; }

        [Column("id_tenant")]
        public int IdTenant { get; set; }

        [Column("email_confirmado")]
        public bool EmailConfirmado { get; set; }

        [Column("id_colaborador")]
        public int? IdColaborador { get; set; }

        [Column("carimbo_concorrencia")]
        public string CarimboConcorrencia { get; set; }

        [Column("nome_usuario")]
        public string NomeUsuario { get; set; }

        /// <summary>
        /// Versão consistente do Nome de usuário para permitir uma pesquisa mais fácil
        /// </summary>
        [Column("nome_usuario_normalizado")]
        public string NomeUsuarioNormalizado { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("email_normalizado")]
        public string EmailNormalizado { get; set; }

        [Column("senha_hash")]
        public string SenhaHash{ get; set; }

        [Column("telefone")]
        public string Telefone { get; set; }

        [Column("carimbo_seguranca")]
        public string CarimboSeguranca { get; set; }

        [SqlKata.Ignore]
        internal List<Claim> Claims { get; set; }

        [SqlKata.Ignore]
        internal List<ApplicationRole> Roles { get; set; }

        [SqlKata.Ignore]
        internal List<UserLoginInfo> Logins { get; set; }

        [SqlKata.Ignore]
        internal List<ApplicationUserTokens> Tokens { get; set; }
    }
}
