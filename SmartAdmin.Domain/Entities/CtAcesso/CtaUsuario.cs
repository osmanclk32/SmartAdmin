using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAdmin.Domain.Entities.CtAcesso
{
    [Table("cta_usuario")]
    public class CtaUsuario : EntityBase
    {
         
        [Column("id_usuario")]
        [Key]
        public int IdUsuario { get; set; }

        [Column("id_tenant")]
        public int IdTenant { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("senha")]
        public string Senha { get; set; }

        [Column("administrador")]
        public string Administrador { get; set; }

        [Column("primeiro_acesso")]
        public string PrimeiroAcesso { get; set; }

        [Column("id_situacao_cadastral")]
        public int IdSituacaoCadastral { get; set; }

        [Column("id_colaborador")]
        public int IdColaborador { get; set; }
    }
}
