using Newtonsoft.Json;

namespace Example.Endpoint.Entities;

public class Entity
{
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("message")]
    public string? Message { get; set; }
}