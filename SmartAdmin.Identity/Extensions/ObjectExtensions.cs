using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartAdmin.Identity.Extensions
{
    internal static class ObjectExtensions
    {
        internal static void ThrowIfNull<T>(this T @object, string paramName)
        {
            if (@object == null)
            {
                throw new ArgumentNullException(paramName, $"O parâmetro {paramName} não pode ser nulo.");
            }
        }
    }
}
