using Newtonsoft.Json;
using System.Text;

namespace Black.Infrastructure.Helpers.HttpHelper.Concrete;

public class JsonContent : StringContent
{
    /// <summary>
    /// Serializes object.
    /// </summary>
    public JsonContent(object obj) :
        base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
    { }
}
