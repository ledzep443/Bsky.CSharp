# Bsky.CSharp

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

- .NET Standard 2.1 or higher (compatible with .NET Core 3.0+, .NET 5+, .NET Framework 4.8 with .NET Standard 2.1 support)

## Installation

```bash
# Via NuGet Package Manager
Install-Package Bsky.CSharp

# Via .NET CLI
dotnet add package Bsky.CSharp
```

## Usage

### Authentication

```csharp
using Bsky.CSharp.AtProto.Services;
using Bsky.CSharp.Http;

// Create the XRPC client
var client = new XrpcClient("https://bsky.social");
var authService = new AuthenticationService(client);

// Create a session (login)
var session = await authService.CreateSessionAsync("yourhandle.bsky.social", "yourpassword");

// Store the authentication token for future requests
client.SetAuth(session.AccessToken);
```

### Creating Posts

```csharp
using Bsky.CSharp.Bluesky.Services;

// Initialize required services
var blobService = new BlobService(client);
var repoService = new RepositoryService(client);
var postService = new PostService(client, blobService, repoService);

// Create a simple text post
var postRef = await postService.CreateTextPostAsync("Hello from Bluesky!");

// Create a post with an image
byte[] imageData = File.ReadAllBytes("path/to/image.jpg");
var postWithImageRef = await postService.CreatePostWithImagesAsync(
    "Check out this photo!", 
    new[] { (imageData, "image/jpeg", "Alt text description") }
);

// Reply to another post
var replyRef = await postService.CreateReplyAsync(
    "This is my reply!",
    originalPostUri,
    originalPostCid
);
```

### User Management

```csharp
using Bsky.CSharp.Bluesky.Services;

// Initialize the user service
var userService = new UserService(client);

// Get a user profile
var profile = await userService.GetProfileAsync("handle.bsky.social");

// Follow a user
await userService.FollowUserAsync("handle.bsky.social");

// Get a user's followers
var followers = await userService.GetFollowersAsync("handle.bsky.social");
```

### Timeline and Feeds

```csharp
using Bsky.CSharp.Bluesky.Services;

// Initialize the feed service
var feedService = new FeedService(client);

// Get the authenticated user's home timeline
var timeline = await feedService.GetTimelineAsync();

// Get the next page of results using the cursor
var nextPage = await feedService.GetTimelineAsync(timeline.Cursor);

// Get a user's authored posts
var authorFeed = await feedService.GetAuthorFeedAsync("handle.bsky.social");
```

## Error Handling

The library uses custom exceptions to provide clear error messages:

```csharp
try 
{
    var profile = await userService.GetProfileAsync("non.existent.handle");
}
catch (InvalidBskyHandleException ex) 
{
    Console.WriteLine($"Invalid handle: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
```

## Advanced Usage

For more advanced scenarios like custom HTTP message handlers, rate limiting, or caching, refer to the documentation or explore the source code.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
