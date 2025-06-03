using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Common.Infrastructure.ServiceExtensions;

public static class ServiceExtensions 
{
    public static void AddMongoService<TContext>(this IServiceCollection services, string databaseName) where TContext : DbContext
    {
        services.AddDbContextPool<TContext>((provider ,options) =>
        {
            var client = provider.GetService<MongoClient>();
            options.UseMongoDB(client ?? throw new InvalidOperationException(), databaseName);
        });
    }
}