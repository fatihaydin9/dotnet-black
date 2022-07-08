using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Black.Infrastructure.Objects;

public class ApiResponse
{
    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public string[] Messages { get; set; }
    public long Id { get; set; }
    public object Data { get; set; }
    public ApiResponse()
    {

    }
    public ApiResponse(int code, bool isSuccess, string[] messages)
    {
        this.StatusCode = code;
        this.IsSuccess = isSuccess;
        this.Messages = messages;
    }

    public ApiResponse(int code, bool isSuccess, string[] messages, object data)
    {
        this.StatusCode = code;
        this.IsSuccess = isSuccess;
        this.Messages = messages;
        this.Data = data;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
    }
}