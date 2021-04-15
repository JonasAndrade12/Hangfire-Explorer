namespace Hangfire_Explorer.Configuration
{
    using Hangfire_Explorer.Models;
    using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Driver;

    public static class Services
    {
        public static IServiceCollection Configuration(this IServiceCollection services, IApplicationSettings settings)
        {
            services.AddSingleton<IMongoClient>(_ => new MongoClient(settings.ConnectionString));

            return services;
        }
    }
}
