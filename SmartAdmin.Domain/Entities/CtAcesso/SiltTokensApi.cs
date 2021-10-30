
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartAdmin.Domain.Entities;

namespace SmartAdmin.Identity.Models
{

 
   [Table("silt_tokens_api")] 
   public class SiltTokensApi : EntityBase
    {	
      public SiltTokensApi ()
      {
      }

      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      [Column("id_token")]
      public int IdToken { get; set; }

      [Column("ativo")]
      public bool Ativo { get; set; }

      [Column("id_usuario")]
      public int IdUsuario { get; set; }

      [Column("expira_em")]
      public DateTime ExpiraEm { get; set; }

      [Column("foi_expirado")]
      public bool FoiExpirado { get; set; }

      [Column("revogado")]
      public bool Revogado { get; set; }

      [Column("data_criacao")]
      public DateTime DataCriacao { get; set; }

      [Column("refresh_token")]
      public string RefreshToken { get; set; }

      [Column("revogado_pelo_ip")]
      public string RevogadoPeloIp { get; set; }

      [Column("criado_pelo_ip")]
      public string CriadoPeloIp { get; set; }

      [Column("alterado_pelo_token")]
      public string AlteradoPeloToken { get; set; }

   }

}
