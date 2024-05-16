using System.Text.Json.Serialization;

namespace Server.Core.src.ValueObject;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Status
{
    processing,
    shipped,
    cancelled,
    // delivered,
    completed,
}
