using System.Text.Json;
using System.Text.Json.Serialization;
using Bsky.CSharp.Bluesky.Services;

namespace Bsky.CSharp.Bluesky.Models;

/// <summary>
/// Represents a thread view containing a post and its replies.
/// </summary>
public class PostThreadView : ThreadView
{
    /// <summary>
    /// The post at this level of the thread.
    /// </summary>
    [JsonPropertyName("post")]
    public required Post Post { get; init; }
    
    /// <summary>
    /// The parent post, if this is a reply.
    /// </summary>
    [JsonPropertyName("parent")]
    public ThreadView? Parent { get; init; }
    
    /// <summary>
    /// Replies to this post.
    /// </summary>
    [JsonPropertyName("replies")]
    public List<ThreadView>? Replies { get; init; }
}

/// <summary>
/// Represents a thread view for a post that's not found.
/// </summary>
public class NotFoundThreadView : ThreadView
{
    /// <summary>
    /// The URI of the not found post.
    /// </summary>
    [JsonPropertyName("uri")]
    public required string Uri { get; init; }
    
    /// <summary>
    /// The reason the post wasn't found.
    /// </summary>
    [JsonPropertyName("notFound")]
    public required bool NotFound { get; init; }
}

/// <summary>
/// Represents a thread view for a blocked post.
/// </summary>
public class BlockedThreadView : ThreadView
{
    /// <summary>
    /// The URI of the blocked post.
    /// </summary>
    [JsonPropertyName("uri")]
    public required string Uri { get; init; }
    
    /// <summary>
    /// Indicates the post is blocked.
    /// </summary>
    [JsonPropertyName("blocked")]
    public required bool Blocked { get; init; }
    
    /// <summary>
    /// The author of the blocked post.
    /// </summary>
    [JsonPropertyName("author")]
    public required Actor Author { get; init; }
}

/// <summary>
/// JSON converter for ThreadView polymorphic deserialization.
/// </summary>
public class ThreadViewConverter : JsonConverter<ThreadView>
{
    public override ThreadView? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Clone the reader to look ahead without advancing the original
        var readerClone = reader;
        
        // Read the JSON into a JsonDocument
        using var doc = JsonDocument.ParseValue(ref readerClone);
        var root = doc.RootElement;
        
        // Check if the $type property exists
        if (!root.TryGetProperty("$type", out var typeProperty))
        {
            throw new JsonException("Cannot determine the type of thread view");
        }
        
        string? type = typeProperty.GetString();
        
        // Based on the $type property, deserialize to the appropriate concrete type
        ThreadView? result = type switch
        {
            "app.bsky.feed.defs#threadViewPost" => JsonSerializer.Deserialize<PostThreadView>(ref reader, options),
            "app.bsky.feed.defs#notFoundPost" => JsonSerializer.Deserialize<NotFoundThreadView>(ref reader, options),
            "app.bsky.feed.defs#blockedPost" => JsonSerializer.Deserialize<BlockedThreadView>(ref reader, options),
            _ => throw new JsonException($"Unknown thread view type: {type}")
        };
        
        return result;
    }

    public override void Write(Utf8JsonWriter writer, ThreadView value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
