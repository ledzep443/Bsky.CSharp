using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bsky.CSharp.AtProto.Models;

/// <summary>
/// Represents an application password for API authentication.
/// </summary>
public class AppPassword
{
    /// <summary>
    /// The name of the application password.
    /// </summary>
    [JsonPropertyName("name")]
    [Required]
    public string? Name { get; set; }
    
    /// <summary>
    /// The generated password value.
    /// </summary>
    [JsonPropertyName("password")]
    public string? Password { get; set; }
    
    /// <summary>
    /// The creation date of the application password.
    /// </summary>
    [JsonPropertyName("createdAt")]
    [Required]
    public DateTime? CreatedAt { get; set; }
}
