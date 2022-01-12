using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SmartAdmin.AppServices.CtaAcesso.Interfaces;
using SmartAdmin.Domain.Entities.CtAcesso;
using SmartAdmin.Domain.Repositories.Interfaces.CtAcesso;
using SmartAdmin.Infra.Repositories.CtAcesso;

namespace SmartAdmin.AppServices.CtaAcesso
{
    public class CtaUsuarioService : ICtaUsuarioService 
    {
        private readonly ICtaUsuario _ctaUsuario;
        private readonly IVwCtaItensMenuGrupo _vwCtaItensMenuGrupo;

        public CtaUsuarioService(ICtaUsuario ctaUsuario,IVwCtaItensMenuGrupo vwCtaItensMenuGrupo)
        {
            _ctaUsuario = ctaUsuario;
            _vwCtaItensMenuGrupo = vwCtaItensMenuGrupo;
        }

        public int Count()
        {
            return _ctaUsuario.Count();
        }

        public int Count(Expression<Func<CtaUsuario, bool>> where)
        {
            throw new NotImplementedException();
        }

        public int Delete(CtaUsuario obj)
        {
            throw new NotImplementedException();
        }

        public int Delete(Expression<Func<CtaUsuario, bool>> where)
        {
            throw new NotImplementedException();
        }

        public CtaUsuario Find(Expression<Func<CtaUsuario, bool>> where)
        {
            return _ctaUsuario.Find(where);
        }

        public CtaUsuario FindBySQL(string sql)
        {
            throw new NotImplementedException();
        }

        public int GetCurrentTenatId()
        {
            throw new NotImplementedException();
        }

        public int Insert(CtaUsuario obj)
        {
            throw new NotImplementedException();
        }

        public IList<CtaUsuario> List(Expression<Func<CtaUsuario, bool>> where)
        {
            return _ctaUsuario.List(where);
        }

        public IList<CtaUsuario> ListAll()
        {
            return  _ctaUsuario.ListAll();     
        }

        public int Update(CtaUsuario obj)
        {
            throw new NotImplementedException();
        }

        public void FazAlgumaCoisa()
        {

        }

        int? IService<CtaUsuario>.Insert(CtaUsuario obj)
        {
            throw new NotImplementedException();
        }

        public List<VwCtaItensMenuGrupo> ListaItensMenu(Expression<Func<VwCtaItensMenuGrupo, bool>> where)
        {
            return _vwCtaItensMenuGrupo.List(where) as List<VwCtaItensMenuGrupo>;
        }
        
    }
}
