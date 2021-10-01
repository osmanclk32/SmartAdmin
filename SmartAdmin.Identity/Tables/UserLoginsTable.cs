using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;
using SmartAdmin.Identity.Interfaces;
using SmartAdmin.Identity.Models;

namespace SmartAdmin.Identity.Tables
{
    internal class UserLoginsTable
    {
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

        public UserLoginsTable(IDatabaseConnectionFactory databaseConnectionFactory) => _databaseConnectionFactory = databaseConnectionFactory;

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user, int idTenant)
        {
            const string command = "SELECT * " +
                                   "FROM cta_usuario_logins " +
                                   "WHERE id_usaurio = @id_usuario And id_tenant = @id_tenant;";

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
            {
                return (
                    await sqlConnection.QueryAsync<ApplicationUserLogins>(command, new { IdUsuario = user.IdUsuario, IdTenant = idTenant })
                )
                .Select(x => new UserLoginInfo(x.ProvedorLogin, x.ChaveProvedor, x.NomeProvedor))
                .ToList(); ;
            }
        }

        public async Task<ApplicationUser> FindByLoginAsync(string loginProvider, string providerKey, int idTentant)
        {
            string[] command =
            {
                "SELECT id_usuario " +
                "FROM cta_usuario_logins " +
                "WHERE login_provider = @LoginProvider AND provider_key = @ProviderKey AND id_tenant = @id_tenant;"
            };

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
            {
                var userId = await sqlConnection.QuerySingleOrDefaultAsync<int>(command[0], new
                {
                    LoginProvider = loginProvider,
                    ProviderKey = providerKey,
                    IdTentant = idTentant
                });

                if (userId == null)
                {
                    return null;
                }

                command[0] = "SELECT * " +
                             "FROM cta_usuario " +
                             "WHERE id_usuario = @id_usuario  AND id_tenant = @id_tenant;";

                return await sqlConnection.QuerySingleAsync<ApplicationUser>(command[0], new { IdUsuario = userId, IdTentant = idTentant });
            }
        }
    }
}
