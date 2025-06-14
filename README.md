# Bsky.CSharp - Bluesky API SDK for .NET

A modern, fully-featured C# client library for the [Bluesky](https://bsky.app) social network and the AT Protocol.

## Overview

Bsky.CSharp provides a robust and type-safe way to interact with Bluesky's APIs in .NET applications. The library handles authentication, content creation, user management, and other core Bluesky functionality.

## Features

- **Authentication**: Handle user login and session management
- **Posts**: Create, retrieve, and delete posts with text, images, and links
- **User Management**: Work with user profiles, follows, and followers
- **Content Discovery**: Search posts and users
- **AT Protocol Support**: Direct integration with the underlying AT Protocol

## Requirements

- .NET Standard 2.1 or higher (compatible with .NET Core 3.0+, .NET 5+, .NET 6+, .NET 7+, .NET 8+)

## Installation

```bash
# Via NuGet Package Manager
Install-Package Bsky.CSharp

# Via .NET CLI
dotnet add package Bsky.CSharp
```

## Quick Start

### Setting up Dependency Injection

```csharp
using Bsky.AspNetCore.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

// Set up the service collection
var services = new ServiceCollection();

// Add Bluesky services to the DI container
services.AddBluesky();

// Build the service provider
var serviceProvider = services.BuildServiceProvider();

// In a real application, you would typically use constructor injection rather than
// directly accessing the service provider, as shown in the examples below
```

### Authentication

```csharp
using Bsky.CSharp.AtProto.Services;
using Bsky.CSharp.Http;

// Using manual instantiation
var client = new XrpcClient("https://bsky.social");
var authService = new AuthenticationService(client);
var session = await authService.CreateSessionAsync("yourhandle.bsky.social", "yourpassword");

// Using constructor injection in a class
public class BlueskyAuthenticationService
{
    private readonly AuthenticationService _authService;
    
    // Inject the AuthenticationService through the constructor
    public BlueskyAuthenticationService(AuthenticationService authService)
    {
        _authService = authService;
    }
    
    public async Task<SessionInfo> LoginAsync(string handle, string password)
    {
        return await _authService.CreateSessionAsync(handle, password);
    }
}
```

### Creating a Text Post

```csharp
using Bsky.CSharp.Bluesky.Services;

// Using manual instantiation
var blobService = new BlobService(client);
var repoService = new RepositoryService(client);
var postService = new PostService(client, blobService, repoService);
var postRef = await postService.CreateTextPostAsync("Hello from Bsky.CSharp!");

// Using constructor injection in a class
public class BlueskyPostingService
{
    private readonly IPostService _postService;
    
    // Inject the IPostService through the constructor
    public BlueskyPostingService(IPostService postService)
    {
        _postService = postService;
    }
    
    public async Task<RecordRef> CreateTextPostAsync(string text)
    {
        return await _postService.CreateTextPostAsync(text);
    }
}
```

### Creating an Image Post

```csharp
// Prepare image data
var imageData = File.ReadAllBytes("path/to/image.jpg");

var image = new ImageUpload
{
    Data = imageData,
    ContentType = "image/jpeg",
    AltText = "Description of the image"
};

// Using manual instantiation
var postRef = await postService.CreateImagePostAsync(
    "Check out this photo!", 
    new[] { image }
);

// Using constructor injection in a class
public class BlueskyMediaService
{
    private readonly IPostService _postService;
    
    public BlueskyMediaService(IPostService postService)
    {
        _postService = postService;
    }
    
    public async Task<RecordRef> CreateImagePostAsync(string text, ImageUpload[] images)
    {
        return await _postService.CreateImagePostAsync(text, images);
    }
}

// Usage in an application
public class ImageUploaderComponent
{
    private readonly BlueskyMediaService _mediaService;
    
    public ImageUploaderComponent(BlueskyMediaService mediaService)
    {
        _mediaService = mediaService;
    }
    
    public async Task UploadImageAsync(byte[] imageData, string description)
    {
        var image = new ImageUpload
        {
            Data = imageData,
            ContentType = "image/jpeg",
            AltText = description
        };
        
        await _mediaService.CreateImagePostAsync("Check out this photo!", new[] { image });
    }
}
```

### Getting a User Profile

```csharp
// Using manual instantiation
var identityService = new IdentityService(client);
var userService = new UserService(client, repoService, blobService, identityService);
var profile = await userService.GetProfileAsync("user.bsky.social");

// Using constructor injection in a class
public class BlueskyUserService
{
    private readonly IUserService _userService;
    
    public BlueskyUserService(IUserService userService)
    {
        _userService = userService;
    }
    
    public async Task<UserProfile> GetUserProfileAsync(string handle)
    {
        var profile = await _userService.GetProfileAsync(handle);
        
        // Process profile data
        Console.WriteLine($"Display Name: {profile.DisplayName}");
        Console.WriteLine($"Description: {profile.Description}");
        Console.WriteLine($"Followers: {profile.FollowersCount}");
        
        return profile;
    }
}
```

## Main Components

### AT Protocol Services

- `AuthenticationService`: Session creation and management
- `BlobService`: Handle binary data uploads
- `RepositoryService`: Manage collections and records
- `IdentityService`: Work with DIDs and user identities
- `ServerService`: Access server information
- `SyncService`: Synchronize data across the network

### Bluesky Services

- `PostService`: Create and manage posts
- `UserService`: Work with user profiles and relationships
- `FeedService`: Access user feeds and timelines
- `NotificationService`: Handle user notifications

## Data Models

### ImageUpload

The `ImageUpload` class represents an image to be uploaded with a post:

```csharp
public class ImageUpload
{
    // The binary data of the image
    public byte[]? Data { get; set; }
    
    // The MIME type of the image (e.g., "image/jpeg", "image/png")
    public string? ContentType { get; set; }
    
    // Alternative text for the image for accessibility
    public string? AltText { get; set; }
}
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgements

- Built for the [Bluesky](https://bsky.app) social network
- Based on the [AT Protocol](https://atproto.com/) specification
