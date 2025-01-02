using HashProcessorApp.Data;
using HashProcessorApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Check if the RabbitMQ connection string is present
var rabbitMqConnectionString = builder.Configuration.GetConnectionString("RabbitMQConnectionString");
if (string.IsNullOrEmpty(rabbitMqConnectionString))
{
    throw new InvalidOperationException("RabbitMQ connection string is missing or empty in appsettings.json.");
}

// Register RabbitMQService as Singleton
builder.Services.AddSingleton<RabbitMQService>(serviceProvider =>
{
    return new RabbitMQService(rabbitMqConnectionString); // Pass the connection string to initialize RabbitMQService
});

// Register other services like HashProcessorService
builder.Services.AddScoped<HashProcessorService>();

// Register DbContext for MariaDb
/*builder.Services.AddDbContext<HashesDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        new MySqlServerVersion(new Version(11, 6, 2))) // specify the correct MariaDB version
);*/

builder.Services.AddDbContext<HashesDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// Register Controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use Swagger for API documentation
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
