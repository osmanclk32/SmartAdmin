using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSPS.ComponentsApp.Extensions
{
    public static class StringExtensions
    {
        public static int ToInt(this string current)
        {
            int convertedValue;

            int.TryParse(current, out convertedValue);

            return convertedValue;
        }

        public static int ToInt(this string number, int defaultInt)
        {
            int resultNum = defaultInt;
            try
            {
                if (!string.IsNullOrEmpty(number))
                    resultNum = Convert.ToInt32(number);
            }
            catch
            {
            }
            return resultNum;
        }

        public static bool In(this string searchInString, string[] searchWhat)
        {
            bool contains = false;

            Array.ForEach(searchWhat, f => { if (searchInString.Contains(f)) { contains = true; } });

            return contains;
        }

        /// <summary>
        /// Remove caracteres usualmente utilizados em ataques sql injection
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ClearInjection(this string value)
        {
            value = value.Replace("'", String.Empty);
            value = value.Replace("\"", String.Empty);
            value = value.Replace("´", String.Empty);
            value = value.Replace(";", String.Empty);
            value = value.Replace("--", String.Empty);
            value = value.Replace("/", String.Empty);
            value = value.Replace("/*", String.Empty);
            value = value.Replace("*/", String.Empty);
            value = value.Replace("*/", String.Empty);

            value = value.Replace("--", string.Empty);
            value = value.Replace("drop", string.Empty).Replace("Drop", string.Empty).Replace("DROP", string.Empty);
            value = value.Replace("insert", string.Empty).Replace("Insert", string.Empty).Replace("INSERT", string.Empty);
            value = value.Replace("delete", string.Empty).Replace("Delete", string.Empty).Replace("DELETE", string.Empty);
            value = value.Replace("update", string.Empty).Replace("Update", string.Empty).Replace("UPDATE", string.Empty);
            value = value.Replace("select", string.Empty).Replace("Select", string.Empty).Replace("SELECT", string.Empty);
            value = value.Replace("union", string.Empty).Replace("Union", string.Empty).Replace("UNION", string.Empty);
            value = value.Replace("xp_cmdshell", string.Empty);

            return value;
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool HasValue(this string self)
        {
            var result = !string.IsNullOrEmpty(self);

            return result;
        }

        public static object CheckDbNull(this string obj)
        {
            object ret = DBNull.Value;

            if ( !obj.IsNullOrEmpty() )
            {
                if (string.IsNullOrEmpty(obj))
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

        public static string ToUnderscoreCase(this string str, bool isLower = false)
        {
            if (isLower)
            {
                return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
            }
            else
            {
                return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToUpper();
            }
        }

        public static string ClearMask(this string value)
        {
            if (value.IsNullOrEmpty())
            {
                return value;
            }

            //O padrão da expressão regular [^\w\@//] corresponde a qualquer caractere que não seja um caractere de palavra,um símbolo de @ ou uma barra /
            try
            {
                return Regex.Replace(value, @"[^\w\@//]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            //se atingir o tempo limite ao substituir caracteres inválidos, retorna vazio
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        public static string SafeToLower(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            return value.ToLower();
        }

        public static string SafeToUpper(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            return value.ToUpper();
        }
    }
}
