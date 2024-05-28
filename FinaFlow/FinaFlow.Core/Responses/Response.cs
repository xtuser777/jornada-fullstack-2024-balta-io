using System.Text.Json.Serialization;

namespace FinaFlow.Core.Responses;

public class Response<TData>
{
    private int _code = Configuration.DefaultStatusCode;

    [JsonConstructor]
    public Response() => _code = Configuration.DefaultStatusCode;

    public Response(TData? data, int code = Configuration.DefaultStatusCode, string? message = null)
    {
        Data = data;
        _code = code;
        Message = message;
    }

    public string?  Message { get; set; }

    public TData? Data { get; set; }

    [JsonIgnore]
    public bool IsSuccess => _code is >= 200 and <= 299;
}