using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Base class for facet features (mention, link, tag).
/// </summary>
[JsonConverter(typeof(FacetFeatureConverter))]
public abstract class FacetFeature
{
    /// <summary>
    /// The type of facet feature.
    /// </summary>
    [JsonPropertyName("$type"), Required, JsonRequired]
    public string? Type { get; set; }
}

/// <summary>
/// Represents a mention in a post facet.
/// </summary>
public class FacetMention : FacetFeature
{
    /// <summary>
    /// The DID of the mentioned user.
    /// </summary>
    [JsonPropertyName("did"), Required, JsonRequired]
    public string? Did { get; set; }
}

/// <summary>
/// Represents a link in a post facet.
/// </summary>
public class FacetLink : FacetFeature
{
    /// <summary>
    /// The URL being linked.
    /// </summary>
    [JsonPropertyName("uri"), Required, JsonRequired]
    public string? Uri { get; set; }
}

/// <summary>
/// Represents a tag in a post facet.
/// </summary>
public class FacetTag : FacetFeature
{
    /// <summary>
    /// The tag name.
    /// </summary>
    [JsonPropertyName("tag"), Required, JsonRequired]
    public string? Tag { get; set; }
}

/// <summary>
/// JSON converter for FacetFeature polymorphic deserialization.
/// </summary>
public class FacetFeatureConverter : JsonConverter<FacetFeature>
{
    public override FacetFeature? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Clone the reader to look ahead without advancing the original
        var readerClone = reader;
        
        // Read the JSON into a JsonDocument
        using var doc = JsonDocument.ParseValue(ref readerClone);
        var root = doc.RootElement;
        
        // Check if the $type property exists
        if (!root.TryGetProperty("$type", out var typeProperty))
        {
            throw new JsonException("Cannot determine the type of facet feature");
        }
        
        string? type = typeProperty.GetString();
        
        // Based on the $type property, deserialize to the appropriate concrete type
        FacetFeature? result = type switch
        {
            "app.bsky.richtext.facet#mention" => JsonSerializer.Deserialize<FacetMention>(ref reader, options),
            "app.bsky.richtext.facet#link" => JsonSerializer.Deserialize<FacetLink>(ref reader, options),
            "app.bsky.richtext.facet#tag" => JsonSerializer.Deserialize<FacetTag>(ref reader, options),
            _ => throw new JsonException($"Unknown facet feature type: {type}")
        };
        
        return result;
    }

    public override void Write(Utf8JsonWriter writer, FacetFeature value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
