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

- .NET 9.0 or higher

## Installation

```bash
# Via NuGet Package Manager
Install-Package Bsky.CSharp

# Via .NET CLI
dotnet add package Bsky.CSharp
```

## Quick Start

### Authentication

```csharp
using Bsky.CSharp.Http;
using Bsky.CSharp.AtProto.Services;

// Create the XRPC client with the Bluesky API endpoint
var client = new XrpcClient("https://bsky.social");

// Create the authentication service
var authService = new AuthenticationService(client);

// Login and authenticate
var session = await authService.CreateSessionAsync("yourhandle.bsky.social", "yourpassword");
```

### Creating a Text Post

```csharp
using Bsky.CSharp.Bluesky.Services;

// Create the post service (requires authenticated client)
var blobService = new BlobService(client);
var repoService = new RepositoryService(client);
var postService = new PostService(client, blobService, repoService);

// Create a simple text post
var postRef = await postService.CreateTextPostAsync("Hello from Bsky.CSharp!");
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

// Create a post with an image
var postRef = await postService.CreateImagePostAsync(
    "Check out this photo!", 
    new[] { image }
);
```

### Getting a User Profile

```csharp
var identityService = new IdentityService(client);
var userService = new UserService(client, repoService, blobService, identityService);

// Get a user's profile
var profile = await userService.GetProfileAsync("user.bsky.social");

// Display user information
Console.WriteLine($"Display Name: {profile.DisplayName}");
Console.WriteLine($"Description: {profile.Description}");
Console.WriteLine($"Followers: {profile.FollowersCount}");
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

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgements

- Built for the [Bluesky](https://bsky.app) social network
- Based on the [AT Protocol](https://atproto.com/) specification
