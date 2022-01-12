using System.Collections.Generic;
using System.Linq.Expressions;
using System;

using SmartAdmin.Domain.Entities.CtAcesso;

namespace SmartAdmin.AppServices.CtaAcesso.Interfaces
{
    public interface ICtaUsuarioService : IService<CtaUsuario>
    {
        List<VwCtaItensMenuGrupo> ListaItensMenu(Expression<Func<VwCtaItensMenuGrupo, bool>> where);
    }
}
