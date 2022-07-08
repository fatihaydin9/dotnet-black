using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Black.Infrastructure.Extensions;

public static class StringExtension
{
    /// <summary>
    /// Checks string is valid json or not
    /// </summary>
    /// <param name="text">Json Text</param>
    public static bool IsValidJson(this string text)
    {
        text = text.Trim();
        if ((text.StartsWith("{") && text.EndsWith("}")) || //For object
            (text.StartsWith("[") && text.EndsWith("]"))) //For array
        {
            try
            {
                var obj = JToken.Parse(text);
                return true;
            }
            catch (JsonReaderException jex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
