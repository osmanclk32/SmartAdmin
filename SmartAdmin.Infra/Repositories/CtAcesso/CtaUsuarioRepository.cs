using DapperExt;
using SmartAdmin.Domain.Entities.CtAcesso;
using SmartAdmin.Domain.Repositories;
using SmartAdmin.Domain.Repositories.Interfaces.CtAcesso;

namespace SmartAdmin.Infra.Repositories.CtAcesso
{
    public class CtaUsuarioRepository : BaseRepository<CtaUsuario>, ICtaUsuario
    {
        public CtaUsuarioRepository(IDapperDbContext context) : base(context)
        {
        }
    }

}
