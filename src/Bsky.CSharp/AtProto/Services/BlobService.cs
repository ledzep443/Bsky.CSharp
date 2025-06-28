using Bsky.CSharp.AtProto.Models;
using Bsky.CSharp.Http;

namespace Bsky.CSharp.AtProto.Services;

/// <summary>
/// Service for working with blobs in the AT Protocol.
/// </summary>
public class BlobService : IBlobService
{
    private readonly XrpcClient _client;
    private const string UploadBlobEndpoint = "com.atproto.blob.upload";
    
    /// <summary>
    /// Creates a new blob service.
    /// </summary>
    /// <param name="client">The XRPC client to use for API requests.</param>
    public BlobService(XrpcClient client)
    {
        _client = client;
    }
    
    /// <summary>
    /// Uploads a blob to the server.
    /// </summary>
    /// <param name="data">The binary data to upload.</param>
    /// <param name="contentType">The MIME type of the data.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>Information about the uploaded blob.</returns>
    public async Task<BlobRef> UploadBlobAsync(
        byte[] data,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        var content = new ByteArrayContent(data);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
        
        var request = new HttpRequestMessage(HttpMethod.Post, UploadBlobEndpoint)
        {
            Content = content
        };
        
        // Add authentication if available
        if (_client.GetAccessToken() != null)
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _client.GetAccessToken());
        }
        
        var response = await _client.SendRawRequestAsync(request, cancellationToken).ConfigureAwait(false);
        var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        
        var options = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
        
        var result = await System.Text.Json.JsonSerializer.DeserializeAsync<BlobRef>(responseStream, options, cancellationToken).ConfigureAwait(false);
        
        if (result == null)
        {
            throw new InvalidOperationException("Failed to deserialize the response");
        }
        
        return result;
    }

    /// <summary>
    /// Retrieves a blob from the server.
    /// </summary>
    /// <param name="did">The DID of the blob owner.</param>
    /// <param name="cid">The CID of the blob.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The binary data and content type of the blob.</returns>
    public async Task<(byte[] Data, string ContentType)> GetBlobAsync(string did, string cid, CancellationToken cancellationToken = default)
    {
        const string endpoint = "com.atproto.sync.getBlob";
        var parameters = new Dictionary<string, string>
        {
            ["did"] = did,
            ["cid"] = cid
        };
        
        var request = new HttpRequestMessage(HttpMethod.Get, 
            $"{endpoint}?{string.Join("&", parameters.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"))}");
        
        // Add authentication if available
        if (_client.GetAccessToken() != null)
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _client.GetAccessToken());
        }
        
        var response = await _client.SendRawRequestAsync(request, cancellationToken).ConfigureAwait(false);
        
        // Get the content type from the response headers
        string contentType = response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";
        
        // Read the binary data
        var data = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        
        return (data, contentType);
    }
}
