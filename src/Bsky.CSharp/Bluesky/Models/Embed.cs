using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Base class for embedded content in posts.
/// </summary>
[JsonConverter(typeof(EmbedConverter))]
public abstract record Embed
{
    /// <summary>
    /// The type of embedded content.
    /// </summary>
    [JsonPropertyName("$type")]
    public required string Type { get; init; }
}

/// <summary>
/// Represents embedded images in a post.
/// </summary>
public record EmbedImages : Embed
{
    /// <summary>
    /// The list of images embedded in the post.
    /// </summary>
    [JsonPropertyName("images")]
    public required List<Image> Images { get; init; }
}

/// <summary>
/// Represents an embedded external link in a post.
/// </summary>
public record EmbedExternal : Embed
{
    /// <summary>
    /// External link information.
    /// </summary>
    [JsonPropertyName("external")]
    public required EmbedExternalInfo External { get; init; }
}

/// <summary>
/// Represents an embedded record (e.g., quoted post) in a post.
/// </summary>
public record EmbedRecord : Embed
{
    /// <summary>
    /// Reference to the embedded record.
    /// </summary>
    [JsonPropertyName("record")]
    public required ReplyRef Record { get; init; }
}

/// <summary>
/// Represents both a record and media embedded in a post.
/// </summary>
public record EmbedRecordWithMedia : Embed
{
    /// <summary>
    /// Reference to the embedded record.
    /// </summary>
    [JsonPropertyName("record")]
    public required ReplyRef Record { get; init; }
    
    /// <summary>
    /// The media associated with the embedded record.
    /// </summary>
    [JsonPropertyName("media")]
    public required EmbedMedia Media { get; init; }
}

/// <summary>
/// Represents external link information.
/// </summary>
public record EmbedExternalInfo
{
    /// <summary>
    /// The external URL.
    /// </summary>
    [JsonPropertyName("uri")]
    public required string Uri { get; init; }
    
    /// <summary>
    /// Title of the external content.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; init; }
    
    /// <summary>
    /// Description of the external content.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
    
    /// <summary>
    /// Thumbnail image information.
    /// </summary>
    [JsonPropertyName("thumb")]
    public EmbedExternalThumb? Thumb { get; set; }
}

/// <summary>
/// Represents a thumbnail image for an external link.
/// </summary>
public record EmbedExternalThumb
{
    /// <summary>
    /// The URL of the thumbnail image.
    /// </summary>
    [JsonPropertyName("uri")]
    public required string Uri { get; init; }
    
    /// <summary>
    /// MIME type of the thumbnail.
    /// </summary>
    [JsonPropertyName("mimetype")]
    public string? MimeType { get; init; }
    
    /// <summary>
    /// Width of the thumbnail in pixels.
    /// </summary>
    [JsonPropertyName("width")]
    public int? Width { get; init; }
    
    /// <summary>
    /// Height of the thumbnail in pixels.
    /// </summary>
    [JsonPropertyName("height")]
    public int? Height { get; init; }
}

/// <summary>
/// Represents embedded media types (either images or external).
/// </summary>
[JsonConverter(typeof(EmbedMediaConverter))]
public abstract record EmbedMedia
{
    /// <summary>
    /// The type of media.
    /// </summary>
    [JsonPropertyName("$type")]
    public required string Type { get; init; }
}

/// <summary>
/// JSON converter for Embed polymorphic deserialization.
/// </summary>
public class EmbedConverter : JsonConverter<Embed>
{
    public override Embed? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Clone the reader to look ahead without advancing the original
        var readerClone = reader;
        
        // Read the JSON into a JsonDocument
        using var doc = JsonDocument.ParseValue(ref readerClone);
        var root = doc.RootElement;
        
        // Check if the $type property exists
        if (!root.TryGetProperty("$type", out var typeProperty))
        {
            throw new JsonException("Cannot determine the type of embed");
        }
        
        string? type = typeProperty.GetString();
        
        // Based on the $type property, deserialize to the appropriate concrete type
        Embed? result = type switch
        {
            "app.bsky.embed.images" => JsonSerializer.Deserialize<EmbedImages>(ref reader, options),
            "app.bsky.embed.external" => JsonSerializer.Deserialize<EmbedExternal>(ref reader, options),
            "app.bsky.embed.record" => JsonSerializer.Deserialize<EmbedRecord>(ref reader, options),
            "app.bsky.embed.recordWithMedia" => JsonSerializer.Deserialize<EmbedRecordWithMedia>(ref reader, options),
            _ => throw new JsonException($"Unknown embed type: {type}")
        };
        
        return result;
    }

    public override void Write(Utf8JsonWriter writer, Embed value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}

/// <summary>
/// JSON converter for EmbedMedia polymorphic deserialization.
/// </summary>
public class EmbedMediaConverter : JsonConverter<EmbedMedia>
{
    public override EmbedMedia? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Implementation would be similar to EmbedConverter
        throw new NotImplementedException("EmbedMedia converter not yet implemented");
    }

    public override void Write(Utf8JsonWriter writer, EmbedMedia value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
