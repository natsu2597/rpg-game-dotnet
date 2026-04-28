using MongoDB.Driver;
using Rpg.Catalog.Service.Models;
using Rpg.Catalog.Service.Settings;

namespace Rpg.Catalog.Service.Repositories
{
    public static class Extension
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            
            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var serviceSettings = configuration!
                                    .GetSection(nameof(ServiceSettings))
                                    .Get<ServiceSettings>();
                var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
                return mongoClient.GetDatabase(serviceSettings.ServiceName);
            });

            return services;

        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : IModel
        {
            services.AddSingleton<IRepository<T>>(serviceProvider =>
            {
                var database = serviceProvider.GetService<IMongoDatabase>();
                return new MongoRepository<T>(database!, collectionName);
            });

            return services;
        }
    }
}
