
using DapperExt;
using SmartAdmin.Domain.Entities.CtAcesso;
using SmartAdmin.Domain.Repositories.Interfaces.CtAcesso;

namespace SmartAdmin.Infra.Repositories.CtAcesso
{

    public class VwCtaItensMenuGrupoRepository : BaseRepository<VwCtaItensMenuGrupo>, IVwCtaItensMenuGrupo
    {
        public VwCtaItensMenuGrupoRepository(IDapperDbContext context) : base(context)
        {
        }

		
    }
}