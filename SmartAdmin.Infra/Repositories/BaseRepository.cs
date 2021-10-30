using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Dapper;

using DapperExt;

using Npgsql;

using SmartAdmin.Domain.Entities;
using SmartAdmin.Domain.Entities.Tenants;
using SmartAdmin.Domain.Repositories.Interfaces;
using SmartAdmin.Infra.Data;

namespace SmartAdmin.Infra.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : EntityBase
    {
        private readonly string _connectionString;
        private readonly string _schema;
        private readonly int _idTenant;
        private readonly IDbSession _session;
        private readonly IDapperDbContext _context;

        public BaseRepository(IDapperDbContext context)
        {
            _context = context;
           
           // SetTenant(_idTenant);
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public int Count(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public int Delete(T obj)
        {
            throw new NotImplementedException();
        }

        public int Delete(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            return _context.Get<T>(where);
        }

        public T FindBySQL(string sql)
        {
            throw new NotImplementedException();
        }

       

        public int Update(T obj)
        {
            throw new NotImplementedException();
        }

        public IList<T> List(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IList<T> ListAll()
        {
            try
            {
                return _context.GetList<T>().AsList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int GetCurrentTenatId()
        {
            return _idTenant;
        }

        private void SetTenant(int idTenant)
        {
            try
            {
                var sql = $"set app.current_tenant = '{idTenant}';";
                              
                _session.Connection.Execute(sql);

            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public int? Insert(T obj, string sequenceName="")
        {
            return _context.Insert(obj, sequenceName);
        }
    }
}
