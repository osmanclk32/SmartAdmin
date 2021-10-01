using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;

using Npgsql;
using SmartAdmin.Identity.Interfaces;

namespace SmartAdmin.Domain
{
    public class PostgresConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly string _connectionString;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        public PostgresConnectionFactory(string connectionString) => _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        /// <inheritdoc/>
        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var sqlConnection = new NpgsqlConnection(_connectionString);
            
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            
            await sqlConnection.OpenAsync();
            
            return sqlConnection;
        }
    }
}
