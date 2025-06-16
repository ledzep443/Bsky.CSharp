using Bsky.AspNetCore.DependencyInjection.Configuration;
using Bsky.CSharp.AtProto.Services;
using Bsky.CSharp.Bluesky.Services;
using Bsky.CSharp.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Bsky.AspNetCore.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddBluesky(this IServiceCollection services, Func<IServiceProvider, BskySettings> settingsFactory)
    {
        services.AddSingleton(settingsFactory);

        services.AddHttpClient<IXrpcClient, XrpcClient>(BlueskyConstants.BlueskyClientName)
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                BskySettings settings = settingsFactory(serviceProvider);
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