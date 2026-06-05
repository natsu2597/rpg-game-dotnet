using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Rpg.Catalog.Service.Models;
using Rpg.Common.Settings;
using Rpg.Common.MongoDb;
using Rpg.Common.MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var serviceSettings = builder.Configuration
    .GetSection(nameof(ServiceSettings))
    .Get<ServiceSettings>();

builder.Services.AddMongo()
    .AddMongoRepository<Item>("items")
    .AddMassTransitWithRabbitMq();






builder.Services.AddControllers();

BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;
});

app.UseCors("AllowAll");

app.MapControllers();

app.UseHttpsRedirection();

app.Run();
