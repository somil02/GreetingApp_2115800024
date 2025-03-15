using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Web;
using BusinessLayer.Interface;
using BusinessLayer.Service;
using RepositoryLayer.Interface; 
using RepositoryLayer.Service;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using Middleware.GlobalExceptionFilter;
using RepositoryLayer.Hashing;
using Middleware.ExceptionHandler;
using RepositoryLayer.Token;


var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    logger.Info("Application is starting...");

    var builder = WebApplication.CreateBuilder(args);
    var connectionString = builder.Configuration.GetConnectionString("GreetingConnection");

    // Configure Entity Framework and Database Context
    builder.Services.AddDbContext<GreetingDbContext>(options => options.UseSqlServer(connectionString));

    // Register Business and Repository Layer services
    builder.Services.AddScoped<IGreetingBL, GreetingBL>();
    builder.Services.AddScoped<IGreetingRL, GreetingRL>();

    builder.Services.AddScoped<IUserBL, UserBL>();
    builder.Services.AddScoped<IUserRL, UserRL>();
    builder.Services.AddScoped<Password_Hash>();
    builder.Services.AddScoped<JwtToken>();
    builder.Services.AddScoped<EmailService>();



    // Add Controllers and Global Exception Filter
    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<GlobalExceptionFilter>(); // Register GlobalExceptionFilter
    });


    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Configure Logging with NLog
    builder.Logging.ClearProviders( );
    builder.Host.UseNLog();

    var app = builder.Build();
   

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseMiddleware<ExceptionHandler>();


    app.UseRouting();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Application startup failed.");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}

//uii forget password ka mail aara h ab reset wala dekhlo ki jo aara h otken usee kse krnege'
//oki??
//srf tab tab nhi kr dena dekhna kse kiya h kya hoga oki

//hellooo???