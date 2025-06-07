using System.Text.Json;
using System.Text.Json.Serialization;
using Bsky.CSharp.Bluesky.Models;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// JSON converter for Reason polymorphic deserialization.
/// </summary>
public class ReasonConverter : JsonConverter<Reason>
{
    public override Reason? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Clone the reader to look ahead without advancing the original
        var readerClone = reader;
        
        // Read the JSON into a JsonDocument
        using var doc = JsonDocument.ParseValue(ref readerClone);
        var root = doc.RootElement;
        
        // Check if the $type property exists
        if (!root.TryGetProperty("$type", out var typeProperty))
        {
            throw new JsonException("Cannot determine the type of reason");
        }
        
        string? type = typeProperty.GetString();
        
        // Based on the $type property, deserialize to the appropriate concrete type
        Reason? result = type switch
        {
            "app.bsky.feed.defs#reasonRepost" => JsonSerializer.Deserialize<ReasonRepost>(ref reader, options),
            _ => throw new JsonException($"Unknown reason type: {type}")
        };
        
        return result;
    }

    public override void Write(Utf8JsonWriter writer, Reason value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
