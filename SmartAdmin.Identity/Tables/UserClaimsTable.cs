using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dapper;

using SmartAdmin.Identity.Interfaces;

using SmartAdmin.Identity.Models;

namespace SmartAdmin.Identity.Tables
{
    internal class UserClaimsTable
    {
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

        public UserClaimsTable(IDatabaseConnectionFactory databaseConnectionFactory) => _databaseConnectionFactory = databaseConnectionFactory;

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user,int idTenant)
        {
            const string command = "SELECT * " +
                                   "FROM cta_usuario_claims " +
                                   "WHERE id_usuario = @id_usuario And id_tenant = @id_tenant;";

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
            {
                return (
                        await sqlConnection.QueryAsync<ApplicationUserClaims>(command, new { IdUsuario = user.IdUsuario,IdTenant = idTenant })
                    )
                    .Select(e => new Claim(e.ClaimType, e.ClaimValue))
                    .ToList(); ;
            }
        }
    }
}
