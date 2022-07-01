using Microsoft.EntityFrameworkCore;
using ASP.NET6WithRedisCache.Interface;
using ASP.NET6WithRedisCache.Models;
using ASP.NET6WithRedisCache.Utilities;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CoreDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnectionString"), options => options.EnableRetryOnFailure()));
builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = configuration["RedisConfiguration"]; });

builder.Services.Configure<RepositoryOptions>(options =>
{
    //  options.ConnectionString = configuration.GetConnectionString("ConnectionString");
    options.RedisConfiguration = configuration["RedisConfiguration"];

});

builder.Services.AddSingleton<RedisCache>();
builder.Services.AddScoped<IRepositoryHelper, RepositoryHelper>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
