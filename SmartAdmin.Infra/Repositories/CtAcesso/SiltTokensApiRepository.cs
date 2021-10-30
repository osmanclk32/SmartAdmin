
using DapperExt;
using SmartAdmin.Domain.Repositories.Interfaces.CtAcesso;
using SmartAdmin.Identity.Models;

namespace SmartAdmin.Infra.Repositories.CtAcesso
{
    public class SiltTokensApiRepository : BaseRepository<SiltTokensApi>, ISiltTokensApi
    {
        public SiltTokensApiRepository(IDapperDbContext context) : base(context)
        {
        }
    }
}
