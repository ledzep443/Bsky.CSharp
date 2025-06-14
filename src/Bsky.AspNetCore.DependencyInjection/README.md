# Bsky.AspNetCore.DependencyInjection

Extension methods to easily integrate the Bsky.CSharp library into ASP.NET Core applications using dependency injection.

## Overview

This package provides convenient extension methods to register all necessary Bsky.CSharp services in an ASP.NET Core application's dependency injection container. It simplifies the setup process for interacting with the Bluesky social network in your web applications.

## Requirements

- .NET 8.0 or higher
- ASP.NET Core 8.0 or higher
- Bsky.CSharp package

## Installation

```bash
# Via NuGet Package Manager
Install-Package Bsky.AspNetCore.DependencyInjection

# Via .NET CLI
dotnet add package Bsky.AspNetCore.DependencyInjection
```

## Usage

### Basic Setup

Add Bluesky services to your ASP.NET Core application in the `Program.cs` or `Startup.cs` file:

```csharp
using Bsky.AspNetCore.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add Bluesky services to the container
builder.Services.AddBluesky();

var app = builder.Build();
// ... rest of your application setup
```

### Custom Configuration

You can customize the Bluesky API settings when registering the services:

```csharp
builder.Services.AddBluesky(options => 
{
    // Configure the base URL for the Bluesky API (default is https://bsky.social)
    options.BaseUrl = "https://bsky.social";
    
    // Configure HTTP client timeout (in seconds)
    options.Timeout = 30;
    
    // Configure default credentials (optional)
    options.DefaultHandle = "yourhandle.bsky.social";
    options.DefaultPassword = "your-secure-password"; // Not recommended for production
});
```

### Using Bluesky Services in Controllers

After registering the services, you can inject and use them in your controllers:

```csharp
using Bsky.CSharp.Bluesky.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BlueskyController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IUserService _userService;
    
    public BlueskyController(IPostService postService, IUserService userService)
    {
        _postService = postService;
        _userService = userService;
    }
    
    [HttpGet("timeline")]
    public async Task<IActionResult> GetTimeline()
    {
        var feedService = HttpContext.RequestServices.GetRequiredService<IFeedService>();
        var timeline = await feedService.GetTimelineAsync();
        return Ok(timeline);
    }
    
    [HttpPost("post")]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
    {
        var postRef = await _postService.CreateTextPostAsync(request.Text);
        return Ok(new { postRef.Uri, postRef.Cid });
    }
    
    [HttpGet("profile/{handle}")]
    public async Task<IActionResult> GetProfile(string handle)
    {
        var profile = await _userService.GetProfileAsync(handle);
        return Ok(profile);
    }
}
```

### Authentication

To authenticate requests, you'll typically need to handle user authentication and store the Bluesky session token:

```csharp
using Bsky.CSharp.AtProto.Services;
using Bsky.CSharp.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthenticationService _authService;
    private readonly IXrpcClient _client;
    
    public AuthController(AuthenticationService authService, IXrpcClient client)
    {
        _authService = authService;
        _client = client;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var session = await _authService.CreateSessionAsync(request.Handle, request.Password);
        
        // Store the token in your authentication system
        // This is a simplified example - use proper auth management in a real app
        HttpContext.Session.SetString("BlueskyToken", session.AccessToken);
        
        return Ok(new { success = true });
    }
    
    [HttpGet("set-auth")]
    public IActionResult SetAuth()
    {
        // Get the stored token and set it on the client
        // In a real application, this might be middleware or an auth handler
        var token = HttpContext.Session.GetString("BlueskyToken");
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized();
        }
        
        _client.SetAuth(token);
        return Ok(new { success = true });
    }
}
```

## Advanced Configuration

### Custom HTTP Client Configuration

You can configure the underlying HTTP client by providing a custom configuration action:

```csharp
builder.Services.AddBluesky(options => { }, configureClient: client => 
{
    client.DefaultRequestHeaders.Add("User-Agent", "MyAwesomeBlueskyApp/1.0");
    client.Timeout = TimeSpan.FromMinutes(2);
});
```

## License

This project is licensed under the MIT License.
