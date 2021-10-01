using System.Data;

namespace SmartAdmin.Infra.Data
{
    public interface IDbSession
    {
         IDbConnection Connection { get; }
         IDbTransaction Transaction { get; set; }
         string Schema { get; set; }   
    }
}
