using System;
using System.Data;
using Npgsql;

namespace SmartAdmin.Infra.Data
{
    public sealed class DbSession : IDbSession, IDisposable
    {
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; set; }
        public string Schema { get; set; }

        public DbSession(string connectionString,string schema)
        {
            Connection = new NpgsqlConnection(connectionString);
            Schema = schema;

            Connection.Open();           
        }

        public void Dispose() => Connection?.Dispose();
    }
}
