using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAdmin.Domain.Entities.Tenants
{


    [Table("silt_tenants")]
    public class SiltTenants : EntityBase, ITenat
    {
        public SiltTenants()
        {
        }

        [Key]
        [Column("ID_TENANT")]
        public int IdTenant { get; set; }

        [Column("RAZAO_SOCIAL")]
        public string RazaoSocial { get; set; }

        [Column("NOME_FANTASIA")]
        public string NomeFantasia { get; set; }

        [Column("CNPJ")]
        public string Cnpj { get; set; }

        [Column("ENDERECO")]
        public string Endereco { get; set; }

        [Column("NUMERO")]
        public string Numero { get; set; }

        [Column("BAIRRO")]
        public string Bairro { get; set; }

        [Column("COMPLEMENTO")]
        public string Complemento { get; set; }

        [Column("CEP")]
        public string Cep { get; set; }

        [Column("EMAIL")]
        public string Email { get; set; }

        [Column("TELEFONE")]
        public string Telefone { get; set; }

        [Column("CELULAR")]
        public string Celular { get; set; }

        [Column("DATA_INICIO_VIGENCIA")]
        public string DataInicioVigencia { get; set; }

        [Column("DATA_VALIDADE")]
        public string DataValidade { get; set; }

        [Column("SUB_DOMINIO")]
        public string SubDominio { get; set; }

        [Column("BLOQUEADO")]
        public string Bloqueado { get; set; }

        [Column("ID_CIDADE")]
        public int IdCidade { get; set; }

        [Column("ID_PLANO")]
        public int IdPlano { get; set; }

        [Column("schema")]
        public string Schema { get; set; }

        [NotMapped]
        public string UserId { get; set; }
        
        [NotMapped]
        public string Senha { get; set; }
    }
}
