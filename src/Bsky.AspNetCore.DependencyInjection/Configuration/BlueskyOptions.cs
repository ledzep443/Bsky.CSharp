namespace Bsky.AspNetCore.DependencyInjection.Configuration;

public class BlueskyOptions
{
    public string BaseUrl { get; set; } = "https://bsky.social";
    public int Timeout { get; set; } = 10000;
    public string DefaultHandle { get; set; } = "@bluesky";
    public string DefaultPassword { get; set; } = "password";
}