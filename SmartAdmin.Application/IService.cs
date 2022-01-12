using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SmartAdmin.Domain.Entities;

namespace SmartAdmin.AppServices
{
    public interface IService<T> where T : EntityBase
    {
        IList<T> List(Expression<Func<T, bool>> where);

        T Find(Expression<Func<T, bool>> where);

        int? Insert(T obj);

        int Update(T obj);

        int Delete(T obj);

        int Delete(Expression<Func<T, bool>> where);

        int Count();

        int Count(Expression<Func<T, bool>> where);

        T FindBySQL(string sql);

        int GetCurrentTenatId();
    }
}
