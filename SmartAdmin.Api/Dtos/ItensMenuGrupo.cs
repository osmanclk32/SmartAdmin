using System.Collections.Generic;
using Newtonsoft.Json;

namespace SmartAdmin.Api.Dtos
{
    public class SmartNavigationMenu
    {
        public SmartNavigationMenu()
        {
            SubMenus = new List<SmartNavigationMenu>();
        }

        public int? Key { get; set; }

        public int? ParentKey { get; set; }

        /// <summary>
        /// Título do Menu
        /// </summary>
        public string TituloMenu { get; set; }

        /// <summary>
        /// Area a qual o Menu pertence
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Nome da Action que o item de Menu deve executar
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Nome do Controller que o menu deve chamar
        /// </summary>
        public string ControllerName { get; set; }
        

        public string Imagem { get; set; }

        /// <summary>
        /// Indica se Menu está ou não ativo
        /// </summary>
        public bool Active { get; set; }

      
        /// <summary>
        /// Nivel hierarquico do menu
        /// </summary>
        public int Nivel { get; set; }

        /// <summary>
        /// Tags para pesquisar um determinado item na barra de menus
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Lista de Menus Filhos
        /// </summary>
        [JsonProperty]
        public List<SmartNavigationMenu> SubMenus;

    }
}
