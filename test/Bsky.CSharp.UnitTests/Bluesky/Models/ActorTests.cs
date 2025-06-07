using System.Text.Json;
using Bsky.CSharp.Bluesky.Models;

namespace Bsky.CSharp.UnitTests.Bluesky.Models;

public class ActorTests
{
    [Fact]
    public void Serialize_Actor_SerializesCorrectly()
    {
        // Arrange
        var actor = new Actor
        {
            Did = "did:plc:testuser",
            Handle = "test.user.bsky.app",
            DisplayName = "Test User",
            Description = "This is a test user",
            Avatar = "https://example.com/avatar.jpg",
            Banner = "https://example.com/banner.jpg",
            FollowersCount = 42,
            FollowsCount = 24,
            PostsCount = 100,
            IndexedAt = new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc)
        };

        // Act
        var json = JsonSerializer.Serialize(actor);
        
        // Assert
        Assert.Contains("\"did\":\"did:plc:testuser\"", json);
        Assert.Contains("\"handle\":\"test.user.bsky.app\"", json);
        Assert.Contains("\"displayName\":\"Test User\"", json);
        Assert.Contains("\"description\":\"This is a test user\"", json);
        Assert.Contains("\"avatar\":\"https://example.com/avatar.jpg\"", json);
        Assert.Contains("\"banner\":\"https://example.com/banner.jpg\"", json);
        Assert.Contains("\"followersCount\":42", json);
        Assert.Contains("\"followsCount\":24", json);
        Assert.Contains("\"postsCount\":100", json);
        Assert.Contains("\"indexedAt\":\"2023-01-01T12:00:00", json);
    }
    
    [Fact]
    public void Deserialize_Actor_DeserializesCorrectly()
    {
        // Arrange
        var json = """
        {
            "did": "did:plc:testuser",
            "handle": "test.user.bsky.app",
            "displayName": "Test User",
            "description": "This is a test user",
            "avatar": "https://example.com/avatar.jpg",
            "banner": "https://example.com/banner.jpg",
            "followersCount": 42,
            "followsCount": 24,
            "postsCount": 100,
            "indexedAt": "2023-01-01T12:00:00Z"
        }
        """;
        
        // Act
        var actor = JsonSerializer.Deserialize<Actor>(json);
        
        // Assert
        Assert.NotNull(actor);
        Assert.Equal("did:plc:testuser", actor!.Did);
        Assert.Equal("test.user.bsky.app", actor.Handle);
        Assert.Equal("Test User", actor.DisplayName);
        Assert.Equal("This is a test user", actor.Description);
        Assert.Equal("https://example.com/avatar.jpg", actor.Avatar);
        Assert.Equal("https://example.com/banner.jpg", actor.Banner);
        Assert.Equal(42, actor.FollowersCount);
        Assert.Equal(24, actor.FollowsCount);
        Assert.Equal(100, actor.PostsCount);
        Assert.Equal(new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc), actor.IndexedAt);
    }
    
    [Fact]
    public void Deserialize_ActorWithMissingOptionalFields_DeserializesCorrectly()
    {
        // Arrange
        var json = """
        {
            "did": "did:plc:testuser",
            "handle": "test.user.bsky.app"
        }
        """;
        
        // Act
        var actor = JsonSerializer.Deserialize<Actor>(json);
        
        // Assert
        Assert.NotNull(actor);
        Assert.Equal("did:plc:testuser", actor!.Did);
        Assert.Equal("test.user.bsky.app", actor.Handle);
        Assert.Null(actor.DisplayName);
        Assert.Null(actor.Description);
        Assert.Null(actor.Avatar);
        Assert.Null(actor.Banner);
        Assert.Null(actor.FollowersCount);
        Assert.Null(actor.FollowsCount);
        Assert.Null(actor.PostsCount);
        Assert.Null(actor.IndexedAt);
        Assert.Null(actor.Viewer);
    }
}
