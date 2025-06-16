using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Bsky.CSharp.Bluesky.Models;
using Bsky.CSharp.Http;
using Bsky.CSharp.AtProto.Services;
using Bsky.CSharp.AtProto.Models;

namespace Bsky.CSharp.Bluesky.Services;

/// <inheritdoc />
public class UserService : IUserService
{
    private readonly XrpcClient _client;
    private readonly RepositoryService _repoService;
    private readonly BlobService _blobService;
    private readonly IdentityService _identityService;
    private const string ProfileCollectionId = "app.bsky.actor.profile";
    
    
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
    
    /// <inheritdoc />
    public async Task<Actor> GetMyProfileAsync(CancellationToken cancellationToken = default)
    {
        SessionInfo session = await GetSessionAsync(cancellationToken);
        return await GetProfileAsync(session.Did, cancellationToken);
    }
    
    /// <inheritdoc />
    public async Task<Actor> GetProfileAsync(string handleOrDid, CancellationToken cancellationToken = default)
    {
        const string endpoint = "app.bsky.actor.getProfile";
        var parameters = new Dictionary<string, string>
        {
            ["actor"] = handleOrDid
        };
        
        return await _client.GetAsync<Actor>(endpoint, parameters, cancellationToken).ConfigureAwait(false);
    }
    
    /// <inheritdoc />
    public async Task<ProfileRecord> UpdateProfileAsync(ProfileUpdate update, CancellationToken cancellationToken = default)
    {
        SessionInfo session = await GetSessionAsync(cancellationToken);
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
            BlobRef bannerBlob = await _blobService.UploadBlobAsync(
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
    
    /// <inheritdoc />
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
    
    /// <inheritdoc />
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
    
    /// <inheritdoc />
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
    
    /// <inheritdoc />
    public async Task<RecordRef> FollowUserAsync(string user, CancellationToken cancellationToken = default)
    {
        SessionInfo session = await GetSessionAsync(cancellationToken);
        string did = await ResolveDid(user, cancellationToken);
        
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
    
    /// <inheritdoc />
    public async Task UnfollowUserAsync(string user, CancellationToken cancellationToken = default)
    {
        SessionInfo session = await GetSessionAsync(cancellationToken);
        
        // Get the follow record URI
        ActorsResponse follows = await GetFollowsAsync(session.Did, 100, null, cancellationToken);
        Actor? followRecord = follows.Actors.FirstOrDefault(a => 
            a.Did == user || 
            a.Handle.Equals(user, StringComparison.OrdinalIgnoreCase));
        
        if (followRecord == null)
        {
            throw new InvalidOperationException($"Not following user: {user}");
        }
        
        // Extract the record key from the URI
        string uri = followRecord.Viewer!.Following!;
        string[]? parts = uri.Split('/');
        if (parts.Length < 4)
        {
            throw new InvalidOperationException($"Invalid follow record URI format: {uri}");
        }
        
        string rkey = parts[4];
        
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
