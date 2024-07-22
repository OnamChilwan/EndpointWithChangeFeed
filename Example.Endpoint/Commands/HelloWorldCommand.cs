using Newtonsoft.Json;

namespace Example.Endpoint.Commands;

public class HelloWorldCommand
{
    [JsonProperty("message")]
    public string? Message { get; set; }
}