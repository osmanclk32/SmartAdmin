using System;
using System.Collections.Generic;
using System.Text;

namespace CSPS.ComponentsApp.Extensions
{
    public static class ObjectExtensions
    {
        public static T CastTo<T>(this object obj) => (T)obj;
        public static T As<T>(this object obj) where T : class => obj as T;

        public static object CheckDbNull(this object obj)
        {
            object ret = DBNull.Value;

            if (obj != null)
            {
                if (string.IsNullOrEmpty(obj.ToString()))
                {
                    ret = DBNull.Value;
                }
                else
                {
                    ret = obj;
                }

            }
            else
            {
                ret = DBNull.Value;
            }

            return ret;
        }

        //public static TOutput Decode<TInput, TOutput>(this TInput expression, params Tuple<TInput, TOutput>[] searchResultPairs)  => DecodeWithDefault(expression, default(TOutput), searchResultPairs);

        //public static TOutput DecodeWithDefault<TInput, TOutput>(TInput expression, TOutput defaultValue, params Tuple<TInput, TOutput>[] searchResultPairs)
        //{
        //    foreach (var searchResultPair in searchResultPairs)
        //    {
        //        if ((expression == null && searchResultPair.Item1 == null)
        //            || (expression != null && expression.Equals(searchResultPair.Item1)))
        //        {
        //            return searchResultPair.Item2;
        //        }
        //    }

        //    return defaultValue;
        //}

        public static object DbNullIfNull(this object obj)
        {
            if (obj == null  )
                return DBNull.Value;
            else if ( obj.GetType() == typeof(string) && obj.ToString().IsNullOrEmpty() )
            {
                return DBNull.Value;
            }
            else
                return obj;
        }

        public static object DbNullIfZero(this object obj)
        {
            if (obj == null || Convert.ToInt32(obj) == 0)
                return DBNull.Value;
            else
                return obj;
        }


        public static bool IsNullOrDefault<T>(this Nullable<T> value) where T : struct
        {
            return default(T).Equals(value.GetValueOrDefault());
        }

        public static bool IsValue<T>(this Nullable<T> value, T valueToCheck) where T : struct
        {
            return valueToCheck.Equals((value ?? valueToCheck));
        }
    }
}
