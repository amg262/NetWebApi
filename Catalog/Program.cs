using Catalog.Repositories;
using Catalog.Settings;
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

// This is the line that adds the mongo client to the container
builder.Services.AddSingleton<IMongoClient>(new MongoClient(builder.Configuration.GetSection(nameof(MongoDbSettings))
    .Get<MongoDbSettings>()
    ?.ConnectionString));


// This is the line that adds the repository to the container
builder.Services.AddSingleton<IItemsRepository, MongoDbItemsRepository>();

// This is the line that adds in memory repository to the container - this is for testing but MongoDbItemsRepository is for production
// builder.Services.AddSingleton<IItemsRepository, InMemItemsRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Versioning API with Swagger 
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Catalog", Version = "v1"}); });


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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//app.MapDefaultControllerRoute();
app.UseCors("Local");
app.MapControllers();

app.Run();