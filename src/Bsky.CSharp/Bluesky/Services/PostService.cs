using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Bsky.CSharp.AtProto.Models;
using Bsky.CSharp.AtProto.Services;
using Bsky.CSharp.Bluesky.Models;
using Bsky.CSharp.Http;

namespace Bsky.CSharp.Bluesky.Services;




public class PostService : IPostService
{
    private readonly XrpcClient _client;
    private readonly BlobService _blobService;
    private readonly RepositoryService _repoService;
    private const string PostCollectionId = "app.bsky.feed.post";
    
    
    public PostService(XrpcClient client, BlobService blobService, RepositoryService repoService)
    {
        _client = client;
        _blobService = blobService;
        _repoService = repoService;
    }
    
    /// <inheritdoc />
    public async Task<RecordRef> CreateTextPostAsync(
        string text,
        ReplyRef? replyTo = null,
        IEnumerable<string>? languageTags = null,
        CancellationToken cancellationToken = default)
    {
        var record = CreatePostRecord(text, null, replyTo, languageTags);
        var did = await GetUserDidAsync(cancellationToken);
        
        return await _repoService.CreateRecordAsync(
            did,
            PostCollectionId,
            record,
            null,
            true,
            cancellationToken)
            .ConfigureAwait(false);
    }
    
    /// <inheritdoc />
    public async Task<RecordRef> CreateImagePostAsync(
        string text,
        IEnumerable<ImageUpload> images,
        ReplyRef? replyTo = null,
        IEnumerable<string>? languageTags = null,
        CancellationToken cancellationToken = default)
    {
        var imagesList = images.ToList();
        if (!imagesList.Any())
        {
            throw new ArgumentException("At least one image must be provided", nameof(images));
        }
        
        // Upload each image
        var uploadedImages = new List<Image>();
        foreach (var image in imagesList)
        {
            var blob = await UploadImageAsync(image.Data, image.ContentType, cancellationToken);
            uploadedImages.Add(new Image
            {
                ImageUrl = blob.Blob.Ref!.Link,
                Alt = image.AltText ?? string.Empty,
                AspectRatio = image.AspectRatio
            });
        }
        
        // Create embed for images
        var embed = new EmbedImages
        {
            Type = "app.bsky.embed.images",
            Images = uploadedImages
        };
        
        var record = CreatePostRecord(text, embed, replyTo, languageTags);
        var did = await GetUserDidAsync(cancellationToken);
        
        return await _repoService.CreateRecordAsync(
            did,
            PostCollectionId,
            record,
            null,
            true,
            cancellationToken)
            .ConfigureAwait(false);
    }
    
    /// <inheritdoc />
    public async Task<RecordRef> CreateLinkPostAsync(
        string text,
        string url,
        string? title = null,
        string? description = null,
        byte[]? thumbnailData = null,
        string? thumbnailContentType = null,
        ReplyRef? replyTo = null,
        IEnumerable<string>? languageTags = null,
        CancellationToken cancellationToken = default)
    {
        // Create external embed info
        var externalInfo = new EmbedExternalInfo
        {
            Uri = url,
            Title = title,
            Description = description,
            Thumb = await CreateThumbnailEmbedAsync(thumbnailData, thumbnailContentType, cancellationToken)
        };
        
        
        // Create embed for external link
        var embed = new EmbedExternal
        {
            Type = "app.bsky.embed.external",
            External = externalInfo
        };
        
        var record = CreatePostRecord(text, embed, replyTo, languageTags);
        var did = await GetUserDidAsync(cancellationToken);
        
        return await _repoService.CreateRecordAsync(
            did,
            PostCollectionId,
            record,
            null,
            true,
            cancellationToken)
            .ConfigureAwait(false);
    }
    
    private async Task<EmbedExternalThumb?> CreateThumbnailEmbedAsync(
        byte[]? thumbnailData,
        string? contentType,
        CancellationToken cancellationToken)
    {
        if (thumbnailData == null || string.IsNullOrEmpty(contentType)) return null;
        BlobRef blob = await UploadImageAsync(thumbnailData, contentType, cancellationToken);
        return new EmbedExternalThumb
        {
            Uri = blob.Blob.Ref!.Link,
            MimeType = contentType
        };
    }
    
    /// <inheritdoc />
    public async Task<Post> GetPostAsync(string uri, CancellationToken cancellationToken = default)
    {
        const string endpoint = "app.bsky.feed.getPostThread";
        var parameters = new Dictionary<string, string>
        {
            ["uri"] = uri
        };
        
        var response = await _client.GetAsync<ThreadResponse>(endpoint, parameters, cancellationToken).ConfigureAwait(false);
        if (response.Thread is PostThreadView postThread)
        {
            return postThread.Post;
        }
        
        throw new InvalidOperationException($"Post not found or inaccessible: {uri}");
    }
    
    /// <inheritdoc />
    public async Task DeletePostAsync(string uri, CancellationToken cancellationToken = default)
    {
        var parts = uri.Split('/');
        if (parts.Length < 4)
        {
            throw new ArgumentException("Invalid post URI format", nameof(uri));
        }
        
        var repo = parts[2];
        var rkey = parts[4];
        
        await _repoService.DeleteRecordAsync(
            repo,
            PostCollectionId,
            rkey,
            cancellationToken)
            .ConfigureAwait(false);
    }
    
    private PostRecord CreatePostRecord(
        string text,
        Embed? embed = null,
        ReplyRef? replyTo = null,
        IEnumerable<string>? languageTags = null)
    {
        Reply? reply = null;
        if (replyTo != null)
        {
            reply = new Reply
            {
                Root = replyTo,
                Parent = replyTo
            };
        }
        
        return new PostRecord
        {
            Text = text,
            CreatedAt = DateTime.UtcNow,
            Embed = embed,
            Reply = reply,
            Langs = languageTags?.ToList()
        };
    }
    
    private async Task<BlobRef> UploadImageAsync(byte[] imageData, string contentType, CancellationToken cancellationToken)
    {
        return await _blobService.UploadBlobAsync(imageData, contentType, cancellationToken).ConfigureAwait(false);
    }
    
    private async Task<string> GetUserDidAsync(CancellationToken cancellationToken)
    {
        const string endpoint = "com.atproto.server.getSession";
        var session = await _client.GetAsync<SessionInfo>(endpoint, null, cancellationToken).ConfigureAwait(false);
        return session.Did;
    }
}
