using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartAdmin.Identity.Models
{
    public interface IAppTenant
    {
        int IdTenant { get; set; }
    }

    public class ApplicationTenant : IAppTenant
    {
        public int IdTenant { get; set; }

        public string RazaoSocial { get; set; }
        
        public string NomeFantasia { get; set; }

        public string Cnpj { get; set; }

        public string Endereco { get; set; }

        public string Numero { get; set; }

        public string Bairro { get; set; }

        public string Complemento { get; set; }

        public string Cep { get; set; }

        public string Email { get; set; }
        
        public string Telefone { get; set; }

        public string Celular { get; set; }
        
        public string DataInicioVigencia { get; set; }
        
        public string DataValidade { get; set; }

        public string SubDominio { get; set; }

        public string Bloqueado { get; set; }
        
        public int IdCidade { get; set; }

        public int IdPlano { get; set; }

        public string Schema { get; set; }

        public string UserId { get; set; }

        public string Senha { get; set; }
    }
}
