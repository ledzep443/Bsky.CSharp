using System.Text.Json;
using Bsky.CSharp.AtProto.Models;

namespace Bsky.CSharp.UnitTests.AtProto.Models;

public class ServiceAuthTokenTests
{
    [Fact]
    public void Serialize_ServiceAuthToken_SerializesCorrectly()
    {
        // Arrange
        var token = new ServiceAuthToken
        {
            AccessToken = "access-token-value",
            RefreshToken = "refresh-token-value",
            TokenType = "Bearer",
            ExpiresIn = 3600,
            Handle = "test.user.bsky.app",
            Did = "did:plc:testuser"
        };

        // Act
        var json = JsonSerializer.Serialize(token);
        
        // Assert
        Assert.Contains("\"accessToken\":\"access-token-value\"", json);
        Assert.Contains("\"refreshToken\":\"refresh-token-value\"", json);
        Assert.Contains("\"tokenType\":\"Bearer\"", json);
        Assert.Contains("\"expiresIn\":3600", json);
        Assert.Contains("\"handle\":\"test.user.bsky.app\"", json);
        Assert.Contains("\"did\":\"did:plc:testuser\"", json);
    }
    
    [Fact]
    public void Deserialize_ServiceAuthToken_DeserializesCorrectly()
    {
        // Arrange
        var json = """
        {
            "accessToken": "access-token-value",
            "refreshToken": "refresh-token-value",
            "tokenType": "Bearer",
            "expiresIn": 3600,
            "handle": "test.user.bsky.app",
            "did": "did:plc:testuser"
        }
        """;
        
        // Act
        var token = JsonSerializer.Deserialize<ServiceAuthToken>(json);
        
        // Assert
        Assert.NotNull(token);
        Assert.Equal("access-token-value", token!.AccessToken);
        Assert.Equal("refresh-token-value", token.RefreshToken);
        Assert.Equal("Bearer", token.TokenType);
        Assert.Equal(3600, token.ExpiresIn);
        Assert.Equal("test.user.bsky.app", token.Handle);
        Assert.Equal("did:plc:testuser", token.Did);
    }
}
