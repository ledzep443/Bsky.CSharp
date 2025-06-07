using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Base class for view versions of embedded content in posts.
/// </summary>
[JsonConverter(typeof(EmbedViewConverter))]
public abstract record EmbedView
{
    /// <summary>
    /// The type of embedded content view.
    /// </summary>
    [JsonPropertyName("$type")]
    public required string Type { get; init; }
}

/// <summary>
/// Represents a view of embedded images in a post.
/// </summary>
public record EmbedViewImages : EmbedView
{
    /// <summary>
    /// The list of images embedded in the post.
    /// </summary>
    [JsonPropertyName("images")]
    public required List<ImageView> Images { get; init; }
}

/// <summary>
/// Represents a view of an embedded external link in a post.
/// </summary>
public record EmbedViewExternal : EmbedView
{
    /// <summary>
    /// External link information.
    /// </summary>
    [JsonPropertyName("external")]
    public required EmbedViewExternalInfo External { get; init; }
}

/// <summary>
/// Represents a view of an embedded record (e.g., quoted post) in a post.
/// </summary>
public record EmbedViewRecord : EmbedView
{
    /// <summary>
    /// Reference to the embedded record view.
    /// </summary>
    [JsonPropertyName("record")]
    public required EmbedViewRecordView Record { get; init; }
}

/// <summary>
/// Represents a view of both a record and media embedded in a post.
/// </summary>
public record EmbedViewRecordWithMedia : EmbedView
{
    /// <summary>
    /// Reference to the embedded record.
    /// </summary>
    [JsonPropertyName("record")]
    public required EmbedViewRecordView Record { get; init; }
    
    /// <summary>
    /// The media associated with the embedded record.
    /// </summary>
    [JsonPropertyName("media")]
    public required EmbedViewMedia Media { get; init; }
}

/// <summary>
/// Base class for embedded record views.
/// </summary>
[JsonConverter(typeof(EmbedViewRecordViewConverter))]
public abstract record EmbedViewRecordView
{
    /// <summary>
    /// The type of embedded record view.
    /// </summary>
    [JsonPropertyName("$type")]
    public required string Type { get; init; }
}

/// <summary>
/// Represents an image view with display metadata.
/// </summary>
public record ImageView
{
    /// <summary>
    /// The blob URL of the image.
    /// </summary>
    [JsonPropertyName("fullsize")]
    public required string Fullsize { get; init; }
    
    /// <summary>
    /// The URL of the thumbnail image.
    /// </summary>
    [JsonPropertyName("thumb")]
    public required string Thumb { get; init; }
    
    /// <summary>
    /// Alternative text for the image.
    /// </summary>
    [JsonPropertyName("alt")]
    public required string Alt { get; init; }
    
    /// <summary>
    /// The aspect ratio of the image.
    /// </summary>
    [JsonPropertyName("aspectRatio")]
    public AspectRatio? AspectRatio { get; init; }
}

/// <summary>
/// Represents external link information in a view context.
/// </summary>
public record EmbedViewExternalInfo
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
    /// The URL of the thumbnail image.
    /// </summary>
    [JsonPropertyName("thumb")]
    public string? Thumb { get; init; }
}

/// <summary>
/// Base class for embedded media view types (either images or external).
/// </summary>
[JsonConverter(typeof(EmbedViewMediaConverter))]
public abstract record EmbedViewMedia
{
    /// <summary>
    /// The type of media view.
    /// </summary>
    [JsonPropertyName("$type")]
    public required string Type { get; init; }
}

/// <summary>
/// JSON converter for EmbedView polymorphic deserialization.
/// </summary>
public class EmbedViewConverter : JsonConverter<EmbedView>
{
    public override EmbedView? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Clone the reader to look ahead without advancing the original
        var readerClone = reader;
        
        // Read the JSON into a JsonDocument
        using var doc = JsonDocument.ParseValue(ref readerClone);
        var root = doc.RootElement;
        
        // Check if the $type property exists
        if (!root.TryGetProperty("$type", out var typeProperty))
        {
            throw new JsonException("Cannot determine the type of embed view");
        }
        
        string? type = typeProperty.GetString();
        
        // Based on the $type property, deserialize to the appropriate concrete type
        EmbedView? result = type switch
        {
            "app.bsky.embed.images#view" => JsonSerializer.Deserialize<EmbedViewImages>(ref reader, options),
            "app.bsky.embed.external#view" => JsonSerializer.Deserialize<EmbedViewExternal>(ref reader, options),
            "app.bsky.embed.record#view" => JsonSerializer.Deserialize<EmbedViewRecord>(ref reader, options),
            "app.bsky.embed.recordWithMedia#view" => JsonSerializer.Deserialize<EmbedViewRecordWithMedia>(ref reader, options),
            _ => throw new JsonException($"Unknown embed view type: {type}")
        };
        
        return result;
    }

    public override void Write(Utf8JsonWriter writer, EmbedView value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}

/// <summary>
/// JSON converter for EmbedViewRecordView polymorphic deserialization.
/// </summary>
public class EmbedViewRecordViewConverter : JsonConverter<EmbedViewRecordView>
{
    public override EmbedViewRecordView? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Implementation would be similar to EmbedViewConverter
        throw new NotImplementedException("EmbedViewRecordView converter not yet implemented");
    }

    public override void Write(Utf8JsonWriter writer, EmbedViewRecordView value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}

/// <summary>
/// JSON converter for EmbedViewMedia polymorphic deserialization.
/// </summary>
public class EmbedViewMediaConverter : JsonConverter<EmbedViewMedia>
{
    public override EmbedViewMedia? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Implementation would be similar to EmbedViewConverter
        throw new NotImplementedException("EmbedViewMedia converter not yet implemented");
    }

    public override void Write(Utf8JsonWriter writer, EmbedViewMedia value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
