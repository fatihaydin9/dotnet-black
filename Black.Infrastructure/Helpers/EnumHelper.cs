using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace Black.Infrastructure.Helpers;

public static class EnumHelper
{
    public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
    {
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
        return (attributes.Length > 0) ? (T)attributes[0] : null;
    }

    public static string GetEnumDescription(this Enum columnAttribute)
    { 
        var description = columnAttribute.GetType()
            .GetMember(columnAttribute.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>().Name;
        return description;
    }
}