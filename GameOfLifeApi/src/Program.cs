using GameOfLife.Data;
using GameOfLife.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var redisConnectionString = builder.Configuration.GetSection("Redis:ConnectionString").Value;

if (string.IsNullOrEmpty(redisConnectionString))
{
    throw new InvalidOperationException("Redis connection string is not configured.");
}

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));

builder.Services.AddScoped<GameOfLifeRepository>();

builder.Services.AddScoped<GameOfLifeService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
