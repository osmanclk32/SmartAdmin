using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CSharp.RuntimeBinder;

using SmartAdmin.Identity.Interfaces;

using SqlKata;
using SqlKata.Compilers;

namespace SmartAdmin.Identity.Helpers
{
    public static class BuildSql
    {
        private static PostgresCompiler compiler = new PostgresCompiler();

        public static SqlResult Insert(string tableName, object entityToInsert, bool returnId = false)
        {
            var query = new Query(tableName).AsInsert(entityToInsert, true);

            var sqlResult = compiler.Compile(query);

            return sqlResult;

        }

        public static string Update(string tableName, object entityToUpdate,string colWhere, object valueWhere )
        {
            var query = new Query(tableName).Where(colWhere,valueWhere).AsUpdate(entityToUpdate);

            
            SqlResult result = compiler.Compile(query);

            return result.Sql;
        }

        //public static void BuildSelect(StringBuilder sb, IEnumerable<PropertyInfo> props)
        //{
        //    var propertyInfos = props as IList<PropertyInfo> ?? props.ToList();
        //    var addedAny = false;
        //    for (var i = 0; i < propertyInfos.Count(); i++)
        //    {
        //        if (propertyInfos.ElementAt(i).GetCustomAttributes(true).Any(attr => attr.GetType().Name == typeof(IgnoreSelectAttribute).Name || attr.GetType().Name == typeof(NotMappedAttribute).Name)) continue;

        //        if (addedAny)
        //            sb.Append(",");
        //        sb.Append(GetColumnName(propertyInfos.ElementAt(i)));
        //        //if there is a custom column name add an "as customcolumnname" to the item so it maps properly
        //        if (propertyInfos.ElementAt(i).GetCustomAttributes(true).SingleOrDefault(attr => attr.GetType().Name == typeof(ColumnAttribute).Name) != null)
        //            sb.Append(" as " + Encapsulate(propertyInfos.ElementAt(i).Name));
        //        addedAny = true;

        //    }
        //}
    }



}
