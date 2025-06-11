using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models.Converters;

/// <summary>
/// JSON converter for ThreadView objects.
/// </summary>
public class ThreadViewConverter : JsonConverter<ThreadView>
{
    /// <summary>
    /// Determines whether the specified type can be converted.
    /// </summary>
    /// <param name="typeToConvert">The type to compare against.</param>
    /// <returns>True if the type can be converted, false otherwise.</returns>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(ThreadView).IsAssignableFrom(typeToConvert);
    }
    
    /// <summary>
    /// Reads and converts the JSON to a ThreadView object.
    /// </summary>
    /// <param name="reader">The JSON reader.</param>
    /// <param name="typeToConvert">The type to convert to.</param>
    /// <param name="options">The serializer options.</param>
    /// <returns>The converted ThreadView object.</returns>
    public override ThreadView? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }
        
        // Read the JSON object into a document
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        
        if (!root.TryGetProperty("$type", out var typeProperty))
        {
            throw new JsonException("ThreadView JSON missing $type property");
        }
        
        var typeValue = typeProperty.GetString();
        
        // Convert back to JSON string and deserialize based on type
        var jsonString = root.GetRawText();
        return typeValue switch
        {
            "app.bsky.feed.defs#threadViewPost" => JsonSerializer.Deserialize<PostThreadView>(jsonString, options),
            "app.bsky.feed.defs#notFoundPost" => JsonSerializer.Deserialize<NotFoundThreadView>(jsonString, options),
            "app.bsky.feed.defs#blockedPost" => JsonSerializer.Deserialize<BlockedThreadView>(jsonString, options),
            _ => throw new JsonException($"Unknown ThreadView type: {typeValue}")
        };
    }
    
    /// <summary>
    /// Writes the ThreadView object to JSON.
    /// </summary>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="value">The ThreadView object to write.</param>
    /// <param name="options">The serializer options.</param>
    public override void Write(Utf8JsonWriter writer, ThreadView value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
