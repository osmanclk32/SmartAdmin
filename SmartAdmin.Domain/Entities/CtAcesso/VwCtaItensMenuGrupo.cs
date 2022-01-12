using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SmartAdmin.Domain.Entities.CtAcesso 
{

 
   [Table("vw_cta_itens_menu_grupo")] 
   public class VwCtaItensMenuGrupo : EntityBase
    {	
      public VwCtaItensMenuGrupo ()
      {
      }

      [Key]
      [Column("id_grupo")]
      public int IdGrupo { get; set; }

      [Column("id_tenant")]
      public int IdTenant { get; set; }

      [Column("id_menu_pai")]
      public int IdMenuPai { get; set; }

      [Column("nivel")]
      public int Nivel { get; set; }

      [Column("ordem")]
      public int? Ordem { get; set; }

      [Column("id_menu")]
      public int IdMenu { get; set; }

      [Column("permite_alterar")]
      public string PermiteAlterar { get; set; }

      [Column("permite_excluir")]
      public string PermiteExcluir { get; set; }

      [Column("permite_consultar")]
      public string PermiteConsultar { get; set; }

      [Column("habilitado")]
      public string Habilitado { get; set; }

      [Column("action_name")]
      public string ActionName { get; set; }

      [Column("controller_name")]
      public string ControllerName { get; set; }

      [Column("area")]
      public string Area { get; set; }

      [Column("tags")]
      public string Tags { get; set; }

      [Column("titulo_menu")]
      public string TituloMenu { get; set; }

      [Column("descricao_menu")]
      public string DescricaoMenu { get; set; }

      [Column("imagem")]
      public string Imagem { get; set; }

      [Column("nome_grupo")]
      public string NomeGrupo { get; set; }

      [Column("permite_inserir")]
      public string PermiteInserir { get; set; }

   }

}
