
using SqlKata;

namespace SmartAdmin.Identity.Models
{


   public class ApplicationUserRole 
   {	
      public ApplicationUserRole ()
      {
      }

      [Key]
      [Column("id_grupo")]
      public int IdGrupo { get; set; }

      [Key]
      [Column("id_usuario")]
      public int IdUsuario { get; set; }

      [Column("id_tenant")]
      public int IdTenant { get; set; }

    }

}
