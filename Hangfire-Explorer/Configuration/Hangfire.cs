namespace Hangfire_Explorer.Configuration
{
    using global::Hangfire;
    using global::Hangfire.Mongo;
    using global::Hangfire.Mongo.Migration.Strategies;
    using global::Hangfire.Mongo.Migration.Strategies.Backup;
    using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Driver;

    public static class Hangfire
    {
        public static IServiceCollection ConfigureHangfire(this IServiceCollection services)
        {
            services.AddHangfire((serviceProvider, hangfireConfigurations) =>
            {
                var mongoDatabase = serviceProvider.GetRequiredService<IMongoClient>().GetDatabase("HANGFIRE_EXPLORER");

                hangfireConfigurations
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseMongoStorage(
                        (MongoClient)mongoDatabase.Client,
                        mongoDatabase.DatabaseNamespace.DatabaseName,
                        new MongoStorageOptions
                        {
                            MigrationOptions = new MongoMigrationOptions
                            {
                                MigrationStrategy = new MigrateMongoMigrationStrategy(),
                                BackupStrategy = new CollectionMongoBackupStrategy()
                            },
                            Prefix = "hangfire.mongo",
                            CheckConnection = true
                        });
            });

            services.AddHangfireServer(serverOptions =>
            {
                serverOptions.WorkerCount = 10;
            });

            return services;
        }
    }
}
