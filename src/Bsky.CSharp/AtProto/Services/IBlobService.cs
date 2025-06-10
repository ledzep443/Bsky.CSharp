using Bsky.CSharp.AtProto.Models;

namespace Bsky.CSharp.AtProto.Services;

/// <summary>
/// Interface for handling blob operations in the AT Protocol.
/// </summary>
public interface IBlobService
{
    /// <summary>
    /// Uploads a blob to the server.
    /// </summary>
    /// <param name="data">The binary data to upload.</param>
    /// <param name="contentType">The MIME type of the data.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A reference to the uploaded blob.</returns>
    Task<BlobRef> UploadBlobAsync(byte[] data, string contentType, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a blob from the server.
    /// </summary>
    /// <param name="did">The DID of the repository.</param>
    /// <param name="cid">The CID of the blob.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The blob data and content type.</returns>
    Task<(byte[] Data, string ContentType)> GetBlobAsync(string did, string cid, CancellationToken cancellationToken = default);
}
