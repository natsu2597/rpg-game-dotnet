using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Rpg.Common.MassTransit;
using Rpg.Common.MongoDb;
using Rpg.Identity.Service.Jwt;
using Rpg.Identity.Service.Models;
using Rpg.Identity.Service.Settings;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddMongo()
    .AddMongoRepository<User>("users")
    .AddMassTransitWithRabbitMq();

builder.Services.AddControllers();

builder.Services.Configure<JwtSettings>(
        builder.Configuration.GetSection(nameof(JwtSettings))
    );

builder.Services.AddSingleton<JwtService>();

var app = builder.Build();





BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();




app.Run();

