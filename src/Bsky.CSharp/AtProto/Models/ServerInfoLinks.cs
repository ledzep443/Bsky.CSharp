using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Important links related to a server.
/// </summary>
public record ServerInfoLinks
{
    /// <summary>
    /// Link to the terms of service.
    /// </summary>
    [JsonPropertyName("termsOfService")]
    public string? TermsOfService { get; init; }
    
    /// <summary>
    /// Link to the privacy policy.
    /// </summary>
    [JsonPropertyName("privacyPolicy")]
    public string? PrivacyPolicy { get; init; }
}
