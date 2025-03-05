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
using System;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    logger.Info("Application is starting...");

    var builder = WebApplication.CreateBuilder(args);
    var connectionString = builder.Configuration.GetConnectionString("GreetingConnection");

    builder.Services.AddDbContext<GreetingDbContext>(options => options.UseSqlServer(connectionString));

    // Add services to the container
    builder.Services.AddControllers();

    builder.Services.AddScoped<IGreetingBL, GreetingBL>();
    builder.Services.AddScoped<IGreetingRL, GreetingRL>();


    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Configure Logging
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }


    // Configure the HTTP request pipeline
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
    }

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