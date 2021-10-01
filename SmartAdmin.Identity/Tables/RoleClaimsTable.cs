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
    internal class RoleClaimsTable
    {
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

        public RoleClaimsTable(IDatabaseConnectionFactory databaseConnectionFactory) =>
            _databaseConnectionFactory = databaseConnectionFactory;

        public async Task<IList<Claim>> GetClaimsAsync(int roleId, int idTenant)
        {
            const string command = "SELECT * " +
                                   "FROM cta_grupo_claims " +
                                   "WHERE id_grupo = @id_grupo AND id_tenant = @id_tenant;";

            IEnumerable<ApplicationRoleClaims> roleClaims = new List<ApplicationRoleClaims>();

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
            {
                return (
                        await sqlConnection.QueryAsync<ApplicationRoleClaims>(command,
                            new {IdGrupo = roleId, IdTenant = idTenant})
                    )
                    .Select(x => new Claim(x.ClaimType, x.ClaimValue))
                    .ToList();

            }
        }
    }
}
