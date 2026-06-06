using Rpg.Common.MassTransit;
using Rpg.Common.MongoDb;
using Rpg.Player.Service.Models;
using Rpg.Player.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMongo()
    .AddMongoRepository<Ryu>("players").
    AddMongoRepository<Master>("masters")
    .AddMassTransitWithRabbitMq();

builder.Services.AddControllers();

builder.Services.AddSingleton<GrowthTableService>();
builder.Services.AddSingleton<LevellingService>();

var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



app.UseHttpsRedirection();


app.Run();


