using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSPS.ComponentsApp.Extensions
{
    public static class DynamicExtensions
    {
        //public static dynamic DefaultIfNull<T>(this T item)
        //{
        //    if (item != null)
        //        return item;

        //    var type = typeof(T);

        //    if (type.IsArray)
        //    {
        //        return Array.CreateInstance(type.GetElementType(), 0);
        //    }

        //    if (type.GetConstructor(Type.EmptyTypes) == null)
        //    {
        //        var paramCount = type.GetConstructors().Min(construct => construct.GetParameters().Count());
        //        var constructorToUse = type.GetConstructors().Where(construct => construct.GetParameters().Count() == paramCount).First();

        //        var paramNullList = new object[paramCount];

        //        var parameters = constructorToUse.GetParameters();
        //        for (int i = 0; i < paramCount; i++)
        //        {
        //            paramNullList[i] = parameters[i].ParameterType.IsValueType ? Activator.CreateInstance(parameters[i].ParameterType) : null;
        //        }

        //        return Activator.CreateInstance(type, paramNullList);
        //    }

        //    return Activator.CreateInstance(type);
       // }
    }
}
