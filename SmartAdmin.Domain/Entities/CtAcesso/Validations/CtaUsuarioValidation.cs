using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Flunt.Validations;

namespace SmartAdmin.Domain.Entities.CtAcesso.Validations
{
    public class CtaUsuarioValidation : Contract<CtaUsuario>
    {
        public CtaUsuarioValidation(CtaUsuario usuario)
        {
            Requires()
                .IsNotNullOrEmpty(usuario.Login, "Login", "Login não informado");

            IsEmail(usuario.Email, "Email");
        }
    }
}
