using System;
using System.Data;
using System.Threading.Tasks;
using Npgsql;
using Dapper;
using SmartAdmin.Identity.Interfaces;

namespace SmartAdmin.Identity.ConnectionFactories
{
    /// <summary>
    ///  Fabricador de conexões PostGres
    /// </summary>
    public class PostgresConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly string _connectionString;

        /// <summary>
        /// 
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
