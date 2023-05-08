// using Catalog.Repositories;
// using Catalog.Settings;
// using Microsoft.OpenApi.Models;
// using MongoDB.Bson;
// using MongoDB.Bson.Serialization;
// using MongoDB.Bson.Serialization.Serializers;
// using MongoDB.Driver;
//
// // var builder = WebApplication.CreateBuilder(args);
// //
// // // Add services to the container.
// // builder.Services.AddControllers();
//
//
// public class Program3
// {
//     public static void Main(string[] args)
//     {
//         CreateHostBuilder(args).Build().Run();
//     }
//
//     public static IHostBuilder CreateHostBuilder(string[] args) =>
//         Host.CreateDefaultBuilder(args)
//             .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
// }
//
//
// public class Startup
// {
//     public Startup(IConfiguration configuration)
//     {
//         Configuration = configuration;
//     }
//
//     public IConfiguration Configuration { get; }
//
//     // This method gets called by the runtime. Use this method to add services to the container.
//     public void ConfigureServices(IServiceCollection services)
//     {
//         BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
//         BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
//         var mongoDbSettings = Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
//         var mongoDbSettings2 = services.Configure<MongoDbSettings>(Configuration.GetSection("ConnectionString"));
//
//         services.Configure<MongoDbSettings>(Configuration.GetSection("ConnectionString:MongoDbSettings"));
//         services.Configure<MongoDbSettings>(Configuration.GetSection("ConnectionString"));
//         services.Configure<MongoDbSettings>(Configuration.GetSection("ConnectionString"));
//         //var mongoDbSettings2 = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
//         
//         services.AddSingleton<IMongoClient>(serviceProvider =>
//         {
//             return new MongoClient();
//         });
//
//
//         // services.AddSingleton<IMongoClient>(serviceProvider =>
//         // {
//         //     //return new MongoClient("mongodb://localhost:27017");
//         //     return new MongoClient(mongoDbSettings?.ConnectionString);
//         // });
//
//         services.AddSingleton<IItemsRepository, MongoDbItemsRepository>();
//
//         services.AddControllers(options => { options.SuppressAsyncSuffixInActionNames = false; });
//         services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Catalog", Version = "v1"}); });
//     }
//
//     // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//     public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//     {
//         if (env.IsDevelopment())
//         {
//             app.UseDeveloperExceptionPage();
//             app.UseSwagger();
//             app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog v1"));
//         }
//
//         if (env.IsDevelopment())
//         {
//             app.UseHttpsRedirection();
//         }
//
//         app.UseRouting();
//
//         app.UseAuthorization();
//
//         app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
//     }
// }
//
// /*
//  * THIS SECTION ISNT GETTING THE CONNECTION STRING
//  */
// // This is the line that adds the mongo client to the container
// // builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
// // {
// //     // Get the settings from the appsettings.json file
// //     var settings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
// //     // Return a new instance of the mongo client instance
// //     return new MongoClient(settings?.ConnectionString);
// // });
// //
// // // This is the line that adds the repository to the container
// // builder.Services.AddSingleton<IItemsRepository, MongoDbItemsRepository>();
// //
// // // This is the line that adds the repository to the container
// // builder.Services.AddSingleton<IItemsRepository, InMemItemsRepository>();
// //
// // // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// // builder.Services.AddEndpointsApiExplorer();
// //
// // // Versioning API with Swagger 
// // builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Catalog", Version = "v1"}); });
// //
// //
// // // CORS - Cross Origin Resource Sharing - allows you to make requests from one domain to another
// // builder.Services.AddCors(options =>
// // {
// //     options.AddPolicy(name: "Local",
// //         corsPolicyBuilder =>
// //         {
// //             corsPolicyBuilder
// //                 .AllowAnyMethod()
// //                 .AllowAnyHeader()
// //                 .WithOrigins("https://localhost")
// //                 .AllowCredentials();
// //         });
// // });
// //
// // var app = builder.Build();
// //
// // // Configure the HTTP request pipeline.
// // if (app.Environment.IsDevelopment())
// // {
// //     app.UseSwagger();
// //     app.UseSwaggerUI();
// // }
// //
// // app.UseHttpsRedirection();
// //
// // app.UseAuthorization();
// //
// // app.MapControllers();
// //
// // app.Run();