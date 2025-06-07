using Bsky.CSharp.AtProto.Models;
using Bsky.CSharp.Http;

namespace Bsky.CSharp.AtProto.Services;

/// <summary>
/// Service for working with blobs in the AT Protocol.
/// </summary>
public class BlobService
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
        
        var response = await _client.SendRawRequestAsync(request, cancellationToken);
        var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        
        var options = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
        
        var result = await System.Text.Json.JsonSerializer.DeserializeAsync<BlobRef>(responseStream, options, cancellationToken);
        
        if (result == null)
        {
            throw new InvalidOperationException("Failed to deserialize the response");
        }
        
        return result;
    }
}
