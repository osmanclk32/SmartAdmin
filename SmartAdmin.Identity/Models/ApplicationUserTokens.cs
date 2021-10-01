using SqlKata;

namespace SmartAdmin.Identity.Models
{ 
   public class ApplicationUserTokens 
   {	
      public ApplicationUserTokens ()
      {
      }

      [Key]
      [Column("id_usuario")]
      public int IdUsuario { get; set; }

      [Key]
      [Column("provedor_login")]
      public string ProvedorLogin { get; set; }

      [Key]
      [Column("nome")]
      public string Nome { get; set; }

      [Column("valor")]
      public string Valor { get; set; }

      [Column("ID_TENANT")]
      public int IdTenant { get; set; }

   }

}
