using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DapperExt;

namespace SmartAdmin.Infra.Data
{
    public class DbContext : DapperDbContext
    {
        public DbContext(string connectionString, DbProviderFactory dbProviderFactory, string schema = "siltec", string dataBaseDialect = "PostgreSQL",  int tenant = 0) 
        : base(connectionString, dbProviderFactory, schema, dataBaseDialect,  tenant)
        {

        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
