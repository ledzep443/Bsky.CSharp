using Bsky.AspNetCore.DependencyInjection.Configuration;
using Bsky.CSharp.AtProto.Services;
using Bsky.CSharp.Bluesky.Services;
using Bsky.CSharp.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bsky.AspNetCore.DependencyInjection;

public static class DependencyInjection
{
    /// <summary>
    /// Registers Bluesky-related services and configuration for dependency injection.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="configureOptions">An optional configuration action to customize BlueskyOptions.</param>
    /// <returns>The updated service collection with the Bluesky services added.</returns>
    public static IServiceCollection AddBluesky(this IServiceCollection services,
        Action<BlueskyOptions>? configureOptions = null)
    {
        services.AddOptions<BlueskyOptions>()
            .BindConfiguration(BlueskyConstants.BlueskySection)
            .Configure(options => configureOptions?.Invoke(options));

        services.AddHttpClient<IXrpcClient, XrpcClient>(BlueskyConstants.BlueskyClientName)
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                BlueskyOptions settings = serviceProvider.GetRequiredService<IOptions<BlueskyOptions>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
                client.Timeout = TimeSpan.FromMilliseconds(settings.Timeout);
            });
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IBlobService, BlobService>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<IRepositoryService, RepositoryService>();
        services.AddTransient<IServerService, ServerService>();
        services.AddTransient<ISyncService, SyncService>();
        services.AddTransient<IFeedService, FeedService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IPostService, PostService>();
        return services;
    }
}