using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SmartAdmin.Identity.Interfaces;
using SmartAdmin.Identity.Models;

namespace SmartAdmin.Identity.Tables
{
    internal class UserRolesTable
    {
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

        public UserRolesTable(IDatabaseConnectionFactory databaseConnectionFactory) => _databaseConnectionFactory = databaseConnectionFactory;

        public async Task<IEnumerable<ApplicationRole>> GetRolesAsync(ApplicationUser user,int idTenant)
        {
            IEnumerable<ApplicationRole> roles = null;

            const string command = "SELECT r.id_grupo AS IdGrupo, r.nome_grupo AS NomeGrupo, ur.id_tenant as IdTenant " +
                                   "FROM cta_grupo AS r " +
                                   "INNER JOIN cta_usuario_grupo AS ur ON ur.id_grupo = r.id_grupo " +
                                   "WHERE ur.id_usuario = @id_usuario AND ur.id_tenant = @id_tenant;";

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
            {
                roles = await sqlConnection.QueryAsync<ApplicationRole>(command, new
                {
                    id_usuario = user.IdUsuario,
                    id_tenant = idTenant
                });
            }

            return roles;
        }
    }
}
