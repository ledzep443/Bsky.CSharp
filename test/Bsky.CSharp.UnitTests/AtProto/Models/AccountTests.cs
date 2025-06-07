using System.Text.Json;
using Bsky.CSharp.AtProto.Models;

namespace Bsky.CSharp.UnitTests.AtProto.Models;

public class AccountTests
{
    [Fact]
    public void Serialize_Account_SerializesCorrectly()
    {
        // Arrange
        var account = new Account
        {
            Did = "did:plc:testuser",
            Handle = "test.user.bsky.app",
            Email = "test@example.com",
            EmailConfirmed = true
        };

        // Act
        var json = JsonSerializer.Serialize(account);
        
        // Assert
        Assert.Contains("\"did\":\"did:plc:testuser\"", json);
        Assert.Contains("\"handle\":\"test.user.bsky.app\"", json);
        Assert.Contains("\"email\":\"test@example.com\"", json);
        Assert.Contains("\"emailConfirmed\":true", json);
    }
    
    [Fact]
    public void Deserialize_Account_DeserializesCorrectly()
    {
        // Arrange
        var json = """
        {
            "did": "did:plc:testuser",
            "handle": "test.user.bsky.app",
            "email": "test@example.com",
            "emailConfirmed": true
        }
        """;
        
        // Act
        var account = JsonSerializer.Deserialize<Account>(json);
        
        // Assert
        Assert.NotNull(account);
        Assert.Equal("did:plc:testuser", account!.Did);
        Assert.Equal("test.user.bsky.app", account.Handle);
        Assert.Equal("test@example.com", account.Email);
        Assert.True(account.EmailConfirmed);
    }
    
    [Fact]
    public void Deserialize_AccountWithMissingOptionalFields_DeserializesCorrectly()
    {
        // Arrange
        var json = """
        {
            "did": "did:plc:testuser",
            "handle": "test.user.bsky.app"
        }
        """;
        
        // Act
        var account = JsonSerializer.Deserialize<Account>(json);
        
        // Assert
        Assert.NotNull(account);
        Assert.Equal("did:plc:testuser", account!.Did);
        Assert.Equal("test.user.bsky.app", account.Handle);
        Assert.Null(account.Email);
        Assert.Null(account.EmailConfirmed);
    }
}
