using System.Collections;
using System.Dynamic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Black.Infrastructure.Extensions;

public static class TypeConversionExtensions
{
    /// <summary>
    /// Converts object to string.
    /// </summary>
    /// <param name="obj">Object Value</param>
    public static string ToStr(this object obj)
    {
        return obj == null ? "" : obj.ToString();
    }

    /// <summary>
    /// Converts object to boolean.
    /// </summary>
    /// <param name="obj">Object Value</param>
    public static Boolean ToBoolean(this object obj)
    {
        return Boolean.TryParse(obj.ToStr(), out var result) ? result : default;
    }

    /// <summary>
    /// Converts object to nullable -datetime?- object.
    /// </summary>
    /// <param name="obj">Object Value</param>
    public static DateTime? ToDate(this object obj)
    {
        return DateTime.TryParse(obj.ToStr(), out var result) ? result.Date : (DateTime?)null;
    }

    /// <summary>
    /// Converts object to nullable -datetime?- object.
    /// </summary>
    /// <param name="obj">Object Value</param>
    public static DateTime? ToDateTime(this object obj)
    {
        return DateTime.TryParse(obj.ToStr(), out var result) ? result : (DateTime?)null;
    }

    /// <summary>
    /// Converts object to decimal.
    /// </summary>
    /// <param name="obj">Object Value</param>
    public static decimal ToDecimal(this object obj)
    {
        return decimal.TryParse(obj.ToStr(), out var i) ? i : 0;
    }

    /// <summary>
    /// Converts object to double.
    /// </summary>
    /// <param name="obj">Object Value</param>
    public static double ToDouble(this object obj)
    {
        return double.TryParse(obj.ToStr(), out var i) ? i : 0;
    }

    /// <summary>
    /// Converts string to long.
    /// </summary>
    /// <param name="s">String Value</param>
    public static long ToLong(this string s)
    {
        return long.TryParse(s, out var i) ? i : 0;
    }

    /// <summary>
    /// Converts object to int.
    /// </summary>
    /// <param name="obj">Object Value</param>
    public static int ToInt(this object obj)
    {
        return int.TryParse(obj.ToStr(), out var i) ? i : 0;
    }

    /// <summary>
    /// Converts string to int.
    /// </summary>
    /// <param name="s">String Value</param>
    public static int ToInt(this string s)
    {
        return int.TryParse(s, out var i) ? i : 0;
    }

    /// <summary>
    /// Converts decimal to double.
    /// </summary>
    /// <param name="source">Decimal Value</param>
    public static double ToDouble(this decimal source)
    {
        return Convert.ToDouble(source, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Converts string to datetime object.
    /// </summary>
    /// <param name="s">String Value</param>
    public static DateTime ToDate(this string s)
    {
        return DateTime.TryParse(s, out var result) ? result.Date : default;
    }

    /// <summary>
    /// Converts Turkish letters to upper agnostic culture.
    /// </summary>
    /// <param name="s">String Value</param>
    public static string ToUpperCultureAgnostic(this string s)
    {
        var result = string.Empty;
        if (string.IsNullOrWhiteSpace(s)) return result;
        foreach (var c in s.Trim().ToUpperInvariant())
        {
            if (char.IsWhiteSpace(c)) continue;
            switch (c)
            {
                case 'Ğ':
                    result += 'G';
                    break;
                case 'Ü':
                    result += 'U';
                    break;
                case 'Ş':
                    result += 'S';
                    break;
                case 'İ':
                    result += 'I';
                    break;
                case 'Ö':
                    result += 'O';
                    break;
                case 'Ç':
                    result += 'C';
                    break;
                default:
                    result += c;
                    break;
            }
        }
        return result;
    }

    /// <summary>
    /// Describes date as a long value.
    /// </summary>
    /// <param name="dateTime">DateTimeOffset</param>
    public static long ToDateAsLong(this DateTimeOffset dateTime)
    {
        return long.Parse(
            $"{dateTime.Year.ToString("0000")}{dateTime.Month.ToString("00")}{dateTime.Day.ToString("00")}",
            NumberStyles.Integer, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Describes date as a long value.
    /// </summary>
    /// <param name="dateTime">DateTime</param>
    public static long ToDateAsLong(this DateTime dateTime)
    {
        return long.Parse(
            $"{dateTime.Year.ToString("0000")}{dateTime.Month.ToString("00")}{dateTime.Day.ToString("00")}",
            NumberStyles.Integer, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Creates ExpandoObject(dynamic object for json,xml) from simple object.
    /// </summary>
    /// <param name="model">DateTime</param>
    public static ExpandoObject ToExpandoObject(this object model)
    {
        if (model is ExpandoObject expandoObject)
        {
            return expandoObject;
        }

        IDictionary<string, object> expando = new ExpandoObject();
        Type modelType = model.GetType();
        TypeInfo modelTypeInfo = modelType.GetTypeInfo();
        PropertyInfo[] properties = modelTypeInfo.GetProperties();

        foreach (PropertyInfo property in properties)
        {
            object obj = property.GetValue(model);

            if (obj != null)
            {
                Type objectType = obj.GetType();
                if (objectType.IsAnonymousType())
                {
                    obj = obj.ToExpandoObject();
                }
                else if (obj is ICollection collection)
                {
                    var list = new List<dynamic>();
                    foreach (var item in collection)
                    {
                        ExpandoObject keyValuePair = item.ToExpandoObject();
                        list.Add(keyValuePair);
                    }
                    obj = list;
                }
            }

            expando.Add(property.Name, obj);
        }

        return (ExpandoObject)expando;
    }

    /// <summary>
    /// Finds is type anonymous or not.
    /// </summary>
    /// <param name="type">Type</param>
    public static bool IsAnonymousType(this Type type)
    {
        bool hasCompilerGeneratedAttribute = type.GetTypeInfo()
            .GetCustomAttributes(typeof(CompilerGeneratedAttribute), false)
            .Any();

        bool nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
        bool isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

        return isAnonymousType;
    }

    /// <summary>
    /// Converts byte array to string.
    /// </summary>
    /// <param name="value">Byte Array Value</param>
    public static string ConvertByteArrayToString(byte[] value)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < value.Length; i++)
        {
            builder.Append(value[i].ToString("x2"));
        }
        return builder.ToString();
    }

    /// <summary>
    /// Checks is string valid guid or not.
    /// </summary>
    /// <param name="value">Guid as a string</param>
    public static bool IsGuid(string value)
    {
        Guid x;
        return Guid.TryParse(value, out x);
    }


    /// <summary>
    /// Converts long value to string.
    /// </summary>
    /// <param name="value">Guid as a long</param>
    public static Guid ToGuid(this long value)
    {
        string longValue = value.ToString();
        string guidPart3 = "0000", guidPart4 = "0000", guidPart5 = "000000000000";
        if (longValue.Length <= 12)
        {
            guidPart5 = longValue.PadLeft(12, '0');
        }
        else if (longValue.Length <= 16)
        {
            guidPart5 = longValue.Substring(longValue.Length - 12).PadLeft(12, '0');
            guidPart4 = longValue.Substring(0, longValue.Length - 12).PadLeft(4, '0');
        }
        else if (longValue.Length <= 19)
        {
            guidPart5 = longValue.Substring(7).PadLeft(12, '0');
            guidPart4 = longValue.Substring(3, 4).PadLeft(4, '0');
            guidPart3 = longValue.Substring(0, 3).PadLeft(4, '0');
        }
        string guidValue = $"00000000-0000-{guidPart3}-{guidPart4}-{guidPart5}";
        return new Guid(guidValue);
    }

}
