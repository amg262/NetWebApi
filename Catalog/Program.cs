using System.Net.Mime;
using System.Text.Json;
using Catalog.Repositories;
using Catalog.Settings;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// This is to make sure that the controller methods end with Async - otherwise it'll strip them out
builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);


// Might end up with not good representation of data in db - so we need to tell mongo how to serialize the Guid data (string)
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));


var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
// This is the line that adds the mongo client to the container |   OLD WAY 
// builder.Services.AddSingleton<IMongoClient>(new MongoClient(builder.Configuration.GetSection(nameof(MongoDbSettings))
//     .Get<MongoDbSettings>()?.ConnectionString));

builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    return new MongoClient(mongoDbSettings?.ConnectionString);
});

// This is the line that adds the repository to the container
builder.Services.AddSingleton<IItemsRepository, MongoDbItemsRepository>();

// This is the line that adds in memory repository to the container - this is for testing but MongoDbItemsRepository is for production
// builder.Services.AddSingleton<IItemsRepository, InMemItemsRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Versioning API with Swagger 
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Catalog", Version = "v1"}); });


// builder.Services.AddHealthChecksUI();

// CORS - Cross Origin Resource Sharing - allows you to make requests from one domain to another
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Local",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithOrigins("https://localhost")
                .AllowCredentials();
        });
});

// Health checks
builder.Services.AddHealthChecks()
    .AddMongoDb(mongoDbSettings?.ConnectionString,
        name: "mongodb",
        timeout: TimeSpan.FromSeconds(3),
        tags: new[] {"ready"}); // This is to check if the db is ready to accept requests

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();


//app.MapDefaultControllerRoute();
app.UseCors("Local");


// This checks if the db is ready to accept requests and gives a response
app.MapHealthChecks("/api/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = async (context, report) =>
    {
        var result = JsonSerializer.Serialize(
            new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(entry => new
                {
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                    duration = entry.Value.Duration.ToString()
                })
            }
        );

        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result);
    }
});

// This checks if API is live
app.MapHealthChecks("/api/health/live", new HealthCheckOptions
{
    Predicate = _ => false,
});



app.MapControllers();

app.Run();