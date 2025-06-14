using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Bsky.CSharp.Bluesky.Models;
using Bsky.CSharp.Http;
using Bsky.CSharp.AtProto.Services;
using Bsky.CSharp.AtProto.Models;

namespace Bsky.CSharp.Bluesky.Services;

/// <summary>
/// Interface for managing user accounts and profiles on Bluesky.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets the current user's profile.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The user's profile.</returns>
    Task<Actor> GetMyProfileAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a user's profile by handle or DID.
    /// </summary>
    /// <param name="handleOrDid">The handle or DID of the user.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The user's profile.</returns>
    Task<Actor> GetProfileAsync(string handleOrDid, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates the current user's profile.
    /// </summary>
    /// <param name="update">The profile update information.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A reference to the updated profile record.</returns>
    Task<ProfileRecord> UpdateProfileAsync(ProfileUpdate update, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Searches for users by handle or display name.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="limit">Maximum number of results to return.</param>
    /// <param name="cursor">Pagination cursor from a previous request.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A list of matching users.</returns>
    Task<ActorsResponse> SearchUsersAsync(
        string query,
        int? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a list of users that the specified user follows.
    /// </summary>
    /// <param name="user">The handle or DID of the user.</param>
    /// <param name="limit">Maximum number of results to return.</param>
    /// <param name="cursor">Pagination cursor from a previous request.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A list of users that the specified user follows.</returns>
    Task<ActorsResponse> GetFollowsAsync(
        string user,
        int? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a list of users that follow the specified user.
    /// </summary>
    /// <param name="user">The handle or DID of the user.</param>
    /// <param name="limit">Maximum number of results to return.</param>
    /// <param name="cursor">Pagination cursor from a previous request.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A list of users that follow the specified user.</returns>
    Task<ActorsResponse> GetFollowersAsync(
        string user,
        int? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default);
}


/// <summary>
/// Service for managing user accounts and profiles on Bluesky.
/// </summary>
public class UserService : IUserService
{
    private readonly XrpcClient _client;
    private readonly RepositoryService _repoService;
    private readonly BlobService _blobService;
    private readonly IdentityService _identityService;
    private const string ProfileCollectionId = "app.bsky.actor.profile";
    
    /// <summary>
    /// Creates a new user service.
    /// </summary>
    /// <param name="client">The XRPC client to use for API requests.</param>
    /// <param name="repoService">The repository service.</param>
    /// <param name="blobService">The blob service.</param>
    /// <param name="identityService">The identity service.</param>
    public UserService(
        XrpcClient client, 
        RepositoryService repoService, 
        BlobService blobService,
        IdentityService identityService)
    {
        _client = client;
        _repoService = repoService;
        _blobService = blobService;
        _identityService = identityService;
    }
    
    /// <summary>
    /// Gets the current user's profile.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The user's profile.</returns>
    public async Task<Actor> GetMyProfileAsync(CancellationToken cancellationToken = default)
    {
        var session = await GetSessionAsync(cancellationToken);
        return await GetProfileAsync(session.Did, cancellationToken);
    }
    
    /// <summary>
    /// Gets a user's profile by handle or DID.
    /// </summary>
    /// <param name="handleOrDid">The handle or DID of the user.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The user's profile.</returns>
    public async Task<Actor> GetProfileAsync(string handleOrDid, CancellationToken cancellationToken = default)
    {
        const string endpoint = "app.bsky.actor.getProfile";
        var parameters = new Dictionary<string, string>
        {
            ["actor"] = handleOrDid
        };
        
        return await _client.GetAsync<Actor>(endpoint, parameters, cancellationToken).ConfigureAwait(false);
    }
    
    /// <summary>
    /// Updates the current user's profile.
    /// </summary>
    /// <param name="update">The profile update information.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A reference to the updated profile record.</returns>
    public async Task<ProfileRecord> UpdateProfileAsync(ProfileUpdate update, CancellationToken cancellationToken = default)
    {
        var session = await GetSessionAsync(cancellationToken);
        var profileRecord = new ProfileRecord();
        
        // Set the properties that are being updated
        if (update.DisplayName != null)
        {
            profileRecord.DisplayName = update.DisplayName;
        }
        
        if (update.Description != null)
        {
            profileRecord.Description = update.Description;
        }
        
        // Upload avatar if provided
        if (update.AvatarImage != null)
        {
            var avatarBlob = await _blobService.UploadBlobAsync(
                update.AvatarImage, 
                update.AvatarImageContentType ?? "image/jpeg", 
                cancellationToken);
            
            profileRecord.Avatar = new BlobReference
            {
                Cid = avatarBlob.Blob.Ref!.Link,
                MimeType = update.AvatarImageContentType ?? "image/jpeg"
            };
        }
        
        // Upload banner if provided
        if (update.BannerImage != null)
        {
            var bannerBlob = await _blobService.UploadBlobAsync(
                update.BannerImage, 
                update.BannerImageContentType ?? "image/jpeg", 
                cancellationToken);
            
            profileRecord.Banner = new BlobReference
            {
                Cid = bannerBlob.Blob.Ref!.Link,
                MimeType = update.BannerImageContentType ?? "image/jpeg"
            };
        }
        
        // Put the updated profile record
        await _repoService.PutRecordAsync(
            session.Did,
            ProfileCollectionId,
            "self",
            profileRecord,
            true,
            cancellationToken)
            .ConfigureAwait(false);
        
        return profileRecord;
    }
    
    /// <summary>
    /// Searches for users by handle or display name.
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="limit">Maximum number of results to return.</param>
    /// <param name="cursor">Pagination cursor from a previous request.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A list of matching users.</returns>
    public async Task<ActorsResponse> SearchUsersAsync(
        string query,
        int? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default)
    {
        const string endpoint = "app.bsky.actor.searchActors";
        var parameters = new Dictionary<string, string>
        {
            ["term"] = query
        };
        
        if (limit.HasValue)
        {
            parameters["limit"] = limit.Value.ToString();
        }
        
        if (!string.IsNullOrEmpty(cursor))
        {
            parameters["cursor"] = cursor;
        }
        
        return await _client.GetAsync<ActorsResponse>(endpoint, parameters, cancellationToken).ConfigureAwait(false);
    }
    
    /// <summary>
    /// Gets a list of users that the specified user follows.
    /// </summary>
    /// <param name="user">The handle or DID of the user.</param>
    /// <param name="limit">Maximum number of results to return.</param>
    /// <param name="cursor">Pagination cursor from a previous request.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A list of users that the specified user follows.</returns>
    public async Task<ActorsResponse> GetFollowsAsync(
        string user,
        int? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default)
    {
        const string endpoint = "app.bsky.graph.getFollows";
        var parameters = new Dictionary<string, string>
        {
            ["actor"] = user
        };
        
        if (limit.HasValue)
        {
            parameters["limit"] = limit.Value.ToString();
        }
        
        if (!string.IsNullOrEmpty(cursor))
        {
            parameters["cursor"] = cursor;
        }
        
        return await _client.GetAsync<ActorsResponse>(endpoint, parameters, cancellationToken).ConfigureAwait(false);
    }
    
    /// <summary>
    /// Gets a list of users that follow the specified user.
    /// </summary>
    /// <param name="user">The handle or DID of the user.</param>
    /// <param name="limit">Maximum number of results to return.</param>
    /// <param name="cursor">Pagination cursor from a previous request.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A list of users that follow the specified user.</returns>
    public async Task<ActorsResponse> GetFollowersAsync(
        string user,
        int? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default)
    {
        const string endpoint = "app.bsky.graph.getFollowers";
        var parameters = new Dictionary<string, string>
        {
            ["actor"] = user
        };
        
        if (limit.HasValue)
        {
            parameters["limit"] = limit.Value.ToString();
        }
        
        if (!string.IsNullOrEmpty(cursor))
        {
            parameters["cursor"] = cursor;
        }
        
        return await _client.GetAsync<ActorsResponse>(endpoint, parameters, cancellationToken).ConfigureAwait(false);
    }
    
    /// <summary>
    /// Follows a user.
    /// </summary>
    /// <param name="user">The handle or DID of the user to follow.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A reference to the created follow record.</returns>
    public async Task<RecordRef> FollowUserAsync(string user, CancellationToken cancellationToken = default)
    {
        var session = await GetSessionAsync(cancellationToken);
        var did = await ResolveDid(user, cancellationToken);
        
        var followRecord = new FollowRecord
        {
            Subject = did,
            CreatedAt = DateTime.UtcNow
        };
        
        return await _repoService.CreateRecordAsync(
            session.Did,
            "app.bsky.graph.follow",
            followRecord,
            null,
            true,
            cancellationToken)
            .ConfigureAwait(false);
    }
    
    /// <summary>
    /// Unfollows a user.
    /// </summary>
    /// <param name="user">The handle or DID of the user to unfollow.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UnfollowUserAsync(string user, CancellationToken cancellationToken = default)
    {
        var session = await GetSessionAsync(cancellationToken);
        
        // Get the follow record URI
        var follows = await GetFollowsAsync(session.Did, 100, null, cancellationToken);
        var followRecord = follows.Actors.FirstOrDefault(a => 
            a.Did == user || 
            a.Handle.Equals(user, StringComparison.OrdinalIgnoreCase));
        
        if (followRecord == null)
        {
            throw new InvalidOperationException($"Not following user: {user}");
        }
        
        // Extract the record key from the URI
        var uri = followRecord.Viewer!.Following!;
        var parts = uri.Split('/');
        if (parts.Length < 4)
        {
            throw new InvalidOperationException($"Invalid follow record URI format: {uri}");
        }
        
        var rkey = parts[4];
        
        await _repoService.DeleteRecordAsync(
            session.Did,
            "app.bsky.graph.follow",
            rkey,
            cancellationToken)
            .ConfigureAwait(false);
    }
    
    private async Task<SessionInfo> GetSessionAsync(CancellationToken cancellationToken)
    {
        const string endpoint = "com.atproto.server.getSession";
        return await _client.GetAsync<SessionInfo>(endpoint, null, cancellationToken).ConfigureAwait(false);
    }
    
    private async Task<string> ResolveDid(string handleOrDid, CancellationToken cancellationToken)
    {
        if (handleOrDid.StartsWith("did:"))
        {
            return handleOrDid;
        }
        
        return await _identityService.ResolveHandleAsync(handleOrDid, cancellationToken).ConfigureAwait(false);
    }
}

/// <summary>
/// Represents a profile update request.
/// </summary>
public class ProfileUpdate
{
    /// <summary>
    /// The new display name for the profile.
    /// </summary>
    public string? DisplayName { get; set; }
    
    /// <summary>
    /// The new description for the profile.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// The new avatar image data.
    /// </summary>
    public byte[]? AvatarImage { get; set; }
    
    /// <summary>
    /// The content type of the avatar image.
    /// </summary>
    public string? AvatarImageContentType { get; set; }
    
    /// <summary>
    /// The new banner image data.
    /// </summary>
    public byte[]? BannerImage { get; set; }
    
    /// <summary>
    /// The content type of the banner image.
    /// </summary>
    public string? BannerImageContentType { get; set; }
}

/// <summary>
/// Response from actor list endpoints.
/// </summary>
public class ActorsResponse
{
    /// <summary>
    /// The list of actors.
    /// </summary>
    [JsonPropertyName("actors"), Required, JsonRequired]
    public List<Actor> Actors { get; set; }
    
    /// <summary>
    /// Pagination cursor for the next page of results.
    /// </summary>
    [JsonPropertyName("cursor")]
    public string? Cursor { get; set; }
}

/// <summary>
/// A record for following another user.
/// </summary>
public class FollowRecord
{
    /// <summary>
    /// The DID of the user being followed.
    /// </summary>
    [JsonPropertyName("subject"), Required, JsonRequired]
    public string Subject { get; set; }
    
    /// <summary>
    /// When the follow relationship was created.
    /// </summary>
    [JsonPropertyName("createdAt"), Required, JsonRequired]
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// A reference to a blob with metadata.
/// </summary>
public class BlobReference
{
    /// <summary>
    /// The Content Identifier (CID) of the blob.
    /// </summary>
    [JsonPropertyName("$link"), Required, JsonRequired]
    public string Cid { get; set; }
    
    /// <summary>
    /// The MIME type of the blob.
    /// </summary>
    [JsonPropertyName("mimeType"), Required, JsonRequired]
    public string MimeType { get; set; }
}

/// <summary>
/// A record containing profile information.
/// </summary>
public class ProfileRecord
{
    /// <summary>
    /// The display name for the profile.
    /// </summary>
    [JsonPropertyName("displayName")]
    public string? DisplayName { get; set; }
    
    /// <summary>
    /// The description for the profile.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    /// <summary>
    /// The avatar image reference.
    /// </summary>
    [JsonPropertyName("avatar")]
    public BlobReference? Avatar { get; set; }
    
    /// <summary>
    /// The banner image reference.
    /// </summary>
    [JsonPropertyName("banner")]
    public BlobReference? Banner { get; set; }
}
