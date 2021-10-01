using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace CSPS.ComponentsApp.Extensions
{
    public static class EnumExtensions
    {
        public static string GetEnumValueFirstPos(this Enum enumValue)
        {
            Type enumType = enumValue.GetType();

            MemberInfo[] memberInfos = enumType.GetMember(enumValue.ToString());

            if (memberInfos != null && memberInfos.Length > 0)
            {
                object[] attrs = memberInfos[0].GetCustomAttributes(typeof(EnumMemberAttribute), false);

                if (attrs != null && attrs.Length > 0 && ((EnumMemberAttribute)attrs[0]).Value != null)
                {
                    return ((EnumMemberAttribute)attrs[0]).Value.Substring(0, 1);
                }
            }

            return enumValue.ToString().Substring(0, 1);
        }
    }
}
