using Microsoft.OpenApi;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using Rpg.Common.MongoDb;
using Rpg.Inventory.Service.Clients;
using Rpg.Inventory.Service.Models;





var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddMongo()
    .AddMongoRepository<InventoryItem>("inventoryItems");
builder.Services.AddHttpClient<CatalogClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5177");
})
    .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.Or<TimeoutRejectedException>()
    .WaitAndRetryAsync(
        5,
        retryCount => TimeSpan.FromSeconds(Math.Pow(2,retryCount)),
        onRetry : (outcome,timespan, retryCount) =>
        {
            var serviceProvider = builder?.Services?.BuildServiceProvider();
            serviceProvider?.GetService<ILogger<CatalogClient>>()?
            .LogWarning($"Delaying for {timespan} seconds, then retry attempt {retryCount}");
        }
        )
    )
    .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.Or<TimeoutRejectedException>()
        .CircuitBreakerAsync(
            3,
            TimeSpan.FromSeconds(15),
            onBreak : (outcome, timespan) =>
            {
                var serviceProvider = builder?.Services?.BuildServiceProvider();
                serviceProvider?.GetService<ILogger<CatalogClient>>()?
                .LogWarning($"Circuit opening for {timespan} seconds");
            },
            onReset : () =>
            {
                var serviceProvider = builder?.Services?.BuildServiceProvider();
                serviceProvider?.GetService<ILogger<CatalogClient>>()?
                .LogWarning($"Circuit closing");
            }
        )
    )
    .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

BsonSerializer.RegisterSerializer(
    new GuidSerializer(MongoDB.Bson.BsonType.String)
);

var app = builder.Build();


app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;
});


app.UseHttpsRedirection();


app.MapControllers();


app.Run();

