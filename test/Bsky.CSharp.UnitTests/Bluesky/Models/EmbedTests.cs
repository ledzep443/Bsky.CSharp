using System.Text.Json;
using Bsky.CSharp.AtProto.Models;
using Bsky.CSharp.Bluesky.Models;

namespace Bsky.CSharp.UnitTests.Bluesky.Models;

public class EmbedTests
{
    [Fact]
    public void Serialize_EmbedImages_SerializesCorrectly()
    {
        // Arrange
        var embed = new EmbedImages
        {
            Type = "app.bsky.embed.images",
            Images = new List<Image>
            {
                new Image
                {
                    ImageUrl = "https://example.com/image.jpg",
                    Alt = "Example image",
                    AspectRatio = new AspectRatio
                    {
                        Width = 800,
                        Height = 600
                    }
                }
            }
        };

        // Act
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var json = JsonSerializer.Serialize(embed, options);
        
        // Assert
        Assert.Contains("\"$type\": \"app.bsky.embed.images\"", json);
        Assert.Contains("\"images\"", json);
        Assert.Contains("\"image\": \"https://example.com/image.jpg\"", json);
        Assert.Contains("\"alt\": \"Example image\"", json);
        Assert.Contains("\"aspectRatio\"", json);
        Assert.Contains("\"width\": 800", json);
        Assert.Contains("\"height\": 600", json);
    }
    
    [Fact]
    public void Serialize_EmbedExternal_SerializesCorrectly()
    {
        // Arrange
        var embed = new EmbedExternal
        {
            Type = "app.bsky.embed.external",
            External = new EmbedExternalInfo
            {
                Uri = "https://example.com",
                Title = "Example Website",
                Description = "This is an example website",
                Thumb = new EmbedExternalThumb
                {
                    Uri = "https://example.com/thumb.jpg",
                    MimeType = "image/jpeg",
                }
            }
        };

        // Act
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var json = JsonSerializer.Serialize(embed, options);
        
        // Assert
        Assert.Contains("\"$type\": \"app.bsky.embed.external\"", json);
        Assert.Contains("\"external\"", json);
        Assert.Contains("\"uri\": \"https://example.com\"", json);
        Assert.Contains("\"title\": \"Example Website\"", json);
        Assert.Contains("\"description\": \"This is an example website\"", json);
        Assert.Contains("\"thumb\"", json);
        Assert.Contains("\"uri\": \"https://example.com/thumb.jpg\"", json);
        Assert.Contains("\"mimeType\": \"image/jpeg\"", json);
    }
    
    [Fact]
    public void Serialize_EmbedRecord_SerializesCorrectly()
    {
        // Arrange
        var embed = new EmbedRecord
        {
            Type = "app.bsky.embed.record",
            Record = new RecordRef
            {
                Uri = "at://did:plc:user/app.bsky.feed.post/1234",
                Cid = "bafyreia..."
            }
        };

        // Act
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var json = JsonSerializer.Serialize(embed, options);
        
        // Assert
        Assert.Contains("\"$type\": \"app.bsky.embed.record\"", json);
        Assert.Contains("\"record\"", json);
        Assert.Contains("\"uri\": \"at://did:plc:user/app.bsky.feed.post/1234\"", json);
        Assert.Contains("\"cid\": \"bafyreia...\"", json);
    }
}
