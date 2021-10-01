
using Dapper;

using DapperExt;
using Npgsql;

using SmartAdmin.Domain.Entities.Tenants;
using SmartAdmin.Domain.Repositories.Interfaces;

namespace SmartAdmin.Infra.Repositories.Tenants
{
    public interface ISiltTenantsRepository : IBaseRepository<SiltTenants>
    {
        
        void SetCurrentTenant(int idTenant);
    }

    public class SiltTenantsRepository : BaseRepository<SiltTenants>, ISiltTenantsRepository
    {
        private readonly IDapperDbContext _context;

        public SiltTenantsRepository(IDapperDbContext context) : base(context)
        {
            _context = context;
        }
        
        public void SetCurrentTenant(int idTenant)
        {

            try
            {

                _context.Execute($"set app.current_tenant = '{idTenant}';");
                
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }

}
