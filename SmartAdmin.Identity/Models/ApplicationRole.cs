using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace SmartAdmin.Identity.Models
{
    [Table("cta_grupo")]
    public class ApplicationRole
    {
        public ApplicationRole()
        {
        }

        [Key]
        [Column("id_grupo")]
        public int IdGrupo { get; set; }

        [Column("id_tenant")]
        public int IdTenant { get; set; }

        [Column("nome_grupo")]
        public string NomeGrupo { get; set; }

        internal List<Claim> Claims { get; set; }
    }
}
