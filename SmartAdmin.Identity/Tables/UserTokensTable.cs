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
    internal class UserTokensTable
    {
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

        public UserTokensTable(IDatabaseConnectionFactory databaseConnectionFactory) => _databaseConnectionFactory = databaseConnectionFactory;

        public async Task<IEnumerable<ApplicationUserTokens>> GetTokensAsync(int userId,int idTenant)
        {
            const string command = "SELECT * " +
                                   "FROM cta_usuario_tokens " +
                                   "WHERE id_usuario = @id_usuario AND id_tenant = @id_tenant;";

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
            {
                return await sqlConnection.QueryAsync<ApplicationUserTokens>(command, new
                {
                    IdUsuario = userId,
                    IdTenant = idTenant
                });
            }
        }
    }
}
