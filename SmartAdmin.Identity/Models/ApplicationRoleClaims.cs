

using SqlKata;

namespace SmartAdmin.Identity.Models
{


   public class ApplicationRoleClaims 
   {	
      public ApplicationRoleClaims ()
      {
      }

      [Key]
      [Column("id_grupo_claim")]
      public int IdGrupoClaim { get; set; }

      [Column("id_grupo")]
      public int? IdGrupo { get; set; }

      [Column("id_tenant")]
      public int IdTenant { get; set; }

      [Column("claim_type")]
      public string ClaimType { get; set; }

      [Column("claim_value")]
      public string ClaimValue { get; set; }

   }

}
